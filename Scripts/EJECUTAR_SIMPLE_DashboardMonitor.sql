-- =========================================
-- Script SIMPLE: Datos Dashboard Monitor
-- Sin usar IDENTITY_INSERT
-- =========================================

USE TransportesGenesis;
GO

PRINT '========================================='
PRINT 'Insertando datos de prueba (versión simple)'
PRINT '========================================='

-- ============================================
-- 1. BUS
-- ============================================
IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE Placa = 'BUS-001')
BEGIN
    INSERT INTO genesis.Buses (Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
    VALUES ('BUS-001', 'Mercedes-Benz Sprinter', 35, 1, 1, GETDATE());
    PRINT '✓ Bus BUS-001 insertado';
END
ELSE
    PRINT '✓ Bus BUS-001 ya existe';

DECLARE @IdBus INT = (SELECT IdBus FROM genesis.Buses WHERE Placa = 'BUS-001');

-- ============================================
-- 2. PADRES
-- ============================================
DECLARE @IdPadre1 INT, @IdPadre2 INT, @IdPadre3 INT, @IdPadre4 INT, @IdPadre5 INT, @IdPadre6 INT, @IdPadre7 INT, @IdPadre8 INT;

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Carlos' AND Apellido = 'Martínez López')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES ('Carlos', 'Martínez López', 1, GETDATE());
    SET @IdPadre1 = SCOPE_IDENTITY();
END
ELSE
    SET @IdPadre1 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Carlos' AND Apellido = 'Martínez López');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'María' AND Apellido = 'García Hernández')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES ('María', 'García Hernández', 1, GETDATE());
    SET @IdPadre2 = SCOPE_IDENTITY();
END
ELSE
    SET @IdPadre2 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'María' AND Apellido = 'García Hernández');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'José' AND Apellido = 'Rodríguez Pérez')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES ('José', 'Rodríguez Pérez', 1, GETDATE());
    SET @IdPadre3 = SCOPE_IDENTITY();
END
ELSE
    SET @IdPadre3 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'José' AND Apellido = 'Rodríguez Pérez');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Ana' AND Apellido = 'López González')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES ('Ana', 'López González', 1, GETDATE());
    SET @IdPadre4 = SCOPE_IDENTITY();
END
ELSE
    SET @IdPadre4 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Ana' AND Apellido = 'López González');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Luis' AND Apellido = 'Hernández Morales')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES ('Luis', 'Hernández Morales', 1, GETDATE());
    SET @IdPadre5 = SCOPE_IDENTITY();
END
ELSE
    SET @IdPadre5 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Luis' AND Apellido = 'Hernández Morales');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Patricia' AND Apellido = 'Gómez Ramírez')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES ('Patricia', 'Gómez Ramírez', 1, GETDATE());
    SET @IdPadre6 = SCOPE_IDENTITY();
END
ELSE
    SET @IdPadre6 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Patricia' AND Apellido = 'Gómez Ramírez');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Roberto' AND Apellido = 'Díaz Castro')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES ('Roberto', 'Díaz Castro', 1, GETDATE());
    SET @IdPadre7 = SCOPE_IDENTITY();
END
ELSE
    SET @IdPadre7 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Roberto' AND Apellido = 'Díaz Castro');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Carmen' AND Apellido = 'Torres Flores')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES ('Carmen', 'Torres Flores', 1, GETDATE());
    SET @IdPadre8 = SCOPE_IDENTITY();
END
ELSE
    SET @IdPadre8 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Carmen' AND Apellido = 'Torres Flores');

PRINT '✓ Padres verificados/insertados';

-- ============================================
-- 3. ALUMNOS
-- ============================================
DECLARE @IdAlum1 INT, @IdAlum2 INT, @IdAlum3 INT, @IdAlum4 INT, @IdAlum5 INT, @IdAlum6 INT, @IdAlum7 INT, @IdAlum8 INT, @IdAlum9 INT, @IdAlum10 INT;

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Diego' AND Apellido = 'Martínez' AND IdPadre = @IdPadre1)
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre1, 'Diego', 'Martínez', @IdBus, 14.6449, -90.5069, '5a Avenida 12-45, Zona 1', 1, GETDATE());
    SET @IdAlum1 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.6449, Longitud = -90.5069, Direccion = '5a Avenida 12-45, Zona 1'
    WHERE Nombre = 'Diego' AND Apellido = 'Martínez' AND IdPadre = @IdPadre1;
    SET @IdAlum1 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Diego' AND Apellido = 'Martínez' AND IdPadre = @IdPadre1);
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Sofía' AND Apellido = 'Martínez' AND IdPadre = @IdPadre1)
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre1, 'Sofía', 'Martínez', @IdBus, 14.6455, -90.5075, '5a Avenida 12-45, Zona 1', 1, GETDATE());
    SET @IdAlum2 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus WHERE Nombre = 'Sofía' AND Apellido = 'Martínez' AND IdPadre = @IdPadre1;
    SET @IdAlum2 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Sofía' AND Apellido = 'Martínez' AND IdPadre = @IdPadre1);
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Mateo' AND Apellido = 'García')
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre2, 'Mateo', 'García', @IdBus, 14.6521, -90.5134, '12 Calle 8-30, Zona 2', 1, GETDATE());
    SET @IdAlum3 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus WHERE Nombre = 'Mateo' AND Apellido = 'García';
    SET @IdAlum3 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Mateo' AND Apellido = 'García');
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Isabella' AND Apellido = 'Rodríguez')
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre3, 'Isabella', 'Rodríguez', @IdBus, 14.6489, -90.5089, '7a Avenida 15-22, Zona 2', 1, GETDATE());
    SET @IdAlum4 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus WHERE Nombre = 'Isabella' AND Apellido = 'Rodríguez';
    SET @IdAlum4 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Isabella' AND Apellido = 'Rodríguez');
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Santiago' AND Apellido = 'López')
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre4, 'Santiago', 'López', @IdBus, 14.6286, -90.5140, 'Ruta 6 9-55, Zona 4', 1, GETDATE());
    SET @IdAlum5 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus WHERE Nombre = 'Santiago' AND Apellido = 'López';
    SET @IdAlum5 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Santiago' AND Apellido = 'López');
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Valentina' AND Apellido = 'Hernández')
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre5, 'Valentina', 'Hernández', @IdBus, 14.6312, -90.5167, '11 Avenida 18-40, Zona 4', 1, GETDATE());
    SET @IdAlum6 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus WHERE Nombre = 'Valentina' AND Apellido = 'Hernández';
    SET @IdAlum6 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Valentina' AND Apellido = 'Hernández');
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Sebastián' AND Apellido = 'Gómez')
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre6, 'Sebastián', 'Gómez', @IdBus, 14.6097, -90.5245, '4a Avenida 12-20, Zona 9', 1, GETDATE());
    SET @IdAlum7 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus WHERE Nombre = 'Sebastián' AND Apellido = 'Gómez';
    SET @IdAlum7 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Sebastián' AND Apellido = 'Gómez');
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Camila' AND Apellido = 'Díaz')
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre7, 'Camila', 'Díaz', @IdBus, 14.6045, -90.5198, 'Blvd Los Próceres 22-45, Zona 10', 1, GETDATE());
    SET @IdAlum8 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus WHERE Nombre = 'Camila' AND Apellido = 'Díaz';
    SET @IdAlum8 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Camila' AND Apellido = 'Díaz');
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Matías' AND Apellido = 'Torres')
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre8, 'Matías', 'Torres', @IdBus, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 1, GETDATE());
    SET @IdAlum9 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus WHERE Nombre = 'Matías' AND Apellido = 'Torres';
    SET @IdAlum9 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Matías' AND Apellido = 'Torres');
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Lucía' AND Apellido = 'Torres')
BEGIN
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre8, 'Lucía', 'Torres', @IdBus, 14.5912, -90.5567, 'Calzada Roosevelt 32-10, Zona 11', 1, GETDATE());
    SET @IdAlum10 = SCOPE_IDENTITY();
END
ELSE
BEGIN
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus WHERE Nombre = 'Lucía' AND Apellido = 'Torres';
    SET @IdAlum10 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Lucía' AND Apellido = 'Torres');
END

PRINT '✓ Alumnos verificados/insertados';

-- ============================================
-- 4. RUTAS
-- ============================================
DECLARE @IdRutaManana INT, @IdRutaTarde INT;

IF NOT EXISTS (SELECT 1 FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = 'Mañana')
BEGIN
    INSERT INTO genesis.Rutas (IdBus, Nombre, Descripcion, TipoRuta, HoraInicio, EsActiva, Activo, FechaRegistro)
    VALUES (@IdBus, 'Ruta Mañana - BUS-001', 'Recorrido matutino', 'Mañana', '06:00:00', 1, 1, GETDATE());
    SET @IdRutaManana = SCOPE_IDENTITY();
END
ELSE
    SET @IdRutaManana = (SELECT IdRuta FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = 'Mañana');

IF NOT EXISTS (SELECT 1 FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = 'Tarde')
BEGIN
    INSERT INTO genesis.Rutas (IdBus, Nombre, Descripcion, TipoRuta, HoraInicio, EsActiva, Activo, FechaRegistro)
    VALUES (@IdBus, 'Ruta Tarde - BUS-001', 'Recorrido vespertino', 'Tarde', '14:00:00', 1, 1, GETDATE());
    SET @IdRutaTarde = SCOPE_IDENTITY();
END
ELSE
    SET @IdRutaTarde = (SELECT IdRuta FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = 'Tarde');

PRINT '✓ Rutas verificadas/insertadas';

-- ============================================
-- 5. PARADAS RUTA TARDE (La más importante para la demo)
-- ============================================
DELETE FROM genesis.Paradas WHERE IdRuta = @IdRutaTarde;

INSERT INTO genesis.Paradas (IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
VALUES 
    (@IdRutaTarde, NULL, 14.6350, -90.5125, 'Colegio Genesis, Zona 1', 1, '14:00:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum1, 14.6449, -90.5069, '5a Avenida 12-45, Zona 1', 2, '14:08:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum2, 14.6455, -90.5075, '5a Avenida 12-45, Zona 1', 3, '14:09:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum3, 14.6521, -90.5134, '12 Calle 8-30, Zona 2', 4, '14:15:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum4, 14.6489, -90.5089, '7a Avenida 15-22, Zona 2', 5, '14:20:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum5, 14.6286, -90.5140, 'Ruta 6 9-55, Zona 4', 6, '14:28:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum6, 14.6312, -90.5167, '11 Avenida 18-40, Zona 4', 7, '14:32:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum7, 14.6097, -90.5245, '4a Avenida 12-20, Zona 9', 8, '14:40:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum8, 14.6045, -90.5198, 'Blvd Los Próceres, Zona 10', 9, '14:45:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum9, 14.5912, -90.5567, 'Calzada Roosevelt, Zona 11', 10, '14:55:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum10, 14.5912, -90.5567, 'Calzada Roosevelt, Zona 11', 11, '14:56:00', 0, 1, GETDATE());

PRINT '✓ Paradas Ruta Tarde insertadas';

-- ============================================
-- VERIFICAR
-- ============================================
PRINT '';
PRINT '=========================================';
PRINT 'RESUMEN FINAL:';
SELECT 'Bus BUS-001' AS Tipo, COUNT(*) AS Cantidad FROM genesis.Buses WHERE Placa = 'BUS-001'
UNION ALL
SELECT 'Padres', COUNT(*) FROM genesis.Padres WHERE IdPadre IN (@IdPadre1, @IdPadre2, @IdPadre3, @IdPadre4, @IdPadre5, @IdPadre6, @IdPadre7, @IdPadre8)
UNION ALL
SELECT 'Alumnos', COUNT(*) FROM genesis.Alumnos WHERE IdBusAsignado = @IdBus
UNION ALL
SELECT 'Rutas', COUNT(*) FROM genesis.Rutas WHERE IdBus = @IdBus
UNION ALL
SELECT 'Paradas', COUNT(*) FROM genesis.Paradas WHERE IdRuta = @IdRutaTarde;

PRINT '';
PRINT '✓ ✓ ✓ DATOS INSERTADOS CORRECTAMENTE ✓ ✓ ✓';
PRINT 'Recarga el Dashboard Monitor (F5) para ver los alumnos';
PRINT '=========================================';
GO
