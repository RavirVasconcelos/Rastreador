using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Senha = table.Column<string>(type: "text", nullable: false),
                    Cpf = table.Column<string>(type: "text", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.ClienteId);
                });

            migrationBuilder.CreateTable(
                name: "Ancoragems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    RaioMetros = table.Column<double>(type: "double precision", nullable: false),
                    Ativa = table.Column<bool>(type: "boolean", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    HoraFim = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    VeiculoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ancoragems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PosicaoVeiculos",
                columns: table => new
                {
                    PosicaoVeiculoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    VelocidadeKmH = table.Column<double>(type: "double precision", nullable: false),
                    DirecaoGraus = table.Column<double>(type: "double precision", nullable: true),
                    Estado = table.Column<int>(type: "integer", nullable: false),
                    DataHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RastreadorModuloId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosicaoVeiculos", x => x.PosicaoVeiculoId);
                });

            migrationBuilder.CreateTable(
                name: "Rastreadors",
                columns: table => new
                {
                    RastreadorId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Identificador = table.Column<Guid>(type: "uuid", nullable: false),
                    Imei = table.Column<string>(type: "text", nullable: false),
                    NumeroChip = table.Column<string>(type: "text", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    ModoEconomia = table.Column<bool>(type: "boolean", nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    VeiculoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rastreadors", x => x.RastreadorId);
                });

            migrationBuilder.CreateTable(
                name: "SonoRastreadors",
                columns: table => new
                {
                    SonoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HoraInicio = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    HoraFim = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    RaioPerimetro = table.Column<double>(type: "double precision", nullable: false),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Veiculos",
                columns: table => new
                {
                    VeiculoId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Placa = table.Column<string>(type: "text", nullable: true),
                    MarcaModelo = table.Column<string>(type: "text", nullable: true),
                    TipoVeiculo = table.Column<int>(type: "integer", nullable: true),
                    AnoVeiculo = table.Column<int>(type: "integer", nullable: true),
                    CpfProprietario = table.Column<string>(type: "text", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    SonoRastreadorSonoId = table.Column<int>(type: "integer", nullable: true)
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
