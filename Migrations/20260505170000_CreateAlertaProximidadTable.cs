using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportesGenesis.Migrations
{
    /// <inheritdoc />
    public partial class CreateAlertaProximidadTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Crear la tabla AlertasProximidad
            migrationBuilder.CreateTable(
                name: "AlertasProximidad",
                schema: "genesis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoAlerta = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdBus = table.Column<int>(type: "int", nullable: false),
                    IdAlumno = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, defaultValue: "activo"),
                    FechaResolucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ParadasRestantes = table.Column<int>(type: "int", nullable: true),
                    ParadaActual = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ParadaDestino = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ConfirmacionPadre = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FechaConfirmacionPadre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdPadre = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertasProximidad", x => x.Id);
                    table.CheckConstraint("CK_AlertaProximidad_Estado", "[Estado] IN ('activo', 'resuelto')");
                    table.CheckConstraint("CK_AlertaProximidad_TipoAlerta", "[TipoAlerta] IN ('proximidad', 'retraso')");

                    // Solo agregar FK si las tablas existen
                    table.ForeignKey(
                        name: "FK_AlertasProximidad_Buses_IdBus",
                        column: x => x.IdBus,
                        principalSchema: "genesis",
                        principalTable: "Buses",
                        principalColumn: "IdBus",
                        onDelete: ReferentialAction.Restrict);

                    table.ForeignKey(
                        name: "FK_AlertasProximidad_Alumnos_IdAlumno",
                        column: x => x.IdAlumno,
                        principalSchema: "genesis",
                        principalTable: "Alumnos",
                        principalColumn: "IdAlumno",
                        onDelete: ReferentialAction.SetNull);
                });

            // Crear índices para mejorar el rendimiento
            migrationBuilder.CreateIndex(
                name: "IX_AlertasProximidad_IdBus",
                schema: "genesis",
                table: "AlertasProximidad",
                column: "IdBus");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasProximidad_IdAlumno",
                schema: "genesis",
                table: "AlertasProximidad",
                column: "IdAlumno");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasProximidad_Estado",
                schema: "genesis",
                table: "AlertasProximidad",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasProximidad_FechaHora",
                schema: "genesis",
                table: "AlertasProximidad",
                column: "FechaHora");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasProximidad_IdPadre",
                schema: "genesis",
                table: "AlertasProximidad",
                column: "IdPadre");

            migrationBuilder.CreateIndex(
                name: "IX_AlertasProximidad_IdBus_Estado",
                schema: "genesis",
                table: "AlertasProximidad",
                columns: new[] { "IdBus", "Estado" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlertasProximidad",
                schema: "genesis");
        }
    }
}