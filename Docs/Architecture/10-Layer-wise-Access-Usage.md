## 10. Layer-wise Access Usage

The **common access model** described in the previous section is platform-wide, but it does not apply in exactly the same way to all three Forge layers.

Forge has three distinct layers:

1. **Metadata**
2. **AdminConfigData**
3. **BusinessData**

Each layer has a different purpose, different users, and different operational concerns.  
Because of that, the access model should be understood **layer-wise**.

This section explains how access is expected to apply to each layer in Forge v1.

---

## 10.1 Why Access Must Be Considered Layer-Wise

The same access concepts may exist across the platform, but the actual enforcement needs differ by layer.

### Example
- Metadata is a platform-definition concern
- AdminConfigData is an application-configuration concern
- BusinessData is an end-user runtime concern

It would be incorrect to assume that all three layers need the exact same access behavior from day one.

So Forge should distinguish between:
- **platform-level access concepts**
and
- **layer-specific access enforcement**

---

## 10.2 Metadata Access

Metadata belongs to **Layer 1 — Metadata** and represents the platform schema definition model.

Examples:
- Class definitions
- Interface definitions
- Property definitions
- Relationship definitions
- Graph definitions
- View definitions

### Nature of Metadata Access
Metadata is not runtime business data.  
It is platform-level structural definition, so access to metadata should be treated as a **platform administration / platform development concern** rather than a normal end-user authorization concern.

### Typical metadata actions that may eventually need access control
- create metadata definition
- update metadata definition
- publish metadata definition
- archive metadata definition
- view metadata definition
- import/export metadata definition

### Forge v1 position
Forge v1 does **not** focus its detailed policy model on Metadata.  
Metadata access can still exist conceptually through roles and permissions, but detailed row-level policy enforcement for Metadata is **not the primary v1 concern**.

### Practical interpretation for v1
Metadata access is expected to be controlled by higher-level platform roles such as:
- platform developer
- platform architect
- platform administrator

But the detailed runtime policy model defined later is primarily meant for BusinessData, not for Metadata.

---

## 10.3 AdminConfigData Access

AdminConfigData belongs to **Layer 2 — AdminConfigData** and represents the application configuration layer of Forge.

Examples:
- application configuration
- configured object usage
- view configuration
- graph configuration
- deployment-ready admin-side application setup

### Nature of AdminConfigData Access
AdminConfigData is not runtime business data, but it is more operational than Metadata because it is where application configuration actually happens.

This means access to AdminConfigData is expected to be an **administrative configuration concern** rather than an end-user business data concern.

### Typical AdminConfigData actions that may eventually need access control
- create application configuration
- update object configuration
- publish configuration package
- import configuration package
- archive configuration
- view configuration history

### Forge v1 position
Forge v1 does **not** make AdminConfigData the main target of the detailed policy model either.

Like Metadata, AdminConfigData can still use:
- roles
- permissions
- user assignments

but the row-level/action-level policy system discussed in the Policy Model section is primarily intended for **BusinessData**.

### Practical interpretation for v1
AdminConfigData access is expected to be governed by admin/configuration roles such as:
- PlatformAdmin
- Configurator
- ApplicationAdmin
- SolutionAdmin

Detailed AdminConfigData policy evaluation can be added later if needed, but it is not the first target of Forge v1 policy implementation.

---

## 10.4 BusinessData Access

BusinessData belongs to **Layer 3 — BusinessData** and represents the runtime business records used by end users.

Examples:
- Employee records
- Leave requests
- Asset records
- approvals
- operational project/site/business-unit records

### Nature of BusinessData Access
BusinessData access is the most important access problem in Forge v1 because it involves:
- runtime users
- business visibility
- business actions
- row-level restrictions
- organizational boundaries
- user context and business scope

BusinessData is where questions like the following must be answered:

- Can this user view this record?
- Can this user edit only their own records?
- Can this role delete records in this BusinessUnit?
- Can a user see only records from their Department or Project?
- Can approved records be edited by some users but not others?

Because of this, **BusinessData is the primary target of the Forge v1 policy model**.

---

## 10.5 BusinessData Access in Forge v1

Forge v1 should support policy-driven access for BusinessData using the common access model concepts.

### Key BusinessData access concepts in v1
- **Role**
- **UserRole**
- **UserContainerScope**
- **Policy**
- **target BusinessData object/class**
- **row-level conditions**
- **action-level control**

### BusinessData access should be able to answer:
- whether a user can view a given record
- whether a user can create a record in a given business context
- whether a user can edit a record based on status, ownership, or business scope
- whether a user can delete a record

---

## 10.6 BusinessData Access and Business Context

BusinessData access is tightly connected to **Availability** and **UserContainerScope**.

A user’s access to a business record may depend on:
- BusinessEntity
- BusinessUnit
- Department
- Site
- SubSite
- Project

Similarly, the record itself may carry those same business context values.

This is why BusinessData access in Forge cannot rely only on broad role names.  
It needs conditional policy evaluation using:
- current user values
- record values
- role-based rules
- action being attempted

---

## 10.7 BusinessData Access and Policy Enforcement

The policy model defined later in this document is specifically designed for BusinessData in Forge v1.

### Policy v1 scope
- applies to **BusinessData**
- attaches to **Role**
- targets a specific **BusinessData object/class**
- controls **row-level + action-level** access

### Policy can evaluate conditions using:
- user business context
- record business context
- record creator
- record status
- and similar runtime values

This makes BusinessData the first fully policy-governed layer in Forge.

---

## 10.8 Layer-wise Access Summary Table

| Layer | Nature of Access | v1 Enforcement Focus |
|---|---|---|
| Metadata | Platform schema administration access | High-level role/permission based; detailed policy not primary in v1 |
| AdminConfigData | Application configuration administration access | High-level role/permission based; detailed policy not primary in v1 |
| BusinessData | Runtime business record access | **Primary v1 focus**; role + policy based row/action control |

---

## 10.9 Access Maturity by Layer in Forge v1

Forge v1 should be understood as having different access maturity levels per layer.

### Metadata
- conceptually governed by access roles/permissions
- detailed row-level policy not required initially

### AdminConfigData
- conceptually governed by admin/configuration roles/permissions
- detailed row-level policy not required initially

### BusinessData
- fully targeted by the v1 policy model
- supports row-level and action-level policy evaluation

This is an intentional design decision to keep the first implementation focused on the runtime layer where access complexity is highest.

---

## 10.10 Future Direction

In future versions of Forge, the same common access model may be extended so that:
- Metadata can have more granular policy-based editing/publishing control
- AdminConfigData can have configuration-level approval and restriction rules
- deployment actions can have their own policy boundaries
- platform administration can be separated more precisely from application administration

These are valid future directions, but they are **not required to lock Forge v1**.

---

## 10.11 Layer-wise Access Usage Summary

Forge v1 uses one **common access model**, but it applies differently by layer:

1. **Metadata**
   - platform-level administrative access concern
   - detailed policy not the v1 focus

2. **AdminConfigData**
   - application configuration administrative access concern
   - detailed policy not the v1 focus

3. **BusinessData**
   - runtime business authorization concern
   - **primary v1 policy target**
   - row-level + action-level policy evaluation supported

This should be treated as the layer-wise access positioning for Forge v1.