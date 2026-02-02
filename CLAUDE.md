# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & Run Commands

```bash
# Build solution
dotnet build

# Run the application (from repo root)
dotnet run --project BlazorApp

# Run with hot reload
dotnet watch run --project BlazorApp

# Run all tests
dotnet test

# Run a specific test class
dotnet test --filter "FullyQualifiedName~BookingServiceTests"

# Run a specific test method
dotnet test --filter "FullyQualifiedName~CreateBookingAsync_ShouldCreateBooking_WhenValid"

# Generate test coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Application URLs

- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

## Architecture Overview

This is a **Blazor Web App** (.NET 10) for music class registration. It uses **Interactive Server** rendering mode with no authentication (role switching via UI for demo purposes).

### Project Structure

```
BlazorApp/                    # Main Blazor application
├── Components/
│   ├── Layout/               # MainLayout, NavMenu, ViewSelector
│   └── Pages/
│       ├── Public/           # Student-facing (BrowseClasses, BookClass, Instructors)
│       ├── Instructor/       # Instructor portal (MySchedule, MyBookings)
│       └── Admin/            # Admin dashboard (ManageInstructors, ManageClasses, AllBookings)
├── Data/
│   ├── Entities/             # Instructor, MusicClass, Booking, Instrument
│   ├── Enums/                # ClassStatus, BookingStatus, SkillLevel
│   └── ApplicationDbContext.cs
├── Services/
│   ├── Interfaces/           # Service contracts
│   └── Implementation/       # Service implementations
└── Models/DTOs/              # BookingRequest

BlazorApp.UnitTests/          # Unit tests (xUnit + FluentAssertions + Moq)
├── Helpers/                  # MockDbContextFactory, TestDataBuilder
└── Services/                 # One test class per service
```

### Service Layer Pattern

Services directly use `ApplicationDbContext` (no repository layer). All services are registered as **Scoped** in DI.

- **ViewModeService**: Manages role switching state (Public/Instructor/Admin) with `OnChange` event
- **InstructorService**: CRUD for instructors
- **ClassService**: CRUD + filtering for music classes
- **BookingService**: Booking creation with validation, cancellation

### Entity Relationships

- `Instructor` → `MusicClass` (1:N)
- `MusicClass` → `Booking` (1:0..1) - each class can only be booked once

### Data Layer

Uses **EF Core In-Memory Database** - data resets on app restart. Sample data seeded via `DbSeeder.cs`:
- 5 instructors, 7 instruments, 100 classes, 6 sample bookings

## Testing Patterns

- **Framework**: xUnit with `[Fact]` and `[Theory]`
- **Assertions**: FluentAssertions (`Should()`, `BeEquivalentTo()`)
- **Mocking**: Moq (though tests primarily use in-memory DbContext)
- **Test naming**: `MethodName_Scenario_ExpectedBehavior`
- **Database**: Each test creates unique in-memory DB via `MockDbContextFactory`
- **Test data**: `TestDataBuilder` provides factory methods

## Code Patterns

- Nullable reference types enabled (`<Nullable>enable</Nullable>`)
- File-scoped namespaces
- Async/await for all data operations
- Navigation properties use null-forgiving operator (`= null!`)
- `.Include()` / `.ThenInclude()` for eager loading
- Form validation via `EditForm` with `DataAnnotationsValidator`
