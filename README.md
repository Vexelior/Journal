# Journal

A full-featured digital journaling application built with ASP.NET Core MVC and Entity Framework Core, designed to help users organize and maintain their daily journal entries with customizable prompts.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Database Setup](#database-setup)
  - [Configuration](#configuration)
- [Project Structure](#project-structure)
- [Usage](#usage)
  - [Creating Journals](#creating-journals)
  - [Managing Prompts](#managing-prompts)
  - [Writing Entries](#writing-entries)
  - [Exporting Journals](#exporting-journals)
- [Authentication & Authorization](#authentication--authorization)
- [Database Schema](#database-schema)
- [Development](#development)
  - [Running Locally](#running-locally)
  - [Building the Solution](#building-the-solution)
  - [Migrations](#migrations)
- [Contributing](#contributing)
- [License](#license)

## Overview

The Journal Application is a comprehensive web-based journaling system that allows users to create monthly journals, write daily entries in response to prompts, and export their thoughts in various formats. The application follows clean architecture principles with clear separation between domain logic, application services, and infrastructure concerns.

## Features

### Core Features

- **Monthly Journal Organization**: Create and manage journals organized by month and year
- **Prompt-Based Entries**: Write journal entries in response to customizable prompts
- **Entry Management**: Create, read, update, and delete journal entries with timestamps
- **Prompt Management**: Create and manage writing prompts with active/inactive states
- **PDF Export**: Export entire journals to professionally formatted PDF documents
- **Responsive Design**: Mobile-friendly interface with Bootstrap 5

### User Management

- **Identity Framework Integration**: Secure user authentication and authorization
- **Role-Based Access Control**: Admin role for full journal management
- **Account Management**: User registration, login, password reset, and profile management
- **Email Confirmation**: Optional email verification for new accounts
- **Two-Factor Authentication**: Enhanced security with 2FA support

### User Experience

- **Intuitive Navigation**: Clean, organized interface for easy journal management
- **Date-Based Organization**: Entries displayed chronologically with formatted dates
- **Visual Indicators**: Status badges for active/inactive prompts
- **Modal Confirmations**: Safe deletion with confirmation dialogs
- **Dropdown Actions**: Context menus for journal operations

## Architecture

The application follows a **Clean Architecture** pattern with clear separation of concerns:

```
Journal/
??? Domain/          # Core business entities and models
??? Application/     # Business logic and services
??? Infrastructure/  # Data access and external dependencies
??? Web/            # Presentation layer (MVC/Razor Pages)
```

### Architectural Benefits

- **Dependency Inversion**: Core business logic doesn't depend on infrastructure
- **Testability**: Easy to unit test services independently
- **Maintainability**: Clear boundaries between layers
- **Scalability**: Easy to add new features without affecting existing code

## Technology Stack

### Backend

- **.NET 10.0**: Latest .NET framework
- **ASP.NET Core MVC**: Web framework with Model-View-Controller pattern
- **Entity Framework Core 10.0**: ORM for database access
- **ASP.NET Core Identity**: Authentication and authorization
- **SQL Server**: Relational database (LocalDB for development)

### Frontend

- **Razor Views**: Server-side rendering
- **Bootstrap 5**: Responsive UI framework
- **jQuery**: DOM manipulation and AJAX
- **Bootstrap Icons**: Icon library

### Libraries & Tools

- **QuestPDF 2025.12.0**: PDF generation library
- **DocumentFormat.OpenXml 3.3.0**: Document processing
- **Entity Framework Tools**: Database migrations

## Getting Started

### Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server LocalDB](https://learn.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) or SQL Server
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommended) or any C# IDE
- Git (for cloning the repository)

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/Vexelior/Journal.git
   cd Journal/Journal
   ```

2. **Restore NuGet packages**

   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

### Database Setup

1. **Update connection string** (if needed)

   Edit `Web/appsettings.json` or `Web/appsettings.Development.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=Journal;Trusted_Connection=true;MultipleActiveResultSets=true"
     }
   }
   ```

2. **Apply migrations**

   The database will be automatically created and seeded on first run. Alternatively, you can manually apply migrations:

   ```bash
   cd Web
   dotnet ef database update
   ```

### Configuration

#### Development Environment

The application uses different configuration files for different environments:

- `appsettings.json` - Base configuration
- `appsettings.Development.json` - Development overrides
- `appsettings.Production.json` - Production overrides

#### User Secrets (Development)

For sensitive data in development, use User Secrets:

```bash
cd Web
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
```

## Project Structure

```
Journal/
??? Domain/
?   ??? Models/
?       ??? Journal.cs              # Journal entity (month/year)
?       ??? JournalEntry.cs         # Entry entity (content, date)
?       ??? Prompt.cs               # Writing prompt entity
?       ??? PromptAnswers.cs        # Future prompt-answer relationship
?
??? Application/
?   ??? Repository/
?   ?   ??? IRepository.cs          # Generic repository interface
?   ?   ??? IEntriesRepository.cs   # Entry-specific repository
?   ?   ??? IPromptRepository.cs    # Prompt-specific repository
?   ?   ??? IJournalRepository.cs   # Journal-specific repository
?   ??? Services/
?       ??? JournalService.cs       # Journal business logic
?       ??? EntriesService.cs       # Entry business logic
?       ??? PromptService.cs        # Prompt business logic
?       ??? JournalExportService.cs # Text export functionality
?       ??? DocumentExportService.cs # PDF export functionality
?
??? Infrastructure/
?   ??? Data/
?   ?   ??? ApplicationDbContext.cs         # EF Core DbContext
?   ?   ??? ApplicationDbContextFactory.cs  # Design-time factory
?   ?   ??? DbInitializer.cs               # Database seeding
?   ?   ??? Migrations/                     # EF Core migrations
?   ??? Repository/
?       ??? Repository.cs           # Generic repository implementation
?       ??? EntriesRepository.cs    # Entry repository implementation
?       ??? PromptRepository.cs     # Prompt repository implementation
?       ??? JournalRepository.cs    # Journal repository implementation
?
??? Web/
    ??? Controllers/
    ?   ??? HomeController.cs       # Home page controller
    ?   ??? JournalController.cs    # Journal CRUD operations
    ?   ??? EntriesController.cs    # Entry CRUD operations
    ?   ??? PromptController.cs     # Prompt CRUD operations
    ?   ??? ErrorController.cs      # Error handling
    ??? Views/
    ?   ??? Home/                   # Home page views
    ?   ??? Journal/                # Journal views (Index, Create, Edit, Details)
    ?   ??? Entries/                # Entry views (Create, Edit, Details, List)
    ?   ??? Prompt/                 # Prompt views (Index, Create, Edit, Details)
    ?   ??? Shared/                 # Shared layouts and partials
    ?   ??? Error/                  # Error pages
    ??? Areas/
    ?   ??? Identity/               # Identity scaffolded pages
    ??? ViewModels/                 # View models for complex views
    ??? wwwroot/                    # Static files (CSS, JS, images)
    ??? Program.cs                  # Application entry point and configuration
    ??? appsettings.json            # Application configuration
```

## Usage

### Creating Journals

1. Navigate to **Journals** from the main menu
2. Click **New Journal**
3. Select a month and year
4. Submit to create the journal
5. The journal will appear in your journals list

### Managing Prompts

1. Navigate to **Prompts** from the main menu
2. Click **New Prompt** to create a writing prompt
3. Enter the prompt text (e.g., "What are you grateful for today?")
4. Toggle the **Active** checkbox to make it available for entries
5. Active prompts appear in the dropdown when creating entries
6. Click the status icon to quickly toggle active/inactive state

### Writing Entries

1. Navigate to a specific journal from the **Journals** page
2. Click **Add Entry**
3. Select a date for the entry
4. Choose a prompt from the dropdown
5. Write your entry content
6. Submit to save the entry
7. Entries are displayed chronologically with their prompts

### Exporting Journals

#### PDF Export

1. On the Journals page, click the three-dot menu on any journal card
2. Select **PDF** to download a professionally formatted PDF
3. The PDF includes:
   - Journal title (Month, Year)
   - All entries with dates and prompts
   - Formatted content with proper spacing
   - Page numbers

#### Text Export (via service)

The `JournalExportService` provides text export functionality that can be integrated into the UI.

## Authentication & Authorization

### Roles

- **Admin**: Full access to all journals, entries, and prompts
- Future: **User** role for personal journals (not yet implemented)

### Security Features

- Password requirements (uppercase, lowercase, number, special character)
- Email confirmation (optional, configurable)
- Account lockout after failed login attempts
- Two-factor authentication support
- Secure password recovery

### Authorization

All journal operations require the **Admin** role:

```csharp
[Authorize(Roles = "Admin")]
```

## Database Schema

### Tables

#### Journals

- `Id` (PK, int) - Primary key
- `Month` (int) - Month number (1-12)
- `Year` (int) - Four-digit year
- `CreatedAt` (datetime) - Creation timestamp

#### JournalEntries

- `Id` (PK, int) - Primary key
- `JournalId` (FK, int) - References Journals.Id
- `PromptId` (FK, int) - References Prompts.Id
- `EntryDate` (datetime) - Date of the entry
- `Content` (nvarchar(max)) - Entry content
- `CreatedAt` (datetime) - Creation timestamp
- `UpdatedAt` (datetime, nullable) - Last update timestamp

**Indexes**:

- Composite index on `(JournalId, PromptId, EntryDate)` for efficient querying

#### Prompts

- `Id` (PK, int) - Primary key
- `Text` (nvarchar(500)) - Prompt text
- `IsActive` (bit) - Whether prompt is available for new entries
- `CreatedAt` (datetime) - Creation timestamp

#### AspNetUsers, AspNetRoles, etc.

Standard ASP.NET Core Identity tables for authentication and authorization.

### Relationships

- Journal ? JournalEntries (One-to-Many, Cascade Delete)
- Prompt ? JournalEntries (One-to-Many, Restrict Delete)

## Development

### Running Locally

1. **Set the startup project**

   ```bash
   cd Web
   dotnet run
   ```

2. **Access the application**

   - HTTPS: `https://localhost:5001`
   - HTTP: `http://localhost:5000`

3. **Login with default admin account**
   - Email: asanderson1994s@gmail.com
   - Password: Pass1234!

### Building the Solution

```bash
# Build all projects
dotnet build

# Build in Release mode
dotnet build -c Release

# Clean solution
dotnet clean
```

### Migrations

#### Create a new migration

```bash
cd Infrastructure
dotnet ef migrations add MigrationName --startup-project ../Web
```

#### Apply migrations

```bash
cd Infrastructure
dotnet ef database update --startup-project ../Web
```

#### Remove last migration (if not applied)

```bash
cd Infrastructure
dotnet ef migrations remove --startup-project ../Web
```

#### Generate SQL script

```bash
cd Infrastructure
dotnet ef migrations script --startup-project ../Web -o migration.sql
```

### Testing

While no test projects are currently in the solution, the architecture supports easy testing:

```bash
# Run tests (when implemented)
dotnet test
```

**Recommended test structure**:

- `Domain.Tests` - Unit tests for domain entities
- `Application.Tests` - Unit tests for services and business logic
- `Infrastructure.Tests` - Integration tests for repositories and database
- `Web.Tests` - Integration tests for controllers and views

## Contributing

Contributions are welcome! Please follow these guidelines:

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/amazing-feature
   ```
3. **Commit your changes**
   ```bash
   git commit -m 'Add some amazing feature'
   ```
4. **Push to the branch**
   ```bash
   git push origin feature/amazing-feature
   ```
5. **Open a Pull Request**

### Coding Standards

- Follow C# coding conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Write unit tests for new features
- Ensure all tests pass before submitting PR
- Keep commits atomic and well-described

### Areas for Contribution

- Unit and integration tests
- Additional export formats (Word, Markdown)
- User-specific journals (multi-tenancy)
- Rich text editor for entries
- Search and filtering functionality
- Tags and categories
- Journal templates
- Mobile app (API development)
- Localization/internationalization

## License

This project is currently unlicensed. Please contact the repository owner for licensing information.

## Acknowledgments

- **ASP.NET Core Team** - For the excellent web framework
- **QuestPDF** - For the powerful PDF generation library
- **Bootstrap** - For the responsive UI components
- **Entity Framework Core** - For the robust ORM

## Support

For issues, questions, or suggestions:

- Open an issue on [GitHub](https://github.com/Vexelior/Journal/issues)
- Contact the maintainer at asanderson1994s@gmail.com

## Roadmap

### Version 2.0 (Planned)

- [ ] User-specific journals (multi-user support)
- [x] Rich text editor with formatting
- [x] Image attachments in entries
- [ ] Search and filter functionality
- [ ] Tags and categories
- [ ] Journal sharing capabilities
- [ ] Mobile-responsive improvements

### Version 2.1 (Planned)

- [ ] REST API for mobile apps
- [ ] Entry reminders/notifications
- [ ] Journal statistics and insights
- [ ] Export to additional formats (Word, Markdown)
- [ ] Journal templates
- [ ] Mood tracking integration

### Version 3.0 (Future)

- [ ] AI-powered writing suggestions
- [ ] Voice-to-text entry creation

---

**Last Updated**: December 2025
**Version**: 1.0.0  
**Framework**: .NET 10.0
