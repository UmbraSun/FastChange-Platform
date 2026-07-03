# Architecture Overview

## Solution Structure

- Domain
- Application
- Persistence
- Infrastructure
- API

---

## Dependency Rules

- Domain depends on nothing
- Application depends only on Domain
- Persistence depends on Domain and Application
- Infrastructure depends only on Application (via interfaces)
- API depends on Application and Infrastructure

---

## Domain Layer

Contains business logic:

- Entities:
  - Wallet
  - Transaction
  - User

- Domain Events:
  - WalletDepositedDomainEvent
  - WalletWithdrawnDomainEvent

- Factories:
  - TransactionFactory

- Enums:
  - TransactionType

---

## Application Layer

Contains use cases:

- Features:
  - Exchange
  - Deposit
  - Withdraw

- Common:
  - Interfaces
  - Services
  - Behaviors (Validation, Logging)
  - Exceptions
  - Mapping

---

## Persistence Layer

Responsible for data storage:

- ApplicationDbContext
- EF Configurations
- Repositories
- UnitOfWork
- Outbox
- ProcessedEventStore

---

## Infrastructure Layer

Integrations:

- Kafka (Producer / Consumer)
- RabbitMQ
- Redis
- External APIs
- Telegram Bot
- SignalR (future)

---

## Exchange Flow

ExchangeCommand => ExchangeCommandHandler => Domain logic (Wallet / Transaction) => UnitOfWork.SaveChanges => Outbox / Events => Kafka / RabbitMQ => Consumers => Handlers

---

## Messaging Strategy

- RabbitMQ => transactional messaging / commands
- Kafka => event streaming / history / integration events

---

## Consistency Model

- Database is source of truth
- Outbox ensures reliable event publishing
- Consumers are idempotent