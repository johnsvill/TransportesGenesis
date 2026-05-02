-- =========================================
-- Script: Datos de Prueba - Módulo Geolocalización
-- Propósito: Insertar buses de prueba para validar FASE 2
-- =========================================

USE TransportesGenesis;
GO

PRINT '========================================='
PRINT 'Insertando datos de prueba'
PRINT '========================================='

BEGIN TRANSACTION;

BEGIN TRY
    -- Insertar 3 buses de prueba
    PRINT 'Insertando buses...'

    INSERT INTO genesis.Buses (Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
    VALUES 
        ('P-001GT', 'Mercedes-Benz OF 1721', 45, 1, 1, GETDATE()),
        ('P-002GT', 'Volvo B290R', 50, 1, 1, GETDATE()),
        ('P-003GT', 'Scania K340IB', 40, 0, 1, GETDATE());

    PRINT '✓ 3 buses insertados'
    PRINT ''

    -- Mostrar los buses insertados
    PRINT 'Buses registrados:'
    SELECT 
        IdBus,
        Placa,
        Modelo,
        Capacidad,
        CASE WHEN Estado = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado
    FROM genesis.Buses
    ORDER BY IdBus;

    COMMIT TRANSACTION;
    PRINT ''
    PRINT '========================================='
    PRINT 'Datos insertados exitosamente'
    PRINT '========================================='

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT '✗ ERROR: ' + ERROR_MESSAGE()
END CATCH
GO
