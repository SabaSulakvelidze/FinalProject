# FinalProject

FinalProject is a practice and portfolio ASP.NET Core Web API project for an internal management system. It is focused on managing users, permissions, projects, project members, and project tasks, while also demonstrating JWT authentication, SignalR notifications, Entity Framework Core integration, and Swagger-based API documentation.

## Project Goal

This project simulates an internal company management system where users can register, authenticate, manage projects, assign members, create tasks, and receive real-time notifications.

It was built primarily for learning and portfolio purposes, with emphasis on:
- ASP.NET Core Web API development
- Entity Framework Core with SQL Server
- JWT authentication and authorization
- AutoMapper-based DTO mapping
- Role/permission-driven access control
- SignalR-based real-time notifications

---

## Tech Stack

- **Framework:** ASP.NET Core Web API (.NET 10)
- **Database:** SQL Server / LocalDB
- **ORM:** Entity Framework Core
- **Authentication:** JWT Bearer Authentication
- **Object Mapping:** AutoMapper
- **Password Hashing:** BCrypt.Net-Next
- **Real-Time Communication:** SignalR
- **API Documentation:** Swagger / Swashbuckle

### Main Packages

- AutoMapper.Extensions.Microsoft.DependencyInjection
- BCrypt.Net-Next
- Microsoft.AspNetCore.Authentication.JwtBearer
- Microsoft.EntityFrameworkCore.Design
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.VisualStudio.Web.CodeGeneration.Design
- Swashbuckle.AspNetCore
- System.IdentityModel.Tokens.Jwt

---

## Features

Based on the repository structure and application setup, the project includes the following major areas:

- User registration and login
- JWT token-based authentication
- Permission management
- User management
- Project create/update/delete operations
- Project member assignment and management
- Project task create/update/delete operations
- Real-time notifications through SignalR
- Current authenticated user access
- Global exception handling with JSON error responses
- Swagger UI with JWT Bearer support

---

## Roles and Authorization

The project is intended as an internal management system with three main permission levels described in the repository README:

- **Admin**
  - manages roles and permissions
  - has full management access

- **Manager**
  - manages projects and related workflows

- **Employee**
  - participates in assigned projects and tasks

Authentication is implemented using **JWT Bearer tokens**, and the project setup also includes permission-oriented service and model layers.

---

## Project Structure

```text
FinalProject/
├── Controllers/
│   ├── PermissionController.cs
│   ├── ProjectController.cs
│   ├── ProjectTasksController.cs
│   └── UserController.cs
│
├── Services/
│   ├── CurrentUserService.cs
│   ├── PermissionService.cs
│   ├── ProjectService.cs
│   ├── ProjectTaskService.cs
│   ├── UserServices.cs
│   ├── NotificationService.cs
│   ├── PasswordService.cs
│   ├── NameIdUserIdProvider.cs
│   └── interface definitions
│
├── Hubs/
│   └── NotificationHub.cs
│
├── Models/
│   ├── Requests/
│   ├── Responses/
│   ├── AlgoUniFinalProjectDbContext.cs
│   ├── Permission.cs
│   ├── PermissionsForUser.cs
│   ├── Project.cs
│   ├── ProjectMember.cs
│   ├── ProjectTask.cs
│   └── User.cs
│
├── Mappers/
├── Exceptions/
├── wwwroot/
├── Properties/
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── FinalProject.csproj
└── README.md
```

---

## Architecture Notes

The project follows a layered Web API structure:

- **Controllers** handle HTTP requests and responses
- **Services** contain business logic
- **Models** contain entities, request models, and response models
- **Mappers** contain AutoMapper profiles
- **Exceptions** contain custom application exceptions
- **Hubs** handle SignalR-based real-time communication

### Implemented Architectural Elements

- Service layer abstraction using interfaces
- DTO-based request/response separation
- AutoMapper profiles for mapping entities and DTOs
- JWT-based authentication
- SignalR hub for notifications
- `IUserIdProvider`-based user targeting for notifications
- Global exception handling with JSON error responses
- Static file support via `wwwroot`

---

## Main Entities

Based on the repository model structure, the main domain entities include:

- **User** — application user account
- **Permission** — permission/role definition
- **PermissionsForUser** — relationship between users and permissions
- **Project** — project entity with management details
- **ProjectMember** — relationship between users and projects
- **ProjectTask** — task entity belonging to a project

---

## Real-Time Notifications

The project includes SignalR integration and maps a notification hub at:

```text
/hubs/notifications
```

JWT tokens can also be passed through the SignalR connection query string for hub authentication. This makes the project capable of sending user-specific real-time updates, such as task or project-related notifications.

---

## API Overview

### Authentication / Users
- Register users
- Log in users
- Retrieve current authenticated user information
- Manage users

### Permissions
- Manage permissions
- Assign and control permission-based access

### Projects
- Create, update, delete, and retrieve projects
- Manage project members

### Project Tasks
- Create, update, delete, and retrieve tasks
- Track work within projects

### Notifications
- Send and receive real-time notifications through SignalR

---

## Setup and Run

### Prerequisites

- .NET SDK 10
- SQL Server or SQL Server LocalDB
- Visual Studio or another .NET-compatible IDE

### 1. Clone the Repository

```bash
git clone https://github.com/SabaSulakvelidze/FinalProject.git
cd FinalProject
```

### 2. Configure Database Connection

Update the connection string in `appsettings.json` to match your local SQL Server environment.

Example section:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AlgoUniFinalProjectDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### 3. Configure JWT Settings

Make sure the JWT settings in `appsettings.json` are present and valid:

```json
"JwtSettings": {
  "Issuer": "your-issuer",
  "Audience": "your-audience",
  "Key": "your-secret-key"
}
```

### 4. Apply Database Migrations

If migrations exist in your local version of the project, run:

```powershell
Update-Database
```

If migrations are not yet created for a fresh database, generate them first and then apply them:

```powershell
Add-Migration InitialCreate
Update-Database
```

### 5. Run the Project

```bash
dotnet run
```

---

## Swagger

Swagger is enabled in development mode and includes JWT Bearer authentication support.

You can authorize requests in Swagger by providing the token in this format:

```text
Bearer your_jwt_token
```

---

## Error Handling

The project uses centralized exception handling and returns JSON error responses.

### Custom Exception Mapping

Based on the application pipeline:

- `ElementNotFoundException` → `404 Not Found`
- `ConflictException` → `409 Conflict`
- all other exceptions → `500 Internal Server Error`

Example response format:

```json
{
  "error": "Error message here"
}
```

---

## Static Files / Local UI Support

The application enables static files and includes a `wwwroot` folder, which can be used for lightweight local HTML pages for testing or demo purposes.

---

## Notes

- This project is structured as a layered ASP.NET Core Web API application.
- It uses SQL Server through Entity Framework Core.
- JWT authentication is configured in `Program.cs`.
- SignalR is configured for notification delivery.
- Swagger is enabled for development-time API testing.
- The project is intended for backend practice, learning, and portfolio use.

---

## Possible Future Improvements

- Add refresh token support
- Add unit and integration tests
- Add richer authorization policies
- Improve deployment readiness
- Add audit logging for project and task changes
- Add task comments and history tracking
- Add a full front-end client
- Improve role and permission administration UX

---

## Repository

GitHub repository:

`https://github.com/SabaSulakvelidze/FinalProject`
