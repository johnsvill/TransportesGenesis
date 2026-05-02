-- Script para eliminar el DEFAULT CONSTRAINT problemático de la columna Discriminator
-- y permitir que EF Core maneje el valor automáticamente

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE TransportesGenesis;
GO

PRINT '=== VERIFICANDO CONSTRAINTS EN AspNetUsers ===';

-- Encontrar el nombre del default constraint
DECLARE @ConstraintName NVARCHAR(200);
DECLARE @SQL NVARCHAR(MAX);

SELECT @ConstraintName = dc.name
FROM sys.default_constraints dc
INNER JOIN sys.columns c ON dc.parent_column_id = c.column_id AND dc.parent_object_id = c.object_id
WHERE 
    c.object_id = OBJECT_ID('AspNetUsers') 
    AND c.name = 'Discriminator';

IF @ConstraintName IS NOT NULL
BEGIN
    PRINT 'Se encontró el constraint: ' + @ConstraintName;
    PRINT 'Eliminando constraint...';

    SET @SQL = 'ALTER TABLE AspNetUsers DROP CONSTRAINT [' + @ConstraintName + '];';
    EXEC sp_executesql @SQL;

    PRINT 'Constraint eliminado exitosamente.';
END
ELSE
BEGIN
    PRINT 'No se encontró ningún default constraint en la columna Discriminator.';
END
GO

PRINT '=== VERIFICACIÓN COMPLETADA ===';
GO
