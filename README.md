# Task Manager API

A production-style REST API built with ASP.NET Core 8, demonstrating clean layered architecture, secure authentication, and real-world backend engineering practices — built as a focused portfolio project to transition from Unity/game development into backend engineering.

**Live demo:** [https://task-manager-api-c5qi.onrender.com/swagger/index.html](https://task-manager-api-c5qi.onrender.com/swagger/index.html)
*(Hosted on Render's free tier — the app may take 30–60 seconds to wake up on first request after inactivity.)*

---

## What it does

A multi-user task management API. Each user registers and logs in independently; every task is owned by exactly one user, and the API enforces that a user can only ever see or modify their own tasks — even if they guess another user's task ID directly.

Core features:
- User registration and login with hashed passwords
- JWT-based authentication on all task endpoints
- Full CRUD for tasks (Create, Read, Update, Delete)
- Per-user data isolation enforced at the data-access layer, not just at the auth layer
- Input validation on all incoming requests
- Automated tests covering ownership and security-critical logic

---

## Tech stack

| Layer | Choice |
|---|---|
| Framework | ASP.NET Core 8 (Web API, Controllers) |
| Database | PostgreSQL |
| ORM | Entity Framework Core 8 |
| Auth | JWT Bearer tokens, ASP.NET Core Identity password hashing |
| Testing | xUnit, EF Core InMemory provider |
| Containerization | Docker (multi-stage build) |
| Hosting | Render (Web Service + managed PostgreSQL) |

---

## Architecture

The project follows a layered, dependency-injected design:

```
Controller → Service (interface-based) → EF Core DbContext → PostgreSQL
```

- **Controllers** handle HTTP concerns only — routing, status codes, request/response shape.
- **Services** contain business logic behind interfaces (`ITaskService`), so implementations can be swapped or mocked without touching controllers.
- **DTOs**, not EF Core entities, define request/response shapes — this keeps internal data (like password hashes) from ever being exposed in an API contract, and keeps clients from being able to set fields they shouldn't control (like `UserId` on a new task).
- **Ownership checks live in the service layer**, not just behind `[Authorize]` — every query is filtered by the authenticated user's ID, so authentication (*who are you*) and authorization (*what can you access*) are enforced independently. This is verified directly by the test suite.

### A few deliberate decisions worth noting

- **PostgreSQL over SQLite**: chosen specifically to match what's commonly used in production environments and listed in job requirements, over SQLite's zero-config convenience.
- **.NET 8 (LTS) over .NET 10**: matched to available tooling at the time of building, and reflects that most production codebases run on an LTS release rather than the newest major version.
- **`Scoped` service lifetime, not `Singleton`**: required once the service layer depends on `DbContext`, which isn't thread-safe — a deliberate lifetime choice, not a default left unexamined.

---

## Running it locally

**Prerequisites:** .NET 8 SDK, PostgreSQL (or Docker)

```bash
# Clone the repo
git clone https://github.com/aftab-games/task-manager-api.git
cd task-manager-api

# Set required configuration (User Secrets recommended locally)
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=taskmanagerdb;Username=postgres;Password=yourpassword"
dotnet user-secrets set "Jwt:Key" "your-secret-key-min-32-chars"
dotnet user-secrets set "Jwt:Issuer" "TaskManagerApi"
dotnet user-secrets set "Jwt:Audience" "TaskManagerApiUsers"
dotnet user-secrets set "Jwt:ExpiryMinutes" "60"

# Apply database migrations
dotnet ef database update

# Run
dotnet run
```

Then open `https://localhost:{port}/swagger` to explore the API interactively.

### Running with Docker

```bash
docker build -t taskmanagerapi .
docker run -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=taskmanagerdb;Username=postgres;Password=yourpassword" \
  -e Jwt__Key="your-secret-key-min-32-chars" \
  -e Jwt__Issuer="TaskManagerApi" \
  -e Jwt__Audience="TaskManagerApiUsers" \
  -e Jwt__ExpiryMinutes="60" \
  taskmanagerapi
```

---

## API Overview

| Method | Endpoint | Auth required | Description |
|---|---|---|---|
| POST | `/api/auth/register` | No | Create a new user account |
| POST | `/api/auth/login` | No | Authenticate and receive a JWT |
| GET | `/api/tasks` | Yes | Get all tasks for the logged-in user |
| GET | `/api/tasks/{id}` | Yes | Get a single task (must be owned by the user) |
| POST | `/api/tasks` | Yes | Create a new task |
| PUT | `/api/tasks/{id}` | Yes | Update a task (must be owned by the user) |
| DELETE | `/api/tasks/{id}` | Yes | Delete a task (must be owned by the user) |

Full interactive documentation is available via Swagger at the live demo link above, or locally at `/swagger`.

---

## Testing

Run the test suite:
```bash
dotnet test
```

Tests focus on the service layer's business logic and security-critical behavior, including:
- Task creation and correct ownership assignment
- A user cannot read, update, or delete another user's task
- Task listing correctly scopes results to the requesting user only
- Graceful handling of operations on non-existent tasks

---

## What's next

This project is the first in a planned sequence of backend/architecture-focused portfolio work, including containerized CI/CD pipelines, a real-time features API (SignalR), and a larger system-design-focused project exploring service boundaries and matching logic.

---

## Author

**Md Aftab Uddin**
Unity Game Developer transitioning into backend engineering — 5+ years building shipped, live products.
[LinkedIn](https://www.linkedin.com/in/aftab-games) · [Portfolio](https://aftab-games.github.io) · aftab.uddin.games@gmail.com