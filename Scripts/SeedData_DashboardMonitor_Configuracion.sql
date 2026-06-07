-- =========================================
-- Script: Configuración Dashboard Monitor - PRESENTACIÓN
-- Propósito: Script auxiliar para ajustar datos durante la presentación
-- =========================================

USE TransportesGenesis;
GO

-- ============================================
-- OPCIÓN 1: ACTIVAR/DESACTIVAR RUTAS
-- ============================================
-- Usar estos comandos para simular diferentes escenarios

-- Activar ruta de Mañana
-- UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdRuta = 10;

-- Desactivar ruta de Mañana (para simular "no hay ruta")
-- UPDATE genesis.Rutas SET EsActiva = 0 WHERE IdRuta = 10;

-- Activar ruta de Tarde
-- UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdRuta = 11;

-- Desactivar ruta de Tarde
-- UPDATE genesis.Rutas SET EsActiva = 0 WHERE IdRuta = 11;


-- ============================================
-- OPCIÓN 2: MARCAR PARADAS COMO COMPLETADAS
-- ============================================
-- Simular progreso de la ruta durante la presentación

-- Marcar primera parada como completada (Ruta Mañana)
-- UPDATE genesis.Paradas SET Completada = 1 WHERE IdParada = 100;

-- Marcar primeras 3 paradas como completadas
-- UPDATE genesis.Paradas SET Completada = 1 WHERE IdParada IN (100, 101, 102) AND IdRuta = 10;

-- Marcar primeras 5 paradas como completadas
-- UPDATE genesis.Paradas SET Completada = 1 WHERE IdParada IN (100, 101, 102, 103, 104) AND IdRuta = 10;

-- Resetear todas las paradas (no completadas)
-- UPDATE genesis.Paradas SET Completada = 0;


-- ============================================
-- OPCIÓN 3: AGREGAR MÁS ALUMNOS (Si necesitas más datos)
-- ============================================
/*
-- Agregar 2 alumnos adicionales
SET IDENTITY_INSERT genesis.Alumnos ON;

INSERT INTO genesis.Alumnos (IdAlumno, IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
VALUES 
    (210, 105, 'Emilia', 'Gómez', 4, 14.6180, -90.5290, '8a Avenida 20-15, Zona 9, Guatemala', 1, GETDATE()),
    (211, 106, 'Benjamín', 'Díaz', 4, 14.6025, -90.5175, 'Avenida La Reforma 18-50, Zona 10, Guatemala', 1, GETDATE());

SET IDENTITY_INSERT genesis.Alumnos OFF;

-- Agregar paradas para los nuevos alumnos (Ruta Mañana)
SET IDENTITY_INSERT genesis.Paradas ON;

INSERT INTO genesis.Paradas (IdParada, IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
VALUES 
    (111, 10, 210, 14.6180, -90.5290, '8a Avenida 20-15, Zona 9', 12, '07:05:00', 0, 1, GETDATE()),
    (112, 10, 211, 14.6025, -90.5175, 'Avenida La Reforma 18-50, Zona 10', 13, '07:10:00', 0, 1, GETDATE());

SET IDENTITY_INSERT genesis.Paradas OFF;

-- Agregar paradas para los nuevos alumnos (Ruta Tarde)
INSERT INTO genesis.Paradas (IdParada, IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, HoraEstimada, Completada, Activo, FechaRegistro)
VALUES 
    (131, 11, 210, 14.6180, -90.5290, '8a Avenida 20-15, Zona 9', 12, '15:00:00', 0, 1, GETDATE()),
    (132, 11, 211, 14.6025, -90.5175, 'Avenida La Reforma 18-50, Zona 10', 13, '15:05:00', 0, 1, GETDATE());
*/


-- ============================================
-- OPCIÓN 4: CAMBIAR TIPO DE RUTA ACTIVA SEGÚN HORA
-- ============================================
-- Usar esto para cambiar entre Mañana y Tarde durante la presentación

-- Configurar para TURNO MAÑANA (antes del mediodía)
/*
UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdRuta = 10 AND TipoRuta = 'Mañana';
UPDATE genesis.Rutas SET EsActiva = 0 WHERE IdRuta = 11 AND TipoRuta = 'Tarde';
UPDATE genesis.Paradas SET Completada = 0 WHERE IdRuta = 10;
*/

-- Configurar para TURNO TARDE (después del mediodía)
/*
UPDATE genesis.Rutas SET EsActiva = 0 WHERE IdRuta = 10 AND TipoRuta = 'Mañana';
UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdRuta = 11 AND TipoRuta = 'Tarde';
UPDATE genesis.Paradas SET Completada = 0 WHERE IdRuta = 11;
*/


-- ============================================
-- OPCIÓN 5: CONSULTAS ÚTILES PARA VERIFICAR DATOS
-- ============================================

-- Ver todos los alumnos del Bus BUS-001
/*
SELECT 
    a.IdAlumno,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    p.Nombre + ' ' + p.Apellido AS Padre,
    a.Direccion,
    b.Placa AS Bus
FROM genesis.Alumnos a
INNER JOIN genesis.Padres p ON a.IdPadre = p.IdPadre
INNER JOIN genesis.Buses b ON a.IdBusAsignado = b.IdBus
WHERE b.IdBus = 4
ORDER BY a.IdAlumno;
*/

-- Ver rutas y cantidad de paradas
/*
SELECT 
    r.IdRuta,
    r.Nombre,
    r.TipoRuta,
    CONVERT(VARCHAR(5), r.HoraInicio, 108) AS Hora,
    r.EsActiva,
    COUNT(p.IdParada) AS TotalParadas,
    SUM(CASE WHEN p.Completada = 1 THEN 1 ELSE 0 END) AS ParadasCompletadas
FROM genesis.Rutas r
LEFT JOIN genesis.Paradas p ON r.IdRuta = p.IdRuta
WHERE r.IdBus = 4
GROUP BY r.IdRuta, r.Nombre, r.TipoRuta, r.HoraInicio, r.EsActiva
ORDER BY r.IdRuta;
*/

-- Ver detalle de paradas de una ruta específica
/*
SELECT 
    p.IdParada,
    p.Orden,
    ISNULL(a.Nombre + ' ' + a.Apellido, 'COLEGIO / PUNTO FINAL') AS Alumno,
    p.Direccion,
    CONVERT(VARCHAR(5), p.HoraEstimada, 108) AS HoraEstimada,
    CASE WHEN p.Completada = 1 THEN 'Sí' ELSE 'No' END AS Completada
FROM genesis.Paradas p
LEFT JOIN genesis.Alumnos a ON p.IdAlumno = a.IdAlumno
WHERE p.IdRuta = 11  -- Cambiar por 10 (Mañana) o 11 (Tarde)
ORDER BY p.Orden;
*/

-- Ver estado actual del Bus BUS-001
/*
SELECT 
    b.IdBus,
    b.Placa,
    b.Modelo,
    b.Capacidad,
    CASE WHEN b.Estado = 1 THEN 'Activo' ELSE 'Inactivo' END AS Estado,
    COUNT(DISTINCT a.IdAlumno) AS AlumnosAsignados
FROM genesis.Buses b
LEFT JOIN genesis.Alumnos a ON b.IdBus = a.IdBusAsignado
WHERE b.IdBus = 4
GROUP BY b.IdBus, b.Placa, b.Modelo, b.Capacidad, b.Estado;
*/


-- ============================================
-- OPCIÓN 6: LIMPIAR DATOS DE PRUEBA (Si necesitas empezar de nuevo)
-- ============================================
/*
-- ⚠️ CUIDADO: Esto eliminará TODOS los datos de prueba
BEGIN TRANSACTION;

DELETE FROM genesis.Paradas WHERE IdParada BETWEEN 100 AND 200;
DELETE FROM genesis.Rutas WHERE IdRuta BETWEEN 10 AND 12;
DELETE FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220;
DELETE FROM genesis.Padres WHERE IdPadre BETWEEN 100 AND 110;

-- Si quieres también eliminar el bus (opcional)
-- DELETE FROM genesis.Buses WHERE IdBus = 4;

COMMIT TRANSACTION;

PRINT 'Datos de prueba eliminados. Ejecuta SeedData_DashboardMonitor_Test.sql para volver a insertarlos.';
*/

PRINT '========================================='
PRINT 'Script de Configuración Cargado'
PRINT '========================================='
PRINT 'Descomenta las secciones que necesites usar'
PRINT 'durante la presentación.'
PRINT '========================================='
GO
