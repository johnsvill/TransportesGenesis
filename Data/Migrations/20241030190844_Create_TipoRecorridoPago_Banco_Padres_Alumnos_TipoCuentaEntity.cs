using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportesGenesis.Data.Migrations
{
    /// <inheritdoc />
    public partial class Create_TipoRecorridoPago_Banco_Padres_Alumnos_TipoCuentaEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "genesis");

            migrationBuilder.CreateTable(
                name: "Bancos",
                schema: "genesis",
                columns: table => new
                {
                    IdBanco = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancos", x => x.IdBanco);
                });

            migrationBuilder.CreateTable(
                name: "Padres",
                schema: "genesis",
                columns: table => new
                {
                    IdPadre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Padres", x => x.IdPadre);
                });

            migrationBuilder.CreateTable(
                name: "TipoRecorridoPago",
                schema: "genesis",
                columns: table => new
                {
                    IdTipoRecorrido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoRecorrido = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoRecorridoPago", x => x.IdTipoRecorrido);
                });

            migrationBuilder.CreateTable(
                name: "TipoCuenta",
                schema: "genesis",
                columns: table => new
                {
                    IdTipoCuenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdBanco = table.Column<int>(type: "int", nullable: true),
                    IdPadre = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoCuenta", x => x.IdTipoCuenta);
                    table.ForeignKey(
                        name: "FK_TipoCuenta_Bancos_IdBanco",
                        column: x => x.IdBanco,
                        principalSchema: "genesis",
                        principalTable: "Bancos",
                        principalColumn: "IdBanco");
                    table.ForeignKey(
                        name: "FK_TipoCuenta_Padres_IdPadre",
                        column: x => x.IdPadre,
                        principalSchema: "genesis",
                        principalTable: "Padres",
                        principalColumn: "IdPadre");
                });

            migrationBuilder.CreateTable(
                name: "Alumnos",
                schema: "genesis",
                columns: table => new
                {
                    IdAlumno = table.Column<int>(type: "int", nullable: false),
                    IdPadre = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumnos", x => x.IdAlumno);
                    table.ForeignKey(
                        name: "FK_Alumnos_Padres_IdPadre",
                        column: x => x.IdPadre,
                        principalSchema: "genesis",
                        principalTable: "Padres",
                        principalColumn: "IdPadre");
                    table.ForeignKey(
                        name: "FK_Alumnos_TipoRecorridoPago_IdAlumno",
                        column: x => x.IdAlumno,
                        principalSchema: "genesis",
                        principalTable: "TipoRecorridoPago",
                        principalColumn: "IdTipoRecorrido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_FechaRegistro",
                schema: "genesis",
                table: "Alumnos",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_IdPadre",
                schema: "genesis",
                table: "Alumnos",
                column: "IdPadre");

            migrationBuilder.CreateIndex(
                name: "IX_Bancos_FechaRegistro",
                schema: "genesis",
                table: "Bancos",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Padres_FechaRegistro",
                schema: "genesis",
                table: "Padres",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCuenta_FechaRegistro",
                schema: "genesis",
                table: "TipoCuenta",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCuenta_IdBanco",
                schema: "genesis",
                table: "TipoCuenta",
                column: "IdBanco");

            migrationBuilder.CreateIndex(
                name: "IX_TipoCuenta_IdPadre",
                schema: "genesis",
                table: "TipoCuenta",
                column: "IdPadre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alumnos",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "TipoCuenta",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "TipoRecorridoPago",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Bancos",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Padres",
                schema: "genesis");
        }
    }
}
