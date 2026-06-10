-- =============================================
-- SCRIPT: VERIFICAR RELACIÓN PADRE1 → ALUMNO → BUS → PILOTO1
-- Propósito: Preparar demo de alertas en tiempo real
-- =============================================

USE [TransportesGenesis]
GO

PRINT '========================================='
PRINT '🎯 VERIFICACIÓN PARA DEMO'
PRINT '========================================='
PRINT ''

-- =============================================
-- 1. BUSCAR PADRE1
-- =============================================
PRINT '👨‍👩‍👧 PASO 1: Buscar Padre de Familia disponible'
PRINT '----------------------------------------'

SELECT TOP 1
	p.IdPadre,
	p.Nombre + ' ' + p.Apellido AS 'NombrePadre',
	u.Email AS 'EmailLogin',
	u.EmailConfirmed AS 'Activo',
	COUNT(a.IdAlumno) AS 'CantidadHijos'
FROM genesis.Padres p
LEFT JOIN AspNetUsers u ON p.UsuarioId = u.Id
LEFT JOIN genesis.Alumnos a ON a.IdPadre = p.IdPadre
WHERE u.Email IS NOT NULL
  AND u.EmailConfirmed = 1
GROUP BY p.IdPadre, p.Nombre, p.Apellido, u.Email, u.EmailConfirmed
ORDER BY p.IdPadre

DECLARE @IdPadre INT = (
	SELECT TOP 1 p.IdPadre
	FROM genesis.Padres p
	LEFT JOIN AspNetUsers u ON p.UsuarioId = u.Id
	WHERE u.Email IS NOT NULL AND u.EmailConfirmed = 1
	ORDER BY p.IdPadre
)

IF @IdPadre IS NULL
BEGIN
	PRINT '❌ ERROR: No se encontró ningún padre activo'
	RETURN
END

PRINT ''
PRINT '✅ Padre encontrado: ID = ' + CAST(@IdPadre AS NVARCHAR(10))
PRINT ''

-- =============================================
-- 2. BUSCAR HIJOS DEL PADRE1
-- =============================================
PRINT '👶 PASO 2: Buscar hijos del padre seleccionado'
PRINT '----------------------------------------'

SELECT 
	a.IdAlumno,
	a.Nombre + ' ' + a.Apellido AS 'NombreAlumno',
	a.IdBusAsignado AS 'BusAsignado',
	CASE 
		WHEN a.IdBusAsignado IS NOT NULL THEN 'Tiene bus ✅'
		ELSE 'Sin bus ❌'
	END AS 'EstadoBus',
	a.Latitud,
	a.Longitud,
	CASE 
		WHEN a.Latitud IS NOT NULL AND a.Longitud IS NOT NULL THEN 'Con ubicación ✅'
		ELSE 'Sin ubicación ❌'
	END AS 'EstadoUbicacion'
FROM genesis.Alumnos a
WHERE a.IdPadre = @IdPadre

DECLARE @CantidadHijos INT = (SELECT COUNT(*) FROM genesis.Alumnos WHERE IdPadre = @IdPadre)

IF @CantidadHijos = 0
BEGIN
	PRINT '❌ ERROR: El padre no tiene hijos asignados'
	RETURN
END

PRINT ''
PRINT '✅ Hijos encontrados: ' + CAST(@CantidadHijos AS NVARCHAR(10))
PRINT ''

-- =============================================
-- 3. VERIFICAR BUS ASIGNADO
-- =============================================
PRINT '🚌 PASO 3: Verificar bus asignado a los hijos'
PRINT '----------------------------------------'

SELECT DISTINCT
	b.IdBus,
	b.Placa,
	b.Estado AS 'EstadoBus',
	pi.IdPiloto,
	pi.Nombre + ' ' + pi.Apellido AS 'NombrePiloto',
	up.Email AS 'EmailPiloto',
	COUNT(a.IdAlumno) AS 'AlumnosEnBus'
FROM genesis.Alumnos a
INNER JOIN genesis.Bus b ON a.IdBusAsignado = b.IdBus
LEFT JOIN genesis.Piloto pi ON b.IdBus = pi.IdBusAsignado
LEFT JOIN AspNetUsers up ON pi.UsuarioId = up.Id
WHERE a.IdPadre = @IdPadre
  AND a.IdBusAsignado IS NOT NULL
GROUP BY b.IdBus, b.Placa, b.Estado, pi.IdPiloto, pi.Nombre, pi.Apellido, up.Email

DECLARE @IdBus INT = (
	SELECT TOP 1 a.IdBusAsignado
	FROM genesis.Alumnos a
	WHERE a.IdPadre = @IdPadre AND a.IdBusAsignado IS NOT NULL
)

IF @IdBus IS NULL
BEGIN
	PRINT '⚠️ ADVERTENCIA: Los hijos del padre no tienen bus asignado'
	PRINT ''
	PRINT '💡 SOLUCIÓN: Ejecuta el script "ASIGNAR_Bus_A_Alumno.sql"'
	RETURN
END

PRINT ''
PRINT '✅ Bus encontrado: ID = ' + CAST(@IdBus AS NVARCHAR(10))
PRINT ''

-- =============================================
-- 4. VERIFICAR PILOTO DEL BUS
-- =============================================
PRINT '🚗 PASO 4: Verificar piloto asignado al bus'
PRINT '----------------------------------------'

DECLARE @IdPiloto INT = (
	SELECT pi.IdPiloto
	FROM genesis.Piloto pi
	WHERE pi.IdBusAsignado = @IdBus
)

IF @IdPiloto IS NULL
BEGIN
	PRINT '⚠️ ADVERTENCIA: El bus no tiene piloto asignado'
	PRINT ''
	PRINT '💡 SOLUCIÓN: Asignar un piloto al bus ' + CAST(@IdBus AS NVARCHAR(10))
	RETURN
END

SELECT 
	pi.IdPiloto,
	pi.Nombre + ' ' + pi.Apellido AS 'NombrePiloto',
	u.Email AS 'EmailLogin',
	u.EmailConfirmed AS 'Activo',
	pi.IdBusAsignado AS 'BusAsignado'
FROM genesis.Piloto pi
LEFT JOIN AspNetUsers u ON pi.UsuarioId = u.Id
WHERE pi.IdPiloto = @IdPiloto

PRINT ''
PRINT '✅ Piloto encontrado: ID = ' + CAST(@IdPiloto AS NVARCHAR(10))
PRINT ''

-- =============================================
-- 5. VERIFICAR RUTAS ACTIVAS
-- =============================================
PRINT '🗺️ PASO 5: Verificar rutas activas del bus'
PRINT '----------------------------------------'

SELECT 
	r.IdRuta,
	r.NombreRuta,
	r.TipoRuta,
	r.IdBus,
	r.Activo,
	COUNT(pl.IdParadasLink) AS 'CantidadParadas'
FROM genesis.Ruta r
LEFT JOIN genesis.ParadasLink pl ON r.IdRuta = pl.IdRuta
WHERE r.IdBus = @IdBus
GROUP BY r.IdRuta, r.NombreRuta, r.TipoRuta, r.IdBus, r.Activo

DECLARE @CantidadRutas INT = (
	SELECT COUNT(*) FROM genesis.Ruta WHERE IdBus = @IdBus AND Activo = 1
)

IF @CantidadRutas = 0
BEGIN
	PRINT '⚠️ ADVERTENCIA: El bus no tiene rutas activas'
	PRINT ''
	PRINT '💡 La simulación funcionará igual, pero no habrá paradas reales'
END
ELSE
BEGIN
	PRINT ''
	PRINT '✅ Rutas activas encontradas: ' + CAST(@CantidadRutas AS NVARCHAR(10))
END

PRINT ''

-- =============================================
-- 6. RESUMEN FINAL PARA LA DEMO
-- =============================================
PRINT ''
PRINT '========================================='
PRINT '📋 RESUMEN PARA TU DEMO'
PRINT '========================================='
PRINT ''

DECLARE @EmailPadre NVARCHAR(256) = (
	SELECT u.Email
	FROM genesis.Padres p
	INNER JOIN AspNetUsers u ON p.UsuarioId = u.Id
	WHERE p.IdPadre = @IdPadre
)

DECLARE @EmailPiloto NVARCHAR(256) = (
	SELECT u.Email
	FROM genesis.Piloto pi
	INNER JOIN AspNetUsers u ON pi.UsuarioId = u.Id
	WHERE pi.IdPiloto = @IdPiloto
)

DECLARE @NombrePadre NVARCHAR(100) = (
	SELECT p.Nombre + ' ' + p.Apellido
	FROM genesis.Padres p
	WHERE p.IdPadre = @IdPadre
)

DECLARE @NombrePiloto NVARCHAR(100) = (
	SELECT pi.Nombre + ' ' + pi.Apellido
	FROM genesis.Piloto pi
	WHERE pi.IdPiloto = @IdPiloto
)

DECLARE @PlacaBus NVARCHAR(50) = (
	SELECT b.Placa
	FROM genesis.Bus b
	WHERE b.IdBus = @IdBus
)

PRINT '🎭 CONFIGURACIÓN DE LA DEMO:'
PRINT '----------------------------------------'
PRINT ''
PRINT '👨‍👩‍👧 PADRE (Pestaña 1 - Normal)'
PRINT '   Email: ' + ISNULL(@EmailPadre, 'NO DISPONIBLE')
PRINT '   Contraseña: Admin123!'
PRINT '   Nombre: ' + ISNULL(@NombrePadre, 'NO DISPONIBLE')
PRINT ''
PRINT '🚗 PILOTO (Pestaña 2 - Incógnito)'
PRINT '   Email: ' + ISNULL(@EmailPiloto, 'NO DISPONIBLE')
PRINT '   Contraseña: Admin123!'
PRINT '   Nombre: ' + ISNULL(@NombrePiloto, 'NO DISPONIBLE')
PRINT ''
PRINT '🚌 BUS COMPARTIDO:'
PRINT '   ID: ' + CAST(@IdBus AS NVARCHAR(10))
PRINT '   Placa: ' + ISNULL(@PlacaBus, 'NO DISPONIBLE')
PRINT ''
PRINT '----------------------------------------'
PRINT ''

-- =============================================
-- 7. INSTRUCCIONES PARA LA DEMO
-- =============================================
PRINT '📌 PASOS PARA LA DEMO:'
PRINT '----------------------------------------'
PRINT '1. Abre Chrome en modo NORMAL'
PRINT '   → Login: ' + ISNULL(@EmailPadre, 'NO DISPONIBLE')
PRINT '   → Ve a: /Padres/DashboardRutaBusAsignado'
PRINT ''
PRINT '2. Abre Chrome en modo INCÓGNITO (Ctrl+Shift+N)'
PRINT '   → Login: ' + ISNULL(@EmailPiloto, 'NO DISPONIBLE')
PRINT '   → Ve a: /Piloto/MiRuta'
PRINT ''
PRINT '3. En la ventana del PILOTO:'
PRINT '   → Clic en "🚌 Simular Ruta"'
PRINT '   → Espera que empiece la simulación'
PRINT ''
PRINT '4. En la ventana del PADRE:'
PRINT '   → Observa el mapa en tiempo real'
PRINT '   → Deberías ver notificaciones cuando el bus llegue a paradas'
PRINT ''
PRINT '✅ Si todo está configurado correctamente, verás:'
PRINT '   • El mapa del padre se actualiza automáticamente'
PRINT '   • Alertas en tiempo real cuando el piloto marque paradas'
PRINT '   • El bus moviéndose en ambos mapas simultáneamente'
PRINT ''
PRINT '========================================='
PRINT '🎉 LISTO PARA LA DEMO'
PRINT '========================================='
GO
