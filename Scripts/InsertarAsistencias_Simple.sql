-- Script SIMPLIFICADO para insertar datos de prueba de Asistencias
-- FASE 4: Solo crea asistencias, NO modifica tablas existentes

USE TransportesGenesis;
GO

PRINT '=== Insertando asistencias de prueba para FASE 4 ===';

-- Verificar si hay alumnos en la base de datos
DECLARE @CantidadAlumnos INT;
SELECT @CantidadAlumnos = COUNT(*) FROM genesis.Alumnos WHERE Activo = 1;

IF @CantidadAlumnos = 0
BEGIN
    PRINT 'ERROR: No hay alumnos en la base de datos.';
    PRINT 'Por ahora, puedes probar la interfaz con IdAlumno = 1 (simulado)';
END
ELSE
BEGIN
    PRINT 'Alumnos encontrados: ' + CAST(@CantidadAlumnos AS VARCHAR(10));

    -- Tomar el primer alumno activo
    DECLARE @IdAlumno INT;
    SELECT TOP 1 @IdAlumno = IdAlumno FROM genesis.Alumnos WHERE Activo = 1;

    PRINT 'Usando IdAlumno: ' + CAST(@IdAlumno AS VARCHAR(10));

    -- Insertar asistencia para HOY (pendiente de confirmacion)
    DECLARE @Hoy DATE = CAST(GETDATE() AS DATE);

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

    -- Insertar historial (ultimos 7 dias)
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
                CASE WHEN @Dia % 2 = 0 THEN 1 ELSE 0 END,
                CASE WHEN @Dia % 3 = 0 THEN 0 ELSE 1 END,
                DATEADD(HOUR, -1, @FechaHistorial),
                1, GETDATE()
            );
        END

        SET @Dia = @Dia + 1;
    END

    PRINT 'Historial de asistencias insertado (ultimos 7 dias)';

    -- Mostrar resumen
    PRINT '';
    PRINT '=== RESUMEN ===';

    SELECT 
        asi.Fecha,
        asi.AsisteMañana,
        asi.AsisteTarde,
        CASE 
            WHEN asi.FechaConfirmacion IS NULL THEN 'Pendiente'
            ELSE 'Confirmado'
        END AS Estado,
        asi.FechaConfirmacion
    FROM genesis.AsistenciaAlumno asi
    WHERE asi.IdAlumno = @IdAlumno
    ORDER BY asi.Fecha DESC;
END
GO

PRINT '';
PRINT 'Listo para probar FASE 4!';
PRINT 'Accede a: /Padres/ConfirmarAsistencia';
PRINT 'NOTA: En el codigo HTML, cambia el value del select de alumno al IdAlumno que tengas';
GO
