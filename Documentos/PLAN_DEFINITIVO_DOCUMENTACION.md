# 📋 PLAN DEFINITIVO - DOCUMENTACIÓN COMPLETA TRANSPORTES GENESIS

## 🎯 OBJETIVO CLARO

Crear el archivo **`ESPECIFICACION_COMPLETA_TRANSPORTES_GENESIS.md`** siguiendo EXACTAMENTE la estructura de `Definición de proyecto.md`:

- ✅ **Items numerados (1.1, 2.1, etc.)**: TODOS obligatorios
- ⚡ **Sub-items con viñetas (•)**: SOLO los relevantes/importantes para el proyecto
- 📐 **Diagramas**: Solo descripciones/instrucciones (sin generarlos)
- 📚 **Referencias**: Basarse en el código actual + documentos existentes

---

## 📂 FUENTES DE INFORMACIÓN

### 1. Proyecto Actual (Código C#)
- ✅ Implementación en **.NET 8 + Razor Pages**
- ✅ **Módulos**: Auth, Usuarios, Buses/Rutas, Pagos, Geolocalización, Recogidas
- ✅ **Roles**: Admin, Piloto, Monitor, Padre
- ✅ **Stack**: ASP.NET Core Identity, EF Core, SQL Server, SignalR, Bootstrap 5

### 2. Documentos Existentes
- `Documentos/Definición de proyecto.md` → **GUÍA PRINCIPAL**
- `Documentos/Proyecto_aplicacion_de_transportes.docx` → Especificación funcional (parcial)
- `Documentos/Version_Completa_Transportes_Génesis.docx` → Versión anterior de referencia

### 3. Archivos de Verificación Recientes
- `VERIFICACION_IMPLEMENTACION_PILOTO_MONITOR.md`
- `CHECKLIST_VERIFICACION.md`
- `PRUEBAS_E_INSTRUCCIONES_PENDIENTES.md`

---

## 📘 ESTRUCTURA DEL DOCUMENTO FINAL

```
# ESPECIFICACIÓN COMPLETA - TRANSPORTES GENESIS
## Sistema de Gestión de Transporte Escolar

├─ PORTADA
│  ├─ Título del proyecto
│  ├─ Universidad/Institución
│  ├─ Autor(es)
│  ├─ Fecha
│  └─ Versión del documento
│
├─ 📑 ÍNDICE GENERAL
│
├─ 1️⃣ ESPECIFICACIÓN FUNCIONAL
│  ├─ 1.1 Introducción y alcance del sistema ✅ OBLIGATORIO
│  ├─ 1.2 Contexto organizacional y stakeholders ✅ OBLIGATORIO
│  │   └─ • Matriz RACI (solo si es relevante)
│  │   └─ • Mapa poder-interés (solo si es relevante)
│  ├─ 1.3 Análisis de Requerimientos Funcionales ✅ OBLIGATORIO
│  │   ├─ 1.3.1 Requerimientos funcionales detallados (formato IEEE) ⚡ SELECCIONAR LOS MÁS IMPORTANTES
│  │   │   └─ Ejemplo: RF-001 Login de usuarios, RF-002 Calcular rutas, etc.
│  │   ├─ 1.3.2 Catálogo de Use Cases ⚡ SELECCIONAR ~5-8 CRÍTICOS
│  │   │   └─ [DESCRIPCIÓN PARA DIAGRAMA UML] + especificación textual
│  │   ├─ 1.3.3 User Stories + Acceptance Criteria (Gherkin) ⚡ SELECCIONAR ~10-15 PRINCIPALES
│  │   │   └─ Priorización MoSCoW (Must/Should/Could/Won't)
│  │   └─ 1.3.4 Diagramas de secuencia y actividad ⚡ MÍNIMO 5-8 CRÍTICOS
│  │       └─ [DESCRIPCIONES PARA DIAGRAMAS]
│  ├─ 1.4 Análisis de Requerimientos No Funcionales ✅ OBLIGATORIO
│  │   └─ • Tabla de clasificación ISO/IEC 25010 ⚡ SOLO LAS RELEVANTES:
│  │       ├─ Performance (tiempos de respuesta, throughput)
│  │       ├─ Seguridad (autenticación, autorización, GDPR)
│  │       ├─ Escalabilidad y disponibilidad (99.9% uptime)
│  │       ├─ Usabilidad (responsive, accesible)
│  │       └─ Mantenibilidad (código limpio, documentado)
│  ├─ 1.5 Análisis de Dominio ✅ OBLIGATORIO
│  │   ├─ • Glosario de términos ⚡ MÍNIMO 20-30 TÉRMINOS
│  │   └─ • Reglas de negocio (Business Rules) ⚡ 10-15 PRINCIPALES
│  └─ 1.6 Supuestos, restricciones y dependencias ✅ OBLIGATORIO
│
├─ 2️⃣ ESPECIFICACIÓN TÉCNICA
│  ├─ 2.1 Arquitectura de referencia ✅ OBLIGATORIO
│  │   ├─ • Diagrama C4 (Context, Containers, Components) [DESCRIPCIÓN]
│  │   ├─ • Estilo arquitectónico (Razor Pages + Clean Architecture) + justificación
│  │   └─ • Diagrama de despliegue [DESCRIPCIÓN]
│  ├─ 2.2 Stack tecnológico seleccionado ✅ OBLIGATORIO
│  │   ├─ • Frontend: Razor Pages, Bootstrap 5, jQuery, SignalR Client
│  │   ├─ • Backend: .NET 8, ASP.NET Core, EF Core 8
│  │   ├─ • Base de Datos: SQL Server 2019+
│  │   ├─ • Cloud/Infra: IIS/Azure App Service, Docker (opcional)
│  │   └─ • Tabla de comparación/justificación de alternativas ⚡ OPCIONAL
│  ├─ 2.3 Requerimientos de infraestructura y entornos ✅ OBLIGATORIO
│  │   ├─ • Hardware mínimo y recomendado (on-premise o cloud)
│  │   ├─ • Diagrama de red [DESCRIPCIÓN] ⚡ OPCIONAL
│  │   └─ • Docker + CI/CD pipeline básico ⚡ OPCIONAL
│  ├─ 2.4 Diseño de base de datos ✅ OBLIGATORIO
│  │   ├─ • Esquema físico (tablas principales con campos clave)
│  │   ├─ • [DESCRIPCIÓN PARA DIAGRAMA ER]
│  │   └─ • Índices + estrategias ⚡ SOLO SI APLICA
│  ├─ 2.5 Diseño de APIs e integración ✅ OBLIGATORIO
│  │   ├─ • Endpoints REST principales (ej: /api/rutas, /api/pagos)
│  │   └─ • Hubs SignalR (ej: GeolocalizacionHub)
│  ├─ 2.6 Patrones de diseño y buenas prácticas ✅ OBLIGATORIO
│  │   ├─ • Patrones aplicados (Repository, Dependency Injection, PageModel)
│  │   └─ • Estrategia de testing ⚡ SOLO MENCIONAR (unit, integration)
│  ├─ 2.7 Plan de implementación y roadmap técnico ✅ OBLIGATORIO
│  │   └─ • Fases del proyecto (MVP → v1.0 → futuras)
│  └─ 2.8 Análisis de riesgos técnicos ✅ OBLIGATORIO
│      └─ • Tabla de riesgos (probabilidad × impacto) + mitigación
│
├─ 3️⃣ ESPECIFICACIÓN ECONÓMICA
│  ├─ 3.1 Resumen ejecutivo económico ✅ OBLIGATORIO
│  ├─ 3.2 Análisis de costos (Cost Breakdown Structure) ✅ OBLIGATORIO
│  │   ├─ • Costos de desarrollo (horas × tarifa por rol)
│  │   ├─ • Costos de infraestructura (cloud, licencias)
│  │   └─ • Costos de mantenimiento (años 1-5) ⚡ OPCIONAL
│  ├─ 3.3 Estimación de esfuerzo y duración ✅ OBLIGATORIO
│  │   └─ • Método: Story Points + Velocity o COCOMO II (simplificado)
│  ├─ 3.4 Modelo de negocio ✅ OBLIGATORIO
│  │   ├─ • Business Model Canvas [DESCRIPCIÓN] ⚡ OPCIONAL
│  │   └─ • Modelo de ingresos (suscripción, licencia, etc.)
│  ├─ 3.5 Análisis financiero ✅ OBLIGATORIO
│  │   ├─ • Flujo de caja proyectado (simplificado 3 años)
│  │   └─ • Indicadores: VAN, TIR, ROI, Payback ⚡ CALCULAR SI ES POSIBLE
│  ├─ 3.6 Análisis de mercado y competencia ✅ OBLIGATORIO
│  │   └─ • TAM/SAM/SOM + matriz de competidores ⚡ SIMPLIFICADO
│  ├─ 3.7 Plan de negocio ejecutivo ✅ OBLIGATORIO
│  ├─ 3.8 Fuentes de financiamiento ⚡ OPCIONAL (mencionar brevemente)
│  └─ 3.9 Análisis de riesgos económicos ✅ OBLIGATORIO
│
└─ 📎 ANEXOS
   ├─ Glosario completo de términos
   ├─ Referencias bibliográficas
   ├─ Historial de versiones del documento
   └─ Enlaces a documentación técnica adicional
```

---

## 🔍 DESGLOSE POR SECCIÓN

### 1️⃣ ESPECIFICACIÓN FUNCIONAL

#### 1.1 Introducción y alcance ✅
**Qué incluir:**
- Descripción general del sistema
- Problema que resuelve
- Objetivos del proyecto
- Alcance (qué SI y qué NO incluye)
- Usuarios finales

**Basarse en:**
- Código actual: Sistema de gestión de transporte escolar
- Documentos: `Proyecto_aplicacion_de_transportes.docx` (sección introducción)

---

#### 1.2 Contexto organizacional y stakeholders ✅
**Qué incluir:**
- Organización: Transportes Genesis (empresa ficticia de transporte escolar)
- Stakeholders principales:
  - Administradores (dueños/gerentes)
  - Pilotos (conductores)
  - Monitores (asistentes en el bus)
  - Padres de Familia (clientes)
  - Alumnos (usuarios indirectos)
- Matriz RACI ⚡ SOLO SI ES ÚTIL (opcional):
  - Ejemplo: Administrador = Responsible/Accountable para crear rutas
- Mapa poder-interés ⚡ OPCIONAL

**Basarse en:**
- Código actual: Roles en `Startup.cs` → `SeedRolesAndAdmin`
- 4 roles principales implementados

---

#### 1.3 Análisis de Requerimientos Funcionales ✅

##### 1.3.1 Requerimientos funcionales (IEEE) ⚡ SELECCIONAR ~15-20
**Formato:**
```
RF-001: Login de usuarios
Descripción: El sistema debe permitir que los usuarios se autentiquen con email y contraseña
Precondición: Usuario registrado en el sistema
Postcondición: Usuario autenticado y redirigido según su rol
Flujo principal:
  1. Usuario ingresa email y contraseña
  2. Sistema valida credenciales
  3. Sistema redirige según rol
Flujos alternos:
  - Credenciales incorrectas → mostrar error
  - Primer login → forzar cambio de contraseña
```

**Ejemplos de RF importantes:**
- RF-001: Login
- RF-002: Calcular rutas optimizadas
- RF-003: Asignar bus a piloto
- RF-004: Registrar recogidas de alumnos
- RF-005: Confirmar asistencia (padres)
- RF-006: Realizar pagos
- RF-007: Visualizar geolocalización en tiempo real
- RF-008: Gestionar usuarios
- ... (hasta 15-20 principales)

**Basarse en:**
- Código actual: Funcionalidades implementadas en cada PageModel y Controller

---

##### 1.3.2 Catálogo de Use Cases ⚡ SELECCIONAR ~5-8
**Qué incluir:**
- [DESCRIPCIÓN PARA DIAGRAMA UML Use Case]
  - Actores: Admin, Piloto, Monitor, Padre
  - Casos de uso principales por actor
- Especificación textual de cada uno:
  - UC-001: Calcular Ruta Óptima
  - UC-002: Registrar Recogida de Alumnos
  - UC-003: Confirmar Asistencia
  - UC-004: Ver Ubicación del Bus en Tiempo Real
  - UC-005: Gestionar Pagos
  - ... (5-8 críticos)

**Basarse en:**
- Código: Páginas principales en `Pages/` y `Controllers/`

---

##### 1.3.3 User Stories + Gherkin ⚡ SELECCIONAR ~10-15
**Formato:**
```
US-001: Como Piloto, quiero ver mi ruta asignada para saber qué paradas debo hacer

Acceptance Criteria (Gherkin):
  Scenario: Piloto visualiza su ruta del día
    Given que soy un piloto autenticado
    And tengo un bus asignado
    When accedo a "Mi Ruta"
    Then veo la lista de paradas ordenadas
    And veo la hora estimada de cada parada
    And veo los alumnos asignados a cada parada
```

**Priorización MoSCoW:**
- **Must Have**: Login, Calcular Rutas, Ver Ruta, Geolocalización
- **Should Have**: Pagos, Confirmación asistencia, Registro recogidas
- **Could Have**: Reportes avanzados, Notificaciones push
- **Won't Have** (para v1.0): Integración con apps de terceros, Machine Learning

**Basarse en:**
- Funcionalidades implementadas en el proyecto

---

##### 1.3.4 Diagramas de secuencia y actividad ⚡ MÍNIMO 5-8
**[DESCRIPCIONES PARA DIAGRAMAS]**

1. **Diagrama de Secuencia: Login y Redirección por Rol**
   - Usuario → AuthController
   - AuthController → UserManager (validar)
   - AuthController → RoleManager (obtener roles)
   - AuthController → RedirectToPage según rol

2. **Diagrama de Secuencia: Calcular Ruta Óptima**
   - Admin → API Rutas
   - API → DB (obtener alumnos, paradas, bus)
   - API → Algoritmo TSP (calcular orden óptimo)
   - API → DB (guardar ruta calculada)
   - API → Respuesta

3. **Diagrama de Secuencia: Geolocalización Tiempo Real (SignalR)**
   - Bus (cliente) → GeolocalizacionHub.EnviarUbicacion(lat, lon)
   - Hub → Broadcast a grupo "Mapa"
   - Clientes conectados → RecibirUbicacion(lat, lon)
   - Mapa → Actualizar marcador

4. **Diagrama de Actividad: Registrar Recogida de Alumnos**
   - Inicio → Monitor selecciona parada
   - Monitor marca alumnos recogidos
   - Sistema valida duplicados
   - Sistema guarda en RegistroRecogida
   - Sistema muestra confirmación
   - Fin

5. **Diagrama de Actividad: Confirmación de Asistencia (Padre)**
   - Inicio → Padre selecciona fecha
   - Sistema muestra opciones (Asiste, No Asiste)
   - Padre confirma
   - Sistema guarda en ConfirmacionAsistencia
   - Sistema envía notificación (opcional)
   - Fin

... (hasta 5-8 diagramas críticos)

**Basarse en:**
- Código actual: Flujos en `PageModel.OnGetAsync()` y `OnPostAsync()`

---

#### 1.4 Requerimientos No Funcionales ✅

**Tabla ISO/IEC 25010 (SOLO LAS RELEVANTES):**

| Característica | Requerimiento | Métrica |
|----------------|---------------|---------|
| **Performance** | Tiempo de respuesta < 2s para carga de páginas | 95% de requests < 2s |
| | Throughput: 100 usuarios concurrentes | Load testing con JMeter |
| **Seguridad** | Autenticación con ASP.NET Core Identity | Hashing de contraseñas (SHA256) |
| | Autorización por roles | `[Authorize(Roles = "X")]` |
| | HTTPS obligatorio | SSL/TLS certificado válido |
| **Escalabilidad** | Soportar hasta 500 usuarios | Horizontal scaling (Azure App Service) |
| **Disponibilidad** | 99.9% uptime (8.76 horas downtime/año) | Monitoreo con Application Insights |
| **Usabilidad** | Interfaz responsive (móvil, tablet, desktop) | Bootstrap 5 responsive grid |
| | Accesible WCAG 2.1 nivel AA (mínimo) | Validar con Lighthouse |
| **Mantenibilidad** | Código limpio (Clean Code) | SonarQube score > 80% |
| | Documentación del código | XML comments en métodos públicos |

**Basarse en:**
- Código actual: Bootstrap 5, Identity, HTTPS, etc.

---

#### 1.5 Análisis de Dominio ✅

##### Glosario de Términos ⚡ MÍNIMO 20-30

| Término | Definición |
|---------|------------|
| **Bus** | Vehículo de transporte escolar asignado a un piloto |
| **Ruta** | Secuencia ordenada de paradas que un bus debe seguir en un turno específico |
| **Parada** | Ubicación geográfica donde se recoge o deja a un alumno |
| **Turno** | Periodo del día (Mañana o Tarde) en que opera una ruta |
| **Piloto** | Usuario conductor del bus responsable de seguir la ruta asignada |
| **Monitor** | Usuario asistente en el bus que registra las recogidas de alumnos |
| **Padre de Familia** | Usuario cliente que confirma asistencia y realiza pagos |
| **Asignación** | Relación entre un piloto/monitor y un bus en un periodo determinado |
| **Registro de Recogida** | Acción de marcar que un alumno fue recogido en una parada específica |
| **Confirmación de Asistencia** | Indicación del padre sobre si el alumno asistirá o no en una fecha |
| ... (hasta 20-30 términos) |

##### Reglas de Negocio ⚡ 10-15 PRINCIPALES

```
RN-001: Un piloto solo puede tener un bus asignado activo a la vez (EsActual = true)
RN-002: Una ruta debe tener mínimo 2 paradas (origen y destino)
RN-003: Las paradas en una ruta deben estar ordenadas secuencialmente (campo Orden)
RN-004: No se pueden calcular rutas para fines de semana (sábado y domingo)
RN-005: Un alumno no puede ser registrado como recogido dos veces en la misma parada
RN-006: Los pagos deben ser confirmados antes de permitir el acceso al servicio
RN-007: La geolocalización debe enviarse cada 10 segundos máximo
RN-008: Solo el monitor asignado al bus puede registrar recogidas
RN-009: Un padre solo puede confirmar asistencia para sus propios hijos
RN-010: Las rutas se calculan usando el algoritmo TSP (Traveling Salesman Problem)
... (hasta 10-15 reglas)
```

**Basarse en:**
- Código: Validaciones en modelos, lógica de negocio en PageModels

---

#### 1.6 Supuestos, restricciones y dependencias ✅

**Supuestos:**
- Los usuarios tienen acceso a internet estable
- Los buses cuentan con GPS funcional
- Los padres tienen dispositivos móviles o computadoras para acceder al sistema

**Restricciones:**
- El sistema debe funcionar en navegadores modernos (Chrome, Firefox, Edge, Safari)
- La aplicación debe ser desarrollada en .NET 8 (requisito tecnológico)
- Presupuesto limitado para infraestructura cloud

**Dependencias:**
- Google Maps API para visualización de mapas
- SQL Server 2019+ para base de datos
- Certificado SSL válido para HTTPS
- Conexión a internet para SignalR (WebSockets)

---

### 2️⃣ ESPECIFICACIÓN TÉCNICA

#### 2.1 Arquitectura de referencia ✅

##### Diagrama C4 [DESCRIPCIÓN]

**Nivel 1 - Context:**
```
Sistema: Transportes Genesis
Actores externos:
- Administrador
- Piloto
- Monitor
- Padre de Familia
Sistemas externos:
- Google Maps API
- Proveedor de Pagos (opcional)
```

**Nivel 2 - Containers:**
```
- Web Application (Razor Pages)
- API REST (ASP.NET Core)
- SignalR Hub (Tiempo real)
- Base de Datos SQL Server
- Navegador Web (Cliente)
```

**Nivel 3 - Components (ejemplo: Web Application):**
```
- Pages/Piloto (Razor Pages)
- Pages/Monitor (Razor Pages)
- Pages/Admin (Razor Pages)
- Controllers/AuthController
- Controllers/ApiController
- Hubs/GeolocalizacionHub
- Data/ApplicationDbContext
```

##### Estilo Arquitectónico
- **Patrón principal**: Razor Pages (Page-focused MVC pattern)
- **Complementos**: MVC Controllers para API, SignalR Hub para tiempo real
- **Principios**: Separation of Concerns, Dependency Injection, Repository Pattern (implícito en EF Core)
- **Justificación**: Simplicidad, productividad, soporte oficial de Microsoft

##### Diagrama de Despliegue [DESCRIPCIÓN]
```
- Cliente (Navegador) → HTTPS → IIS/Azure App Service
- IIS/Azure → SQL Server (puerto 1433)
- IIS/Azure → SignalR WebSocket (puerto 443)
```

**Basarse en:**
- Código actual: Estructura de carpetas `Pages/`, `Controllers/`, `Hubs/`, `Data/`

---

#### 2.2 Stack tecnológico ✅

| Capa | Tecnología | Versión | Licencia | Justificación |
|------|------------|---------|----------|---------------|
| **Backend** | .NET | 8.0 | MIT | Última versión LTS, alto rendimiento |
| | ASP.NET Core | 8.0 | MIT | Framework web moderno y cross-platform |
| | Entity Framework Core | 8.0 | MIT | ORM oficial, migraciones automáticas |
| | ASP.NET Core Identity | 8.0 | MIT | Autenticación/autorización integrada |
| | SignalR | 8.0 | MIT | Comunicación en tiempo real (WebSockets) |
| **Frontend** | Razor Pages | 8.0 | MIT | Productividad, integración con backend |
| | Bootstrap | 5.3 | MIT | UI responsive, componentes pre-diseñados |
| | jQuery | 3.7 | MIT | Manipulación DOM, Ajax |
| | SignalR Client | 8.0 | MIT | Cliente JavaScript para WebSockets |
| **Base de Datos** | SQL Server | 2019+ | Comercial | Robustez, soporte empresarial |
| **Infra** | IIS / Azure App Service | - | - | Hosting estable, escalable |
| **Otros** | Google Maps API | v3 | Comercial | Geolocalización y mapas |

**Alternativas consideradas:**
- PostgreSQL vs SQL Server → SQL Server elegido por familiaridad del equipo
- Blazor vs Razor Pages → Razor Pages por simplicidad y madurez
- Angular/React vs jQuery → jQuery por menor curva de aprendizaje

**Basarse en:**
- Código: `.csproj`, `Startup.cs`, referencias NuGet

---

#### 2.3 Requerimientos de infraestructura ✅

**Hardware mínimo (on-premise):**
- CPU: 4 cores, 2.5 GHz
- RAM: 8 GB
- Disco: 50 GB SSD
- Red: 100 Mbps

**Hardware recomendado (cloud - Azure App Service):**
- Plan: B2 (2 cores, 3.5 GB RAM) para desarrollo
- Plan: S2 (2 cores, 3.5 GB RAM) para producción
- SQL Database: Basic (2 GB) para desarrollo, Standard S0 (250 GB) para producción

**Diagrama de red [DESCRIPCIÓN]** ⚡ OPCIONAL
```
Internet → Azure Load Balancer → App Service
                                     ↓
                               SQL Database
```

**Docker + CI/CD** ⚡ OPCIONAL (mencionar brevemente)
- Dockerfile básico para contenerización
- GitHub Actions para CI/CD

**Basarse en:**
- Requisitos típicos de ASP.NET Core

---

#### 2.4 Diseño de base de datos ✅

##### Esquema físico (tablas principales)

**Tablas de Negocio (schema: genesis):**

1. **Bus**
   - IdBus (PK, int)
   - Placa (nvarchar(20), unique)
   - Modelo (nvarchar(100))
   - Capacidad (int)
   - Activo (tinyint)
   - FechaRegistro (datetime2)

2. **Ruta**
   - IdRuta (PK, int)
   - IdBus (FK → Bus)
   - Nombre (nvarchar(200))
   - TipoRuta (nvarchar(20)) -- "Mañana" o "Tarde"
   - HoraInicio (time)
   - EsActiva (bit)
   - FechaCreacion (datetime2)

3. **Parada**
   - IdParada (PK, int)
   - Nombre (nvarchar(200))
   - Direccion (nvarchar(500))
   - Latitud (decimal(10,7))
   - Longitud (decimal(10,7))
   - Activo (tinyint)

4. **RutaParada** (tabla intermedia)
   - IdRutaParada (PK, int)
   - IdRuta (FK → Ruta)
   - IdParada (FK → Parada)
   - Orden (int)
   - HoraEstimada (time)

5. **Alumnos**
   - IdAlumno (PK, int)
   - NombreCompleto (nvarchar(200))
   - FechaNacimiento (date)
   - Grado (nvarchar(50))
   - IdPadre (FK → AspNetUsers)

6. **AsignacionPilotoBus**
   - IdAsignacion (PK, int)
   - IdUsuarioPiloto (FK → AspNetUsers)
   - IdBus (FK → Bus)
   - FechaAsignacion (datetime2)
   - FechaFinAsignacion (datetime2, nullable)
   - EsActual (bit)

7. **RegistroRecogida**
   - IdRegistro (PK, int)
   - IdParada (FK → Parada)
   - IdAlumno (FK → Alumnos)
   - FechaHoraRecogida (datetime2)
   - ConfirmadoPor (FK → AspNetUsers) -- Monitor
   - Latitud (decimal(10,7), nullable)
   - Longitud (decimal(10,7), nullable)
   - AlumnoPresente (bit)

8. **ConfirmacionAsistencia**
   - IdConfirmacion (PK, int)
   - IdAlumno (FK → Alumnos)
   - Fecha (date)
   - VaAsistir (bit)
   - FechaHoraConfirmacion (datetime2)

9. **Pago**
   - IdPago (PK, int)
   - IdPadre (FK → AspNetUsers)
   - Monto (decimal(18,2))
   - FechaPago (datetime2)
   - MetodoPago (nvarchar(50))
   - Estado (nvarchar(20)) -- "Pendiente", "Confirmado"

**Tablas de Identity (schema: dbo):**
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- ... (tablas estándar de Identity)

##### [DESCRIPCIÓN PARA DIAGRAMA ER]
```
Relaciones principales:
- AspNetUsers (1) → (N) AsignacionPilotoBus
- Bus (1) → (N) AsignacionPilotoBus
- Bus (1) → (N) Ruta
- Ruta (1) → (N) RutaParada → (N) Parada
- Parada (1) → (N) RegistroRecogida
- Alumnos (1) → (N) RegistroRecogida
- Alumnos (1) → (N) ConfirmacionAsistencia
- AspNetUsers (1) → (N) Pago
- AspNetUsers (1) → (N) Alumnos (como padre)
```

**Índices importantes:**
- `IX_AsignacionPilotoBus_IdUsuarioPiloto_EsActual` (filtrado)
- `IX_RutaParada_IdRuta_Orden` (ordenamiento)
- `IX_RegistroRecogida_IdParada_IdAlumno` (prevenir duplicados)

**Basarse en:**
- Código: `Models/DB/`, migrations en `Data/Migrations/`

---

#### 2.5 Diseño de APIs e integración ✅

**Endpoints REST principales:**

```
GET  /api/rutas/bus/{idBus}/activa?tipoRuta={Mañana|Tarde}
     Descripción: Obtiene la ruta activa del día para un bus
     Respuesta: RutaDto { IdRuta, Paradas[], HoraInicio, ... }

POST /api/rutas/calcular
     Descripción: Calcula la ruta óptima usando TSP
     Body: { IdBus, Fecha, TipoRuta, AlumnosIds[] }
     Respuesta: { Success, RutaId }

GET  /api/pagos/padre/{idPadre}
     Descripción: Obtiene historial de pagos de un padre
     Respuesta: List<PagoDto>

POST /api/confirmacion-asistencia
     Body: { IdAlumno, Fecha, VaAsistir }
     Respuesta: { Success }
```

**Hubs SignalR:**

```
Hub: GeolocalizacionHub

Métodos del servidor (llamados por cliente):
- EnviarUbicacion(double latitud, double longitud, int idBus)

Métodos del cliente (invocados por servidor):
- RecibirUbicacion(double latitud, double longitud, int idBus)

Grupos:
- "Mapa" (todos los usuarios que visualizan el mapa)
```

**Basarse en:**
- Código: `Controllers/`, `Hubs/`, `DTOs/`

---

#### 2.6 Patrones de diseño y buenas prácticas ✅

**Patrones aplicados:**

1. **Page Model Pattern** (Razor Pages)
   - Separación de lógica de presentación y lógica de negocio
   - Ejemplo: `MiRutaModel.OnGetAsync()`

2. **Dependency Injection** (DI)
   - Registro en `Startup.cs`: `services.AddScoped<ApplicationDbContext>()`
   - Inyección en constructores: `public MiRutaModel(ApplicationDbContext context)`

3. **Repository Pattern** (implícito en EF Core)
   - `ApplicationDbContext` actúa como Unit of Work
   - `DbSet<T>` actúa como Repository

4. **DTO (Data Transfer Objects)**
   - `RutaDto`, `ParadaRutaDto`, `AlumnoEnParadaDto`
   - Evita exponer entidades de dominio directamente

5. **Authorization Pattern**
   - `[Authorize(Roles = "Piloto")]`
   - `[AllowAnonymous]` para login

**Estrategia de testing** ⚡ MENCIONAR BREVEMENTE
- **Unit Testing**: xUnit + Moq para lógica de negocio
- **Integration Testing**: TestServer para endpoints API
- **Pendiente**: Implementar tests para cobertura > 70%

**Basarse en:**
- Código: Arquitectura general del proyecto

---

#### 2.7 Plan de implementación y roadmap ✅

**Fases del proyecto:**

| Fase | Duración | Entregables | Estado |
|------|----------|-------------|--------|
| **Fase 1: MVP** | 4 semanas | Login, Roles, Gestión de usuarios | ✅ COMPLETADO |
| **Fase 2: Rutas** | 3 semanas | Calcular rutas, Asignar buses a pilotos | ✅ COMPLETADO |
| **Fase 3: Geolocalización** | 2 semanas | SignalR Hub, Mapa en tiempo real | ✅ COMPLETADO |
| **Fase 4: Pagos y Asistencia** | 3 semanas | Módulo de pagos, Confirmación de asistencia | ✅ COMPLETADO |
| **Fase 5: Monitores** | 2 semanas | Registro de recogidas, Área del Monitor | ✅ COMPLETADO |
| **Fase 6: Pruebas y Deploy** | 2 semanas | Testing, Documentación, Despliegue | ⏳ EN CURSO |

**Roadmap futuro (post v1.0):**
- Notificaciones push (Firebase)
- Reportes avanzados (Power BI)
- App móvil nativa (Xamarin/MAUI)
- Machine Learning para predicción de tiempos

**DevOps:**
- CI/CD: GitHub Actions (pendiente)
- Monitoreo: Application Insights (pendiente)
- Logging: Serilog (pendiente)

**Basarse en:**
- Historial del proyecto, commits recientes

---

#### 2.8 Análisis de riesgos técnicos ✅

| ID | Riesgo | Probabilidad | Impacto | Mitigación |
|----|--------|--------------|---------|------------|
| RT-01 | Pérdida de conexión SignalR | Media | Alto | Implementar reconexión automática, fallback a polling |
| RT-02 | Sobrecarga de BD con muchos usuarios | Baja | Alto | Índices optimizados, caching con Redis |
| RT-03 | Bug en algoritmo TSP con muchas paradas | Media | Medio | Tests unitarios, límite de 30 paradas por ruta |
| RT-04 | Fallo de Google Maps API | Baja | Alto | Cachear mapas, tener backup (OpenStreetMap) |
| RT-05 | Vulnerabilidad de seguridad (XSS, CSRF) | Media | Alto | Validación de entrada, AntiForgeryToken, sanitización |
| RT-06 | Incompatibilidad con navegadores antiguos | Baja | Bajo | Detectar navegador, mostrar aviso |

**Basarse en:**
- Experiencia de desarrollo, puntos críticos del sistema

---

### 3️⃣ ESPECIFICACIÓN ECONÓMICA

#### 3.1 Resumen ejecutivo económico ✅

El proyecto **Transportes Genesis** requiere una inversión inicial de **$X,XXX USD** para desarrollo e infraestructura, con costos operativos mensuales de **$XXX USD**. El retorno de inversión (ROI) se estima en **X meses** con un modelo de suscripción mensual por usuario.

**Indicadores clave:**
- VAN (3 años): $X,XXX
- TIR: XX%
- Payback: X meses
- ROI: XX%

---

#### 3.2 Análisis de costos ✅

**Cost Breakdown Structure:**

| Categoría | Subcategoría | Cantidad | Costo Unitario | Total |
|-----------|--------------|----------|----------------|-------|
| **Desarrollo** | Análisis y Diseño | 40 horas | $50/hora | $2,000 |
| | Desarrollo Backend (.NET) | 200 horas | $50/hora | $10,000 |
| | Desarrollo Frontend (Razor/Bootstrap) | 100 horas | $40/hora | $4,000 |
| | Desarrollo Geolocalización (SignalR) | 60 horas | $50/hora | $3,000 |
| | Testing y QA | 80 horas | $40/hora | $3,200 |
| | Documentación | 40 horas | $30/hora | $1,200 |
| | **Subtotal Desarrollo** | | | **$23,400** |
| **Infraestructura** | Servidor Azure App Service (S2) | 12 meses | $146/mes | $1,752 |
| | SQL Database (Standard S0) | 12 meses | $30/mes | $360 |
| | Dominio (.com) | 1 año | $15/año | $15 |
| | Certificado SSL | 1 año | $0 (Let's Encrypt) | $0 |
| | Google Maps API (estimado) | 12 meses | $50/mes | $600 |
| | **Subtotal Infraestructura Año 1** | | | **$2,727** |
| **Licencias** | SQL Server (incluido en Azure) | - | $0 | $0 |
| | Visual Studio (Community) | - | $0 | $0 |
| | **Subtotal Licencias** | | | **$0** |
| **Mantenimiento** | Soporte técnico (20 horas/mes) | 12 meses | $40/hora × 20 | $9,600 |
| | Actualizaciones y mejoras | Lump sum | | $3,000 |
| | **Subtotal Mantenimiento Año 1** | | | **$12,600** |
| **Overhead** | Gastos indirectos (15%) | | | $5,859 |
| **TOTAL AÑO 1** | | | | **$44,586** |

**Años 2-5:**
- Desarrollo: Solo mantenimiento (~$8,000/año)
- Infraestructura: ~$2,700/año
- Overhead: ~$1,605/año
- **Total/año**: ~$12,305

**Basarse en:**
- Tarifas estándar del mercado, horas estimadas de desarrollo

---

#### 3.3 Estimación de esfuerzo y duración ✅

**Método: Story Points + Velocity**

- Total Story Points: 320 SP
- Velocity del equipo: 20 SP/semana
- Duración estimada: **16 semanas (4 meses)**
- Rango de confianza (P80): 14-18 semanas

**Desglose por módulo:**

| Módulo | Story Points | Semanas |
|--------|--------------|---------|
| Autenticación y Roles | 40 SP | 2 |
| Gestión de Usuarios | 30 SP | 1.5 |
| Buses y Rutas | 60 SP | 3 |
| Geolocalización | 50 SP | 2.5 |
| Pagos y Asistencia | 60 SP | 3 |
| Área de Monitores | 40 SP | 2 |
| Testing y Deploy | 40 SP | 2 |

**Basarse en:**
- Complejidad de funcionalidades implementadas

---

#### 3.4 Modelo de negocio ✅

**Business Model Canvas [DESCRIPCIÓN]** ⚡ SIMPLIFICADO

- **Segmentos de Clientes**: Empresas de transporte escolar (5-50 buses)
- **Propuesta de Valor**: Sistema completo de gestión de transporte escolar con geolocalización en tiempo real
- **Canales**: Venta directa, página web, partners
- **Relaciones con Clientes**: Soporte técnico, capacitación inicial, actualizaciones continuas
- **Fuentes de Ingreso**: Suscripción mensual ($X por bus/mes)
- **Recursos Clave**: Equipo de desarrollo, infraestructura cloud, código fuente
- **Actividades Clave**: Desarrollo, soporte, marketing
- **Socios Clave**: Azure, Google Maps, proveedores de pago
- **Estructura de Costos**: Desarrollo, infraestructura, salarios

**Modelo de ingresos:**
- **Suscripción mensual**: $50/bus/mes
- **Plan básico**: Hasta 10 buses → $500/mes
- **Plan empresarial**: 11-50 buses → $40/bus/mes

**Basarse en:**
- Análisis de mercado de soluciones similares

---

#### 3.5 Análisis financiero ✅

**Flujo de caja proyectado (simplificado 3 años):**

| Año | Ingresos | Costos | Flujo Neto | Flujo Acumulado |
|-----|----------|--------|------------|-----------------|
| 0 | $0 | -$23,400 | -$23,400 | -$23,400 |
| 1 | $36,000 | -$21,186 | $14,814 | -$8,586 |
| 2 | $72,000 | -$12,305 | $59,695 | $51,109 |
| 3 | $108,000 | -$12,305 | $95,695 | $146,804 |

**Supuestos:**
- Año 1: 5 clientes × 12 meses × $600/mes = $36,000
- Año 2: 10 clientes
- Año 3: 15 clientes
- Tasa de descuento: 10%

**Indicadores:**
- **VAN (3 años)**: $XX,XXX ⚡ CALCULAR
- **TIR**: XX% ⚡ CALCULAR
- **Payback**: ~18 meses
- **ROI**: ~230% (3 años)

**Basarse en:**
- Estimaciones conservadoras de adopción

---

#### 3.6 Análisis de mercado y competencia ✅

**TAM/SAM/SOM (simplificado):**
- **TAM** (Total Addressable Market): Empresas de transporte escolar en el país → $XX millones
- **SAM** (Serviceable Addressable Market): Empresas con 5-50 buses → $X millones
- **SOM** (Serviceable Obtainable Market): Alcance realista en 3 años → $XXX mil

**Matriz de competidores:**

| Competidor | Fortaleza | Debilidad | Precio |
|------------|-----------|-----------|--------|
| Competidor A | Posicionamiento | UI anticuada | $80/bus/mes |
| Competidor B | Funciones avanzadas | Caro | $120/bus/mes |
| **Transportes Genesis** | Precio, Geolocalización | Nuevo en el mercado | $50/bus/mes |

**Basarse en:**
- Investigación de mercado (ejemplo)

---

#### 3.7 Plan de negocio ejecutivo ✅

**Resumen (1 página):**

**Problema:** Las empresas de transporte escolar operan sin sistemas digitales, causando ineficiencia, falta de visibilidad y baja satisfacción de los padres.

**Solución:** Transportes Genesis es un sistema completo de gestión de transporte escolar con geolocalización en tiempo real, gestión de rutas, pagos y comunicación con padres.

**Mercado:** Empresas de transporte escolar con 5-50 buses en [país/región].

**Modelo de negocio:** SaaS con suscripción mensual de $50/bus.

**Equipo:** Desarrolladores con experiencia en .NET, SQL Server y soluciones empresariales.

**Proyección financiera:** ROI de 230% en 3 años, payback de 18 meses.

**Inversión requerida:** $25,000 para desarrollo inicial e infraestructura del primer año.

---

#### 3.8 Fuentes de financiamiento ⚡ OPCIONAL (mencionar brevemente)

- **Fondos propios**: $10,000
- **Inversores ángeles**: $15,000 (a cambio de X% equity)
- **Préstamo bancario**: No requerido en esta etapa

---

#### 3.9 Análisis de riesgos económicos ✅

| ID | Riesgo | Probabilidad | Impacto | Mitigación |
|----|--------|--------------|---------|------------|
| RE-01 | Baja adopción de clientes | Media | Alto | Marketing agresivo, free trial de 1 mes |
| RE-02 | Competencia con precios más bajos | Baja | Medio | Diferenciación por calidad, soporte |
| RE-03 | Aumento de costos de infraestructura | Media | Medio | Optimización de recursos, caching |
| RE-04 | Retraso en el desarrollo | Media | Alto | Metodología ágil, entregas incrementales |
| RE-05 | Cambios regulatorios (privacidad de datos) | Baja | Alto | Cumplimiento GDPR/LOPD desde el inicio |

---

## 📎 ANEXOS

### Glosario completo
(Ver sección 1.5)

### Referencias
- Documentación oficial de .NET: https://docs.microsoft.com/dotnet
- ASP.NET Core Identity: https://docs.microsoft.com/aspnet/core/security/authentication/identity
- SignalR: https://docs.microsoft.com/aspnet/core/signalr

### Historial de versiones
- v1.0 (YYYY-MM-DD): Versión inicial

---

## ✅ RESUMEN DEL PLAN

### Lo que vamos a generar:

1. ✅ **Archivo único**: `ESPECIFICACION_COMPLETA_TRANSPORTES_GENESIS.md`
2. ✅ **Estructura completa**: 1. Funcional, 2. Técnica, 3. Económica
3. ✅ **Items numerados**: TODOS obligatorios
4. ✅ **Sub-items con viñetas**: SOLO los relevantes
5. ✅ **Diagramas**: DESCRIPCIONES, no imágenes
6. ✅ **Basado en**: Código actual + documentos existentes

### Lo que NO vamos a hacer:

- ❌ Generar diagramas gráficos (solo instrucciones)
- ❌ Incluir TODO de los documentos anteriores (solo lo relevante)
- ❌ Inventar datos económicos precisos (usar estimaciones razonables)

---

## ❓ CONFIRMACIÓN FINAL

**¿Este plan es correcto?** ¿Algún ajuste antes de generar el documento completo?

Confirma y procedo a crear el archivo `.md` completo siguiendo este plan. 😊
