# 📋 PLAN DETALLADO: Implementación de Áreas de Piloto y Monitor

## 🎯 OBJETIVO GENERAL
Implementar dos nuevas áreas en el sistema con redirección automática basada en roles:
1. **Área del Piloto** - Para conductores de buses que consultan su ruta asignada
2. **Área del Monitor** - Para monitores que acompañan al bus y registran la asistencia/recogida de alumnos

**Principio fundamental:** NO romper nada de lo que ya funciona. Todo debe ser modular y reutilizar código existente.

---

## 📊 RESUMEN EJECUTIVO

### ✅ **LO QUE YA EXISTE (No tocar):**
- ✅ Sistema de login funcional en `AuthController.cs`
- ✅ Roles "Piloto" y "Monitor" creados en `Startup.cs`
- ✅ Redirección para Piloto ya configurada (línea 78 de AuthController)
- ✅ Página `/Piloto/MiRuta` ya existe
- ✅ Tablas `AsignacionPilotoBus` y `RegistroRecogida` en BD

### ⚠️ **LO QUE FALTA CORREGIR:**
- ⚠️ Agregar `[Authorize(Roles = "Piloto")]` a `MiRuta.cshtml.cs`
- ⚠️ Reemplazar `IdBus = 4` por consulta a `AsignacionPilotoBus`

### ❌ **LO QUE HAY QUE CREAR:**
- ❌ Carpeta `Pages/Monitor/`
- ❌ Página `/Monitor/MiRuta` (clonar de Piloto)
- ❌ Página `/Monitor/RegistrarRecogidas` (nueva funcionalidad)
- ❌ Redirección para Monitor en `AuthController.cs`
- ❌ Navegación en `_Layout.cshtml`
- ❌ Usuarios de prueba con roles Piloto y Monitor

### 📋 **ARCHIVOS A MODIFICAR/CREAR:**
```
✏️ MODIFICAR (3 archivos):
   └─ Pages/Piloto/MiRuta.cshtml.cs
   └─ Controllers/AuthController.cs
   └─ Views/Shared/_Layout.cshtml (o Pages/Shared/_Layout.cshtml)

✨ CREAR (5 archivos):
   └─ Pages/Monitor/MiRuta.cshtml
   └─ Pages/Monitor/MiRuta.cshtml.cs
   └─ Pages/Monitor/RegistrarRecogidas.cshtml
   └─ Pages/Monitor/RegistrarRecogidas.cshtml.cs
   └─ Controllers/AdminController.cs (método CrearUsuariosPrueba)
```

---

## 📚 FASE 1: REVISIÓN Y ANÁLISIS DEL SISTEMA ACTUAL

### ✅ **1.1 Sistema de Login Existente (REFERENCIA PARA PADRE)**

**Archivo de Referencia:** `Controllers/AuthController.cs` (líneas 34-99)

**Flujo de Login Actual:**
```
┌─────────────────────────────────────────────────────────┐
│ 1. Usuario ingresa Email/Password en /Auth/Login       │
│ 2. AuthController valida con UserManager/SignInManager │
│ 3. Si es exitoso → Actualiza LastLoginDate             │
│ 4. Verifica IsFirstLogin (redirige a cambio de pass)   │
│ 5. Obtiene roles del usuario con GetRolesAsync()       │
│ 6. REDIRECCIÓN SEGÚN ROL:                              │
│    ├─ Administrador → /Admin/Index                     │
│    ├─ PadreDeFamilia → /PagosPadresFamilia/Index       │
│    ├─ Piloto → /Piloto/MiRuta ✅ (YA CONFIGURADO)      │
│    └─ Otros → /Home/Index                              │
└─────────────────────────────────────────────────────────┘
```

**Código clave en AuthController.cs:**
```csharp
// Líneas 60-82
var roles = await _userManager.GetRolesAsync(user);

if (roles.Contains("Administrador"))
    return RedirectToAction("Index", "Admin");
else if (roles.Contains("PadreDeFamilia"))
    return RedirectToAction("Index", "PagosPadresFamilia");
else if (roles.Contains("Piloto"))
    return RedirectToPage("/Piloto/MiRuta");  // ✅ YA EXISTE
else
    return RedirectToAction("Index", "Home");
```

**✅ ACCIÓN REQUERIDA:**
- Agregar condición `else if (roles.Contains("Monitor"))` después de Piloto
- No modificar las redirecciones existentes

---

### ✅ **1.2 Confirmación de Roles en Identity**

**Archivo de Referencia:** `Startup.cs` (líneas 143-192)

**Seed de Roles Actual:**
```csharp
private static async Task SeedRolesAndAdmin(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
{
    string[] roleNames = { "Administrador", "Piloto", "Monitor", "PadreDeFamilia" };

    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
    // ... código de creación de admin ...
}
```

**✅ ESTADO ACTUAL:**
- ✅ Rol "Piloto" → **YA EXISTE** en la base de datos
- ✅ Rol "Monitor" → **YA EXISTE** en la base de datos
- ✅ Roles se crean automáticamente al iniciar la aplicación

**✅ ACCIÓN REQUERIDA:**
- ✅ **NINGUNA** - Los roles ya están creados
- ✅ Solo falta crear usuarios y asignarles estos roles
- ✅ Verificar en BD: `SELECT * FROM AspNetRoles WHERE Name IN ('Piloto', 'Monitor')`

---

### ✅ **1.3 Estructura de Páginas Razor Existente (REFERENCIA)**

**Carpetas de páginas actuales:**
```
Pages/
├── Admin/                          [Authorize(Roles = "Administrador")]
│   ├── AlertasHistorial.cshtml    ✅ Ejemplo de autorización
│   ├── CalcularRutas.cshtml
│   ├── GestionarTraslados.cshtml
│   └── Usuarios/Index.cshtml
│
├── Padres/                         [Authorize(Roles = "PadreDeFamilia")]
│   ├── ConfirmarAsistencia.cshtml ✅ Ejemplo de autorización
│   ├── DashboardRutaBusAsignado.cshtml
│   └── Traslados.cshtml
│
├── Piloto/                         [⚠️ SIN AUTORIZACIÓN - PENDIENTE]
│   └── MiRuta.cshtml              ⚠️ Falta [Authorize(Roles = "Piloto")]
│       └── MiRuta.cshtml.cs       ⚠️ Falta obtener IdBus del usuario
│
└── Geolocalizacion/                [Sin autorización - páginas públicas]
    ├── BusesIndex.cshtml
    ├── MapaEnTiempoReal.cshtml
    └── SimulacionMapa.cshtml
```

**✅ ANÁLISIS:**
- ✅ La carpeta `Pages/Piloto/` **YA EXISTE** con `MiRuta.cshtml`
- ⚠️ **PROBLEMA 1:** Falta atributo `[Authorize(Roles = "Piloto")]`
- ⚠️ **PROBLEMA 2:** IdBus está hardcodeado: `public int IdBus { get; set; } = 4;`
- ❌ La carpeta `Pages/Monitor/` **NO EXISTE** - Hay que crearla

**✅ ACCIONES REQUERIDAS:**
1. Agregar `[Authorize(Roles = "Piloto")]` a `MiRuta.cshtml.cs`
2. Reemplazar IdBus hardcodeado por consulta a `AsignacionPilotoBus`
3. Crear carpeta `Pages/Monitor/` con sus páginas

---

### ✅ **1.4 Revisión de Código: Pages/Piloto/MiRuta.cshtml.cs**

**Archivo a Revisar:** `Pages/Piloto/MiRuta.cshtml.cs`

**PROBLEMA 1: Falta Autorización**
```csharp
// ❌ CÓDIGO ACTUAL (línea 7)
namespace TransportesGenesis.Pages.Piloto
{
    public class MiRutaModel : PageModel  // ⚠️ Sin [Authorize]
```

**✅ SOLUCIÓN:**
```csharp
// ✅ CÓDIGO CORREGIDO
using Microsoft.AspNetCore.Authorization;

namespace TransportesGenesis.Pages.Piloto
{
    [Authorize(Roles = "Piloto")]  // ✅ AGREGAR ESTO
    public class MiRutaModel : PageModel
```

---

**PROBLEMA 2: IdBus Hardcodeado**
```csharp
// ❌ CÓDIGO ACTUAL (línea 16)
public int IdBus { get; set; } = 4; // TODO: Obtener del usuario autenticado (Claims)
```

**✅ SOLUCIÓN - Opción A (Recomendada): Consulta Directa en OnGetAsync**
```csharp
// ✅ CÓDIGO CORREGIDO
public int IdBus { get; set; }
private readonly ApplicationDbContext _context;
private readonly IHttpClientFactory _httpClientFactory;

public MiRutaModel(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
{
    _context = context;
    _httpClientFactory = httpClientFactory;
}

public async Task OnGetAsync()
{
    try
    {
        // ✅ OBTENER ID DEL USUARIO AUTENTICADO
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        NombrePiloto = User.Identity?.Name ?? "Piloto";

        // ✅ BUSCAR BUS ASIGNADO EN AsignacionPilotoBus
        var asignacion = await _context.AsignacionesPilotoBusDb
            .Where(a => a.IdUsuarioPiloto == userId && a.EsActual && a.Activo == 1)
            .Include(a => a.Bus)
            .FirstOrDefaultAsync();

        if (asignacion == null)
        {
            MensajeError = "No tienes un bus asignado actualmente. Contacta al administrador.";
            return;
        }

        // ✅ ASIGNAR IdBus DINÁMICAMENTE
        IdBus = asignacion.Bus.IdBus;

        // ... resto del código (obtener ruta, etc.)
    }
    catch (Exception ex)
    {
        MensajeError = $"Error inesperado: {ex.Message}";
    }
}
```

**✅ IMPORTS NECESARIOS:**
```csharp
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
```

---

**RESUMEN DE CAMBIOS EN MiRuta.cshtml.cs:**
1. ✅ Agregar `using Microsoft.AspNetCore.Authorization;`
2. ✅ Agregar `[Authorize(Roles = "Piloto")]` antes de la clase
3. ✅ Agregar `using System.Security.Claims;`
4. ✅ Agregar `using Microsoft.EntityFrameworkCore;`
5. ✅ Agregar `using TransportesGenesis.Data.Context;`
6. ✅ Inyectar `ApplicationDbContext` en el constructor
7. ✅ Cambiar `IdBus = 4` por consulta a `AsignacionPilotoBusDb`
8. ✅ Agregar validación si no tiene bus asignado

### ✅ **1.5 Tablas de Base de Datos a Reutilizar**

**Tabla Principal:** `genesis.AsignacionPilotoBus`
```sql
-- ✅ ESTA TABLA YA EXISTE EN LA BASE DE DATOS
CREATE TABLE genesis.AsignacionPilotoBus (
    IdAsignacion INT PRIMARY KEY IDENTITY(1,1),
    IdUsuarioPiloto NVARCHAR(450) NOT NULL,  -- FK a AspNetUsers.Id
    IdBus INT NOT NULL,                      -- FK a Buses
    FechaAsignacion DATETIME2 NOT NULL,
    FechaFinAsignacion DATETIME2 NULL,
    EsActual BIT NOT NULL DEFAULT 1,         -- ✅ Indicador de asignación activa
    Activo INT NOT NULL,
    FechaRegistro DATETIME2 NOT NULL
);
```

**Uso:**
- ✅ Para **Piloto:** Determinar qué bus maneja
- ✅ Para **Monitor:** Determinar en qué bus viaja (mismo bus que el piloto)

---

**Tabla Secundaria:** `genesis.RegistroRecogida`
```sql
-- ✅ ESTA TABLA YA EXISTE EN LA BASE DE DATOS
CREATE TABLE genesis.RegistroRecogida (
    IdRegistro INT PRIMARY KEY IDENTITY(1,1),
    IdParada INT NOT NULL,                   -- FK a Paradas
    IdAlumno INT NOT NULL,                   -- FK a Alumnos
    FechaHoraRecogida DATETIME2 NOT NULL,
    ConfirmadoPor NVARCHAR(450) NULL,       -- ✅ FK a AspNetUsers.Id (Monitor/Piloto)
    Latitud DECIMAL(10,7) NULL,
    Longitud DECIMAL(10,7) NULL,
    AlumnoPresente BIT NOT NULL DEFAULT 1,   -- ✅ true = recogido, false = ausente
    Activo INT NOT NULL,
    FechaRegistro DATETIME2 NOT NULL
);
```

**Uso:**
- ✅ Para **Monitor:** Registrar qué alumnos fueron recogidos en cada parada
- ✅ Campo `ConfirmadoPor`: Guardar el ID del monitor que registró
- ✅ Campo `AlumnoPresente`: true = alumno abordó, false = alumno no estaba

---

**Tablas Relacionadas:**
```
genesis.Rutas          → Rutas asignadas a buses
genesis.Paradas        → Paradas de cada ruta
genesis.Alumnos        → Información de alumnos
genesis.Buses          → Información de buses
AspNetUsers            → Usuarios del sistema (Piloto/Monitor)
```

**✅ CONCLUSIÓN:**
- ✅ NO se necesitan crear tablas nuevas
- ✅ Se reutilizan las tablas existentes
- ✅ `AsignacionPilotoBus` sirve tanto para Piloto como para Monitor

---

---

## 🛠️ FASE 2: PLAN DE IMPLEMENTACIÓN DETALLADO

**Orden de Ejecución:** Los módulos deben implementarse en este orden específico para evitar errores.

---

## 📦 **MÓDULO 1: CORREGIR ÁREA DE PILOTO EXISTENTE**

**Estado Actual:** 70% completo - Falta autorización y obtener bus dinámicamente

### ✅ **Tarea 1.1: Agregar Autorización a MiRuta**

**Archivo:** `Pages/Piloto/MiRuta.cshtml.cs`

**Acción:** Agregar atributo de autorización

**ANTES:**
```csharp
public class MiRutaModel : PageModel
```

**DESPUÉS:**
```csharp
[Authorize(Roles = "Piloto")]
public class MiRutaModel : PageModel
```

---

### ✅ **1.2 Obtener Bus Asignado del Piloto**

**Problema actual:** En `MiRuta.cshtml.cs` el IdBus está hardcodeado:
```csharp
public int IdBus { get; set; } = 4; // TODO: Obtener del usuario autenticado
```

**Solución:** Crear un servicio o método para obtener el bus asignado

**Opción A: Agregar método en un servicio existente**

**Archivo a crear/modificar:** `Services/Interfaces/IBusService.cs`

Agregar método:
```csharp
Task<int?> ObtenerBusAsignadoAPilotoAsync(string idUsuarioPiloto);
```

**Archivo a modificar:** `Services/Implementations/BusService.cs`

Implementar:
```csharp
public async Task<int?> ObtenerBusAsignadoAPilotoAsync(string idUsuarioPiloto)
{
    var asignacion = await _context.AsignacionesPilotoBusDb
        .Where(a => a.IdUsuarioPiloto == idUsuarioPiloto && a.EsActual && a.Activo == 1)
        .FirstOrDefaultAsync();

    return asignacion?.Bus?.IdBus;
}
```

**Opción B: Consulta directa en el PageModel**

En `MiRuta.cshtml.cs`, agregar:
```csharp
private readonly ApplicationDbContext _context;

// En OnGetAsync():
var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
var asignacion = await _context.AsignacionesPilotoBusDb
    .Where(a => a.IdUsuarioPiloto == userId && a.EsActual)
    .Include(a => a.Bus)
    .FirstOrDefaultAsync();

if (asignacion != null)
{
    IdBus = asignacion.Bus.IdBus;
    NombrePiloto = User.Identity.Name;
}
else
{
    MensajeError = "No tienes un bus asignado actualmente. Contacta al administrador.";
    return;
}
```

**RECOMENDACIÓN:** Usar Opción B (más directo y menos dependencias)

---

### ✅ **1.3 Actualizar Redirección en AuthController**

**Archivo:** `Controllers/AuthController.cs`

**Línea actual (78):**
```csharp
else if (roles.Contains("Piloto"))
    return RedirectToPage("/Piloto/MiRuta");
```

**✅ ESTA LÍNEA YA EXISTE - NO REQUIERE CAMBIOS**

---

## 📦 **MÓDULO 2: ÁREA DE MONITOR**

### ✅ **2.1 Crear Carpeta y Archivos Base**

**Estructura a crear:**
```
Pages/Monitor/
├── MiRuta.cshtml
├── MiRuta.cshtml.cs
├── RegistrarRecogidas.cshtml
└── RegistrarRecogidas.cshtml.cs
```

---

### ✅ **2.2 Crear Página: Monitor/MiRuta**

**Propósito:** Mostrar la ruta asignada al monitor (similar a Piloto)

**Archivo:** `Pages/Monitor/MiRuta.cshtml.cs`

**Contenido base:**
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TransportesGenesis.Data.Context;
using TransportesGenesis.DTOs.Ruta;

namespace TransportesGenesis.Pages.Monitor
{
    [Authorize(Roles = "Monitor")]
    public class MiRutaModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public MiRutaModel(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public RutaDto? RutaActiva { get; set; }
        public int IdBus { get; set; }
        public string TipoRuta { get; set; } = "Tarde"; // Monitores generalmente en turno tarde
        public string MensajeError { get; set; } = string.Empty;
        public string NombreMonitor { get; set; } = string.Empty;
        public DateTime FechaRuta { get; set; }
        public bool EsFinDeSemana { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // Obtener ID del usuario autenticado
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                NombreMonitor = User.Identity?.Name ?? "Monitor";

                // Buscar bus asignado al monitor (similar a piloto)
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
                FechaRuta = DateTime.Now.Date;
                EsFinDeSemana = DateTime.Now.DayOfWeek == DayOfWeek.Saturday || 
                               DateTime.Now.DayOfWeek == DayOfWeek.Sunday;

                // Determinar turno según hora actual
                var horaActual = DateTime.Now.Hour;
                TipoRuta = horaActual < 12 ? "Mañana" : "Tarde";

                // Llamar a la API para obtener la ruta activa
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri($"{Request.Scheme}://{Request.Host}");

                var response = await client.GetAsync($"/api/rutas/bus/{IdBus}/activa?tipoRuta={TipoRuta}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(content, 
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        RutaActiva = JsonSerializer.Deserialize<RutaDto>(
                            apiResponse.Data.ToString()!, 
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    else
                    {
                        MensajeError = apiResponse?.Message ?? 
                            "No hay ruta calculada para este bus en el turno actual.";
                    }
                }
                else
                {
                    MensajeError = "Error al obtener la ruta del servidor.";
                }
            }
            catch (Exception ex)
            {
                MensajeError = $"Error inesperado: {ex.Message}";
            }
        }
    }
}
```

**Archivo:** `Pages/Monitor/MiRuta.cshtml`

**Contenido:** Similar a `Pages/Piloto/MiRuta.cshtml` pero con título "Mi Ruta - Monitor"

---

### ✅ **2.3 Crear Página: Monitor/RegistrarRecogidas**

**Propósito:** Permitir al monitor registrar qué alumnos fueron recogidos en cada parada

**Archivo:** `Pages/Monitor/RegistrarRecogidas.cshtml.cs`

**Contenido base:**
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Pages.Monitor
{
    [Authorize(Roles = "Monitor")]
    public class RegistrarRecogidasModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RegistrarRecogidasModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ParadaConAlumnos> Paradas { get; set; } = new();
        public string TipoRuta { get; set; } = "Tarde";
        public string MensajeError { get; set; } = string.Empty;
        public string MensajeExito { get; set; } = string.Empty;
        public int IdBus { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                // Obtener bus asignado al monitor
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var asignacion = await _context.AsignacionesPilotoBusDb
                    .Where(a => a.IdUsuarioPiloto == userId && a.EsActual && a.Activo == 1)
                    .Include(a => a.Bus)
                    .FirstOrDefaultAsync();

                if (asignacion == null)
                {
                    MensajeError = "No tienes un bus asignado.";
                    return;
                }

                IdBus = asignacion.Bus.IdBus;

                // Determinar turno
                var horaActual = DateTime.Now.Hour;
                TipoRuta = horaActual < 12 ? "Mañana" : "Tarde";

                // Obtener ruta activa del bus
                var ruta = await _context.RutasDb
                    .Where(r => r.IdBus == IdBus && r.TipoRuta == TipoRuta && r.EsActiva)
                    .Include(r => r.ParadasLink)
                        .ThenInclude(p => p.Alumno)
                    .FirstOrDefaultAsync();

                if (ruta == null)
                {
                    MensajeError = "No hay ruta activa para este bus en el turno actual.";
                    return;
                }

                // Cargar paradas con alumnos
                var fechaHoy = DateTime.Now.Date;
                Paradas = ruta.ParadasLink
                    .Where(p => p.IdAlumno.HasValue)
                    .OrderBy(p => p.Orden)
                    .Select(p => new ParadaConAlumnos
                    {
                        IdParada = p.IdParada,
                        IdAlumno = p.IdAlumno.Value,
                        NombreAlumno = $"{p.Alumno.Nombre} {p.Alumno.Apellido}",
                        Direccion = p.Direccion ?? "Sin dirección",
                        Orden = p.Orden,
                        // Verificar si ya fue recogido hoy
                        FueRecogido = _context.RegistrosRecogidaDb.Any(r => 
                            r.IdParada == p.IdParada && 
                            r.IdAlumno == p.IdAlumno.Value &&
                            r.FechaHoraRecogida.Date == fechaHoy)
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                MensajeError = $"Error: {ex.Message}";
            }
        }

        public async Task<IActionResult> OnPostMarcarRecogidalAsync(int idParada, int idAlumno, bool presente)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Verificar si ya existe un registro para hoy
                var fechaHoy = DateTime.Now.Date;
                var registroExistente = await _context.RegistrosRecogidaDb
                    .FirstOrDefaultAsync(r => 
                        r.IdParada == idParada && 
                        r.IdAlumno == idAlumno &&
                        r.FechaHoraRecogida.Date == fechaHoy);

                if (registroExistente != null)
                {
                    // Actualizar registro existente
                    registroExistente.AlumnoPresente = presente;
                    registroExistente.FechaHoraRecogida = DateTime.Now;
                    registroExistente.ConfirmadoPor = userId;
                }
                else
                {
                    // Crear nuevo registro
                    var nuevoRegistro = new RegistroRecogida
                    {
                        IdParada = idParada,
                        IdAlumno = idAlumno,
                        FechaHoraRecogida = DateTime.Now,
                        AlumnoPresente = presente,
                        ConfirmadoPor = userId,
                        Activo = 1,
                        FechaRegistro = DateTime.Now
                    };

                    _context.RegistrosRecogidaDb.Add(nuevoRegistro);
                }

                await _context.SaveChangesAsync();

                TempData["MensajeExito"] = presente 
                    ? "Alumno marcado como recogido" 
                    : "Alumno marcado como ausente";

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = $"Error al registrar: {ex.Message}";
                return RedirectToPage();
            }
        }

        // Clase auxiliar para vista
        public class ParadaConAlumnos
        {
            public int IdParada { get; set; }
            public int IdAlumno { get; set; }
            public string NombreAlumno { get; set; } = string.Empty;
            public string Direccion { get; set; } = string.Empty;
            public int Orden { get; set; }
            public bool FueRecogido { get; set; }
        }
    }
}
```

**Archivo:** `Pages/Monitor/RegistrarRecogidas.cshtml`

**Contenido base:**
```html
@page
@model TransportesGenesis.Pages.Monitor.RegistrarRecogidasModel
@{
    ViewData["Title"] = "Registrar Recogidas";
}

<div class="container mt-4">
    <h2>📋 Registrar Recogidas - Turno @Model.TipoRuta</h2>

    @if (!string.IsNullOrEmpty(Model.MensajeError))
    {
        <div class="alert alert-danger">@Model.MensajeError</div>
    }

    @if (!string.IsNullOrEmpty(Model.MensajeExito))
    {
        <div class="alert alert-success">@Model.MensajeExito</div>
    }

    @if (!string.IsNullOrEmpty(TempData["MensajeExito"] as string))
    {
        <div class="alert alert-success">@TempData["MensajeExito"]</div>
    }

    @if (!string.IsNullOrEmpty(TempData["MensajeError"] as string))
    {
        <div class="alert alert-danger">@TempData["MensajeError"]</div>
    }

    @if (Model.Paradas.Any())
    {
        <div class="card">
            <div class="card-header bg-primary text-white">
                <h5>Alumnos en la Ruta</h5>
            </div>
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Alumno</th>
                                <th>Dirección</th>
                                <th>Estado</th>
                                <th>Acciones</th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var parada in Model.Paradas)
                            {
                                <tr class="@(parada.FueRecogido ? "table-success" : "")">
                                    <td>@parada.Orden</td>
                                    <td>@parada.NombreAlumno</td>
                                    <td>@parada.Direccion</td>
                                    <td>
                                        @if (parada.FueRecogido)
                                        {
                                            <span class="badge bg-success">✓ Recogido</span>
                                        }
                                        else
                                        {
                                            <span class="badge bg-warning">Pendiente</span>
                                        }
                                    </td>
                                    <td>
                                        <form method="post" asp-page-handler="MarcarRecogida" class="d-inline">
                                            <input type="hidden" name="idParada" value="@parada.IdParada" />
                                            <input type="hidden" name="idAlumno" value="@parada.IdAlumno" />
                                            <input type="hidden" name="presente" value="true" />
                                            <button type="submit" class="btn btn-sm btn-success" 
                                                    @(parada.FueRecogido ? "disabled" : "")>
                                                ✓ Presente
                                            </button>
                                        </form>
                                        <form method="post" asp-page-handler="MarcarRecogida" class="d-inline">
                                            <input type="hidden" name="idParada" value="@parada.IdParada" />
                                            <input type="hidden" name="idAlumno" value="@parada.IdAlumno" />
                                            <input type="hidden" name="presente" value="false" />
                                            <button type="submit" class="btn btn-sm btn-danger">
                                                ✗ Ausente
                                            </button>
                                        </form>
                                    </td>
                                </tr>
                            }
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    }
    else
    {
        <div class="alert alert-info">
            No hay alumnos en la ruta actual.
        </div>
    }
</div>
```

---

### ✅ **2.4 Agregar Redirección en AuthController**

**Archivo:** `Controllers/AuthController.cs`

**Líneas actuales (60-82):**
```csharp
var roles = await _userManager.GetRolesAsync(user);

if (roles.Contains("Administrador"))
{
    return RedirectToAction("Index", "Admin");
}
else if (roles.Contains("PadreDeFamilia"))
{
    return RedirectToAction("Index", "PagosPadresFamilia");
}
else if (roles.Contains("Piloto"))
{
    return RedirectToPage("/Piloto/MiRuta");
}
else
{
    return RedirectToAction("Index", "Home");
}
```

**AGREGAR después de la condición del Piloto:**
```csharp
else if (roles.Contains("Monitor"))
{
    return RedirectToPage("/Monitor/MiRuta");
}
```

**Código completo después del cambio:**
```csharp
var roles = await _userManager.GetRolesAsync(user);

if (roles.Contains("Administrador"))
{
    return RedirectToAction("Index", "Admin");
}
else if (roles.Contains("PadreDeFamilia"))
{
    return RedirectToAction("Index", "PagosPadresFamilia");
}
else if (roles.Contains("Piloto"))
{
    return RedirectToPage("/Piloto/MiRuta");
}
else if (roles.Contains("Monitor"))
{
    return RedirectToPage("/Monitor/MiRuta");
}
else
{
    return RedirectToAction("Index", "Home");
}
```

---

## 📦 **MÓDULO 3: CREAR USUARIOS DE PRUEBA**

### ✅ **3.1 Script SQL para Crear Usuarios**

**Opción A: Via SQL Server Management Studio**

```sql
-- ========================================
-- SCRIPT: Crear Usuarios Piloto y Monitor
-- ========================================

USE TransportesGenesis; -- Ajustar nombre de base de datos
GO

-- 1. Crear Usuario Piloto
DECLARE @PilotoUserId NVARCHAR(450) = NEWID();
DECLARE @PilotoPasswordHash NVARCHAR(MAX);

-- Password hash para "Piloto123!" (debe generarse con Identity)
-- Por simplicidad, usar el mismo hash del admin o generar uno nuevo

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Discriminator, IsFirstLogin, LastLoginDate)
VALUES (
    @PilotoUserId,
    'piloto1',
    'PILOTO1',
    'piloto1@transportesgenesis.com',
    'PILOTO1@TRANSPORTESGENESIS.COM',
    1,
    'AQAAAAIAAYagAAAAEBt8...', -- Generar hash válido
    NEWID(),
    NEWID(),
    NULL,
    0,
    0,
    NULL,
    1,
    0,
    'AppUser',
    0,
    GETDATE()
);

-- Asignar rol Piloto
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT @PilotoUserId, Id FROM AspNetRoles WHERE Name = 'Piloto';

-- 2. Crear Usuario Monitor
DECLARE @MonitorUserId NVARCHAR(450) = NEWID();

INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount, Discriminator, IsFirstLogin, LastLoginDate)
VALUES (
    @MonitorUserId,
    'monitor1',
    'MONITOR1',
    'monitor1@transportesgenesis.com',
    'MONITOR1@TRANSPORTESGENESIS.COM',
    1,
    'AQAAAAIAAYagAAAAEBt8...', -- Usar mismo hash
    NEWID(),
    NEWID(),
    NULL,
    0,
    0,
    NULL,
    1,
    0,
    'AppUser',
    0,
    GETDATE()
);

-- Asignar rol Monitor
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT @MonitorUserId, Id FROM AspNetRoles WHERE Name = 'Monitor';

-- 3. Asignar buses a Piloto y Monitor
-- (Asumiendo que existen buses con IdBus = 1 y 2)

INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, FechaFinAsignacion, EsActual, Activo, FechaRegistro)
VALUES 
(@PilotoUserId, 1, GETDATE(), NULL, 1, 1, GETDATE()),
(@MonitorUserId, 1, GETDATE(), NULL, 1, 1, GETDATE());

GO
```

---

### ✅ **3.2 Método Programático (Recomendado)**

**Crear un endpoint temporal para crear usuarios de prueba**

**Archivo:** `Controllers/AdminController.cs` (o crear nuevo controlador)

```csharp
[Authorize(Roles = "Administrador")]
public class AdminController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminController(UserManager<AppUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // GET: /Admin/CrearUsuariosPrueba
    [HttpGet]
    public async Task<IActionResult> CrearUsuariosPrueba()
    {
        try
        {
            // Crear Piloto
            var piloto = await _userManager.FindByEmailAsync("piloto1@transportesgenesis.com");
            if (piloto == null)
            {
                piloto = new AppUser
                {
                    UserName = "piloto1",
                    Email = "piloto1@transportesgenesis.com",
                    EmailConfirmed = true,
                    IsFirstLogin = false,
                    LastLoginDate = DateTime.Now
                };

                var resultPiloto = await _userManager.CreateAsync(piloto, "Piloto123!");
                if (resultPiloto.Succeeded)
                {
                    await _userManager.AddToRoleAsync(piloto, "Piloto");

                    // Asignar bus al piloto (asumiendo que existe IdBus = 1)
                    var asignacionPiloto = new AsignacionPilotoBus
                    {
                        IdUsuarioPiloto = piloto.Id,
                        IdBus = 1,
                        FechaAsignacion = DateTime.Now,
                        EsActual = true,
                        Activo = 1,
                        FechaRegistro = DateTime.Now
                    };
                    _context.AsignacionesPilotoBusDb.Add(asignacionPiloto);
                }
            }

            // Crear Monitor
            var monitor = await _userManager.FindByEmailAsync("monitor1@transportesgenesis.com");
            if (monitor == null)
            {
                monitor = new AppUser
                {
                    UserName = "monitor1",
                    Email = "monitor1@transportesgenesis.com",
                    EmailConfirmed = true,
                    IsFirstLogin = false,
                    LastLoginDate = DateTime.Now
                };

                var resultMonitor = await _userManager.CreateAsync(monitor, "Monitor123!");
                if (resultMonitor.Succeeded)
                {
                    await _userManager.AddToRoleAsync(monitor, "Monitor");

                    // Asignar bus al monitor (mismo bus que piloto)
                    var asignacionMonitor = new AsignacionPilotoBus
                    {
                        IdUsuarioPiloto = monitor.Id,
                        IdBus = 1,
                        FechaAsignacion = DateTime.Now,
                        EsActual = true,
                        Activo = 1,
                        FechaRegistro = DateTime.Now
                    };
                    _context.AsignacionesPilotoBusDb.Add(asignacionMonitor);
                }
            }

            await _context.SaveChangesAsync();

            return Content("Usuarios de prueba creados exitosamente:\\n" +
                          "Piloto: piloto1@transportesgenesis.com / Piloto123!\\n" +
                          "Monitor: monitor1@transportesgenesis.com / Monitor123!");
        }
        catch (Exception ex)
        {
            return Content($"Error: {ex.Message}");
        }
    }
}
```

**Uso:** Navegar a `/Admin/CrearUsuariosPrueba` estando logueado como Admin

---

## 📦 **MÓDULO 4: NAVEGACIÓN Y MENÚS**

### ✅ **4.1 Agregar Links de Navegación**

**Archivo:** `Views/Shared/_Layout.cshtml` o `Pages/Shared/_Layout.cshtml`

**Agregar en el menú de navegación:**

```html
@if (User.IsInRole("Piloto"))
{
    <li class="nav-item">
        <a class="nav-link" asp-page="/Piloto/MiRuta">Mi Ruta</a>
    </li>
}

@if (User.IsInRole("Monitor"))
{
    <li class="nav-item">
        <a class="nav-link" asp-page="/Monitor/MiRuta">Mi Ruta</a>
    </li>
    <li class="nav-item">
        <a class="nav-link" asp-page="/Monitor/RegistrarRecogidas">Registrar Recogidas</a>
    </li>
}
```

---

## 📊 **RESUMEN DE CAMBIOS POR ARCHIVO**

### ✅ **Archivos a CREAR:**

1. ✅ `Pages/Monitor/MiRuta.cshtml`
2. ✅ `Pages/Monitor/MiRuta.cshtml.cs`
3. ✅ `Pages/Monitor/RegistrarRecogidas.cshtml`
4. ✅ `Pages/Monitor/RegistrarRecogidas.cshtml.cs`

### ✅ **Archivos a MODIFICAR:**

5. ✅ `Pages/Piloto/MiRuta.cshtml.cs` - Agregar `[Authorize(Roles = "Piloto")]` y lógica para obtener bus asignado
6. ✅ `Controllers/AuthController.cs` - Agregar redirección para rol "Monitor"
7. ✅ `Views/Shared/_Layout.cshtml` - Agregar links de navegación para Piloto y Monitor

### ✅ **Archivos OPCIONALES (recomendados):**

8. ✅ `Controllers/AdminController.cs` - Método para crear usuarios de prueba

---

## 🎯 **ORDEN DE EJECUCIÓN RECOMENDADO**

### **PASO 1: Preparar Base de Datos**
1. ✅ Ejecutar migraciones si hay pendientes: `Update-Database`
2. ✅ Verificar que existan roles en AspNetRoles: Piloto, Monitor
3. ✅ Verificar que exista al menos 1 bus en `genesis.Buses`

### **PASO 2: Crear Usuarios de Prueba**
4. ✅ Crear método `CrearUsuariosPrueba` en AdminController
5. ✅ Navegar a `/Admin/CrearUsuariosPrueba` como Admin
6. ✅ Verificar usuarios creados en AspNetUsers y AspNetUserRoles

### **PASO 3: Proteger Páginas de Piloto**
7. ✅ Modificar `Pages/Piloto/MiRuta.cshtml.cs`
   - Agregar `[Authorize(Roles = "Piloto")]`
   - Implementar lógica para obtener bus asignado del usuario autenticado

### **PASO 4: Crear Páginas de Monitor**
8. ✅ Crear carpeta `Pages/Monitor/`
9. ✅ Crear `MiRuta.cshtml` y `MiRuta.cshtml.cs`
10. ✅ Crear `RegistrarRecogidas.cshtml` y `RegistrarRecogidas.cshtml.cs`

### **PASO 5: Actualizar Redirección**
11. ✅ Modificar `Controllers/AuthController.cs`
    - Agregar condición para rol "Monitor"

### **PASO 6: Actualizar Navegación**
12. ✅ Modificar `_Layout.cshtml`
    - Agregar links condicionales para Piloto y Monitor

### **PASO 7: Pruebas**
13. ✅ Probar login como Piloto
    - Verificar redirección a `/Piloto/MiRuta`
    - Verificar que se muestre el bus asignado
14. ✅ Probar login como Monitor
    - Verificar redirección a `/Monitor/MiRuta`
    - Verificar acceso a `/Monitor/RegistrarRecogidas`
    - Probar registro de recogidas

---

## 🔒 **SEGURIDAD Y MEJORES PRÁCTICAS**

### ✅ **Validaciones Requeridas:**

1. ✅ **Autorización por Roles:** Todas las páginas deben tener `[Authorize(Roles = "RolEspecifico")]`
2. ✅ **Verificar Bus Asignado:** Siempre validar que el usuario tenga un bus asignado activo
3. ✅ **Validar Fecha:** No permitir registros retroactivos de más de 1 día
4. ✅ **Validar Duplicados:** Evitar registrar la misma recogida múltiples veces
5. ✅ **Logs:** Registrar intentos de acceso no autorizados

### ✅ **Manejo de Errores:**

1. ✅ Mostrar mensajes claros al usuario
2. ✅ Registrar excepciones en logs del sistema
3. ✅ Redirigir a página de error en caso de fallos críticos

---

## 📝 **DATOS DE PRUEBA RECOMENDADOS**

### ✅ **Usuarios:**
```
Piloto:
- Email: piloto1@transportesgenesis.com
- Password: Piloto123!
- Rol: Piloto
- Bus Asignado: IdBus = 1

Monitor:
- Email: monitor1@transportesgenesis.com
- Password: Monitor123!
- Rol: Monitor
- Bus Asignado: IdBus = 1 (mismo que piloto)
```

### ✅ **Datos Relacionados Necesarios:**
- ✅ Al menos 1 bus en `genesis.Buses` (IdBus = 1)
- ✅ Al menos 1 ruta calculada para ese bus
- ✅ Al menos 3 paradas con alumnos asignados
- ✅ Alumnos con asistencia confirmada para el día

---

## 🎉 **RESULTADO ESPERADO**

Al completar este plan:

1. ✅ **Piloto podrá:**
   - Iniciar sesión con su email/password
   - Ser redirigido automáticamente a `/Piloto/MiRuta`
   - Ver su ruta asignada con paradas y alumnos
   - Ver información en tiempo real del bus

2. ✅ **Monitor podrá:**
   - Iniciar sesión con su email/password
   - Ser redirigido automáticamente a `/Monitor/MiRuta`
   - Ver la ruta asignada
   - Acceder a `/Monitor/RegistrarRecogidas`
   - Marcar alumnos como "Presente" o "Ausente" en cada parada
   - Ver el historial de recogidas del día

3. ✅ **Sistema garantiza:**
   - Acceso restringido por roles
   - Redirección automática según el rol
   - Auditoría de recogidas (quién, cuándo, dónde)
   - Interfaz clara y fácil de usar

---

## 📚 **REFERENCIAS DE CÓDIGO EXISTENTE**

- **Login:** `Controllers/AuthController.cs`
- **Roles:** `Startup.cs` (línea 143)
- **Páginas con autorización:** `Pages/Padres/DashboardRutaBusAsignado.cshtml.cs`
- **Asignación de buses:** `Models/DB/Negocio/AsignacionPilotoBus.cs`
- **Registro de recogidas:** `Models/DB/Negocio/RegistroRecogida.cs`
- **Página de Piloto existente:** `Pages/Piloto/MiRuta.cshtml.cs`

---

## ⚠️ **NOTAS FINALES**

1. ✅ **NO modificar migraciones existentes**
2. ✅ **NO alterar la lógica de login de PadreDeFamilia**
3. ✅ **Usar los servicios existentes siempre que sea posible**
4. ✅ **Seguir las convenciones de nombres del proyecto**
5. ✅ **Probar cada módulo antes de continuar con el siguiente**
6. ✅ **Hacer commits por módulo completado**

---

## 🚀 **PRÓXIMOS PASOS OPCIONALES (MEJORAS FUTURAS)**

1. ⭐ Agregar notificaciones en tiempo real (SignalR ya está configurado)
2. ⭐ Agregar historial de recogidas por fecha
3. ⭐ Permitir al monitor tomar foto/geolocalización al registrar
4. ⭐ Dashboard de estadísticas para monitor
5. ⭐ Exportar reportes de asistencia en PDF/Excel

---

**FIN DEL PLAN DETALLADO**

**Fecha de Creación:** 2026-05-08  
**Autor:** GitHub Copilot  
**Estado:** ✅ LISTO PARA IMPLEMENTACIÓN
