using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class InitialTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteId);
                });

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    VeiculoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MarcaModelo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoVeiculo = table.Column<int>(type: "int", nullable: true),
                    AnoVeiculo = table.Column<int>(type: "int", nullable: true),
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veiculos", x => x.VeiculoId);
                    table.ForeignKey(
                        name: "FK_Veiculos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Clientes",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ancoragems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    RaioMetros = table.Column<double>(type: "float", nullable: false),
                    Ativa = table.Column<bool>(type: "bit", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: true),
                    HoraFim = table.Column<TimeOnly>(type: "time", nullable: true),
                    VeiculoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ancoragems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ancoragems_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "VeiculoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rastreadors",
                columns: table => new
                {
                    RastreadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identificador = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Imei = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroChip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    ModoEconomia = table.Column<bool>(type: "bit", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VeiculoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rastreadors", x => x.RastreadorId);
                    table.ForeignKey(
                        name: "FK_Rastreadors_Veiculos_VeiculoId",
                        column: x => x.VeiculoId,
                        principalTable: "Veiculos",
                        principalColumn: "VeiculoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PosicaoVeiculos",
                columns: table => new
                {
                    PosicaoVeiculoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RastreadorModuloId = table.Column<int>(type: "int", nullable: false),
                    Ancorado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosicaoVeiculos", x => x.PosicaoVeiculoId);
                    table.ForeignKey(
                        name: "FK_PosicaoVeiculos_Rastreadors_RastreadorModuloId",
                        column: x => x.RastreadorModuloId,
                        principalTable: "Rastreadors",
                        principalColumn: "RastreadorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ancoragems_VeiculoId",
                table: "Ancoragems",
                column: "VeiculoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosicaoVeiculos_RastreadorModuloId",
                table: "PosicaoVeiculos",
                column: "RastreadorModuloId");

            migrationBuilder.CreateIndex(
                name: "IX_Rastreadors_VeiculoId",
                table: "Rastreadors",
                column: "VeiculoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_ClienteId",
                table: "Veiculos",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ancoragems");

            migrationBuilder.DropTable(
                name: "PosicaoVeiculos");

            migrationBuilder.DropTable(
                name: "Rastreadors");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "Clientes");
        }
    }
}
