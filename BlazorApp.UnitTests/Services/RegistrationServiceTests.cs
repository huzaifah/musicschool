using BlazorApp.Data.Enums;
using BlazorApp.Services.Implementation;
using BlazorApp.UnitTests.Helpers;
using FluentAssertions;

namespace BlazorApp.UnitTests.Services;

public class RegistrationServiceTests
{
    [Fact]
    public async Task CreateRegistrationAsync_ShouldCreateRegistration_WhenValidModel()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);
        var model = TestDataBuilder.CreateTestRegistrationFormModel();

        // Act
        var result = await service.CreateRegistrationAsync(model);

        // Assert
        result.Should().NotBeNull();
        result.StudentName.Should().Be(model.StudentName);
        result.GuardianEmail.Should().Be(model.GuardianEmail);
        result.ClassSlot.Should().Be(model.ClassSlot);
        result.Status.Should().Be(RegistrationStatus.Pending);
        result.ReferenceNumber.Should().StartWith("NADIRITMA-KB-");
    }

    [Fact]
    public async Task CreateRegistrationAsync_ShouldGenerateUniqueReferenceNumber()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);
        var model1 = TestDataBuilder.CreateTestRegistrationFormModel(studentName: "Student 1");
        var model2 = TestDataBuilder.CreateTestRegistrationFormModel(studentName: "Student 2");

        // Act
        var result1 = await service.CreateRegistrationAsync(model1);
        var result2 = await service.CreateRegistrationAsync(model2);

        // Assert
        result1.ReferenceNumber.Should().NotBe(result2.ReferenceNumber);
    }

    [Fact]
    public async Task GetByReferenceAsync_ShouldReturnRegistration_WhenExists()
    {
        // Arrange
        var registration = TestDataBuilder.CreateTestRegistration(
            referenceNumber: "NADIRITMA-KB-2026-999");
        using var context = MockDbContextFactory.CreateMockContextWithData(
            registrations: [registration]);
        var service = new RegistrationService(context);

        // Act
        var result = await service.GetByReferenceAsync("NADIRITMA-KB-2026-999");

        // Assert
        result.Should().NotBeNull();
        result!.StudentName.Should().Be(registration.StudentName);
        result.ReferenceNumber.Should().Be("NADIRITMA-KB-2026-999");
    }

    [Fact]
    public async Task GetByReferenceAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);

        // Act
        var result = await service.GetByReferenceAsync("NADIRITMA-KB-2026-999");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GenerateReferenceNumberAsync_ShouldGenerateCorrectFormat()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);
        var currentYear = DateTime.UtcNow.Year;

        // Act
        var result = await service.GenerateReferenceNumberAsync();

        // Assert
        result.Should().StartWith($"NADIRITMA-KB-{currentYear}-");
        result.Should().EndWith("001");
    }

    [Fact]
    public async Task GenerateReferenceNumberAsync_ShouldIncrementNumber_WhenExistingRegistrations()
    {
        // Arrange
        var currentYear = DateTime.UtcNow.Year;
        var existingRegistration = TestDataBuilder.CreateTestRegistration(
            referenceNumber: $"NADIRITMA-KB-{currentYear}-005");
        using var context = MockDbContextFactory.CreateMockContextWithData(
            registrations: [existingRegistration]);
        var service = new RegistrationService(context);

        // Act
        var result = await service.GenerateReferenceNumberAsync();

        // Assert
        result.Should().Be($"NADIRITMA-KB-{currentYear}-006");
    }

    [Fact]
    public async Task CreateRegistrationAsync_ShouldPreserveAllFormData()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);
        var model = TestDataBuilder.CreateTestRegistrationFormModel(
            studentName: "Test Student",
            guardianName: "Test Guardian",
            guardianPhone: "012-345 6789",
            guardianEmail: "test@example.com",
            classSlot: "Ahad, 11:30 pagi");

        // Act
        var result = await service.CreateRegistrationAsync(model);

        // Assert
        result.StudentName.Should().Be("Test Student");
        result.GuardianName.Should().Be("Test Guardian");
        result.GuardianPhone.Should().Be("012-345 6789");
        result.GuardianEmail.Should().Be("test@example.com");
        result.ClassSlot.Should().Be("Ahad, 11:30 pagi");
        result.Address.Should().Be("123 Jalan Test");
        result.Postcode.Should().Be("40000");
        result.City.Should().Be("Shah Alam");
        result.State.Should().Be("Selangor");
        result.EmergencyName.Should().Be("Fatimah binti Ahmad");
        result.AgreeTerms.Should().BeTrue();
        result.AgreeParticipation.Should().BeTrue();
        result.AgreeMedia.Should().BeTrue();
    }
}
