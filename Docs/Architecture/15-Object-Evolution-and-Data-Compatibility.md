## 15\. Object Evolution and Data Compatibility

## 

Forge objects and configurations are expected to evolve over time. New interfaces may be added, new properties may be introduced, and existing structures may change as the configured application grows.

This creates an important platform question:

> **What happens to existing runtime data when the object definition changes?**

Forge needs a clear object evolution and compatibility model so that configuration can evolve without breaking previously stored business data.

* * *

### 15.1 Problem Statement

## 

Consider an object with the following lifecycle.

#### Step 1 — Initial version

## 

Object version = `1.0.0.1`

At this point, the object exists and runtime data has already been stored.

Example:

-   existing row count = **100**

#### Step 2 — Object is updated

## 

The object is updated and now includes:

-   **1 new interface**
-   that interface has **2 new properties**

New object version may become:

-   `1.0.0.2`  
    or under a new app version lineage:
-   `1.0.2.1`

#### Step 3 — New data is created

## 

New runtime rows are added after the schema change.

Example:

-   new row count = **20**

#### Resulting question

## 

The old 100 rows were created **before** the new interface and properties existed.  
So those old rows do not have values for the new properties.

Forge therefore needs to decide:

-   Are the old rows still valid?
-   Are the new properties treated as null for old rows?
-   Can the new properties be mandatory?
-   Do old rows need migration or backfill?

This is the core object evolution problem.

* * *

### 15.2 Core Forge v1 Principle

## 

The current Forge v1 direction is:

> **Additive schema changes should be allowed without invalidating existing data.**

This means that if an object gains:

-   a new optional interface
-   a new optional property

then **existing runtime rows remain valid** even if they do not contain values for those newly introduced fields.

* * *

### 15.3 Existing Data Remains Valid

## 

When an object is updated by adding new interfaces or properties, old runtime data should not automatically become invalid.

#### Example

## 

Before update:

-   100 Employee rows already exist

After update:

-   Employee object now has 2 new properties introduced through a new interface

Result:

-   the original 100 rows remain valid
-   the new properties are simply absent / null / empty for those older rows
-   new rows created after the change can populate the new properties

This is the baseline compatibility rule for Forge v1.

* * *

### 15.4 Additive Changes

## 

Forge v1 should treat the following as **additive changes**:

-   adding a new optional interface to an object
-   adding a new optional property to an existing interface
-   adding a new optional enum-backed field
-   adding a new optional reference field

These changes should generally be considered **safe evolution changes**, because they do not require previously stored data to already have those values.

#### Compatibility behavior for additive changes

## 

-   existing rows remain valid
-   newly added fields are treated as null / absent for older rows
-   new rows may populate the new fields
-   old rows may optionally be backfilled later if needed

* * *

### 15.5 Optional New Properties

## 

Forge v1 should assume that **newly added properties are optional by default** unless explicitly handled otherwise.

This is important because it allows object evolution without forcing immediate migration of existing business data.

#### Example

## 

Object update introduces:

-   `EmergencyContactNumber`
-   `AssetCostCenter`

If older rows do not have those values, that should be acceptable as long as the new properties are optional.

#### Expected behavior

## 

-   old rows → null / empty / absent values
-   new rows → values may be provided
-   updated old rows → values may be added later if the application chooses to backfill them

* * *

### 15.6 Mandatory New Properties Are a Special Case

## 

The difficult case is when a new property is added and that property is intended to be **mandatory**.

Example:

-   a new `EmployeeCategory` property is introduced
-   the application wants it to be required for every Employee record

This creates a compatibility problem because older rows do not have a value for that property.

Forge v1 therefore should not treat “new mandatory property” as a normal additive change.

* * *

### 15.7 Recommended Rule for Mandatory New Properties

## 

If a new property is added to an already-used object and that property is **mandatory**, Forge should require one of the following before or during publish/deployment:

1.  **Default value**
2.  **Backfill / migration rule**
3.  **Explicit acknowledgement that historical records remain incomplete until manually updated**

#### Meaning

## 

The platform should not silently pretend that older rows satisfy a mandatory field they never had.

So mandatory additions need extra handling.

* * *

### 15.8 Suggested Mandatory-Property Handling Options

## 

Forge v1 can support one or more of the following strategies when a mandatory property is introduced on an already-used object:

#### Option A — Require a default value

## 

The admin/configurator provides a default value that can be used for old rows.

#### Option B — Require a migration/backfill rule

## 

The admin/configurator provides a rule or migration step to populate the old rows.

#### Option C — Allow publish but mark old data as incomplete

## 

The platform allows the new version to be published, but historical rows are treated as incomplete until manually corrected.

The exact implementation can be refined later, but the important v1 principle is:

> **Mandatory additions require explicit handling; they are not a silent compatibility change.**

* * *

### 15.9 Safe vs Risky Evolution Changes

## 

Forge v1 should conceptually distinguish between two broad categories of object evolution:

#### Safe / Additive Changes

## 

Examples:

-   adding a new optional property
-   adding a new optional interface
-   adding a new optional enum/reference field

#### Risky / Compatibility-Sensitive Changes

## 

Examples:

-   adding a mandatory property to an object with existing data
-   removing a property that existing views/data depend on
-   changing a property’s meaning or type incompatibly
-   changing relationships in a way that breaks existing graph/view assumptions

Forge v1 does not need a full-blown change-classification engine immediately, but it should conceptually recognize that not all schema changes are equally safe.

* * *

### 15.10 Object Evolution Does Not Automatically Rewrite Existing Data

## 

A key Forge v1 principle is:

> **Changing the object definition does not automatically rewrite all historical runtime rows.**

This is important because automatic data rewriting can be expensive, risky, and difficult to control.

So unless an explicit migration/backfill process is defined:

-   older rows remain as they were
-   newly introduced fields remain empty for them
-   new or updated rows can gradually adopt the newer structure

* * *

### 15.11 Relationship to Versioning

## 

Object evolution is tightly connected to the versioning model.

When an object changes:

-   a new object version is created
-   older runtime data may still reflect the shape of earlier versions
-   the platform must interpret the current object definition while still respecting historical data compatibility

This is one of the reasons Forge needs explicit object versioning rather than assuming every record always perfectly matches the newest object structure.

* * *

### 15.12 Relationship to Views, Graphs, and Runtime Consumption

## 

If a new property is added to an object, Views and Graphs may start using it. However, those Views/Graphs must still be able to handle older rows that do not yet contain values for the property.

That means Forge runtime consumers should be prepared for:

-   null values
-   absent values in older rows
-   partial historical data coverage for newer fields

This is a normal consequence of schema evolution in a long-lived configurable platform.

* * *

### 15.13 Forge v1 Compatibility Rule Set

## 

The current Forge v1 compatibility direction can be summarized as follows:

#### Rule 1 — Existing rows remain valid after additive changes

## 

Adding new optional interfaces/properties does not invalidate historical rows.

#### Rule 2 — New optional properties may be null for historical data

## 

Old rows are allowed to have no value for newly introduced optional fields.

#### Rule 3 — New rows may use the newer structure immediately

## 

Rows created after the change can populate the new fields.

#### Rule 4 — Mandatory additions require explicit handling

## 

If a newly added field is mandatory, Forge should require defaulting, migration, or explicit acknowledgement of incomplete historical data.

#### Rule 5 — Object evolution does not silently rewrite all existing rows

## 

Any backfill or migration should be deliberate, not implicit.

* * *

### 15.14 What Forge v1 Does Not Fully Lock Yet

## 

The following implementation details are still open and can be refined later:

-   whether Forge will have a formal “schema migration job” concept
-   how default values for mandatory fields are stored and executed
-   whether old rows can be explicitly marked as incompatible/incomplete
-   whether compatibility warnings should appear during publish/import
-   how strongly view/graph validation should enforce compatibility awareness

These are important future details, but they do not change the core Forge v1 direction already agreed.

* * *

### 15.15 Summary

## 

Forge v1 should treat object evolution with the following mindset:

1.  **Additive changes are allowed**
2.  **Historical data remains valid**
3.  **New optional properties can be null for old rows**
4.  **Mandatory new properties require explicit handling**
5.  **Object evolution should not automatically rewrite historical runtime data**

This gives Forge a practical and safe starting point for schema evolution while still leaving room for stronger migration tooling later.