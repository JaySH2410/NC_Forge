## Purpose

The MetaSchema Catalog defines all built-in metadata objects that are seeded into Forge. These objects form the foundation of the platform and are used by every other layer (AdminConfig, BusinessData, Runtime, UI, Workflows, APIs, etc.).

**Version:** 1.0

* * *

# 1\. Meta Types

These describe the metadata language itself.

| Name | Category | Description | Notes |
| --- | --- | --- | --- |
| Class | Meta Type | Defines a business object or system object. | Root object |
| Interface | Meta Type | Defines a contract implemented by one or more classes. |  |
| Property | Meta Type | Defines a property belonging to a class or interface. |  |
| Relationship | Meta Type | Represents an instance relationship between two objects. |  |
| RelationshipType | Meta Type | Defines the type of a relationship. |  |
| RelationshipPath | Meta Type | Defines a traversable graph path. |  |
| EnumList | Meta Type | Defines an enumeration. |  |
| EnumValue | Meta Type | Defines an enumeration value. |  |
| Package | Meta Type | Groups related metadata. |  |

* * *

# 2\. Graph Types

| Name | Category | Description |
| --- | --- | --- |
| Graph | Graph | Graph definition |
| View | Graph | Graph projection/view |

* * *

# 3\. Events

| Name | Category | Description |
| --- | --- | --- |
| Event | Event | Event definition |
| Publish | Event | Publishes an event |
| Subscribe | Event | Subscribes to an event |

* * *

# 4\. Primitive Data Types

| Name | Category | Description |
| --- | --- | --- |
| String | Primitive | Text value |
| EncryptedString | Primitive | Encrypted text |
| Integer | Primitive | Whole number |
| Float | Primitive | Floating point number |
| Decimal | Primitive | High precision number |
| Boolean | Primitive | True / False |
| Date | Primitive | Date only |
| Time | Primitive | Time only |
| DateTime | Primitive | Date and time |
| Json | Primitive | JSON document |
| Object | Primitive | Generic object |
| List | Primitive | Ordered collection |
| Collection | Primitive | Generic collection |
| Uom | Primitive | Unit of Measure |

> **Open Question:** Should `Object` remain as a primitive, or should it be renamed to `ObjectReference`? We can defer this decision if needed.

* * *

# 5\. Runtime Logic

| Name | Category | Description |
| --- | --- | --- |
| Action | Runtime | Executable action |
| Query | Runtime | Retrieves data |
| Transform | Runtime | Converts one structure into another |
| Calculate | Runtime | Computes a value |

* * *

# MetaObjectRelationship

## Purpose

`MetaObjectRelationship` represents a directed relationship between two `MetaObject` instances.

Together with `MetaObject`, it forms the foundation of the Forge MetaSchema. Every metadata connection within Forge is represented using this entity.

The platform is intentionally designed as a graph-based metadata engine instead of relying on numerous specialized tables.

---

# Design Philosophy

Forge follows a graph-oriented architecture.

Rather than creating dedicated tables for concepts such as properties, constraints, views, actions, or workflows, these concepts are connected through metadata relationships.

```
End1 ---- RelationshipType ----> End2
```

Example:

```
Customer ---- Implements ----> ICustomer

ICustomer ---- HasProperty ----> Name

Name ---- HasDataType ----> String

ICustomer ---- HasAction ----> SaveCustomer

ICustomer ---- HasEvent ----> CustomerCreated
```

The runtime understands the platform by traversing this metadata graph.

---

# Architectural Principle

## Interface-Centric Design

Forge adopts an **Interface First** architecture.

A `Class` represents an implementation.

An `Interface` represents the public contract.

All metadata is attached to interfaces rather than classes.

```
Customer
    Implements
        ICustomer
```

```
ICustomer
    HasProperty
        Name

ICustomer
    HasProperty
        Email

ICustomer
    HasAction
        Save

ICustomer
    HasQuery
        GetCustomer

ICustomer
    HasView
        CustomerForm

ICustomer
    HasEvent
        CustomerCreated
```

This design provides:

- Loose coupling
- Metadata reuse
- Multiple implementations
- Easier versioning
- Clear separation between implementation and contract

---

# Entity Structure

| Property | Description |
|----------|-------------|
| Uuid | Unique identifier of the relationship |
| Name | Display name |
| End1Uid | Source MetaObject |
| End2Uid | Target MetaObject |
| RelTypeUid | Relationship type |
| Ordinal | Display or execution order |

---

# Relationship Model

Every relationship consists of three parts.

```
End1
↓
RelationshipType
↓
End2
```

Example

```
ICustomer
    │
    │ HasProperty
    ▼
  Name
```

Where

```
End1Uid      = ICustomer

RelTypeUid   = HasProperty

End2Uid      = Name
```

---

# Why RelationshipType is a MetaObject

Relationship types are themselves metadata.

Examples

- HasProperty
- HasDataType
- Implements
- Inherits
- Contains
- References
- HasEvent
- SubscribesTo

Since relationship types are stored as `MetaObject`, new relationship types can be introduced without modifying the database schema.

Example

```
Validates
```

can be added by simply creating another MetaObject.

No database or application changes are required.

---

# Supported Relationship Examples

## Interface Implementation

```
Customer

    Implements

ICustomer
```

---

## Property Definition

```
ICustomer

    HasProperty

Name
```

---

## Property Type

```
Name

    HasDataType

String
```

---

## Interface Actions

```
ICustomer

    HasAction

SaveCustomer
```

---

## Queries

```
ICustomer

    HasQuery

GetCustomer
```

---

## Views

```
ICustomer

    HasView

CustomerForm
```

---

## Events

```
ICustomer

    HasEvent

CustomerCreated
```

---

## Event Subscription

```
SendEmailAction

    SubscribesTo

CustomerCreated
```

---

## Inheritance

```
Employee

    Inherits

Person
```

---

## Package Membership

```
ICustomer

    MemberOf

CRM
```

---

# Design Decisions

## UUID References

Relationships reference `MetaObject.Uuid` instead of database primary keys.

Benefits

- Environment independent
- Stable identifiers
- Package portability
- Metadata synchronization
- Version compatibility

---

## No Navigation Properties

Entity Framework navigation properties are intentionally omitted.

Reasons

- Prevent accidental eager loading
- Avoid circular object graphs
- Keep entities persistence-agnostic
- Encourage explicit metadata traversal

---

## Delete Behavior

All foreign keys use

```
DeleteBehavior.Restrict
```

Metadata relationships cannot be removed implicitly through cascading deletes.

---

# Scope

`MetaObjectRelationship` stores **metadata relationships only**.

It does **not** store:

- Business data
- Runtime values
- User records
- Transactions
- Property values

These belong to higher layers such as AdminConfig or BusinessData.

---

# Future Usage

This single entity will be capable of representing:

- Data models
- Interfaces
- Properties
- Enumerations
- Views
- Forms
- Actions
- Queries
- Workflows
- Event subscriptions
- Validation rules
- Security rules
- Reporting definitions
- API definitions
- AI metadata

without introducing additional relationship tables.

---

# Guiding Principle

> If a concept can be represented as a `MetaObject` connected by a `MetaObjectRelationship`, a new database table should not be introduced.

The MetaSchema should evolve by introducing new metadata, not by introducing new tables.