# PlayLink - ASP.NET Core Web API

PlayLink is a social media application built using the ASP.NET Core framework. The server's development involved various phases of architectural evolution, pattern implementation, and technologies used. Developing it expanded my knowledge of the ASP.NET Core framework and the C# language, as well as the .NET ecosystem in general alongside giving me a deeper understanding of relational databases and their use in web applications.

## Tech Stack

![C#](https://img.shields.io/badge/-C%23-239120?style=flat&logo=csharp&logoColor=white)
![ASP.NET](https://img.shields.io/badge/-ASP.NET%20Core-512BD4?style=flat&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/-PostgreSQL-336791?style=flat&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/-Docker-2496ED?style=flat&logo=docker&logoColor=white)

## Development Setup Guide

For a comprehensive guide on setting up the server development environment, refer to: [DevelopmentSetupGuide.md](../DevelopmentSetupGuide.md).

## Key Features

- **Real-Time Chat & Presence Tracking:** Utilizes SignalR for real-time communication and user presence tracking.
- **Authentication & Authorization:** Implements the Identity framework for robust role management, authentication, and authorization.
- **Global Error Handling:** Features user-friendly error messages for clients and detailed server logs for debugging during development.
- **Role-Based Access Control:** Controllers are protected with role-based access, determined through received JWTs.
- **Entity Relationships:** Carefully crafted entities with relationship configurations for efficient data retrieval.
- **Performance Optimizations:** Implements rate limiting and in-memory caching to reduce server load and improve response times. Query data retrieval is optimized with pagination.
- **Cloudinary SDK Integration:** Implemented for efficient photo upload functionalities.

## Architecture

While learning the framework I wanted to better understand different patterns and architectural designs and settled on Clean Architecture to promote scalability, maintainability, and separation of concerns. This structure is composed of four distinct layers, each with its own responsibility, ensuring a clear separation of concerns and facilitating easier testing and modification.

- **Domain:** Contains entities and enums, so my core business logic and not dependent on any other layer.
- **Application:** Core business logic, features, application services and interfaces. It acts as a bridge between the user interface and the domain layer, orchestrating the flow of data and interactions.
- **Infrastructure:** Focused on external concerns, the Infrastructure layer handles all interactions with external resources such as database access (PostgreSQL), third-party services, and any other elements external to the application core.
- **WebAPI:** Serving as the entry point for client interactions, the WebAPI layer consists of RESTful API endpoints and SignalR hubs. It's responsible for receiving client requests, processing them through the appropriate layers, and returning the responses.

## Patterns & Technologies

- **MediatR:** For implementing the Mediator pattern, reducing dependencies and decoupling the code.
- **Fluent Validation:** For validating models in an intuitive and flexible way.
- **Docker:** Used during development for running the PostgreSQL database.
- **Entity Framework Core:** For database access and migrations.
- **SignalR:** For real-time web functionalities.
- **ASP.NET Core Identity:** For user identity management.
- **ASP.NET Core Rate Limiting:** To prevent abuse and overuse of the API.
- **ASP.NET Core Caching:** For in-memory caching.
- **Cloudinary SDK:** For efficient photo upload functionalities.
- **PostMan:** For testing the API endpoints.
- **Swagger:** For API documentation and testing.
- **DBeaver:** For database management during development.
- **Serilog:** For logging.

## Testing

For more information on **Unit and Integration Testing** my API, refer to the [Server Testing Directory](../tests/ServerTests/).
