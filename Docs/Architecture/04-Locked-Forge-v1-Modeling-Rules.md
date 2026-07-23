## 4. Locked Forge v1 Modeling Rules

This section captures the **modeling rules that are currently considered locked for Forge v1** based on our design discussions.  
These rules define how Classes, Interfaces, Properties, Relationships, Graphs, and Types should behave in the first version of the platform.

The goal of these rules is to keep Forge v1:
- structurally consistent
- easier to implement
- easier to reason about
- safer for future evolution

---

### 4.1 Properties Belong Only to Interfaces

In Forge v1, **properties belong only to Interfaces**.

A Class does **not** directly own properties.  
Instead, a Class exposes properties through the Interfaces it implements.

#### Meaning
If a business object needs fields such as:
- Name
- EmployeeCode
- JoiningDate
- DepartmentId

those fields should be modeled as **Properties on an Interface**, and the Class should implement that Interface.

#### Why this rule exists
This keeps property definition centralized at the interface level and supports:
- interface-first modeling
- reuse of property contracts
- cleaner class composition
- more predictable metadata structure

#### Forge v1 rule
- Property → belongs to Interface
- Class → implements Interface(s)
- Class does not directly define business properties

---

### 4.2 Relationships Exist Only Between Interfaces

In Forge v1, **relationships are defined only between Interfaces**.

A relationship should not directly be modeled as:
- Class → Class

Instead, it should be modeled as:
- Interface ↔ Interface

#### Meaning
If one object concept is related to another, the relationship is defined at the interface contract level.

Example:
- `IEmployee` related to `IDepartment`
- `ILeaveRequest` related to `IEmployee`

#### Why this rule exists
This keeps relationship modeling aligned with the interface-first approach and makes the relationship reusable across any Class implementing the relevant interfaces.

#### Forge v1 rule
- Relationship source/end 1 → Interface
- Relationship target/end 2 → Interface
- Class-level usage happens through implemented interfaces

---

### 4.3 Every Class Has Exactly One Primary Interface and One IObject Interface

In Forge v1, every Class must have:

1. **exactly one Primary Interface**
2. **exactly one IObject interface**
3. **optional additional interfaces**

---

#### 4.3.1 Primary Interface
Each Class must expose one **Primary Interface**.

The Primary Interface represents the main business contract of the Class.

Example:
- `Employee` class → primary interface = `IEmployee`
- `Asset` class → primary interface = `IAsset`

This gives each Class one clear principal business identity from the metadata point of view.

---

#### 4.3.2 IObject Interface
Each Class must also implement **IObject**.

`IObject` represents the common platform-level object contract for Forge.

Earlier discussions also suggested that a common base metadata contract like `IMetaObject` may exist for metadata concepts, but at the Class modeling rule level the key locked rule is:

- every business Class has one Primary Interface
- every business Class also has IObject

---

#### 4.3.3 Additional Interfaces
A Class may implement **additional interfaces** besides:
- Primary Interface
- IObject

This allows a Class to expose additional capabilities or reusable property groups without changing the primary business identity of the Class.

Example:
- `Employee` class
  - Primary Interface = `IEmployee`
  - Additional Interface = `IAuditable`
  - Additional Interface = `IContactDetails`

---

### 4.4 Class May Implement Additional Interfaces

Forge v1 allows a Class to implement multiple interfaces, but with the following structure:

- **1 Primary Interface**
- **1 IObject Interface**
- **0 or more additional interfaces**

This allows flexible composition while still keeping a stable core identity for the Class.

#### Why this rule exists
It supports:
- reuse of common property groups
- modular class composition
- controlled extensibility
- cleaner separation of concerns between business identity and auxiliary capabilities

#### Example
An `Employee` Class could implement:
- `IEmployee` → primary business interface
- `IObject` → mandatory platform object contract
- `IContactInfo` → additional reusable contact properties
- `IEmploymentInfo` → additional reusable employment properties

---

### 4.5 Interface Inheritance Is Not Supported in Forge v1

Forge v1 will **not support Interface inheritance**.

That means:
- one Interface cannot inherit another Interface in v1
- inherited property exposure between interfaces is not part of the first version

#### Why this rule exists
This is intentionally kept simple for Forge v1.

Interface inheritance introduces additional complexity around:
- property resolution
- relationship resolution
- override behavior
- deployment/versioning implications
- metadata traversal and runtime interpretation

Since Forge v1 is already introducing interface-first modeling, relationship modeling, versioning, deployment, and policy concepts, avoiding interface inheritance keeps the first version simpler and more stable.

#### Forge v1 rule
- Interface A cannot inherit Interface B
- property reuse across interfaces must be handled by composition patterns, not inheritance

---

### 4.6 Graph Root Is Class and Traversal Happens Through Interfaces

In Forge v1, **Graph root should be a Class**, while graph traversal should happen through the Interfaces and Relationships implemented by that Class.

#### Meaning
A Graph is intended to represent connected object data around one central object.  
The graph starts from a Class, but the traversal logic should use:
- interfaces implemented by the class
- relationships defined between interfaces
- direct and indirect relationship paths

#### Why this rule exists
This keeps graph behavior aligned with the rest of the modeling approach:
- runtime objects are class-based
- structural contracts are interface-based
- relationships are interface-based

So the natural combination is:
- **root = Class**
- **traversal = Interface relationships**

---

### 4.7 Forge v1 Property Types

Forge v1 will support the following property type categories:

1. **Primitive**
2. **Enum**
3. **Interface Reference**

---

#### 4.7.1 Primitive Types
Primitive types are simple scalar values such as:
- string
- int
- decimal
- bool
- datetime
- guid
- other basic platform-supported primitives

---

#### 4.7.2 Enum Types
Enum types represent a controlled set of values.

Example:
- EmployeeStatus
- LeaveStatus
- AssetCategory

Enums should be modeled as explicit metadata concepts rather than arbitrary free-text values where a controlled set is required.

---

#### 4.7.3 Interface Reference
An Interface Reference type allows a property to refer to another interface-based business concept.

This is the Forge v1 mechanism for representing reference-style business links through property typing.

It should be treated distinctly from primitive and enum types because it points to another metadata-defined concept rather than storing a simple scalar value.

---

### 4.8 Relationships Are Between Two Interfaces Only

A relationship in Forge v1 can exist **only between two interfaces**.

This point became important during discussion because it clarifies that:
- relationships are not n-ary
- relationships are not directly Class ↔ Class
- relationships are not arbitrary graph edges between any metadata concept

Instead, a relationship always has exactly two ends:

- **End 1**
- **End 2**

and both ends point to Interfaces.

This also aligns with the naming preference to use:
- **End 1**
- **End 2**

rather than source / target terminology.

---

### 4.9 Classes Expose Interface Properties to Runtime

Although properties belong to Interfaces, the Class implementing those interfaces should expose those properties as part of the runtime business object model.

So the design rule is:

- Interface owns the property definition
- Class realizes / exposes the interface contract
- runtime business records use the Class as the object root
- but the available fields come from the interfaces implemented by the Class

This is an important conceptual rule because it explains how interface-defined metadata becomes usable in actual business objects.

---

### 4.10 Locked Forge v1 Modeling Rules Summary

The following are considered locked for Forge v1:

1. **Properties belong only to Interfaces**
2. **Relationships exist only between Interfaces**
3. **Every Class has exactly one Primary Interface**
4. **Every Class has exactly one IObject Interface**
5. **Class may implement additional Interfaces**
6. **Interface inheritance is not supported in v1**
7. **Graph root is Class; traversal uses Interface relationships**
8. **Forge v1 property types are Primitive, Enum, and Interface Reference**
9. **A relationship always exists between exactly two Interfaces**
10. **Class exposes interface-defined properties to runtime through implemented interfaces**

These rules should be treated as the baseline modeling contract for Forge v1 unless a later design revision explicitly changes them.