-- Script para insertar datos de prueba del módulo de Geolocalización
-- Ejecutar después de aplicar las migraciones

USE TransportesGenesis;
GO

-- Insertar buses de prueba
IF NOT EXISTS (SELECT 1 FROM genesis.Buses WHERE Placa = 'BUS-001')
BEGIN
    INSERT INTO genesis.Buses (Placa, Modelo, Capacidad, Estado, Activo, FechaRegistro)
    VALUES 
        ('BUS-001', 'Mercedes-Benz Sprinter', 25, 1, 1, GETDATE()),
        ('BUS-002', 'Volkswagen Crafter', 30, 1, 1, GETDATE()),
        ('BUS-003', 'Ford Transit', 20, 1, 1, GETDATE());

    PRINT 'Buses insertados correctamente';
END
ELSE
BEGIN
    PRINT 'Los buses ya existen en la base de datos';
END
GO

-- Insertar ubicaciones recientes (últimos 5 minutos) para que aparezcan en el mapa
DECLARE @IdBus1 INT, @IdBus2 INT, @IdBus3 INT;

SELECT @IdBus1 = IdBus FROM genesis.Buses WHERE Placa = 'BUS-001';
SELECT @IdBus2 = IdBus FROM genesis.Buses WHERE Placa = 'BUS-002';
SELECT @IdBus3 = IdBus FROM genesis.Buses WHERE Placa = 'BUS-003';

-- Ubicaciones en diferentes zonas de Ciudad de Guatemala
-- BUS-001: En movimiento por Zona 10
INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, FechaHora, Velocidad, Direccion)
VALUES 
    (@IdBus1, 14.5920, -90.5200, DATEADD(MINUTE, -2, GETDATE()), 35.5, 180);

-- BUS-002: Detenido en Zona 9
INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, FechaHora, Velocidad, Direccion)
VALUES 
    (@IdBus2, 14.5995, -90.5134, DATEADD(MINUTE, -1, GETDATE()), 0.0, 90);

-- BUS-003: En movimiento por Carretera a El Salvador
INSERT INTO genesis.UbicacionBusEnTiempoReal (IdBus, Latitud, Longitud, FechaHora, Velocidad, Direccion)
VALUES 
    (@IdBus3, 14.6100, -90.4900, DATEADD(MINUTE, -3, GETDATE()), 45.0, 270);

PRINT 'Ubicaciones de prueba insertadas correctamente';
GO

-- Verificar los datos insertados
SELECT 
    b.Placa,
    b.Modelo,
    u.Latitud,
    u.Longitud,
    u.FechaHora,
    u.Velocidad,
    DATEDIFF(MINUTE, u.FechaHora, GETDATE()) AS MinutosDesdeActualizacion
FROM genesis.Buses b
LEFT JOIN genesis.UbicacionBusEnTiempoReal u ON b.IdBus = u.IdBus
WHERE b.Activo = 1
ORDER BY u.FechaHora DESC;
GO