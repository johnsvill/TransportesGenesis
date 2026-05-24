-- ========================================
-- SCRIPT DE PRUEBA: Resetear Coordenadas y Crear Padre de Prueba
-- ========================================
-- Este script te permite probar el flujo de configuración inicial
-- ========================================

USE TransportesGenesis;
GO

PRINT '========================================';
PRINT 'OPCIÓN 1: RESETEAR COORDENADAS DE UN ALUMNO';
PRINT '========================================';
PRINT 'Esto permite probar el flujo con un padre existente';
PRINT '';

-- Ver estado actual de los alumnos de padre1@gmail.com (Carlos Martínez López)
PRINT 'Estado actual de alumnos de padre1@gmail.com:';
SELECT 
	a.IdAlumno,
	a.Nombre + ' ' + a.Apellido AS Alumno,
	a.Latitud,
	a.Longitud,
	CASE 
		WHEN a.Latitud IS NULL OR a.Latitud = 0 THEN '❌ Sin configurar'
		ELSE '✅ Configurado'
	END AS Estado
FROM genesis.Alumnos a
INNER JOIN genesis.Padres p ON a.IdPadre = p.IdPadre
INNER JOIN AspNetUsers u ON p.UsuarioId = u.Id
WHERE u.Email = 'padre1@gmail.com';

PRINT '';
PRINT 'Para resetear las coordenadas de este padre, descomenta y ejecuta:';
PRINT '';
PRINT '/*';
PRINT 'UPDATE genesis.Alumnos';
PRINT 'SET Latitud = NULL, Longitud = NULL';
PRINT 'WHERE IdPadre = 100; -- padre1@gmail.com (Carlos Martínez López)';
PRINT '';
PRINT '-- Verificar';
PRINT 'SELECT IdAlumno, Nombre, Latitud, Longitud FROM genesis.Alumnos WHERE IdPadre = 100;';
PRINT '*/';
PRINT '';

PRINT '========================================';
PRINT 'OPCIÓN 2: CREAR NUEVO PADRE DE PRUEBA';
PRINT '========================================';
PRINT 'Si quieres crear un nuevo padre desde cero para pruebas';
PRINT '';

-- Ver padres sin vincular que tienen alumnos
PRINT 'Padres disponibles SIN vincular (con alumnos):';
SELECT 
	p.IdPadre,
	p.Nombre + ' ' + p.Apellido AS Padre,
	COUNT(a.IdAlumno) AS CantidadAlumnos,
	CASE 
		WHEN p.UsuarioId IS NULL THEN '❌ Sin usuario'
		ELSE '✅ Tiene usuario'
	END AS EstadoUsuario
FROM genesis.Padres p
LEFT JOIN genesis.Alumnos a ON p.IdPadre = a.IdPadre
WHERE p.UsuarioId IS NULL
GROUP BY p.IdPadre, p.Nombre, p.Apellido, p.UsuarioId
HAVING COUNT(a.IdAlumno) > 0;

PRINT '';
PRINT '========================================';
PRINT 'RECOMENDACIÓN: USAR padre1@gmail.com PARA PRUEBAS';
PRINT '========================================';
PRINT '';
PRINT 'PASOS PARA PROBAR EL FLUJO COMPLETO:';
PRINT '';
PRINT '1. RESETEAR COORDENADAS (ejecutar esto primero):';
PRINT '';
PRINT '   UPDATE genesis.Alumnos';
PRINT '   SET Latitud = NULL, Longitud = NULL';
PRINT '   WHERE IdPadre = 100;';
PRINT '';
PRINT '2. VERIFICAR que se resetearon:';
PRINT '';
PRINT '   SELECT IdAlumno, Nombre, Latitud, Longitud';
PRINT '   FROM genesis.Alumnos';
PRINT '   WHERE IdPadre = 100;';
PRINT '';
PRINT '3. CERRAR SESIÓN en el navegador (o usar modo incógnito)';
PRINT '';
PRINT '4. LOGIN con:';
PRINT '   Email: padre1@gmail.com';
PRINT '   Password: [la contraseña que configuraste]';
PRINT '';
PRINT '5. DEBERÍA REDIRIGIR A: /Padre/ConfiguracionInicial';
PRINT '';
PRINT '6. CONFIGURAR DIRECCIÓN:';
PRINT '   - Seleccionar alumno: Diego Martínez o Sofía Martínez';
PRINT '   - Ingresar dirección: "5ta Avenida 12-34 Zona 10, Guatemala"';
PRINT '   - Buscar en mapa';
PRINT '   - Guardar';
PRINT '';
PRINT '7. VERIFICAR que se guardaron las coordenadas:';
PRINT '';
PRINT '   SELECT IdAlumno, Nombre, Latitud, Longitud';
PRINT '   FROM genesis.Alumnos';
PRINT '   WHERE IdPadre = 100;';
PRINT '';

GO

PRINT '';
PRINT '========================================';
PRINT 'SCRIPT DE RESET RÁPIDO (COPIAR Y EJECUTAR)';
PRINT '========================================';
PRINT '';
PRINT '-- Resetear coordenadas de padre1@gmail.com';
PRINT 'UPDATE genesis.Alumnos SET Latitud = NULL, Longitud = NULL WHERE IdPadre = 100;';
PRINT '';
PRINT '-- Verificar';
PRINT 'SELECT a.IdAlumno, a.Nombre, a.Latitud, a.Longitud,';
PRINT '       CASE WHEN a.Latitud IS NULL THEN ''Sin configurar'' ELSE ''Configurado'' END AS Estado';
PRINT 'FROM genesis.Alumnos a WHERE IdPadre = 100;';
PRINT '';
PRINT '-- Ahora puedes hacer login con padre1@gmail.com';
PRINT '';
