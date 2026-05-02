-- =====================================================
-- SCRIPT: Hacer IdAlumno nullable en Paradas
-- Proposito: Permitir paradas del colegio sin IdAlumno
-- =====================================================

USE TransportesGenesis;
GO

PRINT 'Modificando tabla Paradas para permitir IdAlumno NULL...';

-- Verificar si la columna ya es nullable
IF EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('genesis.Paradas') 
      AND name = 'IdAlumno' 
      AND is_nullable = 0
)
BEGIN
    PRINT 'Haciendo IdAlumno nullable...';

    -- Primero eliminar el constraint de FK si existe
    DECLARE @ConstraintName NVARCHAR(200);
    SELECT @ConstraintName = name 
    FROM sys.foreign_keys 
    WHERE parent_object_id = OBJECT_ID('genesis.Paradas') 
      AND referenced_object_id = OBJECT_ID('genesis.Alumnos');

    IF @ConstraintName IS NOT NULL
    BEGIN
        EXEC('ALTER TABLE genesis.Paradas DROP CONSTRAINT ' + @ConstraintName);
        PRINT 'Foreign Key eliminada: ' + @ConstraintName;
    END

    -- Modificar la columna para permitir NULL
    ALTER TABLE genesis.Paradas 
    ALTER COLUMN IdAlumno INT NULL;

    PRINT 'Columna IdAlumno ahora es nullable';

    -- Recrear el FK (ahora permite NULL)
    ALTER TABLE genesis.Paradas
    ADD CONSTRAINT FK_Paradas_Alumnos 
    FOREIGN KEY (IdAlumno) REFERENCES genesis.Alumnos(IdAlumno);

    PRINT 'Foreign Key recreada';
END
ELSE
BEGIN
    PRINT 'IdAlumno ya es nullable';
END
GO

PRINT 'Modificacion completada exitosamente';
GO
