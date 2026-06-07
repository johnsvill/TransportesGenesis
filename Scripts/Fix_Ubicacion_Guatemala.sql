-- =====================================================
-- Script: Actualizar Ubicación del Colegio a Guatemala
-- Propósito: Cambiar coordenadas de Bogotá a Guatemala
-- Fecha: 26/04/2026
-- =====================================================

USE TransportesGenesis;
GO

PRINT '========================================';
PRINT 'Actualizando ubicación del colegio...';
PRINT '========================================';
PRINT '';

-- Actualizar dirección del colegio
UPDATE genesis.ConfiguracionSistema
SET Valor = 'Zona 10, Ciudad de Guatemala',
    UltimaModificacion = GETDATE(),
    ModificadoPor = 'admin'
WHERE Clave = 'Colegio_Direccion';

PRINT '✓ Dirección actualizada a Guatemala';

-- Actualizar Latitud (Ciudad de Guatemala centro)
UPDATE genesis.ConfiguracionSistema
SET Valor = '14.6349',
    UltimaModificacion = GETDATE(),
    ModificadoPor = 'admin'
WHERE Clave = 'Colegio_Latitud';

PRINT '✓ Latitud actualizada: 14.6349';

-- Actualizar Longitud (Ciudad de Guatemala centro)
UPDATE genesis.ConfiguracionSistema
SET Valor = '-90.5069',
    UltimaModificacion = GETDATE(),
    ModificadoPor = 'admin'
WHERE Clave = 'Colegio_Longitud';

PRINT '✓ Longitud actualizada: -90.5069';

PRINT '';
PRINT '========================================';
PRINT 'Actualización completada';
PRINT '========================================';
PRINT '';

-- Verificar cambios
SELECT 
    Clave,
    Valor,
    Descripcion,
    UltimaModificacion
FROM genesis.ConfiguracionSistema
WHERE Clave IN ('Colegio_Nombre', 'Colegio_Direccion', 'Colegio_Latitud', 'Colegio_Longitud')
ORDER BY Clave;
GO

PRINT '';
PRINT '========================================';
PRINT 'NOTA: Las coordenadas ahora apuntan a:';
PRINT 'Ciudad de Guatemala (Zona 10)';
PRINT 'Latitud: 14.6349, Longitud: -90.5069';
PRINT '========================================';
GO
