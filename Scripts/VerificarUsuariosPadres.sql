-- Script para verificar usuarios con rol "Padre de Familia"
-- Ejecuta este script en SQL Server Management Studio o Azure Data Studio

USE TransportesGenesis;
GO

-- 1. Verificar si existe el rol "PadreDeFamilia"
SELECT 
    Id,
    Name as NombreRol,
    NormalizedName
FROM AspNetRoles
WHERE NormalizedName LIKE '%PADRE%';
GO

-- 2. Buscar usuarios con rol "PadreDeFamilia"
SELECT 
    u.Id as UserId,
    u.UserName,
    u.Email,
    u.EmailConfirmed,
    u.IsFirstLogin,
    u.DebeCambiarPassword,
    r.Name as Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.NormalizedName = 'PADREDEFAMILIA'
ORDER BY u.UserName;
GO

-- 3. Contar usuarios por rol
SELECT 
    r.Name as Rol,
    COUNT(ur.UserId) as TotalUsuarios
FROM AspNetRoles r
LEFT JOIN AspNetUserRoles ur ON r.Id = ur.RoleId
GROUP BY r.Name
ORDER BY COUNT(ur.UserId) DESC;
GO

-- 4. Ver TODOS los usuarios con sus roles
SELECT 
    u.UserName,
    u.Email,
    u.EmailConfirmed,
    u.IsFirstLogin,
    u.DebeCambiarPassword,
    r.Name as Rol
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
ORDER BY r.Name, u.UserName;
GO

-- 5. Ver usuarios SIN rol asignado
SELECT 
    u.UserName,
    u.Email,
    'SIN ROL' as Estado
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
WHERE ur.UserId IS NULL
ORDER BY u.UserName;
GO

-- 6. Ver estructura completa de alumnos y padres (si existe relación)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'genesis' AND TABLE_NAME = 'Alumnos')
BEGIN
    SELECT 
        a.IdAlumno,
        a.Nombre + ' ' + a.Apellido as NombreAlumno,
        a.Codigo,
        u.UserName as UsuarioPadre,
        u.Email as EmailPadre
    FROM genesis.Alumnos a
    LEFT JOIN AspNetUsers u ON a.IdUsuarioPadre = u.Id
    ORDER BY a.Apellido, a.Nombre;
END
ELSE
BEGIN
    PRINT 'La tabla genesis.Alumnos no existe';
END
GO
