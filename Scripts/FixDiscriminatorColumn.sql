-- Script para corregir definitivamente el problema del Discriminator
-- Alterando la columna para que tenga un DEFAULT correcto

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE TransportesGenesis;
GO

PRINT '=== CORRIGIENDO COLUMNA DISCRIMINATOR ===';

-- Paso 1: Verificar si existe algún default constraint y eliminarlo
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
    PRINT 'Eliminando constraint existente: ' + @ConstraintName;
    SET @SQL = 'ALTER TABLE AspNetUsers DROP CONSTRAINT [' + @ConstraintName + '];';
    EXEC sp_executesql @SQL;
END
GO

-- Paso 2: Alterar la columna para cambiar de NOT NULL a NULL
PRINT 'Alterando columna Discriminator para permitir NULL...';
ALTER TABLE AspNetUsers 
ALTER COLUMN Discriminator nvarchar(13) NULL;
GO

-- Paso 3: Agregar un DEFAULT constraint con el valor correcto 'AppUser'
PRINT 'Agregando DEFAULT constraint con valor AppUser...';
ALTER TABLE AspNetUsers 
ADD CONSTRAINT DF_AspNetUsers_Discriminator DEFAULT 'AppUser' FOR Discriminator;
GO

-- Paso 4: Actualizar cualquier registro existente que tenga valores incorrectos
PRINT 'Actualizando registros existentes...';
UPDATE AspNetUsers 
SET Discriminator = 'AppUser' 
WHERE Discriminator IS NULL 
   OR Discriminator = '' 
   OR LEN(RTRIM(Discriminator)) = 0
   OR Discriminator NOT IN ('AppUser', 'IdentityUser');
GO

-- Paso 5: Verificación final
PRINT '=== VERIFICACIÓN FINAL ===';
SELECT 
    COLUMN_NAME, 
    DATA_TYPE, 
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'AspNetUsers' 
  AND COLUMN_NAME = 'Discriminator';
GO

SELECT COUNT(*) AS TotalUsuarios,
       SUM(CASE WHEN Discriminator = 'AppUser' THEN 1 ELSE 0 END) AS UsuariosAppUser,
       SUM(CASE WHEN Discriminator IS NULL OR Discriminator = '' THEN 1 ELSE 0 END) AS UsuariosProblematicos
FROM AspNetUsers;
GO

PRINT '=== CORRECCIÓN COMPLETADA EXITOSAMENTE ===';
