using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class AjustePosicao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PosicaoVeiculos_RastreadorModuloId",
                table: "PosicaoVeiculos");

            migrationBuilder.DropColumn(
                name: "Ancorado",
                table: "PosicaoVeiculos");

            migrationBuilder.CreateIndex(
                name: "IX_PosicaoVeiculos_RastreadorModuloId",
                table: "PosicaoVeiculos",
                column: "RastreadorModuloId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PosicaoVeiculos_RastreadorModuloId",
                table: "PosicaoVeiculos");

            migrationBuilder.AddColumn<bool>(
                name: "Ancorado",
                table: "PosicaoVeiculos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_PosicaoVeiculos_RastreadorModuloId",
                table: "PosicaoVeiculos",
                column: "RastreadorModuloId");
        }
    }
}
