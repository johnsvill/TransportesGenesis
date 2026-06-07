-- ========================================
-- Script de Verificación y Setup para Traslados
-- ========================================

USE TransportesGenesis;
GO

-- ========================================
-- 1. VERIFICAR QUE EXISTA LA TABLA
-- ========================================
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES 
               WHERE TABLE_SCHEMA = 'genesis' 
               AND TABLE_NAME = 'SolicitudTraslado')
BEGIN
    PRINT '❌ Tabla genesis.SolicitudTraslado NO EXISTE';
    PRINT 'Creando tabla...';

    CREATE TABLE genesis.SolicitudTraslado (
        IdSolicitud INT IDENTITY(1,1) PRIMARY KEY,
        IdAlumno INT NOT NULL,
        IdBusOrigen INT NOT NULL,
        IdBusDestino INT NULL,
        FechaTraslado DATE NOT NULL,
        Turno NVARCHAR(20) NOT NULL CHECK (Turno IN ('Mañana', 'Tarde', 'Ambos')),
        Estado NVARCHAR(20) NOT NULL DEFAULT 'Pendiente' CHECK (Estado IN ('Pendiente', 'Aprobado', 'Rechazado')),
        Motivo NVARCHAR(500) NULL,
        AprobadoPor NVARCHAR(100) NULL,
        FechaRespuesta DATETIME NULL,
        ComentarioAdmin NVARCHAR(500) NULL,
        UsuarioCreacion NVARCHAR(100) NOT NULL DEFAULT 'System',
        FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
        UsuarioModificacion NVARCHAR(100) NULL,
        FechaModificacion DATETIME NULL,

        CONSTRAINT FK_SolicitudTraslado_Alumno 
            FOREIGN KEY (IdAlumno) REFERENCES genesis.Alumnos(IdAlumno),
        CONSTRAINT FK_SolicitudTraslado_BusOrigen 
            FOREIGN KEY (IdBusOrigen) REFERENCES genesis.Bus(IdBus),
        CONSTRAINT FK_SolicitudTraslado_BusDestino 
            FOREIGN KEY (IdBusDestino) REFERENCES genesis.Bus(IdBus)
    );

    PRINT '✅ Tabla creada exitosamente';
END
ELSE
BEGIN
    PRINT '✅ Tabla genesis.SolicitudTraslado existe';
END
GO

-- ========================================
-- 2. VERIFICAR ALUMNOS DE PRUEBA
-- ========================================
PRINT '';
PRINT '=== VERIFICANDO ALUMNOS ===';

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE IdAlumno = 1)
BEGIN
    PRINT '❌ No existe alumno con IdAlumno = 1';
    PRINT 'Creando alumno de prueba...';

    SET IDENTITY_INSERT genesis.Alumnos ON;

    INSERT INTO genesis.Alumnos (
        IdAlumno, Nombres, Apellidos, FechaNacimiento, 
        IdBusAsignado, Direccion, Latitud, Longitud,
        UsuarioCreacion, FechaCreacion
    )
    VALUES (
        1, 'Juan Carlos', 'Pérez García', '2015-05-15',
        1, 'Calle Principal 123, San Juan de Lurigancho', -12.0464, -77.0428,
        'System', GETDATE()
    );

    SET IDENTITY_INSERT genesis.Alumnos OFF;

    PRINT '✅ Alumno creado: Juan Carlos Pérez García (ID: 1)';
END
ELSE
BEGIN
    DECLARE @NombreAlumno NVARCHAR(200);
    SELECT @NombreAlumno = Nombres + ' ' + Apellidos 
    FROM genesis.Alumnos 
    WHERE IdAlumno = 1;

    PRINT '✅ Alumno ID=1 existe: ' + @NombreAlumno;
END
GO

-- ========================================
-- 3. VERIFICAR BUSES
-- ========================================
PRINT '';
PRINT '=== VERIFICANDO BUSES ===';

DECLARE @CantidadBuses INT;
SELECT @CantidadBuses = COUNT(*) FROM genesis.Bus;

PRINT 'Buses en la BD: ' + CAST(@CantidadBuses AS NVARCHAR(10));

IF @CantidadBuses = 0
BEGIN
    PRINT '❌ No hay buses en la BD';
    PRINT 'Creando buses de prueba...';

    INSERT INTO genesis.Bus (Placa, Modelo, Capacidad, Estado, UsuarioCreacion, FechaCreacion)
    VALUES 
        ('BUS-001', 'Mercedes Benz OF-1721', 45, 'Activo', 'System', GETDATE()),
        ('BUS-002', 'Volvo B7R', 48, 'Activo', 'System', GETDATE()),
        ('BUS-003', 'Scania K310', 42, 'Activo', 'System', GETDATE()),
        ('BUS-004', 'Mercedes Benz OF-1722', 45, 'Inactivo', 'System', GETDATE());

    PRINT '✅ 4 buses creados';
END
ELSE
BEGIN
    PRINT '✅ Hay ' + CAST(@CantidadBuses AS NVARCHAR(10)) + ' buses en la BD';

    SELECT IdBus, Placa, Modelo, Estado 
    FROM genesis.Bus 
    ORDER BY IdBus;
END
GO

-- ========================================
-- 4. VERIFICAR SOLICITUDES EXISTENTES
-- ========================================
PRINT '';
PRINT '=== VERIFICANDO SOLICITUDES EXISTENTES ===';

DECLARE @CantidadSolicitudes INT;
SELECT @CantidadSolicitudes = COUNT(*) FROM genesis.SolicitudTraslado;

PRINT 'Solicitudes en la BD: ' + CAST(@CantidadSolicitudes AS NVARCHAR(10));

IF @CantidadSolicitudes > 0
BEGIN
    PRINT '';
    PRINT 'Solicitudes actuales:';

    SELECT 
        s.IdSolicitud,
        a.Nombres + ' ' + a.Apellidos AS Alumno,
        s.FechaTraslado,
        s.Turno,
        bOrigen.Placa AS BusOrigen,
        bDestino.Placa AS BusDestino,
        s.Estado,
        s.FechaRegistro
    FROM genesis.SolicitudTraslado s
    INNER JOIN genesis.Alumnos a ON s.IdAlumno = a.IdAlumno
    INNER JOIN genesis.Bus bOrigen ON s.IdBusOrigen = bOrigen.IdBus
    LEFT JOIN genesis.Bus bDestino ON s.IdBusDestino = bDestino.IdBus
    ORDER BY s.FechaRegistro DESC;
END
ELSE
BEGIN
    PRINT 'No hay solicitudes registradas aún';
END
GO

-- ========================================
-- 5. TEST RÁPIDO DE INSERCIÓN
-- ========================================
PRINT '';
PRINT '=== TEST DE INSERCIÓN ===';
PRINT 'Intentando crear solicitud de prueba...';

BEGIN TRY
    INSERT INTO genesis.SolicitudTraslado (
        IdAlumno, IdBusOrigen, IdBusDestino, 
        FechaTraslado, Turno, Estado, Motivo,
        UsuarioCreacion, FechaCreacion
    )
    VALUES (
        1,                              -- IdAlumno (debe existir)
        1,                              -- IdBusOrigen (debe existir)
        3,                              -- IdBusDestino (debe existir)
        DATEADD(DAY, 5, GETDATE()),    -- Fecha: dentro de 5 días
        'Ambos',                        -- Turno
        'Pendiente',                    -- Estado
        'Test desde script SQL',        -- Motivo
        'System',                       -- Usuario
        GETDATE()                       -- Fecha creación
    );

    DECLARE @NuevoID INT = SCOPE_IDENTITY();
    PRINT '✅ Solicitud de prueba creada exitosamente con ID: ' + CAST(@NuevoID AS NVARCHAR(10));

    -- Eliminar la solicitud de prueba
    DELETE FROM genesis.SolicitudTraslado WHERE IdSolicitud = @NuevoID;
    PRINT 'Solicitud de prueba eliminada (era solo para testing)';

END TRY
BEGIN CATCH
    PRINT '❌ ERROR al insertar solicitud de prueba:';
    PRINT ERROR_MESSAGE();

    IF ERROR_NUMBER() = 547 -- Foreign key violation
    BEGIN
        PRINT '';
        PRINT '💡 CAUSA: Problema con Foreign Keys';
        PRINT 'Verifica que:';
        PRINT '  - Existe Alumno con IdAlumno = 1';
        PRINT '  - Existe Bus con IdBus = 1';
        PRINT '  - Existe Bus con IdBus = 3';
    END
END CATCH
GO

-- ========================================
-- 6. RESUMEN FINAL
-- ========================================
PRINT '';
PRINT '======================================';
PRINT '           RESUMEN FINAL             ';
PRINT '======================================';

DECLARE @TablaExiste BIT = CASE WHEN EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = 'genesis' AND TABLE_NAME = 'SolicitudTraslado'
) THEN 1 ELSE 0 END;

DECLARE @AlumnoExiste BIT = CASE WHEN EXISTS (
    SELECT 1 FROM genesis.Alumnos WHERE IdAlumno = 1
) THEN 1 ELSE 0 END;

DECLARE @BusesExisten BIT = CASE WHEN EXISTS (
    SELECT 1 FROM genesis.Bus
) THEN 1 ELSE 0 END;

PRINT 'Tabla SolicitudTraslado: ' + CASE WHEN @TablaExiste = 1 THEN '✅ Existe' ELSE '❌ No existe' END;
PRINT 'Alumno ID=1:             ' + CASE WHEN @AlumnoExiste = 1 THEN '✅ Existe' ELSE '❌ No existe' END;
PRINT 'Buses en BD:             ' + CASE WHEN @BusesExisten = 1 THEN '✅ Existen' ELSE '❌ No existen' END;

IF @TablaExiste = 1 AND @AlumnoExiste = 1 AND @BusesExisten = 1
BEGIN
    PRINT '';
    PRINT '🎉 TODO LISTO PARA CREAR TRASLADOS 🎉';
    PRINT '';
    PRINT 'Puedes:';
    PRINT '  1. Ejecutar el proyecto (F5)';
    PRINT '  2. Ir a /Padres/ConfirmarAsistencia';
    PRINT '  3. Crear una solicitud de traslado';
    PRINT '  4. Ver en /Padres/Traslados';
END
ELSE
BEGIN
    PRINT '';
    PRINT '⚠️  FALTAN COMPONENTES';
    PRINT 'Ejecuta este script de nuevo para crear lo faltante';
END

PRINT '';
PRINT '======================================';
GO
