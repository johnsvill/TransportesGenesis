-- Script para actualizar ubicaciones con fechas ACTUALES
-- Simula buses en tiempo real

USE TransportesGenesis;
GO

PRINT 'Actualizando ubicaciones de buses con fechas actuales...';

-- Actualizar todas las ubicaciones para que sean recientes
UPDATE genesis.UbicacionBusEnTiempoReal
SET 
    FechaHora = CASE 
        -- Algunos buses con ubicación actual (menos de 1 minuto)
        WHEN IdBus IN (SELECT TOP 2 IdBus FROM genesis.Buses WHERE Estado = 1 ORDER BY IdBus) 
            THEN DATEADD(SECOND, -30, GETDATE())
        -- Algunos detenidos (2-3 minutos)
        WHEN IdBus IN (SELECT TOP 2 IdBus FROM genesis.Buses WHERE Estado = 1 ORDER BY IdBus DESC) 
            THEN DATEADD(MINUTE, -2, GETDATE())
        -- Uno sin señal (más de 10 minutos)
        ELSE DATEADD(MINUTE, -15, GETDATE())
    END,
    Velocidad = CASE 
        -- Variar velocidades para simular movimiento/detenido
        WHEN IdBus % 3 = 0 THEN 45.0  -- En movimiento rápido
        WHEN IdBus % 2 = 0 THEN 0.0   -- Detenido
        ELSE 35.5                      -- En movimiento normal
    END
WHERE IdBus IN (SELECT IdBus FROM genesis.Buses WHERE Estado = 1);

PRINT 'Ubicaciones actualizadas correctamente';
GO

-- Verificar resultados
SELECT 
    b.IdBus,
    b.Placa,
    u.Latitud,
    u.Longitud,
    u.FechaHora,
    u.Velocidad,
    DATEDIFF(MINUTE, u.FechaHora, GETDATE()) AS MinutosTranscurridos,
    CASE 
        WHEN DATEDIFF(MINUTE, u.FechaHora, GETDATE()) > 10 THEN 'SinSeñal'
        WHEN u.Velocidad > 5 THEN 'Movimiento'
        ELSE 'Detenido'
    END AS EstadoCalculado
FROM genesis.Buses b
INNER JOIN genesis.UbicacionBusEnTiempoReal u ON b.IdBus = u.IdBus
WHERE b.Estado = 1
ORDER BY u.FechaHora DESC;
GO
