-- ========================================
-- VINCULACIÓN AUTOMÁTICA DE USUARIOS PADRE
-- ========================================
-- Este script vincula los 6 usuarios con rol "PadreDeFamilia" 
-- con los primeros 6 padres que tienen alumnos asignados
-- ========================================

USE TransportesGenesis;
GO

PRINT '========================================';
PRINT 'ESTADO ANTES DE LA VINCULACIÓN';
PRINT '========================================';

-- Ver padres y sus alumnos
SELECT 
	p.IdPadre,
	p.Nombre + ' ' + p.Apellido AS Padre,
	p.UsuarioId AS UsuarioId_Actual,
	COUNT(a.IdAlumno) AS CantidadAlumnos
FROM genesis.Padres p
LEFT JOIN genesis.Alumnos a ON p.IdPadre = a.IdPadre
GROUP BY p.IdPadre, p.Nombre, p.Apellido, p.UsuarioId
HAVING COUNT(a.IdAlumno) > 0
ORDER BY p.IdPadre;

PRINT '';
PRINT '========================================';
PRINT 'USUARIOS PADRE DE FAMILIA DISPONIBLES';
PRINT '========================================';

-- Ver usuarios con rol PadreDeFamilia
SELECT 
	u.Id AS UsuarioId,
	u.Email,
	u.UserName,
	CASE 
		WHEN EXISTS (SELECT 1 FROM genesis.Padres WHERE UsuarioId = u.Id) 
		THEN 'Ya vinculado'
		ELSE 'Disponible'
	END AS Estado
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'PadreDeFamilia'
ORDER BY u.Email;

PRINT '';
PRINT '========================================';
PRINT 'EJECUTANDO VINCULACIÓN AUTOMÁTICA';
PRINT '========================================';
PRINT '';

-- VINCULACIÓN 1: padre@test.com → Padre ID 1 (Juan Carlos García López - 8 alumnos)
UPDATE genesis.Padres
SET UsuarioId = 'b13a3a00-7acb-42d1-b8a0-3d3b04a0126c'
WHERE IdPadre = 1;
PRINT '✅ Vinculado: padre@test.com → Juan Carlos García López (8 alumnos)';

-- VINCULACIÓN 2: padre1@gmail.com → Padre ID 100 (Carlos Martínez López - 2 alumnos)
UPDATE genesis.Padres
SET UsuarioId = 'da0f2a70-6eec-49f9-bb31-ec3faa8c5788'
WHERE IdPadre = 100;
PRINT '✅ Vinculado: padre1@gmail.com → Carlos Martínez López (2 alumnos)';

-- VINCULACIÓN 3: padre2@gmail.com → Padre ID 101 (María García Hernández - 1 alumno)
UPDATE genesis.Padres
SET UsuarioId = '2de8b279-96f4-472b-b046-5b38d2353eb8'
WHERE IdPadre = 101;
PRINT '✅ Vinculado: padre2@gmail.com → María García Hernández (1 alumno)';

-- VINCULACIÓN 4: padre3@gmail.com → Padre ID 102 (José Rodríguez Pérez - 1 alumno)
UPDATE genesis.Padres
SET UsuarioId = '7dfd231f-6860-4f77-8c8d-9e9287e3c089'
WHERE IdPadre = 102;
PRINT '✅ Vinculado: padre3@gmail.com → José Rodríguez Pérez (1 alumno)';

-- VINCULACIÓN 5: padre4@gmail.com → Padre ID 103 (Ana López González - 1 alumno)
UPDATE genesis.Padres
SET UsuarioId = '19117879-b220-4942-b8de-d445084da903'
WHERE IdPadre = 103;
PRINT '✅ Vinculado: padre4@gmail.com → Ana López González (1 alumno)';

-- VINCULACIÓN 6: padre5@gmail.com → Padre ID 104 (Luis Hernández Morales - 1 alumno)
UPDATE genesis.Padres
SET UsuarioId = 'c5e454c3-f529-4f42-a191-1d46a97c017f'
WHERE IdPadre = 104;
PRINT '✅ Vinculado: padre5@gmail.com → Luis Hernández Morales (1 alumno)';

PRINT '';
PRINT '========================================';
PRINT 'VERIFICACIÓN POST-VINCULACIÓN';
PRINT '========================================';

-- Verificar vinculación exitosa
SELECT 
	p.IdPadre,
	p.Nombre + ' ' + p.Apellido AS Padre,
	u.Email AS EmailUsuario,
	COUNT(a.IdAlumno) AS CantidadAlumnos,
	CASE 
		WHEN p.UsuarioId IS NOT NULL AND u.Id IS NOT NULL THEN '✅ Vinculado correctamente'
		WHEN p.UsuarioId IS NOT NULL AND u.Id IS NULL THEN '⚠️ UsuarioId inválido'
		ELSE '❌ Sin vincular'
	END AS Estado
FROM genesis.Padres p
LEFT JOIN AspNetUsers u ON p.UsuarioId = u.Id
LEFT JOIN genesis.Alumnos a ON p.IdPadre = a.IdPadre
GROUP BY p.IdPadre, p.Nombre, p.Apellido, p.UsuarioId, u.Id, u.Email
ORDER BY p.IdPadre;

PRINT '';
PRINT '========================================';
PRINT 'ESTADÍSTICAS FINALES';
PRINT '========================================';

DECLARE @Total INT, @Vinculados INT, @SinVincular INT, @ConAlumnos INT;

SELECT @Total = COUNT(*) FROM genesis.Padres;
SELECT @Vinculados = COUNT(*) FROM genesis.Padres WHERE UsuarioId IS NOT NULL;
SELECT @SinVincular = COUNT(*) FROM genesis.Padres WHERE UsuarioId IS NULL;
SELECT @ConAlumnos = COUNT(DISTINCT p.IdPadre) 
FROM genesis.Padres p
INNER JOIN genesis.Alumnos a ON p.IdPadre = a.IdPadre;

PRINT 'Total de padres: ' + CAST(@Total AS VARCHAR(10));
PRINT 'Padres vinculados: ' + CAST(@Vinculados AS VARCHAR(10));
PRINT 'Padres sin vincular: ' + CAST(@SinVincular AS VARCHAR(10));
PRINT 'Padres con alumnos: ' + CAST(@ConAlumnos AS VARCHAR(10));

IF @Vinculados >= 6
BEGIN
	PRINT '';
	PRINT '✅ VINCULACIÓN COMPLETADA EXITOSAMENTE';
	PRINT '✅ Los padres pueden iniciar sesión y configurar direcciones';
END
ELSE
BEGIN
	PRINT '';
	PRINT '⚠️ ADVERTENCIA: No todos los padres principales fueron vinculados';
END

PRINT '';
PRINT '========================================';
PRINT 'CREDENCIALES PARA PRUEBAS';
PRINT '========================================';
PRINT 'Los padres pueden iniciar sesión con estas credenciales:';
PRINT '';
PRINT '1. Email: padre@test.com      → Juan Carlos García López (8 alumnos)';
PRINT '2. Email: padre1@gmail.com    → Carlos Martínez López (2 alumnos)';
PRINT '3. Email: padre2@gmail.com    → María García Hernández (1 alumno)';
PRINT '4. Email: padre3@gmail.com    → José Rodríguez Pérez (1 alumno)';
PRINT '5. Email: padre4@gmail.com    → Ana López González (1 alumno)';
PRINT '6. Email: padre5@gmail.com    → Luis Hernández Morales (1 alumno)';
PRINT '';
PRINT 'Nota: Las contraseñas son las que se usaron al crear los usuarios';
PRINT '';
PRINT '========================================';
PRINT 'PRÓXIMO PASO: PROBAR CONFIGURACIÓN INICIAL';
PRINT '========================================';
PRINT '1. Abrir navegador en modo incógnito';
PRINT '2. Ir a: http://localhost:[puerto]/Auth/Login';
PRINT '3. Login con: padre@test.com / [contraseña]';
PRINT '4. Sistema debe redirigir a: /Padre/ConfiguracionInicial';
PRINT '5. Ingresar dirección y geocodificar';
PRINT '6. Guardar y verificar redirección a dashboard';
PRINT '';

GO
