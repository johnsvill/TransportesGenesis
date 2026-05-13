-- =========================================
-- Script FINAL: Datos Dashboard Monitor
-- Funciona con tabla Alumnos SIN IDENTITY
-- =========================================

USE TransportesGenesis;
GO

PRINT '========================================='
PRINT 'Insertando datos de prueba Dashboard Monitor'
PRINT '========================================='

-- Obtener el próximo ID disponible para Alumnos
DECLARE @NextIdAlumno INT = ISNULL((SELECT MAX(IdAlumno) FROM genesis.Alumnos), 0) + 1;
DECLARE @IdBus INT;
DECLARE @IdPadre1 INT, @IdPadre2 INT, @IdPadre3 INT, @IdPadre4 INT, @IdPadre5 INT, @IdPadre6 INT, @IdPadre7 INT, @IdPadre8 INT;
DECLARE @IdAlum1 INT, @IdAlum2 INT, @IdAlum3 INT, @IdAlum4 INT, @IdAlum5 INT, @IdAlum6 INT, @IdAlum7 INT, @IdAlum8 INT, @IdAlum9 INT, @IdAlum10 INT;
DECLARE @IdRutaTarde INT;

-- ============================================
-- 1. BUS
-- ============================================
IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE Placa = 'BUS-001')
BEGIN
    INSERT INTO genesis.Buses (Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
    VALUES ('BUS-001', 'Mercedes-Benz Sprinter', 35, 1, 1, GETDATE());
END

SET @IdBus = (SELECT IdBus FROM genesis.Buses WHERE Placa = 'BUS-001');
PRINT '✓ Bus BUS-001: ID = ' + CAST(@IdBus AS VARCHAR);

-- ============================================
-- 2. PADRES
-- ============================================
IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Carlos' AND Apellido = 'Martínez López')
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES ('Carlos', 'Martínez López', 1, GETDATE());
SET @IdPadre1 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Carlos' AND Apellido = 'Martínez López' ORDER BY IdPadre DESC);

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'María' AND Apellido = 'García Hernández')
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES ('María', 'García Hernández', 1, GETDATE());
SET @IdPadre2 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'María' AND Apellido = 'García Hernández' ORDER BY IdPadre DESC);

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'José' AND Apellido = 'Rodríguez Pérez')
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES ('José', 'Rodríguez Pérez', 1, GETDATE());
SET @IdPadre3 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'José' AND Apellido = 'Rodríguez Pérez' ORDER BY IdPadre DESC);

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Ana' AND Apellido = 'López González')
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES ('Ana', 'López González', 1, GETDATE());
SET @IdPadre4 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Ana' AND Apellido = 'López González' ORDER BY IdPadre DESC);

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Luis' AND Apellido = 'Hernández Morales')
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES ('Luis', 'Hernández Morales', 1, GETDATE());
SET @IdPadre5 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Luis' AND Apellido = 'Hernández Morales' ORDER BY IdPadre DESC);

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Patricia' AND Apellido = 'Gómez Ramírez')
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES ('Patricia', 'Gómez Ramírez', 1, GETDATE());
SET @IdPadre6 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Patricia' AND Apellido = 'Gómez Ramírez' ORDER BY IdPadre DESC);

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Roberto' AND Apellido = 'Díaz Castro')
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES ('Roberto', 'Díaz Castro', 1, GETDATE());
SET @IdPadre7 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Roberto' AND Apellido = 'Díaz Castro' ORDER BY IdPadre DESC);

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Carmen' AND Apellido = 'Torres Flores')
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES ('Carmen', 'Torres Flores', 1, GETDATE());
SET @IdPadre8 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = 'Carmen' AND Apellido = 'Torres Flores' ORDER BY IdPadre DESC);

PRINT '✓ 8 Padres verificados/insertados';

-- ============================================
-- 3. ALUMNOS (Especificando IdAlumno manualmente)
-- ============================================
SET @IdAlum1 = @NextIdAlumno;
SET @IdAlum2 = @NextIdAlumno + 1;
SET @IdAlum3 = @NextIdAlumno + 2;
SET @IdAlum4 = @NextIdAlumno + 3;
SET @IdAlum5 = @NextIdAlumno + 4;
SET @IdAlum6 = @NextIdAlumno + 5;
SET @IdAlum7 = @NextIdAlumno + 6;
SET @IdAlum8 = @NextIdAlumno + 7;
SET @IdAlum9 = @NextIdAlumno + 8;
SET @IdAlum10 = @NextIdAlumno + 9;

-- Eliminar alumnos de prueba anteriores si existen
DELETE FROM genesis.Paradas WHERE IdAlumno IN (@IdAlum1, @IdAlum2, @IdAlum3, @IdAlum4, @IdAlum5, @IdAlum6, @IdAlum7, @IdAlum8, @IdAlum9, @IdAlum10);
DELETE FROM genesis.Alumnos WHERE IdAlumno IN (@IdAlum1, @IdAlum2, @IdAlum3, @IdAlum4, @IdAlum5, @IdAlum6, @IdAlum7, @IdAlum8, @IdAlum9, @IdAlum10);

INSERT INTO genesis.Alumnos (IdAlumno, IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
VALUES 
    (@IdAlum1, @IdPadre1, 'Diego', 'Martínez', @IdBus, 14.6449, -90.5069, '5a Avenida 12-45, Zona 1', 1, GETDATE()),
    (@IdAlum2, @IdPadre1, 'Sofía', 'Martínez', @IdBus, 14.6455, -90.5075, '5a Avenida 12-45, Zona 1', 1, GETDATE()),
    (@IdAlum3, @IdPadre2, 'Mateo', 'García', @IdBus, 14.6521, -90.5134, '12 Calle 8-30, Zona 2', 1, GETDATE()),
    (@IdAlum4, @IdPadre3, 'Isabella', 'Rodríguez', @IdBus, 14.6489, -90.5089, '7a Avenida 15-22, Zona 2', 1, GETDATE()),
    (@IdAlum5, @IdPadre4, 'Santiago', 'López', @IdBus, 14.6286, -90.5140, 'Ruta 6 9-55, Zona 4', 1, GETDATE()),
    (@IdAlum6, @IdPadre5, 'Valentina', 'Hernández', @IdBus, 14.6312, -90.5167, '11 Avenida 18-40, Zona 4', 1, GETDATE()),
    (@IdAlum7, @IdPadre6, 'Sebastián', 'Gómez', @IdBus, 14.6097, -90.5245, '4a Avenida 12-20, Zona 9', 1, GETDATE()),
    (@IdAlum8, @IdPadre7, 'Camila', 'Díaz', @IdBus, 14.6045, -90.5198, 'Blvd Los Próceres, Zona 10', 1, GETDATE()),
    (@IdAlum9, @IdPadre8, 'Matías', 'Torres', @IdBus, 14.5912, -90.5567, 'Calzada Roosevelt, Zona 11', 1, GETDATE()),
    (@IdAlum10, @IdPadre8, 'Lucía', 'Torres', @IdBus, 14.5912, -90.5567, 'Calzada Roosevelt, Zona 11', 1, GETDATE());

PRINT '✓ 10 Alumnos insertados (IDs: ' + CAST(@IdAlum1 AS VARCHAR) + ' - ' + CAST(@IdAlum10 AS VARCHAR) + ')';

-- ============================================
-- 4. RUTAS
-- ============================================
IF NOT EXISTS (SELECT 1 FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = 'Tarde')
BEGIN
    INSERT INTO genesis.Rutas (IdBus, Nombre, Descripcion, TipoRuta, HoraInicio, EsActiva, Activo, FechaRegistro)
    VALUES (@IdBus, 'Ruta Tarde - BUS-001', 'Recorrido vespertino', 'Tarde', '14:00:00', 1, 1, GETDATE());
END
ELSE
BEGIN
    UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdBus = @IdBus AND TipoRuta = 'Tarde';
END

SET @IdRutaTarde = (SELECT IdRuta FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = 'Tarde');
PRINT '✓ Ruta Tarde creada/actualizada: ID = ' + CAST(@IdRutaTarde AS VARCHAR);

-- ============================================
-- 5. PARADAS RUTA TARDE
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

PRINT '✓ 11 Paradas insertadas';

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

SELECT 'Alumnos en Bus BUS-001:' AS Info;
SELECT a.IdAlumno, a.Nombre + ' ' + a.Apellido AS NombreCompleto, a.Direccion
FROM genesis.Alumnos a
WHERE a.IdBusAsignado = @IdBus
ORDER BY a.IdAlumno;

PRINT '';
PRINT '=========================================';
PRINT '✓ ✓ ✓ DATOS INSERTADOS CORRECTAMENTE ✓ ✓ ✓';
PRINT '';
PRINT 'SIGUIENTE PASO:';
PRINT '1. Recarga el Dashboard Monitor (F5 en el navegador)';
PRINT '2. Deberías ver los 10 alumnos listados';
PRINT '=========================================';
GO
