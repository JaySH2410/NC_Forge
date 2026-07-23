## 18\. Policy Model Details

## 

Forge needs a policy model because **role alone is not enough** for runtime authorization in a configurable platform.

A role can tell Forge _who the user broadly is_ — for example `HRManager`, `AssetViewer`, or `LeaveApprover` — but it cannot by itself express conditions such as:

-   this user can view only records from their own BusinessUnit
-   this user can edit only records they created
-   this user cannot delete Approved records
-   this user can access one object but not another, even with the same role

For that, Forge needs **Policy**.

The current Forge v1 direction is that policy is a **platform-level concept**, but its **first implementation focus is BusinessData**.

* * *

### 18.1 Purpose of Policy

## 

Policy is the mechanism Forge uses to express **conditional authorization rules**.

Policy should answer questions like:

-   Can this user perform this action on this business object?
-   Does this rule apply only to some rows and not others?
-   Does the user’s BusinessUnit / Department / Project matter?
-   Does record ownership matter?
-   Can a role be allowed one action but denied another under specific conditions?

So policy exists to add **runtime decision logic** on top of:

-   roles
-   permissions/actions
-   user scope
-   target business object

* * *

### 18.2 Forge v1 Scope of Policy

## 

The agreed direction is:

-   **Policy is defined as a common platform concept**
-   **but detailed policy enforcement in v1 is primarily for BusinessData**

So in Forge v1:

#### Metadata

## 

Policy may exist conceptually in future, but detailed policy enforcement is not the first focus.

#### AdminConfigData

## 

Policy may exist conceptually in future, but detailed policy enforcement is not the first focus.

#### BusinessData

## 

This is the **primary v1 target** of the policy model.

* * *

### 18.3 Policy Attachment Model

## 

The current agreed direction is:

> **Policy attaches to Role.**

That means a policy is not assigned directly to a user by default.  
Instead:

-   a **Role** has one or more policies
-   a **User** gets those policies through their assigned role(s)

This keeps the model scalable and avoids writing individual policy rules for every user.

* * *

### 18.4 Policy Target

## 

Although policy attaches to **Role**, the policy itself must still say **what it applies to**.

For Forge v1, the policy target should be a **specific BusinessData object/class**.

So conceptually, a policy says:

-   **Role X**
-   has policy
-   for **Business Object Y**
-   for **Action Z**
-   under **Condition C**
-   with **Effect E**

This is important because one role may need different rules for different business objects.

#### Example

## 

Role = `HRViewer`

Policies:

-   one policy for `Employee`
-   one policy for `LeaveRequest`
-   one policy for `Asset`

So policy is **role-attached**, but **object-scoped**.

* * *

### 18.5 Policy Granularity in Forge v1

## 

The current Forge v1 direction is:

-   **row-level restrictions** → yes
-   **action-level restrictions** → yes
-   **field-level restrictions** → not in v1
-   **column masking / property-level policy** → not in v1

So Forge v1 policy is mainly concerned with:

1.  **Which rows are accessible?**
2.  **Which actions are allowed on those rows?**

This keeps the first policy implementation focused and practical.

* * *

### 18.6 Policy Effect Model

## 

The current agreed effect model is:

1.  **Explicit Deny**
2.  **Explicit Allow**
3.  **Else deny by default**

So if Forge evaluates access and finds:

-   a matching **deny** rule → deny
-   otherwise a matching **allow** rule → allow
-   otherwise → deny

This gives Forge a predictable and security-safe baseline.

* * *

### 18.7 Policy Evaluation Precedence

## 

The current Forge v1 evaluation precedence is:

1.  **Explicit Deny**
2.  **Explicit Allow**
3.  **Default Deny**

This means deny has higher priority than allow.

#### Example

## 

If a user’s roles produce:

-   one rule that allows viewing Employee rows
-   another rule that denies viewing Employees in a certain Department

then the deny should win for those denied rows.

* * *

### 18.8 Policy Scope Dimensions

## 

Since Forge BusinessData is availability-aware and user-scope-aware, policy conditions may need to use business scope values such as:

-   **BusinessEntity**
-   **BusinessUnit**
-   **Department**
-   **Site**
-   **SubSite**
-   **Project**

This means a policy may say things like:

-   allow only if record.BusinessUnit = user.BusinessUnit
-   allow only if record.Project is in user’s allowed projects
-   deny if record.Department is outside user scope

So policy conditions should be able to reference **business scope dimensions** where needed.

* * *

### 18.9 Policy and UserContainerScope

## 

Policy should work together with **UserContainerScope**.

#### Role answers:

## 

“What kind of access persona does this user have?”

#### UserContainerScope answers:

## 

“In which business context does this user operate?”

#### Policy answers:

## 

“What can that role do to this target object/row under which conditions?”

This separation is useful because it prevents role definitions from having to hardcode every business scope rule directly.

* * *

### 18.10 Policy Actions

## 

Policy should evaluate access by **action**.

Forge v1 should at least support the common CRUD-style actions:

-   **View**
-   **Create**
-   **Edit**
-   **Delete**

Depending on the object type, future actions may also exist such as:

-   Approve
-   Publish
-   Import
-   Export
-   Assign
-   Submit

But for BusinessData v1, the core action model should at least cover the common record operations.

* * *

### 18.11 Row-Level Policy

## 

Row-level policy means a user may have access to **some records of an object, but not all**.

#### Example

## 

A user may:

-   view Employee records only in their own Department
-   edit LeaveRequest only if they are the creator
-   view Asset records only in their BusinessUnit

This is one of the most important parts of Forge policy because a configurable business platform rarely has “all rows for everyone” authorization.

* * *

### 18.12 Policy Condition Inputs

## 

To evaluate row-level and action-level access, Forge policy may need to use values from several places.

Common condition inputs may include:

#### From the current user

## 

-   user id
-   assigned roles
-   BusinessEntity / BusinessUnit / Department / Project / Site scope

#### From the target record

## 

-   creator / owner
-   business scope fields
-   lifecycle or business status
-   related object references

#### From the request/action context

## 

-   action being attempted (`View`, `Edit`, `Delete`, etc.)
-   current object/class being evaluated

Forge v1 does not need an overly complicated policy DSL immediately, but these are the kinds of inputs the policy engine should be able to reason about.

* * *

### 18.13 “AND only” Rule Combination for v1

## 

The current agreed simplification for Forge v1 is:

> **Within a policy rule, conditions are combined using AND only.**

That means if a policy has multiple conditions, all of them must be true for the rule to match.

#### Example

## 

Allow `View` on Employee where:

-   record.BusinessUnit = user.BusinessUnit
-   **AND**
-   record.Department = user.Department

This keeps v1 policy evaluation simpler than supporting full nested AND/OR logic from the beginning.

* * *

### 18.14 Suggested Policy Shape for Forge v1

## 

Conceptually, a Forge v1 policy can be thought of as containing the following parts:

-   **Role**
-   **Target Business Object/Class**
-   **Action**
-   **Effect** (`Allow` / `Deny`)
-   **Condition set**
-   **Lifecycle / active flag for the policy itself** if needed later

A simplified conceptual shape is:

-   Role = `HRViewer`
-   Target = `Employee`
-   Action = `View`
-   Effect = `Allow`
-   Conditions:
    -   record.BusinessUnit = user.BusinessUnit
    -   record.Department = user.Department

* * *

### 18.15 Example Policies

#### Example 1 — View within own Department

## 

Role = `HRViewer`  
Target = `Employee`  
Action = `View`  
Effect = `Allow`  
Conditions:

-   record.Department = user.Department

#### Example 2 — Edit only own records

## 

Role = `EmployeeSelfService`  
Target = `LeaveRequest`  
Action = `Edit`  
Effect = `Allow`  
Conditions:

-   record.CreatedBy = currentUser.Id

#### Example 3 — Deny delete for approved records

## 

Role = `LeaveEditor`  
Target = `LeaveRequest`  
Action = `Delete`  
Effect = `Deny`  
Conditions:

-   record.Status = `Approved`

These examples show why policy is needed beyond simple role membership.

* * *

### 18.16 Multiple Roles and Policy Evaluation

## 

A user may have multiple roles.  
That means multiple policies may apply during one access check.

Forge v1 should therefore evaluate the full set of relevant policies from the user’s assigned roles for the target object and action.

Then apply the precedence rule:

1.  **If any matching deny rule applies → deny**
2.  else if any matching allow rule applies → allow
3.  else → deny

This keeps policy evaluation deterministic even when multiple roles are assigned.

* * *

### 18.17 Policy and Availability

## 

Policy is not the same as Availability, but the two are related.

#### Availability

## 

Defines **where a thing is relevant or applicable** in business terms.

#### Policy

## 

Defines **whether a user can perform an action on that thing**.

Example:

-   an Employee record may be available in BusinessUnit A
-   but only HR managers in BusinessUnit A may edit it
-   another role may only view it
-   another user may not see it at all

So policy is one of the mechanisms that operationalizes access within the available business context.

* * *

### 18.18 Policy and Future Expansion

## 

The current Forge v1 model is intentionally focused. Future versions may later extend policy with things such as:

-   OR groups / nested logical expressions
-   field-level restrictions
-   masked fields / hidden properties
-   direct user policy overrides
-   policy templates
-   reusable policy groups
-   Metadata/AdminConfigData policy enforcement
-   approval-aware and workflow-aware policy conditions

These are useful future directions, but they are not required to lock the Forge v1 policy model.

* * *

### 18.19 Recommended Forge v1 Policy Model Summary

## 

Forge v1 should use the following policy direction:

1.  **Policy is a platform concept, but v1 enforcement focus is BusinessData**
2.  **Policy attaches to Role**
3.  **Each policy targets a specific BusinessData object/class**
4.  **Policy is action-aware**
5.  **Policy supports row-level restrictions**
6.  **Effect model is Explicit Deny > Explicit Allow > Default Deny**
7.  **Policy conditions may use business scope values**
8.  **Conditions are AND-only in v1**
9.  **Field-level policy is out of scope for v1**

* * *

### 18.20 Summary

## 

Forge v1 should treat policy as the **conditional authorization layer** on top of roles, user scope, and business objects.

The current direction is:

-   attach policy to **Role**
-   scope policy to a **target BusinessData object/class**
-   evaluate access by **action**
-   support **row-level rules**
-   use **Explicit Deny > Explicit Allow > Default Deny**
-   keep rule conditions **AND-only** for v1

This gives Forge a strong but manageable first version of policy-based access control for runtime business data.