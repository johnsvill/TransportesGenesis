using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace TransportesGenesis.Migrations
{
    /// <inheritdoc />
    /// <summary>
    /// Migración para insertar datos de prueba del Dashboard Monitor.
    /// Incluye: Bus BUS-001, Padres, Alumnos, Rutas y Paradas.
    /// </summary>
    public partial class SeedData_DashboardMonitor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // ============================================
            // 1. INSERTAR BUS BUS-001 (IdBus = 4)
            // ============================================
            migrationBuilder.Sql(@"
                IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE IdBus = 4)
                BEGIN
                    SET IDENTITY_INSERT genesis.Buses ON;

                    INSERT INTO genesis.Buses (IdBus, Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
                    VALUES (4, 'BUS-001', 'Mercedes-Benz Sprinter', 35, 1, 1, GETDATE());

                    SET IDENTITY_INSERT genesis.Buses OFF;
                END
                ELSE
                BEGIN
                    UPDATE genesis.Buses 
                    SET Placa = 'BUS-001', 
                        Modelo = 'Mercedes-Benz Sprinter', 
                        Capacidad = 35,
                        Estado = 1,
                        Activo = 1
                    WHERE IdBus = 4;
                END
            ");

            // ============================================
            // 2. INSERTAR PADRES DE FAMILIA
            // ============================================
            migrationBuilder.Sql(@"
                DELETE FROM genesis.Padres WHERE IdPadre BETWEEN 100 AND 110;

                SET IDENTITY_INSERT genesis.Padres ON;

                INSERT INTO genesis.Padres (IdPadre, Nombre, Apellido, Activo, FechaRegistro)
                VALUES 
                    (100, 'Carlos', 'Martínez López', 1, GETDATE()),
                    (101, 'María', 'García Hernández', 1, GETDATE()),
                    (102, 'José', 'Rodríguez Pérez', 1, GETDATE()),
                    (103, 'Ana', 'López González', 1, GETDATE()),
                    (104, 'Luis', 'Hernández Morales', 1, GETDATE()),
                    (105, 'Patricia', 'Gómez Ramírez', 1, GETDATE()),
                    (106, 'Roberto', 'Díaz Castro', 1, GETDATE()),
                    (107, 'Carmen', 'Torres Flores', 1, GETDATE());

                SET IDENTITY_INSERT genesis.Padres OFF;
            ");

            // ============================================
            // 3. INSERTAR ALUMNOS ASIGNADOS AL BUS-001
            // ============================================
            migrationBuilder.Sql(@"
                DELETE FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220;

                SET IDENTITY_INSERT genesis.Alumnos ON;

                INSERT INTO genesis.Alumnos (IdAlumno, IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
                VALUES 
                    (200, 100, 'Diego', 'Martínez', 4, 14.6449, -90.5069, '5a Avenida 12-45, Zona 1, Guatemala', 1, GETDATE()),
                    (201, 100, 'Sofía', 'Martínez', 4, 14.6455, -90.5075, '5a Avenida 12-45, Zona 1, Guatemala', 1, GETDATE()),
                    (202, 101, 'Mateo', 'García', 4, 14.6521, -90.5134, '12 Calle 8-30, Zona 2, Guatemala', 1, GETDATE()),
                    (203, 102, 'Isabella', 'Rodríguez', 4, 14.6489, -90.5089, '7a Avenida 15-22, Zona 2, Guatemala', 1, GETDATE()),
                    (204, 103, 'Santiago', 'López', 4, 14.6286, -90.5140, 'Ruta 6 9-55, Zona 4, Guatemala', 1, GETDATE()),
                    (205, 104, 'Valentina', 'Hernández', 4, 14.6312, -90.5167, '11 Avenida 18-40, Zona 4, Guatemala', 1, GETDATE()),
                    (206, 105, 'Sebastián', 'Gómez', 4, 14.6097, -90.5245, '4a Avenida 12-20, Zona 9, Guatemala', 1, GETDATE()),
                    (207, 106, 'Camila', 'Díaz', 4, 14.6045, -90.5198, 'Boulevard Los Próceres 22-45, Zona 10, Guatemala', 1, GETDATE()),
                    (208, 107, 'Matías', 'Torres', 4, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11, Guatemala', 1, GETDATE()),
                    (209, 107, 'Lucía', 'Torres', 4, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11, Guatemala', 1, GETDATE());

                SET IDENTITY_INSERT genesis.Alumnos OFF;
            ");

            // ============================================
            // 4. INSERTAR RUTAS PARA EL BUS-001
            // ============================================
            migrationBuilder.Sql(@"
                DELETE FROM genesis.Rutas WHERE IdRuta BETWEEN 10 AND 12;

                SET IDENTITY_INSERT genesis.Rutas ON;

                INSERT INTO genesis.Rutas (IdRuta, IdBus, Nombre, Descripcion, TipoRuta, HoraInicio, EsActiva, Activo, FechaRegistro)
                VALUES 
                    (10, 4, 'Ruta Mañana - BUS-001', 'Recorrido matutino desde Zona 11 hasta Colegio', 'Mañana', '06:00:00', 1, 1, GETDATE()),
                    (11, 4, 'Ruta Tarde - BUS-001', 'Recorrido vespertino desde Colegio hasta Zona 11', 'Tarde', '14:00:00', 1, 1, GETDATE());

                SET IDENTITY_INSERT genesis.Rutas OFF;
            ");

            // ============================================
            // 5. INSERTAR PARADAS PARA RUTA MAÑANA
            // ============================================
            migrationBuilder.Sql(@"
                DELETE FROM genesis.Paradas WHERE IdParada BETWEEN 100 AND 200;

                SET IDENTITY_INSERT genesis.Paradas ON;

                INSERT INTO genesis.Paradas (IdParada, IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
                VALUES 
                    (100, 10, 208, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 1, '06:05:00', 0, 1, GETDATE()),
                    (101, 10, 209, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 2, '06:06:00', 0, 1, GETDATE()),
                    (102, 10, 207, 14.6045, -90.5198, 'Boulevard Los Próceres 22-45, Zona 10', 3, '06:15:00', 0, 1, GETDATE()),
                    (103, 10, 206, 14.6097, -90.5245, '4a Avenida 12-20, Zona 9', 4, '06:20:00', 0, 1, GETDATE()),
                    (104, 10, 205, 14.6312, -90.5167, '11 Avenida 18-40, Zona 4', 5, '06:28:00', 0, 1, GETDATE()),
                    (105, 10, 204, 14.6286, -90.5140, 'Ruta 6 9-55, Zona 4', 6, '06:32:00', 0, 1, GETDATE()),
                    (106, 10, 203, 14.6489, -90.5089, '7a Avenida 15-22, Zona 2', 7, '06:40:00', 0, 1, GETDATE()),
                    (107, 10, 202, 14.6521, -90.5134, '12 Calle 8-30, Zona 2', 8, '06:45:00', 0, 1, GETDATE()),
                    (108, 10, 200, 14.6449, -90.5069, '5a Avenida 12-45, Zona 1', 9, '06:52:00', 0, 1, GETDATE()),
                    (109, 10, 201, 14.6455, -90.5075, '5a Avenida 12-45, Zona 1', 10, '06:53:00', 0, 1, GETDATE()),
                    (110, 10, NULL, 14.6350, -90.5125, 'Colegio Genesis, Zona 1, Guatemala', 11, '07:00:00', 0, 1, GETDATE());

                SET IDENTITY_INSERT genesis.Paradas OFF;
            ");

            // ============================================
            // 6. INSERTAR PARADAS PARA RUTA TARDE
            // ============================================
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT genesis.Paradas ON;

                INSERT INTO genesis.Paradas (IdParada, IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
                VALUES 
                    (120, 11, NULL, 14.6350, -90.5125, 'Colegio Genesis, Zona 1, Guatemala', 1, '14:00:00', 0, 1, GETDATE()),
                    (121, 11, 200, 14.6449, -90.5069, '5a Avenida 12-45, Zona 1', 2, '14:08:00', 0, 1, GETDATE()),
                    (122, 11, 201, 14.6455, -90.5075, '5a Avenida 12-45, Zona 1', 3, '14:09:00', 0, 1, GETDATE()),
                    (123, 11, 202, 14.6521, -90.5134, '12 Calle 8-30, Zona 2', 4, '14:15:00', 0, 1, GETDATE()),
                    (124, 11, 203, 14.6489, -90.5089, '7a Avenida 15-22, Zona 2', 5, '14:20:00', 0, 1, GETDATE()),
                    (125, 11, 204, 14.6286, -90.5140, 'Ruta 6 9-55, Zona 4', 6, '14:28:00', 0, 1, GETDATE()),
                    (126, 11, 205, 14.6312, -90.5167, '11 Avenida 18-40, Zona 4', 7, '14:32:00', 0, 1, GETDATE()),
                    (127, 11, 206, 14.6097, -90.5245, '4a Avenida 12-20, Zona 9', 8, '14:40:00', 0, 1, GETDATE()),
                    (128, 11, 207, 14.6045, -90.5198, 'Boulevard Los Próceres 22-45, Zona 10', 9, '14:45:00', 0, 1, GETDATE()),
                    (129, 11, 208, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 10, '14:55:00', 0, 1, GETDATE()),
                    (130, 11, 209, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 11, '14:56:00', 0, 1, GETDATE());

                SET IDENTITY_INSERT genesis.Paradas OFF;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revertir los datos insertados
            migrationBuilder.Sql(@"
                DELETE FROM genesis.Paradas WHERE IdParada BETWEEN 100 AND 200;
                DELETE FROM genesis.Rutas WHERE IdRuta BETWEEN 10 AND 12;
                DELETE FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220;
                DELETE FROM genesis.Padres WHERE IdPadre BETWEEN 100 AND 110;
                -- Opcional: eliminar el bus
                -- DELETE FROM genesis.Buses WHERE IdBus = 4;
            ");
        }
    }
}
