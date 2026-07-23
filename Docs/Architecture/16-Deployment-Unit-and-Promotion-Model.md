## 16\. Deployment Unit and Promotion Model

## 

Forge is intended to support configuration-driven application building across environments.  
That means the platform needs a structured way to **package, move, import, promote, and activate** metadata/configuration without relying on manual DB edits or ad-hoc scripts.

This section defines the current Forge v1 direction for:

-   what gets exported/imported/promoted
-   how deployment should be packaged
-   how configuration should move across environments
-   how old and new versions should be activated during promotion

* * *

### 16.1 Why Forge Needs a Deployment Model

## 

Forge is not only a runtime application.  
It is a platform where **Person 2** configures business applications and then needs to move that configuration into another environment for use by **Person 3**.

Without a deployment model, Forge would not have a clean answer to questions such as:

-   How does configured application data move from one environment to another?
-   What is the unit of promotion?
-   How do we preserve history of what was imported?
-   How do we activate a new version without losing the old one?
-   How do we keep package imports auditable and repeatable?

So Forge needs a formal **deployment and promotion model**, not just a storage model.

* * *

### 16.2 Core Deployment Direction

## 

The current Forge v1 direction is:

> **Forge should move configuration through a Forge-specific deployment package, imported into Forge through a controlled import pipeline.**

The deployment flow is expected to look broadly like this:

1.  configuration is prepared in a source environment
2.  Forge creates a package file
3.  that package is uploaded into a target Forge environment
4.  Forge validates, logs, and imports the package
5.  target objects are inserted / superseded according to import rules
6.  import history is retained for auditability and rollback reasoning

* * *

### 16.3 Deployment Unit

## 

The current design direction is that the main deployment unit should be a **Forge package** rather than raw individual objects or raw database copies.

#### Why package-based deployment

## 

A package-based model is preferred because it:

-   groups related configuration together
-   supports dependency validation
-   supports environment promotion
-   provides an auditable unit of deployment
-   avoids manual piecemeal object movement

#### Practical interpretation

## 

Instead of saying “move object X and object Y manually,” Forge should say:

> **Export a package representing a coherent configuration release, then import that package into the target environment.**

* * *

### 16.4 Forge Package File

## 

The proposed deployment artifact is a **Forge-specific package file**, conceptually similar to a `.bakpak` file.

#### Expected characteristics

## 

-   platform-specific format
-   intended to be read/imported by Forge
-   may be binary, compressed, or otherwise non-human-readable
-   acts as the transport artifact for configuration movement

#### Important note

## 

The exact file extension or serialization format is not yet the critical design decision.  
The more important decision is that Forge treats deployment as a **platform package artifact**, not as a manual SQL script or raw table copy.

* * *

### 16.5 What the Package Represents

## 

A Forge package should represent a **coherent promoted configuration set**.

Depending on the final implementation boundary, this may include:

-   application configuration
-   metadata references required by that configuration
-   object definitions/configuration
-   view/graph configuration
-   version information
-   import manifest information

The package should represent a unit that Forge can validate and import as one deployment action.

* * *

### 16.6 Recommended Packaging Principle

## 

Forge should package **configuration as a platform release artifact**, not as a random collection of records.

That means the package should conceptually contain:

1.  **identity of the package**
2.  **version of the package / application**
3.  **objects/configuration included**
4.  **dependency or relationship information needed for safe import**
5.  **enough metadata for Forge to validate and process the package**

The exact manifest structure can be refined later, but the package should be treated as a first-class platform artifact.

* * *

### 16.7 Promotion Across Environments

## 

The package model is intended to support promotion across environments such as:

-   development → test
-   test → UAT
-   UAT → production

or more generally:

-   source Forge environment → target Forge environment

#### Promotion principle

## 

The target environment should not be updated by ad-hoc manual editing.  
Instead, the target environment should receive configuration through a Forge package import process.

This makes promotion:

-   auditable
-   repeatable
-   version-aware
-   safer than direct manual manipulation

* * *

### 16.8 Import Pipeline Overview

## 

The current Forge v1 import direction is that package import should happen through a controlled multi-step pipeline.

A reasonable high-level import pipeline is:

1.  **Upload package**
2.  **Register import attempt**
3.  **Read package manifest**
4.  **Validate package structure and version**
5.  **Validate dependencies and identity conflicts**
6.  **Prepare insert/update/supersede actions**
7.  **Apply import transaction**
8.  **Write import history**
9.  **Mark result as success/failure**

This section does not lock every implementation detail, but it establishes that import should be treated as a **platform workflow**, not just a table copy.

* * *

### 16.9 Import History Logging

## 

Forge should maintain an **import history log** in the database.

This was one of the strongest parts of the proposed approach and should be retained.

#### Why import history is important

## 

Import history allows Forge to track:

-   what package was imported
-   when it was imported
-   by whom it was imported
-   which objects were inserted/updated/superseded
-   whether the import succeeded or failed
-   which package/version introduced the current active configuration

#### Import history should support

## 

-   auditability
-   troubleshooting
-   re-import handling
-   rollback reasoning
-   environment traceability

* * *

### 16.10 Insert vs Update Strategy

## 

The current Forge direction is **not** to do destructive in-place overwrite for configuration objects.

Instead, when an import updates an existing target object, Forge should prefer a **versioned supersede model**:

1.  existing active target object is deactivated / superseded
2.  incoming imported version is inserted as a new active record

This preserves history and aligns with the lifecycle/versioning model already discussed.

* * *

### 16.11 Supersede Instead of Overwrite

## 

The current recommended import behavior for an update case is:

-   **old version remains in DB**
-   old version becomes inactive / archived / superseded
-   new imported version is inserted as a new active version

#### Why this is preferred

## 

This approach is better than destructive overwrite because it provides:

-   history preservation
-   safer rollback possibilities
-   cleaner audit trail
-   alignment with object versioning and lifecycle states

* * *

### 16.12 Deployment Behavior for New vs Existing Objects

## 

Forge import should distinguish between two broad cases.

#### Case A — Object does not exist in target

## 

Action:

-   insert as new object/configuration record

#### Case B — Matching object already exists in target

## 

Action:

-   evaluate import/update rules
-   if incoming version should replace active version:
    -   deactivate/supersede existing active version
    -   insert imported object as new active version

This keeps import behavior consistent with versioned configuration management.

* * *

### 16.13 Transactional Import Principle

## 

Import should be treated as a **transactional platform operation**.

That means Forge should avoid leaving the target environment in a half-imported state if something goes wrong mid-import.

#### Preferred principle

## 

-   validate first as much as possible
-   apply import in a controlled transaction/batch
-   on failure, rollback the import operation where feasible

This becomes especially important because imported objects may have dependencies on:

-   other objects
-   interfaces
-   views
-   relationships
-   application configuration structures

* * *

### 16.14 Dependency Awareness During Import

## 

Package import cannot be treated as isolated record insertion.  
Forge must remain aware of dependencies between imported items.

#### Examples

## 

-   a View may depend on a Class and its Properties
-   a Graph may depend on Relationships and Interfaces
-   an application configuration may depend on shared objects or referenced metadata

So before activation/import, Forge should validate that:

-   required referenced items are present
-   imported objects are structurally consistent
-   dependency ordering or dependency availability is satisfied

The exact dependency-validation algorithm can be refined later, but the principle should be explicit in the design.

* * *

### 16.15 Relationship to Versioning and Lifecycle

## 

Deployment/import is tightly connected to both versioning and lifecycle.

#### Versioning

## 

Forge needs to know:

-   what application version the package represents
-   what object versions are being imported
-   whether the imported version is newer / same / conflicting

#### Lifecycle

## 

Forge needs to decide:

-   whether imported items become Draft / Published / Archived
-   whether older active versions become superseded or archived

So package import should not be designed separately from versioning and lifecycle.  
All three need to work together.

* * *

### 16.16 Recommended Forge v1 Deployment Direction

## 

The current Forge v1 deployment direction can be summarized as follows:

1.  Forge should use a **Forge-specific deployment package**
2.  package import should happen through a **controlled Forge import pipeline**
3.  Forge should maintain **import history logs**
4.  updates should prefer **supersede + insert new version**, not destructive overwrite
5.  import should be **dependency-aware**
6.  import should be **transactional where possible**
7.  deployment should align with **versioning + lifecycle**

* * *

### 16.17 Open Details Still to Be Finalized

## 

The following details still need implementation-level refinement:

-   exact Forge package format and extension
-   exact package manifest schema
-   whether package is application-scoped, onboarding-scoped, or environment-scoped
-   exact conflict detection rules during import
-   whether import supports dry-run / preview mode
-   rollback behavior for failed or partially applied promotions
-   whether Metadata and AdminConfigData are always deployed together or can sometimes be separated

These are important implementation questions, but they do not change the current Forge v1 direction already agreed.

* * *

### 16.18 Summary

## 

Forge v1 should treat deployment and promotion as a **package-based configuration movement model**.

The current direction is:

-   build a Forge package in the source environment
-   upload/import it into the target environment
-   validate it through a controlled pipeline
-   log the import in history
-   insert new versions instead of destructively overwriting old ones
-   supersede older active versions when needed

This gives Forge a practical foundation for moving configuration safely across environments while preserving history, versioning, and auditability.