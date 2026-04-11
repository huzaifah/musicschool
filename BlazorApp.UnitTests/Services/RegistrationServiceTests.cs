using BlazorApp.Data.Enums;
using BlazorApp.Models;
using BlazorApp.Services.Implementation;
using BlazorApp.UnitTests.Helpers;
using FluentAssertions;

namespace BlazorApp.UnitTests.Services;

public class RegistrationServiceTests
{
    #region Venue Tests

    [Fact]
    public async Task CreateRegistrationAsync_ShouldPreserveVenue_WhenSriAlAminSelected()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);
        var model = TestDataBuilder.CreateTestRegistrationFormModel(
            selectedVenue: Venue.SriAlAminCherasSelatan,
            classSlot: "Rabu, 3:00 ptg - 3:30 ptg");

        // Act
        var result = await service.CreateRegistrationAsync(model);

        // Assert
        result.Venue.Should().Be(Venue.SriAlAminCherasSelatan);
        result.ClassSlot.Should().Be("Rabu, 3:00 ptg - 3:30 ptg");
    }

    [Fact]
    public async Task CreateRegistrationAsync_ShouldSetClassSlotNull_WhenPrimaSaujanaSelected()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);
        var model = TestDataBuilder.CreateTestRegistrationFormModel(
            selectedVenue: Venue.PrimaSaujanaKajang,
            classSlot: "Some slot"); // This should be ignored

        // Act
        var result = await service.CreateRegistrationAsync(model);

        // Assert
        result.Venue.Should().Be(Venue.PrimaSaujanaKajang);
        result.ClassSlot.Should().BeNull(); // No slot selection for Prima Saujana
    }

    [Fact]
    public async Task CreateRegistrationAsync_ShouldPreserveClassSlot_WhenVenueHasSlotSelection()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);
        var model = TestDataBuilder.CreateTestRegistrationFormModel(
            selectedVenue: Venue.SriAlAminCherasSelatan,
            classSlot: "Sabtu, 12:00 tghari - 12:30 tghari");

        // Act
        var result = await service.CreateRegistrationAsync(model);

        // Assert
        result.ClassSlot.Should().Be("Sabtu, 12:00 tghari - 12:30 tghari");
    }

    #endregion
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
        result.Venue.Should().Be(model.SelectedVenue!.Value);
        result.ClassSlot.Should().Be(model.ClassSlot);
        result.Status.Should().Be(RegistrationStatus.Pending);
        result.ReferenceNumber.Should().StartWith("NR-");
        result.ReferenceNumber.Should().HaveLength(11); // NR- + 8 chars
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
            referenceNumber: "NR-ABC12345");
        using var context = MockDbContextFactory.CreateMockContextWithData(
            registrations: [registration]);
        var service = new RegistrationService(context);

        // Act
        var result = await service.GetByReferenceAsync("NR-ABC12345");

        // Assert
        result.Should().NotBeNull();
        result!.StudentName.Should().Be(registration.StudentName);
        result.ReferenceNumber.Should().Be("NR-ABC12345");
    }

    [Fact]
    public async Task GetByReferenceAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);

        // Act
        var result = await service.GetByReferenceAsync("NR-NOTEXIST");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GenerateReferenceNumberAsync_ShouldGenerateCorrectFormat()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);

        // Act
        var result = await service.GenerateReferenceNumberAsync();

        // Assert
        result.Should().StartWith("NR-");
        result.Should().HaveLength(11); // NR- + 8 chars
        result[3..].Should().MatchRegex("^[A-Z2-9]{8}$"); // Only allowed chars
    }

    [Fact]
    public async Task GenerateReferenceNumberAsync_ShouldGenerateUniqueNumbers()
    {
        // Arrange
        using var context = MockDbContextFactory.CreateMockContext();
        var service = new RegistrationService(context);

        // Act
        var references = new HashSet<string>();
        for (int i = 0; i < 100; i++)
        {
            var reference = await service.GenerateReferenceNumberAsync();
            references.Add(reference);
        }

        // Assert - all 100 should be unique
        references.Should().HaveCount(100);
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
