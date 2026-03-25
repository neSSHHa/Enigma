# Enigma - Project Summary

Enigma is a full-stack ASP.NET Core web application for managing educational "lectures" and user enrollment based on a token system. Administrators can create and maintain lectures, while members enroll and manage their participation. Token balances are updated in real time using SignalR via the `TokenHub`.

The platform is built on **ASP.NET Core (.NET 8)** using **MVC with Razor Pages** and **ASP.NET Core Identity** for authentication and role-based authorization. User access is controlled through the `ADMIN` and `MEMBER` roles. Member accounts are created through the standard Identity Register flow (new users are added to the `MEMBER` role), while `ADMIN` rights must be assigned through role management.

Core business functionality includes lecture lifecycle management (create, edit, cancel, delete), capacity constraints (`StudentLimit`), and enrollment rules tied to lecture status (for example, `AWAITS`, `FINISHED`, `CANCELED`). Members consume tokens when enrolling, and tokens are refunded when canceling based on the lecture timing rules. A background worker (`StatusUpdateService`) automatically transitions lecture statuses based on the end time, so the system stays consistent without manual intervention.

The application also supports proof-of-payment upload, storing user-provided files in the database as a `byte[]` (`proofOfPayment`) together with metadata (description, extension, size). Additionally, a translator module (`TranslationController`) renders translations from an embedded in-code dictionary dataset.

From an engineering standpoint, Enigma uses dependency injection, an EF Core data layer backed by **SQL Server**, and a repository-style abstraction via `IUnitOfWork` and repositories. Real-time communication is handled with **SignalR**, exposing the `/tokenHub` endpoint for pushing updated token counts to connected users.

## Key Technologies

- .NET 8
- ASP.NET Core MVC + Razor Pages
- ASP.NET Core Identity (roles: `ADMIN`, `MEMBER`)
- Entity Framework Core (SQL Server)
- SignalR (`TokenHub`, endpoint: `/tokenHub`)
- Background services (`StatusUpdateService`)

## Local Setup (Quickstart)

Prequisites:

- Visual Studio 2022+
- .NET 8 SDK
- SQL Server LocalDB (or a compatible local SQL Server)

Steps:

1. Open `Enigma.sln`
2. Update the connection string in `Enigma/appsettings.json` under `ConnectionStrings`
3. Apply migrations:

```powershell
dotnet ef database update --project DataAccess --startup-project Enigma
```

4. Start the `Enigma` project
5. Register a user in the app:
   - Users are created as `MEMBER` by default
   - `ADMIN` role must be assigned separately (role management / DB update)

## Security Notes

- E-mail sending is implemented via `Utility/EmailSender.cs` (SMTP). For production, SMTP credentials should be moved to secrets / environment variables, not stored in code.
- `proofOfPayment` stores binary file data in the database (`byte[]`). For production, consider file size limits, audit logging, and encryption at rest where appropriate.

