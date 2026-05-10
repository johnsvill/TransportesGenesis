# 🚀 INSTRUCCIONES: CREAR USUARIOS DE PRUEBA

## ✅ ARCHIVOS CREADOS

He creado dos archivos nuevos:
1. ✅ `Pages/Admin/CrearUsuariosPrueba.cshtml` (Vista)
2. ✅ `Pages/Admin/CrearUsuariosPrueba.cshtml.cs` (Lógica)

---

## 🔧 PASO 1: REINICIAR LA APLICACIÓN

La aplicación está corriendo en modo debug, por lo que necesitas reiniciarla:

### **Opción A: Desde Visual Studio**
1. Presiona `Shift + F5` para **detener** la aplicación
2. Espera unos segundos
3. Presiona `F5` para **iniciar** de nuevo

### **Opción B: Desde Terminal**
Si estás ejecutando con `dotnet run`:
1. Presiona `Ctrl + C` en la terminal para detener
2. Ejecuta de nuevo:
```powershell
dotnet run
```

---

## 🧪 PASO 2: CREAR LOS USUARIOS DE PRUEBA

### 1. **Login como Administrador**
Primero debes iniciar sesión como administrador:
```
Email: admin@transportesgenesis.com
Password: (tu password de admin)
```

### 2. **Navegar al Endpoint de Creación**
Una vez logueado como admin, ve a esta URL:
```
https://localhost:7241/Admin/CrearUsuariosPrueba
```

O desde la consola del navegador (F12), ejecuta:
```javascript
window.location.href = "/Admin/CrearUsuariosPrueba";
```

### 3. **Hacer Clic en el Botón "Crear Usuarios de Prueba"**
La página mostrará un botón grande azul:
```
[+] Crear Usuarios de Prueba
```
Haz clic en él.

### 4. **Verificar Resultado**
Si todo sale bien, verás dos tarjetas con las credenciales:

#### 📋 **Usuario Piloto:**
```
Email: piloto1@transportesgenesis.com
Password: Piloto123!
Rol: Piloto
Bus Asignado: #1
```

#### 📋 **Usuario Monitor:**
```
Email: monitor1@transportesgenesis.com
Password: Monitor123!
Rol: Monitor
Bus Asignado: #1
```

---

## 🔐 PASO 3: PROBAR LOS LOGINS

### **Prueba 1: Login del Piloto**
1. Cierra sesión del administrador
2. Ve a: `/Auth/Login`
3. Introduce:
   - Email: `piloto1@transportesgenesis.com`
   - Password: `Piloto123!`
4. **Resultado esperado:** Debe redirigir a `/Piloto/MiRuta`

---

### **Prueba 2: Login del Monitor**
1. Cierra sesión del piloto
2. Ve a: `/Auth/Login`
3. Introduce:
   - Email: `monitor1@transportesgenesis.com`
   - Password: `Monitor123!`
4. **Resultado esperado:** Debe redirigir a `/Monitor/MiRuta`

---

## ⚠️ POSIBLES ERRORES Y SOLUCIONES

### Error 1: "El rol 'Piloto' no existe en el sistema"
**Causa:** Los roles no están creados en la base de datos.

**Solución:**
```sql
-- Verificar roles existentes
SELECT * FROM AspNetRoles;

-- Si no existen, crearlos (esto debería haberse hecho en el seed inicial)
INSERT INTO AspNetRoles (Id, Name, NormalizedName)
VALUES 
(NEWID(), 'Piloto', 'PILOTO'),
(NEWID(), 'Monitor', 'MONITOR');
```

---

### Error 2: "No existe Bus #1 en la base de datos"
**Causa:** No existe un bus con `IdBus = 1`.

**Solución A (desde Admin UI):**
1. Ir al panel de administración de buses
2. Crear un bus nuevo con ID 1

**Solución B (SQL):**
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

### Error 3: "No se pudo crear el usuario"
**Causa:** La contraseña no cumple con las políticas de ASP.NET Identity.

**Verificar políticas en `Program.cs`:**
```csharp
builder.Services.Configure<IdentityOptions>(options =>
{
    // Configuración de contraseña
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
});
```

Las contraseñas `Piloto123!` y `Monitor123!` cumplen con:
- ✅ Dígito (1, 2, 3)
- ✅ Minúscula (iloto, onitor)
- ✅ Mayúscula (P, M)
- ✅ Carácter especial (!)
- ✅ Longitud mínima (11 caracteres)

---

## 📊 PASO 4: VERIFICAR EN BASE DE DATOS

Después de crear los usuarios, verifica en SQL Server:

### Verificar usuarios creados:
```sql
SELECT 
    u.Id,
    u.UserName,
    u.Email,
    u.Nombre,
    u.Apellido,
    r.Name as Rol
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com');
```

**Resultado esperado:** 2 filas (piloto1 y monitor1)

---

### Verificar asignaciones de bus:
```sql
SELECT 
    a.IdAsignacion,
    u.Email,
    a.IdBus,
    b.Placa,
    a.EsActual,
    a.FechaAsignacion
FROM genesis.AsignacionPilotoBus a
INNER JOIN AspNetUsers u ON a.IdUsuarioPiloto = u.Id
INNER JOIN genesis.Bus b ON a.IdBus = b.IdBus
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com')
    AND a.EsActual = 1;
```

**Resultado esperado:** 2 filas (ambos usuarios asignados al Bus #1)

---

## 🎯 RESUMEN DE COMANDOS RÁPIDOS

### Reiniciar aplicación:
```powershell
# Detener (Shift + F5 en Visual Studio o Ctrl + C en terminal)
# Iniciar (F5 en Visual Studio o):
dotnet run
```

### Navegación rápida (desde navegador, F12 → Console):
```javascript
// Ir a crear usuarios
window.location.href = "/Admin/CrearUsuariosPrueba";

// Ir a login
window.location.href = "/Auth/Login";

// Cerrar sesión
window.location.href = "/Auth/Logout";
```

---

## ✅ CHECKLIST DE VALIDACIÓN

Después de completar los pasos anteriores, verifica:

- [ ] ✅ Aplicación reiniciada sin errores de compilación
- [ ] ✅ Endpoint `/Admin/CrearUsuariosPrueba` accesible
- [ ] ✅ Botón "Crear Usuarios de Prueba" funciona
- [ ] ✅ Se muestran las credenciales de piloto1 y monitor1
- [ ] ✅ Login con piloto1 redirige a `/Piloto/MiRuta`
- [ ] ✅ Login con monitor1 redirige a `/Monitor/MiRuta`
- [ ] ✅ Usuarios verificados en base de datos (query SQL)
- [ ] ✅ Asignaciones de bus verificadas en base de datos (query SQL)

---

## 📞 SI NECESITAS AYUDA

Si encuentras algún error:
1. **Copia el mensaje de error completo** de la consola del navegador (F12)
2. **Copia el mensaje de error** de la consola de Visual Studio (Output)
3. **Ejecuta las queries SQL de verificación** y muestra los resultados

---

**Fecha:** ${new Date().toLocaleDateString('es-ES')}  
**Archivos creados:** 2  
**Estado:** ✅ Listo para reiniciar y probar  

---

## 🚀 PRÓXIMO PASO

**REINICIA LA APLICACIÓN AHORA** y sigue los pasos de arriba. 

Una vez que los usuarios estén creados, podremos continuar con el resto de las pruebas del documento `PRUEBAS_E_INSTRUCCIONES_PENDIENTES.md`.
