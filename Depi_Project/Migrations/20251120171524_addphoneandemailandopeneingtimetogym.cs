using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depi_Project.Migrations
{
    /// <inheritdoc />
    public partial class addphoneandemailandopeneingtimetogym : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpeningHours",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "OpeningHours",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Gyms");
        }
    }
}
