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

The `ForgeDB` SSDT project is not currently present in this workspace. The SQL below defines the required schema, but the matching SSDT files must still be added to the database-project repository.

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

`MetaInterface` is a narrow materialized projection used for frequent Class↔Interface capability checks. It is not the source of truth: the matching `Implements` and optional `PrimaryInterface` edges remain in `MetaObjectRelationship`.

The source edges and projection must be created, updated, activated, deactivated, and deleted atomically. `MetaInterface` must not expose an independent write path.

```sql
CREATE TABLE [dbo].[MetaInterface]
(
    [Id]           INT NOT NULL PRIMARY KEY IDENTITY(1,1),
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

`MetaPropertyValue.Id` is intentionally `BIGINT`. This must not be implemented by changing every shared `BaseEntity.Id` to `long`; use an entity-specific key or a generic base-key design so existing `INT` entities remain compatible.

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
4. Create the authoritative `Implements` relationship.
5. Create `PrimaryInterface` when requested.
6. Create the matching `MetaInterface` projection.
7. Persist all rows in one transaction/save operation.

Generic relationship authoring must reject direct creation of `Implements` and `PrimaryInterface`; otherwise callers can bypass projection synchronization.

Specialized update, activate, deactivate, and delete operations must synchronize the generic relationships and projection in the same transaction. These operations are not implemented yet.

## 5. Current implementation status

Implemented or partially implemented:

- `MetaInterface`, `MetaPropertyValue`, and `MetaPropertyValueDetail` entities.
- EF configurations and `DbSet` properties for the three tables.
- Composite `(ObjUid, IfUid)` EF relationship.
- `MetaInterfaceSeeder` and reseed deletion ordering.
- Interface-implementation request, response, validator, service method, and controller endpoint.
- Atomic single-save creation of source edges and the projection.
- Atomic UUID counter update using `UPDATE ... OUTPUT`.
- Empty-UUID guard in `AppDbContext.SaveChangesAsync`.

Known implementation corrections still required:

- Restore shared entity key types expected by existing tables and consumers; the current global `long BaseEntity.Id` change breaks the build.
- Give `MetaPropertyValue` its required `long` key without changing unrelated entities.
- Remove the duplicate lowercase `MetaInterface.isPrimary`; retain only `IsPrimary`.
- Align `MetaPropertyValue` with SQL: `double? ValueFloat`, `DateTimeOffset? ValueDateTime`, and `bool IsExtended`.
- Decide whether `Ordinal` and `IsActive` belong in `MetaPropertyValue`; they are not part of the finalized SQL above.
- Rename files that contain a trailing space before `.cs`.
- Make relationship/projection seeding atomic.
- Add the actual SSDT table scripts and project entries.
- Prevent generic relationship writes from bypassing interface projection synchronization.
- Implement synchronized update/deactivate/activate/delete operations.

## 6. Testing strategy

Unit tests are intentionally scheduled for a dedicated testing sprint. That sprint should cover:

- Successful interface implementation creation.
- Primary and non-primary paths.
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
