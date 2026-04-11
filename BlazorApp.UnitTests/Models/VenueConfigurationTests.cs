using BlazorApp.Data.Enums;
using BlazorApp.Models;
using FluentAssertions;

namespace BlazorApp.UnitTests.Models;

public class VenueConfigurationTests
{
    [Fact]
    public void GetVenue_ShouldReturnConfiguration_WhenSriAlAminSelected()
    {
        // Act
        var result = VenueConfiguration.GetVenue(Venue.SriAlAminCherasSelatan);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("SRI AL-AMIN Cheras Selatan");
        result.ShortName.Should().Be("Cheras Selatan");
        result.HasSlotSelection.Should().BeTrue();
        result.AvailableSlots.Should().HaveCount(2);
    }

    [Fact]
    public void GetVenue_ShouldReturnConfiguration_WhenPrimaSaujanaSelected()
    {
        // Act
        var result = VenueConfiguration.GetVenue(Venue.PrimaSaujanaKajang);

        // Assert
        result.Should().NotBeNull();
        result!.Name.Should().Be("Prima Saujana Kajang");
        result.ShortName.Should().Be("Kajang");
        result.HasSlotSelection.Should().BeFalse();
        result.AvailableSlots.Should().BeEmpty();
    }

    [Fact]
    public void AllVenues_ShouldContainBothVenues()
    {
        // Assert
        VenueConfiguration.AllVenues.Should().HaveCount(2);
        VenueConfiguration.AllVenues.Should().Contain(v => v.Venue == Venue.SriAlAminCherasSelatan);
        VenueConfiguration.AllVenues.Should().Contain(v => v.Venue == Venue.PrimaSaujanaKajang);
    }

    [Fact]
    public void SriAlAmin_ShouldHaveCorrectSlots()
    {
        // Arrange
        var venue = VenueConfiguration.GetVenue(Venue.SriAlAminCherasSelatan);

        // Assert
        venue.Should().NotBeNull();
        venue!.AvailableSlots.Should().Contain(s => s.Day == "Rabu");
        venue.AvailableSlots.Should().Contain(s => s.Day == "Sabtu");
    }

    [Fact]
    public void PrimaSaujana_ScheduleDisplay_ShouldIndicateTBD()
    {
        // Arrange
        var venue = VenueConfiguration.GetVenue(Venue.PrimaSaujanaKajang);

        // Assert
        venue.Should().NotBeNull();
        venue!.ScheduleDisplay.Should().Contain("dimaklumkan");
    }

    [Theory]
    [InlineData(Venue.SriAlAminCherasSelatan, true)]
    [InlineData(Venue.PrimaSaujanaKajang, false)]
    public void HasSlotSelection_ShouldReturnCorrectValue(Venue venue, bool expectedHasSlotSelection)
    {
        // Act
        var config = VenueConfiguration.GetVenue(venue);

        // Assert
        config.Should().NotBeNull();
        config!.HasSlotSelection.Should().Be(expectedHasSlotSelection);
    }
}
