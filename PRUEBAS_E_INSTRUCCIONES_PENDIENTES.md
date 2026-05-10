# 🧪 PRUEBAS E INSTRUCCIONES PENDIENTES - ÁREAS PILOTO Y MONITOR

## 📋 Estado del Proyecto

**Fecha de implementación**: Completada
**Rama actual**: `dev_david`
**Estado de compilación**: ✅ EXITOSA
**Próximo paso**: PRUEBAS FUNCIONALES

---

## ⚠️ REQUISITOS PREVIOS ANTES DE PROBAR

### 1. Base de Datos
- [ ] **Verificar que existe el Bus #1** en la tabla `genesis.Bus`
  ```sql
  SELECT * FROM genesis.Bus WHERE IdBus = 1;
  ```
  - Si no existe, crear el bus primero desde el panel de administración

### 2. Datos de Prueba
- [ ] **Verificar que existen alumnos** en la tabla `genesis.Alumnos`
- [ ] **Verificar que existen paradas** en la tabla `genesis.Parada`
- [ ] **Opcional**: Tener rutas ya calculadas para el Bus #1

### 3. Roles del Sistema
- [ ] Los roles `Piloto` y `Monitor` ya están configurados en `Startup.cs`
- [ ] El admin ya existe (creado en el seed inicial)

---

## 🚀 INSTRUCCIONES PARA EJECUTAR LA APLICACIÓN

### Opción 1: Desde Visual Studio
1. Presionar **F5** o hacer clic en el botón "Play" (▶️)
2. Esperar a que se abra el navegador automáticamente
3. La URL será algo como: `https://localhost:7XXX` o `http://localhost:5XXX`

### Opción 2: Desde Terminal (PowerShell)
```powershell
# Navegar al directorio del proyecto
cd C:\Proyectos\TransportesGenesis

# Ejecutar la aplicación
dotnet run

# Salida esperada:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: https://localhost:7XXX
#       Now listening on: http://localhost:5XXX
```

### Opción 3: Compilar y Ejecutar
```powershell
# Limpiar solución
dotnet clean

# Compilar
dotnet build

# Ejecutar
dotnet run
```

---

## 🧪 PLAN DE PRUEBAS FUNCIONALES

### PRUEBA 1: Crear Usuarios de Prueba (Piloto y Monitor)

**Objetivo**: Crear los usuarios `piloto1` y `monitor1` con sus respectivos roles y asignación de bus.

#### Pasos:
1. [ ] **Ejecutar la aplicación** (F5 o `dotnet run`)
2. [ ] **Abrir navegador** en la URL mostrada
3. [ ] **Login como Administrador**:
   - Email: `admin@transportesgenesis.com`
   - Password: (el configurado en el seed)
4. [ ] **Navegar a**: `/Admin/CrearUsuariosPrueba`
   - URL completa: `https://localhost:XXXX/Admin/CrearUsuariosPrueba`
5. [ ] **Verificar el resultado en pantalla**:
   - ✅ Debe mostrar: "Usuario Piloto creado exitosamente"
   - ✅ Debe mostrar: "Bus #1 asignado al Piloto"
   - ✅ Debe mostrar: "Usuario Monitor creado exitosamente"
   - ✅ Debe mostrar: "Bus #1 asignado al Monitor"
   - ✅ Debe mostrar el resumen con credenciales:
     ```
     📋 USUARIOS DE PRUEBA
     ========================================

     🔐 PILOTO:
        Email: piloto1@transportesgenesis.com
        Password: Piloto123!
        Rol: Piloto
        Bus Asignado: #1

     🔐 MONITOR:
        Email: monitor1@transportesgenesis.com
        Password: Monitor123!
        Rol: Monitor
        Bus Asignado: #1
     ```

#### ⚠️ Posibles Errores:
- **Error**: "No existe Bus #1 en la base de datos"
  - **Solución**: Crear el bus primero desde el panel de administración
- **Error**: "Usuario ya existe"
  - **Solución**: Normal si ya ejecutaste este endpoint antes. Los usuarios no se duplican.

#### ✅ Resultado Esperado:
- Dos nuevos usuarios creados en `AspNetUsers`
- Dos nuevos registros en `genesis.AsignacionPilotoBus` con `EsActual = true`
- Ambos usuarios asignados al Bus #1

---

### PRUEBA 2: Login y Redirección del Piloto

**Objetivo**: Verificar que el piloto puede loguearse y es redirigido automáticamente a su página.

#### Pasos:
1. [ ] **Cerrar sesión** del administrador:
   - Menú superior → Dropdown del usuario → "Cerrar Sesión"
   - O navegar directamente a: `/Auth/Logout`
2. [ ] **Ir a la página de login**: `/Auth/Login`
3. [ ] **Introducir credenciales del piloto**:
   - Email: `piloto1@transportesgenesis.com`
   - Password: `Piloto123!`
4. [ ] **Hacer clic en "Iniciar Sesión"**
5. [ ] **Verificar redirección automática**:
   - Debe redirigir a: `/Piloto/MiRuta`
6. [ ] **Verificar contenido de la página**:
   - ✅ Encabezado debe mostrar: "Mi Ruta - Piloto"
   - ✅ Debe mostrar el nombre del usuario logueado
   - ✅ Debe mostrar: "Bus #1" (obtenido dinámicamente, NO hardcodeado)
   - ✅ Debe mostrar el turno actual: "Mañana" o "Tarde" según la hora
   - ✅ Debe mostrar la fecha de la ruta
7. [ ] **Verificar el menú de navegación**:
   - ✅ Debe mostrar enlace "Mi Ruta" en el menú superior
   - ✅ Debe mostrar el nombre del usuario con dropdown
   - ✅ NO debe mostrar opciones de Admin o Monitor

#### ⚠️ Posibles Escenarios:
- **Escenario 1**: Si NO hay rutas calculadas para el Bus #1
  - Mensaje esperado: "No hay ruta calculada para este bus en el turno actual"
  - **Solución**: Ir a `/Admin/CalcularRutas` y calcular rutas para el Bus #1

- **Escenario 2**: Es fin de semana (sábado o domingo)
  - Mensaje esperado: "Es fin de semana. La próxima ruta será el [fecha]"
  - **Solución**: Normal, la ruta se mostrará el próximo día hábil

- **Escenario 3**: Hay rutas calculadas
  - ✅ Debe mostrar la tabla de paradas con:
    - Orden de parada
    - Nombre del alumno
    - Hora estimada
    - Coordenadas (enlace a Google Maps)

#### ✅ Resultado Esperado:
- Login exitoso sin errores
- Redirección automática a `/Piloto/MiRuta`
- Bus #1 obtenido dinámicamente desde `AsignacionPilotoBus`
- Página funcional (misma lógica que antes, solo con autorización y lookup dinámico)

---

### PRUEBA 3: Login y Redirección del Monitor

**Objetivo**: Verificar que el monitor puede loguearse y acceder a su área.

#### Pasos:
1. [ ] **Cerrar sesión** del piloto
2. [ ] **Ir a la página de login**: `/Auth/Login`
3. [ ] **Introducir credenciales del monitor**:
   - Email: `monitor1@transportesgenesis.com`
   - Password: `Monitor123!`
4. [ ] **Hacer clic en "Iniciar Sesión"**
5. [ ] **Verificar redirección automática**:
   - Debe redirigir a: `/Monitor/MiRuta`
6. [ ] **Verificar contenido de la página**:
   - ✅ Encabezado debe mostrar: "Mi Ruta - Monitor"
   - ✅ Debe mostrar el nombre del usuario logueado
   - ✅ Debe mostrar: "Bus #1"
   - ✅ Debe mostrar el turno actual
   - ✅ Debe mostrar resumen con:
     - Paradas Totales
     - Alumnos Totales
     - Hora Inicio
     - Hora Fin
7. [ ] **Verificar el menú de navegación**:
   - ✅ Debe mostrar dropdown "Monitor" con:
     - "Mi Ruta"
     - "Mapa en Vivo"
   - ✅ Debe mostrar el nombre del usuario con dropdown
   - ✅ NO debe mostrar opciones de Admin o Piloto

#### ⚠️ Posibles Escenarios:
- Mismo comportamiento que el Piloto si no hay rutas calculadas
- Si hay rutas, debe mostrar el botón destacado: **"Registrar Recogidas de Alumnos"**

#### ✅ Resultado Esperado:
- Login exitoso sin errores
- Redirección automática a `/Monitor/MiRuta`
- Bus #1 obtenido dinámicamente
- Vista similar a la del piloto pero con botón para registrar recogidas

---

### PRUEBA 4: Registrar Recogidas de Alumnos (Monitor)

**Objetivo**: Verificar que el monitor puede registrar qué alumnos han sido recogidos en cada parada.

#### Pre-requisitos:
- ✅ Estar logueado como `monitor1`
- ✅ Tener rutas calculadas para el Bus #1
- ✅ Las rutas deben tener paradas con alumnos asignados

#### Pasos:
1. [ ] **Desde `/Monitor/MiRuta`**, hacer clic en el botón:
   - **"Registrar Recogidas de Alumnos"**
2. [ ] **Verificar redirección**:
   - Debe ir a: `/Monitor/RegistrarRecogidas?idRuta=X`
3. [ ] **Verificar contenido de la página**:
   - ✅ Encabezado: "Registrar Recogidas"
   - ✅ Debe mostrar nombre del monitor y bus
   - ✅ Resumen superior con:
     - Paradas Totales
     - Alumnos Totales
     - Alumnos Recogidos (inicialmente 0)
4. [ ] **Verificar agrupación por paradas**:
   - ✅ Debe haber un card por cada parada
   - ✅ Cada card debe mostrar:
     - Número de orden de la parada
     - Nombre de la parada
     - Hora estimada
5. [ ] **Verificar lista de alumnos**:
   - ✅ Dentro de cada parada, debe haber tarjetas de alumnos
   - ✅ Cada tarjeta debe mostrar:
     - Nombre completo del alumno
     - Grado
     - Checkbox: "Marcar como recogido"
6. [ ] **Marcar algunos alumnos como recogidos**:
   - Hacer clic en los checkboxes de 2-3 alumnos
7. [ ] **Verificar feedback visual**:
   - ✅ Las tarjetas marcadas deben cambiar de color
   - ✅ El icono debe cambiar a check verde
   - ✅ El contador superior "Alumnos Recogidos" debe actualizarse
8. [ ] **Hacer clic en "Guardar Registros de Recogida"**
9. [ ] **Verificar confirmación**:
   - ✅ Debe aparecer un popup de confirmación
   - ✅ Confirmar el guardado
10. [ ] **Verificar resultado**:
    - ✅ Mensaje de éxito: "Se registraron X alumno(s) exitosamente"
    - ✅ La página debe recargar
11. [ ] **Verificar persistencia**:
    - ✅ Los alumnos marcados ahora deben aparecer como "Ya registrado"
    - ✅ Sus checkboxes deben estar deshabilitados
    - ✅ Debe mostrar la fecha/hora del registro

#### ⚠️ Posibles Errores:
- **Error**: "No se pudo cargar la información de la ruta"
  - **Solución**: Verificar que el `idRuta` en la URL es válido
- **Error**: "No has seleccionado ningún alumno para registrar"
  - **Solución**: Marcar al menos un alumno antes de guardar

#### ✅ Resultado Esperado:
- Registros guardados en la tabla `genesis.RegistroRecogida`
- Campos guardados:
  - `IdParada`: ID de la parada
  - `IdAlumno`: ID del alumno
  - `FechaHoraRecogida`: Timestamp actual
  - `ConfirmadoPor`: ID del usuario monitor
  - `AlumnoPresente`: `true`
- Al recargar, los alumnos registrados aparecen deshabilitados
- Prevención de duplicados funcional

---

### PRUEBA 5: Verificar Prevención de Duplicados

**Objetivo**: Confirmar que no se pueden registrar dos veces el mismo alumno en la misma parada.

#### Pasos:
1. [ ] **Repetir PRUEBA 4**: Registrar algunos alumnos
2. [ ] **Sin cerrar sesión**, volver a `/Monitor/RegistrarRecogidas`
3. [ ] **Verificar que los alumnos ya registrados**:
   - ✅ Aparecen con badge "Recogido"
   - ✅ Checkboxes deshabilitados
   - ✅ Fecha/hora del registro anterior visible
4. [ ] **Intentar marcar solo alumnos nuevos**:
   - Marcar alumnos que NO estén deshabilitados
5. [ ] **Guardar nuevamente**
6. [ ] **Verificar**:
   - ✅ Solo se guardan los nuevos registros
   - ✅ Los antiguos no se duplican

#### ✅ Resultado Esperado:
- La tabla `genesis.RegistroRecogida` NO tiene registros duplicados
- Query de validación:
  ```sql
  SELECT IdParada, IdAlumno, COUNT(*) as Duplicados
  FROM genesis.RegistroRecogida
  GROUP BY IdParada, IdAlumno
  HAVING COUNT(*) > 1;
  ```
  - Debe retornar **0 filas** (sin duplicados)

---

### PRUEBA 6: Navegación por Roles

**Objetivo**: Verificar que cada usuario ve solo las opciones de su rol.

#### Sub-prueba 6.1: Menú del Piloto
1. [ ] Login como `piloto1`
2. [ ] **Verificar menú superior**:
   - ✅ Enlace "Inicio"
   - ✅ Enlace "Mi Ruta" (directo, sin dropdown)
   - ✅ Enlace "Mapa"
   - ✅ Dropdown con nombre del usuario
   - ❌ NO debe ver opciones de Admin
   - ❌ NO debe ver opciones de Monitor
3. [ ] **Intentar acceso directo a `/Monitor/MiRuta`**:
   - Navegar manualmente a: `https://localhost:XXXX/Monitor/MiRuta`
   - ✅ Debe redirigir a `/Auth/AccessDenied` (403 Forbidden)

#### Sub-prueba 6.2: Menú del Monitor
1. [ ] Cerrar sesión y login como `monitor1`
2. [ ] **Verificar menú superior**:
   - ✅ Enlace "Inicio"
   - ✅ Dropdown "Monitor" con:
     - "Mi Ruta"
     - "Mapa en Vivo"
   - ✅ Enlace "Mapa"
   - ✅ Dropdown con nombre del usuario
   - ❌ NO debe ver opciones de Admin
   - ❌ NO debe ver opciones de Piloto
3. [ ] **Intentar acceso directo a `/Piloto/MiRuta`**:
   - Navegar manualmente a: `https://localhost:XXXX/Piloto/MiRuta`
   - ✅ Debe redirigir a `/Auth/AccessDenied` (403 Forbidden)

#### Sub-prueba 6.3: Menú del Administrador
1. [ ] Cerrar sesión y login como admin
2. [ ] **Verificar menú superior**:
   - ✅ Enlace "Inicio"
   - ✅ Dropdown "Admin" con:
     - "Calcular Rutas"
     - "Crear Usuarios Prueba"
     - Separador
     - "Vista Piloto"
     - "Vista Monitor"
   - ✅ Enlace "Mapa"
   - ✅ Dropdown con nombre del usuario
3. [ ] **Verificar acceso a todas las vistas**:
   - Hacer clic en "Vista Piloto" → debe abrir `/Piloto/MiRuta` ✅
   - Hacer clic en "Vista Monitor" → debe abrir `/Monitor/MiRuta` ✅
   - Ambos deben funcionar sin errores de autorización

#### ✅ Resultado Esperado:
- Cada rol ve solo sus opciones
- Intentos de acceso no autorizado son bloqueados
- Admin puede acceder a todas las vistas

---

### PRUEBA 7: Verificación en Base de Datos

**Objetivo**: Confirmar que los datos se guardaron correctamente en la base de datos.

#### Consultas SQL de Verificación:

```sql
-- 1. Verificar usuarios creados
SELECT u.Id, u.UserName, u.Email, r.Name as Rol
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email IN ('piloto1@transportesgenesis.com', 'monitor1@transportesgenesis.com');

-- Resultado esperado: 2 filas
-- piloto1 con rol "Piloto"
-- monitor1 con rol "Monitor"

-- 2. Verificar asignaciones de bus
SELECT 
    a.IdAsignacion,
    a.IdUsuarioPiloto,
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

-- Resultado esperado: 2 filas
-- piloto1 asignado al Bus #1
-- monitor1 asignado al Bus #1

-- 3. Verificar registros de recogidas (después de PRUEBA 4)
SELECT 
    r.IdRegistro,
    p.Nombre as Parada,
    a.NombreCompleto as Alumno,
    r.FechaHoraRecogida,
    u.Email as ConfirmadoPor,
    r.AlumnoPresente
FROM genesis.RegistroRecogida r
INNER JOIN genesis.Parada p ON r.IdParada = p.IdParada
INNER JOIN genesis.Alumnos a ON r.IdAlumno = a.IdAlumno
INNER JOIN AspNetUsers u ON r.ConfirmadoPor = u.Id
WHERE u.Email = 'monitor1@transportesgenesis.com'
ORDER BY r.FechaHoraRecogida DESC;

-- Resultado esperado: X filas (según cuántos alumnos marcaste)
-- Cada fila debe tener:
--   - Nombre de parada
--   - Nombre del alumno
--   - Fecha/hora del registro
--   - Email del monitor (monitor1@transportesgenesis.com)
--   - AlumnoPresente = 1 (true)

-- 4. Verificar que no hay duplicados
SELECT IdParada, IdAlumno, COUNT(*) as Duplicados
FROM genesis.RegistroRecogida
GROUP BY IdParada, IdAlumno
HAVING COUNT(*) > 1;

-- Resultado esperado: 0 filas (sin duplicados)
```

#### ✅ Resultado Esperado:
- Todas las consultas retornan los datos esperados
- No hay duplicados en `RegistroRecogida`
- Las asignaciones de bus son correctas
- Los roles están asignados correctamente

---

## 🐛 TROUBLESHOOTING - ERRORES COMUNES

### Error 1: "No existe Bus #1 en la base de datos"
**Causa**: La tabla `genesis.Bus` no tiene un registro con `IdBus = 1`

**Solución**:
```sql
-- Verificar si existe
SELECT * FROM genesis.Bus WHERE IdBus = 1;

-- Si no existe, insertar manualmente
INSERT INTO genesis.Bus (Placa, Modelo, Capacidad, Activo, FechaRegistro)
VALUES ('ABC-123', 'Modelo X', 40, 1, GETDATE());
```

O crear desde el panel de administración.

---

### Error 2: "No tienes un bus asignado actualmente"
**Causa**: El usuario no tiene un registro en `AsignacionPilotoBus` con `EsActual = true`

**Solución**:
1. Ejecutar de nuevo `/Admin/CrearUsuariosPrueba` (no duplicará usuarios)
2. O insertar manualmente:
```sql
INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
VALUES 
('ID_DEL_PILOTO', 1, GETDATE(), 1, 1, GETDATE());
```

---

### Error 3: "No hay ruta calculada para este bus"
**Causa**: No existen rutas calculadas para el Bus #1 en la fecha actual

**Solución**:
1. Login como admin
2. Ir a: `/Admin/CalcularRutas`
3. Calcular rutas para el Bus #1
4. Asegurarse de seleccionar la fecha actual
5. Asegurarse de seleccionar el turno correcto ("Mañana" o "Tarde")

---

### Error 4: Acceso denegado (403 Forbidden)
**Causa**: Usuario intenta acceder a una página sin autorización

**Comportamiento esperado**: Esto es correcto. Solo debe aparecer si:
- Un piloto intenta acceder a `/Monitor/MiRuta`
- Un monitor intenta acceder a `/Piloto/MiRuta`
- Un usuario sin rol intenta acceder a páginas protegidas

**Solución**: Normal, la seguridad está funcionando correctamente.

---

### Error 5: "No se pudo cargar la información de la ruta"
**Causa**: El `idRuta` en la URL no es válido o no existe

**Solución**:
1. Volver a `/Monitor/MiRuta`
2. Hacer clic de nuevo en "Registrar Recogidas"
3. Verificar que la URL tiene el formato: `/Monitor/RegistrarRecogidas/123`

---

## 📊 VALIDACIÓN FINAL

### Checklist de Validación Completa

- [ ] ✅ Compilación sin errores
- [ ] ✅ Aplicación ejecutándose sin crashes
- [ ] ✅ Usuarios `piloto1` y `monitor1` creados
- [ ] ✅ Ambos usuarios tienen Bus #1 asignado
- [ ] ✅ Login del piloto redirige a `/Piloto/MiRuta`
- [ ] ✅ Login del monitor redirige a `/Monitor/MiRuta`
- [ ] ✅ Piloto ve "Bus #1" (dinámico, no hardcodeado)
- [ ] ✅ Monitor puede ver su ruta
- [ ] ✅ Monitor puede registrar recogidas
- [ ] ✅ Prevención de duplicados funciona
- [ ] ✅ Navegación por roles es correcta
- [ ] ✅ Admin puede acceder a todas las vistas
- [ ] ✅ Usuarios no autorizados son bloqueados
- [ ] ✅ Datos guardados correctamente en BD

---

## 📝 NOTAS IMPORTANTES

### ⚠️ Consideraciones de Producción
- Los usuarios `piloto1` y `monitor1` son SOLO para pruebas
- En producción, usar el módulo de gestión de usuarios real
- El endpoint `/Admin/CrearUsuariosPrueba` es temporal
- Considerar eliminar o proteger este endpoint en producción

### 🔐 Seguridad
- Las contraseñas de prueba (`Piloto123!`, `Monitor123!`) son simples
- En producción, usar contraseñas más robustas
- La autorización está implementada a nivel de `PageModel` con `[Authorize(Roles = "X")]`
- ASP.NET Core Identity maneja la autenticación

### 🗃️ Base de Datos
- No se crearon nuevas tablas (se reutilizaron las existentes)
- Solo se agregaron propiedades FK explícitas en los modelos
- No se requieren migraciones adicionales si las tablas ya existen

---

## 🎯 PRÓXIMOS PASOS DESPUÉS DE LAS PRUEBAS

### Si las pruebas son exitosas:
1. [ ] **Commit de los cambios**:
   ```powershell
   git add .
   git commit -m "feat: Implementación de áreas para Piloto y Monitor con autorización por roles"
   git push origin dev_david
   ```

2. [ ] **Crear Pull Request** a la rama principal

3. [ ] **Documentar** en el README del proyecto

### Si se encuentran errores:
1. [ ] Documentar el error en detalle
2. [ ] Revisar logs de la aplicación
3. [ ] Consultar este documento para soluciones rápidas
4. [ ] Solicitar apoyo si es necesario

---

## 📞 CONTACTO Y SOPORTE

**Repositorio**: https://github.com/johnsvill/TransportesGenesis
**Rama actual**: `dev_david`
**Archivos de referencia**:
- `VERIFICACION_IMPLEMENTACION_PILOTO_MONITOR.md` (verificación técnica completa)
- `CHECKLIST_VERIFICACION.md` (checklist rápido)
- `PLAN_IMPLEMENTACION_PILOTO_MONITOR.md` (plan original)

---

**Fecha de creación**: ${new Date().toLocaleDateString('es-ES', { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
**Preparado por**: GitHub Copilot
**Estado**: ⏳ PENDIENTE DE PRUEBAS

---

## ✅ CUANDO COMPLETES LAS PRUEBAS

Por favor, actualiza este archivo marcando las casillas completadas y añade cualquier observación o error encontrado en la sección de notas.

**¡Buena suerte con las pruebas!** 🚀
