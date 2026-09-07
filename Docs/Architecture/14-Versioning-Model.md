# 14. Versioning Model

Forge versions both configured applications and individual metadata objects. The version is stored as one canonical string on each row and is calculated by the server; API clients never submit a complete version.

## 14.1 Application versions

Application versions use:

`major.minor.patch`

New applications start at `0.0.1`. An application update must specify one `VersionIncrement`:

| Increment | Current | Result |
| --- | --- | --- |
| `Patch` | `1.2.3` | `1.2.4` |
| `Minor` | `1.2.3` | `1.3.0` |
| `Major` | `1.2.3` | `2.0.0` |

Minor increments reset patch to zero. Major increments reset minor and patch to zero. Any increment may be selected at any time; patch, minor, and major describe the scope and reset behavior rather than a mandatory sequence.

## 14.2 Object versions

Object versions use:

`app_major.app_minor.app_patch.object_major.object_minor`

The first three components record the application version under which the object was last changed. The final two components record that object's independent evolution.

New objects start at the owning application's current version followed by `1.0`. For example, an object created in application `0.0.1` receives `0.0.1.1.0`.

An object update must specify one `VersionIncrement`:

| Increment | Current object | Current application | Result |
| --- | --- | --- | --- |
| `Minor` | `1.0.0.4.5` | `1.2.3` | `1.2.3.4.6` |
| `Major` | `1.0.0.4.5` | `1.2.3` | `1.2.3.5.0` |

Object major increments reset object minor to zero. Objects do not have an object-level patch component; adding one would require a six-part format.

## 14.3 Application and object independence

Changing an application version does not rewrite its objects. An unchanged object retains the application prefix from the release in which that object was last changed.

When the object is changed later, it adopts the application's current three-part version and applies the requested increment to its existing object `major.minor` lineage. The object lineage does not reset merely because the application version changed.

Example:

1. Application `1.0.0`, object `1.0.0.2.4`.
2. Application advances to `1.1.0`; the unchanged object remains `1.0.0.2.4`.
3. An object minor update produces `1.1.0.2.5`.
4. A later object major update produces `1.1.0.3.0`.

## 14.4 Ownership, validation, and concurrency

- `Application.Version` remains one `NVARCHAR` column containing exactly three canonical non-negative integer components.
- `MetaObject.Version` remains one `NVARCHAR` column containing exactly five canonical non-negative integer components.
- Components have no signs, whitespace, labels, or leading zeroes except the value `0` itself.
- Create endpoints assign initial versions. Update endpoints accept a typed increment intent and calculate the next version from the persisted value.
- Object creation and update require an existing, active owning application.
- Version increments are checked for numeric overflow.
- Version columns are optimistic-concurrency tokens. A competing update returns a conflict and the caller must reload before retrying.
- Object writes hold a repeatable-read transaction while reading the owning application, ensuring the stored prefix corresponds to a consistent application version.

## 14.5 Lifecycle and history

Version and lifecycle remain separate concerns. Activating or deactivating an application or object does not increment its version.

Forge v1 updates the existing application or object row in place. Immutable historical rows and supersede behavior remain deferred work for the deployment/history design.

## 14.6 Deployment compatibility

The earlier object format used four components:

`app_major.app_minor.app_patch.object_revision`

SSDT post-deployment upgrades a canonical legacy value by appending object minor zero. For example, `0.0.1.1` becomes `0.0.1.1.0`. The conversion is idempotent, and deployment fails if stored application or object versions are not canonical numeric values in a recognized format.
