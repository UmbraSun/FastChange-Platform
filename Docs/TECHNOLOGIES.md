# Technologies

This document describes the technologies used in the project, their purpose, and what they demonstrate from an engineering perspective.

---

## .NET 10 / ASP.NET Core

### Purpose
Main backend runtime and web framework.

### Why this technology
Provides modern performance, minimal APIs, strong DI system, background services, and first-class support for distributed systems.

### Project usage
- REST API
- Background services (Kafka/Rabbit consumers)
- CQRS handlers

### Demonstrates
- Modern C# backend development
- Web API design
- Dependency Injection
- Async programming model

---

## PostgreSQL + Entity Framework Core

### Purpose
Primary relational database for storing core business data.

### Why this technology
Industry-standard relational DB with strong consistency and ACID guarantees.

### Project usage
- Wallets
- Transactions
- Users
- Outbox pattern storage

### Demonstrates
- Relational modeling
- Transactions
- EF Core configuration
- Concurrency control

---

## Redis

### Purpose
Distributed in-memory cache.

### Why this technology
Used to reduce database load and improve response times in high-frequency operations.

### Project usage
- Caching wallet balances (optional extension)
- Rate limiting (future)
- Performance optimization layer

### Demonstrates
- Caching strategies
- Distributed systems optimization
- High-performance architecture

---

## RabbitMQ

### Purpose
Message broker for reliable queue-based messaging.

### Why this technology
Classic enterprise messaging system used for command-style asynchronous processing.

### Project usage
- Event delivery (alternative pipeline)
- Background processing

### Demonstrates
- Message queues
- Producer/consumer model
- Reliable delivery patterns

---

## Apache Kafka

### Purpose
Event streaming platform for high-throughput event processing.

### Why this technology
Designed for scalable event-driven architectures with replay capability and partitioning.

### Project usage
- Exchange events publishing
- Event consumers (idempotent processing)
- Integration event stream

### Demonstrates
- Event-driven architecture
- Distributed streaming
- Consumer groups
- Partition-based scaling

---

## MongoDB (planned)

### Purpose
Document database for non-relational data.

### Why this technology
Widely used NoSQL database for flexible schema and high-read scenarios.

### Project usage
- Audit logs
- Read models
- Event history storage

### Demonstrates
- NoSQL modeling
- Polyglot persistence
- Read-optimized design

---

## SignalR (planned)

### Purpose
Real-time communication between backend and clients.

### Why this technology
Enables push-based updates instead of polling.

### Project usage
- Live exchange updates
- Wallet balance updates

### Demonstrates
- Real-time systems
- WebSocket abstraction
- Push-based architecture

---

## FluentValidation

### Purpose
Request validation framework.

### Why this technology
Clean separation of validation logic from business logic.

### Project usage
- Command validation in CQRS pipeline

### Demonstrates
- Clean validation pipelines
- Separation of concerns

---

## Mapster

### Purpose
Object mapping between layers.

### Why this technology
Lightweight and high-performance alternative to AutoMapper.

### Project usage
- DTO ↔ Domain mapping
- API response shaping

### Demonstrates
- Clean separation of layers
- Performance-oriented design

---

## MediatR

### Purpose
In-process messaging and CQRS implementation.

### Why this technology
Decouples request handling from controllers.

### Project usage
- Commands
- Queries
- Domain event dispatching

### Demonstrates
- CQRS pattern
- Decoupled architecture

---

## Docker

### Purpose
Containerization of the system.

### Why this technology
Ensures reproducible environments and simplifies deployment.

### Project usage
- API container
- PostgreSQL
- Kafka / Redis / RabbitMQ

### Demonstrates
- DevOps basics
- Environment consistency
- Microservice readiness
