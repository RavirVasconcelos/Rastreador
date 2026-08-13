using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class IniciarSqlServer : Migration
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
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteId);
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
                });

            migrationBuilder.CreateTable(
                name: "PosicaoVeiculos",
                columns: table => new
                {
                    PosicaoVeiculoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    VelocidadeKmH = table.Column<double>(type: "float", nullable: false),
                    DirecaoGraus = table.Column<double>(type: "float", nullable: true),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    DataHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RastreadorModuloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosicaoVeiculos", x => x.PosicaoVeiculoId);
                });

            migrationBuilder.CreateTable(
                name: "Rastreadors",
                columns: table => new
                {
                    RastreadorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Identificador = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Imei = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroChip = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    ModoEconomia = table.Column<bool>(type: "bit", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VeiculoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rastreadors", x => x.RastreadorId);
                });

            migrationBuilder.CreateTable(
                name: "SonoRastreadors",
                columns: table => new
                {
                    SonoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    HoraFim = table.Column<TimeOnly>(type: "time", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    RaioPerimetro = table.Column<double>(type: "float", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    RastreadorId = table.Column<int>(type: "int", nullable: false)
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
                    CpfProprietario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClienteId = table.Column<int>(type: "int", nullable: false),
                    SonoRastreadorSonoId = table.Column<int>(type: "int", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Veiculos_SonoRastreadors_SonoRastreadorSonoId",
                        column: x => x.SonoRastreadorSonoId,
                        principalTable: "SonoRastreadors",
                        principalColumn: "SonoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ancoragems_VeiculoId",
                table: "Ancoragems",
                column: "VeiculoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PosicaoVeiculos_RastreadorModuloId",
                table: "PosicaoVeiculos",
                column: "RastreadorModuloId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rastreadors_VeiculoId",
                table: "Rastreadors",
                column: "VeiculoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SonoRastreadors_RastreadorId",
                table: "SonoRastreadors",
                column: "RastreadorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_ClienteId",
                table: "Veiculos",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Veiculos_SonoRastreadorSonoId",
                table: "Veiculos",
                column: "SonoRastreadorSonoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ancoragems_Veiculos_VeiculoId",
                table: "Ancoragems",
                column: "VeiculoId",
                principalTable: "Veiculos",
                principalColumn: "VeiculoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PosicaoVeiculos_Rastreadors_RastreadorModuloId",
                table: "PosicaoVeiculos",
                column: "RastreadorModuloId",
                principalTable: "Rastreadors",
                principalColumn: "RastreadorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rastreadors_Veiculos_VeiculoId",
                table: "Rastreadors",
                column: "VeiculoId",
                principalTable: "Veiculos",
                principalColumn: "VeiculoId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rastreadors_Veiculos_VeiculoId",
                table: "Rastreadors");

            migrationBuilder.DropTable(
                name: "Ancoragems");

            migrationBuilder.DropTable(
                name: "PosicaoVeiculos");

            migrationBuilder.DropTable(
                name: "Veiculos");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "SonoRastreadors");

            migrationBuilder.DropTable(
                name: "Rastreadors");
        }
    }
}
