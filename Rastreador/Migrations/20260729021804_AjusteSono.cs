using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class AjusteSono : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "SonoRastreadors",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "SonoRastreadors",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "SonoRastreadors");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "SonoRastreadors");
        }
    }
}
