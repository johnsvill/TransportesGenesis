-- Script para crear usuarios piloto1 y monitor1 usando el MISMO hash del admin
-- Esto significa que TODOS tendrán la misma contraseña
-- Password: La misma que usas para el admin

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

-- Hash del admin (que SÍ funciona)
DECLARE @PasswordHash NVARCHAR(MAX) = 'AQAAAAIAAYagAAAAEEvtF+pVJfi5gRqOAQl5sdzGt0g4M3Qv4ysnyn0XW0CSow9A+cPmo3661f+Aivfn8g==';

-- IDs únicos para los nuevos usuarios
DECLARE @PilotoId NVARCHAR(450) = NEWID();
DECLARE @MonitorId NVARCHAR(450) = NEWID();

-- Crear usuario Piloto con el MISMO hash del admin
INSERT INTO AspNetUsers (
    Id, 
    UserName, 
    NormalizedUserName, 
    Email, 
    NormalizedEmail, 
    EmailConfirmed, 
    PasswordHash, 
    SecurityStamp, 
    ConcurrencyStamp, 
    PhoneNumberConfirmed, 
    TwoFactorEnabled, 
    LockoutEnabled, 
    AccessFailedCount, 
    IsFirstLogin,
    LastLoginDate
)
VALUES (
    @PilotoId, 
    'piloto1', 
    'PILOTO1', 
    'piloto1@transportesgenesis.com', 
    'PILOTO1@TRANSPORTESGENESIS.COM', 
    1, 
    @PasswordHash,  -- ← MISMO HASH DEL ADMIN
    NEWID(), 
    NEWID(), 
    0, 
    0, 
    1, 
    0, 
    1,
    NULL
);

-- Crear usuario Monitor con el MISMO hash del admin
INSERT INTO AspNetUsers (
    Id, 
    UserName, 
    NormalizedUserName, 
    Email, 
    NormalizedEmail, 
    EmailConfirmed, 
    PasswordHash, 
    SecurityStamp, 
    ConcurrencyStamp, 
    PhoneNumberConfirmed, 
    TwoFactorEnabled, 
    LockoutEnabled, 
    AccessFailedCount, 
    IsFirstLogin,
    LastLoginDate
)
VALUES (
    @MonitorId, 
    'monitor1', 
    'MONITOR1', 
    'monitor1@transportesgenesis.com', 
    'MONITOR1@TRANSPORTESGENESIS.COM', 
    1, 
    @PasswordHash,  -- ← MISMO HASH DEL ADMIN
    NEWID(), 
    NEWID(), 
    0, 
    0, 
    1, 
    0, 
    1,
    NULL
);

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
PRINT '✅ Usuarios creados exitosamente';
PRINT '';
PRINT '📋 CREDENCIALES:';
PRINT '════════════════════════════════════════';
PRINT 'PILOTO:';
PRINT '  Email/Username: piloto1@transportesgenesis.com';
PRINT '  Password: [LA MISMA DEL ADMIN]';
PRINT '';
PRINT 'MONITOR:';
PRINT '  Email/Username: monitor1@transportesgenesis.com';
PRINT '  Password: [LA MISMA DEL ADMIN]';
PRINT '════════════════════════════════════════';

-- Verificar
SELECT 
    u.UserName AS Usuario,
    u.Email,
    r.Name AS Rol,
    CASE WHEN a.IdBus IS NOT NULL THEN 'Bus #' + CAST(a.IdBus AS VARCHAR) ELSE 'Sin asignar' END AS BusAsignado
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
LEFT JOIN genesis.AsignacionPilotoBus a ON u.Id = a.IdUsuarioPiloto AND a.EsActual = 1
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
ORDER BY u.UserName;
