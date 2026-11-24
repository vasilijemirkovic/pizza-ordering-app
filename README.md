# Pizza Ordering App

A full-stack pizza menu application built with **ASP.NET Core Web API** and a **Blazor WebAssembly** client. Anyone can browse the menu, users can register and log in, and administrators can add, edit and delete pizzas. Authentication is based on JWT with role-based authorization.
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
- Blazor WebAssembly client: pizza list, registration, login and logout
- Admin-only UI for adding, editing and deleting pizzas, with protected pages
- JWT stored in the browser and attached to API requests automatically (`DelegatingHandler`)
- Shared DTO project used by both API and client, so validation rules apply on both sides
- Friendly error handling when the API is unreachable

## Tech Stack
- .NET 10 / ASP.NET Core Web API
- Entity Framework Core with SQLite
- JWT Bearer authentication
- ASP.NET Core Identity `PasswordHasher` (PBKDF2)
- Swagger / Swashbuckle
- xUnit
- Blazor WebAssembly
- Bootstrap 5, Bootstrap Icons

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

4. In a second terminal, run the Blazor client:
   ```
   cd frontend-blazor
   dotnet run
   ```
   Open the client at `http://localhost:5085`. The API must be running at the same time.

The client reads the API address from `frontend-blazor/wwwroot/appsettings.json` (`ApiBaseUrl`), and the API allows requests from the client through CORS (`ClientUrl` in `api/appsettings.json`). If you change a port, update both values.


## Testing

The solution includes unit tests (services, mapping, exception handler) and
integration tests (API endpoints, authentication and authorization) using xUnit
and `WebApplicationFactory` with an in-memory SQLite database.

Run all tests from the repository root:
    dotnet test


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

```
├── api/               # ASP.NET Core Web API (controllers, services, EF Core, JWT)
├── frontend-blazor/   # Blazor WebAssembly client
├── shared/            # DTOs shared by the API and the client
├── tests/             # Unit and integration tests (xUnit)
└── ContosoPizza.slnx  # Solution file
```

## Roadmap
- [x] Unit and integration tests with xUnit
- [x] Blazor WebAssembly client
- [ ] Angular client consuming the same API
- [ ] Deployment

## Acknowledgements

This project started from Microsoft's ContosoPizza tutorial and was extended with a database, DTOs, validation, global error handling, and JWT authentication with role-based authorization.