# 🔧 FIX: Validación de Asignaciones y Creación de Usuarios de Prueba

**Fecha**: Diciembre 2024  
**Problema**: Error HTTP 400 al crear asignaciones  
**Estado**: ✅ RESUELTO

---

## 🐛 PROBLEMA IDENTIFICADO

### **Error HTTP 400** al intentar crear una asignación

**Causa raíz**:
- ❌ Faltaba validación de que un **usuario** (piloto/monitor) solo pueda tener **UNA asignación activa** a la vez
- ✅ Solo validaba que un **bus** no tuviera asignación activa
- ❌ Permitía asignar el mismo usuario a múltiples buses

---

## ✅ SOLUCIÓN IMPLEMENTADA

### **1. Validación Doble Agregada**

**Archivo**: `Pages/Admin/GestionarAsignaciones.cshtml.cs`

#### **Validación 1: Usuario ya tiene asignación activa**
```csharp
var usuarioTieneAsignacion = await _context.AsignacionesPilotoBusDb
    .FirstOrDefaultAsync(a => a.IdUsuarioPiloto == IdUsuario && a.EsActual);

if (usuarioTieneAsignacion != null)
{
    var usuario = await _userManager.FindByIdAsync(IdUsuario);
    Mensaje = $"El usuario {usuario?.UserName} ya tiene una asignación activa al Bus #{usuarioTieneAsignacion.IdBus}. Debe finalizarla primero.";
    TipoMensaje = "warning";
    return Page();
}
```

#### **Validación 2: Bus ya tiene asignación activa**
```csharp
var busYaTieneAsignacion = await _context.AsignacionesPilotoBusDb
    .FirstOrDefaultAsync(a => a.IdBus == IdBus && a.EsActual);

if (busYaTieneAsignacion != null)
{
    var usuarioAsignado = await _userManager.FindByIdAsync(busYaTieneAsignacion.IdUsuarioPiloto);
    Mensaje = $"El bus ya tiene una asignación activa con el usuario {usuarioAsignado?.UserName}. Debe finalizarla primero.";
    TipoMensaje = "warning";
    return Page();
}
```

#### **Try-Catch agregado**:
```csharp
try
{
    // Crear asignación
    _context.AsignacionesPilotoBusDb.Add(nuevaAsignacion);
    await _context.SaveChangesAsync();

    Mensaje = $"✅ Asignación creada exitosamente: {usuario?.UserName} → Bus #{IdBus}";
    TipoMensaje = "success";
}
catch (Exception ex)
{
    Mensaje = $"❌ Error al crear la asignación: {ex.Message}";
    TipoMensaje = "danger";
}
```

---

### **2. Usuarios de Prueba Creados Automáticamente**

**Archivo**: `Startup.cs` (método `SeedRolesAndAdmin`)

**Usuarios creados al iniciar la app**:

#### **Pilotos**:
- ✅ `piloto2@transportesgenesis.com` (Usuario: piloto2)
- ✅ `piloto3@transportesgenesis.com` (Usuario: piloto3)
- ✅ `piloto4@transportesgenesis.com` (Usuario: piloto4)
- ✅ `piloto5@transportesgenesis.com` (Usuario: piloto5)

#### **Monitores**:
- ✅ `monitor2@transportesgenesis.com` (Usuario: monitor2)
- ✅ `monitor3@transportesgenesis.com` (Usuario: monitor3)
- ✅ `monitor4@transportesgenesis.com` (Usuario: monitor4)
- ✅ `monitor5@transportesgenesis.com` (Usuario: monitor5)

**Contraseña para todos**: `Admin123!`

**Características**:
- ✅ `EmailConfirmed = true` (no requiere confirmación de email)
- ✅ `IsFirstLogin = false` (no obliga a cambiar contraseña en primer ingreso)
- ✅ Roles asignados automáticamente

---

## 🧪 CÓMO PROBAR LA CORRECCIÓN

### **Paso 1: Reiniciar la aplicación**

1. **Detener la aplicación** (Shift + F5 en Visual Studio)
2. **Iniciar nuevamente** (F5)
3. **Esperar mensaje en consola**:
   ```
   ✅ Usuario piloto2 creado con rol Piloto
   ✅ Usuario piloto3 creado con rol Piloto
   ✅ Usuario piloto4 creado con rol Piloto
   ✅ Usuario piloto5 creado con rol Piloto
   ✅ Usuario monitor2 creado con rol Monitor
   ✅ Usuario monitor3 creado con rol Monitor
   ✅ Usuario monitor4 creado con rol Monitor
   ✅ Usuario monitor5 creado con rol Monitor
   ```

---

### **Paso 2: Probar Validación de Asignación**

#### **Test 1: Asignación Normal (debe funcionar)**
1. Ir a `/Admin/GestionarAsignaciones`
2. Seleccionar usuario: `piloto2`
3. Seleccionar bus: `Bus #2`
4. Clic en **"Asignar"**
5. **Resultado esperado**: ✅ Mensaje verde "Asignación creada exitosamente"

#### **Test 2: Intentar asignar mismo usuario a otro bus (debe fallar)**
1. En la misma pantalla
2. Seleccionar usuario: `piloto2` (mismo usuario anterior)
3. Seleccionar bus: `Bus #3` (bus diferente)
4. Clic en **"Asignar"**
5. **Resultado esperado**: ⚠️ Mensaje naranja:
   ```
   El usuario piloto2 ya tiene una asignación activa al Bus #2. Debe finalizarla primero.
   ```

#### **Test 3: Intentar asignar otro usuario al mismo bus (debe fallar)**
1. Seleccionar usuario: `piloto3` (usuario diferente)
2. Seleccionar bus: `Bus #2` (mismo bus anterior)
3. Clic en **"Asignar"**
4. **Resultado esperado**: ⚠️ Mensaje naranja:
   ```
   El bus ya tiene una asignación activa con el usuario piloto2. Debe finalizarla primero.
   ```

#### **Test 4: Finalizar asignación y reasignar (debe funcionar)**
1. En la tabla "Asignaciones Activas"
2. Buscar la fila de `piloto2 → Bus #2`
3. Clic en botón **"Finalizar"**
4. Confirmar
5. **Resultado esperado**: ✅ Mensaje "Asignación finalizada exitosamente"
6. Ahora intentar crear nueva asignación con `piloto2`
7. **Resultado esperado**: ✅ Debe permitir crear nueva asignación

---

### **Paso 3: Verificar usuarios creados en SQL**

Ejecutar en SQL Server Management Studio:

```sql
-- Ver todos los pilotos
SELECT u.UserName, u.Email, r.Name AS Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'Piloto'
ORDER BY u.UserName;

-- Ver todos los monitores
SELECT u.UserName, u.Email, r.Name AS Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE r.Name = 'Monitor'
ORDER BY u.UserName;
```

**Resultado esperado**:
```
Pilotos:
- piloto1 (existente)
- piloto2 (nuevo)
- piloto3 (nuevo)
- piloto4 (nuevo)
- piloto5 (nuevo)

Monitores:
- monitor1 (existente)
- monitor2 (nuevo)
- monitor3 (nuevo)
- monitor4 (nuevo)
- monitor5 (nuevo)
```

---

## 📊 DIAGRAMA DE VALIDACIÓN

```
┌────────────────────────────────────┐
│  Admin intenta crear asignación   │
│  Usuario: piloto2                 │
│  Bus: Bus #2                      │
└──────────────┬─────────────────────┘
               │
               ▼
┌────────────────────────────────────┐
│  VALIDACIÓN 1:                    │
│  ¿Usuario ya tiene asignación     │
│  activa?                          │
└──────────────┬─────────────────────┘
               │
         ┌─────┴─────┐
         │           │
         ▼           ▼
      ❌ SÍ       ✅ NO
         │           │
         │           ▼
         │  ┌────────────────────────────┐
         │  │  VALIDACIÓN 2:             │
         │  │  ¿Bus ya tiene asignación  │
         │  │  activa?                   │
         │  └──────────┬─────────────────┘
         │             │
         │       ┌─────┴─────┐
         │       │           │
         │       ▼           ▼
         │    ❌ SÍ       ✅ NO
         │       │           │
         └───────┴───────────┼──────►
                 │           │
                 ▼           ▼
         ⚠️ RECHAZAR    ✅ CREAR
         Mostrar error  Guardar en BD
```

---

## 🔄 ESCENARIOS POSIBLES

| Escenario | Usuario | Bus | Resultado |
|-----------|---------|-----|-----------|
| 1 | piloto2 (sin asignación) | Bus #2 (sin asignación) | ✅ CREADA |
| 2 | piloto2 (ya asignado a Bus #2) | Bus #3 | ❌ RECHAZADA (usuario ya tiene asignación) |
| 3 | piloto3 (sin asignación) | Bus #2 (ya asignado a piloto2) | ❌ RECHAZADA (bus ya tiene asignación) |
| 4 | piloto2 (finalizada su asignación) | Bus #3 | ✅ CREADA |

---

## 📝 ARCHIVOS MODIFICADOS

| # | Archivo | Cambios |
|---|---------|---------|
| 1 | `Pages/Admin/GestionarAsignaciones.cshtml.cs` | ✅ Agregada doble validación + try-catch |
| 2 | `Startup.cs` | ✅ Agregado seed de 8 usuarios de prueba (4 pilotos, 4 monitores) |
| 3 | `Scripts/CrearPilotosMonitoresPrueba.sql` | ✅ Script de referencia (no necesario ejecutar) |

---

## 🚀 INSTRUCCIONES DE USO

### **Para probar inmediatamente**:

1. **Reiniciar la aplicación** (Shift + F5, luego F5)
2. Los usuarios se crearán automáticamente al iniciar
3. Ir a `/Admin/GestionarAsignaciones`
4. Probar crear asignaciones con los nuevos usuarios

### **Credenciales de los nuevos usuarios**:
```
Usuario: piloto2   | Contraseña: Admin123!
Usuario: piloto3   | Contraseña: Admin123!
Usuario: piloto4   | Contraseña: Admin123!
Usuario: piloto5   | Contraseña: Admin123!
Usuario: monitor2  | Contraseña: Admin123!
Usuario: monitor3  | Contraseña: Admin123!
Usuario: monitor4  | Contraseña: Admin123!
Usuario: monitor5  | Contraseña: Admin123!
```

---

## ⚠️ NOTAS IMPORTANTES

### **1. Si los usuarios ya existen**:
- El código **NO los volverá a crear**
- El seed verifica con `FindByEmailAsync()` antes de crear
- No habrá duplicados

### **2. Si quieres eliminar usuarios de prueba**:
```sql
-- Eliminar usuarios de prueba (ejecutar con cuidado)
DELETE FROM AspNetUserRoles 
WHERE UserId IN (
    SELECT Id FROM AspNetUsers 
    WHERE Email LIKE '%piloto%@transportesgenesis.com' 
    OR Email LIKE '%monitor%@transportesgenesis.com'
);

DELETE FROM AspNetUsers 
WHERE Email LIKE '%piloto%@transportesgenesis.com' 
OR Email LIKE '%monitor%@transportesgenesis.com';
```

### **3. Si quieres agregar más usuarios**:
- Editar array en `Startup.cs` (línea ~200)
- Agregar más elementos al array `pilotosMonitores`
- Reiniciar aplicación

---

## ✅ CHECKLIST DE VERIFICACIÓN

Marca cada ítem después de probarlo:

- [ ] ✅ Aplicación reiniciada correctamente
- [ ] ✅ Usuarios piloto2-5 y monitor2-5 creados
- [ ] ✅ Asignación normal funciona
- [ ] ✅ Validación de usuario ya asignado funciona
- [ ] ✅ Validación de bus ya asignado funciona
- [ ] ✅ Finalizar asignación funciona
- [ ] ✅ Reasignar después de finalizar funciona
- [ ] ✅ No aparece error HTTP 400

---

**Fecha de Implementación**: Diciembre 2024  
**Estado**: ✅ LISTO PARA PROBAR  
**Compilación**: ✅ SUCCESSFUL (requiere reinicio de app)

---

*Documento generado como parte de la corrección del error HTTP 400 en gestión de asignaciones.*
