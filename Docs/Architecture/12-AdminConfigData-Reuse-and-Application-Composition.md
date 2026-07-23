## 12. AdminConfigData Reuse and Application Composition

One of the most important design goals of Forge is that applications should be configurable without repeatedly recreating the same business concepts in multiple places.

That immediately raises an application-composition question:

> If multiple configured applications need the same business concept, how should Forge model and reuse it?

This section captures the current Forge v1 direction for **shared object reuse**, **application composition**, and **application-specific extension** in the **AdminConfigData** layer.

---

## 12.1 Problem Statement

Consider a common business concept such as **Employee**.

Multiple configured applications may need Employee information:

- HR application
- Asset application
- Leave application
- Travel application
- Access management application

If each application creates its own separate Employee object/configuration, the platform will end up with duplicated business concepts and fragmented data ownership.

The problem becomes:

- how to avoid duplicate shared objects
- how to let multiple applications consume the same object
- how to allow one application to store additional application-specific fields without polluting the shared core model
- how to preserve clean ownership and deployment boundaries

This is the main application composition problem discussed for Forge.

---

## 12.2 Core Design Principle

The current Forge direction is:

> **Common business concepts should be modeled once and reused across applications wherever possible.**

Forge should avoid creating duplicate configured objects for the same shared business concept if those objects represent the same real-world entity.

### Example
If HR and Asset both need Employee, the preferred direction is:

- **one shared Employee object**
- multiple applications consume that shared object
- application-specific extra information should be modeled separately if needed

This keeps data more centralized and avoids unnecessary duplication of business meaning.

---

## 12.3 Shared Object Reuse

Forge should support the idea of a **shared object** that can be used by multiple configured applications.

### Example
A shared `Employee` object may be used by:

- HR application
- Asset application
- Leave application

Each of those applications may use Employee differently, but the platform should not automatically create three separate Employee objects if the business concept is the same.

### Why this matters
Shared object reuse improves:

- conceptual consistency
- data centralization
- cross-application interoperability
- reporting across applications
- long-term maintainability

---

## 12.4 Owning Application vs Shared Consumption

Even if an object is shared, Forge still needs to answer:

> Which application owns the configuration of that object?

This is important because one application may be the **primary owner** of a shared business concept, while other applications only consume it.

### Example
- HR application may own the core Employee object definition
- Asset application may consume Employee for assignment/use cases
- Leave application may consume Employee for leave requests

In this model:

- one application acts as the **owning application**
- other applications act as **consumers of the shared object**

This gives Forge a cleaner model than “every application owns its own duplicate Employee”.

---

## 12.5 Shared Consumption Does Not Mean Global Duplication

A consuming application should not need to recreate the shared object definition.

Instead, it should reference and use the shared object from the owning application or shared configuration space.

That means:
- the object identity remains stable
- the business meaning remains centralized
- multiple apps can use it without duplicating the core object definition

---

## 12.6 Application-Specific Extra Data Problem

Although a shared object is desirable, applications may still need **application-specific extra data**.

### Example
A shared Employee object may contain core employee data such as:
- employee code
- name
- department
- business unit

But an Asset application may also need additional Employee-related data such as:
- asset assignment policy preference
- equipment eligibility
- asset cost center mapping

Similarly, a Leave application may need:
- leave approval profile
- leave entitlement override flags

The question becomes:

> Where should this application-specific extra data live if the core Employee object is shared?

---

## 12.7 Recommended Direction — Extension Object / Additional Data Approach

The current Forge direction is:

> Keep the shared object centralized, and store application-specific additional data separately rather than duplicating the shared object.

This means:
- the shared core object remains one object
- each application can define additional configuration/data around it if needed
- app-specific fields do not force duplication of the shared business concept

### Example
Instead of creating:
- `Employee_HR`
- `Employee_Asset`
- `Employee_Leave`

Forge should prefer:

- shared `Employee`
- plus application-specific extension/configuration for Asset or Leave if needed

---

## 12.8 Possible Extension Pattern

A likely pattern for Forge is:

### Shared Core Object
Example:
- `Employee`

### Application-Specific Extension Object or Additional Interface Usage
Example:
- Asset-specific employee extension/configuration
- Leave-specific employee extension/configuration

The exact implementation can vary, but the design intent is:

- **shared core stays centralized**
- **application-specific data stays separate**
- **applications compose on top of shared business concepts**

---

## 12.9 Why This Direction Was Preferred

This direction was preferred because if HR and Asset both need the same employees, creating two separate Employee objects would introduce unnecessary duplication.

### Problems with duplication
- duplicated identity for the same business entity
- synchronization challenges
- inconsistent reporting
- harder cross-application use
- unclear ownership
- more complicated migration and deployment

By contrast, a shared object with extension/app-specific data gives a cleaner platform model.

---

## 12.10 Interaction with BusinessData

This discussion has implications for **BusinessData** as well.

If the shared object is centralized at the configuration level, then runtime business records can also remain conceptually centralized instead of being split into multiple duplicated application-specific objects.

Application-specific runtime information can then be stored through:
- related extension records
- app-specific linked records
- app-specific object extensions
- additional app-scoped data structures

This is preferable to duplicating the main business entity everywhere.

---

## 12.11 Relationship to AdminConfigData

The application composition problem is primarily discussed under **AdminConfigData** because this is the layer where configured application structure is defined.

AdminConfigData needs to support:
- shared object definition or reference
- owning application designation
- application-specific extension patterns
- clean packaging and promotion of configured applications

So even though the business motivation comes from runtime usage, the design responsibility sits largely in the **configuration layer**.

---

## 12.12 Current Forge v1 Direction

The current design direction for Forge v1 can be summarized as follows:

### 1. Common business concepts should be shared
If two applications refer to the same real-world business entity, Forge should prefer one shared configured object rather than duplicates.

### 2. One application or shared configuration context may act as the owner
The object may still have an ownership boundary for configuration and lifecycle purposes.

### 3. Other applications consume the shared object
Consuming applications should reference and use the shared object instead of recreating it.

### 4. Application-specific additional data should be modeled separately
If one application needs extra fields or behavior, that should be handled through extension/configuration patterns rather than duplicating the shared core object.

---

## 12.13 What Is Not Fully Locked Yet

The exact implementation pattern is **not fully finalized** yet.

The following still need final implementation-level decisions:

### A. Owning Application Model
How exactly is “ownership” of a shared object represented in AdminConfigData?

### B. Shared Object Reference Model
How does one configured application reference an object owned by another application or shared configuration space?

### C. Extension Model
Should app-specific extra data be modeled through:
- extension objects
- additional interfaces
- related objects
- a hybrid pattern

### D. Deployment Impact
How should shared objects and consuming applications be packaged together during export/import?

These are design details that still need implementation refinement, but the high-level direction is already clear.

---

## 12.14 Forge v1 Positioning

For Forge v1, the application composition direction should be understood as:

- **do not duplicate shared business concepts unnecessarily**
- **allow shared configured objects to be used by multiple applications**
- **preserve some ownership boundary for lifecycle and configuration control**
- **handle app-specific additional data through extension patterns rather than object duplication**

This is the working application composition model for Forge v1.

---

## 12.15 Summary

Forge v1 should treat AdminConfigData reuse and application composition using the following principles:

1. Shared business concepts should be modeled once wherever possible
2. Multiple configured applications should be able to consume the same shared object
3. One application or shared config boundary may act as the owner of the shared object
4. Application-specific extra fields or behavior should be handled through extension/additional-data patterns
5. Duplicate copies of the same business concept across applications should be avoided unless there is a strong reason to separate them