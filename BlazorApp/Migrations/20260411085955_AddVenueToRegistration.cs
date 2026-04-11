using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class AddVenueToRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClassSlot",
                table: "Registrations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Venue",
                table: "Registrations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "SriAlAminCherasSelatan");

            // Update existing records to have SriAlAminCherasSelatan as venue
            migrationBuilder.Sql("UPDATE Registrations SET Venue = 'SriAlAminCherasSelatan' WHERE Venue = '' OR Venue IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Venue",
                table: "Registrations");

            migrationBuilder.AlterColumn<string>(
                name: "ClassSlot",
                table: "Registrations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
