-- Script para asignar Bus #4 a piloto1 y monitor1 (que tiene rutas creadas)
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

-- Obtener IDs de los usuarios
DECLARE @PilotoId NVARCHAR(450);
DECLARE @MonitorId NVARCHAR(450);

SELECT @PilotoId = Id FROM AspNetUsers WHERE Email = 'piloto1@transportesgenesis.com';
SELECT @MonitorId = Id FROM AspNetUsers WHERE Email = 'monitor1@transportesgenesis.com';

-- Eliminar asignaciones anteriores si existen
DELETE FROM genesis.AsignacionPilotoBus WHERE IdUsuarioPiloto IN (@PilotoId, @MonitorId);

-- Asignar Bus #4 a ambos usuarios (que tiene rutas creadas)
INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
VALUES 
(@PilotoId, 4, GETDATE(), 1, 1, GETDATE()),
(@MonitorId, 4, GETDATE(), 1, 1, GETDATE());

-- Verificar
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
LEFT JOIN genesis.AsignacionPilotoBus a ON u.Id = a.IdUsuarioPiloto AND a.EsActual = 1
LEFT JOIN genesis.Buses b ON a.IdBus = b.IdBus
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
ORDER BY u.UserName;

PRINT '';
PRINT '✅ Usuarios asignados al Bus #4';
PRINT '📋 Este bus tiene rutas activas con alumnos';
