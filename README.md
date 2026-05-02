📘 SkillSnap – Developer Portfolio Manager
SkillSnap is a full‑stack portfolio management application built with Blazor WebAssembly, ASP.NET Core Web API, Entity Framework Core, and JWT Authentication.
It allows users to securely manage their skills, projects, and profile in a clean, modern interface.

🚀 Features
🔐 Authentication & Security
User registration & login using ASP.NET Identity

JWT-based authentication for secure API access

Users can only access their own skills and projects

HTTPS enforced for secure communication

🧠 Portfolio Management
Skill CRUD (Create, Read, Update, Delete)

Project CRUD

Automatic creation of a PortfolioUser profile on registration

Clean, simple DTOs for frontend communication

⚡ Performance Enhancements
In‑Memory Caching (5‑minute TTL)

Cache invalidation on create/update/delete

Lightweight API responses (no navigation properties)

Optimized EF Core queries

🧩 Blazor WebAssembly Frontend
Reusable components (SkillList, SkillForm, ProjectList, ProjectForm)

Service layer for API communication

Built‑in validation and clean UI structure

Smooth state refresh after CRUD operations

🗄️ Database
SQLite database for simplicity and portability

EF Core migrations for schema management

Simplified relational model (no navigation properties)

🏗️ Architecture Overview
Code
SkillSnap/
│── SkillSnap.Api/        → ASP.NET Core Web API
│── SkillSnap.Client/     → Blazor WebAssembly frontend
│── SkillSnap.Shared/     → Shared DTOs and models
│── SkillSnap.sln         → Solution file
Backend
ASP.NET Core Web API

EF Core + SQLite

ASP.NET Identity

JWT Authentication

In‑Memory Cache

Frontend
Blazor WebAssembly

HttpClient service layer

Reusable components

Form validation

🔧 How to Run the Project
1. Restore dependencies
Code
dotnet restore
2. Apply migrations
Code
cd SkillSnap.Api
dotnet ef database update
3. Run the API
Code
dotnet run
4. Run the Blazor client
Code
cd SkillSnap.Client
dotnet run
The client will open in your browser.

🧪 Business Logic Summary
Every Skill and Project is tied to the authenticated user via JWT "uid" claim.

The server assigns PortfolioUserId automatically (client never sends it).

CRUD operations enforce ownership and validation.

Cache is used for reads and invalidated on writes.

DTOs prevent over‑posting and simplify communication.

🔒 Security Measures
JWT authentication

User‑scoped authorization

Identity password hashing

HTTPS enforcement

Input validation

No navigation properties (prevents data leakage)

EF Core protections (parameterized queries)

📈 Performance Improvements
In‑Memory Cache (5‑minute TTL)

Lightweight DTOs

Removed navigation properties

Optimized EF Core queries

Reduced payload sizes

Efficient Blazor component rendering

📚 Technologies Used
C# / .NET 8

Blazor WebAssembly

ASP.NET Core Web API

Entity Framework Core

SQLite

ASP.NET Identity

JWT Authentication

🙌 Author
Hoang  
Full‑Stack Developer (Blazor + ASP.NET Core)
Capstone Project – 2026
