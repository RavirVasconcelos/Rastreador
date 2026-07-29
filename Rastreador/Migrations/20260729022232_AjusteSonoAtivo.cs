using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class AjusteSonoAtivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "SonoRastreadors",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "SonoRastreadors");
        }
    }
}
