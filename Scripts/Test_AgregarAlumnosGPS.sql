-- =====================================================
-- SCRIPT: Agregar mas alumnos de prueba con GPS
-- Proposito: Crear 10 alumnos adicionales con coordenadas
--            aleatorias para testear rutas con multiples paradas
-- =====================================================

USE TransportesGenesis;
GO

PRINT 'Agregando alumnos de prueba con GPS...';
PRINT '';

-- Variables
DECLARE @FechaProximoLunes DATE;
DECLARE @AlumnosCreados INT = 0;

-- Calcular proximo lunes (omite fin de semana)
SET @FechaProximoLunes = CAST(GETDATE() AS DATE);
WHILE DATEPART(WEEKDAY, @FechaProximoLunes) NOT IN (2) -- 2 = Lunes
BEGIN
    SET @FechaProximoLunes = DATEADD(DAY, 1, @FechaProximoLunes);
END

PRINT CONCAT('Fecha calculada (proximo lunes): ', CONVERT(VARCHAR, @FechaProximoLunes, 103));
PRINT '';

-- =====================================================
-- 1. Agregar coordenadas GPS a alumnos existentes
-- =====================================================
PRINT 'Agregando GPS a alumnos existentes del Bus 4...';

UPDATE TOP (10) genesis.Alumnos 
SET 
    -- Bogota: Centro ~4.6097, -74.0817
    -- Rango: +-0.05 grados (~5km radio)
    Latitud = 4.6097 + (CAST(ABS(CHECKSUM(NEWID())) % 100 AS DECIMAL(10,7)) / 1000) - 0.05,
    Longitud = -74.0817 + (CAST(ABS(CHECKSUM(NEWID())) % 100 AS DECIMAL(10,7)) / 1000) - 0.05,
    Direccion = 'Calle ' + CAST((ABS(CHECKSUM(NEWID())) % 150) + 1 AS VARCHAR) + 
                ' #' + CAST((ABS(CHECKSUM(NEWID())) % 100) + 1 AS VARCHAR) + 
                '-' + CAST((ABS(CHECKSUM(NEWID())) % 99) + 1 AS VARCHAR)
WHERE IdBusAsignado = 4 
  AND (Latitud IS NULL OR Longitud IS NULL);

SET @AlumnosCreados = @@ROWCOUNT;
PRINT CONCAT('Alumnos con GPS agregado: ', @AlumnosCreados);
PRINT '';

-- =====================================================
-- 2. Verificar cuantos alumnos tiene el Bus 4 con GPS
-- =====================================================
DECLARE @TotalConGPS INT;
SELECT @TotalConGPS = COUNT(*)
FROM genesis.Alumnos
WHERE IdBusAsignado = 4
  AND Latitud IS NOT NULL
  AND Longitud IS NOT NULL;

PRINT CONCAT('Total alumnos Bus 4 con GPS: ', @TotalConGPS);
PRINT '';

-- =====================================================
-- 3. Si hay menos de 8, crear alumnos ficticios
-- =====================================================
IF @TotalConGPS < 8
BEGIN
    PRINT 'Creando alumnos ficticios adicionales...';

    DECLARE @AlumnosACrear INT = 8 - @TotalConGPS;
    DECLARE @Contador INT = 1;

    WHILE @Contador <= @AlumnosACrear
    BEGIN
        INSERT INTO genesis.Alumnos (
            Nombre,
            Apellido,
            FechaNacimiento,
            Grado,
            IdBusAsignado,
            Latitud,
            Longitud,
            Direccion,
            Activo,
            FechaRegistro
        )
        VALUES (
            'Alumno' + CAST(@Contador + 100 AS VARCHAR),
            'Prueba' + CAST(@Contador AS VARCHAR),
            DATEADD(YEAR, -8, GETDATE()),
            'Tercero',
            4, -- Bus 4
            4.6097 + (CAST(ABS(CHECKSUM(NEWID())) % 100 AS DECIMAL(10,7)) / 1000) - 0.05,
            -74.0817 + (CAST(ABS(CHECKSUM(NEWID())) % 100 AS DECIMAL(10,7)) / 1000) - 0.05,
            'Calle ' + CAST((ABS(CHECKSUM(NEWID())) % 150) + 1 AS VARCHAR) + 
            ' #' + CAST((ABS(CHECKSUM(NEWID())) % 100) + 1 AS VARCHAR) + 
            '-' + CAST((ABS(CHECKSUM(NEWID())) % 99) + 1 AS VARCHAR),
            1,
            GETDATE()
        );

        SET @Contador = @Contador + 1;
    END

    PRINT CONCAT('Alumnos ficticios creados: ', @AlumnosACrear);
END
ELSE
BEGIN
    PRINT 'Ya hay suficientes alumnos con GPS.';
END
PRINT '';

-- =====================================================
-- 4. Crear asistencias confirmadas para proximo lunes
-- =====================================================
PRINT CONCAT('Creando asistencias confirmadas para ', CONVERT(VARCHAR, @FechaProximoLunes, 103), '...');

-- Eliminar asistencias antiguas del Bus 4 para el lunes
DELETE FROM genesis.AsistenciaAlumno
WHERE Fecha = @FechaProximoLunes
  AND IdAlumno IN (
      SELECT IdAlumno FROM genesis.Alumnos WHERE IdBusAsignado = 4
  );

-- Crear nuevas asistencias confirmadas
INSERT INTO genesis.AsistenciaAlumno (
    IdAlumno,
    Fecha,
    AsisteMañana,
    AsisteTarde,
    FechaConfirmacion,
    Activo,
    FechaRegistro
)
SELECT 
    a.IdAlumno,
    @FechaProximoLunes,
    1, -- AsisteMañana
    1, -- AsisteTarde
    GETDATE(), -- Ya confirmada
    1,
    GETDATE()
FROM genesis.Alumnos a
WHERE a.IdBusAsignado = 4
  AND a.Latitud IS NOT NULL
  AND a.Longitud IS NOT NULL;

DECLARE @AsistenciasCreadas INT = @@ROWCOUNT;
PRINT CONCAT('Asistencias confirmadas creadas: ', @AsistenciasCreadas);
PRINT '';

-- =====================================================
-- 5. Verificar resultados
-- =====================================================
PRINT '================================================================';
PRINT 'RESUMEN DE DATOS DE PRUEBA';
PRINT '================================================================';

SELECT 
    a.IdAlumno,
    a.Nombre + ' ' + a.Apellido AS NombreCompleto,
    a.Grado,
    a.IdBusAsignado AS Bus,
    CAST(a.Latitud AS VARCHAR(20)) AS Latitud,
    CAST(a.Longitud AS VARCHAR(20)) AS Longitud,
    a.Direccion,
    CASE WHEN aa.FechaConfirmacion IS NOT NULL THEN 'Confirmado' ELSE 'Sin confirmar' END AS Estado
FROM genesis.Alumnos a
LEFT JOIN genesis.AsistenciaAlumno aa ON a.IdAlumno = aa.IdAlumno AND aa.Fecha = @FechaProximoLunes
WHERE a.IdBusAsignado = 4
  AND a.Latitud IS NOT NULL
ORDER BY a.Nombre;

PRINT '';
PRINT '================================================================';
PRINT CONCAT('Total alumnos Bus 4 con GPS: ', @TotalConGPS + @AlumnosACrear);
PRINT CONCAT('Asistencias confirmadas: ', @AsistenciasCreadas);
PRINT CONCAT('Fecha de las asistencias: ', CONVERT(VARCHAR, @FechaProximoLunes, 103));
PRINT '';
PRINT 'LISTO PARA TESTEAR:';
PRINT '   1. Ir a: https://localhost:7240/Test';
PRINT '   2. Cambiar fecha a: ' + CONVERT(VARCHAR, @FechaProximoLunes, 23);
PRINT '   3. Calcular ruta para Bus 4, Turno Manana';
PRINT '   4. Deberas ver multiples paradas en el mapa';
PRINT '================================================================';
GO
