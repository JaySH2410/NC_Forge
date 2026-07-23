## 2. Forge Platform Layers

Forge is divided into * *three logical layers**.  
These layers separate platform definition, application configuration, and runtime business usage so that each concern can evolve independently while still working together as one platform.

---

### 2.1 Layer 1 — Metadata

Layer 1 contains the **platform schema definition model**.  
It defines the structural building blocks from which business applications can later be configured.

Metadata is not runtime business data and is not tenant business configuration.  
It is the foundational definition layer of the Forge platform.

#### Layer 1 is expected to contain concepts such as:
- Object / Meta Object
- Class
- Interface
- Property
- Type
- Relationship
- Graph
- View
- supporting metadata contracts and supporting metadata abstractions

#### Purpose of Metadata Layer
The purpose of Layer 1 is to define:
-what kinds of objects can exist in the platform
- how those objects are structurally described
- how properties are modeled
- how interfaces expose properties
- how relationships between interfaces are modeled
- how graphs and views can reference those structures

#### Ownership
Layer 1 is primarily built and maintained by **Person 1 — Core Developer**.

Platform admins may use Layer 1 constructs, but the design and capability of the metadata engine itself belongs to platform development.

#### Characteristics of Metadata Layer
- platform-wide and foundational
- not runtime business data
- not end-user editable business content
- reused across applications and configurations
- expected to be relatively stable compared to runtime business data

---

### 2.2 Layer 2 — AdminConfigData

Layer 2 contains the **application configuration model**.  
This is the layer where the platform’s metadata primitives are used to create configurable business applications and end-user features.

If Layer 1 defines the language of the platform, then Layer 2 uses that language to configure actual applications.

#### Layer 2 is expected to contain things such as:
- application definitions
- object configuration for a specific application
- class/interface/ property usage decisions for an application
- graph configuration
- view configuration
- application-level business structure configuration
- deployment-ready admin-side configuration artifacts

#### Purpose of AdminConfigData Layer
The purpose of Layer 2 is to allow **Person 2 — Platform Admin / Configurator** to:
-build or configure business applications inside Forge
- decide which objects and interfaces are used in a specific application
- configure how those objects are presented and used
- prepare application configuration for promotion into another environment

Layer 2 is where “feature creation through configuration” primarily happens.

#### Ownership
Layer 2 is primarily owned and maintained by **Person 2 — Platform Admin / Configurator**, using the capabilities provided by Person 1.

#### Characteristics of AdminConfigData Layer
- sits on top of Layer 1 metadata
- describes application configuration, not runtime business records
- can be promoted across environments
- is expected to change more frequently than Layer 1
- acts as the bridge between platform definition and business runtime usage

---

### 2.3 Layer 3 — BusinessData

Layer 3 contains the **runtime business data** used by end users.

This is the layer that stores the actual records created and used in business operations after an application has been configured through Layers 1 and 2.

#### Layer 3 is expected to contain:
- business records created by end users
- transactional or operational data
- runtime values for configured business objects
- runtime state and lifecycle values relevant to business operations

#### Purpose of BusinessData Layer
The purpose of Layer 3 is to support the day-to-day business usage of the configured application by **Person 3 — End User**.

Examples:
-employee records
- asset records
- leave requests
- approvals
- project - level operational records
-other business entities configured through the platform

#### Ownership
Layer 3 is operationally used by **Person 3 — End User**, but its structure and allowed behavior are governed by Layer 1 and Layer 2.

#### Characteristics of BusinessData Layer
- runtime and business-facing
- high-volume compared to metadata/configuration layers
- expected to evolve continuously as business operations happen
- scoped by business structures such as BusinessEntity, BusinessUnit, Department, Site, SubSite, and Project
- subject to runtime access control and policy enforcement

---

### 2.4 Relationship Between the Three Layers

The three layers should be understood in the following way:

#### Layer 1 — Metadata
Defines the **platform schema language** and structural concepts.

#### Layer 2 — AdminConfigData
Uses Layer 1 definitions to **configure business applications and features**.

#### Layer 3 — BusinessData
Stores the **runtime business records** used by end users in the configured application.

In short:

-**Layer 1 defines * *
-**Layer 2 configures * *
-**Layer 3 runs * *

---

### 2.5 Layer Responsibility Summary

| Layer | Name | Primary Owner | Main Purpose |
| ---| ---| ---| ---|
| Layer 1 | Metadata | Person 1 — Core Developer | Define the platform schema model and structural building blocks |
| Layer 2 | AdminConfigData | Person 2 — Platform Admin / Configurator | Configure applications and features using the metadata model |
| Layer 3 | BusinessData | Person 3 — End User(runtime usage) | Store and operate on runtime business records |

---

### 2.6 Layering Principle for Forge v1

Forge v1 should maintain a clear separation between:
-platform definition
- application configuration
- runtime business usage

This separation is important because it enables:
-low - code application creation through configuration
- controlled deployment and promotion of configured applications
- independent governance of platform schema vs runtime business data
- cleaner versioning and access control boundaries