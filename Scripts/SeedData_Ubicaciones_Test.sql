-- =========================================
-- Script: Ubicaciones de Prueba para Buses
-- Propósito: Simular ubicaciones GPS en Ciudad de Guatemala
-- =========================================

USE TransportesGenesis;
GO

PRINT '========================================='
PRINT 'Insertando ubicaciones de buses'
PRINT '========================================='

BEGIN TRANSACTION;

BEGIN TRY
    -- Bus #1 (P-001GT) - Zona 10, Guatemala
    INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, FechaHora, Velocidad, Direccion)
    VALUES 
        (1, 14.593490, -90.513370, DATEADD(MINUTE, -2, GETDATE()), 35.5, 90.0);

    -- Bus #2 (P-002GT) - Zona 1, Centro Histórico
    INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, FechaHora, Velocidad, Direccion)
    VALUES 
        (2, 14.640820, -90.513150, DATEADD(MINUTE, -1, GETDATE()), 0.0, 180.0);

    -- Bus #3 (P-003GT) - Zona 15, Vista Hermosa
    INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, FechaHora, Velocidad, Direccion)
    VALUES 
        (3, 14.608920, -90.488420, DATEADD(MINUTE, -5, GETDATE()), 15.2, 270.0);

    PRINT '✓ 3 ubicaciones de buses insertadas'
    PRINT ''

    -- Mostrar las ubicaciones
    PRINT 'Ubicaciones registradas:'
    SELECT 
        u.IdUbicacion,
        b.Placa,
        u.Latitud,
        u.Longitud,
        u.FechaHora,
        u.Velocidad,
        DATEDIFF(MINUTE, u.FechaHora, GETDATE()) AS MinutosAtras
    FROM genesis.UbicacionBusEnTiempoReal u
    INNER JOIN genesis.Buses b ON u.IdBus = b.IdBus
    ORDER BY u.FechaHora DESC;

    COMMIT TRANSACTION;
    PRINT ''
    PRINT '========================================='
    PRINT 'Ubicaciones insertadas exitosamente'
    PRINT '========================================='

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '✗ ERROR: ' + ERROR_MESSAGE()
END CATCH
GO
