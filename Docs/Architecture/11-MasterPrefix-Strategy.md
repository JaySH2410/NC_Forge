## 11. MasterPrefix Strategy

Forge is intended to support configuration movement across environments and, over time, may need to support scenarios where configurations from multiple projects, onboardings, or tenant contexts are brought together.

Because of that, Forge needs a way to reduce the risk of **identity collision** when independently created configurations use the same logical names.

The current design direction for this is **MasterPrefix**.

---

## 11.1 Purpose

MasterPrefix is a **configuration-level identity prefix** intended to help avoid naming and identity collisions across independently configured applications or onboarding packages.

It is especially relevant when:
- two different admin-side configurations create objects with the same logical business name
- multiple project/tenant configurations are merged into one environment
- imported configuration packages need a stable naming boundary beyond simple display names

### Example problem
Two separate onboarding/configuration efforts both create an object named:

- `Employee`

If those configurations are later imported into a common environment, relying only on display name or local naming may create ambiguity or collision.

MasterPrefix gives Forge a stable prefixing boundary so the platform can distinguish configuration identity more safely.

---

## 11.2 Where MasterPrefix Applies

The current direction is that **MasterPrefix applies primarily to AdminConfigData**.

### Why AdminConfigData
AdminConfigData is the layer where:
- application configuration is created
- deployable configuration packages are prepared
- configuration identity needs to survive movement across environments

MasterPrefix is therefore most useful as part of the **configuration packaging / promotion / merge story**, rather than as a general runtime business data field.

### Current Forge v1 position
MasterPrefix should be associated with **AdminConfigData ownership / application configuration identity**, not with normal runtime BusinessData records.

---

## 11.3 Why MasterPrefix Is Not Primarily a BusinessData Concept

BusinessData already represents runtime records such as:
- employees
- assets
- leave requests
- approvals

Those records may need business identifiers, but the MasterPrefix problem we discussed is different.  
It is not about identifying one employee record from another employee record; it is about distinguishing **configuration identity** when multiple independently configured applications or onboarding packages are brought together.

So MasterPrefix is not primarily meant to solve runtime business record uniqueness.  
It is meant to help with **configuration namespace and configuration collision control**.

---

## 11.4 Problem It Solves

MasterPrefix is meant to reduce collision risk in cases like:

### 1. Same object name created in different onboarding/configuration efforts
Example:
- Project A config defines `Employee`
- Project B config also defines `Employee`

### 2. Multiple application packages imported into one target environment
Example:
- HR application package
- Asset application package
- another project-specific package

### 3. Future consolidation / merge scenarios
Example:
- two previously separate configuration sets are merged into one platform environment

In such cases, MasterPrefix can help preserve configuration identity boundaries even if user-facing names overlap.

---

## 11.5 Suggested Mental Model

MasterPrefix should be treated as a **configuration namespace marker**.

It is not primarily a display label.  
It is a platform-level identifier component that can help Forge reason about:

- where a configured object came from
- which onboarding/configuration context owns it
- how to distinguish same-named configured objects across imported packages

---

## 11.6 MasterPrefix and Application Composition

MasterPrefix becomes especially relevant when Forge supports:

- multiple configured applications
- shared objects
- extension objects
- package import / promotion
- cross-project onboarding into a common target environment

In those scenarios, Forge may have:
- shared platform-level concepts
- app-specific configuration
- onboarding-specific configuration
- environment-level imported packages

MasterPrefix provides a way to reduce ambiguity in those configuration identities.

---

## 11.7 Current Design Question We Discussed

One question discussed was:

> Should MasterPrefix be defined once per AdminConfig container / project / tenant, or once per application / app onboarding?

The design concern here is that Forge may have:
- shared objects across applications
- multiple applications within one onboarding
- future tenant/project consolidation

So the placement of MasterPrefix needs to reflect the level at which configuration identity should be separated.

---

## 11.8 Current Recommended Direction

The current recommended direction is:

### MasterPrefix should be defined at the **AdminConfig package / onboarding / configuration ownership boundary**, not per individual business object.

In practice, that means it should align with the **configuration source boundary** rather than the runtime object boundary.

This gives Forge a more stable way to reason about:
- imported configuration origin
- merge safety
- object naming collision prevention
- package-level configuration identity

---

## 11.9 Why Not Put MasterPrefix on Every Business Object Manually

Putting MasterPrefix on every business object as a business-level concern would create unnecessary coupling between:
- configuration identity management
and
- normal business object semantics

It would also make runtime business modeling noisier than needed.

Instead, Forge should treat MasterPrefix as a **configuration-level namespace / ownership concept** that influences imported/configured object identity where required.

---

## 11.10 Relationship to UID and Name

Forge is already expected to have identifiers such as:
- `Id`
- `Uid`
- `Name`

MasterPrefix does not replace those.

### Broad interpretation
- **Id** → database/internal identity
- **Uid** → stable platform identity for the object/configuration record
- **Name** → human-readable name
- **MasterPrefix** → configuration namespace / ownership boundary marker used to reduce collision risk across imported or independently configured application sets

So MasterPrefix should be understood as complementary to normal identity fields, not a replacement for them.

---

## 11.11 Collision Handling Intention

MasterPrefix is intended to help in collision handling scenarios such as:
- same object name, different onboarding source
- imported package containing a configured object whose display name already exists
- future merge/import logic where configuration origin matters

It should not be the only collision-handling mechanism.  
Forge will still need:
- stable UIDs
- import history
- merge rules
- package-level validation

But MasterPrefix provides one additional identity dimension that makes those processes safer.

---

## 11.12 Forge v1 Positioning

For Forge v1, MasterPrefix should be positioned as:

- an **AdminConfigData-oriented configuration identity concept**
- used to help separate independently created configuration spaces
- relevant to import / package / merge / onboarding scenarios
- not a replacement for business record identity
- not primarily a BusinessData modeling concept

---

## 11.13 Open Decision Still Implicit

The exact administrative ownership level of MasterPrefix still needs to be finalized in implementation terms.

The most likely options are:

1. **per AdminConfig package / onboarding / configuration source**
2. **per configured application**

The current discussion direction leans more toward **configuration source / onboarding boundary**, because it better supports import and merge reasoning.

This should be locked explicitly when the package/import model is implemented.

---

## 11.14 MasterPrefix Strategy Summary

Forge v1 introduces **MasterPrefix** as a configuration identity aid for AdminConfigData.

It exists to help with:
- object/configuration naming collision control
- import and merge safety
- onboarding/package identity boundaries
- distinguishing independently configured objects with overlapping names

Current positioning:
- primarily applies to **AdminConfigData**
- tied to configuration ownership / onboarding / package identity
- useful in package promotion and merge scenarios
- complementary to `Id`, `Uid`, and `Name`