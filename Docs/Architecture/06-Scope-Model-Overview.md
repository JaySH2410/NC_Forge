## 6. Scope Model Overview

Scope in Forge is not a single concept.  
During discussion, it became clear that “scope” can mean different things depending on what exactly we are trying to control.

To keep Forge understandable and extensible, scope is divided into **three separate dimensions**:

1. **Placement**
2. **Availability**
3. **Access**

These three together define:
- where something belongs
- where it is visible or usable
- who is allowed to do what with it

---

### 6.1 Why Scope Needs to Be Split

If Forge used one generic “scope” concept for everything, it would mix multiple concerns that should remain separate.

For example, these are different questions:

- In which container does this object/configuration belong?
- In which business context is this object/data available?
- Which user/role is allowed to view or edit it?

All of these are “scope-like” questions, but they are **not the same problem**.

So Forge separates them into:
- **Placement** → where the thing lives
- **Availability** → where the thing is visible / applicable
- **Access** → who can do what with it

---

## 6.2 Placement

Placement answers the question:

> **Where does this thing belong structurally inside Forge?**

Placement is about **ownership / storage / structural location**.

It does **not** answer who can access the thing.  
It also does **not** answer where business users can use it.  
It only answers **where the platform considers this item to live**.

### Examples of placement questions
- Does this metadata definition belong to the global metadata container?
- Does this admin configuration belong to the global admin config container?
- Does this business data record belong to the business data layer?

### Typical placement concerns
- container ownership
- layer ownership
- structural location in platform storage
- environment promotion grouping
- project / tenant / onboarding boundary for configuration ownership

---

## 6.3 Availability

Availability answers the question:

> **In which business context is this thing available or usable?**

Availability is about **functional applicability / visibility of usage**, not structural ownership.

Something may be placed globally but only be available in a narrower business context.

### Example
An object or configuration may be structurally defined once, but only be relevant to:
- one BusinessEntity
- one BusinessUnit
- one Department
- one Site
- one Project

### Examples of availability questions
- Is this business data object available only in one BusinessUnit?
- Is this configuration intended for one Project or multiple Projects?
- Is this runtime data visible in one Site but not another?

### Typical availability concerns
- business context applicability
- tenant/business partition visibility
- where configured functionality should be active
- organizational boundaries of use

---

## 6.4 Access

Access answers the question:

> **Who is allowed to view, create, edit, delete, or otherwise operate on this thing?**

Access is about **authorization and security**, not structural ownership and not business placement by itself.

Access is influenced by:
- user
- role
- policy
- business scope of the user
- business scope of the record being accessed

### Examples of access questions
- Can this user edit Employee records in this BusinessUnit?
- Can this role delete LeaveRequests in this Department?
- Can this user view only records created by them?
- Can this user see data for Project A but not Project B?

### Typical access concerns
- authorization
- policy evaluation
- row-level filtering
- action-level permissions
- role-based restrictions

---

## 6.5 Relationship Between Placement, Availability, and Access

These three concepts are related, but they must remain distinct.

### Placement
Defines **where the thing lives** in Forge.

### Availability
Defines **where the thing is functionally applicable / visible**.

### Access
Defines **who is allowed to do what with it**.

---

### 6.5.1 Example to illustrate the difference

Consider an Employee-related object/configuration.

#### Placement
The object configuration may live in:
- Metadata → global metadata container
- AdminConfigData → global admin configuration container
- BusinessData → business data layer

#### Availability
The resulting business functionality may only be available for:
- one BusinessEntity
- one BusinessUnit
- one Department

#### Access
Even within that availability boundary:
- HR Manager may edit
- Employee may only view own record
- another user may have no access

So:
- **placement** says where it belongs structurally
- **availability** says where it is active/usable
- **access** says who can do what with it

---

## 6.6 Scope Dimensions by Layer

The scope model applies differently across the three layers.

### Metadata
- Placement is global
- Availability is global
- Access may later be controlled for metadata editing / publishing, but this is not the v1 policy focus

### AdminConfigData
- Placement is global
- Availability is generally global from a configuration ownership point of view, though it can configure business-layer availability
- Access for admin/configuration actions can be added later

### BusinessData
- Placement is the business data layer
- Availability is the most important here because runtime data is associated with business structures such as BusinessEntity / BusinessUnit / Department / Site / Project
- Access is critical here and is part of the v1 policy model

---

## 6.7 Scope Model Intent for Forge v1

Forge v1 should use this 3-part scope model as a foundational concept:

1. **Placement**
   - structural location / ownership inside the platform

2. **Availability**
   - where the thing is applicable in business terms

3. **Access**
   - who can do what with it

This gives Forge a cleaner mental model and avoids overloading one “scope” term with too many responsibilities.

---

## 6.8 Scope Model Summary Table

| Scope Dimension | Main Question | Typical Concern |
|---|---|---|
| Placement | Where does this thing belong structurally in Forge? | container ownership, layer ownership, storage boundary |
| Availability | In which business context is this thing usable / visible? | business entity, BU, department, site, project applicability |
| Access | Who is allowed to do what with this thing? | authorization, policy, row/action restrictions |

---

## 6.9 Forge v1 Scope Positioning

For Forge v1, the scope model should be understood as:

- **Placement** = structural platform location
- **Availability** = business context of use
- **Access** = authorization and operational permission

The next sections define each of these more concretely, starting with **Placement** and **Availability**, and later connecting **Access** to the common access model and policy model.