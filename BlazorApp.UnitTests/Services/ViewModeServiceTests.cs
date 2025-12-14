using BlazorApp.Models;
using BlazorApp.Services.Implementation;
using FluentAssertions;
using Xunit;

namespace BlazorApp.UnitTests.Services;

public class ViewModeServiceTests
{
    [Fact]
    public void Constructor_ShouldSetDefaultModeToPublic()
    {
        // Arrange & Act
        var service = new ViewModeService();

        // Assert
        service.CurrentMode.Should().Be(ViewMode.Public);
    }

    [Fact]
    public void CurrentInstructorId_ShouldBeNull_Initially()
    {
        // Arrange & Act
        var service = new ViewModeService();

        // Assert
        service.CurrentInstructorId.Should().BeNull();
    }

    [Fact]
    public void SetViewMode_ShouldUpdateCurrentMode()
    {
        // Arrange
        var service = new ViewModeService();

        // Act
        service.SetViewMode(ViewMode.Admin);

        // Assert
        service.CurrentMode.Should().Be(ViewMode.Admin);
    }

    [Fact]
    public void SetViewMode_ShouldTriggerOnChangeEvent()
    {
        // Arrange
        var service = new ViewModeService();
        var eventTriggered = false;
        service.OnChange += () => eventTriggered = true;

        // Act
        service.SetViewMode(ViewMode.Admin);

        // Assert
        eventTriggered.Should().BeTrue();
    }

    [Fact]
    public void SetViewMode_ShouldNotTriggerEvent_WhenModeIsUnchanged()
    {
        // Arrange
        var service = new ViewModeService();
        var eventTriggerCount = 0;
        service.OnChange += () => eventTriggerCount++;

        // Act
        service.SetViewMode(ViewMode.Public); // Already Public
        service.SetViewMode(ViewMode.Public); // Still Public

        // Assert
        eventTriggerCount.Should().Be(0);
    }

    [Fact]
    public void SetViewMode_ShouldClearInstructorId_WhenLeavingInstructorMode()
    {
        // Arrange
        var service = new ViewModeService();
        service.SetViewMode(ViewMode.Instructor);
        service.CurrentInstructorId = 5;

        // Act
        service.SetViewMode(ViewMode.Admin);

        // Assert
        service.CurrentInstructorId.Should().BeNull();
    }

    [Theory]
    [InlineData(ViewMode.Public)]
    [InlineData(ViewMode.Admin)]
    public void SetViewMode_ShouldClearInstructorId_WhenNotInstructorMode(ViewMode mode)
    {
        // Arrange
        var service = new ViewModeService();
        service.SetViewMode(ViewMode.Instructor);
        service.CurrentInstructorId = 10;

        // Act
        service.SetViewMode(mode);

        // Assert
        service.CurrentInstructorId.Should().BeNull();
    }

    [Fact]
    public void SetViewMode_ShouldNotClearInstructorId_WhenInInstructorMode()
    {
        // Arrange
        var service = new ViewModeService();
        service.SetViewMode(ViewMode.Instructor);
        service.CurrentInstructorId = 7;

        // Act
        service.SetViewMode(ViewMode.Instructor); // Same mode

        // Assert
        service.CurrentInstructorId.Should().Be(7);
    }

    [Fact]
    public void CurrentInstructorId_CanBeSet()
    {
        // Arrange
        var service = new ViewModeService();

        // Act
        service.CurrentInstructorId = 3;

        // Assert
        service.CurrentInstructorId.Should().Be(3);
    }

    [Fact]
    public void OnChangeEvent_ShouldBeInvoked_WhenModeChanges()
    {
        // Arrange
        var service = new ViewModeService();
        var invokeCount = 0;
        service.OnChange += () => invokeCount++;

        // Act
        service.SetViewMode(ViewMode.Instructor);
        service.SetViewMode(ViewMode.Admin);
        service.SetViewMode(ViewMode.Public);

        // Assert
        invokeCount.Should().Be(3);
    }

    [Fact]
    public void OnChangeEvent_ShouldNotBeInvoked_WhenModeIsSame()
    {
        // Arrange
        var service = new ViewModeService();
        service.SetViewMode(ViewMode.Admin);
        var invokeCount = 0;
        service.OnChange += () => invokeCount++;

        // Act
        service.SetViewMode(ViewMode.Admin);

        // Assert
        invokeCount.Should().Be(0);
    }

    [Fact]
    public void CurrentInstructorId_ShouldPersist_WhenStayingInInstructorMode()
    {
        // Arrange
        var service = new ViewModeService();
        service.SetViewMode(ViewMode.Instructor);

        // Act
        service.CurrentInstructorId = 15;

        // Assert
        service.CurrentInstructorId.Should().Be(15);
        service.CurrentMode.Should().Be(ViewMode.Instructor);
    }
}
