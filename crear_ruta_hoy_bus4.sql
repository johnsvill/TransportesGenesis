-- Script para crear una ruta de HOY para el Bus #4 (para pruebas de fin de semana)
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

DECLARE @FechaHoy DATE = CAST(GETDATE() AS DATE);
DECLARE @IdBus INT = 4;
DECLARE @TipoRuta NVARCHAR(10) = 'Mañana';
DECLARE @NuevaRutaId INT;

-- Crear nueva ruta para HOY
INSERT INTO genesis.Rutas (IdBus, Nombre, Descripcion, TipoRuta, HoraInicio, EsActiva, Activo, FechaRegistro)
VALUES (
    @IdBus,
    'Ruta ' + @TipoRuta + ' - ' + FORMAT(@FechaHoy, 'dd/MM/yyyy') + ' (PRUEBA)',
    'Ruta de prueba para testing de fin de semana',
    @TipoRuta,
    '06:00:00',
    1,
    1,
    GETDATE()
);

SET @NuevaRutaId = SCOPE_IDENTITY();

-- Asignar las paradas de la ruta más reciente a esta nueva ruta
INSERT INTO genesis.Paradas (IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
SELECT 
    @NuevaRutaId,
    IdAlumno,
    Latitud,
    Longitud,
    Direccion,
    Orden,
    HoraEstimada,
    0, -- No completada
    1, -- Activo
    GETDATE()
FROM genesis.Paradas
WHERE IdRuta = (SELECT TOP 1 IdRuta FROM genesis.Rutas WHERE IdBus = @IdBus AND EsActiva = 1 AND IdRuta != @NuevaRutaId ORDER BY IdRuta DESC);

-- Verificar resultado
SELECT 
    r.IdRuta,
    r.Nombre,
    r.TipoRuta,
    r.HoraInicio,
    r.EsActiva,
    r.FechaRegistro,
    (SELECT COUNT(*) FROM genesis.Paradas WHERE IdRuta = r.IdRuta) AS TotalParadas
FROM genesis.Rutas r
WHERE r.IdRuta = @NuevaRutaId;

PRINT '';
PRINT '✅ Ruta creada para HOY: ' + FORMAT(@FechaHoy, 'dd/MM/yyyy');
PRINT '📍 Paradas copiadas de la ruta anterior';
PRINT '';
PRINT '🔄 Por favor REINICIA la aplicación para que cargue la nueva ruta';
