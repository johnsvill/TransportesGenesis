-- =============================================
-- Script: CONSULTAR USUARIOS EXISTENTES
-- Propósito: Ver qué usuarios ya tienes en la BD
-- =============================================

USE [TransportesGenesis]
GO

PRINT '========================================='
PRINT '📋 USUARIOS REGISTRADOS EN EL SISTEMA'
PRINT '========================================='
PRINT ''

-- Consultar todos los usuarios con sus roles
SELECT 
	u.Email AS '📧 Usuario/Email',
	r.Name AS '👤 Rol',
	CASE 
		WHEN u.EmailConfirmed = 1 THEN '✅ Confirmado' 
		ELSE '❌ No confirmado' 
	END AS '📨 Email',
	CASE 
		WHEN u.LockoutEnabled = 1 AND u.LockoutEnd IS NULL THEN '🟢 Activo'
		WHEN u.LockoutEnd IS NOT NULL AND u.LockoutEnd > GETDATE() THEN '🔒 Bloqueado'
		ELSE '🟡 Inactivo'
	END AS '🔐 Estado',
	u.PhoneNumber AS '📞 Teléfono',
	FORMAT(u.LockoutEnd, 'dd/MM/yyyy HH:mm') AS '⏰ Bloqueado Hasta'
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
ORDER BY 
	CASE r.Name
		WHEN 'Administrador' THEN 1
		WHEN 'Piloto' THEN 2
		WHEN 'Monitor' THEN 3
		WHEN 'PadreDeFamilia' THEN 4
		ELSE 5
	END,
	u.Email

PRINT ''
PRINT '========================================='

-- Estadísticas de usuarios por rol
PRINT ''
PRINT '📊 ESTADÍSTICAS POR ROL:'
PRINT '========================================='

SELECT 
	ISNULL(r.Name, 'Sin Rol') AS 'Rol',
	COUNT(u.Id) AS 'Total Usuarios'
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
GROUP BY r.Name
ORDER BY COUNT(u.Id) DESC

PRINT ''
PRINT '========================================='

-- Mostrar usuarios sin rol asignado
PRINT ''
PRINT '⚠️ USUARIOS SIN ROL ASIGNADO:'
PRINT '========================================='

SELECT 
	u.Email AS 'Email',
	u.UserName AS 'Username',
	CASE WHEN u.EmailConfirmed = 1 THEN '✅' ELSE '❌' END AS 'Confirmado'
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
WHERE ur.UserId IS NULL

-- Si no hay usuarios sin rol
IF NOT EXISTS (
	SELECT 1 FROM AspNetUsers u
	LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
	WHERE ur.UserId IS NULL
)
BEGIN
	PRINT '✅ Todos los usuarios tienen un rol asignado'
END

PRINT ''
PRINT '========================================='
PRINT '✅ Consulta completada'
PRINT '========================================='

-- BONUS: Mostrar información útil para login
PRINT ''
PRINT '🔑 INFORMACIÓN PARA LOGIN:'
PRINT '========================================='
PRINT ''
PRINT 'Para loguearte con estos usuarios, la contraseña'
PRINT 'depende de cómo fueron creados originalmente.'
PRINT ''
PRINT 'Si quieres cambiar una contraseña, usa:'
PRINT 'UPDATE AspNetUsers SET PasswordHash = ''[nuevo_hash]'' WHERE Email = ''usuario@ejemplo.com'''
PRINT ''
PRINT 'O usa la funcionalidad de "Olvidé mi contraseña"'
PRINT 'en la página de login.'
PRINT ''
