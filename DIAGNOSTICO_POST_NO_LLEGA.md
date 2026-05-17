# 🔍 DIAGNÓSTICO: El POST no llega al servidor

## 📊 Estado Actual

### ✅ Lo que SÍ funciona:
- La aplicación arranca correctamente
- El GET de `/Admin/GestionarAsignaciones` carga bien
- Las consultas EF traen datos de la base de datos
- Los dropdowns se rellenan correctamente
- El filtro `IdBus NOT IN (1, 4)` funciona

### ❌ Lo que NO funciona:
- El POST no entra al método `OnPostAsync()`
- No aparece el log `=== INICIO POST CrearAsignacion ===`
- No hay señales en Visual Studio de que el request llegue

---

## 🧪 Pruebas a Realizar

### 1️⃣ Verifica la Pestaña Network en DevTools

1. Abre el navegador (Chrome/Edge)
2. Presiona **F12** para abrir DevTools
3. Ve a la pestaña **"Network"** (Red)
4. Intenta crear una asignación
5. Busca el request **POST**
6. Verifica:
   - **Status Code**: ¿Es 400, 404, 500?
   - **Request URL**: ¿A qué URL se está enviando?
   - **Form Data**: ¿Qué valores se están enviando?
   - **Response**: ¿Qué devuelve el servidor?

### 2️⃣ Verifica la Consola del Navegador

1. En DevTools, ve a la pestaña **"Console"**
2. Busca errores en JavaScript
3. ¿Hay algún mensaje de CORS?
4. ¿Hay algún error de antiforgery token?

### 3️⃣ Verifica la Salida de Visual Studio

1. Ve a **View > Output** (o presiona `Ctrl+Alt+O`)
2. Selecciona **"Debug"** en el dropdown
3. Intenta crear la asignación
4. Busca el mensaje:
   ```
   ========================================
   POST RECIBIDO - INICIO
   ========================================
   ```

Si aparece este mensaje, **el POST SÍ está llegando**.

---

## 🛠️ Soluciones Alternativas

### Opción 1: Crear un método POST más simple

Reemplaza `OnPostAsync()` con esto temporalmente:

```csharp
[HttpPost]
public IActionResult OnPost()
{
    Console.WriteLine("POST SIMPLE RECIBIDO");
    _logger.LogInformation("POST SIMPLE RECIBIDO");

    return Content("POST RECIBIDO CORRECTAMENTE");
}
```

Si esto funciona, el problema está en la lógica del método original.

### Opción 2: Usar JavaScript para enviar el formulario

Reemplaza el `<form>` con este código:

```html
<form id="formAsignacion">
    <!-- ... (mismo contenido) ... -->
    <button type="button" onclick="enviarFormulario()" class="btn btn-success w-100">
        <i class="bi bi-check-circle"></i> Asignar
    </button>
</form>

<script>
function enviarFormulario() {
    const idUsuario = document.querySelector('[name="IdUsuario"]').value;
    const idBus = document.querySelector('[name="IdBus"]').value;

    console.log('Enviando:', { idUsuario, idBus });

    fetch('/Admin/GestionarAsignaciones', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ IdUsuario: idUsuario, IdBus: idBus })
    })
    .then(response => {
        console.log('Respuesta:', response);
        return response.text();
    })
    .then(html => {
        console.log('HTML recibido:', html.substring(0, 200));
        window.location.reload();
    })
    .catch(error => {
        console.error('Error:', error);
        alert('Error al crear asignación: ' + error.message);
    });
}
</script>
```

### Opción 3: Crear un Controller en lugar de Razor Pages

Si Razor Pages sigue dando problemas, podemos crear un `AdminAsignacionesController`:

```csharp
[Authorize(Roles = "Administrador")]
[Route("Admin/[controller]")]
public class AsignacionesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public AsignacionesController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("Crear")]
    public async Task<IActionResult> Crear(string idUsuario, int idBus)
    {
        Console.WriteLine($"POST RECIBIDO: {idUsuario}, {idBus}");

        // ... lógica de creación ...

        return RedirectToAction("Index");
    }
}
```

---

## 🔎 Posibles Causas

### 1. Routing de Razor Pages
- Razor Pages tiene un sistema de routing específico
- El `asp-for` puede estar fallando en el binding
- La URL generada puede estar incorrecta

### 2. Antiforgery Token
- Aunque usamos `[IgnoreAntiforgeryToken]`, puede haber otro middleware bloqueando

### 3. Middleware en Startup.cs
- Algún middleware puede estar interceptando el request
- Verifica el orden de middlewares en `Startup.cs` o `Program.cs`

### 4. IIS Express vs Kestrel
- Puede haber diferencias entre los dos servers
- Prueba con `dotnet run` desde PowerShell

---

## 📝 Siguiente Paso

**Por favor, ejecuta estas verificaciones:**

1. ✅ Abre Network en DevTools
2. ✅ Intenta crear asignación
3. ✅ Toma screenshot del request POST
4. ✅ Comparte el **Request URL** y **Status Code**
5. ✅ Comparte los **Form Data** enviados

Con esa información podré identificar exactamente dónde está fallando.

---

**Fecha:** 2025-01-XX  
**Estado:** 🔍 En diagnóstico  
**Prioridad:** 🔴 Alta
