-- =====================================================
-- FASE 0.7 — Seed único para demo padre ↔ piloto en vivo
-- Inserta asistencias confirmadas para los próximos 20 días hábiles
-- =====================================================

USE TransportesGenesis;
GO

SET NOCOUNT ON;
PRINT '========================================';
PRINT 'FASE 0.7 — Seed Escenarios Completos';
PRINT '========================================';
PRINT '';

-- -----------------------------------------------------
-- 1. Configuración del colegio (Guatemala — Zona 4)
-- -----------------------------------------------------
IF NOT EXISTS (SELECT 1 FROM genesis.ConfiguracionSistema WHERE Clave = 'Colegio_Latitud')
BEGIN
    INSERT INTO genesis.ConfiguracionSistema (Clave, Valor, Descripcion, Tipo, Categoria, Activo, FechaRegistro)
    VALUES
        ('Colegio_Nombre',    'Colegio Genesis',              'Nombre institución',     'Texto',      'Ubicacion', 1, GETDATE()),
        ('Colegio_Direccion', 'Colegio Yulimay PC, Zona 4',   'Dirección colegio',      'Texto',      'Ubicacion', 1, GETDATE()),
        ('Colegio_Latitud',   '14.6270',                      'Latitud colegio',        'Coordenada', 'Ubicacion', 1, GETDATE()),
        ('Colegio_Longitud',  '-90.5125',                     'Longitud colegio',       'Coordenada', 'Ubicacion', 1, GETDATE());
    PRINT '✅ Configuración del colegio insertada';
END
ELSE
BEGIN
    UPDATE genesis.ConfiguracionSistema SET Valor = '14.6270'  WHERE Clave = 'Colegio_Latitud';
    UPDATE genesis.ConfiguracionSistema SET Valor = '-90.5125' WHERE Clave = 'Colegio_Longitud';
    PRINT '✅ Configuración del colegio actualizada (Guatemala)';
END

-- -----------------------------------------------------
-- 2. Asistencias confirmadas: HOY + próximos 20 días hábiles
--    (permite calcular rutas en Admin para mañana u otras fechas)
-- -----------------------------------------------------
DECLARE @FechaIter DATE = CAST(GETDATE() AS DATE);
DECLARE @DiasInsertados INT = 0;
DECLARE @MaxDias INT = 0;

WHILE @MaxDias < 20
BEGIN
    IF DATEPART(WEEKDAY, @FechaIter) NOT IN (1, 7) -- excluir domingo(1) y sábado(7) en SQL Server default
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
        WHERE a.IdBusAsignado IS NOT NULL
          AND a.Latitud IS NOT NULL
          AND a.Longitud IS NOT NULL
          AND a.Activo = 1
          AND NOT EXISTS (
              SELECT 1 FROM genesis.AsistenciaAlumno aa
              WHERE aa.IdAlumno = a.IdAlumno AND aa.Fecha = @FechaIter
          );

        SET @DiasInsertados = @DiasInsertados + @@ROWCOUNT;
        SET @MaxDias = @MaxDias + 1;
    END
    SET @FechaIter = DATEADD(DAY, 1, @FechaIter);
END

PRINT CONCAT('✅ Registros de asistencia insertados/verificados: ', @DiasInsertados, ' (20 días hábiles)');

-- Confirmar asistencias existentes sin FechaConfirmacion (por si ya había filas)
UPDATE genesis.AsistenciaAlumno
SET FechaConfirmacion = GETDATE()
WHERE FechaConfirmacion IS NULL
  AND Fecha >= CAST(GETDATE() AS DATE)
  AND Fecha <= DATEADD(DAY, 30, CAST(GETDATE() AS DATE));

PRINT '✅ Asistencias pendientes de confirmación actualizadas';

-- -----------------------------------------------------
-- 3. Desactivar rutas antiguas (recalcular desde Admin)
-- -----------------------------------------------------
UPDATE genesis.Rutas SET EsActiva = 0;
PRINT '✅ Rutas anteriores desactivadas — recalcular desde Admin/CalcularRutas';

-- -----------------------------------------------------
-- 5. Resumen por bus
-- -----------------------------------------------------
PRINT '';
PRINT '--- ALUMNOS POR BUS (con GPS) ---';
SELECT
    a.IdBusAsignado AS IdBus,
    COUNT(*) AS CantidadAlumnos
FROM genesis.Alumnos a
WHERE a.IdBusAsignado IS NOT NULL
  AND a.Latitud IS NOT NULL
  AND a.Longitud IS NOT NULL
  AND a.Activo = 1
GROUP BY a.IdBusAsignado
ORDER BY a.IdBusAsignado;

PRINT '';
PRINT '--- VERIFICACIÓN PADRE / PILOTO ---';
SELECT TOP 1
    uPadre.Email       AS EmailPadre,
    a.Nombre + ' ' + a.Apellido AS NombreHijo,
    a.IdBusAsignado    AS IdBus,
    b.Placa,
    uPiloto.Email      AS EmailPiloto
FROM genesis.Padres p
INNER JOIN AspNetUsers uPadre ON p.UsuarioId = uPadre.Id
INNER JOIN genesis.Alumnos a  ON a.IdPadre = p.IdPadre
LEFT  JOIN genesis.Buses b     ON b.IdBus = a.IdBusAsignado
LEFT  JOIN genesis.AsignacionPilotoBus ap ON ap.IdBus = a.IdBusAsignado AND ap.EsActual = 1
LEFT  JOIN AspNetUsers uPiloto ON uPiloto.Id = ap.IdUsuarioPiloto
WHERE uPadre.EmailConfirmed = 1
  AND a.IdBusAsignado IS NOT NULL
ORDER BY p.IdPadre;

PRINT '';
PRINT '========================================';
PRINT 'POST-SEED: Admin → /Admin/CalcularRutas';
PRINT '  Fecha: cualquier día hábil (ej. mañana)';
PRINT '  Turno: Mañana y luego Tarde';
PRINT 'Credenciales: padre1@gmail.com / Admin123!';
PRINT 'Piloto demo Bus 4: monitor1@transportesgenesis.com / Admin123! → /Piloto/MiRuta';
PRINT '  NO usar piloto1 — está en Bus #1; padre1 escucha Bus #4 (BUS-001)';
PRINT '========================================';
GO
