-- =========================================
-- Script: Eliminar FK problemática de Alumnos
-- Fecha: 2025-01-25
-- Propósito: Eliminar FK incorrecta FK_Alumnos_TipoRecorridoPago_IdAlumno
-- Esta FK no tiene sentido lógico y bloquea operaciones CRUD
-- =========================================

USE TransportesGenesis;
GO

PRINT '========================================='
PRINT 'Iniciando eliminación de FK problemática'
PRINT '========================================='
PRINT ''

-- Paso 1: Verificar si la FK existe
PRINT 'Paso 1: Verificando existencia de FK...'
IF EXISTS (
    SELECT 1 
    FROM sys.foreign_keys 
    WHERE name = 'FK_Alumnos_TipoRecorridoPago_IdAlumno'
    AND parent_object_id = OBJECT_ID('genesis.Alumnos')
)
BEGIN
    PRINT '✓ FK encontrada: FK_Alumnos_TipoRecorridoPago_IdAlumno'
    PRINT ''

    -- Paso 2: Mostrar detalles de la FK
    PRINT 'Paso 2: Detalles de la FK a eliminar:'
    SELECT 
        f.name AS FK_Name,
        OBJECT_NAME(f.parent_object_id) AS Table_Name,
        COL_NAME(fc.parent_object_id, fc.parent_column_id) AS Column_Name,
        OBJECT_NAME(f.referenced_object_id) AS Referenced_Table,
        COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS Referenced_Column
    FROM sys.foreign_keys AS f
    INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
    WHERE f.name = 'FK_Alumnos_TipoRecorridoPago_IdAlumno'
    PRINT ''

    -- Paso 3: Verificar si hay datos que puedan verse afectados
    PRINT 'Paso 3: Verificando datos existentes...'
    DECLARE @TotalAlumnos INT
    SELECT @TotalAlumnos = COUNT(*) FROM genesis.Alumnos
    PRINT '  - Total de alumnos en la tabla: ' + CAST(@TotalAlumnos AS VARCHAR(10))
    PRINT ''

    -- Paso 4: Eliminar la FK
    PRINT 'Paso 4: Eliminando FK...'
    BEGIN TRY
        ALTER TABLE genesis.Alumnos 
        DROP CONSTRAINT FK_Alumnos_TipoRecorridoPago_IdAlumno;

        PRINT '✓ FK eliminada exitosamente!'
        PRINT ''
    END TRY
    BEGIN CATCH
        PRINT '✗ ERROR al eliminar FK:'
        PRINT '  Mensaje: ' + ERROR_MESSAGE()
        PRINT '  Línea: ' + CAST(ERROR_LINE() AS VARCHAR(10))
        PRINT ''
    END CATCH

    -- Paso 5: Verificar eliminación
    PRINT 'Paso 5: Verificando eliminación...'
    IF NOT EXISTS (
        SELECT 1 
        FROM sys.foreign_keys 
        WHERE name = 'FK_Alumnos_TipoRecorridoPago_IdAlumno'
    )
    BEGIN
        PRINT '✓ FK eliminada correctamente. Ya no existe en la base de datos.'
    END
    ELSE
    BEGIN
        PRINT '✗ La FK todavía existe. Algo salió mal.'
    END
    PRINT ''
END
ELSE
BEGIN
    PRINT '⚠ La FK no existe en la base de datos.'
    PRINT '  Esto puede significar que ya fue eliminada previamente.'
    PRINT ''
END

-- Paso 6: Mostrar FKs restantes en Alumnos
PRINT 'Paso 6: Foreign Keys restantes en tabla Alumnos:'
SELECT 
    f.name AS FK_Name,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS Column_Name,
    OBJECT_NAME(f.referenced_object_id) AS Referenced_Table,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS Referenced_Column
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_key_columns AS fc ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) = 'Alumnos'
ORDER BY f.name
PRINT ''

PRINT '========================================='
PRINT 'Script completado'
PRINT '========================================='
GO

-- Opcional: Test de inserción después de eliminar la FK
-- Descomenta si quieres probar insertar un registro de prueba

/*
PRINT ''
PRINT 'Test: Intentando insertar un alumno de prueba...'
BEGIN TRANSACTION

BEGIN TRY
    DECLARE @IdPadreTest INT = 1 -- Ajusta según tus datos

    -- Si no hay padres, crear uno de prueba
    IF NOT EXISTS (SELECT 1 FROM genesis.Padres WHERE IdPadre = 1)
    BEGIN
        INSERT INTO genesis.Padres (Nombre, Apellido, Activo, FechaRegistro)
        VALUES ('Test', 'Padre', 1, GETDATE())

        SET @IdPadreTest = SCOPE_IDENTITY()
    END

    INSERT INTO genesis.Alumnos (IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@IdPadreTest, 'Test', 'Alumno', NULL, NULL, NULL, NULL, 1, GETDATE())

    PRINT '✓ Inserción exitosa. La FK ya no bloquea inserts.'

    ROLLBACK TRANSACTION -- Deshacer el test
    PRINT '  (Registro de prueba eliminado con ROLLBACK)'
END TRY
BEGIN CATCH
    PRINT '✗ Error en inserción: ' + ERROR_MESSAGE()
    ROLLBACK TRANSACTION
END CATCH
*/
