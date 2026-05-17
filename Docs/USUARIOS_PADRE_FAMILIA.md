# 👨‍👩‍👧 Usuarios Padre de Familia - Configuración y Verificación

## 📋 Resumen

Se agregaron **5 usuarios de prueba** con el rol **"PadreDeFamilia"** al seed de datos en `Startup.cs`.

---

## 👤 Usuarios Creados

| Usuario | Email | Contraseña | Rol | Nombre Completo |
|---------|-------|-----------|-----|-----------------|
| `padre1` | padre1@gmail.com | `Admin123!` | PadreDeFamilia | Juan Pérez |
| `padre2` | padre2@gmail.com | `Admin123!` | PadreDeFamilia | María González |
| `padre3` | padre3@gmail.com | `Admin123!` | PadreDeFamilia | Carlos Rodríguez |
| `padre4` | padre4@gmail.com | `Admin123!` | PadreDeFamilia | Ana Martínez |
| `padre5` | padre5@gmail.com | `Admin123!` | PadreDeFamilia | Luis López |

### 🔑 Credenciales para Testing

**Ejemplo de login:**
- **Usuario:** `padre1`
- **Contraseña:** `Admin123!`

---

## 🔧 Código Agregado en `Startup.cs`

```csharp
// CREAR PADRES DE FAMILIA DE PRUEBA
// ============================================
var padres = new[]
{
    new { UserName = "padre1", Email = "padre1@gmail.com", Nombre = "Juan", Apellido = "Pérez" },
    new { UserName = "padre2", Email = "padre2@gmail.com", Nombre = "María", Apellido = "González" },
    new { UserName = "padre3", Email = "padre3@gmail.com", Nombre = "Carlos", Apellido = "Rodríguez" },
    new { UserName = "padre4", Email = "padre4@gmail.com", Nombre = "Ana", Apellido = "Martínez" },
    new { UserName = "padre5", Email = "padre5@gmail.com", Nombre = "Luis", Apellido = "López" }
};

foreach (var padre in padres)
{
    var existeUsuario = await userManager.FindByEmailAsync(padre.Email);
    if (existeUsuario == null)
    {
        var nuevoUsuario = new AppUser
        {
            UserName = padre.UserName,
            Email = padre.Email,
            EmailConfirmed = true,
            IsFirstLogin = false, // Para testing, no obligar cambio de contraseña
            LastLoginDate = DateTime.Now
        };

        var result = await userManager.CreateAsync(nuevoUsuario, "Admin123!");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(nuevoUsuario, "PadreDeFamilia");
            Console.WriteLine($"✅ Usuario {padre.UserName} creado con rol PadreDeFamilia");
        }
    }
}
```

---

## 📊 Verificar en la Base de Datos

### Script SQL: `Scripts/VerificarUsuariosPadres.sql`

Ejecuta este script para verificar los usuarios creados:

```sql
-- Ver usuarios con rol "PadreDeFamilia"
SELECT 
    u.UserName,
    u.Email,
    u.EmailConfirmed,
    u.IsFirstLogin,
    r.Name as Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.NormalizedName = 'PADREDEFAMILIA'
ORDER BY u.UserName;
```

**Resultado esperado:**

| UserName | Email | EmailConfirmed | IsFirstLogin | Rol |
|----------|-------|----------------|--------------|-----|
| padre1 | padre1@gmail.com | 1 | 0 | PadreDeFamilia |
| padre2 | padre2@gmail.com | 1 | 0 | PadreDeFamilia |
| padre3 | padre3@gmail.com | 1 | 0 | PadreDeFamilia |
| padre4 | padre4@gmail.com | 1 | 0 | PadreDeFamilia |
| padre5 | padre5@gmail.com | 1 | 0 | PadreDeFamilia |

---

## 🔄 Activar los Usuarios

### Opción 1: Reiniciar la Aplicación (Recomendado)

1. **Detén el debugger** en Visual Studio (Shift + F5)
2. **Limpia la solución** (Build → Clean Solution)
3. **Compila** (Build → Build Solution)
4. **Inicia el debugger** (F5)

El método `SeedRolesAndAdmin` se ejecutará automáticamente al arrancar y creará los usuarios si no existen.

### Opción 2: Ejecutar Manualmente el Seed

Si por alguna razón el seed no se ejecuta, puedes forzarlo:

**Archivo:** `Program.cs` o `Startup.cs`

Asegúrate de que se llame al método:
```csharp
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedRolesAndAdmin(services);
}
```

---

## 📁 Estructura de Datos Relacionada

### Tabla `AspNetUsers`
Contiene los usuarios de autenticación.

### Tabla `AspNetRoles`
Contiene los roles: `Administrador`, `Piloto`, `Monitor`, `PadreDeFamilia`.

### Tabla `AspNetUserRoles`
Relaciona usuarios con sus roles.

### Tabla `genesis.Padres`
**Nota:** Esta tabla es diferente de `AspNetUsers`. Almacena información adicional de padres de familia (Nombre, Apellido, etc.) y está relacionada con:
- `genesis.Alumnos` (hijos)
- `genesis.Pago` (pagos realizados)
- `genesis.TipoCuenta` (cuentas bancarias)

### Relación Conceptual

```
AspNetUsers (padre1@gmail.com)
    ↓ [Autenticación]
    Rol: PadreDeFamilia
    ↓ [Puede vincularse en el futuro con]
genesis.Padres (Juan Pérez)
    ↓ [Tiene]
genesis.Alumnos (Hijo del padre)
    ↓ [Asignado a]
genesis.Paradas (Paradas del alumno)
```

**⚠️ Nota Importante:** Actualmente, la tabla `Alumnos` **NO tiene una columna `IdUsuarioPadre`** que vincule directamente con `AspNetUsers`. Si necesitas esta relación, deberás:

1. Crear una migración que agregue `IdUsuarioPadre` (tipo `string`, FK a `AspNetUsers.Id`)
2. Actualizar el modelo `Alumnos.cs` con la propiedad y relación
3. Crear una pantalla de administración para vincular padres con alumnos

---

## 🧪 Probar el Login

### Pasos para Probar:

1. **Abre la aplicación:** `https://localhost:7241/Auth/Login`
2. **Credenciales:**
   - Usuario: `padre1`
   - Contraseña: `Admin123!`
3. **Resultado esperado:**
   - Login exitoso
   - Redirección al dashboard de Padre de Familia (si existe)
   - Si no existe dashboard de Padre, redirigirá a `/Home/Index` o página por defecto

---

## 🎯 Siguientes Pasos Recomendados

### 1. Crear Dashboard para Padre de Familia

**Archivo:** `Pages/Padre/Index.cshtml.cs`

```csharp
[Authorize(Roles = "PadreDeFamilia")]
public class IndexModel : PageModel
{
    public void OnGet()
    {
        // Mostrar información de hijos, pagos, rutas, etc.
    }
}
```

### 2. Vincular Usuarios con Tabla `genesis.Padres`

Agregar migración:
```csharp
migrationBuilder.AddColumn<string>(
    name: "IdUsuario",
    schema: "genesis",
    table: "Padres",
    nullable: true);

migrationBuilder.AddForeignKey(
    name: "FK_Padres_AspNetUsers_IdUsuario",
    schema: "genesis",
    table: "Padres",
    column: "IdUsuario",
    principalTable: "AspNetUsers",
    principalColumn: "Id",
    onDelete: ReferentialAction.Restrict);
```

### 3. Configurar Redirección por Rol

**En `Controllers/AuthController.cs` (o donde manejas login):**

```csharp
if (User.IsInRole("PadreDeFamilia"))
{
    return RedirectToPage("/Padre/Index");
}
```

---

## 📝 Scripts SQL Incluidos

### `Scripts/VerificarUsuariosPadres.sql`

Este script incluye:
1. ✅ Verificar si existe el rol "PadreDeFamilia"
2. ✅ Buscar usuarios con rol "PadreDeFamilia"
3. ✅ Contar usuarios por rol
4. ✅ Ver TODOS los usuarios con sus roles
5. ✅ Ver usuarios SIN rol asignado
6. ✅ Ver estructura de `genesis.Alumnos` y `genesis.Padres`

---

## ✅ Resumen de Cambios

| Archivo | Cambio |
|---------|--------|
| ✅ `Startup.cs` | Agregado seed de 5 usuarios Padre de Familia |
| ✅ `Scripts/VerificarUsuariosPadres.sql` | Creado script de verificación |
| ✅ `Docs/USUARIOS_PADRE_FAMILIA.md` | Creada documentación |

---

## 🔐 Seguridad

**Contraseña de prueba:** `Admin123!`

⚠️ **IMPORTANTE:** Estos usuarios son **SOLO PARA PRUEBAS**. En producción:
- Usa contraseñas más seguras
- Habilita `IsFirstLogin = true` para forzar cambio de contraseña
- Configura políticas de contraseñas más estrictas en `Startup.cs`

---

## 📞 Soporte

Si los usuarios no se crean:
1. Revisa la consola de Visual Studio al iniciar la app
2. Busca mensajes como: `"✅ Usuario padre1 creado con rol PadreDeFamilia"`
3. Si ves errores, verifica que el rol `PadreDeFamilia` exista en `AspNetRoles`
4. Ejecuta `Scripts/VerificarUsuariosPadres.sql` para diagnosticar

---

## 🎓 Próximos Módulos Pendientes

Para completar la funcionalidad de Padre de Familia:
- [ ] Dashboard de Padre de Familia (`/Pages/Padre/Index.cshtml`)
- [ ] Vista de hijos asignados
- [ ] Historial de pagos
- [ ] Seguimiento de rutas en tiempo real
- [ ] Notificaciones de proximidad/llegada
- [ ] Gestión de solicitudes de traslado