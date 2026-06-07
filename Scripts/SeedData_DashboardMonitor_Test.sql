-- =========================================
-- Script: Datos de Prueba - Dashboard Monitor
-- Propósito: Insertar datos para presentación del Dashboard Monitor
-- Incluye: Bus BUS-001, Alumnos, Rutas y Paradas para el piloto/monitor
-- =========================================

USE TransportesGenesis;
GO

PRINT '========================================='
PRINT 'Insertando datos de prueba para Dashboard Monitor'
PRINT '========================================='

BEGIN TRANSACTION;

BEGIN TRY

    -- ============================================
    -- 1. INSERTAR BUS BUS-001 (IdBus = 4)
    -- ============================================
    PRINT '1. Insertando Bus BUS-001...'

    -- Verificar si ya existe el bus con IdBus = 4
    IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE IdBus = 4)
    BEGIN
        SET IDENTITY_INSERT genesis.Buses ON;

        INSERT INTO genesis.Buses (IdBus, Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
        VALUES (4, 'BUS-001', 'Mercedes-Benz Sprinter', 35, 1, 1, GETDATE());

        SET IDENTITY_INSERT genesis.Buses OFF;
        PRINT '   ✓ Bus BUS-001 insertado (IdBus = 4)'
    END
    ELSE
    BEGIN
        -- Actualizar si ya existe
        UPDATE genesis.Buses 
        SET Placa = 'BUS-001', 
            Modelo = 'Mercedes-Benz Sprinter', 
            Capacidad = 35,
            Estado = 1,
            Activo = 1
        WHERE IdBus = 4;
        PRINT '   ✓ Bus BUS-001 actualizado (IdBus = 4)'
    END

    PRINT ''

    -- ============================================
    -- 2. INSERTAR PADRES DE FAMILIA
    -- ============================================
    PRINT '2. Insertando Padres de Familia...'

    -- Limpiar padres existentes de prueba (si existen)
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
    PRINT '   ✓ 8 Padres de familia insertados'
    PRINT ''

    -- ============================================
    -- 3. INSERTAR ALUMNOS ASIGNADOS AL BUS-001
    -- ============================================
    PRINT '3. Insertando Alumnos asignados al Bus BUS-001...'

    -- Limpiar alumnos existentes de prueba (si existen)
    DELETE FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220;

    SET IDENTITY_INSERT genesis.Alumnos ON;

    INSERT INTO genesis.Alumnos (IdAlumno, IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES 
        -- Zona Norte de Guatemala (Zona 1-2)
        (200, 100, 'Diego', 'Martínez', 4, 14.6449, -90.5069, '5a Avenida 12-45, Zona 1, Guatemala', 1, GETDATE()),
        (201, 100, 'Sofía', 'Martínez', 4, 14.6455, -90.5075, '5a Avenida 12-45, Zona 1, Guatemala', 1, GETDATE()),

        -- Zona 2
        (202, 101, 'Mateo', 'García', 4, 14.6521, -90.5134, '12 Calle 8-30, Zona 2, Guatemala', 1, GETDATE()),
        (203, 102, 'Isabella', 'Rodríguez', 4, 14.6489, -90.5089, '7a Avenida 15-22, Zona 2, Guatemala', 1, GETDATE()),

        -- Zona Centro (Zona 4)
        (204, 103, 'Santiago', 'López', 4, 14.6286, -90.5140, 'Ruta 6 9-55, Zona 4, Guatemala', 1, GETDATE()),
        (205, 104, 'Valentina', 'Hernández', 4, 14.6312, -90.5167, '11 Avenida 18-40, Zona 4, Guatemala', 1, GETDATE()),

        -- Zona Sur (Zona 9-10)
        (206, 105, 'Sebastián', 'Gómez', 4, 14.6097, -90.5245, '4a Avenida 12-20, Zona 9, Guatemala', 1, GETDATE()),
        (207, 106, 'Camila', 'Díaz', 4, 14.6045, -90.5198, 'Boulevard Los Próceres 22-45, Zona 10, Guatemala', 1, GETDATE()),

        -- Zona 11
        (208, 107, 'Matías', 'Torres', 4, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11, Guatemala', 1, GETDATE()),
        (209, 107, 'Lucía', 'Torres', 4, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11, Guatemala', 1, GETDATE());

    SET IDENTITY_INSERT genesis.Alumnos OFF;
    PRINT '   ✓ 10 Alumnos insertados y asignados al Bus BUS-001'
    PRINT ''

    -- ============================================
    -- 4. INSERTAR RUTAS PARA EL BUS-001
    -- ============================================
    PRINT '4. Insertando Rutas para Bus BUS-001...'

    -- Limpiar rutas existentes de prueba
    DELETE FROM genesis.Rutas WHERE IdRuta BETWEEN 10 AND 12;

    SET IDENTITY_INSERT genesis.Rutas ON;

    INSERT INTO genesis.Rutas (IdRuta, IdBus, Nombre, Descripcion, TipoRuta, HoraInicio, EsActiva, Activo, FechaRegistro)
    VALUES 
        (10, 4, 'Ruta Mañana - BUS-001', 'Recorrido matutino desde Zona 11 hasta Colegio', 'Mañana', '06:00:00', 1, 1, GETDATE()),
        (11, 4, 'Ruta Tarde - BUS-001', 'Recorrido vespertino desde Colegio hasta Zona 11', 'Tarde', '14:00:00', 1, 1, GETDATE());

    SET IDENTITY_INSERT genesis.Rutas OFF;
    PRINT '   ✓ 2 Rutas insertadas (Mañana y Tarde)'
    PRINT ''

    -- ============================================
    -- 5. INSERTAR PARADAS PARA RUTA MAÑANA (Ida al colegio)
    -- ============================================
    PRINT '5. Insertando Paradas para Ruta Mañana...'

    -- Limpiar paradas existentes
    DELETE FROM genesis.Paradas WHERE IdParada BETWEEN 100 AND 200;

    SET IDENTITY_INSERT genesis.Paradas ON;

    -- Ruta Mañana: Desde casas hasta colegio (Orden inverso geográfico)
    INSERT INTO genesis.Paradas (IdParada, IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
    VALUES 
        -- Inicio: Zona 11 (más lejano)
        (100, 10, 208, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 1, '06:05:00', 0, 1, GETDATE()),
        (101, 10, 209, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 2, '06:06:00', 0, 1, GETDATE()),

        -- Zona 9-10
        (102, 10, 207, 14.6045, -90.5198, 'Boulevard Los Próceres 22-45, Zona 10', 3, '06:15:00', 0, 1, GETDATE()),
        (103, 10, 206, 14.6097, -90.5245, '4a Avenida 12-20, Zona 9', 4, '06:20:00', 0, 1, GETDATE()),

        -- Zona 4
        (104, 10, 205, 14.6312, -90.5167, '11 Avenida 18-40, Zona 4', 5, '06:28:00', 0, 1, GETDATE()),
        (105, 10, 204, 14.6286, -90.5140, 'Ruta 6 9-55, Zona 4', 6, '06:32:00', 0, 1, GETDATE()),

        -- Zona 2
        (106, 10, 203, 14.6489, -90.5089, '7a Avenida 15-22, Zona 2', 7, '06:40:00', 0, 1, GETDATE()),
        (107, 10, 202, 14.6521, -90.5134, '12 Calle 8-30, Zona 2', 8, '06:45:00', 0, 1, GETDATE()),

        -- Zona 1 (más cercano al colegio)
        (108, 10, 200, 14.6449, -90.5069, '5a Avenida 12-45, Zona 1', 9, '06:52:00', 0, 1, GETDATE()),
        (109, 10, 201, 14.6455, -90.5075, '5a Avenida 12-45, Zona 1', 10, '06:53:00', 0, 1, GETDATE()),

        -- Destino Final: Colegio (sin alumno asignado)
        (110, 10, NULL, 14.6350, -90.5125, 'Colegio Genesis, Zona 1, Guatemala', 11, '07:00:00', 0, 1, GETDATE());

    PRINT '   ✓ 11 Paradas insertadas para Ruta Mañana'
    PRINT ''

    -- ============================================
    -- 6. INSERTAR PARADAS PARA RUTA TARDE (Regreso a casa)
    -- ============================================
    PRINT '6. Insertando Paradas para Ruta Tarde...'

    -- Ruta Tarde: Desde colegio hasta casas (Orden normal geográfico)
    INSERT INTO genesis.Paradas (IdParada, IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
    VALUES 
        -- Inicio: Colegio
        (120, 11, NULL, 14.6350, -90.5125, 'Colegio Genesis, Zona 1, Guatemala', 1, '14:00:00', 0, 1, GETDATE()),

        -- Zona 1 (primeras paradas)
        (121, 11, 200, 14.6449, -90.5069, '5a Avenida 12-45, Zona 1', 2, '14:08:00', 0, 1, GETDATE()),
        (122, 11, 201, 14.6455, -90.5075, '5a Avenida 12-45, Zona 1', 3, '14:09:00', 0, 1, GETDATE()),

        -- Zona 2
        (123, 11, 202, 14.6521, -90.5134, '12 Calle 8-30, Zona 2', 4, '14:15:00', 0, 1, GETDATE()),
        (124, 11, 203, 14.6489, -90.5089, '7a Avenida 15-22, Zona 2', 5, '14:20:00', 0, 1, GETDATE()),

        -- Zona 4
        (125, 11, 204, 14.6286, -90.5140, 'Ruta 6 9-55, Zona 4', 6, '14:28:00', 0, 1, GETDATE()),
        (126, 11, 205, 14.6312, -90.5167, '11 Avenida 18-40, Zona 4', 7, '14:32:00', 0, 1, GETDATE()),

        -- Zona 9-10
        (127, 11, 206, 14.6097, -90.5245, '4a Avenida 12-20, Zona 9', 8, '14:40:00', 0, 1, GETDATE()),
        (128, 11, 207, 14.6045, -90.5198, 'Boulevard Los Próceres 22-45, Zona 10', 9, '14:45:00', 0, 1, GETDATE()),

        -- Destino Final: Zona 11 (más lejano)
        (129, 11, 208, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 10, '14:55:00', 0, 1, GETDATE()),
        (130, 11, 209, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 11, '14:56:00', 0, 1, GETDATE());

    SET IDENTITY_INSERT genesis.Paradas OFF;
    PRINT '   ✓ 11 Paradas insertadas para Ruta Tarde'
    PRINT ''

    -- ============================================
    -- 7. MOSTRAR RESUMEN DE DATOS INSERTADOS
    -- ============================================
    PRINT '========================================='
    PRINT 'RESUMEN DE DATOS INSERTADOS'
    PRINT '========================================='
    PRINT ''

    PRINT 'BUS:'
    SELECT IdBus, Placa, Modelo, Capacidad, 
           CASE WHEN Estado = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
    FROM genesis.Buses 
    WHERE IdBus = 4;
    PRINT ''

    PRINT 'PADRES DE FAMILIA:'
    SELECT IdPadre, Nombre, Apellido
    FROM genesis.Padres 
    WHERE IdPadre BETWEEN 100 AND 110
    ORDER BY IdPadre;
    PRINT ''

    PRINT 'ALUMNOS ASIGNADOS AL BUS-001:'
    SELECT a.IdAlumno, 
           a.Nombre + ' ' + a.Apellido AS NombreCompleto,
           p.Nombre + ' ' + p.Apellido AS Padre,
           a.Direccion,
           b.Placa AS Bus
    FROM genesis.Alumnos a
    INNER JOIN genesis.Padres p ON a.IdPadre = p.IdPadre
    INNER JOIN genesis.Buses b ON a.IdBusAsignado = b.IdBus
    WHERE a.IdAlumno BETWEEN 200 AND 220
    ORDER BY a.IdAlumno;
    PRINT ''

    PRINT 'RUTAS DEL BUS-001:'
    SELECT r.IdRuta, r.Nombre, r.TipoRuta, 
           CONVERT(VARCHAR(5), r.HoraInicio, 108) AS HoraInicio,
           COUNT(p.IdParada) AS TotalParadas
    FROM genesis.Rutas r
    LEFT JOIN genesis.Paradas p ON r.IdRuta = p.IdRuta
    WHERE r.IdBus = 4
    GROUP BY r.IdRuta, r.Nombre, r.TipoRuta, r.HoraInicio
    ORDER BY r.IdRuta;
    PRINT ''

    COMMIT TRANSACTION;

    PRINT ''
    PRINT '========================================='
    PRINT '✓ DATOS INSERTADOS EXITOSAMENTE'
    PRINT '========================================='
    PRINT ''
    PRINT 'NOTA: El Dashboard Monitor mostrará estos'
    PRINT 'alumnos cuando el piloto/monitor esté'
    PRINT 'autenticado con el Bus BUS-001 (IdBus = 4)'
    PRINT '========================================='

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT ''
    PRINT '========================================='
    PRINT '✗ ERROR AL INSERTAR DATOS'
    PRINT '========================================='
    PRINT 'Error: ' + ERROR_MESSAGE()
    PRINT 'Línea: ' + CAST(ERROR_LINE() AS VARCHAR(10))
    PRINT '========================================='
END CATCH
GO
