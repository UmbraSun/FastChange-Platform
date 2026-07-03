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

- Domain Layer — business logic and entities
- Application Layer — use cases (CQRS)
- Persistence Layer — database access (EF Core)
- Infrastructure Layer — external systems (Kafka, RabbitMQ, Redis)
- API Layer — HTTP interface

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
- RabbitMQ (queue-based messaging)
- Apache Kafka (event streaming)

### Caching
- Redis

### Planned
- MongoDB (read models / audit logs)
- SignalR (real-time updates)
- Telegram Bot integration
- AI / RAG integration

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

- Wallet system (deposit / withdraw)
- Currency exchange with rate provider
- Transaction tracking
- Event publishing via Outbox Pattern
- Kafka / RabbitMQ integration
- Idempotent consumers

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

> (to be completed with Docker Compose setup)

Planned setup includes:

- PostgreSQL
- Kafka
- RabbitMQ
- Redis
- API service

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