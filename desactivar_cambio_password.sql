-- Script para desactivar IsFirstLogin en piloto1 y monitor1
-- Esto permite saltarse el cambio de contraseña obligatorio (SOLO PARA PRUEBAS)

SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

UPDATE AspNetUsers 
SET IsFirstLogin = 0 
WHERE Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com');

-- Verificar
SELECT 
    UserName AS Usuario,
    Email,
    IsFirstLogin AS PrimerLogin,
    CASE WHEN IsFirstLogin = 1 THEN '❌ Requiere cambio de contraseña' ELSE '✅ Puede entrar directamente' END AS Estado
FROM AspNetUsers 
WHERE Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com');

PRINT '';
PRINT '✅ IsFirstLogin desactivado para piloto1 y monitor1';
PRINT '📋 Ahora pueden hacer login directamente sin cambiar contraseña';
