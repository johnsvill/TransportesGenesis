-- =========================================
-- Script: Crear Pilotos y Monitores de Prueba
-- Propósito: Insertar usuarios con roles Piloto y Monitor para testing
-- Contraseña: Admin123!
-- =========================================

USE TransportesGenesis;
GO

PRINT '========================================='
PRINT 'Creando Pilotos y Monitores de Prueba'
PRINT 'Contraseña para todos: Admin123!'
PRINT '========================================='

-- ============================================
-- NOTA IMPORTANTE:
-- Los usuarios deben crearse desde la aplicación o usar el UserManager
-- porque las contraseñas deben ser hasheadas correctamente.
-- 
-- Este script solo sirve de REFERENCIA.
-- Para crear usuarios correctamente, ejecuta este código desde C#:
-- ============================================

/*
// OPCIÓN 1: Ejecutar desde C# (Recomendado)
// Agregar este código temporal en Startup.cs dentro de SeedRolesAndAdmin()

var pilotosMonitores = new[]
{
    new { UserName = "piloto2", Email = "piloto2@transportesgenesis.com", Rol = "Piloto" },
    new { UserName = "piloto3", Email = "piloto3@transportesgenesis.com", Rol = "Piloto" },
    new { UserName = "piloto4", Email = "piloto4@transportesgenesis.com", Rol = "Piloto" },
    new { UserName = "monitor2", Email = "monitor2@transportesgenesis.com", Rol = "Monitor" },
    new { UserName = "monitor3", Email = "monitor3@transportesgenesis.com", Rol = "Monitor" },
    new { UserName = "monitor4", Email = "monitor4@transportesgenesis.com", Rol = "Monitor" }
};

foreach (var usuario in pilotosMonitores)
{
    var existeUsuario = await userManager.FindByEmailAsync(usuario.Email);
    if (existeUsuario == null)
    {
        var nuevoUsuario = new AppUser
        {
            UserName = usuario.UserName,
            Email = usuario.Email,
            EmailConfirmed = true,
            IsFirstLogin = false, // Para testing, no obligar cambio de contraseña
            LastLoginDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(nuevoUsuario, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(nuevoUsuario, usuario.Rol);
            Console.WriteLine($"✅ Usuario {usuario.UserName} creado con rol {usuario.Rol}");
        }
        else
        {
            Console.WriteLine($"❌ Error al crear {usuario.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}
*/

-- ============================================
-- OPCIÓN 2: Hash de contraseña (para referencia)
-- ============================================
-- La contraseña "Admin123!" hasheada con Identity PasswordHasher es:
-- AQAAAAIAAYagAAAAEIHKcO/M1... (varía cada vez)
-- NO se puede predecir el hash exacto porque usa salt aleatorio

-- Por lo tanto, la mejor forma es usar la aplicación para crear usuarios

-- ============================================
-- VERIFICAR USUARIOS EXISTENTES
-- ============================================

PRINT ''
PRINT 'Usuarios con rol Piloto actualmente registrados:'
SELECT 
    u.UserName, 
    u.Email, 
    r.Name AS Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'Piloto'
ORDER BY u.UserName;

PRINT ''
PRINT 'Usuarios con rol Monitor actualmente registrados:'
SELECT 
    u.UserName, 
    u.Email, 
    r.Name AS Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'Monitor'
ORDER BY u.UserName;

PRINT ''
PRINT '========================================='
PRINT 'INSTRUCCIONES PARA CREAR USUARIOS:'
PRINT '========================================='
PRINT '1. Opción A: Usar la pantalla /Admin/Usuarios'
PRINT '2. Opción B: Agregar código temporal en Startup.cs (ver comentarios arriba)'
PRINT '3. Opción C: Crear usuarios desde /Account/Register'
PRINT ''
PRINT 'Contraseña a usar: Admin123!'
PRINT '========================================='

GO
