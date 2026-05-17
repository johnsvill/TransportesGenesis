# 🔍 DEBUG: Error HTTP 400 en Gestionar Asignaciones

**Fecha**: Diciembre 2024  
**Error**: HTTP 400 Bad Request al intentar crear asignación  
**Estado**: 🔧 LOGGING AGREGADO PARA DIAGNÓSTICO

---

## 🐛 PROBLEMA REPORTADO

### **Error en consola del navegador**:
```
chrome-error://chromewebdata/:1  
POST https://localhost:7241/Admin/GestionarAsignaciones 
net::ERR_HTTP_RESPONSE_CODE_FAILURE 400 (Bad Request)
```

### **Síntomas**:
- ❌ Al intentar asignar piloto/monitor → bus
- ❌ El POST falla con HTTP 400
- ❌ La página muestra "Ahora mismo esta página no está disponible"

---

## 🔧 SOLUCIONES IMPLEMENTADAS

### **1. Logging Detallado Agregado**

Se agregó logging exhaustivo en `OnPostCrearAsignacionAsync()` para capturar:

```csharp
_logger.LogInformation("=== INICIO POST CrearAsignacion ===");
_logger.LogInformation($"IdUsuario recibido: '{IdUsuario}'");
_logger.LogInformation($"IdBus recibido: {IdBus}");
_logger.LogInformation($"ModelState.IsValid: {ModelState.IsValid}");

// Log de errores de ModelState si existen
if (!ModelState.IsValid)
{
    foreach (var modelState in ModelState.Values)
    {
        foreach (var error in modelState.Errors)
        {
            _logger.LogError($"ModelState Error: {error.ErrorMessage}");
        }
    }
}
```

**Logs capturados**:
- ✅ Valores de `IdUsuario` e `IdBus` recibidos
- ✅ Estado de `ModelState.IsValid`
- ✅ Errores de validación del modelo
- ✅ Flujo completo de validaciones
- ✅ Errores de base de datos si ocurren

---

### **2. Corrección de Tipos Nullables**

**Cambio realizado**:
```csharp
// ANTES (incorrecto)
public string IdUsuario { get; set; } = string.Empty;

// DESPUÉS (correcto)
public string? IdUsuario { get; set; }
```

**Razón**: ASP.NET Core puede tener problemas de binding si el tipo no admite null y el valor viene vacío.

---

### **3. Manejo de Errores Mejorado**

Ahora cada paso está envuelto en try-catch con logging específico:

```csharp
try
{
    _logger.LogInformation($"Verificando asignación existente para usuario: {IdUsuario}");
    // Validaciones...

    _logger.LogInformation("Validaciones pasadas, creando asignación...");
    // Creación...

    _logger.LogInformation($"✅ Asignación creada exitosamente");
}
catch (Exception ex)
{
    _logger.LogError(ex, $"❌ Error al crear asignación. IdUsuario: {IdUsuario}, IdBus: {IdBus}");
    // Mostrar error al usuario...
}
```

---

## 🧪 CÓMO DEPURAR EL ERROR

### **Paso 1: Reiniciar la aplicación**

1. **Detener la app** (Shift + F5)
2. **Iniciar en modo Debug** (F5)
3. Los logs ahora aparecerán en la ventana **Output** de Visual Studio

---

### **Paso 2: Reproducir el error y capturar logs**

1. Ir a `/Admin/GestionarAsignaciones`
2. Seleccionar un piloto/monitor
3. Seleccionar un bus
4. Clic en **"Asignar"**
5. **Observar la ventana Output** en Visual Studio

---

### **Paso 3: Analizar los logs**

Busca en la ventana Output los logs que comienzan con:

```
=== INICIO POST CrearAsignacion ===
IdUsuario recibido: '...'
IdBus recibido: ...
ModelState.IsValid: ...
```

---

## 📊 POSIBLES CAUSAS Y SOLUCIONES

### **Causa 1: Antiforgery Token Inválido**

**Síntoma**: Error 400 sin logs de "INICIO POST"

**Logs esperados**: No aparece ningún log porque el request es rechazado antes de llegar al handler

**Solución**:
```csharp
// Agregar en Startup.cs o Program.cs
services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});
```

**Test rápido**: Deshabilitar temporalmente antiforgery
```csharp
[IgnoreAntiforgeryToken] // Solo para testing
public async Task<IActionResult> OnPostCrearAsignacionAsync()
```

---

### **Causa 2: ModelState Inválido**

**Síntoma**: Logs muestran `ModelState.IsValid: False` y errores de validación

**Logs esperados**:
```
ModelState Error: The IdUsuario field is required.
ModelState Error: The IdBus field must be greater than 0.
```

**Solución**: Verificar que los atributos `asp-for` en el formulario coinciden con las propiedades del modelo

---

### **Causa 3: Valores Vacíos desde el Formulario**

**Síntoma**: Logs muestran `IdUsuario recibido: ''` o `IdBus recibido: 0`

**Logs esperados**:
```
IdUsuario recibido: ''
IdBus recibido: 0
Validación falló: Usuario o Bus no seleccionado
```

**Solución**: Verificar que los dropdowns tienen opciones disponibles

**Diagnóstico adicional**:
```razor
@* Agregar en la vista para debug *@
<div class="alert alert-info">
    <strong>DEBUG:</strong>
    Pilotos: @Model.Pilotos.Count |
    Monitores: @Model.Monitores.Count |
    Buses: @Model.BusesDisponibles.Count
</div>
```

---

### **Causa 4: Error de Base de Datos**

**Síntoma**: Logs muestran validaciones OK pero error en `SaveChangesAsync()`

**Logs esperados**:
```
Validaciones pasadas, creando asignación...
❌ Error al crear asignación. IdUsuario: xxx, IdBus: xxx
SqlException: Cannot insert...
```

**Posibles causas**:
- Violación de clave foránea (IdUsuario o IdBus no existen)
- Violación de restricción única
- Campo requerido faltante

---

## 🔍 SCRIPT DE VERIFICACIÓN SQL

Ejecuta esto en SQL Server para verificar datos:

```sql
-- Verificar usuarios con roles Piloto/Monitor
SELECT u.Id, u.UserName, u.Email, r.Name AS Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name IN ('Piloto', 'Monitor')
ORDER BY r.Name, u.UserName;

-- Verificar buses disponibles
SELECT IdBus, Placa, Modelo, Capacidad, Estado, Activo
FROM genesis.Buses
WHERE Estado = 1 AND Activo = 1;

-- Verificar asignaciones activas
SELECT 
    a.IdAsignacion,
    a.IdUsuarioPiloto,
    u.UserName,
    a.IdBus,
    b.Placa,
    a.FechaAsignacion,
    a.EsActual
FROM genesis.AsignacionesPilotoBus a
INNER JOIN AspNetUsers u ON a.IdUsuarioPiloto = u.Id
INNER JOIN genesis.Buses b ON a.IdBus = b.IdBus
WHERE a.EsActual = 1;
```

---

## 🚀 PASOS PARA RESOLVER

### **Opción A: Verificar con Logs (Recomendado)**

1. ✅ **Código ya compilado** con logging
2. 🔄 **Reiniciar app** (Shift + F5, luego F5)
3. 📋 **Reproducir error**
4. 👁️ **Leer logs** en Output window
5. 📧 **Compartir logs** para diagnóstico preciso

---

### **Opción B: Verificar Datos Manualmente**

1. Ejecutar scripts SQL de verificación
2. Confirmar que existen:
   - ✅ Usuarios con rol Piloto/Monitor
   - ✅ Buses con Estado=1 y Activo=1
   - ✅ No hay asignaciones conflictivas

---

### **Opción C: Test Simplificado**

Agregar temporalmente al inicio de `OnPostCrearAsignacionAsync()`:

```csharp
// TEST: Forzar valores para debug
if (string.IsNullOrEmpty(IdUsuario))
{
    IdUsuario = "VALOR_TEST"; // Reemplazar con un ID real de la BD
}
if (IdBus == 0)
{
    IdBus = 1; // Reemplazar con un IdBus real de la BD
}
```

**⚠️ IMPORTANTE**: Esto es solo para testing. Remover después.

---

## 📝 CHECKLIST DE DIAGNÓSTICO

Marca cada ítem después de verificarlo:

- [ ] ✅ Aplicación recompilada correctamente
- [ ] ✅ App reiniciada en modo Debug
- [ ] ✅ Ventana Output visible en Visual Studio
- [ ] ✅ Error reproducido
- [ ] ✅ Logs capturados en Output window
- [ ] ✅ Scripts SQL ejecutados
- [ ] ✅ Datos verificados en base de datos
- [ ] ✅ Usuarios piloto2-5 y monitor2-5 creados
- [ ] ✅ Dropdowns muestran opciones disponibles
- [ ] ✅ Formulario se puede enviar (no bloqueado por validación HTML)

---

## 📤 INFORMACIÓN A COMPARTIR

Si el problema persiste, compartir:

1. **Logs de la ventana Output** (copiar todo desde "=== INICIO POST")
2. **Resultado de scripts SQL** (conteo de usuarios/buses)
3. **Screenshot del formulario** antes de enviar (mostrar dropdowns con opciones)
4. **Screenshot del panel Network** en DevTools (pestaña Headers y Payload)

---

## 🎯 PRÓXIMOS PASOS

### **Inmediato**:
1. Reiniciar aplicación
2. Reproducir error
3. Capturar logs

### **Si logs muestran ModelState inválido**:
- Verificar nombres de propiedades
- Verificar tipos de datos
- Revisar validaciones en el modelo

### **Si logs muestran error de BD**:
- Ejecutar scripts SQL
- Verificar integridad referencial
- Revisar constraints de tabla

### **Si no aparecen logs**:
- Problema es con antiforgery token
- Agregar `[IgnoreAntiforgeryToken]` temporalmente para confirmar

---

**Fecha**: Diciembre 2024  
**Estado**: 🔧 LISTO PARA DEPURACIÓN  
**Compilación**: ✅ SUCCESSFUL

---

*Documento generado para diagnóstico del error HTTP 400 en gestión de asignaciones.*
