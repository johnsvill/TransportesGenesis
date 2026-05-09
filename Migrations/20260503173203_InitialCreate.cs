using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportesGenesis.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "genesis");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    IsFirstLogin = table.Column<bool>(type: "bit", nullable: true),
                    LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

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
                name: "CuentasUsuarios",
                schema: "genesis",
                columns: table => new
                {
                    IdCuentaUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdBanco = table.Column<int>(type: "int", nullable: false),
                    NumeroCuenta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasUsuarios", x => x.IdCuentaUsuario);
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
                name: "PagosPadresDb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoPago = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComprobanteUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    EstadoAdmin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstadoStripe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroComprobante = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaPago = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdBanco = table.Column<int>(type: "int", nullable: true),
                    IdCuentaUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagosPadresDb", x => x.Id);
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
                    DiaMaximoPago = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoRecorridoPago", x => x.IdTipoRecorrido);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CuentasUsuarios",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Pagos",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "PagosPadresDb");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

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
