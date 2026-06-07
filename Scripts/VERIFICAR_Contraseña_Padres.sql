-- =============================================
-- Script: VERIFICAR CONTRASEÑAS DE PADRES
-- Comparar hashes para ver si usan Admin123!
-- =============================================

USE [TransportesGenesis]
GO

PRINT '========================================='
PRINT '🔍 VERIFICANDO PADRES DE FAMILIA'
PRINT '========================================='
PRINT ''

-- Buscar los usuarios padre
DECLARE @PadreTest NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE Email = 'padre@test.com')
DECLARE @Padre1Gmail NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE Email = 'padre1@gmail.com')

-- Mostrar información de padre@test.com
IF @PadreTest IS NOT NULL
BEGIN
	PRINT '📧 Usuario: padre@test.com'
	PRINT '   ID: ' + @PadreTest

	SELECT 
		'   Hash (primeros 50 caracteres): ' + LEFT(PasswordHash, 50) + '...' AS 'Info'
	FROM AspNetUsers 
	WHERE Id = @PadreTest

	SELECT 
		'   Email confirmado: ' + CASE WHEN EmailConfirmed = 1 THEN 'SI ✅' ELSE 'NO ❌' END AS 'Estado'
	FROM AspNetUsers 
	WHERE Id = @PadreTest

	PRINT ''
END
ELSE
BEGIN
	PRINT '❌ NO EXISTE: padre@test.com'
	PRINT ''
END

-- Mostrar información de padre1@gmail.com
IF @Padre1Gmail IS NOT NULL
BEGIN
	PRINT '📧 Usuario: padre1@gmail.com'
	PRINT '   ID: ' + @Padre1Gmail

	SELECT 
		'   Hash (primeros 50 caracteres): ' + LEFT(PasswordHash, 50) + '...' AS 'Info'
	FROM AspNetUsers 
	WHERE Id = @Padre1Gmail

	SELECT 
		'   Email confirmado: ' + CASE WHEN EmailConfirmed = 1 THEN 'SI ✅' ELSE 'NO ❌' END AS 'Estado'
	FROM AspNetUsers 
	WHERE Id = @Padre1Gmail

	PRINT ''
END
ELSE
BEGIN
	PRINT '❌ NO EXISTE: padre1@gmail.com'
	PRINT ''
END

PRINT '========================================='
PRINT ''

-- Comparar con hash conocido (si tienes un usuario con Admin123! para comparar)
PRINT '🔐 COMPARACIÓN DE HASHES:'
PRINT '========================================='

-- Buscar un usuario que sabemos que tiene Admin123! (ej: admin)
DECLARE @AdminHash NVARCHAR(MAX) = (
	SELECT TOP 1 PasswordHash 
	FROM AspNetUsers u
	INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
	INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
	WHERE r.Name = 'Administrador'
)

IF @AdminHash IS NOT NULL
BEGIN
	PRINT '✅ Hash del Administrador (sabemos que es Admin123!):'
	PRINT '   ' + LEFT(@AdminHash, 50) + '...'
	PRINT ''

	-- Comparar con padre@test.com
	IF @PadreTest IS NOT NULL
	BEGIN
		DECLARE @PadreTestHash NVARCHAR(MAX) = (SELECT PasswordHash FROM AspNetUsers WHERE Id = @PadreTest)

		IF @PadreTestHash = @AdminHash
		BEGIN
			PRINT '✅ padre@test.com tiene la MISMA contraseña que el Admin'
			PRINT '   👉 Contraseña: Admin123!'
		END
		ELSE
		BEGIN
			PRINT '⚠️ padre@test.com tiene una contraseña DIFERENTE'
			PRINT '   👉 NO es Admin123!'
		END
		PRINT ''
	END

	-- Comparar con padre1@gmail.com
	IF @Padre1Gmail IS NOT NULL
	BEGIN
		DECLARE @Padre1Hash NVARCHAR(MAX) = (SELECT PasswordHash FROM AspNetUsers WHERE Id = @Padre1Gmail)

		IF @Padre1Hash = @AdminHash
		BEGIN
			PRINT '✅ padre1@gmail.com tiene la MISMA contraseña que el Admin'
			PRINT '   👉 Contraseña: Admin123!'
		END
		ELSE
		BEGIN
			PRINT '⚠️ padre1@gmail.com tiene una contraseña DIFERENTE'
			PRINT '   👉 NO es Admin123!'
		END
		PRINT ''
	END
END
ELSE
BEGIN
	PRINT '⚠️ No se encontró un administrador para comparar'
END

PRINT '========================================='
PRINT ''

-- SOLUCIONES si no coinciden
PRINT '💡 SOLUCIONES:'
PRINT '========================================='
PRINT ''
PRINT 'Si algún usuario NO tiene Admin123!, puedes:'
PRINT ''
PRINT '1️⃣ Usar "Olvidé mi contraseña" en el login'
PRINT '   - Más seguro'
PRINT '   - Envía email de reset'
PRINT ''
PRINT '2️⃣ Cambiar el hash manualmente (más rápido para demo):'
PRINT ''
PRINT '   -- Copiar el hash del admin a padre@test.com:'
PRINT '   UPDATE AspNetUsers'
PRINT '   SET PasswordHash = (SELECT TOP 1 PasswordHash FROM AspNetUsers u'
PRINT '                        INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId'
PRINT '                        INNER JOIN AspNetRoles r ON ur.RoleId = r.Id'
PRINT '                        WHERE r.Name = ''Administrador'')'
PRINT '   WHERE Email = ''padre@test.com'''
PRINT ''
PRINT '   -- Copiar el hash del admin a padre1@gmail.com:'
PRINT '   UPDATE AspNetUsers'
PRINT '   SET PasswordHash = (SELECT TOP 1 PasswordHash FROM AspNetUsers u'
PRINT '                        INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId'
PRINT '                        INNER JOIN AspNetRoles r ON ur.RoleId = r.Id'
PRINT '                        WHERE r.Name = ''Administrador'')'
PRINT '   WHERE Email = ''padre1@gmail.com'''
PRINT ''
PRINT '========================================='
