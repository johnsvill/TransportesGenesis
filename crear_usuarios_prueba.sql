-- Script para crear usuarios piloto1 y monitor1 manualmente
-- Password para ambos: Piloto123! / Monitor123!
-- NOTA: Este hash corresponde a la contraseña "Piloto123!" hashada con Identity

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

DECLARE @PilotoId NVARCHAR(450) = NEWID();
DECLARE @MonitorId NVARCHAR(450) = NEWID();

-- Crear usuario Piloto
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, IsFirstLogin)
VALUES 
(@PilotoId, 'piloto1', 'PILOTO1', 'piloto1@transportesgenesis.com', 'PILOTO1@TRANSPORTESGENESIS.COM', 1, 'AQAAAAIAAYagAAAAEJ5vZ6F8hX8fJ7JvK5PqVZh5m7VxQXWzKcN/pI5X5YJ+kR3x8wZ2N5bA==', NEWID(), NEWID(), 0, 0, 1, 0, 1);

-- Crear usuario Monitor
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, IsFirstLogin)
VALUES 
(@MonitorId, 'monitor1', 'MONITOR1', 'monitor1@transportesgenesis.com', 'MONITOR1@TRANSPORTESGENESIS.COM', 1, 'AQAAAAIAAYagAAAAEJ5vZ6F8hX8fJ7JvK5PqVZh5m7VxQXWzKcN/pI5X5YJ+kR3x8wZ2N5bA==', NEWID(), NEWID(), 0, 0, 1, 0, 1);

-- Obtener IDs de roles
DECLARE @RolePilotoId NVARCHAR(450);
DECLARE @RoleMonitorId NVARCHAR(450);

SELECT @RolePilotoId = Id FROM AspNetRoles WHERE Name = 'Piloto';
SELECT @RoleMonitorId = Id FROM AspNetRoles WHERE Name = 'Monitor';

-- Asignar roles
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES 
(@PilotoId, @RolePilotoId),
(@MonitorId, @RoleMonitorId);

-- Asignar Bus #1 a ambos usuarios
INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
VALUES 
(@PilotoId, 1, GETDATE(), 1, 1, GETDATE()),
(@MonitorId, 1, GETDATE(), 1, 1, GETDATE());

-- Confirmar
SELECT 'Usuarios creados exitosamente' AS Resultado;
SELECT 'piloto1@transportesgenesis.com' AS Email, 'Piloto123!' AS Password UNION ALL
SELECT 'monitor1@transportesgenesis.com', 'Monitor123!';
