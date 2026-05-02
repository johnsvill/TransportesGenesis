-- =====================================================
-- Script: Actualizar Coordenadas Alumnos a Guatemala
-- Propósito: Cambiar GPS de alumnos de Bogotá a Guatemala
-- Fecha: 26/04/2026
-- =====================================================

USE TransportesGenesis;
GO

PRINT '========================================';
PRINT 'Actualizando coordenadas de alumnos...';
PRINT '========================================';
PRINT '';

-- Verificar cuántos alumnos tienen GPS
DECLARE @TotalConGPS INT;
SELECT @TotalConGPS = COUNT(*) 
FROM genesis.Alumnos 
WHERE Latitud IS NOT NULL AND Longitud IS NOT NULL;

PRINT CONCAT('Alumnos con GPS antes: ', @TotalConGPS);
PRINT '';

-- Actualizar coordenadas a Guatemala
-- Ciudad de Guatemala centro: 14.6349, -90.5069
-- Rango: ±0.03 grados (~3km radio)

UPDATE genesis.Alumnos
SET 
    Latitud = 14.6349 + (CAST(ABS(CHECKSUM(NEWID())) % 60 AS DECIMAL(10,7)) / 1000) - 0.03,
    Longitud = -90.5069 + (CAST(ABS(CHECKSUM(NEWID())) % 60 AS DECIMAL(10,7)) / 1000) - 0.03,
    Direccion = 'Zona ' + CAST((ABS(CHECKSUM(NEWID())) % 15) + 1 AS VARCHAR) + 
                ', ' + CAST((ABS(CHECKSUM(NEWID())) % 50) + 1 AS VARCHAR) + 
                ' Calle ' + CAST((ABS(CHECKSUM(NEWID())) % 30) + 1 AS VARCHAR) + 
                '-' + CAST((ABS(CHECKSUM(NEWID())) % 99) + 1 AS VARCHAR) +
                ', Ciudad de Guatemala'
WHERE Latitud IS NOT NULL AND Longitud IS NOT NULL;

PRINT CONCAT('✓ ', @@ROWCOUNT, ' alumnos actualizados a coordenadas de Guatemala');
PRINT '';

-- Mostrar algunos ejemplos
PRINT 'Ejemplos de alumnos actualizados:';
PRINT '';
SELECT TOP 5
    IdAlumno,
    Nombre,
    CAST(Latitud AS DECIMAL(10,6)) as Latitud,
    CAST(Longitud AS DECIMAL(10,6)) as Longitud,
    Direccion,
    IdBusAsignado
FROM genesis.Alumnos
WHERE Latitud IS NOT NULL 
  AND Longitud IS NOT NULL
ORDER BY IdAlumno;

PRINT '';
PRINT '========================================';
PRINT 'Actualización completada';
PRINT '========================================';
PRINT '';
PRINT 'Coordenadas ahora en rango:';
PRINT 'Latitud: 14.6049 a 14.6649';
PRINT 'Longitud: -90.5369 a -90.4769';
PRINT '(~3km radio desde centro de Guatemala)';
GO
