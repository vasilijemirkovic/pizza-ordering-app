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


### Setup

1. Clone the repository and navigate to the API project:
   ```
   git clone https://github.com/vasilijemirkovic/pizza-ordering-app.git
   cd pizza-ordering-app/api
   ```

2. Configure user secrets:
   ```
   dotnet user-secrets set "Jwt:Key" "your-secret-key-at-least-32-characters-long"
   dotnet user-secrets set "Admin:Username" "admin"
   dotnet user-secrets set "Admin:Password" "your-strong-password"
   ```
   - `Jwt:Key` is **required** and must be at least 32 characters long (HMAC-SHA256).
   - `Admin:Username` and `Admin:Password` are **optional**. If set, an admin account is created on first startup.

3. Run the application:
   ```
   dotnet run
   ```
   The SQLite database is created and all migrations are applied automatically on startup.

4. Open Swagger UI at `http://localhost:5153/swagger`.


## API Endpoints

### Pizzas
| Method | Endpoint      | Description              | Access     |
|--------|---------------|--------------------------|------------|
| GET    | `/Pizza`      | Get all pizzas           | Public     |
| GET    | `/Pizza/{id}` | Get a pizza by ID        | Public     |
| POST   | `/Pizza`      | Add a new pizza          | Admin only |
| PUT    | `/Pizza/{id}` | Update an existing pizza | Admin only |
| DELETE | `/Pizza/{id}` | Delete a pizza           | Admin only |

## Authentication

## Project Structure

## Roadmap
