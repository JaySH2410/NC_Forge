## 5. Core Metadata Concepts

This section defines the **core metadata concepts** that form the structural foundation of Forge.  
These concepts belong primarily to **Layer 1 — Metadata**, though they are later consumed by **Layer 2 — AdminConfigData** and indirectly shape **Layer 3 — BusinessData**.

The goal of this section is to define **what each concept means in Forge v1**, how it should be understood, and how it participates in the overall platform model.

---

### 5.1 Object

Object is the broadest conceptual unit in Forge.

It represents a **platform-defined conceptual entity** that can participate in metadata, configuration, or runtime modeling depending on context.  
In practical use, Object often acts as the base concept from which more specialized concepts such as **Class** are understood.

#### In Forge discussions, Object is important because:
- it acts as the conceptual “thing” being modeled
- it can represent a business concept such as Employee, Asset, Department, LeaveRequest, etc.
- it is the base identity around which classes, interfaces, relationships, graphs, and views are built

#### Forge v1 interpretation
For business application modeling, the **runtime object root is expected to be the Class**, but the term “Object” remains useful as the broader conceptual unit of the platform.

---

### 5.2 Class

Class is the **runtime business object definition** in Forge.

A Class represents the concrete object definition that will be used as the root of business records and graph traversal.  
It is the primary metadata concept that bridges platform structure and runtime business usage.

#### A Class in Forge v1:
- implements one **Primary Interface**
- implements one **IObject** interface
- may implement additional interfaces
- acts as the root business object used by runtime data
- exposes properties through the interfaces it implements

#### Examples of Classes
- Employee
- Asset
- Department
- LeaveRequest
- Project
- Site

#### Role of Class
Class is the metadata concept that becomes closest to the actual business object used by the configured application and runtime business data.

---

### 5.3 Interface

Interface is the **primary structural contract** in Forge.

Interfaces define the reusable structural shape of a business concept by exposing:
- properties
- relationships
- interface-level structural identity

In Forge v1, interfaces are central to the platform because:
- properties belong to interfaces
- relationships exist between interfaces
- classes implement interfaces
- graph traversal relies on interface relationships

#### Examples of Interfaces
- IEmployee
- IAsset
- IDepartment
- IContactInfo
- IEmploymentInfo

#### Role of Interface
An Interface defines **what a Class exposes**, but does not itself represent the runtime business record root.  
It is a structural contract rather than the final runtime object.

---

### 5.4 Property

Property is the metadata concept used to define a **field / attribute / value-holding member** of an interface.

In Forge v1:
- a Property belongs only to an Interface
- a Property is later exposed through the Class implementing that Interface
- a Property has a type
- a Property may participate in runtime business data, views, filtering, reporting, policy conditions, and application logic

#### Examples of Properties
- EmployeeName
- EmployeeCode
- JoiningDate
- Status
- BusinessUnitId
- DepartmentId

#### Property ownership rule
A Class does not directly own business properties.  
Properties must be defined on Interfaces.

---

### 5.5 Type

Type defines the **data shape of a Property**.

Forge v1 currently recognizes the following property type categories:

1. **Primitive**
2. **Enum**
3. **Interface Reference**

#### Primitive Type
Represents scalar values such as:
- string
- int
- decimal
- bool
- datetime
- guid

#### Enum Type
Represents a controlled set of values.

Example:
- EmployeeStatus
- LeaveStatus
- ApprovalState

#### Interface Reference Type
Represents a property that refers to another interface-based concept rather than a scalar value.

Type is therefore the metadata concept that tells Forge **what kind of value a Property holds**.

---

### 5.6 Relationship

Relationship defines a **connection between two Interfaces**.

A relationship is not defined directly between classes in Forge v1.  
Instead, it always connects **Interface End 1** and **Interface End 2**.

#### Relationship characteristics in Forge v1
- always between exactly two interfaces
- participates in graph traversal
- helps define direct and indirect connectivity between business concepts
- acts as part of the structural model used by classes that implement the interfaces

#### Examples
- IEmployee ↔ IDepartment
- ILeaveRequest ↔ IEmployee
- IAsset ↔ ISite

#### Relationship purpose
Relationships allow Forge to represent how business concepts connect to one another in a reusable, interface-first way.

---

### 5.7 Graph

Graph represents a **connected data view centered around one Class**, where the graph traverses direct and indirect relationships defined between interfaces.

Graph is intended to model “all related data around one central business object” in a structured way.

#### Graph in Forge v1
- root = Class
- traversal = relationships between interfaces implemented by that class and connected classes
- can include direct and indirect relationships
- is useful for connected business views, data exploration, and reporting-style structures

#### Example
A graph rooted at `Employee` might traverse:
- Employee → Department
- Employee → LeaveRequests
- Employee → BusinessUnit
- Employee → Site

Graph is therefore a **runtime-oriented metadata construct** for connected business object representation.

---

### 5.8 View

View represents a **configured way of presenting or consuming object data**.

A View is not the raw metadata structure itself; instead, it is a configured presentation / reporting / interaction construct built using metadata concepts such as:
- classes
- interfaces
- properties
- relationships
- graphs

#### View may be used for:
- data listing
- detail display
- reporting
- business forms
- filtered application screens
- configured presentation of a graph or object set

#### Role of View
View acts as a bridge between the structural platform model and the user-facing configured application experience.

---

### 5.9 Supporting Metadata Contracts

During design discussions, it was also proposed that Forge may have common platform contracts such as:
- `IObject`
- `IMetaObject`
- `IMetaAuditable`

These are not end-user business concepts by themselves, but rather supporting contracts that provide:
- common identity
- metadata-level base behavior
- platform-wide conventions such as auditability

#### Example direction discussed
- `IMetaObject` may contain only `Id`, `Uid`, and `Name`
- audit fields may live in a separate interface such as `IMetaAuditable`

These contracts are supporting metadata design constructs and may be refined further as the metadata engine is implemented.

---

### 5.10 Concept Relationship Summary

The Forge v1 conceptual relationship between the major metadata concepts can be summarized as follows:

#### Object
Broad conceptual unit.

#### Class
Concrete business object definition and runtime object root.

#### Interface
Structural contract implemented by Class.

#### Property
Field defined on Interface.

#### Type
Data type of a Property.

#### Relationship
Connection between two Interfaces.

#### Graph
Connected traversal structure rooted at a Class.

#### View
Configured presentation / consumption structure built using the metadata model.

---

### 5.11 Metadata Concept Summary Table

| Concept | Purpose in Forge |
|---|---|
| Object | Broad conceptual unit representing a modeled thing in the platform |
| Class | Concrete runtime business object definition; root of runtime object usage |
| Interface | Structural contract defining properties and relationships |
| Property | Field/attribute defined on an Interface |
| Type | Data shape of a Property |
| Relationship | Connection between exactly two Interfaces |
| Graph | Connected traversal model centered on a Class |
| View | Configured presentation/consumption model built using metadata |

---

### 5.12 Forge v1 Concept Positioning

For Forge v1, the most important conceptual positioning is:

- **Class is the runtime business object root**
- **Interface is the primary structural contract**
- **Property belongs to Interface**
- **Relationship exists between Interfaces**
- **Graph is Class-rooted and interface-traversed**
- **View is the configured consumer/presentation layer over the metadata model**

These concepts together form the foundation on which AdminConfigData and BusinessData are built.