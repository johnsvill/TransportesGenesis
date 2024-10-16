using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportesGenesis.Data.Migrations
{
    /// <inheritdoc />
    public partial class Create_Pago_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiaMaximoPago",
                schema: "genesis",
                table: "TipoRecorridoPago",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Pagos",
                schema: "genesis",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlumno = table.Column<int>(type: "int", nullable: true),
                    IdPadre = table.Column<int>(type: "int", nullable: true),
                    IdTipoCuenta = table.Column<int>(type: "int", nullable: true),
                    IdTipoRecorrido = table.Column<int>(type: "int", nullable: true),
                    MesPagado = table.Column<int>(type: "int", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    PagoCompleto = table.Column<bool>(type: "bit", nullable: false),
                    MontoParcial = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Ubicacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FechaModif = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_Pagos_Alumnos_IdAlumno",
                        column: x => x.IdAlumno,
                        principalSchema: "genesis",
                        principalTable: "Alumnos",
                        principalColumn: "IdAlumno");
                    table.ForeignKey(
                        name: "FK_Pagos_Padres_IdPadre",
                        column: x => x.IdPadre,
                        principalSchema: "genesis",
                        principalTable: "Padres",
                        principalColumn: "IdPadre");
                    table.ForeignKey(
                        name: "FK_Pagos_TipoCuenta_IdTipoCuenta",
                        column: x => x.IdTipoCuenta,
                        principalSchema: "genesis",
                        principalTable: "TipoCuenta",
                        principalColumn: "IdTipoCuenta");
                    table.ForeignKey(
                        name: "FK_Pagos_TipoRecorridoPago_IdTipoRecorrido",
                        column: x => x.IdTipoRecorrido,
                        principalSchema: "genesis",
                        principalTable: "TipoRecorridoPago",
                        principalColumn: "IdTipoRecorrido");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_FechaRegistro",
                schema: "genesis",
                table: "Pagos",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdAlumno",
                schema: "genesis",
                table: "Pagos",
                column: "IdAlumno");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdPadre",
                schema: "genesis",
                table: "Pagos",
                column: "IdPadre");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdTipoCuenta",
                schema: "genesis",
                table: "Pagos",
                column: "IdTipoCuenta");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_IdTipoRecorrido",
                schema: "genesis",
                table: "Pagos",
                column: "IdTipoRecorrido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pagos",
                schema: "genesis");

            migrationBuilder.DropColumn(
                name: "DiaMaximoPago",
                schema: "genesis",
                table: "TipoRecorridoPago");
        }
    }
}
