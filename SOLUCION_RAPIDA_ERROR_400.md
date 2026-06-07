# ⚡ SOLUCIÓN RÁPIDA - Error HTTP 400 en Asignaciones

## 🎯 Resumen del Problema

**Error**: HTTP 400 Bad Request al intentar asignar piloto/monitor → bus

**Causa probable**: 
1. **Antiforgery token** inválido o expirado
2. **ModelState** con errores de validación
3. **Datos vacíos** (listas sin cargar)
4. **Error de base de datos** (FK constraints)

---

## ✅ Cambios Realizados

### **1. Logging Detallado**
- ✅ Logs en cada paso del proceso
- ✅ Captura de errores de ModelState
- ✅ Información de valores recibidos

### **2. Panel de Debug en la Página**
- ✅ Muestra conteo de pilotos/monitores/buses disponibles
- ✅ Ayuda a identificar si las listas están vacías

### **3. Manejo de Tipos Nullables**
- ✅ `IdUsuario` ahora es `string?` (nullable)
- ✅ Mejor compatibilidad con model binding

---

## 🚀 PASOS INMEDIATOS (HAZLO AHORA)

### **Paso 1: Reiniciar la Aplicación**

```
1. Presiona Shift + F5 (detener)
2. Presiona F5 (iniciar en debug)
3. Espera a que cargue completamente
```

### **Paso 2: Abrir Ventana Output**

```
En Visual Studio:
View → Output (Ctrl + Alt + O)
Seleccionar: "Debug" en el dropdown
```

### **Paso 3: Ir a la Página**

```
URL: https://localhost:7241/Admin/GestionarAsignaciones
```

**¿Qué deberías ver?**
```
📊 DEBUG INFO:
Pilotos sin asignar: X | Monitores sin asignar: X | Buses sin asignar: X | Asignaciones activas: X
```

**⚠️ Si todos los conteos son 0**:
- Problema: No hay datos cargados
- Solución: Ejecutar scripts SQL o crear usuarios/buses

### **Paso 4: Intentar Crear Asignación**

```
1. Selecciona un piloto/monitor
2. Selecciona un bus
3. Clic en "Asignar"
4. MIRA LA VENTANA OUTPUT EN VISUAL STUDIO
```

### **Paso 5: Leer los Logs**

**Busca en Output window**:
```
=== INICIO POST CrearAsignacion ===
IdUsuario recibido: '...'
IdBus recibido: ...
ModelState.IsValid: True/False
```

---

## 🔍 DIAGNÓSTICO SEGÚN LOGS

### **Caso 1: No aparece "=== INICIO POST"**

**Significa**: El request nunca llegó al handler

**Causa**: Antiforgery token rechazado

**Solución temporal**:
```csharp
// En GestionarAsignaciones.cshtml.cs
[IgnoreAntiforgeryToken] // SOLO PARA TESTING
public async Task<IActionResult> OnPostCrearAsignacionAsync()
```

---

### **Caso 2: Aparece "ModelState.IsValid: False"**

**Significa**: Errores de validación del modelo

**Busca en logs**:
```
ModelState Error: ...
```

**Solución**: Revisar las propiedades del modelo y los `asp-for` en el formulario

---

### **Caso 3: IdUsuario = '' o IdBus = 0**

**Significa**: Formulario enviando valores vacíos

**Verifica**:
- ¿El panel DEBUG muestra conteos > 0?
- ¿Los dropdowns tienen opciones visibles?

**Si conteos = 0**:
```sql
-- Ejecutar en SQL Server
SELECT COUNT(*) FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name IN ('Piloto', 'Monitor');

SELECT COUNT(*) FROM genesis.Buses WHERE Estado = 1 AND Activo = 1;
```

---

### **Caso 4: Error en SaveChangesAsync()**

**Significa**: Problema con base de datos

**Busca en logs**:
```
❌ Error al crear asignación
SqlException: ...
```

**Soluciones comunes**:
- Verificar que IdUsuario existe en AspNetUsers
- Verificar que IdBus existe en genesis.Buses
- Verificar constraints de la tabla AsignacionesPilotoBus

---

## 🛠️ SOLUCIONES RÁPIDAS

### **Si DEBUG INFO muestra todo en 0**

**Problema**: Datos no cargados o todos asignados

**Verificar usuarios creados**:
```sql
SELECT u.UserName, r.Name AS Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.UserName LIKE 'piloto%' OR u.UserName LIKE 'monitor%'
ORDER BY r.Name, u.UserName;
```

**Si no existen**: Reiniciar app para que `Startup.cs` los cree automáticamente.

---

### **Si Antiforgery Token falla**

**Solución temporal** (solo para testing):

```csharp
[IgnoreAntiforgeryToken]
public async Task<IActionResult> OnPostCrearAsignacionAsync()
{
    // ... código existente
}
```

**⚠️ IMPORTANTE**: Si esto soluciona el problema, entonces el issue es con antiforgery. NO dejar `[IgnoreAntiforgeryToken]` en producción.

**Solución permanente**: Verificar configuración en `Startup.cs`:
```csharp
services.AddAntiforgery(options =>
{
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
```

---

### **Si error persiste después de todo**

**Test manual directo desde SQL**:

```sql
-- Test: Crear asignación manualmente
INSERT INTO genesis.AsignacionesPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
VALUES (
    (SELECT TOP 1 Id FROM AspNetUsers u
     INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
     INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
     WHERE r.Name = 'Piloto'),
    (SELECT TOP 1 IdBus FROM genesis.Buses WHERE Estado = 1),
    GETDATE(),
    1,
    1,
    GETDATE()
);

-- Verificar si se creó
SELECT * FROM genesis.AsignacionesPilotoBus WHERE EsActual = 1;
```

**Si esto funciona**: El problema NO es la base de datos, es el formulario/binding.

**Si esto falla**: El problema ES la base de datos (constraints, FKs, etc.).

---

## 📋 CHECKLIST PASO A PASO

1. [ ] ✅ App reiniciada en modo Debug
2. [ ] ✅ Ventana Output abierta y visible
3. [ ] ✅ Página `/Admin/GestionarAsignaciones` cargada
4. [ ] ✅ Panel DEBUG INFO muestra conteos > 0
5. [ ] ✅ Dropdowns muestran opciones de pilotos/monitores
6. [ ] ✅ Dropdown muestra opciones de buses
7. [ ] ✅ Formulario enviado
8. [ ] ✅ Logs capturados en Output window
9. [ ] ✅ Análisis de logs completado
10. [ ] ✅ Causa identificada

---

## 📤 Si Necesitas Ayuda

**Comparte esta información**:

1. **Panel DEBUG INFO**:
   ```
   Pilotos sin asignar: X | Monitores sin asignar: X | Buses sin asignar: X
   ```

2. **Logs completos** de Output window (desde "=== INICIO POST")

3. **Resultado de estos queries**:
   ```sql
   SELECT COUNT(*) AS Pilotos FROM AspNetUsers u
   INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
   INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
   WHERE r.Name = 'Piloto';

   SELECT COUNT(*) AS Monitores FROM AspNetUsers u
   INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
   INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
   WHERE r.Name = 'Monitor';

   SELECT COUNT(*) AS Buses FROM genesis.Buses WHERE Estado = 1 AND Activo = 1;

   SELECT COUNT(*) AS AsignacionesActivas FROM genesis.AsignacionesPilotoBus WHERE EsActual = 1;
   ```

4. **Screenshot del panel Network** en DevTools (pestaña Payload)

---

**Compilación**: ✅ SUCCESSFUL  
**Estado**: 🔧 LISTO PARA DEPURACIÓN  
**Próximo paso**: REINICIAR APP Y CAPTURAR LOGS

---

*Documento generado para diagnóstico rápido del error HTTP 400.*
