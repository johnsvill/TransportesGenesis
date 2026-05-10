# ✅ PROBLEMA SOLUCIONADO - USUARIOS DE PRUEBA

## 🎉 **ESTADO ACTUAL:**
- ✅ Compilación exitosa
- ✅ Archivos creados y corregidos
- ✅ Errores de namespace y tipos de datos solucionados

---

## 🚀 **AHORA PUEDES PROCEDER:**

### **PASO 1: EJECUTAR LA APLICACIÓN** ⏱️ 10 segundos

Presiona **F5** en Visual Studio o ejecuta:
```powershell
dotnet run
```

---

### **PASO 2: LOGIN COMO ADMIN** ⏱️ 30 segundos

1. Ve a: `https://localhost:7241/Auth/Login`
2. Introduce tus credenciales de administrador:
```
Email: admin@transportesgenesis.com
Password: (tu password)
```

---

### **PASO 3: IR AL ENDPOINT DE CREACIÓN** ⏱️ 10 segundos

**Opción A:** Navega manualmente a:
```
https://localhost:7241/Admin/CrearUsuariosPrueba
```

**Opción B:** Desde la consola del navegador (F12 → Console):
```javascript
window.location.href = "/Admin/CrearUsuariosPrueba";
```

---

### **PASO 4: CREAR USUARIOS** ⏱️ 5 segundos

1. La página mostrará un botón azul grande:
```
[+] Crear Usuarios de Prueba
```

2. **Haz clic en el botón**

3. **Espera la confirmación** (debería tomar 1-2 segundos)

---

## 📋 **RESULTADO ESPERADO:**

Si todo sale bien, verás dos tarjetas con estas credenciales:

### 🔐 **Usuario Piloto:**
```
Email: piloto1@transportesgenesis.com
Password: Piloto123!
Rol: Piloto
Bus Asignado: #1
```

### 🔐 **Usuario Monitor:**
```
Email: monitor1@transportesgenesis.com
Password: Monitor123!
Rol: Monitor
Bus Asignado: #1
```

---

## 🧪 **PASO 5: PROBAR LOS LOGINS**

### **Prueba 1: Login del Piloto**
1. **Cierra sesión del admin**: Click en tu nombre → "Cerrar Sesión"
2. Ve a: `/Auth/Login`
3. Introduce:
```
Email: piloto1@transportesgenesis.com
Password: Piloto123!
```
4. **✅ Debe redirigir a:** `/Piloto/MiRuta`
5. **✅ Debe mostrar:** "Bus #1" (obtenido dinámicamente)

---

### **Prueba 2: Login del Monitor**
1. **Cierra sesión del piloto**
2. Ve a: `/Auth/Login`
3. Introduce:
```
Email: monitor1@transportesgenesis.com
Password: Monitor123!
```
4. **✅ Debe redirigir a:** `/Monitor/MiRuta`
5. **✅ Debe mostrar:** "Bus #1" y botón "Registrar Recogidas"

---

## ⚠️ **POSIBLES ERRORES Y SOLUCIONES:**

### **Error 1: "No existe Bus #1 en la base de datos"**

**Solución:** Crear el bus en SQL:
```sql
-- Verificar si existe
SELECT * FROM genesis.Bus WHERE IdBus = 1;

-- Si no existe, crear
SET IDENTITY_INSERT genesis.Bus ON;
INSERT INTO genesis.Bus (IdBus, Placa, Modelo, Capacidad, Activo, FechaRegistro)
VALUES (1, 'ABC-123', 'Mercedes Benz', 40, 1, GETDATE());
SET IDENTITY_INSERT genesis.Bus OFF;
```

---

### **Error 2: "El rol 'Piloto' no existe"**

**Solución:** Crear los roles en SQL:
```sql
-- Verificar roles existentes
SELECT * FROM AspNetRoles;

-- Si no existen, crearlos
INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES 
(NEWID(), 'Piloto', 'PILOTO', NEWID()),
(NEWID(), 'Monitor', 'MONITOR', NEWID());
```

---

### **Error 3: "Invalid login attempt" al intentar loguearse**

**Posibles causas:**
1. El usuario no se creó correctamente
2. La contraseña es incorrecta
3. El usuario está bloqueado

**Verificar en base de datos:**
```sql
-- Verificar si el usuario existe
SELECT * FROM AspNetUsers 
WHERE Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com');

-- Verificar roles asignados
SELECT u.Email, r.Name as Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com');
```

---

## 📊 **VERIFICACIÓN COMPLETA EN BASE DE DATOS:**

Después de crear los usuarios, ejecuta estas queries para confirmar:

### **1. Verificar usuarios creados:**
```sql
SELECT 
    u.Id,
    u.UserName,
    u.Email,
    u.EmailConfirmed,
    r.Name as Rol
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com');
```
**✅ Resultado esperado:** 2 filas

---

### **2. Verificar asignaciones de bus:**
```sql
SELECT 
    a.IdAsignacion,
    u.Email,
    a.IdBus,
    a.EsActual,
    a.Activo,
    a.FechaAsignacion
FROM genesis.AsignacionPilotoBus a
INNER JOIN AspNetUsers u ON a.IdUsuarioPiloto = u.Id
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
    AND a.EsActual = 1
    AND a.Activo = 1;
```
**✅ Resultado esperado:** 2 filas (ambos usuarios asignados al Bus #1)

---

## 🔧 **LO QUE SE CORRIGIÓ:**

| Error Original | Corrección Aplicada |
|----------------|---------------------|
| ❌ `using TransportesGenesis.Models.DB.Identidad;` | ✅ `using TransportesGenesis.Models.DB.Usuarios;` |
| ❌ Propiedades inexistentes en `AppUser`: `Nombre`, `Apellido`, `FechaRegistro`, `Activo` | ✅ Solo usar propiedades existentes: `UserName`, `Email`, `EmailConfirmed` |
| ❌ `Activo = true` (tipo `bool`) | ✅ `Activo = 1` (tipo `int`) |
| ❌ `&& a.Activo` (comparación booleana) | ✅ `&& a.Activo == 1` (comparación entera) |

---

## ✅ **CHECKLIST FINAL:**

Marca las casillas a medida que completes cada paso:

- [ ] ✅ Aplicación ejecutándose (F5)
- [ ] ✅ Login como admin exitoso
- [ ] ✅ Navegado a `/Admin/CrearUsuariosPrueba`
- [ ] ✅ Botón "Crear Usuarios de Prueba" visible
- [ ] ✅ Clic en el botón → Usuarios creados
- [ ] ✅ Credenciales mostradas en pantalla
- [ ] ✅ Cerrar sesión del admin
- [ ] ✅ Login como piloto1 exitoso
- [ ] ✅ Redirigido a `/Piloto/MiRuta`
- [ ] ✅ "Bus #1" mostrado correctamente
- [ ] ✅ Cerrar sesión del piloto
- [ ] ✅ Login como monitor1 exitoso
- [ ] ✅ Redirigido a `/Monitor/MiRuta`
- [ ] ✅ "Bus #1" y botón "Registrar Recogidas" visibles
- [ ] ✅ Queries SQL ejecutadas y validadas

---

## 📞 **SI NECESITAS AYUDA:**

Si encuentras algún error:
1. Copia el **mensaje de error completo** de la página
2. Copia el **error de la consola del navegador** (F12 → Console)
3. Ejecuta las **queries SQL de verificación**
4. Comparte los resultados

---

## 🎯 **PRÓXIMOS PASOS:**

Una vez que los usuarios estén creados y puedas loguearte con ambos, puedes continuar con el resto de las pruebas del documento `PRUEBAS_E_INSTRUCCIONES_PENDIENTES.md`:

- ✅ **PRUEBA 1:** ✅ Completada (usuarios creados)
- ⏳ **PRUEBA 2:** Login y redirección del Piloto
- ⏳ **PRUEBA 3:** Login y redirección del Monitor
- ⏳ **PRUEBA 4:** Registrar recogidas (Monitor)
- ⏳ **PRUEBA 5:** Prevención de duplicados
- ⏳ **PRUEBA 6:** Navegación por roles
- ⏳ **PRUEBA 7:** Verificación en base de datos

---

**Fecha:** ${new Date().toLocaleDateString('es-ES')}  
**Estado:** ✅ Listo para ejecutar  
**Tiempo estimado total:** 5-10 minutos  

---

## 🚀 **¡ADELANTE!**

**Ejecuta la aplicación ahora (F5)** y sigue los pasos de arriba.  
Avísame cuando hayas creado los usuarios y probado los logins. 🎉
