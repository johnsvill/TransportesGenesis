# TransportesGenesis 🚌

Sistema de gestión de transporte escolar con seguimiento en tiempo real, dashboards por rol y gestión de asignaciones.

---

## 🚀 Estado del Proyecto

**Progreso General: 98% Completado**

| Módulo | Estado |
|--------|--------|
| Autenticación y Roles | ✅ 100% |
| Dashboard Administrador | ✅ 100% |
| Dashboard Piloto | ✅ 100% |
| Dashboard Monitor | ✅ 100% |
| Gestión de Asignaciones | ✅ 100% |
| Reportes | ✅ 100% |
| **Gestión de Paradas Interactiva** | ✅ **100%** |
| Mapa en Tiempo Real | ✅ 100% |
| Planner de Rutas | ⚠️ 70% |

---

## 📋 Funcionalidades Principales

### ✅ Módulos Completados

#### 1. **Autenticación con Primer Ingreso**
- Login obligatorio al iniciar la aplicación
- Cambio de contraseña obligatorio en el primer ingreso
- Roles: Administrador, Piloto, Monitor, Padre de Familia

#### 2. **Dashboard Administrador** (`/Admin/Index`)
- Gestión de asignaciones piloto/monitor-bus
- Reportes de rutas y asignaciones
- Acceso al planner de rutas
- Creación y finalización de asignaciones

#### 3. **Dashboard Piloto** (`/Piloto/MiRuta`)
- Visualización de ruta asignada
- Listado de paradas y alumnos
- Alertas de proximidad y retraso
- Mapa con ubicación en tiempo real

#### 4. **Dashboard Monitor** (`/Monitor/MiRuta`)
- Funcionalidades similares al Dashboard Piloto
- Vista de rutas, paradas y alumnos asignados

#### 5. **Gestión de Asignaciones** (`/Admin/GestionarAsignaciones`)
- Asignar piloto/monitor a bus
- Finalizar asignaciones activas
- Validaciones automáticas de disponibilidad
- Historial de asignaciones

#### 6. **Gestión de Paradas Interactiva** ✨ **NUEVO** (`/Admin/GestionarParadas`)
- Mapa interactivo con Leaflet para gestionar paradas
- Crear paradas haciendo clic en el mapa
- Editar paradas con drag-and-drop
- Eliminar paradas (soft delete)
- API REST completa (`/api/paradas`)
- Visualización en tabla con estado activo/inactivo

#### 7. **Reportes**
- Reporte de rutas con buses asignados
- Reporte de asignaciones de personal
- Datos en tiempo real desde SQL Server

### ⚠️ Pendientes

- **Planner de Rutas:** Revisión y actualización (70% completado)

---

## 🛠️ Tecnologías

- **.NET 8** (ASP.NET Core Razor Pages)
- **Entity Framework Core**
- **SQL Server**
- **ASP.NET Core Identity**
- **SignalR** (notificaciones en tiempo real)
- **Bootstrap 5** (UI)
- **Leaflet.js** / **Google Maps** (mapas)

---

## 📂 Estructura del Proyecto

```
TransportesGenesis/
├── Pages/
│   ├── Admin/              # Dashboard y gestión del Administrador
│   ├── Piloto/             # Dashboard del Piloto
│   ├── Monitor/            # Dashboard del Monitor
│   ├── Planner/            # Planner de rutas
│   └── Shared/             # Layout y componentes compartidos
├── Models/
│   └── DB/
│       ├── Negocio/        # Entidades de negocio (Rutas, Buses, etc.)
│       └── Usuarios/       # Entidades de usuarios (AppUser)
├── Data/
│   ├── Context/            # ApplicationDbContext
│   ├── Configuraciones/    # Configuraciones de EF Core
│   └── Migrations/         # Migraciones unificadas
├── Services/               # Servicios de negocio (PilotoService, etc.)
├── Docs/                   # Documentación técnica
└── Scripts/                # Scripts SQL y seed data

```

---

## 🚀 Inicio Rápido

### Requisitos Previos

- **.NET 8 SDK**
- **SQL Server** (LocalDB o instancia completa)
- **Visual Studio 2022** o **VS Code**

### Configuración

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/johnsvill/TransportesGenesis
   cd TransportesGenesis
   ```

2. **Configurar la cadena de conexión:**
   Edita `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TransportesGenesis;Trusted_Connection=True;"
   }
   ```

3. **Aplicar migraciones:**
   ```bash
   dotnet ef database update
   ```

4. **Ejecutar la aplicación:**
   ```bash
   dotnet run
   ```

5. **Acceder:**
   - URL: `https://localhost:7241` o `http://localhost:7240`
   - Usuario por defecto: `admin@transportesgenesis.com`
   - Contraseña: `Admin123!`

---

## 👥 Usuarios de Prueba

El sistema incluye usuarios de prueba con contraseña `Admin123!`:

| Email | Rol | Contraseña |
|-------|-----|------------|
| `admin@transportesgenesis.com` | Administrador | `Admin123!` |
| `piloto2@transportesgenesis.com` | Piloto | `Admin123!` |
| `piloto3@transportesgenesis.com` | Piloto | `Admin123!` |
| `monitor2@transportesgenesis.com` | Monitor | `Admin123!` |
| `monitor3@transportesgenesis.com` | Monitor | `Admin123!` |

---

## 📚 Documentación

- **Plan del Proyecto:** [PLAN_PROYECTO.md](PLAN_PROYECTO.md)
- **Solución de Asignaciones:** [SOLUCION_COMPLETA_ASIGNACIONES.md](SOLUCION_COMPLETA_ASIGNACIONES.md)
- **Dashboard Monitor:** [Scripts/README_DashboardMonitor.md](Scripts/README_DashboardMonitor.md)
- **Alertas de Proximidad:** [Docs/AlertaProximidad_README.md](Docs/AlertaProximidad_README.md)

---

## 🐛 Problemas Conocidos

1. **Índice único en `IdBus`:** La tabla `genesis.AsignacionPilotoBus` tiene un índice único que impide múltiples asignaciones históricas. Solución temporal implementada (eliminación automática de asignaciones inactivas).

2. **Mapa Interactivo:** Falta interfaz de usuario para agregar/editar paradas desde el mapa.

---

## 🤝 Contribuciones

Este proyecto está en desarrollo activo. Para contribuir:

1. Haz fork del repositorio
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit tus cambios (`git commit -am 'Agregar nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Crea un Pull Request

---

## 📞 Contacto

- **Repositorio:** https://github.com/johnsvill/TransportesGenesis
- **Branch actual:** `dev_david`

---

**Última actualización:** 2025-01-XX  
**Versión:** 1.0.0-beta
