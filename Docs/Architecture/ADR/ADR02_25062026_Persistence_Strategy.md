# ADR-002: Persistence Strategy

## 1. Status

**Accepted**

---

## 2. Date

2026-06-25

---

## 3. Context

Forge requires a persistence layer that is simple, maintainable, scalable, and suitable for a long-lived metadata-driven enterprise platform.

The persistence layer must support:

* Enterprise application development
* Metadata-driven architecture
* Automatic auditing
* Future authentication and authorization
* Dynamic forms and runtime data
* SQL Server JSON capabilities
* Future features such as soft delete, caching, background jobs, and integrations

The architecture should minimize duplication while keeping responsibilities clearly separated.

---

## 4. Decision

Forge adopts the following persistence strategy.

### Database

* SQL Server 2025
* Single relational database
* SQL Server JSON data type will be used where appropriate for dynamic runtime data

---

### ORM

Entity Framework Core is the standard ORM.

---

### DbContext

A single `AppDbContext` will be used for the entire platform.

The DbContext is responsible for:

* Database connection
* Entity tracking
* Entity configurations
* Automatic audit handling
* Database transactions

Business logic must never be implemented inside the DbContext.

---

### Entity Configuration

Entity configuration is separated from entity classes.

Each entity has its own configuration class implementing:

```text
IEntityTypeConfiguration<TEntity>
```

All configurations are automatically discovered using:

```csharp
modelBuilder.ApplyConfigurationsFromAssembly(
    typeof(AppDbContext).Assembly);
```

---

### Entity Hierarchy

Forge uses inheritance to eliminate duplication.

```
BaseEntity
    │
    ▼
AuditableEntity
    │
    ├── User
    │
    ▼
NamedEntity
```

#### BaseEntity

Contains only:

* Id

#### AuditableEntity

Contains:

* CreatedAt
* CreatedBy
* UpdatedAt
* UpdatedBy

#### NamedEntity

Contains:

* Name
* Description

---

### Audit Strategy

Audit timestamps are automatically populated inside `AppDbContext`.

Application services and controllers must never manually populate audit fields.

Initially:

* CreatedBy
* UpdatedBy

remain null until authentication is implemented.

Future versions will populate these fields using the authenticated user.

---

### Date and Time

All timestamps use:

```
DateTimeOffset
```

UTC is stored in the database.

Client applications are responsible for converting timestamps to the user's local timezone.

---

### Database Mapping

Database mapping is performed exclusively using Entity Framework Core configuration classes.

Data Annotations are not used for persistence configuration.

---

### Validation

Validation is handled exclusively by FluentValidation.

Validation rules are intentionally separated from persistence rules.

---

### Naming Conventions

Entity names are singular.

Database table names are singular.

Examples:

* User
* Application
* Form
* Workflow

---

### Database Migrations

Database migrations are stored under:

```
Infrastructure/
└── Persistence/
    └── Migrations
```

---

## 5. Rationale

This strategy provides:

* Clear separation of concerns
* Reduced duplication
* Centralized persistence logic
* Automatic auditing
* Better maintainability
* Easier long-term evolution
* Consistent architecture across all platform modules

Separating entities, validation, and persistence configuration allows each concern to evolve independently.

---

## 6. Alternatives Considered

### Multiple DbContexts

**Rejected**

Reason:

A metadata-driven platform benefits from a single transactional boundary and simplified relationships.

---

### Data Annotations

**Rejected**

Reason:

Persistence configuration should remain separate from domain entities.

---

### Auditing via Interface (IAuditable)

**Rejected**

Reason:

Every auditable entity would duplicate audit properties.

Replacing the interface with `AuditableEntity` removes duplication while maintaining a clean inheritance hierarchy.

---

### Entity Configuration inside AppDbContext

**Rejected**

Reason:

Keeping configurations inside `OnModelCreating()` does not scale well as the number of entities increases.

---

### DateTime

**Rejected**

Reason:

`DateTimeOffset` provides a better representation of timestamps in distributed and multi-timezone environments.

---

## 7. Consequences

### Positive

* Clean persistence architecture
* Centralized audit implementation
* Minimal duplication
* Consistent entity design
* Easier maintenance
* Scalable for future platform modules

### Trade-offs

* Slightly more initial setup
* More files compared to small CRUD applications
* Requires discipline to maintain separation of concerns

---

## 8. Implementation Notes

Current implementation includes:

* SQL Server 2025
* Entity Framework Core
* AppDbContext
* Automatic audit handling
* BaseEntity
* AuditableEntity
* NamedEntity
* User entity
* User configuration
* Initial database migration

The persistence foundation is considered complete.

---

## 9. Future Considerations

The persistence layer is designed to support future enhancements without major architectural changes.

Planned capabilities include:

* CurrentUserService integration for CreatedBy and UpdatedBy
* Soft delete support
* Entity activation
* Global query filters
* EF Core interceptors
* Database seeding
* Optimistic concurrency (if required)
* Multi-tenancy
* SQL Server JSON columns for runtime data
* Audit history
* Performance optimizations
* Read/write separation if required

Future enhancements should extend the existing persistence architecture rather than replace it.
