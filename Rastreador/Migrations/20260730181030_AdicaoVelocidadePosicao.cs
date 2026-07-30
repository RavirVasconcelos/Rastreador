using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class AdicaoVelocidadePosicao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DirecaoGraus",
                table: "PosicaoVeiculos",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VelocidadeKmH",
                table: "PosicaoVeiculos",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirecaoGraus",
                table: "PosicaoVeiculos");

            migrationBuilder.DropColumn(
                name: "VelocidadeKmH",
                table: "PosicaoVeiculos");
        }
    }
}
