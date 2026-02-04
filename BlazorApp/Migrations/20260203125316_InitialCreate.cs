using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StudentName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SchoolYear = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    MusicExperience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuardianName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GuardianPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    GuardianEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmergencyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmergencyPhone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    EmergencyRelationship = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassSlot = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AgreeTerms = table.Column<bool>(type: "bit", nullable: false),
                    AgreeParticipation = table.Column<bool>(type: "bit", nullable: false),
                    AgreeMedia = table.Column<bool>(type: "bit", nullable: false),
                    SignatoryName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_ReferenceNumber",
                table: "Registrations",
                column: "ReferenceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Registrations");
        }
    }
}
