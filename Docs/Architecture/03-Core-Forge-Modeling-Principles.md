## 3. Core Forge Modeling Principles

Forge is intended to be a **metadata-driven low-code platform**, so the core modeling principles need to be explicit and stable.  
These principles define how Forge should think about structure, configuration, and runtime data.

---

### 3.1 Metadata-Driven Platform

Forge is designed as a **metadata-driven platform**.

This means that the platform should not rely on hardcoded business feature implementations for every use case.  
Instead, the platform should use **metadata and configuration** to describe:
- business objects
- properties
- relationships
- views
- application structure
- runtime behavior boundaries

The engine built by Person 1 should interpret this metadata and configuration to enable applications to be created by Person 2 without writing new backend feature code each time.

#### Design implication
Business feature creation should move as much as possible from:
- **custom code**
to
- **platform configuration + metadata**

---

### 3.2 Separation of Metadata, AdminConfigData, and BusinessData

Forge must maintain a clear separation between the three layers:

- **Metadata**
- **AdminConfigData**
- **BusinessData**

This is one of the most important platform principles.

#### Metadata
Defines the platform schema model and structural building blocks.

#### AdminConfigData
Uses metadata to configure actual applications and features.

#### BusinessData
Stores runtime business records used by end users.

#### Why this separation matters
Without this separation, the platform would mix:
- platform design concerns
- application configuration concerns
- runtime business concerns

That would make:
- versioning harder
- deployment harder
- access control harder
- reuse harder
- low-code extensibility weaker

So Forge v1 should intentionally preserve these boundaries.

---

### 3.3 Reusability of Configured Objects

Forge should encourage **reuse of configured structures** wherever possible.

The same business concept should not be repeatedly recreated in multiple places if it can be modeled once and reused.

Examples:
- a shared Employee object used across multiple applications
- a common Department concept reused across modules
- a common relationship pattern reused in multiple views or graphs

#### Reuse should exist at multiple levels
- metadata-level reuse
- admin configuration reuse
- shared business object consumption across applications

This principle is important because Forge is intended to support:
- multi-application configuration
- future tenant onboarding
- cross-module business concepts
- platform-level consistency

---

### 3.4 Interface-First Modeling

Forge follows an **interface-first modeling approach**.

This means interfaces are the primary place where structural members are defined and exposed.

In Forge v1:
- properties belong to interfaces
- relationships are defined between interfaces
- classes implement interfaces
- classes expose behavior and identity through interfaces rather than owning properties directly

#### Why interface-first
This provides:
- better reuse of property contracts
- cleaner relationship modeling
- more consistent class composition
- easier future extension and configuration reuse

This principle is one of the core modeling rules of Forge v1.

---

### 3.5 Definition vs Runtime Separation

Forge should separate:
- **definition-time concerns**
from
- **runtime concerns**

#### Definition-time concerns
These include:
- metadata design
- class/interface/property definitions
- relationship definitions
- graph/view definitions
- application configuration

#### Runtime concerns
These include:
- actual business records
- business record updates
- runtime access evaluation
- end-user operations
- operational business workflows

#### Why this matters
A business record should not redefine the schema that created it.  
Similarly, a runtime user action should not directly alter the structural metadata model unless it goes through an explicit admin/configuration process.

This separation keeps the platform predictable and governable.

---

### 3.6 Platform Engine vs Configured Application Separation

Forge should distinguish between:

#### Platform Engine
Built by Person 1.  
Responsible for:
- metadata engine
- runtime engine
- access engine
- deployment engine
- import/export and lifecycle behavior

#### Configured Application
Created by Person 2 using the platform engine.  
Responsible for:
- application-specific object configuration
- interface usage
- views
- business structures
- runtime app behavior through configuration

This distinction is important because Forge is not just one application — it is a platform that should be able to host many configured applications.

---

### 3.7 Controlled Extensibility

Forge should be **extensible**, but the extensibility should be controlled through platform rules.

That means:
- application growth should happen through metadata and configuration patterns
- versioning should be explicit
- deployment should be structured
- object evolution should be governed
- access should be policy-driven

Forge should not become a free-form system where every feature bypasses the metadata and admin configuration model.

---

### 3.8 Low-Code but Server-Side Conscious

Forge is low-code in the sense of **configuration-driven application building**, but it should still be designed with the discipline of a **server-side application platform**.

That means the platform should care about:
- versioning
- lifecycle
- access control
- data compatibility
- deployment integrity
- history and auditability
- controlled schema evolution

Forge should not behave like an ad-hoc form builder.  
It should behave like a structured enterprise platform whose application behavior is configurable.

---

### 3.9 Principle Summary

Forge v1 is based on the following modeling principles:

1. Forge is a **metadata-driven platform**
2. Metadata, AdminConfigData, and BusinessData must remain clearly separated
3. Configured structures should be reusable
4. Forge uses an **interface-first** modeling approach
5. Definition-time and runtime concerns must remain separate
6. Platform engine and configured application responsibilities must remain distinct
7. Extensibility must be controlled through platform rules
8. Forge should remain low-code while still following strong server-side design discipline