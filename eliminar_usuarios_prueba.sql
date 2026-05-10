-- Script para eliminar usuarios piloto1 y monitor1 (con hash incorrecto)
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;

-- Eliminar asignaciones de bus
DELETE FROM genesis.AsignacionPilotoBus 
WHERE IdUsuarioPiloto IN (
    SELECT Id FROM AspNetUsers 
    WHERE Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
);

-- Eliminar roles
DELETE FROM AspNetUserRoles 
WHERE UserId IN (
    SELECT Id FROM AspNetUsers 
    WHERE Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
);

-- Eliminar usuarios
DELETE FROM AspNetUsers 
WHERE Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com');

-- Confirmar
SELECT 'Usuarios eliminados exitosamente' AS Resultado;
