## 7. Placement Model

Placement defines **where a thing belongs structurally inside Forge**.

It answers the question:

> **Which platform container / layer owns this item?**

Placement is **not** about who can access the item, and it is **not** about where it is business-usable.  
It is only about **structural ownership and storage location** within the platform.

Forge v1 currently uses placement to distinguish the three major layer containers:

1. **Metadata**
2. **AdminConfigData**
3. **BusinessData**

---

## 7.1 Metadata Placement

### Placement
Metadata is placed in a **global container**.

### Container Name
`MetaSchemas`

### Meaning
All metadata concepts that define the Forge platform schema model should belong to the global metadata placement area.

This includes concepts such as:
- Class
- Interface
- Property
- Type
- Relationship
- Graph
- View
- supporting metadata contracts and metadata-level abstractions

### Why Metadata Placement Is Global
Metadata defines the **platform language and structural model**.  
It is not tenant-specific runtime data and is not meant to be scattered across many business-owned containers.

Keeping metadata globally placed helps ensure:
- platform consistency
- reuse across configured applications
- stable metadata identity
- simpler metadata versioning and promotion behavior

### Forge v1 decision
Metadata placement is **global** under the `MetaSchemas` container.

---

## 7.2 AdminConfigData Placement

### Placement
AdminConfigData is placed in a **global container**.

### Container Name
`AdminConfigs`

### Meaning
AdminConfigData represents the application configuration layer of Forge.  
Although it configures business applications and may later be promoted to multiple environments, it is still considered a platform-level configuration concern rather than runtime business data.

This includes things such as:
- application configuration
- object configuration for applications
- configured usage of classes/interfaces/properties
- graph/view configuration
- application-level admin-side feature setup
- deployment-ready configuration structures

### Why AdminConfigData Placement Is Global
The current design direction is that configuration should be centrally managed by the platform/configuration layer rather than structurally duplicated under business-owned runtime containers.

This allows:
- consistent promotion across environments
- controlled platform administration
- reuse of configuration across runtime contexts
- a clean separation between application configuration and runtime business data

### Important note
Although AdminConfigData is globally placed, the configured application behavior can still later influence **BusinessData availability and access**.  
So global placement does **not** mean universal runtime visibility; it only means central structural ownership.

### Forge v1 decision
AdminConfigData placement is **global** under the `AdminConfigs` container.

---

## 7.3 BusinessData Placement

### Placement
BusinessData is placed in the **BusinessData layer container**.

### Container Name
`BusinessData`

### Meaning
BusinessData contains the runtime records used by end users.  
This is the operational data layer of Forge and is distinct from metadata and admin configuration.

Examples of BusinessData include:
- employee records
- asset records
- leave requests
- project records
- approvals
- runtime object values configured through the platform

### Why BusinessData Placement Is Separate
BusinessData has fundamentally different characteristics from Metadata and AdminConfigData:
- it is runtime data
- it is high-volume
- it changes frequently
- it is used by end users
- it is subject to business availability and access rules

Because of this, it should have its own placement area rather than sharing structural placement with metadata or admin configuration.

### Forge v1 decision
BusinessData placement is under the `BusinessData` container.

---

## 7.4 Placement Summary

Forge v1 currently uses the following placement model:

| Layer | Placement | Container Name |
|---|---|---|
| Metadata | Global | `MetaSchemas` |
| AdminConfigData | Global | `AdminConfigs` |
| BusinessData | Layer-specific business runtime placement | `BusinessData` |

---

## 7.5 Why Placement Is Kept Simple in Forge v1

Forge v1 intentionally keeps placement simple.

### Metadata
Always globally placed.

### AdminConfigData
Always globally placed.

### BusinessData
Placed in the runtime business data layer.

This simplicity is useful because Forge already has other complexity areas such as:
- interface-first modeling
- object versioning
- package deployment
- import/merge behavior
- policy-based access

Keeping placement simple in v1 helps avoid unnecessary structural complexity too early.

---

## 7.6 Placement vs Availability

It is important to separate **placement** from **availability**.

### Example
A business object configuration may be:
- **placed** in global `AdminConfigs`
- but only **available** for one BusinessEntity / BusinessUnit / Department / Project at runtime

So:
- placement = where the configuration lives structurally
- availability = where the configured functionality is usable

This distinction is especially important for BusinessData and for application configuration that drives BusinessData behavior.

---

## 7.7 Placement vs Access

Placement also must be kept separate from **access**.

Example:
- a metadata object may be placed globally
- but only some platform users may be allowed to edit it
- a business record may be placed in `BusinessData`
- but access to that record may be restricted by role and policy

So placement should not be treated as an authorization concept.

---

## 7.8 Placement Boundaries in Forge v1

The placement model in Forge v1 establishes the following boundaries:

### Metadata Boundary
Platform schema and structural definitions live in `MetaSchemas`.

### AdminConfig Boundary
Application and configuration definitions live in `AdminConfigs`.

### Business Runtime Boundary
End-user operational records live in `BusinessData`.

These boundaries are important because they create a clean structural separation between:
- what the platform **is**
- how the platform is **configured**
- what business users actually **do with it at runtime**

---

## 7.9 Future Considerations

Forge v1 keeps placement intentionally simple, but future versions may choose to introduce additional placement concepts such as:
- more explicit application-level placement containers
- tenant/project-specific configuration containers
- packaging-specific structural grouping
- archived or historical placement partitions

These are not required for the current Forge v1 model and should only be introduced if there is a clear platform need.

---

## 7.10 Placement Model Summary

Forge v1 uses a simple structural placement model:

1. **Metadata** → Global → `MetaSchemas`
2. **AdminConfigData** → Global → `AdminConfigs`
3. **BusinessData** → Runtime business layer → `BusinessData`

This model should be treated as the structural placement baseline for the platform.