# 🚀 GUÍA RÁPIDA DE IMPLEMENTACIÓN - Piloto y Monitor

## ⚡ RESUMEN EN 5 PASOS

```
PASO 1: Crear Usuarios de Prueba          [⏱️ 5 min]
PASO 2: Corregir Página de Piloto          [⏱️ 10 min]
PASO 3: Crear Páginas de Monitor           [⏱️ 30 min]
PASO 4: Actualizar Redirección y Menú      [⏱️ 5 min]
PASO 5: Probar Todo                         [⏱️ 10 min]
                                            ─────────
                                   TOTAL:    60 min
```

---

## 📋 CHECKLIST DE IMPLEMENTACIÓN

### ✅ **PASO 1: Crear Usuarios de Prueba** (5 min)

**Opción A - Método Rápido (Recomendado):**

1. Abrir `Controllers/AdminController.cs` (o crear si no existe)
2. Agregar método:
   ```csharp
   [Authorize(Roles = "Administrador")]
   [HttpGet]
   public async Task<IActionResult> CrearUsuariosPrueba()
   {
       // Código en PLAN_IMPLEMENTACION_PILOTO_MONITOR.md - Sección 3.2
   }
   ```
3. Iniciar sesión como Admin
4. Navegar a: `/Admin/CrearUsuariosPrueba`
5. Verificar mensaje de éxito

**Opción B - SQL Directo:**
```sql
-- Ver script completo en PLAN_IMPLEMENTACION_PILOTO_MONITOR.md - Sección 3.1
```

**✅ Resultado Esperado:**
- Usuario: `piloto1@transportesgenesis.com` / Password: `Piloto123!`
- Usuario: `monitor1@transportesgenesis.com` / Password: `Monitor123!`
- Ambos asignados a Bus #1

---

### ✅ **PASO 2: Corregir Página de Piloto** (10 min)

**Archivo:** `Pages/Piloto/MiRuta.cshtml.cs`

**Cambio 1:** Agregar imports (al inicio del archivo)
```csharp
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
```

**Cambio 2:** Agregar autorización (antes de la clase)
```csharp
[Authorize(Roles = "Piloto")]
public class MiRutaModel : PageModel
```

**Cambio 3:** Modificar constructor y propiedades
```csharp
// ❌ ANTES:
private readonly IHttpClientFactory _httpClientFactory;
public int IdBus { get; set; } = 4; // Hardcodeado

public MiRutaModel(IHttpClientFactory httpClientFactory)
{
    _httpClientFactory = httpClientFactory;
}

// ✅ DESPUÉS:
private readonly ApplicationDbContext _context;
private readonly IHttpClientFactory _httpClientFactory;
public int IdBus { get; set; } // Sin valor inicial

public MiRutaModel(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
{
    _context = context;
    _httpClientFactory = httpClientFactory;
}
```

**Cambio 4:** Modificar OnGetAsync (al inicio del método)
```csharp
public async Task OnGetAsync()
{
    try
    {
        // ✅ AGREGAR ESTAS LÍNEAS AL INICIO
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        NombrePiloto = User.Identity?.Name ?? "Piloto";

        var asignacion = await _context.AsignacionesPilotoBusDb
            .Where(a => a.IdUsuarioPiloto == userId && a.EsActual && a.Activo == 1)
            .Include(a => a.Bus)
            .FirstOrDefaultAsync();

        if (asignacion == null)
        {
            MensajeError = "No tienes un bus asignado. Contacta al administrador.";
            return;
        }

        IdBus = asignacion.Bus.IdBus;

        // ... resto del código existente ...
    }
    catch (Exception ex)
    {
        MensajeError = $"Error inesperado: {ex.Message}";
    }
}
```

**✅ Verificar:**
- [ ] Compilación sin errores
- [ ] Login como piloto1 redirige a /Piloto/MiRuta
- [ ] Se muestra el bus asignado correctamente

---

### ✅ **PASO 3: Crear Páginas de Monitor** (30 min)

#### **3.1 Crear Carpeta**
1. Clic derecho en `Pages` → Agregar → Nueva carpeta
2. Nombre: `Monitor`

#### **3.2 Crear MiRuta.cshtml.cs**
1. Clic derecho en `Pages/Monitor` → Agregar → Razor Page
2. Nombre: `MiRuta`
3. Reemplazar código con:
   ```csharp
   // Ver código completo en PLAN_IMPLEMENTACION_PILOTO_MONITOR.md - Sección 2.2
   ```

#### **3.3 Crear MiRuta.cshtml**
1. Ya se creó automáticamente en el paso anterior
2. Reemplazar contenido con:
   ```html
   @page
   @model TransportesGenesis.Pages.Monitor.MiRutaModel
   <!-- Ver código completo en PLAN_IMPLEMENTACION_PILOTO_MONITOR.md - Sección 2.2 -->
   ```

#### **3.4 Crear RegistrarRecogidas.cshtml.cs**
1. Clic derecho en `Pages/Monitor` → Agregar → Razor Page
2. Nombre: `RegistrarRecogidas`
3. Reemplazar código con:
   ```csharp
   // Ver código completo en PLAN_IMPLEMENTACION_PILOTO_MONITOR.md - Sección 2.3
   ```

#### **3.5 Crear RegistrarRecogidas.cshtml**
1. Ya se creó automáticamente
2. Reemplazar contenido con:
   ```html
   @page
   @model TransportesGenesis.Pages.Monitor.RegistrarRecogidasModel
   <!-- Ver código completo en PLAN_IMPLEMENTACION_PILOTO_MONITOR.md - Sección 2.3 -->
   ```

**✅ Verificar:**
- [ ] Carpeta `Pages/Monitor` con 4 archivos (.cshtml y .cshtml.cs)
- [ ] Compilación sin errores

---

### ✅ **PASO 4: Actualizar Redirección y Menú** (5 min)

#### **4.1 Actualizar AuthController.cs**

**Archivo:** `Controllers/AuthController.cs`

**Ubicación:** Dentro del método `Login`, después de la línea 78

**Cambio:**
```csharp
// ❌ ANTES:
else if (roles.Contains("Piloto"))
{
    return RedirectToPage("/Piloto/MiRuta");
}
else
{
    return RedirectToAction("Index", "Home");
}

// ✅ DESPUÉS:
else if (roles.Contains("Piloto"))
{
    return RedirectToPage("/Piloto/MiRuta");
}
else if (roles.Contains("Monitor"))  // ✅ AGREGAR ESTO
{
    return RedirectToPage("/Monitor/MiRuta");
}
else
{
    return RedirectToAction("Index", "Home");
}
```

#### **4.2 Actualizar _Layout.cshtml**

**Archivo:** `Views/Shared/_Layout.cshtml` o `Pages/Shared/_Layout.cshtml`

**Ubicación:** Dentro del `<nav>` existente

**Agregar:**
```html
<!-- Menú para Piloto -->
@if (User.IsInRole("Piloto"))
{
    <li class="nav-item">
        <a class="nav-link" asp-page="/Piloto/MiRuta">
            <i class="fas fa-bus"></i> Mi Ruta
        </a>
    </li>
}

<!-- Menú para Monitor -->
@if (User.IsInRole("Monitor"))
{
    <li class="nav-item">
        <a class="nav-link" asp-page="/Monitor/MiRuta">
            <i class="fas fa-route"></i> Mi Ruta
        </a>
    </li>
    <li class="nav-item">
        <a class="nav-link" asp-page="/Monitor/RegistrarRecogidas">
            <i class="fas fa-clipboard-check"></i> Registrar Recogidas
        </a>
    </li>
}
```

**✅ Verificar:**
- [ ] Compilación sin errores
- [ ] Login como monitor1 redirige a /Monitor/MiRuta

---

### ✅ **PASO 5: PROBAR TODO** (10 min)

#### **Prueba 1: Login como Piloto**
1. Cerrar sesión si está logueado
2. Ir a `/Auth/Login`
3. Ingresar: `piloto1@transportesgenesis.com` / `Piloto123!`
4. **✅ Verificar:** Redirige automáticamente a `/Piloto/MiRuta`
5. **✅ Verificar:** Se muestra el bus asignado (no el hardcodeado)
6. **✅ Verificar:** En el menú aparece "Mi Ruta" para Piloto

#### **Prueba 2: Login como Monitor**
1. Cerrar sesión
2. Ir a `/Auth/Login`
3. Ingresar: `monitor1@transportesgenesis.com` / `Monitor123!`
4. **✅ Verificar:** Redirige automáticamente a `/Monitor/MiRuta`
5. **✅ Verificar:** Se muestra la ruta asignada
6. **✅ Verificar:** En el menú aparecen 2 opciones: "Mi Ruta" y "Registrar Recogidas"
7. **✅ Verificar:** Clic en "Registrar Recogidas" muestra el listado de alumnos
8. **✅ Verificar:** Puede marcar alumnos como "Presente" o "Ausente"

#### **Prueba 3: Seguridad**
1. Cerrar sesión
2. Intentar acceder directamente a `/Piloto/MiRuta`
3. **✅ Verificar:** Redirige a `/Auth/Login` (protegido)
4. Intentar acceder directamente a `/Monitor/RegistrarRecogidas`
5. **✅ Verificar:** Redirige a `/Auth/Login` (protegido)

#### **Prueba 4: Registrar Recogidas**
1. Login como Monitor
2. Ir a "Registrar Recogidas"
3. Hacer clic en "✓ Presente" en un alumno
4. **✅ Verificar:** Aparece mensaje "Alumno marcado como recogido"
5. **✅ Verificar:** La fila del alumno cambia a color verde
6. **✅ Verificar:** El botón "✓ Presente" se deshabilita
7. Hacer clic en "✗ Ausente" en otro alumno
8. **✅ Verificar:** Aparece mensaje correspondiente

---

## 🐛 SOLUCIÓN DE PROBLEMAS COMUNES

### ❌ Error: "No tienes un bus asignado"

**Causa:** El usuario no tiene registro en `AsignacionPilotoBus`

**Solución:**
```sql
-- Ejecutar en SQL Server Management Studio
USE TransportesGenesis;

-- Obtener ID del usuario
DECLARE @UserId NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE Email = 'piloto1@transportesgenesis.com');

-- Verificar si existe asignación
SELECT * FROM genesis.AsignacionPilotoBus WHERE IdUsuarioPiloto = @UserId;

-- Si no existe, crear una
INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
VALUES (@UserId, 1, GETDATE(), 1, 1, GETDATE());
```

---

### ❌ Error de compilación: "ApplicationDbContext no encontrado"

**Causa:** Falta importar namespace

**Solución:**
```csharp
using TransportesGenesis.Data.Context;
```

---

### ❌ Error: "La ruta /Monitor/MiRuta no existe"

**Causa:** No se creó correctamente la página

**Verificar:**
1. Existe el archivo `Pages/Monitor/MiRuta.cshtml`
2. Existe el archivo `Pages/Monitor/MiRuta.cshtml.cs`
3. El namespace es correcto: `namespace TransportesGenesis.Pages.Monitor`

---

### ❌ Error: "No hay alumnos en la ruta actual"

**Causa:** No hay rutas calculadas o no hay paradas con alumnos asignados

**Solución:**
1. Login como Administrador
2. Ir a `/Admin/CalcularRutas`
3. Calcular rutas para el día actual
4. Verificar que las rutas tengan paradas con alumnos

---

## 📊 CHECKLIST FINAL

### ✅ **Antes de dar por terminado:**

- [ ] ✅ Usuarios piloto1 y monitor1 creados
- [ ] ✅ `MiRuta.cshtml.cs` tiene `[Authorize(Roles = "Piloto")]`
- [ ] ✅ IdBus ya no está hardcodeado en Piloto
- [ ] ✅ Carpeta `Pages/Monitor/` creada con 2 páginas
- [ ] ✅ Redirección para Monitor agregada en `AuthController.cs`
- [ ] ✅ Navegación agregada en `_Layout.cshtml`
- [ ] ✅ Login como Piloto funciona correctamente
- [ ] ✅ Login como Monitor funciona correctamente
- [ ] ✅ Registro de recogidas guarda en `RegistroRecogida`
- [ ] ✅ Páginas protegidas con `[Authorize]`
- [ ] ✅ No se rompió funcionalidad existente (Admin, Padres)

---

## 📚 REFERENCIAS

- **Plan Completo:** `PLAN_IMPLEMENTACION_PILOTO_MONITOR.md`
- **Código de AuthController:** Líneas 60-90
- **Código de Startup:** Líneas 143-192
- **Tabla AsignacionPilotoBus:** `Models/DB/Negocio/AsignacionPilotoBus.cs`
- **Tabla RegistroRecogida:** `Models/DB/Negocio/RegistroRecogida.cs`

---

## ⏱️ TIEMPO ESTIMADO POR EXPERIENCIA

| Nivel             | Tiempo Estimado |
|-------------------|-----------------|
| Senior Developer  | 30-45 min       |
| Mid Developer     | 60-90 min       |
| Junior Developer  | 90-120 min      |

---

**✅ ¡LISTO PARA IMPLEMENTAR!**

**Fecha:** 2026-05-08  
**Versión:** 1.0
