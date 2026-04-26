using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransportesGenesis.Migrations
{
    /// <inheritdoc />
    public partial class Add_Sistema_Geolocalizacion_Completo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "genesis");

            migrationBuilder.CreateTable(
                name: "Alertas",
                schema: "genesis",
                columns: table => new
                {
                    IdAlerta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoAlerta = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IdRemitente = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    IdDestinatario = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    FechaEnvio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Leida = table.Column<bool>(type: "bit", nullable: false),
                    FechaLectura = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.IdAlerta);
                });

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
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bancos", x => x.IdBanco);
                });

            migrationBuilder.CreateTable(
                name: "Buses",
                schema: "genesis",
                columns: table => new
                {
                    IdBus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Placa = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Capacidad = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buses", x => x.IdBus);
                });

            migrationBuilder.CreateTable(
                name: "Padres",
                schema: "genesis",
                columns: table => new
                {
                    IdPadre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
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
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
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
                name: "AsignacionPilotoBus",
                schema: "genesis",
                columns: table => new
                {
                    IdAsignacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdUsuarioPiloto = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    IdBus = table.Column<int>(type: "int", nullable: false),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaFinAsignacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EsActual = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignacionPilotoBus", x => x.IdAsignacion);
                    table.ForeignKey(
                        name: "FK_AsignacionPilotoBus_Buses_IdBus",
                        column: x => x.IdBus,
                        principalSchema: "genesis",
                        principalTable: "Buses",
                        principalColumn: "IdBus",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rutas",
                schema: "genesis",
                columns: table => new
                {
                    IdRuta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBus = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TipoRuta = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    HoraInicio = table.Column<TimeSpan>(type: "time", nullable: false),
                    EsActiva = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rutas", x => x.IdRuta);
                    table.ForeignKey(
                        name: "FK_Rutas_Buses_IdBus",
                        column: x => x.IdBus,
                        principalSchema: "genesis",
                        principalTable: "Buses",
                        principalColumn: "IdBus",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UbicacionBusEnTiempoReal",
                schema: "genesis",
                columns: table => new
                {
                    IdUbicacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdBus = table.Column<int>(type: "int", nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Velocidad = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    Direccion = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UbicacionBusEnTiempoReal", x => x.IdUbicacion);
                    table.ForeignKey(
                        name: "FK_UbicacionBusEnTiempoReal_Buses_IdBus",
                        column: x => x.IdBus,
                        principalSchema: "genesis",
                        principalTable: "Buses",
                        principalColumn: "IdBus",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoCuenta",
                schema: "genesis",
                columns: table => new
                {
                    IdTipoCuenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdBanco = table.Column<int>(type: "int", nullable: false),
                    IdPadre = table.Column<int>(type: "int", nullable: false),
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
                        principalColumn: "IdBanco",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TipoCuenta_Padres_IdPadre",
                        column: x => x.IdPadre,
                        principalSchema: "genesis",
                        principalTable: "Padres",
                        principalColumn: "IdPadre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alumnos",
                schema: "genesis",
                columns: table => new
                {
                    IdAlumno = table.Column<int>(type: "int", nullable: false),
                    IdPadre = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IdBusAsignado = table.Column<int>(type: "int", nullable: true),
                    Latitud = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Longitud = table.Column<decimal>(type: "decimal(10,7)", nullable: true),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
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
                        principalColumn: "IdPadre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumnos_TipoRecorridoPago_IdAlumno",
                        column: x => x.IdAlumno,
                        principalSchema: "genesis",
                        principalTable: "TipoRecorridoPago",
                        principalColumn: "IdTipoRecorrido",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificacionRetraso",
                schema: "genesis",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRuta = table.Column<int>(type: "int", nullable: false),
                    TiempoRetrasoMinutos = table.Column<int>(type: "int", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NotificadoAPadres = table.Column<bool>(type: "bit", nullable: false),
                    FechaNotificacionPadres = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacionRetraso", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_NotificacionRetraso_Rutas_IdRuta",
                        column: x => x.IdRuta,
                        principalSchema: "genesis",
                        principalTable: "Rutas",
                        principalColumn: "IdRuta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsistenciaAlumno",
                schema: "genesis",
                columns: table => new
                {
                    IdAsistencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlumno = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AsisteMañana = table.Column<bool>(type: "bit", nullable: false),
                    AsisteTarde = table.Column<bool>(type: "bit", nullable: false),
                    FechaConfirmacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdBusTemporalMañana = table.Column<int>(type: "int", nullable: true),
                    IdBusTemporalTarde = table.Column<int>(type: "int", nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsistenciaAlumno", x => x.IdAsistencia);
                    table.ForeignKey(
                        name: "FK_AsistenciaAlumno_Alumnos_IdAlumno",
                        column: x => x.IdAlumno,
                        principalSchema: "genesis",
                        principalTable: "Alumnos",
                        principalColumn: "IdAlumno",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                schema: "genesis",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlumno = table.Column<int>(type: "int", nullable: false),
                    IdPadre = table.Column<int>(type: "int", nullable: false),
                    IdTipoCuenta = table.Column<int>(type: "int", nullable: false),
                    IdTipoRecorrido = table.Column<int>(type: "int", nullable: false),
                    MesPagado = table.Column<int>(type: "int", nullable: false),
                    Anio = table.Column<int>(type: "int", nullable: false),
                    PagoCompleto = table.Column<bool>(type: "bit", nullable: false),
                    MontoParcial = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Ubicacion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
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
                        principalColumn: "IdAlumno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_Padres_IdPadre",
                        column: x => x.IdPadre,
                        principalSchema: "genesis",
                        principalTable: "Padres",
                        principalColumn: "IdPadre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_TipoCuenta_IdTipoCuenta",
                        column: x => x.IdTipoCuenta,
                        principalSchema: "genesis",
                        principalTable: "TipoCuenta",
                        principalColumn: "IdTipoCuenta",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pagos_TipoRecorridoPago_IdTipoRecorrido",
                        column: x => x.IdTipoRecorrido,
                        principalSchema: "genesis",
                        principalTable: "TipoRecorridoPago",
                        principalColumn: "IdTipoRecorrido",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Paradas",
                schema: "genesis",
                columns: table => new
                {
                    IdParada = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRuta = table.Column<int>(type: "int", nullable: false),
                    IdAlumno = table.Column<int>(type: "int", nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    HoraEstimada = table.Column<TimeSpan>(type: "time", nullable: true),
                    Completada = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paradas", x => x.IdParada);
                    table.ForeignKey(
                        name: "FK_Paradas_Alumnos_IdAlumno",
                        column: x => x.IdAlumno,
                        principalSchema: "genesis",
                        principalTable: "Alumnos",
                        principalColumn: "IdAlumno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Paradas_Rutas_IdRuta",
                        column: x => x.IdRuta,
                        principalSchema: "genesis",
                        principalTable: "Rutas",
                        principalColumn: "IdRuta",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SolicitudTraslado",
                schema: "genesis",
                columns: table => new
                {
                    IdSolicitud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAlumno = table.Column<int>(type: "int", nullable: false),
                    IdBusOrigen = table.Column<int>(type: "int", nullable: false),
                    IdBusDestino = table.Column<int>(type: "int", nullable: true),
                    FechaTraslado = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Motivo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AprobadoPor = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    FechaRespuesta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ComentarioAdmin = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitudTraslado", x => x.IdSolicitud);
                    table.ForeignKey(
                        name: "FK_SolicitudTraslado_Alumnos_IdAlumno",
                        column: x => x.IdAlumno,
                        principalSchema: "genesis",
                        principalTable: "Alumnos",
                        principalColumn: "IdAlumno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SolicitudTraslado_Buses_IdBusOrigen",
                        column: x => x.IdBusOrigen,
                        principalSchema: "genesis",
                        principalTable: "Buses",
                        principalColumn: "IdBus",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificacionProximidad",
                schema: "genesis",
                columns: table => new
                {
                    IdNotificacion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdParada = table.Column<int>(type: "int", nullable: false),
                    IdPadre = table.Column<int>(type: "int", nullable: false),
                    TipoNotificacion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Enviada = table.Column<bool>(type: "bit", nullable: false),
                    FechaHoraEnvio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificacionProximidad", x => x.IdNotificacion);
                    table.ForeignKey(
                        name: "FK_NotificacionProximidad_Padres_IdPadre",
                        column: x => x.IdPadre,
                        principalSchema: "genesis",
                        principalTable: "Padres",
                        principalColumn: "IdPadre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificacionProximidad_Paradas_IdParada",
                        column: x => x.IdParada,
                        principalSchema: "genesis",
                        principalTable: "Paradas",
                        principalColumn: "IdParada",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RegistroRecogida",
                schema: "genesis",
                columns: table => new
                {
                    IdRegistro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdParada = table.Column<int>(type: "int", nullable: false),
                    IdAlumno = table.Column<int>(type: "int", nullable: false),
                    FechaHoraRecogida = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmadoPor = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Latitud = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: true),
                    Longitud = table.Column<decimal>(type: "decimal(10,7)", precision: 10, scale: 7, nullable: true),
                    AlumnoPresente = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroRecogida", x => x.IdRegistro);
                    table.ForeignKey(
                        name: "FK_RegistroRecogida_Alumnos_IdAlumno",
                        column: x => x.IdAlumno,
                        principalSchema: "genesis",
                        principalTable: "Alumnos",
                        principalColumn: "IdAlumno",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RegistroRecogida_Paradas_IdParada",
                        column: x => x.IdParada,
                        principalSchema: "genesis",
                        principalTable: "Paradas",
                        principalColumn: "IdParada",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_FechaEnvio",
                schema: "genesis",
                table: "Alertas",
                column: "FechaEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_IdDestinatario",
                schema: "genesis",
                table: "Alertas",
                column: "IdDestinatario");

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_Leida",
                schema: "genesis",
                table: "Alertas",
                column: "Leida");

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
                name: "IX_AsignacionPilotoBus_EsActual",
                schema: "genesis",
                table: "AsignacionPilotoBus",
                column: "EsActual");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionPilotoBus_FechaAsignacion",
                schema: "genesis",
                table: "AsignacionPilotoBus",
                column: "FechaAsignacion");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionPilotoBus_IdBus",
                schema: "genesis",
                table: "AsignacionPilotoBus",
                column: "IdBus",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionPilotoBus_IdUsuarioPiloto",
                schema: "genesis",
                table: "AsignacionPilotoBus",
                column: "IdUsuarioPiloto");

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaAlumno_Fecha",
                schema: "genesis",
                table: "AsistenciaAlumno",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaAlumno_FechaRegistro",
                schema: "genesis",
                table: "AsistenciaAlumno",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaAlumno_IdAlumno",
                schema: "genesis",
                table: "AsistenciaAlumno",
                column: "IdAlumno");

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
                name: "IX_Buses_FechaRegistro",
                schema: "genesis",
                table: "Buses",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Buses_Placa",
                schema: "genesis",
                table: "Buses",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionProximidad_Enviada",
                schema: "genesis",
                table: "NotificacionProximidad",
                column: "Enviada");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionProximidad_FechaHoraEnvio",
                schema: "genesis",
                table: "NotificacionProximidad",
                column: "FechaHoraEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionProximidad_IdPadre",
                schema: "genesis",
                table: "NotificacionProximidad",
                column: "IdPadre");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionProximidad_IdParada",
                schema: "genesis",
                table: "NotificacionProximidad",
                column: "IdParada");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionRetraso_FechaHora",
                schema: "genesis",
                table: "NotificacionRetraso",
                column: "FechaHora");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionRetraso_IdRuta",
                schema: "genesis",
                table: "NotificacionRetraso",
                column: "IdRuta");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionRetraso_NotificadoAPadres",
                schema: "genesis",
                table: "NotificacionRetraso",
                column: "NotificadoAPadres");

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
                name: "IX_Paradas_FechaRegistro",
                schema: "genesis",
                table: "Paradas",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Paradas_IdAlumno",
                schema: "genesis",
                table: "Paradas",
                column: "IdAlumno");

            migrationBuilder.CreateIndex(
                name: "IX_Paradas_IdRuta",
                schema: "genesis",
                table: "Paradas",
                column: "IdRuta");

            migrationBuilder.CreateIndex(
                name: "IX_Paradas_Orden",
                schema: "genesis",
                table: "Paradas",
                column: "Orden");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroRecogida_AlumnoPresente",
                schema: "genesis",
                table: "RegistroRecogida",
                column: "AlumnoPresente");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroRecogida_FechaHoraRecogida",
                schema: "genesis",
                table: "RegistroRecogida",
                column: "FechaHoraRecogida");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroRecogida_IdAlumno",
                schema: "genesis",
                table: "RegistroRecogida",
                column: "IdAlumno");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroRecogida_IdParada",
                schema: "genesis",
                table: "RegistroRecogida",
                column: "IdParada");

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_FechaRegistro",
                schema: "genesis",
                table: "Rutas",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_IdBus",
                schema: "genesis",
                table: "Rutas",
                column: "IdBus");

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_TipoRuta",
                schema: "genesis",
                table: "Rutas",
                column: "TipoRuta");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTraslado_Estado",
                schema: "genesis",
                table: "SolicitudTraslado",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTraslado_FechaRegistro",
                schema: "genesis",
                table: "SolicitudTraslado",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTraslado_FechaTraslado",
                schema: "genesis",
                table: "SolicitudTraslado",
                column: "FechaTraslado");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTraslado_IdAlumno",
                schema: "genesis",
                table: "SolicitudTraslado",
                column: "IdAlumno");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitudTraslado_IdBusOrigen",
                schema: "genesis",
                table: "SolicitudTraslado",
                column: "IdBusOrigen");

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

            migrationBuilder.CreateIndex(
                name: "IX_UbicacionBusEnTiempoReal_FechaHora",
                schema: "genesis",
                table: "UbicacionBusEnTiempoReal",
                column: "FechaHora");

            migrationBuilder.CreateIndex(
                name: "IX_UbicacionBusEnTiempoReal_IdBus",
                schema: "genesis",
                table: "UbicacionBusEnTiempoReal",
                column: "IdBus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "AsignacionPilotoBus",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "AsistenciaAlumno",
                schema: "genesis");

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
                name: "NotificacionProximidad",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "NotificacionRetraso",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Pagos",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "RegistroRecogida",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "SolicitudTraslado",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "UbicacionBusEnTiempoReal",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "TipoCuenta",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Paradas",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Bancos",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Alumnos",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Rutas",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Padres",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "TipoRecorridoPago",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Buses",
                schema: "genesis");
        }
    }
}
