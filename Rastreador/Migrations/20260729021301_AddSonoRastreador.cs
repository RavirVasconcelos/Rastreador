using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class AddSonoRastreador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SonoRastreadorSonoId",
                table: "Veiculos",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SonoRastreadors",
                columns: table => new
                {
                    SonoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HoraInicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    HoraFim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RastreadorId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SonoRastreadors", x => x.SonoId);
                    table.ForeignKey(
                        name: "FK_SonoRastreadors_Rastreadors_RastreadorId",
                        column: x => x.RastreadorId,
                        principalTable: "Rastreadors",
                        principalColumn: "RastreadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_SonoRastreadorSonoId",
                table: "Veiculos",
                column: "SonoRastreadorSonoId");

            migrationBuilder.CreateIndex(
                name: "IX_SonoRastreadors_RastreadorId",
                table: "SonoRastreadors",
                column: "RastreadorId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Veiculos_SonoRastreadors_SonoRastreadorSonoId",
                table: "Veiculos",
                column: "SonoRastreadorSonoId",
                principalTable: "SonoRastreadors",
                principalColumn: "SonoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Veiculos_SonoRastreadors_SonoRastreadorSonoId",
                table: "Veiculos");

            migrationBuilder.DropTable(
                name: "SonoRastreadors");

            migrationBuilder.DropIndex(
                name: "IX_Veiculos_SonoRastreadorSonoId",
                table: "Veiculos");

            migrationBuilder.DropColumn(
                name: "SonoRastreadorSonoId",
                table: "Veiculos");
        }
    }
}
