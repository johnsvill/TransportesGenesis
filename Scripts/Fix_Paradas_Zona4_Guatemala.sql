-- =====================================================
-- Script: Corregir Paradas y Alumnos a Zona 4, Guatemala
-- Propósito: Reemplazar coordenadas fuera de Guatemala
--            con puntos válidos dentro de Zona 4,
--            Ciudad de Guatemala.
-- Bounds válidos Guatemala: lat 13.7-17.9 / lon -92.3 a -88.2
-- Fecha: 2026
-- =====================================================

USE TransportesGenesis;
GO

PRINT '========================================';
PRINT 'Corrigiendo paradas fuera de Guatemala...';
PRINT '========================================';

-- Ver cuántas paradas tienen coordenadas inválidas
SELECT COUNT(*) AS ParadasFueraDeGuatemala
FROM genesis.Paradas
WHERE Latitud < 13.7 OR Latitud > 17.9
   OR Longitud < -92.3 OR Longitud > -88.2;

-- Actualizar paradas del Bus 1 / Ruta Mañana a Zona 4
-- (6 paradas de referencia en Zona 4)
UPDATE genesis.Paradas
SET
	Latitud   = CASE Orden
					WHEN 1 THEN 14.6380
					WHEN 2 THEN 14.6358
					WHEN 3 THEN 14.6336
					WHEN 4 THEN 14.6314
					WHEN 5 THEN 14.6292
					ELSE        14.6270   -- Colegio (última parada)
				END,
	Longitud  = CASE Orden
					WHEN 1 THEN -90.5240
					WHEN 2 THEN -90.5215
					WHEN 3 THEN -90.5190
					WHEN 4 THEN -90.5165
					WHEN 5 THEN -90.5145
					ELSE        -90.5125  -- Colegio (última parada)
				END,
	Direccion = CASE Orden
					WHEN 1 THEN '7ma Av. y 2da Calle, Zona 4, Guatemala'
					WHEN 2 THEN '5ta Av. y 4ta Calle, Zona 4, Guatemala'
					WHEN 3 THEN 'Terminal Central, 3ra Av., Zona 4, Guatemala'
					WHEN 4 THEN 'Mercado El Guarda, Zona 4, Guatemala'
					WHEN 5 THEN 'Av. Bolívar y 8va Calle, Zona 4, Guatemala'
					ELSE        'Colegio Yulimay PC, Zona 4, Guatemala'
				END
WHERE (Latitud < 13.7 OR Latitud > 17.9
	OR Longitud < -92.3 OR Longitud > -88.2);

PRINT CONCAT('✔ ', @@ROWCOUNT, ' paradas corregidas a Zona 4, Guatemala');

GO

-- -------------------------------------------------------
-- Corregir alumnos que también tengan coordenadas inválidas
-- -------------------------------------------------------
PRINT '';
PRINT '========================================';
PRINT 'Corrigiendo alumnos fuera de Guatemala...';
PRINT '========================================';

-- Puntos distribuidos en Zona 4 para los alumnos
UPDATE genesis.Alumnos
SET
	Latitud   = 14.6380 - (CAST(ABS(CHECKSUM(NEWID())) % 12 AS DECIMAL(10,6)) / 1000),
	Longitud  = -90.5240 + (CAST(ABS(CHECKSUM(NEWID())) % 12 AS DECIMAL(10,6)) / 1000),
	Direccion = 'Zona 4, Ciudad de Guatemala'
WHERE Latitud IS NOT NULL AND Longitud IS NOT NULL
  AND (Latitud < 13.7 OR Latitud > 17.9
	OR Longitud < -92.3 OR Longitud > -88.2);

PRINT CONCAT('✔ ', @@ROWCOUNT, ' alumnos corregidos a Zona 4, Guatemala');

GO

-- -------------------------------------------------------
-- Verificar resultado
-- -------------------------------------------------------
PRINT '';
PRINT '========================================';
PRINT 'Verificación final de paradas:';
PRINT '========================================';

SELECT
	p.IdParada,
	r.Nombre    AS Ruta,
	p.Orden,
	CAST(p.Latitud  AS DECIMAL(8,5)) AS Latitud,
	CAST(p.Longitud AS DECIMAL(8,5)) AS Longitud,
	p.Direccion
FROM genesis.Paradas p
LEFT JOIN genesis.Rutas r ON p.IdRuta = r.IdRuta
ORDER BY p.IdRuta, p.Orden;

PRINT '';
PRINT 'Paradas aún fuera de Guatemala (debe ser 0):';
SELECT COUNT(*) AS RestanteFuera
FROM genesis.Paradas
WHERE Latitud < 13.7 OR Latitud > 17.9
   OR Longitud < -92.3 OR Longitud > -88.2;
GO
Scripts/Fix_Paradas_Zona4_Guatemala.sql