-- Script para corregir el problema de Discriminator en AspNetUsers
-- Este script:
-- 1. Sincroniza el historial de migraciones con el estado real de la base de datos
-- 2. Actualiza los valores de Discriminator incorrectos a 'AppUser'

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE TransportesGenesis;
GO

-- Paso 1: Verificar registros problemáticos ANTES de la corrección
PRINT '=== VERIFICACIÓN ANTES DE LA CORRECCIÓN ===';
SELECT 
    Id, 
    UserName, 
    Email, 
    Discriminator,
    CASE 
        WHEN Discriminator IS NULL THEN 'NULL'
        WHEN Discriminator = '' THEN 'VACÍO'
        WHEN LEN(RTRIM(Discriminator)) = 0 THEN 'SOLO ESPACIOS'
        ELSE 'OK'
    END AS Estado
FROM AspNetUsers 
WHERE Discriminator IS NULL 
   OR Discriminator = '' 
   OR LEN(RTRIM(Discriminator)) = 0
   OR Discriminator NOT IN ('AppUser', 'IdentityUser');
GO

-- Paso 2: Registrar todas las migraciones aplicadas en el historial
-- (Esto sincroniza el historial con el estado real de la base de datos)
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    PRINT '=== SINCRONIZANDO HISTORIAL DE MIGRACIONES ===';

    -- Solo insertar si no existen ya
    IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '00000000000000_CreateIdentitySchema')
        INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('00000000000000_CreateIdentitySchema', '8.0.10');

    IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20241029172435_Scaffold_de_usuarios')
        INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20241029172435_Scaffold_de_usuarios', '8.0.10');

    IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20241030190844_Create_TipoRecorridoPago_Banco_Padres_Alumnos_TipoCuentaEntity')
        INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20241030190844_Create_TipoRecorridoPago_Banco_Padres_Alumnos_TipoCuentaEntity', '8.0.10');

    IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20241030214842_Create_Pago_Entity')
        INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20241030214842_Create_Pago_Entity', '8.0.10');

    IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20260425154631_AddIsFirstLoginToAppUser')
        INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20260425154631_AddIsFirstLoginToAppUser', '8.0.10');

    IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20260426162657_AddLoginTrackingToAppUser')
        INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20260426162657_AddLoginTrackingToAppUser', '8.0.10');

    IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20260426174720_AddPagosPadres')
        INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20260426174720_AddPagosPadres', '8.0.10');

    IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20260426185106_AddPagosPadresMesAnio')
        INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20260426185106_AddPagosPadresMesAnio', '8.0.10');

    PRINT 'Historial de migraciones sincronizado correctamente.';
END
GO

-- Paso 3: CORREGIR los valores de Discriminator
PRINT '=== APLICANDO CORRECCIÓN ===';

UPDATE AspNetUsers 
SET Discriminator = 'AppUser' 
WHERE Discriminator IS NULL 
   OR Discriminator = '' 
   OR LEN(RTRIM(Discriminator)) = 0
   OR Discriminator NOT IN ('AppUser', 'IdentityUser');

PRINT CONCAT('Se actualizaron ', @@ROWCOUNT, ' registros.');
GO

-- Paso 4: Registrar la migración de corrección en el historial
IF NOT EXISTS (SELECT 1 FROM __EFMigrationsHistory WHERE MigrationId = '20260502193500_FixDiscriminatorValues')
BEGIN
    INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) 
    VALUES ('20260502193500_FixDiscriminatorValues', '8.0.10');
    PRINT 'Migración FixDiscriminatorValues registrada en el historial.';
END
GO

-- Paso 5: Verificar registros DESPUÉS de la corrección
PRINT '=== VERIFICACIÓN DESPUÉS DE LA CORRECCIÓN ===';
SELECT 
    Id, 
    UserName, 
    Email, 
    Discriminator
FROM AspNetUsers;
GO

PRINT '=== CORRECCIÓN COMPLETADA EXITOSAMENTE ===';
GO
