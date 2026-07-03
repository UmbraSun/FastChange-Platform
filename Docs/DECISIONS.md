# Architecture Decisions

This document records important architectural decisions made during the development of the FastChange Platform.

Each decision includes context, alternatives, and reasoning.

---

## ADR-001: Kafka vs RabbitMQ

### Context

The system requires asynchronous communication between services and event-driven architecture support.

We needed a message broker for:
- domain events
- integration events
- background processing

---

### Decision

Both Kafka and RabbitMQ are used in the system.

---

### Reasoning

#### RabbitMQ
Used for:
- classic message queueing
- command-style processing
- simple consumer workflows

Strengths:
- simple model
- low latency
- easy routing

---

#### Kafka
Used for:
- event streaming
- high-throughput event processing
- event history retention
- consumer groups

Strengths:
- replayable events
- horizontal scalability
- partitioning model
- durable log storage

---

### Outcome

Using both systems allows demonstrating:
- message queues (RabbitMQ)
- event streaming (Kafka)

This reflects real-world enterprise systems where both technologies coexist.

---

## ADR-002: Outbox Pattern

### Context

Direct publishing of events from application code can lead to inconsistency between:
- database state
- message broker state

---

### Decision

The Outbox Pattern is used to ensure reliable event delivery.

---

### Reasoning

- Events are stored in the database first
- A dispatcher publishes them asynchronously
- Ensures eventual consistency

---

### Outcome

- No lost events
- Reliable integration events
- Decoupling of DB and messaging systems

---

## ADR-003: CQRS with MediatR

### Context

The system requires clear separation between write and read operations.

---

### Decision

CQRS pattern implemented using MediatR.

---

### Reasoning

- Commands represent state changes
- Queries represent read-only operations
- Handlers encapsulate business logic

---

### Outcome

- Clear separation of responsibilities
- Easier testing
- Reduced controller complexity

---

## ADR-004: Mapster over AutoMapper

### Context

Object mapping is required between layers (Domain ↔ DTO).

---

### Decision

Mapster is used instead of AutoMapper.

---

### Reasoning

- Better performance
- Less runtime overhead
- Compile-time configuration possible

---

### Outcome

- Faster mapping
- Lightweight dependency

---

## ADR-005: Redis usage strategy

### Context

System requires caching layer for performance optimization.

---

### Decision

Redis is used as distributed cache, not as primary storage.

---

### Reasoning

- PostgreSQL remains source of truth
- Redis improves performance only

---

### Outcome

- Clear separation of concerns
- No data inconsistency risk

---

## ADR-006: MongoDB introduction (planned)

### Context

System will require flexible schema storage for:
- audit logs
- event history
- read models

---

### Decision

MongoDB will be introduced as a secondary database.

---

### Reasoning

- document-based storage fits logs and events
- avoids overloading relational DB

---

### Outcome

- polyglot persistence architecture
- separation of OLTP and analytical data