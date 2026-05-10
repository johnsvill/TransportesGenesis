# ✅ CHECKLIST DE VERIFICACIÓN RÁPIDA - PILOTO Y MONITOR

## 🎯 PASO 1: Usuarios de Prueba

- [ ] Archivo `Controllers/AdminController.cs` contiene método `CrearUsuariosPrueba()`
- [ ] Método tiene `[HttpGet]` y `[Authorize(Roles = "Administrador")]`
- [ ] Crea usuario `piloto1@transportesgenesis.com` con password `Piloto123!`
- [ ] Crea usuario `monitor1@transportesgenesis.com` con password `Monitor123!`
- [ ] Asigna rol "Piloto" al piloto1
- [ ] Asigna rol "Monitor" al monitor1
- [ ] Asigna Bus #1 mediante `AsignacionPilotoBus` a ambos
- [ ] Valida que Bus #1 existe antes de asignar
- [ ] No duplica usuarios si ya existen
- [ ] Muestra resumen con credenciales al finalizar

**Ruta de acceso**: `/Admin/CrearUsuariosPrueba`

---

## 🎯 PASO 2: Página del Piloto

- [ ] Archivo `Pages/Piloto/MiRuta.cshtml.cs` existe
- [ ] Clase tiene `[Authorize(Roles = "Piloto")]`
- [ ] Constructor inyecta `ApplicationDbContext _context`
- [ ] Método `OnGetAsync()` obtiene `IdPiloto` desde `ClaimTypes.NameIdentifier`
- [ ] Busca asignación activa en `AsignacionesPilotoBusDb` con `EsActual = true`
- [ ] Obtiene `IdBus` desde la asignación (NO hardcodeado)
- [ ] Muestra error si el piloto no tiene bus asignado
- [ ] Lógica de turnos, fechas y API permanece sin cambios
- [ ] NO hay `IdBus = 4` hardcodeado

**Cambio clave**: De `IdBus = 4;` a `IdBus = asignacion.IdBus;`

---

## 🎯 PASO 3.1: Monitor - Mi Ruta

- [ ] Archivo `Pages/Monitor/MiRuta.cshtml` existe
- [ ] Archivo `Pages/Monitor/MiRuta.cshtml.cs` existe
- [ ] Clase tiene `[Authorize(Roles = "Monitor")]`
- [ ] Constructor inyecta `ApplicationDbContext _context`
- [ ] Método `OnGetAsync()` obtiene `IdMonitor` desde `ClaimTypes.NameIdentifier`
- [ ] Busca asignación activa en `AsignacionesPilotoBusDb`
- [ ] Obtiene `IdBus` dinámicamente
- [ ] Vista muestra encabezado con nombre del monitor y bus
- [ ] Vista muestra resumen (paradas totales, alumnos, horarios)
- [ ] Vista tiene botón "Registrar Recogidas de Alumnos"
- [ ] Vista muestra tabla de paradas ordenadas
- [ ] Enlaces a Google Maps funcionan
- [ ] Auto-refresh cada 2 minutos configurado

**Patrón**: Similar a `/Piloto/MiRuta` pero para Monitor

---

## 🎯 PASO 3.2: Monitor - Registrar Recogidas

- [ ] Archivo `Pages/Monitor/RegistrarRecogidas.cshtml` existe
- [ ] Archivo `Pages/Monitor/RegistrarRecogidas.cshtml.cs` existe
- [ ] Clase tiene `[Authorize(Roles = "Monitor")]`
- [ ] Página acepta parámetro `{idRuta:int}` en la ruta
- [ ] Método `OnGetAsync(int idRuta)` carga la ruta desde API
- [ ] Carga registros existentes desde `RegistrosRecogidaDb`
- [ ] Vista muestra alumnos agrupados por parada
- [ ] Checkboxes con formato `Registros_{idParada}_{idAlumno}`
- [ ] Alumnos ya registrados aparecen deshabilitados
- [ ] Contador de alumnos recogidos actualiza en tiempo real
- [ ] Método `OnPostAsync()` guarda en tabla `RegistroRecogida`
- [ ] Guarda `IdParada`, `IdAlumno`, `FechaHoraRecogida`, `ConfirmadoPor`
- [ ] Previene duplicados con validación previa
- [ ] Confirmación JavaScript antes de guardar
- [ ] Feedback visual: cards cambian de color al marcar

**Tabla usada**: `genesis.RegistroRecogida` (existente)

---

## 🎯 PASO 4.1: Redirección de Login

- [ ] Archivo `Controllers/AuthController.cs` modificado
- [ ] Método `Login(LoginViewModel model)` tiene lógica de redirección
- [ ] Obtiene roles con `await _userManager.GetRolesAsync(user)`
- [ ] Redirección para "Administrador" → `RedirectToAction("Index", "Admin")`
- [ ] Redirección para "PadreDeFamilia" → `RedirectToAction("Index", "PagosPadresFamilia")`
- [ ] Redirección para "Piloto" → `RedirectToPage("/Piloto/MiRuta")`
- [ ] Redirección para "Monitor" → `RedirectToPage("/Monitor/MiRuta")` ✅ **NUEVA**
- [ ] Redirección por defecto → `RedirectToAction("Index", "Home")`

**Línea clave agregada**:
```csharp
else if (roles.Contains("Monitor"))
    return RedirectToPage("/Monitor/MiRuta");
```

---

## 🎯 PASO 4.2: Navegación en Layout

- [ ] Archivo `Pages/Shared/_Layout.cshtml` modificado
- [ ] Navegación usa `@if (User.Identity?.IsAuthenticated == true)`
- [ ] Para `User.IsInRole("Piloto")`:
  - [ ] Enlace directo "Mi Ruta" → `/Piloto/MiRuta`
- [ ] Para `User.IsInRole("Monitor")`:
  - [ ] Dropdown "Monitor" con:
    - [ ] "Mi Ruta" → `/Monitor/MiRuta`
    - [ ] "Mapa en Vivo" → `/Geolocalizacion/MapaEnTiempoReal`
- [ ] Para `User.IsInRole("Administrador")`:
  - [ ] Dropdown "Admin" con:
    - [ ] "Calcular Rutas" → `/Admin/CalcularRutas`
    - [ ] "Crear Usuarios Prueba" → `/Admin/CrearUsuariosPrueba`
    - [ ] "Vista Piloto" → `/Piloto/MiRuta`
    - [ ] "Vista Monitor" → `/Monitor/MiRuta`
- [ ] Para `User.IsInRole("PadreDeFamilia")`:
  - [ ] Enlace "Asistencia" → `/Padres/ConfirmarAsistencia`
- [ ] Enlace "Mapa" disponible para todos los roles autenticados
- [ ] Dropdown de usuario con "Cerrar Sesión" → `/Auth/Logout`
- [ ] Enlace "Iniciar Sesión" para usuarios no autenticados

**Navegación condicional**: Cada usuario ve solo sus opciones

---

## 🎯 MODELOS ACTUALIZADOS

### `AsignacionPilotoBus.cs`
- [ ] Archivo `Models/DB/Negocio/AsignacionPilotoBus.cs` modificado
- [ ] Agregada propiedad `public int IdBus { get; set; }`
- [ ] Mantiene `[ForeignKey("IdBus")]` sobre `public Bus Bus { get; set; }`

**Antes**:
```csharp
[ForeignKey("IdBus")]
public Bus Bus { get; set; }
```

**Ahora**:
```csharp
public int IdBus { get; set; }

[ForeignKey("IdBus")]
public Bus Bus { get; set; }
```

---

### `RegistroRecogida.cs`
- [ ] Archivo `Models/DB/Negocio/RegistroRecogida.cs` modificado
- [ ] Agregada propiedad `public int IdParada { get; set; }`
- [ ] Agregada propiedad `public int IdAlumno { get; set; }`
- [ ] Mantiene navegaciones `Parada` y `Alumno`

**Antes**:
```csharp
[ForeignKey("IdParada")]
public Parada Parada { get; set; }
```

**Ahora**:
```csharp
public int IdParada { get; set; }

[ForeignKey("IdParada")]
public Parada Parada { get; set; }
```

---

### `RutaDto.cs` (DTOs)
- [ ] Archivo `DTOs/Ruta/RutaDto.cs` modificado
- [ ] `ParadaRutaDto` tiene propiedad `public string? NombreParada { get; set; }`
- [ ] `ParadaRutaDto` tiene propiedad `public List<AlumnoEnParadaDto>? Alumnos { get; set; }`
- [ ] Clase nueva `AlumnoEnParadaDto` con:
  - [ ] `public int IdAlumno { get; set; }`
  - [ ] `public string NombreCompleto { get; set; }`
  - [ ] `public string? Grado { get; set; }`

---

## 🧪 PRUEBAS FINALES

### Compilación
- [ ] Ejecutar `dotnet build` en terminal
- [ ] Resultado: `Build succeeded. 0 Error(s)`
- [ ] No hay warnings críticos

### Prueba 1: Crear Usuarios
1. [ ] Ejecutar aplicación (`dotnet run` o F5)
2. [ ] Login como admin
3. [ ] Navegar a `/Admin/CrearUsuariosPrueba`
4. [ ] Verificar mensaje de éxito con credenciales

### Prueba 2: Login Piloto
1. [ ] Cerrar sesión
2. [ ] Login con `piloto1@transportesgenesis.com` / `Piloto123!`
3. [ ] Verificar redirección a `/Piloto/MiRuta`
4. [ ] Verificar que muestra "Bus #1"
5. [ ] Verificar menú muestra "Mi Ruta"

### Prueba 3: Login Monitor
1. [ ] Cerrar sesión
2. [ ] Login con `monitor1@transportesgenesis.com` / `Monitor123!`
3. [ ] Verificar redirección a `/Monitor/MiRuta`
4. [ ] Verificar que muestra "Bus #1"
5. [ ] Verificar menú muestra dropdown "Monitor"
6. [ ] Clic en "Registrar Recogidas"
7. [ ] Marcar algunos alumnos
8. [ ] Guardar y verificar éxito
9. [ ] Recargar y verificar "Ya registrado"

### Prueba 4: Navegación Admin
1. [ ] Login como admin
2. [ ] Verificar dropdown "Admin"
3. [ ] Clic en "Vista Piloto" funciona
4. [ ] Clic en "Vista Monitor" funciona

---

## ✅ RESULTADO FINAL

**Status de compilación**: ✅ EXITOSA
**Archivos creados**: 4 nuevos (Monitor páginas)
**Archivos modificados**: 5 (AuthController, Layout, AdminController, 2 modelos)
**Funcionalidad preservada**: ✅ `/Piloto/MiRuta` sin cambios en lógica
**Pruebas manuales**: ⏳ Pendiente

---

## 📋 ARCHIVOS AFECTADOS

### Creados (4)
1. `Pages/Monitor/MiRuta.cshtml`
2. `Pages/Monitor/MiRuta.cshtml.cs`
3. `Pages/Monitor/RegistrarRecogidas.cshtml`
4. `Pages/Monitor/RegistrarRecogidas.cshtml.cs`

### Modificados (7)
1. `Controllers/AdminController.cs` (método `CrearUsuariosPrueba`)
2. `Controllers/AuthController.cs` (redirección Monitor)
3. `Pages/Shared/_Layout.cshtml` (navegación por roles)
4. `Pages/Piloto/MiRuta.cshtml.cs` (lookup dinámico)
5. `Models/DB/Negocio/AsignacionPilotoBus.cs` (FK `IdBus`)
6. `Models/DB/Negocio/RegistroRecogida.cs` (FK `IdParada`, `IdAlumno`)
7. `DTOs/Ruta/RutaDto.cs` (extensión de DTOs)

---

**¡TODO LISTO PARA PRUEBAS!** 🎉
