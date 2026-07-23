# Forge Platform Design v1

## 1. Forge Platform Vision

### 1.1 Purpose
<!-- What Forge is and what problem it solves -->

### 1.2 Personas and Workflow
<!--
Person 1 — Core Developer
Person 2 — Platform Admin / Configurator
Person 3 — End User
-->

### 1.3 Core Goal
<!--
New end-user features should be built by configuration/data in platform DB,
not by writing new backend feature code each time.
-->

---

## 2. Forge Platform Layers

### 2.1 Layer 1 — Metadata
<!-- Purpose, ownership, examples -->

### 2.2 Layer 2 — AdminConfigData
<!-- Purpose, ownership, examples -->

### 2.3 Layer 3 — BusinessData
<!-- Purpose, ownership, examples -->

---

## 3. Core Forge Modeling Principles

### 3.1 Metadata-driven platform
### 3.2 Separation of Metadata, AdminConfigData, and BusinessData
### 3.3 Reusability of configured objects
### 3.4 Interface-first modeling
### 3.5 Definition vs Runtime separation

---

## 4. Locked Forge v1 Modeling Rules

### 4.1 Properties belong only to Interfaces
### 4.2 Relationships exist only between Interfaces
### 4.3 Every Class has:
- exactly 1 Primary Interface
- exactly 1 IObject Interface
- optional additional Interfaces

### 4.4 Class may implement additional Interfaces
### 4.5 Interface inheritance is not supported in Forge v1
### 4.6 Graph root is Class and traversal happens through Interfaces
### 4.7 Forge v1 Property Types
- Primitive
- Enum
- Interface Reference

---

## 5. Core Metadata Concepts

### 5.1 Object
### 5.2 Class
### 5.3 Interface
### 5.4 Property
### 5.5 Type
### 5.6 Relationship
### 5.7 Graph
### 5.8 View

---

## 6. Scope Model Overview

### 6.1 Placement
### 6.2 Availability
### 6.3 Access

---

## 7. Placement Model

### 7.1 Metadata Placement
- Global
- Container Name: `MetaSchemas`

### 7.2 AdminConfigData Placement
- Global
- Container Name: `AdminConfigs`

### 7.3 BusinessData Placement
- Global
- Container Name: `BusinessData`

---

## 8. Availability Model

### 8.1 Metadata Availability
- Global

### 8.2 AdminConfigData Availability
- Global

### 8.3 BusinessData Availability
- BusinessEntity
- BusinessUnit
- Department
- Site
- SubSite
- Project

---

## 9. Common Access Model

### 9.1 Role
### 9.2 Permission
### 9.3 UserRole
### 9.4 UserContainerScope
### 9.5 Policy
### 9.6 Policy Attachment

---

## 10. Layer-wise Access Usage

### 10.1 Metadata Access
<!-- How common access concepts apply to Layer 1 -->

### 10.2 AdminConfigData Access
<!-- How common access concepts apply to Layer 2 -->

### 10.3 BusinessData Access
<!-- How common access concepts apply to Layer 3 -->

---

## 11. MasterPrefix Strategy

### 11.1 Purpose
### 11.2 Where it applies
### 11.3 Project / Tenant / Onboarding boundary discussion
### 11.4 Collision handling intention

---

## 12. AdminConfigData Reuse and Application Composition

### 12.1 Problem Statement
<!-- Shared objects like Employee across HR / Asset / Leave -->

### 12.2 Shared Object Reuse
### 12.3 Owning Application vs Shared Consumption
### 12.4 Extension Object / Additional Data approach
### 12.5 Current Direction / Open Decision

---

## 13. Lifecycle States

### 13.1 Draft
### 13.2 Published
### 13.3 Archived

---

## 14. Versioning Model

### 14.1 Application Version
- `major.minor.patch`

### 14.2 Object Version
- `app_major.app_minor.app_patch.object_revision`

### 14.3 Versioning Rules
<!--
When app version changes but object not updated
When object updated after app version changes
Resetting object revision
-->

### 14.4 Suggested Storage Model
<!--
Store AppVersion + ObjectRevision separately
Display combined version
-->

---

## 15. Object Evolution and Data Compatibility

### 15.1 Problem Statement
<!-- Existing data when object gains new interfaces/properties -->

### 15.2 Additive Changes
<!-- New optional interface/property -->

### 15.3 Mandatory New Properties
<!-- Default / backfill / migration rule -->

### 15.4 Compatibility Rules

---

## 16. Deployment Unit and Promotion Model
<!-- Pending discussion -->

### 16.1 What gets exported/imported/promoted
### 16.2 Application vs Object vs Package deployment unit
### 16.3 Promotion across environments

---

## 17. Merge / Import Conflict Handling
<!-- Pending discussion -->

### 17.1 Existing object collision
### 17.2 Version conflict
### 17.3 MasterPrefix conflict handling
### 17.4 Merge / overwrite / skip rules

---

## 18. Policy Model Details
<!-- To be discussed later -->

### 18.1 Policy scope
### 18.2 Row-level vs field-level restrictions
### 18.3 Policy evaluation logic
### 18.4 RolePolicy vs UserPolicy

---

## 19. Open Questions / Pending Decisions

### 19.1 Deployment model
### 19.2 Merge strategy
### 19.3 Detailed policy model
### 19.4 Application composition finalization

---

## 20. Appendix

### 20.1 Terminology
<!-- Metadata, AdminConfigData, BusinessData, Application, Interface, etc. -->

### 20.2 Example Scenarios
<!-- HR + Asset + Leave shared Employee example -->

### 20.3 Future Enhancements
<!-- Collections, interface inheritance revisit, advanced packaging, etc. -->