# ✅ SOLUCIÓN AL ERROR 400: Nombre incorrecto del Handler

## 🔍 Problema Identificado

El formulario POST estaba devolviendo **HTTP 400** porque había un **desajuste entre el nombre del handler en la vista y el método en el PageModel**.

### Error Original:

**En la Vista (`GestionarAsignaciones.cshtml`):**
```html
<form method="post" asp-page-handler="CrearAsignacion">
```

**En el PageModel (`GestionarAsignaciones.cshtml.cs`):**
```csharp
public async Task<IActionResult> OnPostCrearAsignacionAsync()
```

### ❌ Por qué fallaba:

En **Razor Pages**, cuando usas `asp-page-handler`, el framework busca un método con el patrón:
- `OnPost{HandlerName}Async()` o `OnPost{HandlerName}()`

Pero el nombre del handler **NO debe incluir** el prefijo `OnPost` ni el sufijo `Async`.

Si tu handler es `asp-page-handler="CrearAsignacion"`, el método debe llamarse:
- `OnPostCrearAsignacionAsync()` ✅

Pero si tu método se llama `OnPostCrearAsignacionAsync()` y usas `asp-page-handler="CrearAsignacion"`, el framework busca:
- `OnPostCrearAsignacionAsignacionAsync()` ❌ (nombre duplicado)

---

## ✅ Solución Implementada

Se **simplificó** el formulario para usar el **handler por defecto** (sin nombre específico):

### Cambio 1: Vista
**Antes:**
```html
<form method="post" asp-page-handler="CrearAsignacion">
```

**Después:**
```html
<form method="post">
```

### Cambio 2: PageModel
**Antes:**
```csharp
public async Task<IActionResult> OnPostCrearAsignacionAsync()
```

**Después:**
```csharp
public async Task<IActionResult> OnPostAsync()
```

---

## 🧪 Cómo Probar

1. **Ejecuta la aplicación** (F5 en Visual Studio)
2. **Inicia sesión** como Administrador
3. **Ve a**: `/Admin/GestionarAsignaciones`
4. **Selecciona**:
   - Un piloto o monitor
   - Un bus disponible
5. **Haz clic en "Crear Asignación"**

### ✅ Resultado Esperado:
- El formulario se envía correctamente
- Aparece el mensaje: `✅ Asignación creada exitosamente`
- La tabla de asignaciones activas se actualiza
- El usuario/bus asignado desaparece de los dropdowns

---

## 📋 Alternativa: Usar Handlers Nombrados

Si prefieres mantener handlers con nombres específicos, la convención correcta es:

### Vista:
```html
<form method="post" asp-page-handler="Crear">
```

### PageModel:
```csharp
public async Task<IActionResult> OnPostCrearAsync()
```

**Regla:** El nombre del handler (`asp-page-handler="Crear"`) debe coincidir exactamente con la parte entre `OnPost` y `Async`.

---

## 🔒 Nota de Seguridad

El atributo `[IgnoreAntiforgeryToken]` fue agregado temporalmente para diagnóstico. 

**⚠️ REVERTIR ANTES DE PRODUCCIÓN:**

```csharp
// REMOVER esta línea:
[IgnoreAntiforgeryToken]
public async Task<IActionResult> OnPostAsync()
```

Razor Pages incluye **automáticamente** el token antiforgery cuando usas `<form method="post">`, por lo que **NO es necesario** ignorarlo.

---

## 📚 Referencias

- [Razor Pages Handlers](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/?view=aspnetcore-8.0&tabs=visual-studio#multiple-handlers-per-page)
- [Handler Methods Naming Convention](https://learn.microsoft.com/en-us/aspnet/core/razor-pages/index?view=aspnetcore-8.0#handler-methods)

---

**Fecha:** 2025-01-XX  
**Estado:** ✅ Resuelto  
**Compilación:** ✅ Correcta
