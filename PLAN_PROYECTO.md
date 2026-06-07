# 📋 Plan de Proyecto - TransportesGenesis

## 🎯 Objetivo General
Sistema de gestión de transporte escolar con seguimiento en tiempo real, dashboards por rol y gestión de asignaciones.

---

## ✅ COMPLETADO (100%)

### 1. Migraciones y Base de Datos
- ✅ Migraciones unificadas en carpeta `/Migrations`
- ✅ Tablas de geolocalización en esquema `genesis`
- ✅ Migración de pagos integrada
- ✅ Seed data para roles, usuarios de prueba (pilotos, monitores, admin)
- ✅ Sin datos quemados en código

### 2. Autenticación y Primer Ingreso
- ✅ Redirección automática al Login al iniciar la app
- ✅ Validación de primer ingreso (`IsFirstLogin = true`)
- ✅ Cambio obligatorio de contraseña para:
  - Padre de familia
  - Piloto
  - Monitor
- ✅ Campo `DebeCambiarPassword` verificado en login
- ✅ Redirección post-login según rol

### 3. Dashboard Administrador
- ✅ Página: `/Admin/Index`
- ✅ Autorización: `[Authorize(Roles = "Administrador")]`
- ✅ Botones de navegación:
  - ✅ Reportes de Rutas → `/Admin/ReporteRutas`
  - ✅ Reportes de Asignaciones → `/Admin/ReporteAsignaciones`
  - ✅ Gestionar Asignaciones → `/Admin/GestionarAsignaciones`
  - ✅ Planner de Rutas → `/Planner`
- ✅ Menú de navegación con opciones visibles solo para Administrador

### 4. Dashboard Piloto
- ✅ Página: `/Piloto/MiRuta`
- ✅ Autorización: `[Authorize(Roles = "Piloto")]`
- ✅ Redirección automática al login como Piloto
- ✅ Muestra:
  - ✅ Ruta asignada al piloto
  - ✅ Paradas de la ruta
  - ✅ Listado de alumnos por parada
  - ✅ Panel de alertas de proximidad y retraso
  - ✅ Mapa con ubicación en tiempo real del bus
- ✅ Integración con `IPilotoService` para obtener bus asignado
- ✅ Llamadas a API de rutas activas

### 5. Dashboard Monitor
- ✅ Página: `/Monitor/MiRuta`
- ✅ Autorización: `[Authorize(Roles = "Monitor")]`
- ✅ Funcionalidades similares al Dashboard Piloto
- ✅ Vista de rutas, paradas y alumnos asignados
- ✅ Panel de alertas
- ✅ Mapa en tiempo real

### 6. Gestión de Asignaciones Personal-Bus
- ✅ Página: `/Admin/GestionarAsignaciones`
- ✅ Funcionalidades:
  - ✅ **Crear Asignación:** Asignar piloto/monitor a un bus
    - ✅ Validación: Usuario/Bus no pueden estar vacíos
    - ✅ Validación: Usuario no puede tener asignación activa
    - ✅ Validación: Bus no puede tener asignación activa
    - ✅ Eliminación automática de asignaciones inactivas por restricción de índice único
  - ✅ **Finalizar Asignación:** Marcar asignación como inactiva (`EsActual = false`)
    - ✅ Validación: ID de asignación válido
    - ✅ Validación: Asignación debe estar activa
    - ✅ Actualización de `FechaFinAsignacion`
  - ✅ **Listado de Asignaciones Activas:** Tabla con información completa
  - ✅ **Filtros automáticos:** Usuarios y buses ya asignados no aparecen en dropdowns
- ✅ Seguridad Antiforgery: Tokens explícitos en formularios
- ✅ Arquitectura:
  - ✅ Un solo handler POST: `OnPostAsync()`
  - ✅ Campo oculto `action` para distinguir entre Crear/Finalizar
  - ✅ Delegación a métodos privados: `CrearAsignacionAsync()` / `FinalizarAsignacionAsync()`
  - ✅ Validaciones completamente separadas
- ✅ Logging exhaustivo para depuración
- ✅ Manejo de excepciones con inner exceptions detalladas

### 7. Reportes
- ✅ **Reporte de Rutas:** `/Admin/ReporteRutas`
  - ✅ Lista de rutas con buses asignados
  - ✅ Paradas vinculadas (`ParadasLink`)
  - ✅ Datos desde SQL, no hardcodeados
- ✅ **Reporte de Asignaciones:** `/Admin/ReporteAsignaciones`
  - ✅ Cruza `AsignacionPilotoBus`, `AspNetUsers`, `AspNetUserRoles`, `AspNetRoles`
  - ✅ Muestra piloto/monitor, bus, fechas de asignación
  - ✅ Filtros por estado activo/inactivo

### 8. Servicios
- ✅ `IPilotoService` / `PilotoService`
  - ✅ `GetAsignacionActualAsync()`: Obtener asignación activa del piloto
  - ✅ `GetIdBusAsignadoAsync()`: Obtener ID del bus asignado
  - ✅ Integrado con `ApplicationDbContext`
- ✅ Registrado en `Program.cs` con DI

### 9. Modelos y Entidades
- ✅ `AsignacionPilotoBus`:
  - ✅ `IdBus` explícito en lugar de solo navegación
  - ✅ Hereda de `Auditoria` (Activo, FechaRegistro)
  - ✅ Foreign Key a `Bus`
  - ✅ Campos: `IdAsignacion`, `IdUsuarioPiloto`, `IdBus`, `FechaAsignacion`, `FechaFinAsignacion`, `EsActual`, `Activo`
- ✅ `AppUser`: Usuario con `IsFirstLogin`, `LastLoginDate`
- ✅ `Bus`, `Ruta`, `Parada`: Entidades de geolocalización

### 10. Configuraciones de Entity Framework
- ✅ `AsignacionPilotoBusConfig`:
  - ✅ Índices en `IdUsuarioPiloto`, `FechaAsignacion`, `EsActual`
  - ⚠️ **Problema conocido:** Existe índice único en `IdBus` en la BD (no definido en configuración)
    - 🔧 Solución aplicada: Eliminación automática de asignaciones inactivas
    - 📌 Recomendación: Eliminar índice único de la BD con script SQL

---

## ⚠️ PENDIENTE / PARCIAL

### 11. Mapa Interactivo de Paradas
- ✅ **Completado (90%):**
  - Integración de mapa en dashboards Piloto/Monitor
  - Visualización de ubicación en tiempo real
  - Paradas marcadas en el mapa
- ⚠️ **PENDIENTE (10%):**
  - **Vista de mapa con paradas marcables por el usuario**
    - Interfaz para que el administrador pueda hacer clic en el mapa y agregar/editar paradas
    - Funcionalidad drag-and-drop para mover paradas existentes
    - Formulario modal para editar detalles de parada (nombre, horario, etc.)
  - **Herramientas de edición:**
    - Botón "Agregar Parada" que permita hacer clic en el mapa
    - Guardar coordenadas lat/lng directamente desde el mapa
    - Integración con la tabla `genesis.Paradas`

### 12. Planner de Rutas
- ✅ Página existente: `/Planner/Index`
- ⚠️ Estado: Requiere revisión/actualización
- 📌 Funcionalidades esperadas:
  - Crear/editar rutas
  - Asignar paradas a rutas
  - Visualización de rutas en mapa
  - Optimización de recorridos

---

## 🔧 MEJORAS TÉCNICAS APLICADAS

### Problema Resuelto: Conflicto de Handlers en Razor Pages
**Descripción:** El botón "Finalizar" ejecutaba la validación del handler de creación.

**Causa:** Uso de handlers nombrados (`OnPostAsync()` vs `OnPostFinalizarAsync()`) con binding automático de parámetros.

**Solución Implementada:**
1. Eliminado `[BindProperty]` de `IdUsuario` e `IdBus`
2. Binding explícito con `[FromForm]` en `OnPostAsync()`
3. Campo oculto `action="finalizar"` en el formulario de Finalizar
4. Delegación condicional a métodos privados según `action`

**Beneficios:**
- ✅ Lógica completamente separada
- ✅ Validaciones independientes
- ✅ No depende de handlers nombrados con problemas de enrutamiento
- ✅ Fácil de depurar

### Documentación Técnica Generada
- ✅ `SOLUCION_COMPLETA_ASIGNACIONES.md`: Guía completa del problema y solución de handlers

---

## 📊 Progreso General

| Módulo | Estado | %Completado |
|--------|--------|-------------|
| **Migraciones y BD** | ✅ Completo | 100% |
| **Autenticación** | ✅ Completo | 100% |
| **Dashboard Admin** | ✅ Completo | 100% |
| **Dashboard Piloto** | ✅ Completo | 100% |
| **Dashboard Monitor** | ✅ Completo | 100% |
| **Gestión Asignaciones** | ✅ Completo | 100% |
| **Reportes** | ✅ Completo | 100% |
| **Servicios** | ✅ Completo | 100% |
| **Mapa Interactivo** | ⚠️ Parcial | 90% |
| **Planner de Rutas** | ⚠️ Por revisar | 70% |

**Progreso Total del Proyecto: ~95%**

---

## 🚀 Próximos Pasos

### Prioridad Alta
1. **Completar Mapa Interactivo de Paradas (10% restante)**
   - Implementar UI para agregar paradas desde el mapa
   - Guardar coordenadas lat/lng en BD
   - Modal de edición de detalles de parada

### Prioridad Media
2. **Revisar y actualizar `/Planner`**
   - Verificar funcionalidades actuales
   - Integrar con sistema de paradas actualizado

### Prioridad Baja
3. **Eliminar índice único de `IdBus` en BD**
   - Crear migración para eliminar `IX_AsignacionPilotoBus_IdBus`
   - Permitir historial completo de asignaciones

4. **Optimizaciones**
   - Remover logging de diagnóstico excesivo
   - Implementar caché para consultas frecuentes

---

## 📚 Documentación Relacionada

- `SOLUCION_COMPLETA_ASIGNACIONES.md`: Solución del problema de handlers en Gestionar Asignaciones
- `Scripts/README_DashboardMonitor.md`: Guía de datos de prueba para Dashboard Monitor
- `Docs/AlertaProximidad_README.md`: Sistema de alertas de proximidad
- `Docs/AlertasSignalR_README.md`: Integración de SignalR para notificaciones en tiempo real

---

## 👥 Roles y Permisos

| Rol | Pantalla Principal | Acceso |
|-----|-------------------|--------|
| **Administrador** | `/Admin/Index` | Todos los módulos |
| **Piloto** | `/Piloto/MiRuta` | Dashboard, mapa, alertas |
| **Monitor** | `/Monitor/MiRuta` | Dashboard, mapa, alertas |
| **Padre de Familia** | `/PadreDeFamilia/Index` | Información de hijos, pagos |

---

**Última actualización:** 2025-01-XX  
**Estado del proyecto:** ✅ **95% Completado**  
**Repositorio:** https://github.com/johnsvill/TransportesGenesis (branch: dev_david)
