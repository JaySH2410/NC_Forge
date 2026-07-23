## 9. Common Access Model

Forge needs a common access model so that authorization can be expressed consistently across the platform, even if enforcement differs by layer.

The access model is separate from:
- **Placement** → where something lives structurally
- **Availability** → where something is applicable in business terms

Access answers the question:

> **Who is allowed to do what with a given object, configuration, or business record?**

The common access model introduces the building blocks Forge can use for authorization across Metadata, AdminConfigData, and BusinessData, even though Forge v1 policy enforcement is focused primarily on **BusinessData**.

---

## 9.1 Why a Common Access Model Is Needed

Forge is a platform, not a single hardcoded application.  
That means access cannot be treated as an afterthought or handled separately for every feature.

A common access model is needed so Forge can consistently represent:
- who a user is
- which roles they have
- which business scopes they belong to
- what permissions/actions they can perform
- what policy-based restrictions apply to them

This is important because the same platform may later need to govern:
- metadata administration
- admin-side application configuration
- runtime business operations

Even if Forge v1 enforces policy mainly on BusinessData, the access model itself should be defined in a reusable platform-wide way.

---

## 9.2 Access Building Blocks in Forge

The current common access model introduces the following concepts:

1. **Role**
2. **Permission**
3. **UserRole**
4. **UserContainerScope**
5. **Policy**
6. **Policy Attachment**

These concepts together form the access foundation for Forge.

---

## 9.3 Role

Role represents a **named authorization identity** used to group access behavior.

A role answers the question:

> **What kind of access persona is this user acting as?**

Examples:
- `PlatformAdmin`
- `HRManager`
- `EmployeeViewer`
- `LeaveEditor`
- `AssetApprover`

### Purpose of Role
Role is the main grouping mechanism for access behavior in Forge.  
Instead of attaching every rule directly to every user, Forge can attach rules to roles and then assign roles to users.

### Role in Forge v1
Role is especially important because **Policy v1 attaches to Role**.

That means:
- a role can carry business data access rules
- users receive those rules through their assigned roles

---

## 9.4 Permission

Permission represents a **capability or action right** that may be granted within Forge.

Permissions are useful when access needs to be expressed in action-oriented terms such as:
- View
- Create
- Edit
- Delete
- Publish
- Configure
- Import
- Export

### Purpose of Permission
Permission gives Forge a vocabulary for “what kind of action is being controlled.”

### Important distinction
A Permission by itself may say *what kind of action exists*, but it does not automatically say:
- on which object it applies
- in which business scope it applies
- under which row-level conditions it applies

That is where Policy and business scope concepts become important.

---

## 9.5 UserRole

UserRole represents the assignment of a **Role to a User**.

This is the bridge between:
- a platform user
and
- the role-based access model

### Purpose of UserRole
UserRole allows Forge to answer:
- which roles does this user currently have?
- which policies should be evaluated for this user?
- which access behavior should be inherited from assigned roles?

### Example
If a user is assigned:
- `HRViewer`
- `LeaveEditor`

then Forge can evaluate policies attached to both of those roles when the user interacts with BusinessData.

---

## 9.6 UserContainerScope

UserContainerScope represents the **business context boundary of a user**.

This concept was introduced because role alone is not enough for runtime access.  
A user may have a role, but their access may still need to be limited to specific business contexts such as:
- one BusinessEntity
- one BusinessUnit
- one Department
- one Site
- one Project

### Purpose of UserContainerScope
UserContainerScope helps Forge understand the business scope in which a user operates.

This becomes useful for:
- filtering accessible runtime records
- policy evaluation
- narrowing business data visibility
- future multi-tenant / multi-project access boundaries

### Typical values that may participate in user scope
- BusinessEntity
- BusinessUnit
- Department
- Site
- SubSite
- Project

### Important note
UserContainerScope is conceptually different from Role:
- Role says **what kind of access persona the user has**
- UserContainerScope says **in which business context the user operates**

---

## 9.7 Policy

Policy is the **conditional access rule model** of Forge.

A policy does more than say “this role can edit.”  
It can express **under what conditions** that edit is allowed or denied.

Examples:
- user can view Employee only in same Department
- user can edit LeaveRequest only if they created it
- user cannot delete records in Approved status
- user can access records only in their own BusinessUnit and Project

### Purpose of Policy
Policy is the main mechanism for **row-level and action-level access control** in Forge v1.

Policy is needed because runtime access in a configurable platform usually cannot be represented by simple role names alone.

---

## 9.8 Policy Attachment

Policy Attachment defines **what the policy is attached to**.

In Forge v1, the locked direction is:

- Policy attaches to **Role**
- but each policy also specifies the **target BusinessData object/class** it applies to

This means a role can have different policies for different business objects.

### Example
Role: `Viewer`

Policies:
- one policy for `Employee`
- one policy for `LeaveRequest`
- one policy for `Asset`

So the policy is still role-based, but it is scoped to a specific target business object/class.

---

## 9.9 Relationship Between Access Concepts

The common access model concepts work together in the following way:

### Role
Defines an access persona or access grouping.

### Permission
Defines the kind of action or capability being controlled.

### UserRole
Assigns one or more roles to a user.

### UserContainerScope
Defines the business context boundary of the user.

### Policy
Defines the conditional access rules for a role against a target business object/class.

### Policy Attachment
Defines where the policy is attached and what it targets.

---

## 9.10 Example of the Access Model Working Together

Imagine a user has:

- Role = `HRViewer`
- UserContainerScope = `BusinessUnit A`, `Department HR`

And the `HRViewer` role has a policy for the `Employee` object:

- Action = `View`
- Effect = `Allow`
- Condition = `record.Department = currentUser.Department`

In this case:
- **Role** gives the user the access persona
- **UserRole** links the user to that role
- **UserContainerScope** provides business context values
- **Policy** checks whether the Employee record matches the user’s allowed context
- **Permission/Action** identifies what kind of operation is being evaluated

---

## 9.11 Access Model vs Policy Model

The **common access model** is broader than the **policy model**.

### Common access model
Defines the platform-wide building blocks:
- Role
- Permission
- UserRole
- UserContainerScope
- Policy
- PolicyAttachment

### Policy model
Defines the **specific evaluation rules** for conditional authorization, especially on BusinessData.

So:
- the access model is the **foundation**
- the policy model is the **runtime rule system built on top of it**

---

## 9.12 Forge v1 Positioning

Forge v1 should treat the common access model as a **platform-level access foundation**, even though detailed policy enforcement is initially focused on BusinessData.

### In Forge v1:
- Roles exist as the main authorization grouping
- Users can be assigned roles
- user business scope is represented separately from role
- policies attach to roles and target BusinessData classes
- policy evaluation controls row-level and action-level access in BusinessData

This makes the access model reusable for future expansion into Metadata and AdminConfigData access if needed later.

---

## 9.13 Common Access Model Summary Table

| Concept | Purpose |
|---|---|
| Role | Named access persona / grouping of authorization behavior |
| Permission | Action or capability being controlled |
| UserRole | Assignment of Role to User |
| UserContainerScope | Business context boundary of the user |
| Policy | Conditional rule controlling access |
| Policy Attachment | Defines where policy is attached and what target object it applies to |

---

## 9.14 Common Access Model Summary

Forge v1 introduces a common access model with six core concepts:

1. **Role**
2. **Permission**
3. **UserRole**
4. **UserContainerScope**
5. **Policy**
6. **Policy Attachment**

These concepts should be treated as the reusable access foundation of the platform, with the detailed policy behavior defined later in the Policy Model section.