# 📢 Notification Service

A robust microservice-based notification system built with **ASP.NET Core**, **MassTransit**, and **Polly**, supporting Email, SMS, and Push notifications. Designed to be resilient, idempotent, and extensible with support for OTPs and dynamic templates.

---

## ✨ Features

- **Multi-channel Notifications**: Email, SMS, and Push.
- **Pluggable Provider Architecture**: Easily swap or add notification service providers.
- **Fallback Logic**: Auto-switches between providers if one fails.
- **Idempotency Handling**: Prevents duplicate message delivery.
- **OTP TTL Enforcement**: Discards expired OTPs before sending.
- **Dynamic Templating**: Templates configurable via external JSON/db for marketing flexibility.
- **Resiliency Patterns**:
  - Retry with exponential backoff
  - Circuit Breaker to isolate failing providers

---

## 📦 Technologies Used

- **ASP.NET Core Web API**
- **MassTransit** with RabbitMQ 
- **Polly** for Retry/Circuit Breaker
- **System.Text.Json** for serialization
- **Dependency Injection** (built-in)
- **Clean Architecture** principles

---
