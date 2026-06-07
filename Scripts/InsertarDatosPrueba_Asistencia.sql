-- Script para insertar datos de prueba de Alumnos y Asistencias
-- FASE 4: Confirmacion de asistencia de padres

USE TransportesGenesis;
GO

PRINT '=== Insertando datos de prueba para Fase 4 ===';

-- Insertar un padre de prueba si no existe
IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE Nombre = 'Juan Carlos' AND Apellido = 'Garcia Lopez')
BEGIN
    INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
    VALUES ('Juan Carlos', 'Garcia Lopez', 1, GETDATE());

    PRINT 'Padre de prueba insertado';
END
ELSE
BEGIN
    PRINT 'Padre de prueba ya existe';
END
GO

-- Insertar un alumno de prueba vinculado al bus BUS-001
DECLARE @IdPadre INT;
DECLARE @IdBusPrueba INT;

SELECT @IdPadre = IdPadre FROM genesis.Padres WHERE Nombre = 'Juan Carlos' AND Apellido = 'Garcia Lopez';
SELECT @IdBusPrueba = IdBus FROM genesis.Buses WHERE Placa = 'BUS-001';

IF @IdBusPrueba IS NULL
BEGIN
    PRINT 'ERROR: No se encontro el bus BUS-001. Ejecuta primero InsertarDatosPrueba_Geolocalizacion.sql';
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = 'Maria Fernanda' AND Apellido = 'Garcia Perez')
    BEGIN
        INSERT INTO genesis.Alumnos (
            IdPadre, Nombre, Apellido, 
            IdBusAsignado, Latitud, Longitud, Direccion,
            Activo, FechaRegistro
        )
        VALUES (
            @IdPadre, 'Maria Fernanda', 'Garcia Perez',
            @IdBusPrueba, 14.5920, -90.5200, 'Zona 10, Ciudad de Guatemala',
            1, GETDATE()
        );

        PRINT 'Alumno de prueba insertado y vinculado al bus BUS-001';
    END
    ELSE
    BEGIN
        -- Actualizar el alumno existente con bus asignado
        UPDATE genesis.Alumnos
        SET IdBusAsignado = @IdBusPrueba,
            Latitud = 14.5920,
            Longitud = -90.5200,
            Direccion = 'Zona 10, Ciudad de Guatemala'
        WHERE Nombre = 'Maria Fernanda' AND Apellido = 'Garcia Perez';

        PRINT 'Alumno de prueba actualizado con bus asignado';
    END
END
GO

-- Insertar asistencia para HOY (para probar confirmacion)
DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);
DECLARE @IdAlumno INT;

SELECT @IdAlumno = IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Maria Fernanda' AND Apellido = 'Garcia Perez';

IF @IdAlumno IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM genesis.AsistenciaAlumno WHERE IdAlumno = @IdAlumno AND Fecha = @Hoy)
    BEGIN
        INSERT INTO genesis.AsistenciaAlumno (
            IdAlumno, Fecha, AsisteMañana, AsisteTarde, 
            FechaConfirmacion, Activo, FechaRegistro
        )
        VALUES (
            @IdAlumno, @Hoy, 1, 1, 
            NULL, 1, GETDATE()
        );

        PRINT 'Asistencia de HOY insertada (pendiente de confirmacion)';
    END
    ELSE
    BEGIN
        PRINT 'Asistencia de HOY ya existe';
    END

    -- Insertar historial de asistencias (ultimos 7 dias)
    DECLARE @Dia INT = 1;
    DECLARE @FechaHistorial DATE;

    WHILE @Dia <= 7
    BEGIN
        SET @FechaHistorial = CAST(DATEADD(DAY, -@Dia, GETDATE()) AS DATE);

        IF NOT EXISTS (SELECT 1 FROM genesis.AsistenciaAlumno WHERE IdAlumno = @IdAlumno AND Fecha = @FechaHistorial)
        BEGIN
            INSERT INTO genesis.AsistenciaAlumno (
                IdAlumno, Fecha, AsisteMañana, AsisteTarde, 
                FechaConfirmacion, Activo, FechaRegistro
            )
            VALUES (
                @IdAlumno, @FechaHistorial, 
                CASE WHEN @Dia % 2 = 0 THEN 1 ELSE 0 END, -- Alternar asistencia manana
                CASE WHEN @Dia % 3 = 0 THEN 0 ELSE 1 END, -- Alternar asistencia tarde
                DATEADD(MINUTE, -30, @FechaHistorial), -- Confirmado 30 min antes
                1, GETDATE()
            );
        END

        SET @Dia = @Dia + 1;
    END

    PRINT 'Historial de asistencias insertado (ultimos 7 dias)';
END
GO

-- Verificar resultados
PRINT '';
PRINT '=== RESUMEN DE DATOS INSERTADOS ===';

DECLARE @IdAlumnoVerif INT;
SELECT @IdAlumnoVerif = IdAlumno FROM genesis.Alumnos WHERE Nombre = 'Maria Fernanda' AND Apellido = 'Garcia Perez';

SELECT 
    al.IdAlumno,
    al.Nombre + ' ' + al.Apellido AS NombreCompleto,
    b.Placa AS BusAsignado,
    al.Direccion,
    COUNT(asi.IdAsistencia) AS TotalAsistencias
FROM genesis.Alumnos al
LEFT JOIN genesis.Buses b ON al.IdBusAsignado = b.IdBus
LEFT JOIN genesis.AsistenciaAlumno asi ON al.IdAlumno = asi.IdAlumno
WHERE al.IdAlumno = @IdAlumnoVerif
GROUP BY al.IdAlumno, al.Nombre, al.Apellido, b.Placa, al.Direccion;

SELECT 
    asi.Fecha,
    asi.AsisteMañana,
    asi.AsisteTarde,
    CASE 
        WHEN asi.FechaConfirmacion IS NULL THEN 'Pendiente'
        ELSE 'Confirmado'
    END AS EstadoConfirmacion,
    asi.FechaConfirmacion
FROM genesis.AsistenciaAlumno asi
WHERE asi.IdAlumno = @IdAlumnoVerif
ORDER BY asi.Fecha DESC;

PRINT '';
PRINT 'Datos de prueba listos para FASE 4';
PRINT 'Accede a: /Padres/ConfirmarAsistencia';
GO
