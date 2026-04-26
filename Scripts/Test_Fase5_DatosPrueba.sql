-- =====================================================
-- SCRIPT: Datos de Prueba para FASE 5 - Rutas Dinamicas
-- Proposito: Crear asistencias confirmadas con alumnos
--            que tienen coordenadas GPS para testear el algoritmo
-- =====================================================

USE TransportesGenesis;
GO

PRINT 'Iniciando insercion de datos de prueba para FASE 5...';
PRINT '';

-- =====================================================
-- 1. Verificar que existan alumnos con GPS
-- =====================================================
PRINT 'Verificando alumnos con coordenadas GPS...';

SELECT 
    IdAlumno,
    Nombre,
    IdBusAsignado,
    Latitud,
    Longitud,
    Direccion
FROM genesis.Alumnos
WHERE Latitud IS NOT NULL 
  AND Longitud IS NOT NULL 
  AND IdBusAsignado IS NOT NULL
ORDER BY IdBusAsignado, Nombre;

DECLARE @TotalAlumnosGPS INT = (
    SELECT COUNT(*) 
    FROM genesis.Alumnos 
    WHERE Latitud IS NOT NULL 
      AND Longitud IS NOT NULL 
      AND IdBusAsignado IS NOT NULL
);

PRINT CONCAT('Total alumnos con GPS: ', @TotalAlumnosGPS);
PRINT '';

-- =====================================================
-- 2. Crear asistencias confirmadas para HOY
-- =====================================================
PRINT 'Creando asistencias confirmadas para hoy...';

DECLARE @FechaHoy DATE = CAST(GETDATE() AS DATE);
DECLARE @AlumnosInsertados INT = 0;

-- Insertar asistencias para todos los alumnos con GPS del BUS 4
INSERT INTO genesis.AsistenciaAlumno (
    IdAlumno,
    Fecha,
    AsisteMañana,
    AsisteTarde,
    FechaConfirmacion,
    IdBusTemporalMañana,
    IdBusTemporalTarde,
    Activo,
    FechaRegistro
)
SELECT 
    a.IdAlumno,
    @FechaHoy,
    1, -- AsisteMañana = true
    1, -- AsisteTarde = true
    GETDATE(), -- FechaConfirmacion = ahora (significa que ya está confirmada)
    NULL, -- Sin traslado temporal mañana
    NULL, -- Sin traslado temporal tarde
    1,
    GETDATE()
FROM genesis.Alumnos a
WHERE a.Latitud IS NOT NULL 
  AND a.Longitud IS NOT NULL 
  AND a.IdBusAsignado = 4 -- BUS 4 (el que usamos en las pruebas)
  AND NOT EXISTS (
      SELECT 1 
      FROM genesis.AsistenciaAlumno aa 
      WHERE aa.IdAlumno = a.IdAlumno 
        AND aa.Fecha = @FechaHoy
  );

SET @AlumnosInsertados = @@ROWCOUNT;
PRINT CONCAT('Asistencias creadas: ', @AlumnosInsertados);
PRINT '';

-- =====================================================
-- 3. Verificar las asistencias creadas
-- =====================================================
PRINT 'Verificando asistencias confirmadas del dia...';

SELECT 
    aa.IdAsistencia,
    a.IdAlumno,
    a.Nombre AS NombreAlumno,
    a.IdBusAsignado AS BusAsignado,
    aa.Fecha,
    aa.AsisteMañana,
    aa.AsisteTarde,
    aa.FechaConfirmacion,
    a.Latitud,
    a.Longitud,
    a.Direccion
FROM genesis.AsistenciaAlumno aa
INNER JOIN genesis.Alumnos a ON aa.IdAlumno = a.IdAlumno
WHERE aa.Fecha = @FechaHoy
  AND aa.FechaConfirmacion IS NOT NULL
  AND a.IdBusAsignado = 4
ORDER BY a.Nombre;

DECLARE @TotalConfirmados INT = (
    SELECT COUNT(*) 
    FROM genesis.AsistenciaAlumno aa
    INNER JOIN genesis.Alumnos a ON aa.IdAlumno = a.IdAlumno
    WHERE aa.Fecha = @FechaHoy
      AND aa.FechaConfirmacion IS NOT NULL
      AND a.IdBusAsignado = 4
);

PRINT '';
PRINT CONCAT('Total confirmados para BUS 4 hoy: ', @TotalConfirmados);
PRINT '';

-- =====================================================
-- 4. Verificar buses disponibles
-- =====================================================
PRINT 'Verificando buses activos...';

SELECT 
    IdBus,
    Placa,
    Capacidad,
    EstaActivo,
    ModeloBus,
    PlacaVehiculo
FROM genesis.Buses
WHERE EstaActivo = 1
ORDER BY IdBus;

PRINT '';

-- =====================================================
-- 5. Resumen de estado
-- =====================================================
PRINT '================================================================';
PRINT 'RESUMEN DE DATOS DE PRUEBA';
PRINT '================================================================';
PRINT CONCAT('Fecha de prueba: ', CONVERT(VARCHAR, @FechaHoy, 103));
PRINT CONCAT('Alumnos con GPS: ', @TotalAlumnosGPS);
PRINT CONCAT('Asistencias creadas: ', @AlumnosInsertados);
PRINT CONCAT('Confirmados para BUS 4: ', @TotalConfirmados);
PRINT '';
PRINT 'LISTO PARA TESTEAR:';
PRINT '   1. Abrir: https://localhost:XXXX/Test/TestRutasAPI';
PRINT '   2. TEST 1: Calcular ruta para Bus 4, Turno Manana';
PRINT '   3. TEST 2: Obtener ruta activa del Bus 4';
PRINT '   4. TEST 3: Marcar primera parada como completada';
PRINT '================================================================';
GO
