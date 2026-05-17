-- Script para verificar las paradas en la base de datos
-- Ejecuta este script en SQL Server Management Studio o Azure Data Studio

USE TransportesGenesis;
GO

-- Verificar cuántas paradas hay
SELECT 
    COUNT(*) as TotalParadas
FROM genesis.Paradas;
GO

-- Ver todas las paradas con sus coordenadas
SELECT 
    IdParada,
    IdRuta,
    Latitud,
    Longitud,
    Direccion,
    Orden,
    Activo,
    FechaRegistro
FROM genesis.Paradas
ORDER BY Orden;
GO

-- Verificar tipos de datos de las columnas
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    NUMERIC_PRECISION,
    NUMERIC_SCALE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'genesis' 
  AND TABLE_NAME = 'Paradas'
  AND COLUMN_NAME IN ('Latitud', 'Longitud');
GO

-- Ver paradas con sus rutas
SELECT 
    p.IdParada,
    p.Latitud,
    p.Longitud,
    p.Direccion,
    p.Orden,
    r.Nombre as NombreRuta,
    p.Activo
FROM genesis.Paradas p
LEFT JOIN genesis.Rutas r ON p.IdRuta = r.IdRuta
ORDER BY p.Orden;
GO
