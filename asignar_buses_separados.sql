-- Script para asignar buses a piloto1 (Bus #1) y monitor1 (Bus #4 que tiene rutas)
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

-- Obtener IDs de los usuarios
DECLARE @PilotoId NVARCHAR(450);
DECLARE @MonitorId NVARCHAR(450);

SELECT @PilotoId = Id FROM AspNetUsers WHERE Email = 'piloto1@transportesgenesis.com';
SELECT @MonitorId = Id FROM AspNetUsers WHERE Email = 'monitor1@transportesgenesis.com';

-- Limpiar tabla por seguridad
DELETE FROM genesis.AsignacionPilotoBus;

-- Asignar Bus #1 al piloto y Bus #4 al monitor (que tiene rutas)
INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
VALUES 
(@PilotoId, 1, GETDATE(), 1, 1, GETDATE()),
(@MonitorId, 4, GETDATE(), 1, 1, GETDATE());

-- Verificar resultado
SELECT 
    u.UserName AS Usuario,
    u.Email,
    r.Name AS Rol,
    a.IdBus AS BusAsignado,
    b.Placa,
    b.Modelo,
    (SELECT COUNT(*) FROM genesis.Rutas WHERE IdBus = a.IdBus AND EsActiva = 1) AS RutasActivas
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
INNER JOIN genesis.AsignacionPilotoBus a ON u.Id = a.IdUsuarioPiloto AND a.EsActual = 1
INNER JOIN genesis.Buses b ON a.IdBus = b.IdBus
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
ORDER BY u.UserName;

PRINT '';
PRINT '✅ Piloto asignado al Bus #1';
PRINT '✅ Monitor asignado al Bus #4 (tiene 5 rutas activas)';
