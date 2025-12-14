# Music Class Registration System

[![Build and Deploy](https://github.com/huzaifah/musicschool/actions/workflows/azure-deploy.yml/badge.svg)](https://github.com/huzaifah/musicschool/actions/workflows/azure-deploy.yml)

A Blazor Web App built with .NET 10 that allows the public to register for one-to-one music classes with professional instructors.

## Features

### Public (Student) Features
- **Browse Available Classes**: View all available music classes filtered by instrument and skill level
- **View Instructors**: Browse profiles of all active music instructors with their specializations and rates
- **Book Classes**: Register for classes by providing name, email, and phone number (no account required)
- **Booking Confirmation**: Receive immediate confirmation with all class and booking details

### Instructor Features
- **My Schedule**: View all scheduled classes (both available and booked)
- **My Bookings**: See all student bookings with contact information and notes

### Admin Features
- **Dashboard**: Overview of system statistics (instructors, classes, bookings, revenue)
- **Manage Instructors**: Full CRUD operations for instructor records
- **Manage Classes**: Create, edit, and delete class schedules
- **View All Bookings**: Monitor all bookings with search and cancellation capabilities

## Live Demo

🚀 **Live Application**: [https://nadiritma.azurewebsites.net](https://nadiritma.azurewebsites.net)

The application is automatically deployed to Azure App Service via GitHub Actions on every push to the main branch.

## Technology Stack

- **Framework**: .NET 10 Blazor Web App
- **Database**: Entity Framework Core with In-Memory provider
- **Architecture**: Service layer pattern with dependency injection
- **UI**: Bootstrap 5 with responsive design

## Project Structure

```
BlazorApp/
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   ├── NavMenu.razor
│   │   └── ViewSelector.razor
│   └── Pages/
│       ├── Public/          # Student-facing pages
│       ├── Instructor/      # Instructor portal
│       └── Admin/           # Admin dashboard
├── Data/
│   ├── Entities/           # Domain models
│   ├── Enums/              # Status enums
│   ├── ApplicationDbContext.cs
│   └── DbSeeder.cs         # Sample data
├── Services/
│   ├── Interfaces/
│   └── Implementation/
├── Models/
│   └── DTOs/
└── Program.cs
```

## Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or later

### Running the Application

1. **Navigate to the project directory**:
   ```bash
   cd /Users/huzaifahdzulkifli/BlazorApp/BlazorApp
   ```

2. **Build the project**:
   ```bash
   dotnet build
   ```

3. **Run the application**:
   ```bash
   dotnet run
   ```

4. **Open your browser** and navigate to:
   - HTTPS: `https://localhost:5001`
   - HTTP: `http://localhost:5000`

## How to Use

### Switching Between Views

The application uses a **View Mode Selector** at the top of the page to switch between different user perspectives:

1. **Student (Public) Mode**: Default view for browsing and booking classes
2. **Instructor Mode**: Select an instructor from the dropdown to view their schedule and bookings
3. **Admin Mode**: Access administrative functions for managing the entire system

> **Note**: Since there's no authentication, the view mode selector allows you to easily switch between different user roles for demonstration purposes.

### Public User Flow

1. Click "Browse Classes" from the home page
2. Use filters to find classes by instrument or skill level
3. Click "Book Now" on a desired class
4. Fill in your contact information
5. Submit to receive instant confirmation

### Instructor Flow

1. Switch to "Instructor" mode using the View Mode Selector
2. Select your instructor profile from the dropdown
3. Navigate to "My Schedule" to see all your classes
4. Navigate to "My Bookings" to view student bookings

### Admin Flow

1. Switch to "Admin" mode using the View Mode Selector
2. Access the Dashboard for system overview
3. Manage instructors, classes, and bookings through the admin menu

## Seeded Data

The application comes pre-loaded with sample data:

### Instructors (5)
- Sarah Johnson - Piano, Music Theory ($75/hr)
- Michael Chen - Guitar, Bass Guitar ($65/hr)
- Emma Rodriguez - Violin, Cello ($80/hr)
- David Thompson - Drums, Percussion ($70/hr)
- Lisa Martinez - Saxophone, Flute ($72/hr)

### Instruments (7)
- Piano, Guitar, Violin, Drums, Saxophone, Flute, Cello

### Classes
- 100 classes distributed across 30 days
- Various time slots (morning, afternoon, evening)
- Mix of skill levels (Beginner, Intermediate, Advanced)

### Sample Bookings
- 6 confirmed bookings with different students

> **Note**: Data is stored in-memory and will reset when the application restarts.

## Data Model

### Core Entities

**Instructor**
- Name, Email, Phone, Bio
- Specialization, Hourly Rate
- Active status

**MusicClass**
- Instructor reference
- Instrument, Skill Level
- Scheduled DateTime, Duration
- Price, Description, Status

**Booking**
- MusicClass reference
- Student Name, Email, Phone
- Notes, Booked DateTime, Status

**Relationships**
- One Instructor → Many MusicClasses
- One MusicClass → One Booking (optional)

## Database

The application uses **Entity Framework Core In-Memory Database** for development and demonstration purposes.

**Advantages**:
- Fast setup and testing
- No external dependencies
- Perfect for prototypes

**Limitations**:
- Data resets on application restart
- Not suitable for production

**Migration to Production DB**:
To use a real database (SQL Server, PostgreSQL, etc.):
1. Install the appropriate EF Core provider package
2. Update `Program.cs` to use the new provider:
   ```csharp
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(connectionString));
   ```
3. Run migrations: `dotnet ef migrations add InitialCreate`
4. Update database: `dotnet ef database update`

## Features Implemented

✅ Browse and filter available music classes
✅ View instructor profiles
✅ Book classes with form validation
✅ Booking confirmation page
✅ Instructor schedule view
✅ Instructor bookings management
✅ Admin dashboard with statistics
✅ Full CRUD for instructors
✅ Full CRUD for class schedules
✅ View and manage all bookings
✅ Responsive design (mobile-friendly)
✅ View mode switching (no authentication required)

## Future Enhancements

Potential improvements for production:
- [ ] User authentication (ASP.NET Identity)
- [ ] Email notifications for bookings
- [ ] Payment integration
- [ ] Calendar view for schedules
- [ ] Recurring class schedules
- [ ] Student booking history
- [ ] Instructor availability management
- [ ] Waitlist for popular classes
- [ ] Reviews and ratings
- [ ] File upload for instructor photos

## Deployment

### Current Deployment
- **Platform**: Azure App Service (Free Tier)
- **URL**: https://nadiritma.azurewebsites.net
- **CI/CD**: GitHub Actions
- **Auto-deploy**: Enabled on push to main branch

### GitHub Actions Workflow
The application uses automated CI/CD pipeline:
- **Build**: Restore, build, test, and publish .NET 10 app
- **Deploy**: Deploy to Azure App Service using publish profile
- **Trigger**: Push to main branch or manual dispatch

View workflow runs: [GitHub Actions](https://github.com/huzaifah/musicschool/actions)

### Important Notes
- **Database**: In-memory database (data resets on deployment/restart)
- **Free Tier**: 60 min/day compute, may sleep after inactivity
- **Cost**: $0/month

## License

This project is created for demonstration purposes.

## Support

For questions or issues, please refer to the [.NET Documentation](https://docs.microsoft.com/dotnet/) or [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor/).
