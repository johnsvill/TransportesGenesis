-- =====================================================
-- Script: Setup Buses de Prueba
-- Descripción: Inserta buses básicos para testing si no existen
-- Fecha: 2026-04-26
-- =====================================================

USE TransportesGenesis;
GO

-- Verificar si ya existen buses
IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE IdBus = 1)
BEGIN
    PRINT 'Insertando buses de prueba...';

    SET IDENTITY_INSERT genesis.Buses ON;

    INSERT INTO genesis.Buses (IdBus, Placa, Modelo, Capacidad, Activo, FechaRegistro, RegistradoPor)
    VALUES 
        (1, 'ABC-123', 'Mercedes Sprinter 2020', 20, 1, GETDATE(), 'admin'),
        (2, 'DEF-456', 'Mercedes Sprinter 2021', 20, 1, GETDATE(), 'admin'),
        (3, 'GHI-789', 'Mercedes Sprinter 2021', 20, 1, GETDATE(), 'admin'),
        (4, 'JKL-012', 'Mercedes Sprinter 2022', 20, 1, GETDATE(), 'admin');

    SET IDENTITY_INSERT genesis.Buses OFF;

    PRINT '✓ 4 buses insertados exitosamente';
END
ELSE
BEGIN
    PRINT 'Los buses ya existen, no se requiere inserción';
END
GO

-- Verificar buses existentes
SELECT 
    IdBus,
    Placa,
    Modelo,
    Capacidad,
    Activo,
    FechaRegistro
FROM genesis.Buses
ORDER BY IdBus;
GO
