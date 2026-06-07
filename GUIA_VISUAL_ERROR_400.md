# 🎯 GUÍA VISUAL - Depurar Error 400

## 🔴 PROBLEMA
```
❌ HTTP 400 Bad Request
❌ Página "no disponible"
❌ Error en consola: ERR_HTTP_RESPONSE_CODE_FAILURE
```

---

## ✅ SOLUCIÓN EN 5 PASOS

### **1️⃣ REINICIAR APP**
```
Shift + F5  →  F5
```

### **2️⃣ ABRIR OUTPUT WINDOW**
```
View → Output (Ctrl + Alt + O)
Dropdown: "Debug"
```

### **3️⃣ IR A LA PÁGINA**
```
https://localhost:7241/Admin/GestionarAsignaciones
```

**Verás esto**:
```
┌─────────────────────────────────────────────┐
│ 📊 DEBUG INFO:                              │
│ Pilotos sin asignar: X                      │
│ Monitores sin asignar: X                    │
│ Buses sin asignar: X                        │
│ Asignaciones activas: X                     │
└─────────────────────────────────────────────┘
```

### **4️⃣ INTENTAR ASIGNAR**
```
1. Seleccionar piloto/monitor ▼
2. Seleccionar bus ▼
3. Clic en [Asignar] 🎯
```

### **5️⃣ LEER LOGS EN OUTPUT WINDOW**
```
=== INICIO POST CrearAsignacion ===
IdUsuario recibido: '...'
IdBus recibido: ...
ModelState.IsValid: True/False
```

---

## 🔍 DIAGNÓSTICO RÁPIDO

### **🟢 CASO A: DEBUG INFO muestra números > 0**
```
Pilotos sin asignar: 4
Monitores sin asignar: 4
Buses sin asignar: 3
```
✅ **Datos OK** → Continúa al paso 4

---

### **🔴 CASO B: DEBUG INFO muestra todos 0**
```
Pilotos sin asignar: 0
Monitores sin asignar: 0
Buses sin asignar: 0
```
❌ **Sin datos disponibles**

**Causa**: Usuarios no creados o todos ya asignados

**Solución**:
```
1. Detener app (Shift + F5)
2. Reiniciar (F5)
3. Startup.cs creará piloto2-5 y monitor2-5 automáticamente
4. Volver al paso 3
```

---

### **🟡 CASO C: No aparece "=== INICIO POST" en logs**
```
(No hay logs después de clic en Asignar)
```
❌ **Antiforgery token rechazado**

**Test rápido**:
```csharp
// Agregar temporalmente en GestionarAsignaciones.cshtml.cs
[IgnoreAntiforgeryToken] // ⚠️ SOLO PARA TESTING
public async Task<IActionResult> OnPostCrearAsignacionAsync()
```

**Si esto soluciona el problema**:
→ El issue es configuración de antiforgery
→ NO dejar `[IgnoreAntiforgeryToken]` en producción

---

### **🟠 CASO D: Logs muestran "ModelState.IsValid: False"**
```
ModelState.IsValid: False
ModelState Error: The IdUsuario field is required
```
❌ **Validación del modelo falla**

**Solución**:
```
Ver logs completos para identificar qué campo falla
```

---

### **🔴 CASO E: Error en SaveChangesAsync()**
```
Validaciones pasadas, creando asignación...
❌ Error al crear asignación
SqlException: Cannot insert...
```
❌ **Error de base de datos**

**Verificar con SQL**:
```sql
-- ¿Existe el usuario?
SELECT * FROM AspNetUsers WHERE Id = 'xxx';

-- ¿Existe el bus?
SELECT * FROM genesis.Buses WHERE IdBus = X;

-- ¿Hay constraints violados?
SELECT * FROM genesis.AsignacionesPilotoBus 
WHERE IdUsuarioPiloto = 'xxx' AND EsActual = 1;
```

---

## 🛠️ SOLUCIONES COMUNES

### **Problema: Todos los conteos son 0**
```sql
-- Verificar usuarios
SELECT u.UserName, r.Name AS Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.UserName LIKE 'piloto%' OR u.UserName LIKE 'monitor%';
```

**Si no devuelve resultados**:
→ Reiniciar app para que `Startup.cs` cree usuarios

---

### **Problema: Antiforgery token**

**Test**:
```csharp
[IgnoreAntiforgeryToken]
public async Task<IActionResult> OnPostCrearAsignacionAsync()
```

**Si funciona**:
```csharp
// En Startup.cs/Program.cs
services.AddAntiforgery(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax; // Cambiar de Strict a Lax
});
```

---

### **Problema: Dropdowns vacíos**

**Verificar en la vista**:
```razor
@if (!Model.Pilotos.Any())
{
    <div class="alert alert-warning">
        ⚠️ No hay pilotos disponibles para asignar
    </div>
}
```

---

## 📊 FLOWCHART DE DIAGNÓSTICO

```
Inicio
  │
  ▼
¿App reiniciada? ───NO───► Reiniciar (Shift+F5, F5)
  │ SÍ
  ▼
¿Output window abierta? ───NO───► Abrir (Ctrl+Alt+O)
  │ SÍ
  ▼
Ir a /Admin/GestionarAsignaciones
  │
  ▼
¿DEBUG INFO muestra números > 0? ───NO───┐
  │ SÍ                                    │
  ▼                                       │
Seleccionar piloto + bus                  │
  │                                       │
  ▼                                       ▼
Clic en Asignar                    Reiniciar app
  │                                (usuarios se crean)
  ▼                                       │
¿Aparece "=== INICIO POST"? ───NO───► Antiforgery issue
  │ SÍ                             (usar [IgnoreAntiforgeryToken])
  ▼
¿ModelState.IsValid: True? ───NO───► Ver errores en logs
  │ SÍ
  ▼
¿Error en SaveChanges? ───SÍ───► Verificar BD con SQL
  │ NO
  ▼
✅ ÉXITO: Asignación creada
```

---

## 🎯 ACCIÓN INMEDIATA

**HAZLO AHORA**:

1. ✅ Presiona **Shift + F5** (detener app)
2. ✅ Presiona **F5** (iniciar app)
3. ✅ Presiona **Ctrl + Alt + O** (abrir Output)
4. ✅ Ve a `https://localhost:7241/Admin/GestionarAsignaciones`
5. ✅ **Mira el panel DEBUG INFO**
6. ✅ Intenta crear asignación
7. ✅ **LEE LOS LOGS EN OUTPUT WINDOW**

---

## 📧 INFORMACIÓN A COMPARTIR

Si necesitas ayuda, comparte:

1. **Panel DEBUG INFO** (screenshot o texto)
2. **Logs completos** (desde "=== INICIO POST")
3. **Screenshot** de dropdowns (muestra si tienen opciones)
4. **Resultado** de queries SQL de verificación

---

**Compilación**: ✅ SUCCESSFUL  
**Estado**: 🚀 LISTO PARA DEPURAR  
**Archivos**: 
- ✅ `DEBUG_ERROR_400_ASIGNACIONES.md` (técnico detallado)
- ✅ `SOLUCION_RAPIDA_ERROR_400.md` (pasos específicos)
- ✅ `GUIA_VISUAL_ERROR_400.md` (este archivo - visual rápido)

---

**PRÓXIMO PASO**: ⬆️ REINICIAR APP AHORA Y SEGUIR LOS 5 PASOS ⬆️
