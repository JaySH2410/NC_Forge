# ADR-001: Platform Architecture

## Status

Accepted

---

## Date

2026-06-25

---

## Context

Forge is being designed as a long-term metadata-driven application platform rather than a traditional CRUD application.

The platform should enable users to build enterprise applications through configuration instead of writing backend code.

Typical applications that Forge should be capable of supporting include:

* Manufacturing Execution Systems (MES)
* Asset Management
* Quality Management
* Inventory Management
* CRM
* ERP Extensions
* Internal Business Applications

The platform should remain maintainable as it grows to hundreds of entities, thousands of forms, millions of records, and multiple runtime engines.

---

## Decision

Forge will be designed as a **metadata-driven application platform**.

The backend architecture will prioritize platform capabilities over individual business modules.

Business features (such as Hobby, Equipment, Employee, etc.) will be implemented on top of the platform instead of driving the architecture.

---

## Guiding Principles

### Platform First

Infrastructure and platform capabilities are implemented before business features.

Priority order:

1. Infrastructure
2. Persistence
3. Authentication & Authorization
4. Metadata Engine
5. Runtime Engine
6. Business Modules

---

### Metadata Driven

Applications should be generated from metadata wherever possible.

Examples include:

* Forms
* Fields
* Relationships
* Dashboards
* Reports
* Workflows
* Integrations

---

### Separation of Concerns

Each responsibility has exactly one owner.

| Concern             | Owner                         |
| ------------------- | ----------------------------- |
| Validation          | FluentValidation              |
| Exception Handling  | Global Exception Middleware   |
| Database Mapping    | EF Core Configuration Classes |
| Authentication      | JWT                           |
| Logging             | ILogger / Serilog             |
| API Response Format | ApiResponse                   |
| Audit               | DbContext                     |

---

### Configuration over Code

Business behaviour should be configurable whenever possible.

Examples:

* Forms
* Validation Rules
* Dashboards
* Reports
* Workflows
* Import Mappings

---

### Reusability

New functionality should be implemented in a reusable manner rather than solving only the immediate business requirement.

---

### Enterprise Readiness

Architecture decisions should favour:

* Maintainability
* Extensibility
* Performance
* Clear separation of responsibilities
* Long-term scalability

over short-term convenience.

---

## Technology Direction

### Backend

* ASP.NET Core (.NET 10)
* EF Core
* SQL Server 2025

### Database Strategy

* SQL Server is the primary data store.
* Metadata is stored relationally.
* Dynamic runtime data will use SQL Server JSON capabilities where appropriate.
* No NoSQL database is planned.

### API

REST APIs will be the primary communication mechanism.

SignalR will be introduced for real-time communication where required.

---

## Consequences

### Positive

* Highly extensible architecture.
* Suitable for enterprise applications.
* Clear separation between infrastructure and business logic.
* Easier long-term maintenance.

### Trade-offs

* Higher initial design effort.
* Slightly slower feature development during early phases.
* Requires discipline to keep platform concerns separate from business concerns.

---

## Alternatives Considered

### Traditional CRUD Application

Rejected.

Reason:

The long-term vision extends far beyond CRUD functionality.

---

### Multiple Independent Modules

Rejected.

Reason:

Would result in duplicated infrastructure and inconsistent architecture.

---

## Future Roadmap

Phase 1

* Infrastructure
* Persistence
* Authentication
* Audit
* Logging

Phase 2

* Metadata Engine
* Applications
* Forms
* Fields
* Relationships

Phase 3

* Runtime Engine
* Dynamic Forms
* Dynamic Grids
* Dashboards
* Workflows

Phase 4

* Integrations
* Excel Processing
* APIs
* Background Jobs
* SignalR

---

## Notes

This ADR establishes the architectural vision for Forge.

All future architectural decisions should be evaluated against this document.
