-- =============================================
-- Script: BUSCAR USUARIOS EXISTENTES
-- Contraseña común: Admin123!
-- =============================================

USE [TransportesGenesis]
GO

PRINT '========================================='
PRINT '🔍 BUSCANDO USUARIOS DEL SISTEMA'
PRINT '========================================='
PRINT ''

-- =============================================
-- USUARIOS POR ROL
-- =============================================

-- ADMINISTRADORES
PRINT '👤 ADMINISTRADORES:'
PRINT '----------------------------------------'
SELECT 
	'   Usuario: ' + u.Email AS 'Login',
	'   Contraseña: Admin123!' AS 'Password',
	'   Estado: ' + CASE WHEN u.EmailConfirmed = 1 THEN 'Activo ✅' ELSE 'Inactivo ❌' END AS 'Estado'
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'Administrador'

IF @@ROWCOUNT = 0
	PRINT '   ⚠️ No se encontraron Administradores'

PRINT ''

-- PILOTOS
PRINT '🚗 PILOTOS:'
PRINT '----------------------------------------'
SELECT 
	'   Usuario: ' + u.Email AS 'Login',
	'   Contraseña: Admin123!' AS 'Password',
	'   Estado: ' + CASE WHEN u.EmailConfirmed = 1 THEN 'Activo ✅' ELSE 'Inactivo ❌' END AS 'Estado'
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'Piloto'

IF @@ROWCOUNT = 0
	PRINT '   ⚠️ No se encontraron Pilotos'

PRINT ''

-- MONITORES
PRINT '👨‍🏫 MONITORES:'
PRINT '----------------------------------------'
SELECT 
	'   Usuario: ' + u.Email AS 'Login',
	'   Contraseña: Admin123!' AS 'Password',
	'   Estado: ' + CASE WHEN u.EmailConfirmed = 1 THEN 'Activo ✅' ELSE 'Inactivo ❌' END AS 'Estado'
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'Monitor'

IF @@ROWCOUNT = 0
	PRINT '   ⚠️ No se encontraron Monitores'

PRINT ''

-- PADRES DE FAMILIA
PRINT '👨‍👩‍👧 PADRES DE FAMILIA:'
PRINT '----------------------------------------'
SELECT 
	'   Usuario: ' + u.Email AS 'Login',
	'   Contraseña: Admin123!' AS 'Password',
	'   Estado: ' + CASE WHEN u.EmailConfirmed = 1 THEN 'Activo ✅' ELSE 'Inactivo ❌' END AS 'Estado'
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'PadreDeFamilia'

IF @@ROWCOUNT = 0
	PRINT '   ⚠️ No se encontraron Padres de Familia'

PRINT ''
PRINT '========================================='

-- =============================================
-- RESUMEN RÁPIDO PARA COPIAR/PEGAR
-- =============================================

PRINT ''
PRINT '📋 RESUMEN PARA TU PRESENTACIÓN:'
PRINT '========================================='
PRINT ''

DECLARE @AdminEmail NVARCHAR(256) = (
	SELECT TOP 1 u.Email 
	FROM AspNetUsers u
	INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
	INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
	WHERE r.Name = 'Administrador'
)

DECLARE @PilotoEmail NVARCHAR(256) = (
	SELECT TOP 1 u.Email 
	FROM AspNetUsers u
	INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
	INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
	WHERE r.Name = 'Piloto'
)

DECLARE @MonitorEmail NVARCHAR(256) = (
	SELECT TOP 1 u.Email 
	FROM AspNetUsers u
	INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
	INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
	WHERE r.Name = 'Monitor'
)

DECLARE @PadreEmail NVARCHAR(256) = (
	SELECT TOP 1 u.Email 
	FROM AspNetUsers u
	INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
	INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
	WHERE r.Name = 'PadreDeFamilia'
)

-- Mostrar en formato limpio
IF @AdminEmail IS NOT NULL
BEGIN
	PRINT '👤 ADMINISTRADOR:'
	PRINT '   Email: ' + @AdminEmail
	PRINT '   Contraseña: Admin123!'
	PRINT ''
END

IF @PilotoEmail IS NOT NULL
BEGIN
	PRINT '🚗 PILOTO:'
	PRINT '   Email: ' + @PilotoEmail
	PRINT '   Contraseña: Admin123!'
	PRINT ''
END

IF @MonitorEmail IS NOT NULL
BEGIN
	PRINT '👨‍🏫 MONITOR:'
	PRINT '   Email: ' + @MonitorEmail
	PRINT '   Contraseña: Admin123!'
	PRINT ''
END

IF @PadreEmail IS NOT NULL
BEGIN
	PRINT '👨‍👩‍👧 PADRE DE FAMILIA:'
	PRINT '   Email: ' + @PadreEmail
	PRINT '   Contraseña: Admin123!'
	PRINT ''
END

PRINT '========================================='

-- =============================================
-- VERIFICAR ROLES FALTANTES
-- =============================================

PRINT ''
PRINT '⚠️ VERIFICACIÓN DE ROLES:'
PRINT '========================================='

DECLARE @MissingRoles TABLE (Rol NVARCHAR(50))

IF @AdminEmail IS NULL
	INSERT INTO @MissingRoles VALUES ('Administrador')

IF @PilotoEmail IS NULL
	INSERT INTO @MissingRoles VALUES ('Piloto')

IF @MonitorEmail IS NULL
	INSERT INTO @MissingRoles VALUES ('Monitor')

IF @PadreEmail IS NULL
	INSERT INTO @MissingRoles VALUES ('PadreDeFamilia')

IF EXISTS (SELECT 1 FROM @MissingRoles)
BEGIN
	PRINT ''
	PRINT '⚠️ ROLES FALTANTES (necesitas crear usuarios):'
	SELECT '   ❌ ' + Rol AS 'Rol Faltante' FROM @MissingRoles
	PRINT ''
END
ELSE
BEGIN
	PRINT ''
	PRINT '✅ ¡PERFECTO! Tienes usuarios para todos los roles'
	PRINT ''
END

PRINT '========================================='
PRINT '✅ Consulta completada'
PRINT '========================================='
