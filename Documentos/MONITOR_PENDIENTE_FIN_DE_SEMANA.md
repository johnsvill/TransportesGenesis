# 🔧 Monitor - Pendientes y Errores Encontrados

**Fecha**: 09/05/2026 (Viernes - Fin de Semana)  
**Usuario de Prueba**: monitor1@transportesgenesis.com  
**Estado**: ⚠️ NO FUNCIONAL - Error de redirección post-login

---

## 📋 Resumen del Problema

Después de iniciar sesión como Monitor, la aplicación intenta redirigir a una **vista MVC inexistente** llamada `MonitorDashboard` en lugar de la **Razor Page** `/Monitor/MiRuta` que sí existe.

---

## 🐛 Error Principal

### Error en `Controllers/AuthController.cs` - Método `Login`

**Línea problemática** (aproximadamente línea 80-90):
```csharp
else if (roles.Contains("Monitor"))
{
    return View("MonitorDashboard"); // ❌ INCORRECTO - Vista MVC no existe
}
```

**Log del error**:
```
Microsoft.AspNetCore.Mvc.ViewFeatures.ViewResultExecutor: Error: The view 'MonitorDashboard' was not found. 
Searched locations: 
/Views/Home/MonitorDashboard.cshtml
/Views/Shared/MonitorDashboard.cshtml
/Pages/Shared/MonitorDashboard.cshtml
```

### ✅ Solución Correcta

Cambiar a redirección de Razor Page:
```csharp
else if (roles.Contains("Monitor"))
{
    return RedirectToPage("/Monitor/MiRuta"); // ✅ CORRECTO
}
```

---

## 🔍 Investigación Realizada

### 1. Base de Datos Verificada ✅

- **Usuario `monitor1` creado**: ✅
  - Email: monitor1@transportesgenesis.com
  - Password: Admin123! (hash copiado del admin)
  - Rol: Monitor
  - IsFirstLogin: 0 (desactivado para pruebas)

- **Bus #4 asignado al monitor**: ✅
  - IdBus: 4
  - Placa: BUS-001
  - Modelo: Mercedes-Benz Sprinter
  - Capacidad: 25

- **Rutas activas del Bus #4**: ✅
  - Total: 6 rutas (todas tipo "Mañana")
  - Ruta #5: 8 paradas
  - Ruta #6: 8 paradas
  - Alumnos: 8 totales asignados al Bus #4

### 2. Cambios Temporales para Testing

#### En `Pages/Monitor/MiRuta.cshtml.cs`:
```csharp
// ⚠️ MODO TESTING: Desactivar validación de fin de semana
EsFinDeSemana = false; // FORZADO A FALSE PARA TESTING
```

#### En `Pages/Monitor/MiRuta.cshtml`:
```razor
@* ⚠️ MODO TESTING: Badge de fin de semana comentado
@if (Model.EsFinDeSemana)
{
    <span class="badge bg-warning">Fin de Semana</span>
}
*@
```

#### En `Repositories/Implementations/RutaRepository.cs`:
```csharp
// TODO PRODUCCIÓN: Agregar filtro por fecha cuando se implemente gestión de fechas de rutas
// ⚠️ MODO TESTING: Ordenar por cantidad de paradas descendente para priorizar rutas completas
return await _dbSet
    .Where(r => r.TipoRuta == tipoRuta && r.EsActiva)
    .Include(r => r.Bus)
    .Include(r => r.ParadasLink.Where(p => !p.Completada))
        .ThenInclude(p => p.Alumno)
    .OrderByDescending(r => r.ParadasLink.Count) // Priorizar rutas con más paradas
    .ThenBy(r => r.HoraInicio)
    .ToListAsync();
```

### 3. Logs de la Aplicación

**El sistema SÍ está encontrando la ruta del Bus #4**:
```
Microsoft.EntityFrameworkCore.Database.Command: Information: Executed DbCommand (25ms)
SELECT [r].[IdRuta], [r].[Activo], [r].[Descripcion], [r].[EsActiva]...
FROM [genesis].[Rutas] AS [r]
WHERE [r].[TipoRuta] = @__tipoRuta_0 AND [r].[EsActiva] = CAST(1 AS bit)
ORDER BY (SELECT COUNT(*) FROM [genesis].[Paradas] AS [p]...) DESC
```

**La API `/api/rutas/bus/4/activa?tipoRuta=Tarde` responde con 200 OK**:
```
System.Net.Http.HttpClient.Default.ClientHandler: Information: Received HTTP response headers after 645ms - 200
```

---

## 📝 Tareas Pendientes para el Lunes

### 1. ⚠️ CRÍTICO: Corregir Redirección del Monitor

**Archivo**: `Controllers/AuthController.cs`  
**Método**: `Login(LoginViewModel model)`  

**Cambio necesario**:
```csharp
// Buscar la línea que dice:
else if (roles.Contains("Monitor"))
{
    return View("MonitorDashboard"); // ❌ ELIMINAR ESTO
}

// Y reemplazarla por:
else if (roles.Contains("Monitor"))
{
    return RedirectToPage("/Monitor/MiRuta"); // ✅ AGREGAR ESTO
}
```

### 2. Verificar Redirección del Piloto

**Archivo**: `Controllers/AuthController.cs`  
**Método**: `Login(LoginViewModel model)`  

Asegurarse de que la redirección del Piloto sea:
```csharp
else if (roles.Contains("Piloto"))
{
    return RedirectToPage("/Piloto/MiRuta"); // ✅ DEBE SER RedirectToPage
}
```

### 3. Revisar Método `ForceChangePassword`

Ya fue corregido previamente, pero verificar que use `RedirectToPage`:
```csharp
else if (roles.Contains("Monitor"))
{
    return RedirectToPage("/Monitor/MiRuta"); // ✅
}
else if (roles.Contains("Piloto"))
{
    return RedirectToPage("/Piloto/MiRuta"); // ✅
}
```

### 4. TODO de Producción: Filtro de Fecha en Rutas

**Archivo**: `Repositories/Implementations/RutaRepository.cs`  
**Método**: `GetRutasDelDiaAsync(DateTime fecha, string tipoRuta)`

Actualmente **NO filtra por fecha**, solo por tipo de ruta. Agregar:
```csharp
.Where(r => r.TipoRuta == tipoRuta 
         && r.EsActiva 
         && r.FechaRuta.Date == fecha.Date) // 👈 AGREGAR ESTO
```

**Nota**: La tabla `genesis.Rutas` actualmente **NO tiene columna `FechaRuta`**, solo tiene `FechaRegistro`. Considerar:
- Agregar columna `FechaRuta` tipo `DATE`
- O usar `FechaRegistro.Date` como filtro temporal

### 5. Restaurar Validación de Fin de Semana (Producción)

**Archivo**: `Pages/Monitor/MiRuta.cshtml.cs`

Restaurar el código original:
```csharp
// PRODUCCIÓN: Activar validación de días hábiles
EsFinDeSemana = DateTime.Now.DayOfWeek == DayOfWeek.Saturday 
             || DateTime.Now.DayOfWeek == DayOfWeek.Sunday;
```

Y en la vista `Pages/Monitor/MiRuta.cshtml`:
```razor
@if (Model.EsFinDeSemana)
{
    <span class="badge bg-warning">Fin de Semana</span>
}
```

---

## 🗂️ Archivos Modificados (Temporalmente para Testing)

1. ✅ `Pages/Monitor/MiRuta.cshtml.cs` - Desactivado fin de semana
2. ✅ `Pages/Monitor/MiRuta.cshtml` - Comentado badge de fin de semana
3. ✅ `Repositories/Implementations/RutaRepository.cs` - Orden por cantidad de paradas
4. ✅ `Controllers/AuthController.cs` - **PENDIENTE: Corregir redirección Monitor**

---

## 🧪 Scripts SQL Ejecutados

### `asignar_buses_separados.sql`
```sql
-- Asignar Bus #1 al piloto y Bus #4 al monitor
INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
VALUES 
(@PilotoId, 1, GETDATE(), 1, 1, GETDATE()),
(@MonitorId, 4, GETDATE(), 1, 1, GETDATE());
```

### `crear_ruta_hoy_bus4.sql`
```sql
-- Crear ruta de HOY para pruebas de fin de semana
INSERT INTO genesis.Rutas (IdBus, Nombre, Descripcion, TipoRuta, HoraInicio, EsActiva, Activo, FechaRegistro)
VALUES (
    4,
    'Ruta Mañana - ' + FORMAT(@FechaHoy, 'dd/MM/yyyy') + ' (PRUEBA)',
    'Ruta de prueba para testing de fin de semana',
    'Mañana',
    '06:00:00',
    1,
    1,
    GETDATE()
);
```

### Verificación de Datos
```sql
-- Verificar rutas activas del Bus #4
SELECT r.IdRuta, r.IdBus, r.TipoRuta, r.EsActiva, COUNT(p.IdParada) AS TotalParadas 
FROM genesis.Rutas r 
LEFT JOIN genesis.Paradas p ON r.IdRuta = p.IdRuta 
WHERE r.IdBus = 4 AND r.EsActiva = 1 
GROUP BY r.IdRuta, r.IdBus, r.TipoRuta, r.EsActiva;

-- Resultado:
-- Ruta #5: 8 paradas
-- Ruta #6: 8 paradas (la más reciente)
```

---

## ✅ Estado Actual

### Lo que SÍ funciona:
- ✅ Login del monitor1
- ✅ Asignación de Bus #4 al monitor
- ✅ Rutas activas con alumnos y paradas
- ✅ API `/api/rutas/bus/4/activa` responde correctamente
- ✅ Desactivación de validación de fin de semana para testing
- ✅ Base de datos con data de prueba

### Lo que NO funciona:
- ❌ Redirección post-login intenta ir a vista MVC `MonitorDashboard` que no existe
- ❌ Usuario nunca llega a la página `/Monitor/MiRuta`

---

## 🎯 Plan de Prueba para el Lunes

1. **Corregir** `Controllers/AuthController.cs` línea ~88-92
2. **Compilar** y reiniciar la aplicación
3. **Iniciar sesión** como monitor1@transportesgenesis.com / Admin123!
4. **Verificar** que redirecciona a `/Monitor/MiRuta`
5. **Validar** que se muestra:
   - Bus #4 asignado
   - Ruta activa con 8 paradas
   - Lista de alumnos
   - Botón "Registrar Recogidas de Alumnos"
6. **Probar** el flujo completo de registro de recogidas
7. **Restaurar** validación de fin de semana para producción

---

## 📌 Notas Adicionales

- El sistema está bien diseñado: la arquitectura Razor Pages + API + EF Core funciona correctamente
- El único error es una **redirección incorrecta** que usa `View()` en lugar de `RedirectToPage()`
- La data de prueba está lista y funcionando
- Los cambios temporales para testing están bien comentados y documentados

---

## 👤 Usuarios de Prueba Disponibles

| Usuario | Email | Password | Rol | Bus Asignado | Estado |
|---------|-------|----------|-----|--------------|--------|
| admin | admin@transportesgenesis.com | Admin123! | Administrador | - | ✅ Funcional |
| piloto1 | piloto1@transportesgenesis.com | Admin123! | Piloto | Bus #1 | ⚠️ Verificar redirección |
| monitor1 | monitor1@transportesgenesis.com | Admin123! | Monitor | Bus #4 | ❌ Bloqueado por error de redirección |

---

**Creado**: 09/05/2026 22:30 hrs  
**Última actualización**: 09/05/2026 22:30 hrs  
**Prioridad**: 🔴 CRÍTICA - Bloqueante para testing del módulo Monitor
