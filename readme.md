# Forge — Architecture & Implementation Guide

Forge is a configurable, metadata-driven platform. Classes, interfaces, properties, relationships, views, actions, APIs, and events are represented as data rather than hardcoded business schema. New product types can therefore be defined without recompiling the API or adding one database table per business type.

This document is the architecture handoff and implementation source of truth. Read it before changing the meta-schema because decisions here may intentionally differ from assumptions suggested by partial code.

## 1. Platform layers

| Layer | Purpose | Status |
|---|---|---|
| Meta | Platform primitives such as `Class`, `Interface`, `Property`, `Relation`, `Graph`, `View`, and `Action` | Partially implemented |
| Configure | Product definitions such as `FeatureSet`, `Method`, `Form`, `DataGrid`, and `DataColumn` | Not started |
| Data | High-volume runtime business objects and their values | Not started |

The Configure layer does not require a separate table family. Configurable concepts are deeper `MetaObject` rows connected through `ObjTypeUid` and the generic relationship graph.

Examples:

| Concept | Representation |
|---|---|
| `FeatureSet` | `MetaObject.ObjTypeUid → Class` |
| `Method` | Product-facing name for `Action` |
| `Form` | A `View` reached through `Interface → Graph → View` |
| `DataGrid` | A specialized view whose type ultimately derives from `View` |
| `DataColumn` | Metadata object connected to the property or relation path it displays |

## 2. Persistence conventions

- Backend: .NET with feature folders.
- ORM: EF Core for queries and writes only.
- Database: SQL Server.
- Database schema source of truth: the `ForgeDB` SSDT project and its `.dacpac`.
- Do not create or apply EF migrations during normal development.
- Do not call `Database.Migrate()` or `EnsureCreated()` from application startup.
- EF entity configurations must manually match SSDT column names, types, nullability, alternate keys, foreign keys, indexes, and delete behavior.
- Existing files under `Infrastructure/Persistence/Migrations` are legacy and excluded from compilation.

The external `ForgeDB` SSDT project is available in the separate `DB_Forge` repository. Its table scripts and generated `.dacpac` are the database schema source of truth; the SQL below documents the schema that the EF model must match.

## 3. Meta-schema storage model

### `MetaObject`

The universal metadata node. Concrete platform concepts and product-configured concepts are rows in this table. Root types may have `ObjTypeUid = NULL`; configured types point to another `MetaObject` through `ObjTypeUid`.

### `MetaObjectRelationship`

The authoritative generic relationship graph. It stores all relationship facts, including:

- `Implements`
- `PrimaryInterface`
- `Exposes`
- `HasDirectedRel`
- `HasDirectedRelPath`
- `HasGraph`
- `HasView`
- `HasAction`
- `HasApi`
- `HasEvent`
- `InvokesAction`
- `DataTypeScope`
- `Requires*`

### `MetaInterface`

`MetaInterface` is a narrow materialized projection used for frequent Class↔Interface capability checks. It is not the source of truth. Each implementation has exactly one authoritative `MetaObjectRelationship`: `Implements` for a non-primary implementation or `PrimaryInterface` for a primary implementation.

`MetaInterface.IsPrimary` projects which of those two relationship types is authoritative for the implementation; it does not replace the `PrimaryInterface` relationship type.

The source edges and projection must be created, updated, activated, deactivated, and deleted atomically. `MetaInterface` must not expose an independent write path.

```sql
CREATE TABLE [dbo].[MetaInterface]
(
    [Id]           BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [IfUid]        UNIQUEIDENTIFIER NOT NULL,
    [ObjUid]       UNIQUEIDENTIFIER NOT NULL,
    [InterfaceUid] UNIQUEIDENTIFIER NOT NULL,
    [IsPrimary]    BIT NOT NULL DEFAULT 0,
    [Ordinal]      INT NOT NULL DEFAULT 0,
    [IsActive]     BIT NOT NULL DEFAULT 1,
    [CreatedAt]    DATETIMEOFFSET NOT NULL,
    [CreatedBy]    INT NULL,
    [UpdatedAt]    DATETIMEOFFSET NULL,
    [UpdatedBy]    INT NULL,
    [DeletedAt]    DATETIMEOFFSET NULL,
    [DeletedBy]    INT NULL,
    CONSTRAINT [FK_MetaInterface_MetaObject_ObjUid]
        FOREIGN KEY ([ObjUid]) REFERENCES [dbo].[MetaObject]([ObjUid]),
    CONSTRAINT [FK_MetaInterface_MetaObject_InterfaceUid]
        FOREIGN KEY ([InterfaceUid]) REFERENCES [dbo].[MetaObject]([ObjUid])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_MetaInterface_IfUid]
    ON [dbo].[MetaInterface] ([IfUid])
GO

-- Required as the principal key of MetaPropertyValue's composite FK.
CREATE UNIQUE NONCLUSTERED INDEX [IX_MetaInterface_ObjUid_IfUid]
    ON [dbo].[MetaInterface] ([ObjUid], [IfUid])
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_MetaInterface_ObjUid_InterfaceUid]
    ON [dbo].[MetaInterface] ([ObjUid], [InterfaceUid])
GO

CREATE NONCLUSTERED INDEX [IX_MetaInterface_InterfaceUid]
    ON [dbo].[MetaInterface] ([InterfaceUid])
GO
```

### `MetaPropertyValue`

This is a value store, not the definition of which properties an interface exposes. Interface→Property definitions remain `Exposes` relationships.

Both `ObjUid` and `IfUid` are retained deliberately:

- `ObjUid` supports direct, high-frequency loading of all values owned by an object.
- `IfUid` records the interface implementation through which the property value is supplied.
- The composite FK prevents an `IfUid` belonging to one object from being attached to another object's value.

```sql
CREATE TABLE [dbo].[MetaPropertyValue]
(
    [Id]            BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [PrUid]         UNIQUEIDENTIFIER NOT NULL,
    [ObjUid]        UNIQUEIDENTIFIER NOT NULL,
    [IfUid]         UNIQUEIDENTIFIER NOT NULL,
    [PropertyUid]   UNIQUEIDENTIFIER NOT NULL,
    [ValueString]   NVARCHAR(4000) NULL,
    [ValueFloat]    FLOAT NULL,
    [ValueBool]     BIT NULL,
    [ValueDateTime] DATETIMEOFFSET NULL,
    [Uom]           NVARCHAR(128) NULL,
    [IsExtended]    BIT NOT NULL DEFAULT 0,
    [IsActive]      BIT NOT NULL DEFAULT 1,
    [CreatedAt]     DATETIMEOFFSET NOT NULL,
    [CreatedBy]     INT NULL,
    [UpdatedAt]     DATETIMEOFFSET NULL,
    [UpdatedBy]     INT NULL,
    [DeletedAt]     DATETIMEOFFSET NULL,
    [DeletedBy]     INT NULL,
    CONSTRAINT [FK_MetaPropertyValue_MetaObject_ObjUid]
        FOREIGN KEY ([ObjUid]) REFERENCES [dbo].[MetaObject]([ObjUid]),
    CONSTRAINT [FK_MetaPropertyValue_MetaInterface_ObjUid_IfUid]
        FOREIGN KEY ([ObjUid], [IfUid])
        REFERENCES [dbo].[MetaInterface]([ObjUid], [IfUid]),
    CONSTRAINT [FK_MetaPropertyValue_MetaObject_PropertyUid]
        FOREIGN KEY ([PropertyUid]) REFERENCES [dbo].[MetaObject]([ObjUid])
)
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_MetaPropertyValue_PrUid]
    ON [dbo].[MetaPropertyValue] ([PrUid])
GO

CREATE NONCLUSTERED INDEX [IX_MetaPropertyValue_IfUid_PropertyUid]
    ON [dbo].[MetaPropertyValue] ([IfUid], [PropertyUid])
GO

CREATE NONCLUSTERED INDEX [IX_MetaPropertyValue_PropertyUid]
    ON [dbo].[MetaPropertyValue] ([PropertyUid])
GO
```

Entity primary keys use `BIGINT`, including `MetaPropertyValue.Id`, and the shared `BaseEntity.Id` is mapped as `long`.

### `MetaPropertyValueDetail`

One-to-one overflow storage for values too large for inline columns. When used, `MetaPropertyValue.IsExtended = 1`.

```sql
CREATE TABLE [dbo].[MetaPropertyValueDetail]
(
    [PropertyValueId] BIGINT NOT NULL PRIMARY KEY,
    [Value] VARBINARY(MAX) NULL,
    CONSTRAINT [FK_MetaPropertyValueDetail_MetaPropertyValue]
        FOREIGN KEY ([PropertyValueId])
        REFERENCES [dbo].[MetaPropertyValue]([Id])
)
GO
```

## 4. Interface implementation authoring

`POST /api/MetaSchema/interface-implementations` is the dedicated creation path.

One call must:

1. Validate the object and interface.
2. Reject duplicate object/interface implementations.
3. Enforce at most one active primary interface per object.
4. Create exactly one authoritative relationship:
   - `Implements` for a non-primary interface implementation.
   - `PrimaryInterface` for a primary interface implementation.
5. Create the matching `MetaInterface` projection.
6. Persist the relationship and projection in one explicit transaction. The shared
   relationship creation path saves the relationship before the projection is saved.

Generic relationship authoring must reject direct creation of `Implements` and `PrimaryInterface`; otherwise callers can bypass projection synchronization.

Specialized update, activate, deactivate, and delete operations must synchronize the generic relationships and projection in the same transaction. These operations are not implemented yet.

## 5. Current implementation status

Implemented or partially implemented:

- `MetaInterface`, `MetaPropertyValue`, and `MetaPropertyValueDetail` entities.
- EF configurations and `DbSet` properties for the three tables.
- Composite `(ObjUid, IfUid)` EF relationship.
- `MetaInterfaceSeeder` and reseed deletion ordering.
- Interface-implementation request, response, validator, service method, and controller endpoint.
- Transactional creation of one source edge and the projection through the shared
  relationship creation path.
- Atomic UUID counter update using `UPDATE ... OUTPUT`.
- Dedicated `GenerateInterfaceUuidAsync` generation using UUID entity type `4` for `MetaInterface.IfUid`.
- SSDT table scripts for `MetaInterface`, `MetaPropertyValue`, and `MetaPropertyValueDetail`.
- Unique `(ObjUid, IfUid)` principal key and matching composite `MetaPropertyValue` foreign key.
- `MetaPropertyValue.IsActive`, inherited through `ActivatableEntity` and mapped to the SSDT column.
- Generic relationship writes reject `Implements` and `PrimaryInterface`; specialized interface authoring uses the shared relationship creation path internally.
- Empty-UUID guard in `AppDbContext.SaveChangesAsync`.

Outstanding implementation work:

- Make relationship/projection seeding atomic and align it with the one-relationship model (GitHub issue #19).
- Implement synchronized update/deactivate/activate/delete operations.

## 6. Testing strategy

Unit tests are intentionally scheduled for a dedicated testing sprint. That sprint should cover:

- Successful interface implementation creation.
- Primary and non-primary paths, each producing exactly one relationship.
- Duplicate implementation rejection.
- Multiple-primary rejection.
- Missing object/interface validation.
- Atomic rollback when any source-edge or projection write fails.
- Composite `(ObjUid, IfUid)` integrity.
- Synchronization during update, activation, deactivation, and deletion.
- Rejection of direct `Implements`/`PrimaryInterface` writes through the generic endpoint.

Integration tests should run against SQL Server or a compatible test container because EF's in-memory provider does not enforce relational foreign keys and indexes.

## 7. Deferred work

- High-volume Data-layer tables: `DataObject`, `DataObjectValue`, `DataObjectValueDetail`, and `DataObjectRelationship`.
- Dynamic OData `IEdmModel` generation from the metadata catalog.
- Multi-domain/tenant federation.
- Baseline/configuration versioning.
- Multi-vendor organization tracking.
- Instance-level interface claims beyond the object's configured class.
- Denormalized read projections beyond `MetaInterface`, until measured load requires them.

## 8. Decisions still required

- Activate `DataTypeScope` now or defer it until property-value write APIs are implemented.
- Seed `GridView` and `FormView` now or during the Configure-layer milestone.
- Define the allowed value-column invariant for `MetaPropertyValue` and enforce it with validation and, where practical, a SQL check constraint.
