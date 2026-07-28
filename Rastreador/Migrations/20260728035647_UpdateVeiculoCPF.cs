using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rastreador.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVeiculoCPF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CpfProprietario",
                table: "Veiculos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CpfProprietario",
                table: "Veiculos");
        }
    }
}
