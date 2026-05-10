# 🔍 VERIFICACIÓN DE USUARIOS CREADOS

## 📊 PASO 1: VERIFICAR EN BASE DE DATOS

Ejecuta estas consultas SQL para confirmar que los usuarios se crearon:

### **Query 1: Verificar usuarios creados**
```sql
SELECT 
    u.Id,
    u.UserName,
    u.Email,
    u.EmailConfirmed,
    u.LockoutEnabled,
    u.AccessFailedCount
FROM AspNetUsers u
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
   OR u.UserName IN ('piloto1', 'monitor1');
```

**✅ Si la query retorna 2 filas:** Los usuarios SÍ se crearon  
**❌ Si retorna 0 filas:** Los usuarios NO se crearon

---

### **Query 2: Verificar roles asignados**
```sql
SELECT 
    u.UserName,
    u.Email,
    r.Name as Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
   OR u.UserName IN ('piloto1', 'monitor1');
```

**✅ Si retorna 2 filas:** Los roles están asignados correctamente  
**❌ Si retorna 0 filas:** Los roles NO se asignaron

---

### **Query 3: Verificar asignaciones de bus**
```sql
SELECT 
    u.UserName,
    u.Email,
    a.IdBus,
    a.EsActual,
    a.Activo,
    a.FechaAsignacion
FROM genesis.AsignacionPilotoBus a
INNER JOIN AspNetUsers u ON a.IdUsuarioPiloto = u.Id
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
   OR u.UserName IN ('piloto1', 'monitor1');
```

**✅ Si retorna 2 filas:** Las asignaciones de bus están correctas  
**❌ Si retorna 0 filas:** Las asignaciones NO se crearon

---

### **Query 4: Ver TODOS los usuarios del sistema**
```sql
SELECT 
    u.UserName,
    u.Email,
    u.EmailConfirmed,
    STRING_AGG(r.Name, ', ') as Roles
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
GROUP BY u.UserName, u.Email, u.EmailConfirmed
ORDER BY u.UserName;
```

Esta query muestra **todos** los usuarios del sistema con sus roles.

---

## 🐛 POSIBLES PROBLEMAS Y SOLUCIONES:

### **Problema 1: Los usuarios SÍ se crearon pero no aparecen en la página**

**Causa:** El método `CargarUsuarios()` tiene un problema o la vista no está renderizando correctamente.

**Solución:** Recargar la página manualmente:
```javascript
// En la consola del navegador (F12):
window.location.reload();
```

O hacer clic en el botón de recargar del navegador.

---

### **Problema 2: Los usuarios NO se crearon (la query retorna 0 filas)**

**Posibles causas:**
1. Hubo un error al crear el usuario y no se mostró el mensaje
2. La transacción se revirtió (rollback)
3. El password no cumple con las políticas de Identity

**Solución:** Intentar crear los usuarios de nuevo desde `/Admin/Usuarios`

---

### **Problema 3: Error "Usuario ya existe"**

**Causa:** Los usuarios ya se crearon previamente.

**Solución:** No hacer nada, los usuarios ya existen. Proceder a probar los logins.

---

## 🔧 DEBUGGING: Ver mensajes de error en la consola

### **1. Abrir las herramientas de desarrollo del navegador:**
```
Presiona F12
```

### **2. Ver errores en la pestaña "Console":**
Busca mensajes en rojo que indiquen errores JavaScript o de red.

### **3. Ver la respuesta del servidor en la pestaña "Network":**
1. Ve a la pestaña "Network"
2. Recarga la página (`F5`)
3. Busca la petición a `/Admin/Usuarios`
4. Haz clic en ella
5. Ve a la pestaña "Response" para ver la respuesta HTML completa

---

## 🎯 PLAN DE ACCIÓN:

### **OPCIÓN A: Si las queries SQL muestran que los usuarios SÍ existen:**

1. **Recargar la página:** `F5` o `Ctrl + F5` (forzar recarga)
2. **Si aún no aparecen:** Cerrar sesión y volver a hacer login como admin
3. **Si aún no aparecen:** Reiniciar la aplicación (detener con `Shift + F5`, iniciar con `F5`)

---

### **OPCIÓN B: Si las queries SQL muestran que los usuarios NO existen:**

1. **Verificar que no hubo errores al crear:**
   - ¿Apareció un mensaje de error después de hacer clic en "Crear Usuario"?
   - ¿El mensaje decía "creado exitosamente"?

2. **Intentar crear de nuevo:**
   - Ir a `/Admin/Usuarios`
   - Intentar crear `piloto1` de nuevo
   - Anotar EXACTAMENTE el mensaje que aparece

3. **Verificar que existe el Bus #1:**
```sql
SELECT * FROM genesis.Bus WHERE IdBus = 1;
```

Si no existe, crearlo:
```sql
SET IDENTITY_INSERT genesis.Bus ON;
INSERT INTO genesis.Bus (IdBus, Placa, Modelo, Capacidad, Activo, FechaRegistro)
VALUES (1, 'ABC-123', 'Mercedes Benz', 40, 1, GETDATE());
SET IDENTITY_INSERT genesis.Bus OFF;
```

---

## 🚀 SOLUCIÓN RÁPIDA: Crear usuarios directamente por SQL

Si todo lo anterior falla, puedes crear los usuarios manualmente en SQL:

### **⚠️ ADVERTENCIA:** Esta es una solución temporal. Las contraseñas deben hashearse correctamente.

**NO recomendado** porque las contraseñas de Identity están hasheadas con algoritmos específicos.

**MEJOR:** Arreglar el problema de la interfaz web.

---

## 📞 INFORMACIÓN PARA REPORTAR:

Si los usuarios NO aparecen, necesito que me proporciones:

1. **Resultado de la Query 1** (usuarios creados):
```
Pega aquí el resultado
```

2. **Resultado de la Query 4** (todos los usuarios):
```
Pega aquí el resultado
```

3. **Mensaje que apareció después de crear el usuario:**
```
Pega aquí el mensaje exacto
```

4. **Errores en la consola del navegador (F12 → Console):**
```
Pega aquí cualquier error en rojo
```

5. **Errores en Visual Studio (Output window):**
```
Pega aquí los logs del servidor
```

---

## ✅ CHECKLIST DE VERIFICACIÓN:

- [ ] Ejecuté la Query 1 → Resultado: _____ filas
- [ ] Ejecuté la Query 2 → Resultado: _____ filas
- [ ] Ejecuté la Query 3 → Resultado: _____ filas
- [ ] Ejecuté la Query 4 → Resultado: _____ usuarios en total
- [ ] Recargué la página con F5
- [ ] Recargué con Ctrl + F5 (forzar)
- [ ] Cerré sesión y volví a hacer login
- [ ] Reinicié la aplicación
- [ ] Los usuarios aparecen ahora: SÍ / NO

---

**Por favor, ejecuta la Query 1 primero y dime cuántas filas retorna.** 📊
