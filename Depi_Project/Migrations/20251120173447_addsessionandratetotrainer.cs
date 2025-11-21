using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Depi_Project.Migrations
{
    /// <inheritdoc />
    public partial class addsessionandratetotrainer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Trainers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SessionsCount",
                table: "Trainers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "SessionsCount",
                table: "Trainers");
        }
    }
}
