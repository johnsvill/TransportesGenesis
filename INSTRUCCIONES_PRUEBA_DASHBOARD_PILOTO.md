# 🧪 INSTRUCCIONES DE PRUEBA - Dashboard Piloto

**Fecha**: Diciembre 2024  
**Desarrollador**: David (Geolocalización)  
**Estado**: ✅ LISTO PARA PROBAR

---

## ✅ **ESTADO DE LA BASE DE DATOS**

### 📊 Datos Existentes (Verificado):

| Tabla | Registros |
|-------|-----------|
| Buses | 6 |
| Rutas | 7 |
| Alumnos | 18 |
| Paradas | 30 |
| AsignacionPilotoBus | 2 |

### 👥 Usuarios Disponibles:

| Usuario | Email | Rol | Bus Asignado | Placa |
|---------|-------|-----|--------------|-------|
| `piloto1` | piloto1@transportesgenesis.com | Piloto | Bus #1 | P-001GT |
| `monitor1` | (verificar en BD) | Monitor | Bus #4 | BUS-001 |

---

## 🧪 PRUEBAS A REALIZAR

### **Prueba 1: Verificar Inicio Automático en Login** ✅

**Pasos:**
1. Cerrar Visual Studio completamente
2. Abrir Visual Studio 2026
3. Abrir el proyecto `TransportesGenesis`
4. Presionar **F5** (Run)
5. Esperar que abra el navegador

**Resultado Esperado:**
```
✅ Debe abrir en: https://localhost:7241/Auth/Login
✅ Debe mostrar la pantalla de login
❌ NO debe abrir en Home/Index
```

---

### **Prueba 2: Login como Piloto** ✅

**Credenciales:**
- **Usuario**: `piloto1@transportesgenesis.com`
- **Contraseña**: (usar la contraseña configurada, probablemente: `Piloto123!` o `Password123!`)

**Pasos:**
1. Ingresar usuario y contraseña
2. Click en "Iniciar Sesión"

**Resultado Esperado:**
```
✅ Login exitoso
✅ Redirigido automáticamente a: /Piloto/MiRuta
✅ NO debe quedarse en la pantalla de login
```

**Si pide cambiar contraseña:**
```
✅ Es correcto si IsFirstLogin = true
✅ Cambiar la contraseña
✅ Después debe redirigir a /Piloto/MiRuta
```

---

### **Prueba 3: Verificar Datos del Dashboard** ✅

**En la pantalla /Piloto/MiRuta, verificar:**

#### **Panel: Información del Piloto**
```
✅ Nombre del Piloto: piloto1 (o el nombre del usuario)
✅ Bus Asignado: Bus #1
✅ Estado: Activo (badge verde)
❌ NO debe decir "Juan Pérez"
❌ NO debe decir "Bus #4"
```

#### **Panel: Ruta Activa**
```
✅ Debe mostrar ruta según la hora:
   - Antes del mediodía (< 12:00 PM) → Ruta Mañana
   - Después del mediodía (>= 12:00 PM) → Ruta Tarde
✅ Debe mostrar tipo de ruta (Mañana/Tarde)
✅ Debe mostrar cantidad de paradas
```

#### **Panel: Listado de Paradas**
```
✅ Debe mostrar lista de paradas
✅ Cada parada debe tener:
   - Orden (1, 2, 3, ...)
   - Dirección
   - Hora estimada
   - Alumnos asignados (si aplica)
✅ La última parada debe ser el colegio (sin alumno)
```

---

### **Prueba 4: Verificar Protección de Rol** 🔒

**Objetivo**: Verificar que solo Pilotos pueden acceder al dashboard

**Pasos:**
1. Cerrar sesión (si está iniciada)
2. Iniciar sesión con un usuario que **NO sea Piloto**:
   - Ejemplo: `padre@test.com` (rol: PadreDeFamilia)
   - O: `admin@transportesgenesis.com` (rol: Administrador)
3. En la barra de direcciones, escribir manualmente:
   ```
   https://localhost:7241/Piloto/MiRuta
   ```
4. Presionar Enter

**Resultado Esperado:**
```
✅ Debe redirigir a: /Auth/AccessDenied
✅ Debe mostrar mensaje "Acceso Denegado"
❌ NO debe permitir ver el dashboard
```

---

### **Prueba 5: Verificar Menú de Navegación** 🧭

**Escenario A: Usuario Piloto**
1. Iniciar sesión como `piloto1`
2. Verificar el menú superior

**Resultado Esperado:**
```
✅ Debe aparecer opción: "Mi Ruta" (con ícono de ubicación)
✅ Al hacer click, debe ir a /Piloto/MiRuta
```

**Escenario B: Usuario NO Piloto**
1. Cerrar sesión
2. Iniciar sesión con usuario PadreDeFamilia o Administrador
3. Verificar el menú superior

**Resultado Esperado:**
```
✅ NO debe aparecer opción "Mi Ruta"
✅ El menú debe mostrar solo opciones según su rol
```

---

### **Prueba 6: Verificar Manejo de Bus No Asignado** ⚠️

**Objetivo**: Ver qué pasa si un piloto NO tiene bus asignado

**Preparación (ejecutar en SQL):**
```sql
-- Desactivar temporalmente la asignación del piloto1
UPDATE genesis.AsignacionPilotoBus 
SET EsActual = 0 
WHERE IdUsuarioPiloto = (SELECT Id FROM AspNetUsers WHERE UserName = 'piloto1');
```

**Pasos:**
1. Iniciar sesión como `piloto1`
2. Ir a `/Piloto/MiRuta`

**Resultado Esperado:**
```
✅ Debe mostrar mensaje de error:
   "No tienes un bus asignado. Por favor contacta al administrador."
✅ NO debe mostrar ruta ni paradas
✅ NO debe crashear la aplicación
```

**Restaurar después de la prueba:**
```sql
-- Reactivar la asignación
UPDATE genesis.AsignacionPilotoBus 
SET EsActual = 1 
WHERE IdUsuarioPiloto = (SELECT Id FROM AspNetUsers WHERE UserName = 'piloto1');
```

---

## 🐛 PROBLEMAS COMUNES Y SOLUCIONES

### **Problema 1: "No se encontró la ruta"**

**Síntomas:**
```
⚠️ Mensaje: "No hay ruta calculada para este bus en el turno actual"
```

**Causas posibles:**
1. No hay rutas para el Bus #1 en el tipo de ruta actual (Mañana/Tarde)
2. La tabla `Rutas` no tiene registros con `IdBus = 1` y `EsActiva = 1`

**Solución:**
```sql
-- Verificar rutas del Bus #1
SELECT * FROM genesis.Rutas WHERE IdBus = 1 AND EsActiva = 1;

-- Si no hay resultados, activar una ruta:
UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdBus = 1 AND TipoRuta = 'Mañana';
```

---

### **Problema 2: "Error de login"**

**Síntomas:**
```
❌ Mensaje: "Credenciales inválidas"
```

**Solución:**
```sql
-- Verificar si el usuario existe
SELECT UserName, Email FROM AspNetUsers WHERE UserName = 'piloto1';

-- Si no existe, crear usuario (ejecutar desde la app o con script)
-- O usar las credenciales del admin:
-- Usuario: admin@transportesgenesis.com
-- Contraseña: Admin123!
```

---

### **Problema 3: "Bus Asignado: Sin asignar"**

**Síntomas:**
```
⚠️ En el dashboard aparece: "Bus Asignado: Sin asignar"
```

**Causa:**
- La tabla `AsignacionPilotoBus` no tiene registro activo para ese usuario

**Solución:**
```sql
-- Verificar asignaciones
SELECT * FROM genesis.AsignacionPilotoBus 
WHERE IdUsuarioPiloto = (SELECT Id FROM AspNetUsers WHERE UserName = 'piloto1');

-- Si existe pero EsActual = 0, activarla:
UPDATE genesis.AsignacionPilotoBus 
SET EsActual = 1 
WHERE IdUsuarioPiloto = (SELECT Id FROM AspNetUsers WHERE UserName = 'piloto1');

-- Si no existe, crear asignación (reemplazar {GUID} con el Id real del usuario):
INSERT INTO genesis.AsignacionPilotoBus 
(IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
VALUES 
((SELECT Id FROM AspNetUsers WHERE UserName = 'piloto1'), 1, GETDATE(), 1, 1, GETDATE());
```

---

### **Problema 4: "AccessDenied al intentar acceder"**

**Síntomas:**
```
❌ Redirige a /Auth/AccessDenied
```

**Causa:**
- El usuario NO tiene el rol "Piloto"

**Solución:**
```sql
-- Verificar rol del usuario
SELECT u.UserName, r.Name AS Rol 
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.UserName = 'piloto1';

-- Si no tiene rol Piloto, asignarlo:
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (
    (SELECT Id FROM AspNetUsers WHERE UserName = 'piloto1'),
    (SELECT Id FROM AspNetRoles WHERE Name = 'Piloto')
);
```

---

## 📝 CHECKLIST DE VERIFICACIÓN

Marcar cada ítem después de probarlo:

- [ ] ✅ Aplicación inicia en `/Auth/Login`
- [ ] ✅ Login con `piloto1` funciona correctamente
- [ ] ✅ Redirección automática a `/Piloto/MiRuta` después del login
- [ ] ✅ Dashboard muestra nombre del piloto (NO "Juan Pérez")
- [ ] ✅ Dashboard muestra bus asignado (Bus #1)
- [ ] ✅ Dashboard muestra ruta activa (Mañana o Tarde según hora)
- [ ] ✅ Dashboard muestra listado de paradas
- [ ] ✅ Dashboard muestra alumnos en cada parada
- [ ] ✅ Menú muestra "Mi Ruta" para usuarios Piloto
- [ ] ✅ Menú NO muestra "Mi Ruta" para otros roles
- [ ] ✅ Protección de rol funciona (AccessDenied para NO Pilotos)
- [ ] ✅ Maneja correctamente "sin bus asignado"
- [ ] ✅ Compila sin errores
- [ ] ✅ No hay errores en consola del navegador (F12)

---

## 🚀 SIGUIENTE PASO DESPUÉS DE LAS PRUEBAS

Si todas las pruebas pasan:

1. ✅ **Commit de cambios:**
   ```bash
   git add .
   git commit -m "feat: Implementar dashboard de Piloto con datos dinámicos y protección de roles"
   git push origin dev_david
   ```

2. ✅ **Notificar a Jonathan:**
   ```
   Hola Jonathan,

   He completado el dashboard de Piloto con las siguientes mejoras:

   ✅ Eliminados datos hardcodeados
   ✅ Datos ahora se obtienen del usuario autenticado
   ✅ Dashboard protegido con [Authorize(Roles = "Piloto")]
   ✅ Inicio automático en /Auth/Login
   ✅ Todas las pruebas pasaron correctamente

   Puedes revisar los cambios en la rama dev_david.
   Documentación completa en:
   - RESUMEN_CAMBIOS_DASHBOARD_PILOTO.md
   - INSTRUCCIONES_PRUEBA_DASHBOARD_PILOTO.md

   Listo para merge cuando lo revises.

   Saludos,
   David
   ```

---

## 📞 CONTACTO

Si encuentras algún problema durante las pruebas:

1. Verificar logs en:
   - Consola de Visual Studio (Output Window)
   - Consola del navegador (F12 → Console)

2. Verificar datos en SQL:
   - Usar scripts de verificación de este documento
   - Revisar `Scripts/README.md` para más ayuda

3. Revisar documentación:
   - `RESUMEN_CAMBIOS_DASHBOARD_PILOTO.md`
   - `ANALISIS_VALIDACION_LOGIN_PRIMER_INGRESO.md`

---

**Fecha**: Diciembre 2024  
**Última Actualización**: Después de verificar datos en base de datos  
**Estado**: ✅ LISTO PARA PRUEBAS

---

*Documento generado como guía de pruebas para el dashboard de Piloto en TransportesGenesis.*
