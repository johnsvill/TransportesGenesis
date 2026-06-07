-- ========================================
-- Verificar y Corregir Solicitudes de Traslado
-- ========================================

USE TransportesGenesis;
GO

PRINT '=== VERIFICANDO SOLICITUDES ACTUALES ===';
PRINT '';

-- Ver todas las solicitudes con TODOS los campos
SELECT 
    IdSolicitud,
    IdAlumno,
    IdBusOrigen,
    IdBusDestino,
    FORMAT(FechaTraslado, 'dd/MM/yyyy') AS FechaTraslado,
    Turno,
    Estado,
    Motivo,
    FORMAT(FechaRegistro, 'dd/MM/yyyy HH:mm:ss') AS FechaRegistro,
    Activo
FROM genesis.SolicitudTraslado
ORDER BY IdSolicitud;

PRINT '';
PRINT '=== CORRIGIENDO FECHA DE REGISTRO ===';

-- Actualizar FechaRegistro para solicitudes que tienen 1/1/1 o NULL
UPDATE genesis.SolicitudTraslado
SET FechaRegistro = GETDATE()
WHERE FechaRegistro IS NULL 
   OR FechaRegistro < '2020-01-01' 
   OR FechaRegistro = '0001-01-01';

DECLARE @FilasActualizadas INT = @@ROWCOUNT;
PRINT 'Solicitudes actualizadas: ' + CAST(@FilasActualizadas AS VARCHAR);

PRINT '';
PRINT '=== VERIFICANDO RELACIONES ===';

-- Ver solicitudes con información completa (JOIN con Alumnos y Buses)
SELECT 
    s.IdSolicitud,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    FORMAT(s.FechaTraslado, 'dd/MM/yyyy') AS FechaTraslado,
    s.Turno,
    bOrigen.Placa AS BusOrigen,
    bDestino.Placa AS BusDestino,
    ISNULL(s.Motivo, '[Sin motivo]') AS Motivo,
    s.Estado,
    FORMAT(s.FechaRegistro, 'dd/MM/yyyy HH:mm:ss') AS FechaRegistro
FROM genesis.SolicitudTraslado s
LEFT JOIN genesis.Alumnos a ON s.IdAlumno = a.IdAlumno
LEFT JOIN genesis.Buses bOrigen ON s.IdBusOrigen = bOrigen.IdBus
LEFT JOIN genesis.Buses bDestino ON s.IdBusDestino = bDestino.IdBus
ORDER BY s.IdSolicitud;

PRINT '';
PRINT '=== DETECTANDO PROBLEMAS ===';

-- Verificar si hay solicitudes sin alumno válido
IF EXISTS (SELECT 1 FROM genesis.SolicitudTraslado WHERE IdAlumno NOT IN (SELECT IdAlumno FROM genesis.Alumnos))
BEGIN
    PRINT '⚠️  Hay solicitudes con IdAlumno inválido:';
    SELECT IdSolicitud, IdAlumno, 'Alumno no existe' AS Problema
    FROM genesis.SolicitudTraslado
    WHERE IdAlumno NOT IN (SELECT IdAlumno FROM genesis.Alumnos);
END
ELSE
BEGIN
    PRINT '✅ Todas las solicitudes tienen IdAlumno válido';
END

-- Verificar si hay solicitudes sin bus origen válido
IF EXISTS (SELECT 1 FROM genesis.SolicitudTraslado WHERE IdBusOrigen NOT IN (SELECT IdBus FROM genesis.Buses))
BEGIN
    PRINT '⚠️  Hay solicitudes con IdBusOrigen inválido:';
    SELECT IdSolicitud, IdBusOrigen, 'Bus origen no existe' AS Problema
    FROM genesis.SolicitudTraslado
    WHERE IdBusOrigen NOT IN (SELECT IdBus FROM genesis.Buses);

    PRINT '';
    PRINT 'Corrigiendo IdBusOrigen inválidos...';
    UPDATE genesis.SolicitudTraslado
    SET IdBusOrigen = 4 -- ID del BUS-001
    WHERE IdBusOrigen NOT IN (SELECT IdBus FROM genesis.Buses);

    PRINT 'Solicitudes corregidas: ' + CAST(@@ROWCOUNT AS VARCHAR);
END
ELSE
BEGIN
    PRINT '✅ Todas las solicitudes tienen IdBusOrigen válido';
END

-- Verificar si hay solicitudes sin bus destino válido (solo si tiene IdBusDestino)
IF EXISTS (SELECT 1 FROM genesis.SolicitudTraslado 
           WHERE IdBusDestino IS NOT NULL 
           AND IdBusDestino NOT IN (SELECT IdBus FROM genesis.Buses))
BEGIN
    PRINT '⚠️  Hay solicitudes con IdBusDestino inválido:';
    SELECT IdSolicitud, IdBusDestino, 'Bus destino no existe' AS Problema
    FROM genesis.SolicitudTraslado
    WHERE IdBusDestino IS NOT NULL 
    AND IdBusDestino NOT IN (SELECT IdBus FROM genesis.Buses);

    PRINT '';
    PRINT 'Corrigiendo IdBusDestino inválidos...';
    UPDATE genesis.SolicitudTraslado
    SET IdBusDestino = 5 -- ID del BUS-002
    WHERE IdBusDestino IS NOT NULL 
    AND IdBusDestino NOT IN (SELECT IdBus FROM genesis.Buses);

    PRINT 'Solicitudes corregidas: ' + CAST(@@ROWCOUNT AS VARCHAR);
END
ELSE
BEGIN
    PRINT '✅ Todas las solicitudes tienen IdBusDestino válido (o NULL)';
END

PRINT '';
PRINT '=== RESULTADO FINAL ===';
PRINT '';

SELECT 
    s.IdSolicitud,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    FORMAT(s.FechaTraslado, 'dddd dd ''de'' MMMM ''de'' yyyy', 'es-ES') AS FechaTraslado,
    s.Turno,
    bOrigen.Placa AS BusOrigen,
    ISNULL(bDestino.Placa, '[Sin asignar]') AS BusDestino,
    ISNULL(s.Motivo, '[Sin motivo]') AS Motivo,
    s.Estado,
    FORMAT(s.FechaRegistro, 'dd/MM/yyyy HH:mm:ss') AS FechaRegistro
FROM genesis.SolicitudTraslado s
INNER JOIN genesis.Alumnos a ON s.IdAlumno = a.IdAlumno
LEFT JOIN genesis.Buses bOrigen ON s.IdBusOrigen = bOrigen.IdBus
LEFT JOIN genesis.Buses bDestino ON s.IdBusDestino = bDestino.IdBus
ORDER BY s.FechaRegistro DESC;

PRINT '';
PRINT '✅ Verificación y corrección completada';
GO
