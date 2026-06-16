-- =============================================
-- Script: IGUALAR CONTRASEÑA DE PADRES
-- Establecer Admin123! para todos los padres
-- =============================================

USE [TransportesGenesis]
GO

PRINT '========================================='
PRINT '🔐 IGUALANDO CONTRASEÑA A Admin123!'
PRINT '========================================='
PRINT ''

-- Obtener el hash del administrador (que sabemos es Admin123!)
DECLARE @HashAdmin123 NVARCHAR(MAX) = (
	SELECT TOP 1 PasswordHash 
	FROM AspNetUsers u
	INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
	INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
	WHERE r.Name = 'Administrador'
)

IF @HashAdmin123 IS NULL
BEGIN
	PRINT '❌ ERROR: No se encontró un Administrador'
	PRINT 'Necesitas tener al menos un Admin para copiar su hash'
	RETURN
END

PRINT '✅ Hash de Admin123! obtenido'
PRINT ''

-- Actualizar padre@test.com
IF EXISTS (SELECT 1 FROM AspNetUsers WHERE Email = 'padre@test.com')
BEGIN
	UPDATE AspNetUsers
	SET PasswordHash = @HashAdmin123,
		EmailConfirmed = 1,  -- Confirmar email también
		LockoutEnabled = 1,  -- Permitir login
		LockoutEnd = NULL    -- No bloqueado
	WHERE Email = 'padre@test.com'

	PRINT '✅ padre@test.com actualizado'
	PRINT '   Usuario: padre@test.com'
	PRINT '   Contraseña: Admin123!'
	PRINT '   Email confirmado: SI'
	PRINT ''
END
ELSE
BEGIN
	PRINT '⚠️ padre@test.com NO EXISTE en la base de datos'
	PRINT ''
END

-- Actualizar padre1@gmail.com
IF EXISTS (SELECT 1 FROM AspNetUsers WHERE Email = 'padre1@gmail.com')
BEGIN
	UPDATE AspNetUsers
	SET PasswordHash = @HashAdmin123,
		EmailConfirmed = 1,  -- Confirmar email también
		LockoutEnabled = 1,  -- Permitir login
		LockoutEnd = NULL    -- No bloqueado
	WHERE Email = 'padre1@gmail.com'

	PRINT '✅ padre1@gmail.com actualizado'
	PRINT '   Usuario: padre1@gmail.com'
	PRINT '   Contraseña: Admin123!'
	PRINT '   Email confirmado: SI'
	PRINT ''
END
ELSE
BEGIN
	PRINT '⚠️ padre1@gmail.com NO EXISTE en la base de datos'
	PRINT ''
END

PRINT '========================================='
PRINT ''
PRINT '🎉 LISTO PARA LA DEMO'
PRINT '========================================='
PRINT ''
PRINT '✅ Ahora TODOS los usuarios tienen: Admin123!'
PRINT ''
PRINT '📋 Puedes usar cualquiera de estos para la demo:'
PRINT ''

-- Mostrar todos los padres con su estado
SELECT 
	u.Email AS 'Usuario',
	'Admin123!' AS 'Contraseña',
	CASE WHEN u.EmailConfirmed = 1 THEN 'Activo ✅' ELSE 'Inactivo ❌' END AS 'Estado'
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'PadreDeFamilia'
  AND u.Email IN ('padre@test.com', 'padre1@gmail.com')

PRINT ''
PRINT '========================================='
PRINT '✅ Script completado exitosamente'
PRINT '========================================='
