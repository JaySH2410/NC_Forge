## 14\. Versioning Model

## 

Forge needs a versioning model because metadata and configuration are expected to evolve over time. Objects will be created, updated, published, promoted, and eventually superseded. Without a structured versioning approach, Forge would struggle to answer questions such as:

-   Which version of an application/configuration is currently active?
-   Which object revision belongs to which application release?
-   How do we distinguish an object that was updated under app version `1.0.0` vs `1.2.1`?
-   How should import and deployment reason about old vs new definitions?

The current Forge v1 direction uses **two related versioning levels**:

1.  **Application Version**
2.  **Object Version**

* * *

### 14.1 Why Forge Needs Two Versioning Levels

## 

Forge does not only version individual objects. It also needs to version the **configured application / admin configuration release context** in which those objects exist.

This is why Forge v1 separates:

#### Application Version

## 

Represents the version of the configured application / admin configuration release.

#### Object Version

## 

Represents the revision of a specific object within the context of an application version.

This allows Forge to answer both:

-   “What version of the application/configuration is this?”
-   “What revision of this object exists under that application version lineage?”

* * *

### 14.2 Application Version

## 

Application Version follows the format:

`major.minor.patch`

#### Examples

## 

-   `1.0.0`
-   `1.0.1`
-   `1.1.0`
-   `2.0.0`

#### Meaning

## 

Application Version represents the release version of the configured application / admin configuration package.

This is the version that should be associated with:

-   app-level release planning
-   deployment packages
-   promoted configuration versions
-   major/minor/patch evolution of the configured application

* * *

### 14.3 Object Version

## 

Object Version follows the conceptual format:

`app_major.app_minor.app_patch.object_revision`

#### Examples

## 

-   `1.0.0.1`
-   `1.0.0.2`
-   `1.0.2.1`
-   `1.1.0.1`

#### Meaning

## 

Object Version represents the revision of a specific object **within the context of an application version**.

The first three parts show the application version lineage under which the object was last updated, and the last part shows the object’s own revision count within that lineage.

* * *

### 14.4 Example Versioning Behavior

## 

The following example captures the currently discussed Forge versioning behavior.

#### Initial application version

## 

Application version = `1.0.0`

#### Object created

## 

Object version = `1.0.0.1`

#### Object updated again

## 

Object version = `1.0.0.2`

#### Object updated again

## 

Object version = `1.0.0.3`

#### Application patch update

## 

Application becomes `1.0.1`

If the object is **not changed**, its object version remains `1.0.0.3`

#### Application patch update again

## 

Application becomes `1.0.2`

If the object is now updated, object version becomes `1.0.2.1`

Notice what happened:

-   the object inherited the current application version
-   the object revision reset to `1`

#### Application minor update

## 

Application becomes `1.1.0`

If the object is not changed, object version remains `1.0.2.1`

If the object is later updated under `1.1.0`, object version becomes `1.1.0.1`

* * *

### 14.5 Core Versioning Rule

## 

The key Forge v1 versioning rule is:

> **An object version changes only when the object itself changes.**

Application version may change independently, but object version does **not** automatically change unless that object is updated.

#### This means

## 

-   app version can move from `1.0.0` to `1.0.1`
-   object version may still remain `1.0.0.3`
-   only when the object is updated again does it adopt the current app version lineage and start a new object revision sequence

* * *

### 14.6 Object Revision Reset Rule

## 

When an object is updated under a **new application version lineage**, its object revision resets.

#### Example

## 

Old object version = `1.0.0.3`

Application later becomes `1.0.2`

Object updated now becomes `1.0.2.1`

The object revision resets because it is now part of a new application version lineage.

* * *

### 14.7 Why This Versioning Model Was Preferred

## 

This model was preferred because it preserves both:

#### 1\. Application Release Context

## 

You can see which application release lineage an object revision belongs to.

#### 2\. Object-Level Change Tracking

## 

You can still distinguish multiple updates to the same object under the same application version lineage.

This makes object history easier to reason about than a single flat object revision counter.

* * *

### 14.8 Recommended Storage Model

## 

Although the display form of object version can be:

`1.0.2.3`

the recommended storage approach is to store object version in **two separate parts**:

#### A. ObjectAppVersion

## 

The application version under which the object was last updated.

Examples:

-   `1.0.0`
-   `1.0.2`
-   `1.1.0`

#### B. ObjectRevision

## 

The object’s revision number within that application version lineage.

Examples:

-   `1`
-   `2`
-   `3`

#### Display Version

## 

The combined display form can still be rendered as:

`ObjectAppVersion.ObjectRevision`

Example:

-   `1.0.2.3`

* * *

### 14.9 Why Separate Storage Is Better Than One Opaque String

## 

Storing version in separate parts makes it easier to:

-   query all objects updated under app version `1.2.0`
-   sort/filter by application version and object revision
-   implement import/merge/version checks
-   reason about object evolution without parsing a single opaque version string repeatedly

So Forge should conceptually expose:

-   `AppVersion`
-   `ObjectRevision`

while still displaying object version in the combined four-part format when useful.

* * *

### 14.10 Relationship Between Versioning and Lifecycle

## 

Version and lifecycle are related but separate:

#### Version

## 

Tells Forge **which revision** this is.

#### Lifecycle State

## 

Tells Forge **how that revision should be treated operationally**:

-   Draft
-   Published
-   Archived

#### Example

## 

An object may have:

-   version = `1.0.2.1`
-   lifecycle = `Draft`

Later:

-   version = `1.0.2.1`
-   lifecycle = `Published`

Later still:

-   version = `1.0.2.1`
-   lifecycle = `Archived`

So version identifies the revision; lifecycle identifies the state of that revision.

* * *

### 14.11 Relationship Between Versioning and Deployment

## 

Versioning is important for deployment and import because Forge needs to know:

-   whether an incoming object is newer than the target version
-   whether an import is re-importing the same version
-   whether an older object should be archived when a newer version is activated
-   which application version a deployment package represents

The deployment/import section later uses this versioning model as one of its foundations.

* * *

### 14.12 Forge v1 Versioning Summary

## 

Forge v1 uses the following versioning model:

#### Application Version

## 

Format:  
`major.minor.patch`

#### Object Version

## 

Display format:  
`app_major.app_minor.app_patch.object_revision`

#### Core rules

## 

1.  Application version can change independently of object version
2.  Object version changes only when the object changes
3.  When an object changes under a new app version lineage, its object revision resets
4.  Recommended storage is:
    -   `ObjectAppVersion`
    -   `ObjectRevision`

This should be treated as the working versioning model for Forge v1.