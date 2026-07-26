-- =============================================================================
-- seed_completo_demo_geolocalizacion.sql
-- TransportesGenesis — Datos demo geolocalización (BUS-001)
-- =============================================================================
-- Equivalente consolidado a:
--   EJECUTAR_SIMPLE_DashboardMonitor.sql  (bus, padres, alumnos, rutas tarde)
--   SeedData_DashboardMonitor_Test.sql      (paradas ruta mañana)
--   + vinculación padre1, asignación monitor/piloto, config colegio, GPS inicial
--
-- PREREQUISITOS:
--   1. dotnet ef database update
--   2. Arrancar la app UNA VEZ (Startup.cs crea usuarios Identity)
--
-- USO: SSMS / Azure Data Studio → ejecutar completo (F5)
-- =============================================================================

USE TransportesGenesis;
GO

SET NOCOUNT ON;
PRINT '================================================================';
PRINT ' seed_completo_demo_geolocalizacion — BUS-001';
PRINT ' Fecha: ' + CONVERT(VARCHAR(20), GETDATE(), 120);
PRINT '================================================================';
PRINT '';

-- =============================================================================
-- 1. CONFIGURACIÓN COLEGIO (mismas coords que paradas demo: Zona 1)
-- =============================================================================
IF EXISTS (SELECT 1 FROM sys.tables t
           JOIN sys.schemas s ON t.schema_id = s.schema_id
           WHERE s.name = 'genesis' AND t.name = 'ConfiguracionSistema')
BEGIN
    MERGE genesis.ConfiguracionSistema AS tgt
    USING (VALUES
        ('Colegio_Nombre',           N'Colegio Genesis',             N'Nombre institución', 'Texto',      'Ubicacion'),
        ('Colegio_Direccion',        N'Colegio Genesis, Zona 1',     N'Dirección colegio',  'Texto',      'Ubicacion'),
        ('Colegio_Latitud',          N'14.6350',                     N'Latitud colegio',    'Coordenada', 'Ubicacion'),
        ('Colegio_Longitud',         N'-90.5125',                    N'Longitud colegio',   'Coordenada', 'Ubicacion'),
        ('Colegio_HoraInicioClases', N'07:00',                       N'Hora inicio',        'Texto',      'General'),
        ('Colegio_HoraFinClases',    N'14:30',                       N'Hora fin',           'Texto',      'General')
    ) AS src (Clave, Valor, Descripcion, Tipo, Categoria)
    ON tgt.Clave = src.Clave
    WHEN MATCHED THEN
        UPDATE SET Valor = src.Valor, Descripcion = src.Descripcion, UltimaModificacion = GETDATE()
    WHEN NOT MATCHED THEN
        INSERT (Clave, Valor, Descripcion, Tipo, Categoria, Activo, FechaRegistro)
        VALUES (src.Clave, src.Valor, src.Descripcion, src.Tipo, src.Categoria, 1, GETDATE());

    PRINT '✓ Configuración colegio (14.6350, -90.5125)';
END
GO

-- =============================================================================
-- 2. BUS BUS-001
-- =============================================================================
IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE Placa = N'BUS-001')
BEGIN
    INSERT INTO genesis.Buses (Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
    VALUES (N'BUS-001', N'Mercedes-Benz Sprinter', 35, 1, 1, GETDATE());
    PRINT '✓ Bus BUS-001 insertado';
END
ELSE
    PRINT '✓ Bus BUS-001 ya existe';

DECLARE @IdBus INT = (SELECT IdBus FROM genesis.Buses WHERE Placa = N'BUS-001');
PRINT '  IdBus = ' + CAST(@IdBus AS VARCHAR(10));
GO

-- =============================================================================
-- 3. PADRES (8 familias — igual que EJECUTAR_SIMPLE)
-- =============================================================================
DECLARE @IdPadre1 INT, @IdPadre2 INT, @IdPadre3 INT, @IdPadre4 INT;
DECLARE @IdPadre5 INT, @IdPadre6 INT, @IdPadre7 INT, @IdPadre8 INT;

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'Carlos' AND Apellido = N'Martínez López')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES (N'Carlos', N'Martínez López', 1, GETDATE());
    SET @IdPadre1 = SCOPE_IDENTITY();
END ELSE
    SET @IdPadre1 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Carlos' AND Apellido = N'Martínez López');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'María' AND Apellido = N'García Hernández')
BEGIN INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES (N'María', N'García Hernández', 1, GETDATE()); SET @IdPadre2 = SCOPE_IDENTITY(); END
ELSE SET @IdPadre2 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'María' AND Apellido = N'García Hernández');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'José' AND Apellido = N'Rodríguez Pérez')
BEGIN INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES (N'José', N'Rodríguez Pérez', 1, GETDATE()); SET @IdPadre3 = SCOPE_IDENTITY(); END
ELSE SET @IdPadre3 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'José' AND Apellido = N'Rodríguez Pérez');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'Ana' AND Apellido = N'López González')
BEGIN INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES (N'Ana', N'López González', 1, GETDATE()); SET @IdPadre4 = SCOPE_IDENTITY(); END
ELSE SET @IdPadre4 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Ana' AND Apellido = N'López González');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'Luis' AND Apellido = N'Hernández Morales')
BEGIN INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES (N'Luis', N'Hernández Morales', 1, GETDATE()); SET @IdPadre5 = SCOPE_IDENTITY(); END
ELSE SET @IdPadre5 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Luis' AND Apellido = N'Hernández Morales');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'Patricia' AND Apellido = N'Gómez Ramírez')
BEGIN INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES (N'Patricia', N'Gómez Ramírez', 1, GETDATE()); SET @IdPadre6 = SCOPE_IDENTITY(); END
ELSE SET @IdPadre6 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Patricia' AND Apellido = N'Gómez Ramírez');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'Roberto' AND Apellido = N'Díaz Castro')
BEGIN INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES (N'Roberto', N'Díaz Castro', 1, GETDATE()); SET @IdPadre7 = SCOPE_IDENTITY(); END
ELSE SET @IdPadre7 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Roberto' AND Apellido = N'Díaz Castro');

IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'Carmen' AND Apellido = N'Torres Flores')
BEGIN INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro) VALUES (N'Carmen', N'Torres Flores', 1, GETDATE()); SET @IdPadre8 = SCOPE_IDENTITY(); END
ELSE SET @IdPadre8 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Carmen' AND Apellido = N'Torres Flores');

PRINT '✓ Padres demo (8)';
GO

-- =============================================================================
-- 4. VINCULAR padre1@gmail.com → Carlos Martínez López
-- =============================================================================
DECLARE @UserPadre1 NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE Email = N'padre1@gmail.com');

IF @UserPadre1 IS NULL
    PRINT '⚠ padre1@gmail.com no existe — arranca la app una vez.';
ELSE
BEGIN
    UPDATE genesis.Padres SET UsuarioId = @UserPadre1
    WHERE Nombre = N'Carlos' AND Apellido = N'Martínez López';
    PRINT '✓ padre1@gmail.com → Carlos Martínez López';
END
GO

-- =============================================================================
-- 5. ALUMNOS (10 en BUS-001 — mismas coords que EJECUTAR_SIMPLE)
-- =============================================================================
DECLARE @IdBus INT = (SELECT IdBus FROM genesis.Buses WHERE Placa = N'BUS-001');
DECLARE @IdPadre1 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Carlos' AND Apellido = N'Martínez López');
DECLARE @IdPadre2 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'María' AND Apellido = N'García Hernández');
DECLARE @IdPadre3 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'José' AND Apellido = N'Rodríguez Pérez');
DECLARE @IdPadre4 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Ana' AND Apellido = N'López González');
DECLARE @IdPadre5 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Luis' AND Apellido = N'Hernández Morales');
DECLARE @IdPadre6 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Patricia' AND Apellido = N'Gómez Ramírez');
DECLARE @IdPadre7 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Roberto' AND Apellido = N'Díaz Castro');
DECLARE @IdPadre8 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Carmen' AND Apellido = N'Torres Flores');

DECLARE @IdAlum1 INT, @IdAlum2 INT, @IdAlum3 INT, @IdAlum4 INT, @IdAlum5 INT;
DECLARE @IdAlum6 INT, @IdAlum7 INT, @IdAlum8 INT, @IdAlum9 INT, @IdAlum10 INT;

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Diego' AND Apellido = N'Martínez' AND IdPadre = @IdPadre1)
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre1, N'Diego', N'Martínez', @IdBus, 14.6449, -90.5069, N'5a Avenida 12-45, Zona 1', 1, GETDATE());
ELSE
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.6449, Longitud = -90.5069, Direccion = N'5a Avenida 12-45, Zona 1', Activo = 1
    WHERE Nombre = N'Diego' AND Apellido = N'Martínez' AND IdPadre = @IdPadre1;
SET @IdAlum1 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Diego' AND Apellido = N'Martínez' AND IdPadre = @IdPadre1);

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Sofía' AND Apellido = N'Martínez' AND IdPadre = @IdPadre1)
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre1, N'Sofía', N'Martínez', @IdBus, 14.6455, -90.5075, N'5a Avenida 12-45, Zona 1', 1, GETDATE());
ELSE
    UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.6455, Longitud = -90.5075, Direccion = N'5a Avenida 12-45, Zona 1', Activo = 1
    WHERE Nombre = N'Sofía' AND Apellido = N'Martínez' AND IdPadre = @IdPadre1;
SET @IdAlum2 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Sofía' AND Apellido = N'Martínez' AND IdPadre = @IdPadre1);

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Mateo' AND Apellido = N'García')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre2, N'Mateo', N'García', @IdBus, 14.6521, -90.5134, N'12 Calle 8-30, Zona 2', 1, GETDATE());
ELSE UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.6521, Longitud = -90.5134, Direccion = N'12 Calle 8-30, Zona 2', Activo = 1 WHERE Nombre = N'Mateo' AND Apellido = N'García';
SET @IdAlum3 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Mateo' AND Apellido = N'García');

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Isabella' AND Apellido = N'Rodríguez')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre3, N'Isabella', N'Rodríguez', @IdBus, 14.6489, -90.5089, N'7a Avenida 15-22, Zona 2', 1, GETDATE());
ELSE UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.6489, Longitud = -90.5089, Direccion = N'7a Avenida 15-22, Zona 2', Activo = 1 WHERE Nombre = N'Isabella' AND Apellido = N'Rodríguez';
SET @IdAlum4 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Isabella' AND Apellido = N'Rodríguez');

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Santiago' AND Apellido = N'López')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre4, N'Santiago', N'López', @IdBus, 14.6286, -90.5140, N'Ruta 6 9-55, Zona 4', 1, GETDATE());
ELSE UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.6286, Longitud = -90.5140, Direccion = N'Ruta 6 9-55, Zona 4', Activo = 1 WHERE Nombre = N'Santiago' AND Apellido = N'López';
SET @IdAlum5 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Santiago' AND Apellido = N'López');

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Valentina' AND Apellido = N'Hernández')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre5, N'Valentina', N'Hernández', @IdBus, 14.6312, -90.5167, N'11 Avenida 18-40, Zona 4', 1, GETDATE());
ELSE UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.6312, Longitud = -90.5167, Direccion = N'11 Avenida 18-40, Zona 4', Activo = 1 WHERE Nombre = N'Valentina' AND Apellido = N'Hernández';
SET @IdAlum6 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Valentina' AND Apellido = N'Hernández');

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Sebastián' AND Apellido = N'Gómez')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre6, N'Sebastián', N'Gómez', @IdBus, 14.6097, -90.5245, N'4a Avenida 12-20, Zona 9', 1, GETDATE());
ELSE UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.6097, Longitud = -90.5245, Direccion = N'4a Avenida 12-20, Zona 9', Activo = 1 WHERE Nombre = N'Sebastián' AND Apellido = N'Gómez';
SET @IdAlum7 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Sebastián' AND Apellido = N'Gómez');

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Camila' AND Apellido = N'Díaz')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre7, N'Camila', N'Díaz', @IdBus, 14.6045, -90.5198, N'Blvd Los Próceres 22-45, Zona 10', 1, GETDATE());
ELSE UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.6045, Longitud = -90.5198, Direccion = N'Blvd Los Próceres 22-45, Zona 10', Activo = 1 WHERE Nombre = N'Camila' AND Apellido = N'Díaz';
SET @IdAlum8 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Camila' AND Apellido = N'Díaz');

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Matías' AND Apellido = N'Torres')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre8, N'Matías', N'Torres', @IdBus, 14.5912, -90.5567, N'Calzada Roosevelt 32-10, Zona 11', 1, GETDATE());
ELSE UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.5912, Longitud = -90.5567, Direccion = N'Calzada Roosevelt 32-10, Zona 11', Activo = 1 WHERE Nombre = N'Matías' AND Apellido = N'Torres';
SET @IdAlum9 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Matías' AND Apellido = N'Torres');

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Lucía' AND Apellido = N'Torres')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre8, N'Lucía', N'Torres', @IdBus, 14.5912, -90.5567, N'Calzada Roosevelt 32-10, Zona 11', 1, GETDATE());
ELSE UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus, Latitud = 14.5912, Longitud = -90.5567, Direccion = N'Calzada Roosevelt 32-10, Zona 11', Activo = 1 WHERE Nombre = N'Lucía' AND Apellido = N'Torres';
SET @IdAlum10 = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Lucía' AND Apellido = N'Torres');

PRINT '✓ 10 alumnos BUS-001 (Diego + Sofía → padre1)';
GO

-- =============================================================================
-- 6. RUTAS MAÑANA Y TARDE (nombres iguales a EJECUTAR_SIMPLE)
-- =============================================================================
DECLARE @IdBus INT = (SELECT IdBus FROM genesis.Buses WHERE Placa = N'BUS-001');
DECLARE @IdRutaManana INT, @IdRutaTarde INT;

IF NOT EXISTS (SELECT 1 FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = N'Mañana')
BEGIN
    INSERT INTO genesis.Rutas (IdBus, Nombre, Descripcion, TipoRuta, HoraInicio, EsActiva, Activo, FechaRegistro)
    VALUES (@IdBus, N'Ruta Mañana - BUS-001', N'Recorrido matutino', N'Mañana', '06:00:00', 1, 1, GETDATE());
    SET @IdRutaManana = SCOPE_IDENTITY();
END ELSE
    SET @IdRutaManana = (SELECT IdRuta FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = N'Mañana');

IF NOT EXISTS (SELECT 1 FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = N'Tarde')
BEGIN
    INSERT INTO genesis.Rutas (IdBus, Nombre, Descripcion, TipoRuta, HoraInicio, EsActiva, Activo, FechaRegistro)
    VALUES (@IdBus, N'Ruta Tarde - BUS-001', N'Recorrido vespertino', N'Tarde', '14:00:00', 1, 1, GETDATE());
    SET @IdRutaTarde = SCOPE_IDENTITY();
END ELSE
    SET @IdRutaTarde = (SELECT IdRuta FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = N'Tarde');

UPDATE genesis.Rutas SET EsActiva = 1, Activo = 1, Nombre = N'Ruta Mañana - BUS-001', Descripcion = N'Recorrido matutino'
WHERE IdRuta = @IdRutaManana;
UPDATE genesis.Rutas SET EsActiva = 1, Activo = 1, Nombre = N'Ruta Tarde - BUS-001', Descripcion = N'Recorrido vespertino'
WHERE IdRuta = @IdRutaTarde;

PRINT '✓ Rutas Mañana y Tarde activas';
GO

-- =============================================================================
-- 7. PARADAS (Mañana: SeedData_DashboardMonitor | Tarde: EJECUTAR_SIMPLE)
-- =============================================================================
DECLARE @IdBus INT = (SELECT IdBus FROM genesis.Buses WHERE Placa = N'BUS-001');
DECLARE @IdRutaManana INT = (SELECT IdRuta FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = N'Mañana');
DECLARE @IdRutaTarde INT = (SELECT IdRuta FROM genesis.Rutas WHERE IdBus = @IdBus AND TipoRuta = N'Tarde');
DECLARE @IdPadre1 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Carlos' AND Apellido = N'Martínez López');

DECLARE @IdAlum1 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Diego' AND Apellido = N'Martínez' AND IdPadre = @IdPadre1);
DECLARE @IdAlum2 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Sofía' AND Apellido = N'Martínez' AND IdPadre = @IdPadre1);
DECLARE @IdAlum3 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Mateo' AND Apellido = N'García');
DECLARE @IdAlum4 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Isabella' AND Apellido = N'Rodríguez');
DECLARE @IdAlum5 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Santiago' AND Apellido = N'López');
DECLARE @IdAlum6 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Valentina' AND Apellido = N'Hernández');
DECLARE @IdAlum7 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Sebastián' AND Apellido = N'Gómez');
DECLARE @IdAlum8 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Camila' AND Apellido = N'Díaz');
DECLARE @IdAlum9 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Matías' AND Apellido = N'Torres');
DECLARE @IdAlum10 INT = (SELECT IdAlumno FROM genesis.Alumnos WHERE Nombre = N'Lucía' AND Apellido = N'Torres');

DELETE FROM genesis.Paradas WHERE IdRuta = @IdRutaManana;
INSERT INTO genesis.Paradas (IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
VALUES
    (@IdRutaManana, @IdAlum9,  14.5912, -90.5567, N'Calzada Roosevelt 32-10, Zona 11', 1,  '06:05:00', 0, 1, GETDATE()),
    (@IdRutaManana, @IdAlum10, 14.5912, -90.5567, N'Calzada Roosevelt 32-10, Zona 11', 2,  '06:06:00', 0, 1, GETDATE()),
    (@IdRutaManana, @IdAlum8,  14.6045, -90.5198, N'Blvd Los Próceres 22-45, Zona 10', 3,  '06:15:00', 0, 1, GETDATE()),
    (@IdRutaManana, @IdAlum7,  14.6097, -90.5245, N'4a Avenida 12-20, Zona 9',         4,  '06:20:00', 0, 1, GETDATE()),
    (@IdRutaManana, @IdAlum6,  14.6312, -90.5167, N'11 Avenida 18-40, Zona 4',         5,  '06:28:00', 0, 1, GETDATE()),
    (@IdRutaManana, @IdAlum5,  14.6286, -90.5140, N'Ruta 6 9-55, Zona 4',              6,  '06:32:00', 0, 1, GETDATE()),
    (@IdRutaManana, @IdAlum4,  14.6489, -90.5089, N'7a Avenida 15-22, Zona 2',         7,  '06:40:00', 0, 1, GETDATE()),
    (@IdRutaManana, @IdAlum3,  14.6521, -90.5134, N'12 Calle 8-30, Zona 2',            8,  '06:45:00', 0, 1, GETDATE()),
    (@IdRutaManana, @IdAlum1,  14.6449, -90.5069, N'5a Avenida 12-45, Zona 1',         9,  '06:52:00', 0, 1, GETDATE()),
    (@IdRutaManana, @IdAlum2,  14.6455, -90.5075, N'5a Avenida 12-45, Zona 1',         10, '06:53:00', 0, 1, GETDATE()),
    (@IdRutaManana, NULL,      14.6350, -90.5125, N'Colegio Genesis, Zona 1',          11, '07:00:00', 0, 1, GETDATE());

DELETE FROM genesis.Paradas WHERE IdRuta = @IdRutaTarde;
INSERT INTO genesis.Paradas (IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
VALUES
    (@IdRutaTarde, NULL,      14.6350, -90.5125, N'Colegio Genesis, Zona 1',          1,  '14:00:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum1,  14.6449, -90.5069, N'5a Avenida 12-45, Zona 1',         2,  '14:08:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum2,  14.6455, -90.5075, N'5a Avenida 12-45, Zona 1',         3,  '14:09:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum3,  14.6521, -90.5134, N'12 Calle 8-30, Zona 2',            4,  '14:15:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum4,  14.6489, -90.5089, N'7a Avenida 15-22, Zona 2',         5,  '14:20:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum5,  14.6286, -90.5140, N'Ruta 6 9-55, Zona 4',              6,  '14:28:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum6,  14.6312, -90.5167, N'11 Avenida 18-40, Zona 4',         7,  '14:32:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum7,  14.6097, -90.5245, N'4a Avenida 12-20, Zona 9',         8,  '14:40:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum8,  14.6045, -90.5198, N'Blvd Los Próceres, Zona 10',       9,  '14:45:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum9,  14.5912, -90.5567, N'Calzada Roosevelt, Zona 11',       10, '14:55:00', 0, 1, GETDATE()),
    (@IdRutaTarde, @IdAlum10, 14.5912, -90.5567, N'Calzada Roosevelt, Zona 11',       11, '14:56:00', 0, 1, GETDATE());

PRINT '✓ Paradas: 11 Mañana + 11 Tarde';
GO

-- =============================================================================
-- 8. ASIGNACIONES monitor1/monitor2 y piloto2 → BUS-001
-- =============================================================================
DECLARE @IdBus INT = (SELECT IdBus FROM genesis.Buses WHERE Placa = N'BUS-001');

DECLARE @UserMonitor NVARCHAR(450) = (
    SELECT TOP 1 u.Id FROM AspNetUsers u
    INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
    INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
    WHERE r.Name = N'Monitor'
      AND u.Email IN (N'monitor1@transportesgenesis.com', N'monitor2@transportesgenesis.com')
    ORDER BY CASE u.Email WHEN N'monitor1@transportesgenesis.com' THEN 0 ELSE 1 END
);

DECLARE @UserPiloto NVARCHAR(450) = (
    SELECT TOP 1 u.Id FROM AspNetUsers u
    INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
    INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
    WHERE r.Name = N'Piloto'
      AND u.Email IN (N'piloto2@transportesgenesis.com', N'piloto1@transportesgenesis.com')
    ORDER BY CASE u.Email WHEN N'piloto2@transportesgenesis.com' THEN 0 ELSE 1 END
);

IF @UserMonitor IS NOT NULL AND @IdBus IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM genesis.AsignacionPilotoBus WHERE IdUsuarioPiloto = @UserMonitor AND IdBus = @IdBus AND EsActual = 1)
    BEGIN
        INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
        VALUES (@UserMonitor, @IdBus, GETDATE(), 1, 1, GETDATE());
    END
    PRINT '✓ Monitor asignado a BUS-001';
END
ELSE
    PRINT '⚠ Sin monitor — asignar manualmente en Admin/GestionarAsignaciones';

IF @UserPiloto IS NOT NULL AND @IdBus IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM genesis.AsignacionPilotoBus WHERE IdUsuarioPiloto = @UserPiloto AND IdBus = @IdBus AND EsActual = 1)
    BEGIN
        INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
        VALUES (@UserPiloto, @IdBus, GETDATE(), 1, 1, GETDATE());
    END
    PRINT '✓ Piloto asignado a BUS-001';
END
GO

-- =============================================================================
-- 9. UBICACIÓN INICIAL (mapa admin)
-- =============================================================================
DECLARE @IdBus INT = (SELECT IdBus FROM genesis.Buses WHERE Placa = N'BUS-001');

IF @IdBus IS NOT NULL
BEGIN
    INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, Velocidad, Direccion, FechaHora)
    VALUES (@IdBus, 14.6350, -90.5125, 0, 0, GETDATE());
    PRINT '✓ Ubicación inicial en colegio';
END
GO

-- =============================================================================
-- 10. VERIFICACIÓN
-- =============================================================================
PRINT '';
PRINT '================================================================';
PRINT ' RESUMEN (debe coincidir con tu entorno local)';
PRINT '================================================================';

SELECT 'Bus BUS-001' AS Item, COUNT(*) AS Cantidad FROM genesis.Buses WHERE Placa = N'BUS-001'
UNION ALL SELECT 'Alumnos en BUS-001', COUNT(*) FROM genesis.Alumnos a INNER JOIN genesis.Buses b ON a.IdBusAsignado = b.IdBus WHERE b.Placa = N'BUS-001'
UNION ALL SELECT 'Paradas Mañana', COUNT(*) FROM genesis.Paradas p INNER JOIN genesis.Rutas r ON p.IdRuta = r.IdRuta INNER JOIN genesis.Buses b ON r.IdBus = b.IdBus WHERE b.Placa = N'BUS-001' AND r.TipoRuta = N'Mañana'
UNION ALL SELECT 'Paradas Tarde', COUNT(*) FROM genesis.Paradas p INNER JOIN genesis.Rutas r ON p.IdRuta = r.IdRuta INNER JOIN genesis.Buses b ON r.IdBus = b.IdBus WHERE b.Placa = N'BUS-001' AND r.TipoRuta = N'Tarde';

PRINT '';
SELECT u.Email AS Padre, a.Nombre + ' ' + a.Apellido AS Hijo, b.Placa, b.IdBus
FROM genesis.Padres p
INNER JOIN AspNetUsers u ON p.UsuarioId = u.Id
INNER JOIN genesis.Alumnos a ON a.IdPadre = p.IdPadre
INNER JOIN genesis.Buses b ON a.IdBusAsignado = b.IdBus
WHERE u.Email = N'padre1@gmail.com';

PRINT '';
PRINT 'Credenciales: padre1@gmail.com | monitor1 o monitor2 | Admin123!';
PRINT 'Guía: GUIA_GEOLOCALIZACION_COMPLETA.md';
PRINT '================================================================';
GO
