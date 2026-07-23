## 13\. Lifecycle States

Forge needs explicit lifecycle states for metadata and configuration objects so the platform can distinguish between:

-   work that is still being prepared
-   work that is ready for runtime usage / deployment
-   work that should no longer be actively used

The current Forge v1 lifecycle model is intentionally simple and uses **three states**:

1.  **Draft**
2.  **Published**
3.  **Archived**

These lifecycle states apply primarily to **Metadata** and **AdminConfigData** objects.  
BusinessData may have its own business statuses, but those are separate from the platform lifecycle model defined here.

* * *

### 13.1 Why Lifecycle States Are Needed

Without lifecycle states, Forge would not be able to clearly distinguish between:

-   unfinished configuration vs released configuration
-   editable work-in-progress vs approved configuration
-   active definitions vs retired definitions

Lifecycle states help Forge support:

-   controlled configuration work
-   publish/release behavior
-   deployment readiness
-   history and auditability
-   safe retirement of old configuration

* * *

### 13.2 Draft

**Draft** represents a work-in-progress state.

An object/configuration in Draft exists in the platform but is **not yet considered released / active for official use**.

#### Draft is intended for:

-   new metadata/configuration being created
-   edits being prepared before release
-   incomplete or under-review work
-   configuration that should not yet be treated as the active published version

#### Typical behavior of Draft

-   editable by the relevant admin/developer users
-   not yet considered the final active released version
-   may be replaced, revised, or discarded before publishing
-   suitable for iterative platform/configuration work

#### Examples

-   a newly created interface still being designed
-   an application view configuration still under setup
-   an updated object definition waiting for review before promotion

* * *

### 13.3 Published

**Published** represents the released and active state of a metadata/configuration item.

An item in Published state is considered the version that Forge should treat as the official active version for use in the relevant runtime/configuration context.

#### Published is intended for:

-   released metadata/configuration
-   versions that are ready for deployment or runtime use
-   stable versions that should be consumed by configured applications

#### Typical behavior of Published

-   treated as the active version for the relevant context
-   used by downstream configuration/runtime behavior
-   should not be treated as casual work-in-progress
-   may later be superseded by a newer published version

#### Examples

-   a published Class definition
-   a published application configuration package
-   a published View used by the configured application

* * *

### 13.4 Archived

**Archived** represents a retired state.

An item in Archived state is kept for history, traceability, or compatibility reasons, but is **not considered active for current use**.

#### Archived is intended for:

-   old definitions no longer in active use
-   superseded versions that should be retained historically
-   retired configuration that should not be deleted outright

#### Typical behavior of Archived

-   not treated as the active configuration/version
-   retained for history and auditability
-   may still be referenced for import history, version lineage, or troubleshooting
-   should not normally be used for new runtime/configuration activity

#### Examples

-   an older object definition replaced by a newer published version
-   a retired configuration package kept for historical traceability
-   an old graph/view definition no longer actively used

* * *

### 13.5 Lifecycle Flow

The expected Forge v1 lifecycle flow is:

**Draft → Published → Archived**

This should be treated as the normal lifecycle path for metadata/configuration items.

#### Meaning

-   an item is first created or edited in **Draft**
-   once ready, it becomes **Published**
-   once retired or superseded, it may become **Archived**

* * *

### 13.6 Why the Lifecycle Is Kept Simple in v1

Forge already introduces significant complexity through:

-   metadata modeling
-   configuration layering
-   versioning
-   package deployment
-   import/merge rules
-   policy-based access

Because of that, lifecycle is intentionally kept simple in Forge v1.

The platform does **not** currently introduce additional lifecycle states such as:

-   Pending Approval
-   Deprecated
-   Disabled
-   Rejected
-   Soft Deleted

Those may be useful later, but they are not required to lock the first version of the platform.

* * *

### 13.7 Relationship Between Lifecycle and Versioning

Lifecycle state and version are related, but they are **not the same concept**.

#### Version

Represents **which revision of the object/configuration this is**.

#### Lifecycle State

Represents **what operational state that revision is currently in**.

#### Example

A Class definition may have:

-   version `1.2.0.1`
-   lifecycle state `Published`

Later, a new version may be created:

-   version `1.2.1.1`
-   lifecycle state `Draft`

And once the new version is published, the older published version may become `Archived`.

So version tells Forge **which revision**, while lifecycle tells Forge **how that revision should be treated operationally**.

* * *

### 13.8 Relationship Between Lifecycle and Import / Deployment

Lifecycle states also matter during deployment/import scenarios.

#### Example

A package import may:

-   introduce a new **Draft** configuration into a target environment
-   publish a new version of an existing object
-   archive an older version that is being superseded

The exact import/deployment rules are covered later, but lifecycle states provide the foundation for that behavior.

* * *

### 13.9 Expected Layer Usage

#### Metadata

Lifecycle states are useful for metadata definitions such as Classes, Interfaces, Relationships, Graphs, and Views.

#### AdminConfigData

Lifecycle states are useful for configured application objects, views, graph configurations, and deployment-ready configuration items.

#### BusinessData

Lifecycle states are not primarily discussed here for runtime business records. BusinessData may have its own business statuses, but those are conceptually separate from the metadata/configuration lifecycle model.

* * *

### 13.10 Forge v1 Lifecycle Summary

Forge v1 uses a simple three-state lifecycle model:

1.  **Draft**
    -   work in progress
    -   editable
    -   not yet the active released version
2.  **Published**
    -   released / active version
    -   ready for runtime/configuration use
3.  **Archived**
    -   retired / superseded / historical version
    -   retained for traceability but not actively used

This lifecycle model should be treated as the baseline state model for Forge metadata and configuration objects.