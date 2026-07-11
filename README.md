# Task Manager — SoftOne Interview Assignment

A full-stack task management application built as an interview assignment.

## Tech Stack

| Layer    | Technology |
|----------|------------|
| Backend  | ASP.NET Core 8 Web API — Clean Architecture, CQRS (MediatR) |
| Frontend | Angular 19 — Standalone components, Reactive Forms |
| Database | SQL Server (LocalDB for development) + Entity Framework Core |
| Auth     | HTTP Basic Auth + BCrypt password hashing |

## Project Structure

```
softone-assignment/
├── database/        # SQL schema and seed scripts
├── backend/         # ASP.NET Core solution (Domain, Application, Infrastructure, API)
├── frontend/        # Angular 19 application
└── README.md
```

## Architecture

The backend follows **Clean Architecture** with a strict dependency rule (outer layers depend on inner layers, never the reverse):

```
API → Application → Domain
Infrastructure → Domain
```

CQRS is implemented via **MediatR** — reads (Queries) and writes (Commands) are fully separated.

## Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js v18+
- SQL Server or LocalDB
- Angular CLI (`npm install -g @angular/cli`)

### Database
```sql
-- Run database/schema.sql against your SQL Server instance
```

### Backend
```bash
cd backend
dotnet restore
dotnet run --project TaskManager.API
```

API runs at `https://localhost:5001` — Swagger available at `/swagger`

### Frontend
```bash
cd frontend/task-manager-web
npm install
ng serve
```

App runs at `http://localhost:4200`

## Default Credentials

| Username | Password  |
|----------|-----------|
| admin    | admin123  |

## Features

- User authentication (HTTP Basic Auth)
- Create, read, update, delete tasks
- Filter tasks by status (Pending / InProgress / Done)
- Dual-panel UI — task list alongside add/edit form
