# 🚌 PROYECTO TRANSPORTES GENESIS - Estado Actual del Desarrollo

## 📋 Información General

**Proyecto:** Sistema de Geolocalización para Transporte Escolar  
**Desarrolladores:** Jonathan Villeda (Pagos/Auth) + David (Geolocalización)  
**Framework:** .NET 8 - ASP.NET Core Razor Pages  
**Base de Datos:** SQL Server (Local)  
**Estado General:** 55% Completado  
**Fecha Actualización:** Abril 2025  

---

## 🛠️ Tecnologías Utilizadas

### **Backend**
- **.NET 8** - Framework principal
- **ASP.NET Core Razor Pages** - Arquitectura web
- **Entity Framework Core 8.0.10** - ORM
- **SQL Server** (Local) - Base de datos
- **ASP.NET Core Identity** - Sistema de autenticación
- **AutoMapper 13.0.1** - Mapeo objeto-objeto
- **Dapper 2.1.35** - Consultas SQL optimizadas
- **SignalR** - Comunicación en tiempo real

### **Frontend & Mapas**
- **Leaflet.js 1.9.4** - Biblioteca de mapas JavaScript
- **OpenStreetMap** - Proveedor de mapas (GRATUITO, sin límites)
- **Bootstrap 5** - Framework CSS
- **Bootstrap Icons** - Iconografía
- **JavaScript ES6+** - Interactividad del frontend

### **Arquitectura**
- **Repository Pattern** - Capa de acceso a datos
- **Service Layer** - Lógica de negocio
- **DTO Pattern** - Transferencia de datos
- **Dependency Injection** - Inyección de dependencias nativa .NET
- **RESTful APIs** - Endpoints para comunicación frontend-backend

---

## 📊 Estado de Sprints

### ✅ **Sprint 1 - Autenticación y Login (100% - Jonathan)**
- Sistema de login/registro completo
- ASP.NET Core Identity configurado
- Páginas de autenticación funcionales
- Migraciones de base de datos aplicadas

### 🟡 **Sprint 2 - Pagos & Geolocalización Base (80%)**
**Jonathan - Módulo de Pagos (100%):**
- ✅ Entidades: Pago, TipoRecorridoPago, Banco, TipoCuenta
- ✅ Migraciones de BD creadas y aplicadas
- ✅ Repositorios y Services completos
- ✅ Páginas CRUD funcionales

**David - Geolocalización (80%):**
- ✅ 11 entidades nuevas creadas
- ✅ Backend completo (DTOs, Repositories, Services)
- ✅ Integración Leaflet.js + OpenStreetMap
- ✅ Mapa en tiempo real funcional
- 🟡 **Pendiente:** Integrar con sistema de Login

### 🟡 **Sprint 3 - SignalR & Rutas (70% - David)**
**Completado:**
- ✅ NotificacionesHub (SignalR) implementado
- ✅ Backend de Rutas completo (Entidades, Repos, Services)
- ✅ RutasController API REST
- ✅ Página "Mi Ruta" para Pilotos

**Pendiente:**
- 🟡 Integrar SignalR con autenticación
- 🟡 Algoritmo TSP para optimización de rutas
- 🟡 Conectar Admin: Calcular Rutas

### 🟡 **Sprint 4 - Calendario de Asistencia (65% - David)**
**Completado:**
- ✅ Backend de Asistencia (Repository, Service, DTOs)
- ✅ AsistenciaController API
- ✅ Página "Confirmar Asistencia" con calendario interactivo
- ✅ Validación de horarios (Mañana: 2PM-4AM, Tarde: 5PM-11AM)

**Pendiente:**
- 🟡 Integrar calendario con sistema de Login
- 🟡 Jobs automáticos (5 AM / 11 AM)
- 🟡 Vista para pilotos (asistencias confirmadas del día)

### 🟡 **Sprint 5 - Sistema de Traslados (60% - David)**
**Completado:**
- ✅ Backend de Traslados (SolicitudTrasladoRepository, Service)
- ✅ TrasladosController API
- ✅ Página "Mis Traslados" (Padres)
- ✅ Página "Gestionar Traslados" (Admin) con filtros

**Pendiente:**
- 🟡 Integrar con sistema de Login
- 🟡 Notificaciones de traslado aprobado/rechazado
- 🟡 Validación de capacidad de buses

### ⭕ **Sprint 6 - Sistema de Alertas (PENDIENTE)**
- Alertas de proximidad para padres
- Notificaciones de retrasos
- Panel de historial de alertas

### ⭕ **Sprint 7 - Testing & Deploy (PENDIENTE)**
- Testing de integración end-to-end
- Corrección de bugs
- Documentación final
- Presentación del proyecto

---

## 🗃️ Base de Datos

### **Configuración**
- **Servidor:** (local) / LAPTOP-TTNKEI4H
- **Base de Datos:** TransportesGenesis
- **Schema:** genesis
- **Total Tablas:** 17 (6 originales + 11 geolocalización)

### **Entidades Geolocalización (Nuevas)**
1. **Bus** - Vehículos del sistema
2. **Ruta** - Rutas de transporte
3. **Parada** - Puntos de recogida/entrega
4. **AsistenciaAlumno** - Confirmaciones diarias
5. **UbicacionBusEnTiempoReal** - GPS tracking
6. **Alerta** - Sistema de notificaciones
7. **SolicitudTraslado** - Cambios de bus temporales
8. **AsignacionPilotoBus** - Asignación conductores
9. **NotificacionProximidad** - Alertas de proximidad
10. **RegistroRecogida** - Log de recogidas
11. **NotificacionRetraso** - Alertas de retrasos

### **Entidades Modificadas**
- **Alumnos**: Agregados campos nullable (IdBusAsignado, Latitud, Longitud, Direccion)

### **Migración Principal**
- **Archivo:** `20260425225130_Add_Sistema_Geolocalizacion_Completo.cs`
- **Estado:** ✅ Aplicada exitosamente

---

## 🔧 Configuración de Desarrollo

### **Puertos de Desarrollo**
- **HTTP:** 5280
- **HTTPS:** 7240

### **Cadena de Conexión**
```json
"DefaultConnection": "Server=(local);Database=TransportesGenesis;Trusted_Connection=true;MultipleActiveResultSets=true"
```

### **Estructura del Proyecto**
```
TransportesGenesis/
├── Controllers/Api/          # APIs REST
├── Data/
│   ├── Context/             # ApplicationDbContext
│   ├── Configuraciones/     # EF Configurations
│   └── Migrations/          # Migraciones EF
├── DTOs/                    # Data Transfer Objects
├── Models/DB/Negocio/       # Entidades de dominio
├── Repositories/            # Patrón Repository
├── Services/                # Lógica de negocio
├── Pages/                   # Razor Pages
├── Hubs/                    # SignalR Hubs
├── Mappings/                # AutoMapper Profiles
└── Scripts/                 # Scripts SQL auxiliares
```

---

## 🌐 APIs Disponibles

### **UbicacionesController**
- `GET /api/ubicaciones/buses-activos` - Buses activos en mapa
- `GET /api/ubicaciones/{idBus}/ultima` - Última ubicación de bus
- `POST /api/ubicaciones` - Registrar coordenada GPS
- `GET /api/ubicaciones/{idBus}/historial` - Historial de ubicaciones

### **RutasController**
- `GET /api/rutas` - Listar rutas
- `GET /api/rutas/{id}` - Obtener ruta específica
- `POST /api/rutas` - Crear nueva ruta
- `PUT /api/rutas/{id}` - Actualizar ruta
- `DELETE /api/rutas/{id}` - Eliminar ruta

### **AsistenciaController**
- `GET /api/asistencia/alumno/{id}/mes/{mes}/{anio}` - Calendario mensual
- `POST /api/asistencia/confirmar` - Confirmar asistencia
- `GET /api/asistencia/dia/{fecha}` - Asistencias del día

### **TrasladosController**
- `GET /api/traslados/solicitudes` - Listar solicitudes
- `POST /api/traslados/solicitar` - Nueva solicitud
- `PUT /api/traslados/{id}/aprobar` - Aprobar traslado
- `PUT /api/traslados/{id}/rechazar` - Rechazar traslado

---

## 📄 Páginas Razor Implementadas

### **Geolocalización**
- `/Geolocalizacion/MapaEnTiempoReal` - Mapa con buses en tiempo real
- `/Geolocalizacion/BusesIndex` - Gestión de buses

### **Padres**
- `/Padres/ConfirmarAsistencia` - Calendario de confirmaciones
- `/Padres/Traslados` - Gestión de solicitudes de traslado

### **Pilotos**
- `/Piloto/MiRuta` - Ruta del día con mapa y paradas

### **Administradores**
- `/Admin/GestionarTraslados` - Aprobar/rechazar solicitudes
- `/Admin/CalcularRutas` - Optimización de rutas (en desarrollo)

### **Proyecto**
- `/Planner` - Dashboard de progreso del proyecto

---

## 🔧 Scripts SQL Ejecutados

### **Fixes Aplicados**
1. `Fix_Remove_Invalid_FK_Alumnos.sql` - Removió FK problemática
2. `SeedData_Geolocalizacion_Test.sql` - Datos de prueba (3 buses)
3. `SeedData_Ubicaciones_Test.sql` - Coordenadas GPS Guatemala City

### **Configuraciones FK**
- Todos los FK configurados con `DeleteBehavior.Restrict` para evitar cascadas problemáticas

---

## 🎯 Próximas Prioridades

### **Inmediato (Crítico)**
1. **Integrar Geolocalización con Login**
   - Agregar `[Authorize]` a páginas
   - Configurar roles (Padre, Piloto, Admin)
   - Filtrar datos por usuario autenticado

2. **Integrar Calendario con Login**
   - Mostrar solo datos del alumno/padre logueado
   - Autorización por roles

3. **Integrar Traslados con Login**
   - Filtrar solicitudes por usuario actual

### **Medio Plazo**
1. **SignalR con autenticación**
2. **Algoritmo TSP para rutas**
3. **Jobs automáticos (5 AM / 11 AM)**
4. **Sistema de alertas completo**

### **Largo Plazo**
1. **Testing de integración**
2. **Deploy a producción**
3. **Documentación de usuario**

---

## 🐛 Issues Conocidos

1. **Autenticación pendiente** - Principal bloqueador actual
2. **Jobs en background** - Falta implementar Hangfire o similar
3. **Optimización de consultas** - Algunas N+1 queries por optimizar
4. **Validaciones de negocio** - Faltan algunas validaciones complejas

---

## 📱 Funcionalidades Destacadas

### **Mapa en Tiempo Real**
- ✅ Visualización con Leaflet.js
- ✅ Colores por estado: Verde (Movimiento), Naranja (Parado), Rojo (Sin señal)
- ✅ Actualización cada 30 segundos
- ✅ Popups informativos en marcadores

### **Sistema de Asistencia**
- ✅ Calendario interactivo mensual
- ✅ Horarios específicos: Mañana (2PM-4AM), Tarde (5PM-11AM)
- ✅ Estados visuales por día
- ✅ Validaciones de fin de semana

### **Gestión de Traslados**
- ✅ Solicitudes con fecha específica
- ✅ Workflow: Pendiente → Aprobado/Rechazado
- ✅ Filtros por estado, fecha, alumno
- ✅ Interfaz admin con acciones rápidas

---

## 🚀 Comandos de Desarrollo

```bash
# Ejecutar aplicación
dotnet run

# Aplicar migraciones
dotnet ef database update

# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Compilar proyecto
dotnet build

# Ver puertos configurados
cat Properties/launchSettings.json
```

---

## 📞 Contacto del Equipo

**Jonathan Villeda** - Backend & Autenticación (dev_jonathan)  
**David** - Geolocalización & Frontend (dev_david)

---

*Última actualización: Abril 2025*  
*Versión del documento: 1.0*