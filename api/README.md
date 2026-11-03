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

### Authentication
| Method | Endpoint         | Description                      | Access |
|--------|------------------|----------------------------------|--------|
| POST   | `/Auth/register` | Register a new user              | Public |
| POST   | `/Auth/login`    | Log in and receive a JWT token   | Public |


## Authentication

The API uses JWT Bearer authentication with role-based authorization.

1. Register a user via `POST /Auth/register`. New users are assigned the `User` role.
2. Log in via `POST /Auth/login` to receive a token:
   ```json
   { "token": "eyJhbGciOiJIUzI1NiIs..." }
   ```
3. Send the token in the `Authorization` header with each request to a protected endpoint:
   ```
   Authorization: Bearer <user-token>
   ```
4. In Swagger UI, click **Authorize** and paste the token.

Tokens expire after 45 minutes. Requests without a valid token return `401 Unauthorized`; requests from users without the required role return `403 Forbidden`.


## Project Structure

## Roadmap
