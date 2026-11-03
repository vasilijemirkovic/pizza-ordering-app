# Pizza Ordering App

A REST API for a pizza shop, built with ASP.NET Core. Anyone can browse the menu, while only administrators can add, update or delete pizzas. Users can register and log in using JWT-based authentication.

A frontend client is currently in progress.

## Features
- CRUD operations for pizzas
- Public read access to the menu; create, update and delete restricted to admins
- User registration with securely hashed passwords
- Login that issues a JWT token
- Role-based authorization (`Admin`, `User`)
- Request validation using DTOs and data annotations
- Global exception handling with standardized `ProblemDetails` responses
- Automatic admin account seeding on startup, configured through user secrets
- Swagger UI with JWT support for testing protected endpoints

## Tech Stack
- .NET 10 / ASP.NET Core Web API
- Entity Framework Core with SQLite
- JWT Bearer authentication
- ASP.NET Core Identity `PasswordHasher` (PBKDF2)
- Swagger / Swashbuckle

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Git
- Any code editor (Visual Studio, VS Code, Rider...)