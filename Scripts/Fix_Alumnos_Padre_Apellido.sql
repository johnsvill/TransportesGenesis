-- =====================================================
-- Corrige alumnos asignados a un padre con apellido distinto
-- (un padre por apellido de familia del alumno)
-- =====================================================

USE TransportesGenesis;
GO

SET NOCOUNT ON;
PRINT 'Fix: alumnos <-> padres por apellido';

DECLARE @Corregidos INT = 0;

DECLARE @AlumnoId INT, @AlumnoNombre NVARCHAR(100), @AlumnoApellido NVARCHAR(100), @IdPadreActual INT;
DECLARE @NuevoIdPadre INT;

DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
SELECT a.IdAlumno, a.Nombre, a.Apellido, a.IdPadre
FROM genesis.Alumnos a
INNER JOIN genesis.Padres p ON p.IdPadre = a.IdPadre
WHERE LTRIM(RTRIM(a.Apellido)) COLLATE Latin1_General_CI_AI <> LTRIM(RTRIM(p.Apellido)) COLLATE Latin1_General_CI_AI;

OPEN cur;
FETCH NEXT FROM cur INTO @AlumnoId, @AlumnoNombre, @AlumnoApellido, @IdPadreActual;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @NuevoIdPadre = NULL;

    SELECT TOP 1 @NuevoIdPadre = IdPadre
    FROM genesis.Padres
    WHERE LTRIM(RTRIM(Apellido)) COLLATE Latin1_General_CI_AI = LTRIM(RTRIM(@AlumnoApellido)) COLLATE Latin1_General_CI_AI
    ORDER BY IdPadre;

    IF @NuevoIdPadre IS NULL
    BEGIN
        INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
        VALUES (CONCAT(N'Padre/Madre de ', @AlumnoNombre), LTRIM(RTRIM(@AlumnoApellido)), 1, GETDATE());

        SET @NuevoIdPadre = SCOPE_IDENTITY();
        PRINT CONCAT('  + Padre creado IdPadre=', @NuevoIdPadre, ' Apellido=', @AlumnoApellido);
    END

    UPDATE genesis.Alumnos
    SET IdPadre = @NuevoIdPadre
    WHERE IdAlumno = @AlumnoId;

    SET @Corregidos = @Corregidos + 1;
    PRINT CONCAT('  OK Alumno ', @AlumnoId, ' (', @AlumnoNombre, ' ', @AlumnoApellido, ') -> IdPadre ', @NuevoIdPadre);

    FETCH NEXT FROM cur INTO @AlumnoId, @AlumnoNombre, @AlumnoApellido, @IdPadreActual;
END

CLOSE cur;
DEALLOCATE cur;

PRINT CONCAT('Total alumnos corregidos: ', @Corregidos);
GO
