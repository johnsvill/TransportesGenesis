-- ========================================
-- Script para Vincular Padres con Usuarios
-- ========================================
-- Este script te ayuda a vincular los padres existentes con usuarios de AspNetUsers
-- Ejecuta este script DESPUÉS de crear los usuarios correspondientes en el sistema

USE TransportesGenesis;
GO

-- ========================================
-- PASO 1: Ver padres sin vincular
-- ========================================
PRINT '========================================';
PRINT 'PADRES SIN VINCULAR CON USUARIOS';
PRINT '========================================';
SELECT 
	p.IdPadre,
	p.Nombre + ' ' + p.Apellido AS NombreCompleto,
	p.UsuarioId,
	CASE 
		WHEN p.UsuarioId IS NULL THEN '❌ Sin vincular'
		ELSE '✅ Vinculado'
	END AS Estado
FROM genesis.Padres p
ORDER BY p.IdPadre;
GO

-- ========================================
-- PASO 2: Ver usuarios disponibles (rol Padre)
-- ========================================
PRINT '';
PRINT '========================================';
PRINT 'USUARIOS CON ROL PADRE DISPONIBLES';
PRINT '========================================';
SELECT 
	u.Id AS UsuarioId,
	u.UserName,
	u.Email,
	u.EmailConfirmed,
	CASE 
		WHEN EXISTS (SELECT 1 FROM genesis.Padres WHERE UsuarioId = u.Id) THEN '✅ Ya vinculado'
		ELSE '❌ Sin vincular'
	END AS Estado
FROM AspNetUsers u
WHERE u.Id IN (
	SELECT UserId 
	FROM AspNetUserRoles ur
	INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
	WHERE r.Name = 'Padre'
)
ORDER BY u.Email;
GO

-- ========================================
-- PASO 3: Plantillas de vinculación
-- ========================================
PRINT '';
PRINT '========================================';
PRINT 'PLANTILLAS PARA VINCULAR';
PRINT '========================================';
PRINT 'Copia y modifica estas plantillas según corresponda:';
PRINT '';

-- Ejemplo 1: Vincular por nombre y email
PRINT '-- Ejemplo 1: Vincular padre por coincidencia de nombre/email';
PRINT '/*';
PRINT 'UPDATE genesis.Padres';
PRINT 'SET UsuarioId = (SELECT Id FROM AspNetUsers WHERE Email = ''juan.lopez@gmail.com'')';
PRINT 'WHERE Nombre = ''Juan'' AND Apellido LIKE ''López%'';';
PRINT '*/';
PRINT '';

-- Ejemplo 2: Vincular por ID específico
PRINT '-- Ejemplo 2: Vincular padre por ID específico';
PRINT '/*';
PRINT 'UPDATE genesis.Padres';
PRINT 'SET UsuarioId = ''<ID_USUARIO_AQUI>''';
PRINT 'WHERE IdPadre = 98;';
PRINT '*/';
PRINT '';

-- ========================================
-- PASO 4: Script de vinculación automática (comentado)
-- ========================================
PRINT '-- PASO 4: Script de vinculación automática';
PRINT '-- Descomenta y ajusta según tus necesidades:';
PRINT '';

/*
-- OPCIÓN A: Vincular si el email coincide con el nombre del padre
UPDATE p
SET p.UsuarioId = u.Id
FROM genesis.Padres p
INNER JOIN AspNetUsers u ON 
	LOWER(u.Email) LIKE LOWER(p.Nombre) + '%' OR
	LOWER(u.Email) LIKE '%' + LOWER(p.Apellido) + '%'
WHERE p.UsuarioId IS NULL
  AND u.Id IN (
	  SELECT UserId 
	  FROM AspNetUserRoles ur
	  INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
	  WHERE r.Name = 'Padre'
  );

PRINT 'Vinculación automática completada';
*/

-- ========================================
-- PASO 5: Verificación final
-- ========================================
PRINT '';
PRINT '========================================';
PRINT 'VERIFICACIÓN FINAL';
PRINT '========================================';
SELECT 
	p.IdPadre,
	p.Nombre + ' ' + p.Apellido AS Padre,
	u.Email AS EmailUsuario,
	CASE 
		WHEN p.UsuarioId IS NOT NULL AND u.Id IS NOT NULL THEN '✅ Vinculado correctamente'
		WHEN p.UsuarioId IS NOT NULL AND u.Id IS NULL THEN '⚠️ UsuarioId inválido'
		ELSE '❌ Sin vincular'
	END AS EstadoVinculacion
FROM genesis.Padres p
LEFT JOIN AspNetUsers u ON p.UsuarioId = u.Id
ORDER BY p.IdPadre;
GO

-- ========================================
-- PASO 6: Contar estadísticas
-- ========================================
PRINT '';
PRINT '========================================';
PRINT 'ESTADÍSTICAS';
PRINT '========================================';
DECLARE @TotalPadres INT, @PadresVinculados INT, @PadresSinVincular INT;

SELECT @TotalPadres = COUNT(*) FROM genesis.Padres;
SELECT @PadresVinculados = COUNT(*) FROM genesis.Padres WHERE UsuarioId IS NOT NULL;
SELECT @PadresSinVincular = COUNT(*) FROM genesis.Padres WHERE UsuarioId IS NULL;

PRINT 'Total de padres: ' + CAST(@TotalPadres AS VARCHAR(10));
PRINT 'Padres vinculados: ' + CAST(@PadresVinculados AS VARCHAR(10));
PRINT 'Padres sin vincular: ' + CAST(@PadresSinVincular AS VARCHAR(10));
PRINT 'Porcentaje vinculado: ' + CAST((@PadresVinculados * 100.0 / NULLIF(@TotalPadres, 0)) AS VARCHAR(10)) + '%';
GO

PRINT '';
PRINT '========================================';
PRINT 'SCRIPT COMPLETADO';
PRINT '========================================';
PRINT 'Siguiente paso: Ejecuta los UPDATE necesarios para vincular padres con usuarios';
PRINT 'Usa las plantillas de arriba o crea tus propias consultas UPDATE';
