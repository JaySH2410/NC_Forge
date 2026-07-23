# Forge Platform Design v1

## 1. Forge Platform Vision

### 1.1 Purpose
Forge is a **metadata-driven low-code platform** intended to allow new business applications and end-user features to be built primarily through **configuration and data stored in the platform**, rather than by writing new backend code for every feature.

The platform should provide a stable engine built by core developers, while allowing platform admins to define business structures, relationships, views, and behavior using platform configuration. End users then interact with the final configured applications and runtime business data.

In simple terms, Forge should make it possible to:
- define business objects, interfaces, properties, and relationships through metadata
- configure applications and features through admin-side configuration
- promote these configurations across environments
- allow runtime users to use the resulting applications without requiring feature-specific backend development for each new requirement

---

### 1.2 Personas and Workflow

Forge is designed around three personas:

#### Person 1 — Core Developer
Responsible for building the **Forge platform itself**.

This includes:
- metadata engine
- admin configuration engine
- runtime business data engine
- import/export and deployment framework
- security, access, and policy infrastructure
- shared platform services and technical foundations

Person 1 does **not** build each business feature separately. Instead, Person 1 builds the platform capabilities that make feature creation possible through configuration.

---

#### Person 2 — Platform Admin / Configurator
Responsible for configuring business applications and features **inside Forge**.

This includes:
- creating and configuring business objects
- assigning interfaces to classes
- defining properties and relationships
- configuring views, graphs, and business structures
- preparing deployment packages
- promoting configuration from one environment to another

Person 2 should be able to build or extend end-user features through platform configuration, without requiring new backend feature code from Person 1 for every request.

---

#### Person 3 — End User
Responsible for using the final configured application and working with **runtime business data**.

This includes:
- creating and updating business records
- viewing and operating configured applications
- using features that were configured by Person 2 on top of the Forge platform built by Person 1

Person 3 does not interact with metadata or configuration directly; Person 3 interacts with the business application that is produced from it.

---

### 1.3 Core Goal

The core goal of Forge is to support the following workflow:

1. **Person 1 builds the platform engine once**
   - metadata model
   - configuration model
   - deployment model
   - access model
   - runtime engine

2. **Person 2 configures applications and features inside the platform**
   - business objects
   - interfaces
   - properties
   - relationships
   - views / graphs / application behavior

3. **Person 3 uses the configured business application**
   - runtime data entry
   - business process execution
   - reporting and operational usage

The key design principle is:

> New end-user features should be delivered primarily by **configuration and data changes in Forge**, not by writing new backend feature code each time.

---

### 1.4 Vision Summary

Forge is intended to act as a layered platform with a clear separation between:

- **platform engine development**
- **application configuration**
- **runtime business usage**

This separation allows Forge to behave as a low-code enterprise platform where:
- platform behavior is standardized
- business applications are configurable
- deployment across environments is structured
- runtime business data remains separate from platform definition and admin configuration