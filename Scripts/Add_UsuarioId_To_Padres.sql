# 🔧 Migración Manual: Agregar UsuarioId a Padres

## Script SQL para ejecutar en la base de datos

```sql
USE TransportesGenesisDB;
GO

-- Agregar columna UsuarioId a la tabla Padres
ALTER TABLE genesis.Padres
ADD UsuarioId NVARCHAR(450) NULL;
GO

-- Crear índice para mejorar performance
CREATE INDEX IX_Padres_UsuarioId ON genesis.Padres(UsuarioId);
GO

-- Comentario descriptivo
EXEC sp_addextendedproperty 
	@name = N'MS_Description', 
	@value = N'ID del usuario en AspNetUsers (Identity)', 
	@level0type = N'SCHEMA', @level0name = N'genesis',
	@level1type = N'TABLE',  @level1name = N'Padres',
	@level2type = N'COLUMN', @level2name = N'UsuarioId';
GO

PRINT 'Columna UsuarioId agregada exitosamente a genesis.Padres';
```

## Instrucciones

1. Abre SQL Server Management Studio (SSMS)
2. Conéctate a tu servidor
3. Abre una nueva consulta
4. Copia y pega el script anterior
5. Ejecuta (F5)

## Verificar

```sql
-- Verificar que la columna se agregó
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'genesis' 
  AND TABLE_NAME = 'Padres'
  AND COLUMN_NAME = 'UsuarioId';
```

## Vincular Usuarios Existentes (Opcional)

Si ya tienes padres y usuarios creados, necesitas vincularlos:

```sql
-- Ejemplo: Vincular padre "Juan Pérez" con usuario "juan@gmail.com"
UPDATE genesis.Padres
SET UsuarioId = (
	SELECT Id FROM AspNetUsers WHERE Email = 'juan@gmail.com'
)
WHERE Nombre = 'Juan' AND Apellido = 'Pérez';
```
-- Ver: Scripts\Add_UsuarioId_To_Padres.sql
ALTER TABLE genesis.Padres ADD UsuarioId NVARCHAR(450) NULL;