# FastChange Platform

A distributed FinTech demo platform designed to showcase modern backend, frontend, and mobile engineering practices using .NET ecosystem and event-driven architecture.

---

## Purpose

FastChange Platform is a portfolio-level engineering project that simulates a currency exchange system with support for:

- Wallet management
- Currency exchange operations
- Event-driven architecture
- Distributed messaging systems
- Real-time updates (planned)
- AI integration (planned)

The goal is to demonstrate real-world system design, not just isolated features.

---

## Architecture Overview

The system is built using Clean Architecture principles:

- Domain Layer — business rules and entities
- Application Layer — CQRS use cases, commands, queries and abstractions
- Infrastructure Layer — external integrations, messaging, background services
- Persistence Layer — PostgreSQL and MongoDB implementations
- Contracts Layer — integration events and shared contracts
- API Layer — HTTP endpoints

The system follows:

- CQRS (Command Query Responsibility Segregation)
- Event-Driven Architecture
- Outbox Pattern for reliable messaging
- Idempotent event processing

---

## Tech Stack

### Backend
- .NET 10
- ASP.NET Core
- Entity Framework Core
- PostgreSQL
- MediatR
- FluentValidation
- Mapster

### Messaging
- Apache Kafka
- Event-driven communication
- Outbox Pattern
- Background consumers

### Caching
- Redis

### Frontend
- React
- TypeScript
- Vite
- Tailwind CSS

### Mobile
- .NET MAUI
- MVVM pattern
- CommunityToolkit.Mvvm

---

## Key Features

- JWT authentication with refresh tokens
- Multi-currency wallet system
- Currency exchange operations
- User-to-user transfers
- Transaction history with pagination
- Optimistic concurrency control
- Kafka event publishing
- Outbox Pattern
- MongoDB read model
- Telegram notifications
- Integration testing with Testcontainers

---

## Key Design Concepts

- Clean Architecture
- CQRS
- Event-Driven Architecture
- Outbox Pattern
- Idempotency
- Polyglot persistence (planned)
- Distributed systems fundamentals

---

## Documentation

Detailed documentation is available in the `/docs` folder:

- Architecture overview
- Technologies explanation
- Architecture Decision Records (ADR)
- Roadmap
- API specification

---

## Getting Started

Docker Compose configuration is planned.
Currently infrastructure dependencies can be started using Testcontainers for integration testing.

---

## Status

This project is under active development and used as a portfolio-grade system design showcase.

---

## Key Goal

The main objective is to demonstrate:

- Ability to design distributed systems
- Understanding of messaging systems (Kafka, RabbitMQ)
- Practical implementation of Clean Architecture
- Real-world backend engineering skills in .NET ecosystem