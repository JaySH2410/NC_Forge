## 8. Availability Model

Availability defines **where a thing is usable, visible, or applicable in business terms**.

It answers the question:

> **In which business context should this thing be available?**

Availability is different from placement:

- **Placement** answers where the thing lives structurally in Forge.
- **Availability** answers where the thing should be usable in the configured application or runtime business context.

Availability is especially important for **BusinessData**, but it is still useful to define it explicitly for all three layers so the platform has a consistent mental model.

---

## 8.1 Metadata Availability

### Availability
Metadata is **globally available**.

### Meaning
Metadata defines the platform’s structural language:
- classes
- interfaces
- properties
- relationships
- types
- graphs
- views

Since metadata acts as the common schema definition layer of the platform, it should not be restricted by business runtime boundaries such as:
- BusinessEntity
- BusinessUnit
- Department
- Site
- Project

### Why Metadata Availability Is Global
Metadata is meant to be reusable across applications and runtime business scenarios.  
It is a platform-level definition concern, not a runtime business partitioning concern.

### Forge v1 decision
Metadata availability is **global**.

---

## 8.2 AdminConfigData Availability

### Availability
AdminConfigData is **globally available** from a configuration ownership perspective.

### Meaning
AdminConfigData is stored and managed centrally as application configuration.  
It can define features, object usage, views, graphs, and business structures that later affect specific business areas, but the configuration itself is treated as a global configuration concern.

### Important distinction
Although AdminConfigData is globally available as a configuration layer, it may still **configure business functionality that is only intended for a specific business context**.

So global availability here means:
- centrally available to the configuration model
- not automatically runtime-available everywhere in business terms

### Example
A platform admin may configure an object or view globally in `AdminConfigs`, but the resulting runtime feature may only be intended for:
- one BusinessEntity
- one BusinessUnit
- one Department
- one Project

That business limitation belongs to **BusinessData availability / application runtime behavior**, not to the structural availability of AdminConfigData itself.

### Forge v1 decision
AdminConfigData availability is **global**.

---

## 8.3 BusinessData Availability

### Availability
BusinessData availability is **business-context driven**.

This is the most important availability model in Forge v1 because runtime business data is where organizational boundaries matter the most.

The currently discussed business availability dimensions are:

- **BusinessEntity**
- **BusinessUnit**
- **Department**
- **Site**
- **SubSite**
- **Project**

These dimensions represent the runtime business context in which a business record, feature, or configured data object may be available.

---

### 8.3.1 BusinessEntity

BusinessEntity represents the highest-level business grouping currently considered in the availability model.

It can be used when a business record or runtime feature should be available only within one top-level business entity.

#### Example
An employee or approval flow may belong only to one BusinessEntity.

---

### 8.3.2 BusinessUnit

BusinessUnit is the next important availability boundary for runtime data.

It is useful when a business object or process is relevant only within a particular business unit of the organization.

#### Example
A business object may be visible only to one BusinessUnit even though the overall platform supports multiple units.

---

### 8.3.3 Department

Department is a narrower organizational availability boundary.

It is useful when runtime records or configured functionality should only be applicable to one department.

#### Example
A department-specific approval configuration or department-specific business records may be available only inside that department.

---

### 8.3.4 Site

Site represents a physical or operational location-level availability boundary.

It is useful when runtime business data needs to be restricted or scoped to a particular site.

#### Example
Site-specific assets, employees, or operational records may be available only for one site.

---

### 8.3.5 SubSite

SubSite is a more granular location boundary below Site.

This is useful when one site contains smaller operational areas and the runtime data must distinguish between them.

#### Example
A plant area, floor, zone, or operational subsection can be modeled through SubSite if needed.

---

### 8.3.6 Project

Project represents a project-level availability boundary.

It is useful when runtime business data or configured application functionality should be available only within the context of a particular project.

#### Example
A project-specific onboarding, implementation, or operational feature may be available only to that project’s users and records.

---

## 8.4 Why Availability Is Most Important for BusinessData

Metadata and AdminConfigData are intentionally kept global in Forge v1.  
BusinessData is different because runtime business records naturally belong to specific organizational or operational contexts.

Without a business availability model, Forge would not be able to answer questions like:

- Is this record available only in one BusinessUnit?
- Should this feature work only in one Project?
- Can a user in one Site see records from another Site?
- Does this runtime object belong to one Department or many?

Availability gives Forge the business context layer needed to answer those questions.

---

## 8.5 Availability Is Not the Same as Access

A record being available in a business context does **not** automatically mean every user in that context can access it.

Example:
- A record may be available in BusinessUnit A
- but only Managers in BusinessUnit A may edit it
- another user in the same BusinessUnit may only view it
- another user may not see it at all due to policy

So:

- **Availability** = where the thing is relevant / active / applicable
- **Access** = who is allowed to perform actions on it

---

## 8.6 Availability Is Not the Same as Placement

A thing may be globally placed but only locally available.

### Example
- AdminConfigData for a feature may be stored globally in `AdminConfigs`
- but the feature may be intended only for:
  - one BusinessEntity
  - one BusinessUnit
  - one Project

So:
- placement = structural location in Forge
- availability = business runtime applicability

This distinction is especially important when configuration is centrally stored but used only in selected runtime contexts.

---

## 8.7 Availability and Runtime Business Context

Availability is effectively the **business context model** of Forge runtime.

When Forge v1 evaluates runtime business usage, the business context of a record or feature may be described using one or more of:

- BusinessEntity
- BusinessUnit
- Department
- Site
- SubSite
- Project

This context can later be used by:
- application logic
- filtering
- reporting
- policy evaluation
- deployment targeting
- environment-specific onboarding logic

---

## 8.8 Availability Model Summary Table

| Layer | Availability |
|---|---|
| Metadata | Global |
| AdminConfigData | Global |
| BusinessData | BusinessEntity / BusinessUnit / Department / Site / SubSite / Project |

---

## 8.9 Current Forge v1 Availability Direction

Forge v1 currently uses the following availability direction:

1. **Metadata**
   - globally available

2. **AdminConfigData**
   - globally available as platform/application configuration

3. **BusinessData**
   - available within business context boundaries defined by:
     - BusinessEntity
     - BusinessUnit
     - Department
     - Site
     - SubSite
     - Project

This should be treated as the working availability model for Forge v1.

---

## 8.10 Future Considerations

Future versions of Forge may choose to:
- make availability rules more explicit in configuration
- allow application-specific availability templates
- introduce availability inheritance or availability groups
- use availability as part of deployment targeting or onboarding packages

These are not required to lock the Forge v1 model, but the current availability design should leave room for them later.

---

## 8.11 Availability Model Summary

Forge v1 uses a simple but important availability model:

- **Metadata** → globally available
- **AdminConfigData** → globally available as configuration
- **BusinessData** → available within business runtime context defined by BusinessEntity / BusinessUnit / Department / Site / SubSite / Project

Availability should be treated as the **business applicability dimension** of scope in Forge.