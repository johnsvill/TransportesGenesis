using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportesGenesis.Migrations
{
    /// <inheritdoc />
    public partial class AddPagosPadresMesAnio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Anio",
                table: "PagosPadres",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mes",
                table: "PagosPadres",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anio",
                table: "PagosPadres");

            migrationBuilder.DropColumn(
                name: "Mes",
                table: "PagosPadres");
        }
    }
}
