-- Script para insertar asistencias variadas en el mes actual
-- Para probar el calendario con diferentes estados

USE TransportesGenesis;
GO

PRINT '=== Insertando asistencias variadas para el mes actual ===';

DECLARE @IdAlumno INT = 1;
DECLARE @FechaInicio DATE = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1);
DECLARE @FechaFin DATE = EOMONTH(GETDATE());
DECLARE @FechaActual DATE = @FechaInicio;

-- Limpiar asistencias del mes actual para este alumno
DELETE FROM genesis.AsistenciaAlumno 
WHERE IdAlumno = @IdAlumno 
  AND Fecha >= @FechaInicio 
  AND Fecha <= @FechaFin;

PRINT 'Asistencias anteriores eliminadas';

-- Insertar asistencias variadas (solo lunes a viernes)
WHILE @FechaActual <= @FechaFin
BEGIN
    DECLARE @DiaSemana INT = DATEPART(WEEKDAY, @FechaActual); -- 1=Domingo, 7=Sábado

    -- Solo lunes (2) a viernes (6)
    IF @DiaSemana >= 2 AND @DiaSemana <= 6
    BEGIN
        DECLARE @AsisteMañana BIT;
        DECLARE @AsisteTarde BIT;
        DECLARE @FechaConfirmacion DATETIME;
        DECLARE @Dia INT = DAY(@FechaActual);

        -- Variar los estados para tener ejemplos visuales
        IF @Dia % 5 = 0
        BEGIN
            -- Cada 5 días: No asiste
            SET @AsisteMañana = 0;
            SET @AsisteTarde = 0;
            SET @FechaConfirmacion = DATEADD(HOUR, -12, @FechaActual);
        END
        ELSE IF @Dia % 3 = 0
        BEGIN
            -- Cada 3 días: Solo asiste en la mañana
            SET @AsisteMañana = 1;
            SET @AsisteTarde = 0;
            SET @FechaConfirmacion = DATEADD(HOUR, -8, @FechaActual);
        END
        ELSE IF @Dia % 7 = 0
        BEGIN
            -- Cada 7 días: Solo asiste en la tarde
            SET @AsisteMañana = 0;
            SET @AsisteTarde = 1;
            SET @FechaConfirmacion = DATEADD(HOUR, -6, @FechaActual);
        END
        ELSE IF @FechaActual > CAST(GETDATE() AS DATE)
        BEGIN
            -- Días futuros: Por default ambas rutas sin confirmar aún
            SET @AsisteMañana = 1;
            SET @AsisteTarde = 1;
            SET @FechaConfirmacion = NULL;
        END
        ELSE IF @FechaActual = CAST(GETDATE() AS DATE)
        BEGIN
            -- Hoy: Pendiente de confirmar
            SET @AsisteMañana = 1;
            SET @AsisteTarde = 1;
            SET @FechaConfirmacion = NULL;
        END
        ELSE
        BEGIN
            -- Días pasados normales: Confirmado ambas rutas
            SET @AsisteMañana = 1;
            SET @AsisteTarde = 1;
            SET @FechaConfirmacion = DATEADD(HOUR, -12, @FechaActual);
        END

        INSERT INTO genesis.AsistenciaAlumno (
            IdAlumno, Fecha, AsisteMañana, AsisteTarde, 
            FechaConfirmacion, Activo, FechaRegistro
        )
        VALUES (
            @IdAlumno, @FechaActual, @AsisteMañana, @AsisteTarde,
            @FechaConfirmacion, 1, GETDATE()
        );
    END

    SET @FechaActual = DATEADD(DAY, 1, @FechaActual);
END

PRINT 'Asistencias del mes insertadas con estados variados';
GO

-- Verificar resultados
PRINT '';
PRINT '=== RESUMEN DE ASISTENCIAS DEL MES ===';

SELECT 
    DATENAME(WEEKDAY, Fecha) AS DiaSemana,
    Fecha,
    CASE WHEN AsisteMañana = 1 THEN 'Si' ELSE 'No' END AS Mañana,
    CASE WHEN AsisteTarde = 1 THEN 'Si' ELSE 'No' END AS Tarde,
    CASE 
        WHEN FechaConfirmacion IS NULL THEN 'Pendiente'
        ELSE 'Confirmado'
    END AS Estado
FROM genesis.AsistenciaAlumno
WHERE IdAlumno = 1
  AND Fecha >= DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)
  AND Fecha <= EOMONTH(GETDATE())
ORDER BY Fecha;

PRINT '';
PRINT 'Listo! Accede a /Padres/ConfirmarAsistencia para ver el calendario';
GO
