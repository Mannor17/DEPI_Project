using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depi_Project.Migrations
{
    /// <inheritdoc />
    public partial class addpricetogym : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePerDay",
                table: "Gyms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerMonth",
                table: "Gyms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceWithTrainer",
                table: "Gyms",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerDay",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "PricePerMonth",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "PriceWithTrainer",
                table: "Gyms");
        }
    }
}
