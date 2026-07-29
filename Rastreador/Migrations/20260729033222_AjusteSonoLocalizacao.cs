using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class AjusteSonoLocalizacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RaioPerimetro",
                table: "SonoRastreadors",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RaioPerimetro",
                table: "SonoRastreadors");
        }
    }
}
