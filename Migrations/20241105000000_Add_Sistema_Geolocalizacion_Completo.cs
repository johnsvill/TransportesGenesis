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
            // Agregar columnas de geolocalización a la tabla Alumnos existente
            migrationBuilder.AddColumn<int>(
                name: "IdBusAsignado",
                schema: "genesis",
                table: "Alumnos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitud",
                schema: "genesis",
                table: "Alumnos",
                type: "decimal(10,7)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud",
                schema: "genesis",
                table: "Alumnos",
                type: "decimal(10,7)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                schema: "genesis",
                table: "Alumnos",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            // Crear tabla Buses
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

            // Crear tabla Rutas
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

            // Crear tabla UbicacionBusEnTiempoReal
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

            // Crear tabla AsignacionPilotoBus
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

            // Crear tabla Paradas
            migrationBuilder.CreateTable(
                name: "Paradas",
                schema: "genesis",
                columns: table => new
                {
                    IdParada = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdRuta = table.Column<int>(type: "int", nullable: false),
                    IdAlumno = table.Column<int>(type: "int", nullable: true),
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

            // Crear tabla AsistenciaAlumno
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
                    table.ForeignKey(
                        name: "FK_AsistenciaAlumno_Buses_IdBusTemporalMañana",
                        column: x => x.IdBusTemporalMañana,
                        principalSchema: "genesis",
                        principalTable: "Buses",
                        principalColumn: "IdBus",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AsistenciaAlumno_Buses_IdBusTemporalTarde",
                        column: x => x.IdBusTemporalTarde,
                        principalSchema: "genesis",
                        principalTable: "Buses",
                        principalColumn: "IdBus",
                        onDelete: ReferentialAction.SetNull);
                });

            // Crear tabla SolicitudTraslado
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

            // Crear tabla Alertas
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

            // Crear tabla NotificacionRetraso
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

            // Crear tabla NotificacionProximidad
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

            // Crear tabla RegistroRecogida
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

            // Crear tabla ConfiguracionSistema
            migrationBuilder.CreateTable(
                name: "ConfiguracionSistema",
                schema: "genesis",
                columns: table => new
                {
                    IdConfiguracion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clave = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    TipoDato = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Activo = table.Column<int>(type: "int", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionSistema", x => x.IdConfiguracion);
                });

            // Foreign Key de Alumnos a Buses
            migrationBuilder.CreateIndex(
                name: "IX_Alumnos_IdBusAsignado",
                schema: "genesis",
                table: "Alumnos",
                column: "IdBusAsignado");

            migrationBuilder.AddForeignKey(
                name: "FK_Alumnos_Buses_IdBusAsignado",
                schema: "genesis",
                table: "Alumnos",
                column: "IdBusAsignado",
                principalSchema: "genesis",
                principalTable: "Buses",
                principalColumn: "IdBus",
                onDelete: ReferentialAction.SetNull);

            // Indices adicionales
            migrationBuilder.CreateIndex(
                name: "IX_Buses_Placa",
                schema: "genesis",
                table: "Buses",
                column: "Placa",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buses_FechaRegistro",
                schema: "genesis",
                table: "Buses",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_IdBus",
                schema: "genesis",
                table: "Rutas",
                column: "IdBus");

            migrationBuilder.CreateIndex(
                name: "IX_Rutas_FechaRegistro",
                schema: "genesis",
                table: "Rutas",
                column: "FechaRegistro");

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionPilotoBus_IdBus",
                schema: "genesis",
                table: "AsignacionPilotoBus",
                column: "IdBus",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsignacionPilotoBus_EsActual",
                schema: "genesis",
                table: "AsignacionPilotoBus",
                column: "EsActual");

            migrationBuilder.CreateIndex(
                name: "IX_UbicacionBusEnTiempoReal_IdBus",
                schema: "genesis",
                table: "UbicacionBusEnTiempoReal",
                column: "IdBus");

            migrationBuilder.CreateIndex(
                name: "IX_Paradas_IdRuta",
                schema: "genesis",
                table: "Paradas",
                column: "IdRuta");

            migrationBuilder.CreateIndex(
                name: "IX_Paradas_IdAlumno",
                schema: "genesis",
                table: "Paradas",
                column: "IdAlumno");

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaAlumno_IdAlumno",
                schema: "genesis",
                table: "AsistenciaAlumno",
                column: "IdAlumno");

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaAlumno_Fecha",
                schema: "genesis",
                table: "AsistenciaAlumno",
                column: "Fecha");

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaAlumno_IdBusTemporalMañana",
                schema: "genesis",
                table: "AsistenciaAlumno",
                column: "IdBusTemporalMañana");

            migrationBuilder.CreateIndex(
                name: "IX_AsistenciaAlumno_IdBusTemporalTarde",
                schema: "genesis",
                table: "AsistenciaAlumno",
                column: "IdBusTemporalTarde");

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
                name: "IX_Alertas_FechaEnvio",
                schema: "genesis",
                table: "Alertas",
                column: "FechaEnvio");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionRetraso_IdRuta",
                schema: "genesis",
                table: "NotificacionRetraso",
                column: "IdRuta");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionProximidad_IdParada",
                schema: "genesis",
                table: "NotificacionProximidad",
                column: "IdParada");

            migrationBuilder.CreateIndex(
                name: "IX_NotificacionProximidad_IdPadre",
                schema: "genesis",
                table: "NotificacionProximidad",
                column: "IdPadre");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroRecogida_IdParada",
                schema: "genesis",
                table: "RegistroRecogida",
                column: "IdParada");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroRecogida_IdAlumno",
                schema: "genesis",
                table: "RegistroRecogida",
                column: "IdAlumno");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionSistema_Clave",
                schema: "genesis",
                table: "ConfiguracionSistema",
                column: "Clave",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alumnos_Buses_IdBusAsignado",
                schema: "genesis",
                table: "Alumnos");

            migrationBuilder.DropTable(
                name: "Alertas",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "AsignacionPilotoBus",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "NotificacionProximidad",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "NotificacionRetraso",
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
                name: "AsistenciaAlumno",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "ConfiguracionSistema",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Paradas",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Rutas",
                schema: "genesis");

            migrationBuilder.DropTable(
                name: "Buses",
                schema: "genesis");

            migrationBuilder.DropIndex(
                name: "IX_Alumnos_IdBusAsignado",
                schema: "genesis",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "IdBusAsignado",
                schema: "genesis",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "Latitud",
                schema: "genesis",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "Longitud",
                schema: "genesis",
                table: "Alumnos");

            migrationBuilder.DropColumn(
                name: "Direccion",
                schema: "genesis",
                table: "Alumnos");
        }
    }
}
