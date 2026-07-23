##   

## 17\. Merge / Import Conflict Handling

## 

Once Forge supports package-based deployment and promotion, it also needs a clear strategy for **what happens when imported configuration collides with configuration that already exists in the target environment**.

This section defines the current Forge v1 direction for handling conflicts during package import, especially for:

-   existing object collisions
-   version conflicts
-   MasterPrefix-related collisions
-   update vs skip vs reject behavior

The goal is to make import behavior **predictable, auditable, and safe** rather than allowing silent overwrites or ambiguous merges.

* * *

### 17.1 Why Conflict Handling Is Needed

## 

When a Forge package is imported into a target environment, the target may already contain:

-   the same application
-   the same object/class/interface/view/graph
-   an older version of that object
-   a different object with the same display name
-   a different configuration package that used the same logical naming

Without conflict handling rules, Forge would not know whether it should:

-   insert the incoming item as new
-   replace the existing item
-   reject the import
-   skip the incoming item
-   archive the old one
-   flag the import for manual review

So conflict handling is a required part of the deployment model.

* * *

### 17.2 Conflict Types Forge Must Handle

## 

Forge v1 should be prepared to handle at least the following conflict categories:

1.  **Existing object collision**
2.  **Version conflict**
3.  **MasterPrefix / namespace collision**
4.  **Dependency conflict**
5.  **Package re-import / duplicate import attempt**

This section focuses primarily on the first three, since they are the most central to the current Forge design.

* * *

### 17.3 Existing Object Collision

## 

An **existing object collision** happens when the imported package contains an object/configuration item that Forge believes already exists in the target environment.

Examples:

-   same object `Uid` already exists in target
-   same configured application object already exists in target
-   same shared object already exists but at a different version
-   same logical config artifact already exists in active state

This is the most common type of import collision.

* * *

### 17.4 Primary Identity Rule for Collision Detection

## 

Forge should not use **display name alone** to decide whether two objects are the same.

The primary identity for collision detection should come from platform identity fields such as:

-   `Uid`
-   package/application identity context
-   object type / metadata type
-   possibly MasterPrefix / package namespace context where relevant

### Why name alone is not enough

## 

Two different objects may both be named `Employee`, but they may not represent the same configuration identity.

So collision detection must be based on **stable identity**, not just friendly names.

* * *

### 17.5 Recommended Collision Detection Order

## 

A reasonable Forge v1 collision-detection order is:

#### Level 1 — Stable identity match

## 

Check whether the imported item matches an existing target item by stable identity, such as:

-   `Uid`
-   object type
-   package/application identity context

#### Level 2 — Namespace / MasterPrefix context

## 

If needed, check whether the item belongs to the same MasterPrefix / configuration namespace.

#### Level 3 — Name collision warning

## 

If identity does not match but name overlaps, treat it as a **warning / validation concern**, not automatic equality.

This helps avoid accidental merging of unrelated objects just because they share a display name.

* * *

### 17.6 Version Conflict

## 

A **version conflict** occurs when Forge identifies the imported object as the same logical object as an existing target object, but the version relationship is not trivial.

Examples:

-   target already has the exact same version
-   target has an older version
-   target has a newer version than the package being imported
-   target has a different branch/history of the same object

Version conflict handling is important because Forge uses **versioned supersede behavior** rather than destructive overwrite.

* * *

### 17.7 Recommended Version Conflict Rules

## 

Forge v1 should broadly treat version conflicts as follows.

#### Case A — Incoming version is newer than active target version

## 

Recommended action:

-   allow import
-   supersede the old active version
-   insert the new imported version as active

#### Case B — Incoming version is exactly the same as target active version

## 

Recommended action:

-   do **not** create a duplicate
-   mark as already imported / no-op / skip
-   still record the import attempt in history

#### Case C — Incoming version is older than target active version

## 

Recommended action:

-   reject by default, or require explicit override
-   do not silently downgrade active configuration

This protects the target environment from accidental rollback-by-import.

* * *

### 17.8 Re-import of the Same Package / Same Version

## 

Forge should be able to detect when the same package or same object version is imported again.

In such cases, Forge should normally:

-   avoid creating duplicate active records
-   record the import attempt in history
-   treat the operation as **already applied** or **no-op**, unless explicit re-apply behavior is supported later

This makes repeated deployment safer and more auditable.

* * *

### 17.9 MasterPrefix / Namespace Collision

## 

MasterPrefix exists to help reduce configuration identity collisions across independently configured applications or onboarding packages.

A **MasterPrefix conflict** occurs when:

-   two imported configurations appear to come from different configuration spaces
-   but they collide in identity or naming in the same target environment
-   or the same logical object appears with inconsistent namespace / package ownership signals

### Example

## 

-   package A introduces `HR.Employee`
-   package B introduces another `Employee` from a different onboarding context
-   both land in the same target environment

MasterPrefix is not the only identity mechanism, but it helps Forge reason about whether two similarly named objects belong to the same configuration space or not.

* * *

### 17.10 Recommended MasterPrefix Conflict Behavior

## 

If two imported objects:

-   have different MasterPrefix / package namespace context
-   but collide in naming or intended target identity

Forge should **not silently merge them**.

Instead, Forge should treat this as one of the following:

1.  **validation error**
2.  **manual review required**
3.  **explicit import override scenario**

The key principle is:

> **Different namespace / MasterPrefix context should not be silently treated as the same object unless Forge has a very strong identity match and explicit merge rule.**

* * *

### 17.11 Dependency Conflict

## 

A dependency conflict occurs when an imported item depends on another object/configuration that is:

-   missing in the target
-   present but incompatible
-   present at an incompatible version
-   present but archived / inactive when an active dependency is expected

Examples:

-   a View references a Property that does not exist in target
-   a Graph depends on a Relationship that was not imported
-   an application object depends on a shared object that is missing or older than required

Forge should treat dependency conflict as an import validation failure unless a clear recovery path exists.

* * *

### 17.12 Merge Strategy — Forge v1 Direction

## 

The current Forge v1 direction is **not** to do free-form record merging.

Instead, Forge should prefer a controlled strategy:

#### For known same-object updates

## 

-   supersede old version
-   insert new version

#### For exact duplicates / same version

## 

-   skip as already imported

#### For older incoming versions

## 

-   reject by default

#### For ambiguous collisions

## 

-   reject or require manual review rather than guessing

This keeps Forge import deterministic and safer.

* * *

### 17.13 Overwrite vs Supersede vs Skip vs Reject

## 

Forge v1 should conceptually support four broad outcomes during import evaluation:

#### 1\. Insert

## 

Used when the object does not exist in target.

#### 2\. Supersede + Insert

## 

Used when the same object exists and the incoming version is a valid newer version.

#### 3\. Skip / No-op

## 

Used when the exact same version already exists or the package has already effectively been applied.

#### 4\. Reject

## 

Used when:

-   the incoming version is older
-   identity is ambiguous
-   dependency validation fails
-   MasterPrefix / namespace conflict cannot be resolved safely

Forge should strongly prefer these explicit outcomes over hidden partial merges.

* * *

### 17.14 Why Forge Should Avoid “Smart Auto Merge” in v1

## 

It may be tempting to let Forge automatically merge partially overlapping objects/configuration, but this is risky in a metadata-driven platform.

Auto-merge can create problems such as:

-   unintended field/property mixing
-   incorrect object lineage
-   broken dependencies
-   loss of auditability
-   hard-to-debug runtime behavior

For Forge v1, the safer direction is:

> **Be strict and explicit during import. Prefer reject/skip/supersede over magical merge behavior.**

More advanced merge support can be introduced later if there is a strong need.

* * *

### 17.15 Relationship to Import History

## 

Conflict handling should always be recorded in import history.

Import history should capture at least:

-   package identity
-   import timestamp
-   import result
-   whether each item was inserted / superseded / skipped / rejected
-   conflict reason where applicable

This is important because conflict behavior is often one of the hardest parts of deployment troubleshooting.

* * *

### 17.16 Relationship to Lifecycle and Versioning

## 

Conflict handling cannot be separated from lifecycle and versioning.

#### Versioning decides

## 

-   whether the incoming object is newer / same / older

#### Lifecycle helps decide

## 

-   which target version is active
-   which older versions should be archived or superseded

So merge/import conflict handling must always operate in combination with:

-   object identity
-   object version
-   lifecycle state
-   package/application context

* * *

### 17.17 Recommended Forge v1 Conflict-Handling Rules

## 

The current Forge v1 direction can be summarized as follows:

#### Rule 1 — Use stable identity, not name alone

## 

Collision detection should be based on `Uid` and related identity context, not just display name.

#### Rule 2 — Newer version may supersede active older version

## 

If the incoming version is clearly newer and identity matches, supersede the old active version and insert the new one.

#### Rule 3 — Same version should not duplicate

## 

If the same version already exists, treat the import as skip / already applied.

#### Rule 4 — Older version should be rejected by default

## 

Do not silently downgrade target configuration.

#### Rule 5 — Ambiguous namespace / MasterPrefix conflicts should not auto-merge

## 

Prefer reject or manual review over unsafe merging.

#### Rule 6 — Dependency conflicts should fail validation

## 

Do not activate incomplete or broken configuration.

#### Rule 7 — Every decision should be logged in import history

## 

Insert, supersede, skip, and reject should all be auditable.

* * *

### 17.18 Open Details Still to Be Finalized

## 

The following details still need implementation-level refinement:

-   exact identity matching algorithm
-   exact “newer than / older than” comparison logic for all imported artifacts
-   whether manual override mode exists during import
-   whether dry-run conflict preview will be supported
-   whether partial package import is allowed if only some objects conflict
-   whether conflicts are resolved per object, per application, or per package
-   how shared objects across multiple consuming applications should behave during conflict resolution

These are important implementation questions, but they do not change the core Forge v1 direction already agreed.

* * *

### 17.19 Summary

## 

Forge v1 should treat merge/import conflict handling as a **strict, identity-aware, version-aware import decision model**.

The current direction is:

-   detect conflicts using stable identity, not name alone
-   supersede older versions with newer valid versions
-   skip exact duplicates
-   reject older imports and ambiguous collisions by default
-   use MasterPrefix / namespace information as an additional safety boundary
-   validate dependencies before activation
-   record all outcomes in import history

This gives Forge a safer and more predictable import model for package-based deployment across environments.