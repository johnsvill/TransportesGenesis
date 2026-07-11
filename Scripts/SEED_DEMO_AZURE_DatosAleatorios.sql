-- =====================================================
-- SEED DEMO AZURE — Datos aleatorios / demo piloto
-- =====================================================
-- Propósito: poblar BD vacía o casi vacía para tesis/demo.
-- Seguro de re-ejecutar (usa IF NOT EXISTS / MERGE por placa/email).
--
-- Cómo usarlo:
-- 1. En Azure Data Studio / SSMS conecta a TransportesGenesis_db
-- 2. Selecciona la BD correcta (cambia el USE si tu BD tiene otro nombre)
-- 3. Ejecuta TODO el script
-- 4. Luego en la app: Admin → Calcular Rutas (Mañana y Tarde)
--
-- Prerrequisito: la app ya debe haber arrancado al menos una vez
-- para crear roles y usuarios Identity (admin, padre1, piloto2, monitor2…).
-- Contraseña típica seed: Admin123!
-- =====================================================

-- IMPORTANTE: cambia el nombre si en Azure es otro
-- USE TransportesGenesis_db;
-- GO

SET NOCOUNT ON;
PRINT '========================================';
PRINT 'SEED DEMO AZURE — inicio';
PRINT '========================================';
PRINT '';

-- -----------------------------------------------------
-- 1) Colegio (parada fija) — Guatemala Zona 4
-- -----------------------------------------------------
MERGE genesis.ConfiguracionSistema AS t
USING (VALUES
    ('Colegio_Nombre',          N'Colegio Genesis Demo',            N'Nombre institución',        N'Texto',      N'Ubicacion'),
    ('Colegio_Direccion',       N'Zona 4, Ciudad de Guatemala',     N'Dirección colegio',         N'Texto',      N'Ubicacion'),
    ('Colegio_Latitud',         N'14.6270',                         N'Latitud colegio',           N'Coordenada', N'Ubicacion'),
    ('Colegio_Longitud',        N'-90.5125',                        N'Longitud colegio',          N'Coordenada', N'Ubicacion'),
    ('Colegio_HoraInicioClases',N'07:00',                           N'Inicio clases',             N'Texto',      N'General'),
    ('Colegio_HoraFinClases',   N'14:30',                           N'Fin clases',                N'Texto',      N'General')
) AS s(Clave, Valor, Descripcion, Tipo, Categoria)
ON t.Clave = s.Clave
WHEN MATCHED THEN
    UPDATE SET Valor = s.Valor, Descripcion = s.Descripcion, Tipo = s.Tipo,
               Categoria = s.Categoria, Activo = 1, UltimaModificacion = GETDATE(), ModificadoPor = N'seed_azure'
WHEN NOT MATCHED THEN
    INSERT (Clave, Valor, Descripcion, Tipo, Categoria, Activo, FechaRegistro, UltimaModificacion, ModificadoPor)
    VALUES (s.Clave, s.Valor, s.Descripcion, s.Tipo, s.Categoria, 1, GETDATE(), GETDATE(), N'seed_azure');

PRINT 'OK Colegio / ConfiguracionSistema';

-- -----------------------------------------------------
-- 2) Buses demo (3)
-- -----------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE Placa = N'DEMO-001')
    INSERT INTO genesis.Buses (Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
    VALUES (N'DEMO-001', N'Mercedes-Benz Sprinter', 28, 1, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE Placa = N'DEMO-002')
    INSERT INTO genesis.Buses (Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
    VALUES (N'DEMO-002', N'Volkswagen Crafter', 32, 1, 1, GETDATE());

IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE Placa = N'DEMO-003')
    INSERT INTO genesis.Buses (Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
    VALUES (N'DEMO-003', N'Ford Transit', 24, 1, 1, GETDATE());

PRINT 'OK Buses DEMO-001 / DEMO-002 / DEMO-003';

DECLARE @IdBus1 INT = (SELECT TOP 1 IdBus FROM genesis.Buses WHERE Placa = N'DEMO-001');
DECLARE @IdBus2 INT = (SELECT TOP 1 IdBus FROM genesis.Buses WHERE Placa = N'DEMO-002');
DECLARE @IdBus3 INT = (SELECT TOP 1 IdBus FROM genesis.Buses WHERE Placa = N'DEMO-003');

-- -----------------------------------------------------
-- 3) Padres de negocio vinculados a usuarios Identity
-- -----------------------------------------------------
DECLARE @UserPadre1 NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE Email = N'padre1@gmail.com' OR UserName = N'padre1');
DECLARE @UserPadre2 NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE Email = N'padre2@gmail.com' OR UserName = N'padre2');
DECLARE @UserPiloto NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE Email = N'piloto2@transportesgenesis.com' OR UserName = N'piloto2');
DECLARE @UserMonitor NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetUsers WHERE Email = N'monitor2@transportesgenesis.com' OR UserName = N'monitor2');

IF @UserPadre1 IS NULL
    PRINT 'AVISO: no existe padre1@gmail.com en AspNetUsers — arranca la app una vez para el seed de Identity';
IF @UserPiloto IS NULL
    PRINT 'AVISO: no existe piloto2@transportesgenesis.com — arranca la app una vez';

-- Padre 1
IF @UserPadre1 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE UsuarioId = @UserPadre1)
    INSERT INTO genesis.Padres (Nombre, Apellido, UsuarioId, Activo, FechaRegistro)
    VALUES (N'Juan', N'Pérez', @UserPadre1, 1, GETDATE());

IF @UserPadre1 IS NOT NULL
    UPDATE genesis.Padres SET Nombre = N'Juan', Apellido = N'Pérez', Activo = 1
    WHERE UsuarioId = @UserPadre1 AND (Nombre IS NULL OR Nombre = N'');

-- Padre 2
IF @UserPadre2 IS NOT NULL AND NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE UsuarioId = @UserPadre2)
    INSERT INTO genesis.Padres (Nombre, Apellido, UsuarioId, Activo, FechaRegistro)
    VALUES (N'María', N'González', @UserPadre2, 1, GETDATE());

DECLARE @IdPadre1 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE UsuarioId = @UserPadre1);
DECLARE @IdPadre2 INT = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE UsuarioId = @UserPadre2);

-- Si no hay Identity aún, crear padres sueltos para no fallar el resto
IF @IdPadre1 IS NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'Juan' AND Apellido = N'Pérez Demo')
        INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
        VALUES (N'Juan', N'Pérez Demo', 1, GETDATE());
    SET @IdPadre1 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'Juan' AND Apellido = N'Pérez Demo');
END

IF @IdPadre2 IS NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = N'María' AND Apellido = N'González Demo')
        INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
        VALUES (N'María', N'González Demo', 1, GETDATE());
    SET @IdPadre2 = (SELECT TOP 1 IdPadre FROM genesis.Padres WHERE Nombre = N'María' AND Apellido = N'González Demo');
END

PRINT CONCAT('OK Padres IdPadre1=', @IdPadre1, ' IdPadre2=', ISNULL(CAST(@IdPadre2 AS NVARCHAR(20)), 'NULL'));

-- -----------------------------------------------------
-- 4) Alumnos con GPS (zonas Guatemala) — 2 por bus
-- -----------------------------------------------------
-- Bus 1
IF @IdPadre1 IS NOT NULL AND @IdBus1 IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Diego' AND Apellido = N'Pérez Demo')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre1, N'Diego', N'Pérez Demo', @IdBus1, 14.6105, -90.5250, N'Casa Diego — Zona 9', 1, GETDATE());

IF @IdPadre1 IS NOT NULL AND @IdBus1 IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Sofía' AND Apellido = N'Pérez Demo')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre1, N'Sofía', N'Pérez Demo', @IdBus1, 14.6180, -90.5180, N'Casa Sofía — Zona 10', 1, GETDATE());

-- Bus 2
IF @IdPadre2 IS NOT NULL AND @IdBus2 IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Luis' AND Apellido = N'González Demo')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre2, N'Luis', N'González Demo', @IdBus2, 14.6350, -90.5060, N'Casa Luis — Zona 1', 1, GETDATE());

IF @IdPadre2 IS NOT NULL AND @IdBus2 IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Ana' AND Apellido = N'González Demo')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre2, N'Ana', N'González Demo', @IdBus2, 14.6280, -90.5000, N'Casa Ana — Zona 4', 1, GETDATE());

-- Bus 3 (mismo padre1 para demo multi-bus aviso)
IF @IdPadre1 IS NOT NULL AND @IdBus3 IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Carlos' AND Apellido = N'Pérez Demo')
    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadre1, N'Carlos', N'Pérez Demo', @IdBus3, 14.6020, -90.5400, N'Casa Carlos — Zona 7', 1, GETDATE());

PRINT 'OK Alumnos demo con GPS';

-- Asegurar GPS / bus en alumnos demo si ya existían
UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus1, Latitud = 14.6105, Longitud = -90.5250, Activo = 1
WHERE Nombre = N'Diego' AND Apellido = N'Pérez Demo' AND @IdBus1 IS NOT NULL;
UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus1, Latitud = 14.6180, Longitud = -90.5180, Activo = 1
WHERE Nombre = N'Sofía' AND Apellido = N'Pérez Demo' AND @IdBus1 IS NOT NULL;
UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus2, Latitud = 14.6350, Longitud = -90.5060, Activo = 1
WHERE Nombre = N'Luis' AND Apellido = N'González Demo' AND @IdBus2 IS NOT NULL;
UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus2, Latitud = 14.6280, Longitud = -90.5000, Activo = 1
WHERE Nombre = N'Ana' AND Apellido = N'González Demo' AND @IdBus2 IS NOT NULL;
UPDATE genesis.Alumnos SET IdBusAsignado = @IdBus3, Latitud = 14.6020, Longitud = -90.5400, Activo = 1
WHERE Nombre = N'Carlos' AND Apellido = N'Pérez Demo' AND @IdBus3 IS NOT NULL;

-- -----------------------------------------------------
-- 5) Asignación piloto / monitor → buses
-- -----------------------------------------------------
IF @UserPiloto IS NOT NULL AND @IdBus1 IS NOT NULL
BEGIN
    UPDATE genesis.AsignacionPilotoBus SET EsActual = 0, FechaFinAsignacion = GETDATE()
    WHERE IdBus = @IdBus1 AND EsActual = 1;

    IF NOT EXISTS (
        SELECT 1 FROM genesis.AsignacionPilotoBus
        WHERE IdUsuarioPiloto = @UserPiloto AND IdBus = @IdBus1 AND EsActual = 1)
    BEGIN
        INSERT INTO genesis.AsignacionPilotoBus
            (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
        VALUES (@UserPiloto, @IdBus1, GETDATE(), 1, 1, GETDATE());
    END
    PRINT 'OK Piloto2 → DEMO-001';
END

IF @UserMonitor IS NOT NULL AND @IdBus2 IS NOT NULL
BEGIN
    UPDATE genesis.AsignacionPilotoBus SET EsActual = 0, FechaFinAsignacion = GETDATE()
    WHERE IdBus = @IdBus2 AND EsActual = 1;

    IF NOT EXISTS (
        SELECT 1 FROM genesis.AsignacionPilotoBus
        WHERE IdUsuarioPiloto = @UserMonitor AND IdBus = @IdBus2 AND EsActual = 1)
    BEGIN
        INSERT INTO genesis.AsignacionPilotoBus
            (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
        VALUES (@UserMonitor, @IdBus2, GETDATE(), 1, 1, GETDATE());
    END
    PRINT 'OK Monitor2 → DEMO-002';
END

-- -----------------------------------------------------
-- 6) Asistencias confirmadas (hoy + 15 días hábiles)
-- -----------------------------------------------------
DECLARE @FechaIter DATE = CAST(GETDATE() AS DATE);
DECLARE @DiasOk INT = 0;

WHILE @DiasOk < 15
BEGIN
    IF DATEPART(WEEKDAY, @FechaIter) NOT IN (1, 7) -- Dom=1, Sáb=7 (DATEFIRST default)
    BEGIN
        INSERT INTO genesis.AsistenciaAlumno (
            IdAlumno, Fecha, AsisteMañana, AsisteTarde,
            FechaConfirmacion, IdBusTemporalMañana, IdBusTemporalTarde,
            Activo, FechaRegistro
        )
        SELECT
            a.IdAlumno, @FechaIter, 1, 1,
            GETDATE(), NULL, NULL, 1, GETDATE()
        FROM genesis.Alumnos a
        WHERE a.Activo = 1
          AND a.IdBusAsignado IS NOT NULL
          AND a.Latitud IS NOT NULL
          AND a.Longitud IS NOT NULL
          AND a.Apellido LIKE N'%Demo'
          AND NOT EXISTS (
              SELECT 1 FROM genesis.AsistenciaAlumno x
              WHERE x.IdAlumno = a.IdAlumno AND x.Fecha = @FechaIter
          );

        SET @DiasOk = @DiasOk + 1;
    END
    SET @FechaIter = DATEADD(DAY, 1, @FechaIter);
END

PRINT 'OK Asistencias demo (15 días hábiles)';

-- -----------------------------------------------------
-- 7) Ubicación reciente de buses (mapa admin)
-- -----------------------------------------------------
IF @IdBus1 IS NOT NULL
    INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, FechaHora, Velocidad, Direccion)
    VALUES (@IdBus1, 14.6150, -90.5200, DATEADD(MINUTE, -1, GETDATE()), 28, 90);

IF @IdBus2 IS NOT NULL
    INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, FechaHora, Velocidad, Direccion)
    VALUES (@IdBus2, 14.6300, -90.5050, DATEADD(MINUTE, -1, GETDATE()), 0, 0);

IF @IdBus3 IS NOT NULL
    INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, FechaHora, Velocidad, Direccion)
    VALUES (@IdBus3, 14.6050, -90.5350, DATEADD(MINUTE, -2, GETDATE()), 35, 180);

PRINT 'OK Ubicaciones recientes';

-- -----------------------------------------------------
-- 8) Resumen
-- -----------------------------------------------------
PRINT '';
PRINT '--- BUSES ---';
SELECT IdBus, Placa, Modelo, Capacidad, Estado FROM genesis.Buses WHERE Placa LIKE N'DEMO-%' ORDER BY Placa;

PRINT '--- ALUMNOS DEMO ---';
SELECT a.IdAlumno, a.Nombre, a.Apellido, a.IdBusAsignado, b.Placa, a.Latitud, a.Longitud
FROM genesis.Alumnos a
LEFT JOIN genesis.Buses b ON b.IdBus = a.IdBusAsignado
WHERE a.Apellido LIKE N'%Demo'
ORDER BY a.IdBusAsignado, a.Nombre;

PRINT '';
PRINT '========================================';
PRINT 'POST-SEED:';
PRINT ' 1) Admin → /Admin/ConfigurarColegio (verificar coords)';
PRINT ' 2) Admin → /Admin/CalcularRutas (Mañana y Tarde)';
PRINT ' 3) Login padre1@gmail.com / Admin123! → Mi Ruta';
PRINT ' 4) Login piloto2@... / Admin123! → Mi Ruta (Bus DEMO-001)';
PRINT '========================================';
GO
