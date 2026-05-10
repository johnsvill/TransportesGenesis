# 📋 TAREAS PENDIENTES - TRANSPORTES GÉNESIS
## Estado Actual: 85% Completado | 15% Restante

**Fecha de Generación:** Enero 2025  
**Proyecto:** Sistema de Gestión de Transporte Escolar  
**Framework:** .NET 8 + Razor Pages  

---

## 📊 RESUMEN EJECUTIVO

| Categoría | Estado | Prioridad |
|-----------|--------|-----------|
| **Sprint 5 - Sistema de Traslados** | 🟡 85% (Bloqueado) | 🔴 Alta |
| **Sprint 8 - Testing & Deploy** | ⏳ 0% (Pendiente) | 🔴 Crítica |
| **Mejoras de Calidad (Technical Debt)** | ⏳ 0% (Pendiente) | 🟡 Media |
| **Funcionalidades v2.0** | ⏳ 0% (Planificado) | 🟢 Baja |

---

## 🚨 BLOQUEADORES CRÍTICOS (Sprint 5)

### 🔴 PRIORIDAD CRÍTICA - Responsable: Jonathan

#### 1. Migraciones de Tabla Pagos
**Estado:** ⚠️ **BLOQUEADO - REQUERIDO**  
**Responsable:** Jonathan Villeda  
**Estimación:** 8-12 horas

**Descripción:**
Las migraciones de la tabla `Pagos` y entidades relacionadas están pendientes, lo que bloquea funcionalidades completas del módulo de pagos.

**Tareas Específicas:**
```bash
# 1. Crear migraciones faltantes
dotnet ef migrations add AddPagosCompleteMigration

# 2. Verificar el script SQL generado
dotnet ef migrations script

# 3. Aplicar migraciones a la BD
dotnet ef database update

# 4. Validar que las tablas se crearon correctamente
```

**Tablas Afectadas:**
- `Pagos` (tabla principal)
- `TipoRecorridoPago` (relación)
- `Banco` (catálogo)
- `TipoCuenta` (catálogo)

**Dependencias Bloqueadas:**
- ✅ Backend de Pagos (completado)
- ✅ Repositorios y Services (completado)
- ✅ Páginas CRUD (completado)
- ❌ **Validaciones de fecha** (bloqueado)
- ❌ **Integración con Login** (bloqueado)
- ❌ **Testing de pagos** (bloqueado)

**Validación:**
```sql
-- Verificar que las tablas existen
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME IN ('Pagos', 'TipoRecorridoPago', 'Banco', 'TipoCuenta');

-- Verificar estructura de tabla Pagos
SELECT COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Pagos';
```

---

#### 2. Validaciones de Pagos
**Estado:** ⚠️ **BLOQUEADO** (Requiere migraciones)  
**Responsable:** Jonathan Villeda  
**Estimación:** 4-6 horas

**Tareas:**
- [ ] Validar que `FechaPago` no sea futura
- [ ] Validar que `Monto` sea > 0
- [ ] Validar que `IdAlumno` exista en BD
- [ ] Validar que `Comprobante` sea una imagen válida (JPEG/PNG)
- [ ] Validar tamaño máximo de comprobante (5MB)
- [ ] Agregar validaciones en modelo con `DataAnnotations`
- [ ] Agregar validaciones en PageModel con `ModelState`

**Código de Ejemplo:**
```csharp
// Models/DB/Negocio/Pago.cs
public class Pago
{
    public int IdPago { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [DateNotInFuture(ErrorMessage = "La fecha de pago no puede ser futura")]
    public DateTime FechaPago { get; set; }

    [Required]
    [Range(0.01, 10000, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Monto { get; set; }

    [Required]
    public int IdAlumno { get; set; }

    [ValidImageFile(MaxSizeInMB = 5)]
    public string? Comprobante { get; set; }

    // ... resto de propiedades
}
```

---

#### 3. Integración Login con Pagos
**Estado:** ⚠️ **BLOQUEADO** (Requiere migraciones)  
**Responsable:** Jonathan Villeda  
**Estimación:** 6-8 horas

**Tareas:**
- [ ] Filtrar pagos por usuario autenticado (`User.FindFirstValue(ClaimTypes.NameIdentifier)`)
- [ ] Mostrar solo pagos de hijos del padre logueado
- [ ] Implementar autorización en páginas de pagos: `[Authorize(Roles = "PadreDeFamilia")]`
- [ ] Agregar navegación a pagos en `_Layout.cshtml` (solo visible para padres)
- [ ] Crear vista de "Mis Pagos" para padres
- [ ] Crear vista de "Gestión de Pagos" para admin (confirmar/rechazar)
- [ ] Integrar subida de comprobantes con Azure Blob Storage o sistema de archivos
- [ ] Agregar lógica de confirmación de pagos por administrador

**Archivos a Modificar:**
```
📁 Pages/Padres/
├── 📄 MisPagos.cshtml (nueva)
├── 📄 MisPagos.cshtml.cs (nueva)
└── 📄 RegistrarPago.cshtml (nueva)

📁 Pages/Admin/
├── 📄 GestionPagos.cshtml (nueva)
└── 📄 GestionPagos.cshtml.cs (nueva)

📁 Pages/Shared/
└── 📄 _Layout.cshtml (agregar link a pagos)
```

---

## 🧪 SPRINT 8: TESTING FINAL & DEPLOY A PRODUCCIÓN

### Estado: ⏳ **PENDIENTE - NO INICIADO**
### Responsables: David + Jonathan
### Estimación Total: 80-100 horas

---

### 📝 A. TESTING Y VALIDACIÓN

#### A.1. Testing Unitario (xUnit)
**Estimación:** 40 horas  
**Objetivo:** 60% Code Coverage mínimo

**Tareas:**
- [ ] Configurar proyecto de testing `TransportesGenesis.Tests`
- [ ] Instalar NuGet packages: `xUnit`, `xUnit.runner.visualstudio`, `Moq`, `FluentAssertions`
- [ ] Crear tests para **Algoritmo TSP** (`RutaService`)
  - [ ] Test: Ruta con 3 paradas (caso simple)
  - [ ] Test: Ruta con 10 paradas (caso medio)
  - [ ] Test: Ruta con 30 paradas (caso complejo)
  - [ ] Test: Ruta sin paradas (caso edge)
  - [ ] Test: Ruta con una sola parada (caso edge)
- [ ] Crear tests para **Servicios**
  - [ ] `AlertaService`: Crear alerta, verificar duplicados, marcar resuelta
  - [ ] `AsistenciaService`: Confirmar asistencia, validar horarios
  - [ ] `TrasladoService`: Crear solicitud, aprobar/rechazar
- [ ] Crear tests para **Validaciones**
  - [ ] Validaciones de `Pago` (fecha no futura, monto > 0)
  - [ ] Validaciones de `Alumno` (edad entre 3-18 años)
  - [ ] Validaciones de `Parada` (latitud/longitud válidas)
- [ ] Crear tests para **DTOs y Mappers**
  - [ ] `RutaDto` mapping correcto
  - [ ] `AlumnoEnParadaDto` mapping correcto
  - [ ] AutoMapper configuration tests

**Estructura de Tests:**
```
📁 TransportesGenesis.Tests/
├── 📁 Services/
│   ├── RutaServiceTests.cs
│   ├── AlertaServiceTests.cs
│   └── AsistenciaServiceTests.cs
├── 📁 Validations/
│   ├── PagoValidationTests.cs
│   └── AlumnoValidationTests.cs
├── 📁 DTOs/
│   └── RutaDtoMappingTests.cs
└── 📁 Helpers/
    └── TestDataBuilder.cs
```

**Ejemplo de Test:**
```csharp
public class RutaServiceTests
{
    [Fact]
    public void CalcularRutaOptima_ConTresParadas_DebeRetornarOrdenCorrecto()
    {
        // Arrange
        var paradas = new List<Parada>
        {
            new Parada { Id = 1, Latitud = 14.1, Longitud = -87.2 },
            new Parada { Id = 2, Latitud = 14.15, Longitud = -87.25 },
            new Parada { Id = 3, Latitud = 14.08, Longitud = -87.18 }
        };
        var service = new RutaService();

        // Act
        var resultado = service.CalcularRutaOptima(paradas);

        // Assert
        resultado.Should().HaveCount(3);
        resultado[0].Orden.Should().Be(1);
        resultado[1].Orden.Should().Be(2);
        resultado[2].Orden.Should().Be(3);
    }
}
```

---

#### A.2. Testing de Integración
**Estimación:** 20 horas

**Tareas:**
- [ ] Configurar `WebApplicationFactory<Program>` para testing de integración
- [ ] Crear tests para **APIs REST**
  - [ ] GET `/api/rutas/bus/{id}/activa` retorna 200 con ruta
  - [ ] GET `/api/rutas/bus/{id}/activa` retorna 404 si no hay ruta
  - [ ] POST `/api/traslados` crea solicitud correctamente
  - [ ] PUT `/api/alertas/{id}/resolver` marca alerta como resuelta
- [ ] Crear tests para **SignalR Hub**
  - [ ] Conexión exitosa al `NotificacionesHub`
  - [ ] Envío de ubicación vía `SendLocation`
  - [ ] Recepción de ubicación vía `ReceiveLocation`
  - [ ] Envío de alerta vía `SendAlerta`
- [ ] Crear tests para **Flujos completos por rol**
  - [ ] Piloto: Login → Ver ruta → Enviar ubicación
  - [ ] Monitor: Login → Ver ruta → Registrar recogidas
  - [ ] Padre: Login → Ver mapa → Confirmar asistencia → Ver pagos

**Ejemplo de Test de Integración:**
```csharp
public class RutasApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RutasApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetRutaActiva_ConIdValido_DebeRetornar200()
    {
        // Arrange
        var idBus = 4;

        // Act
        var response = await _client.GetAsync($"/api/rutas/bus/{idBus}/activa");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        var ruta = JsonSerializer.Deserialize<RutaDto>(content);
        ruta.Should().NotBeNull();
        ruta.IdBus.Should().Be(idBus);
    }
}
```

---

#### A.3. Testing Manual Exhaustivo
**Estimación:** 8 horas

**Tareas:**
- [ ] Seguir **checklist de verificación** (`Documentos/CHECKLIST_VERIFICACION.md`)
- [ ] Ejecutar casos de prueba documentados (`Documentos/PRUEBAS_E_INSTRUCCIONES_PENDIENTES.md`)
- [ ] Probar flujos completos:
  - [ ] Admin: Crear usuarios → Asignar buses → Calcular rutas
  - [ ] Piloto: Login → Ver ruta → Enviar ubicación GPS
  - [ ] Monitor: Login → Ver ruta → Registrar recogidas
  - [ ] Padre: Login → Ver mapa tiempo real → Confirmar asistencia → Registrar pago
- [ ] Probar en diferentes navegadores:
  - [ ] Chrome (últimas 2 versiones)
  - [ ] Firefox (últimas 2 versiones)
  - [ ] Edge (últimas 2 versiones)
  - [ ] Safari (si disponible)
- [ ] Probar en diferentes dispositivos:
  - [ ] Desktop (1920x1080)
  - [ ] Tablet (768x1024)
  - [ ] Móvil (375x667)
- [ ] Documentar bugs encontrados en un archivo `BUGS_ENCONTRADOS.md`

---

#### A.4. Corrección de Bugs
**Estimación:** 12 horas (variable según bugs encontrados)

**Proceso:**
1. Priorizar bugs por severidad (Crítico → Alto → Medio → Bajo)
2. Crear issues en GitHub para cada bug
3. Resolver bugs críticos y altos antes de deploy
4. Documentar bugs medios/bajos para v2.0

---

### 🚀 B. DEPLOY A PRODUCCIÓN

#### B.1. Configuración Azure Production
**Estimación:** 12 horas

**Tareas:**
- [ ] **Crear recursos en Azure Portal:**
  - [ ] App Service Plan (Standard S1 o superior)
  - [ ] App Service para la aplicación web
  - [ ] Azure SQL Database (Standard S0 o S1)
  - [ ] Azure Storage Account (para comprobantes de pago)
  - [ ] Azure Application Insights (monitoreo)
- [ ] **Configurar App Service:**
  - [ ] Configurar .NET 8 runtime
  - [ ] Habilitar Always On
  - [ ] Configurar SSL/TLS (certificado gratis de Azure)
  - [ ] Configurar variables de entorno (Connection Strings, API Keys)
  - [ ] Habilitar logging y diagnósticos
- [ ] **Configurar Azure SQL Database:**
  - [ ] Configurar firewall rules
  - [ ] Crear usuario de aplicación con permisos mínimos
  - [ ] Habilitar backups automáticos diarios
  - [ ] Configurar retention de 30 días
- [ ] **Configurar Azure Storage:**
  - [ ] Crear container `comprobantes-pagos` (privado)
  - [ ] Configurar SAS tokens para acceso temporal
  - [ ] Implementar limpieza automática de archivos antiguos

**Variables de Entorno a Configurar:**
```json
{
  "ConnectionStrings:DefaultConnection": "Server=tcp:...",
  "GoogleMapsApiKey": "YOUR_API_KEY",
  "AzureStorage:ConnectionString": "DefaultEndpointsProtocol=https;...",
  "AzureStorage:ContainerName": "comprobantes-pagos",
  "SignalR:ConnectionString": "...",
  "ApplicationInsights:InstrumentationKey": "..."
}
```

---

#### B.2. CI/CD con GitHub Actions
**Estimación:** 8 horas

**Tareas:**
- [ ] Crear workflow file `.github/workflows/azure-deploy.yml`
- [ ] Configurar triggers: push a `main` branch
- [ ] Configurar secrets en GitHub:
  - [ ] `AZURE_WEBAPP_PUBLISH_PROFILE`
  - [ ] `AZURE_SQL_CONNECTION_STRING`
- [ ] Configurar pipeline:
  - [ ] Build (dotnet build)
  - [ ] Test (dotnet test)
  - [ ] Publish (dotnet publish)
  - [ ] Deploy to Azure App Service
- [ ] Configurar notificaciones de deploy (email/Slack)
- [ ] Probar deploy manual primero
- [ ] Validar deploy automático con commit de prueba

**Ejemplo de Workflow:**
```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --configuration Release

      - name: Test
        run: dotnet test --no-build --verbosity normal

      - name: Publish
        run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/myapp

      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: 'transportes-genesis-prod'
          publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
          package: ${{env.DOTNET_ROOT}}/myapp
```

---

#### B.3. Backups y Monitoreo
**Estimación:** 6 horas

**Tareas Backups:**
- [ ] Configurar backups automáticos de Azure SQL Database
  - [ ] Frecuencia: Diaria a las 3 AM
  - [ ] Retention: 30 días
  - [ ] Point-in-time restore habilitado (7 días)
- [ ] Configurar backups de Azure Storage (comprobantes)
  - [ ] Replicación: LRS (Locally Redundant Storage) mínimo
  - [ ] Soft delete habilitado (14 días)
- [ ] Documentar proceso de restauración de backups
- [ ] Probar restauración de backup al menos una vez

**Tareas Monitoreo:**
- [ ] Configurar **Azure Application Insights**
  - [ ] Logging de excepciones
  - [ ] Tracking de performance (response time)
  - [ ] Alertas de disponibilidad (uptime < 99%)
  - [ ] Alertas de errores (> 10 errores/hora)
- [ ] Configurar **Azure Monitor**
  - [ ] Alertas de CPU (> 80% por 5 min)
  - [ ] Alertas de memoria (> 90% por 5 min)
  - [ ] Alertas de conexiones SQL (> 80% límite)
- [ ] Configurar **Health Checks**
  - [ ] Endpoint `/health` para ping
  - [ ] Verificar BD, Storage, SignalR
- [ ] Crear **Dashboard de Monitoreo**
  - [ ] Métricas de usuarios activos
  - [ ] Requests por segundo
  - [ ] Errores 4xx y 5xx
  - [ ] Tiempo de respuesta promedio

---

#### B.4. Documentación de Deploy
**Estimación:** 4 horas

**Tareas:**
- [ ] Crear documento `GUIA_DEPLOY.md` con:
  - [ ] Prerrequisitos (cuenta Azure, GitHub, etc.)
  - [ ] Paso a paso de configuración de recursos Azure
  - [ ] Paso a paso de configuración CI/CD
  - [ ] Variables de entorno requeridas
  - [ ] Comandos de verificación post-deploy
- [ ] Crear documento `TROUBLESHOOTING.md` con:
  - [ ] Problemas comunes y soluciones
  - [ ] Cómo acceder a logs de App Service
  - [ ] Cómo verificar estado de la BD
  - [ ] Cómo hacer rollback a versión anterior
- [ ] Crear documento `MAINTENANCE.md` con:
  - [ ] Proceso de actualización de paquetes NuGet
  - [ ] Proceso de actualización de .NET version
  - [ ] Proceso de limpieza de logs antiguos
  - [ ] Proceso de revisión de backups

---

## 🔧 MEJORAS DE CALIDAD (TECHNICAL DEBT)

### Estado: ⏳ **PENDIENTE - BACKLOG**
### Prioridad: 🟡 Media
### Estimación Total: 100-120 horas

---

### 1. Implementar Caching con Redis
**Prioridad:** 🟡 Media  
**Estimación:** 16 horas

**Justificación:**
Reducir consultas repetitivas a la BD para datos que no cambian frecuentemente (rutas activas, catálogos, etc.)

**Tareas:**
- [ ] Instalar NuGet package `Microsoft.Extensions.Caching.StackExchangeRedis`
- [ ] Configurar Azure Redis Cache (Basic C0 $15/mes)
- [ ] Implementar caching en `RutaService`:
  - [ ] Cache de rutas activas (TTL: 1 hora)
  - [ ] Invalidación al recalcular ruta
- [ ] Implementar caching en catálogos:
  - [ ] Bancos, TipoCuenta, etc. (TTL: 24 horas)
- [ ] Implementar caching distribuido para sesiones (opcional)
- [ ] Medir impacto en performance (antes/después)

**Costo Estimado:** $15-$50/mes (Azure Redis Cache)

---

### 2. Auditoría de Seguridad (Penetration Testing)
**Prioridad:** 🟡 Media  
**Estimación:** 20 horas (incluye remediación)

**Tareas:**
- [ ] Contratar auditoría de seguridad externa (opcional)
- [ ] Usar herramientas automáticas:
  - [ ] OWASP ZAP (escaneo de vulnerabilidades)
  - [ ] SonarQube (análisis estático de código)
  - [ ] Dependabot (vulnerabilidades en paquetes NuGet)
- [ ] Verificar puntos críticos:
  - [ ] SQL Injection (EF Core parametrizado ✅)
  - [ ] XSS (Razor Pages sanitiza ✅)
  - [ ] CSRF (antiforgery tokens ✅)
  - [ ] Authentication bypass
  - [ ] Authorization flaws
  - [ ] Sensitive data exposure
- [ ] Corregir vulnerabilidades encontradas
- [ ] Documentar hallazgos y soluciones

---

### 3. Centralizar Logging con Serilog + Azure Application Insights
**Prioridad:** 🟡 Media  
**Estimación:** 12 horas

**Tareas:**
- [ ] Instalar NuGet packages:
  - [ ] `Serilog.AspNetCore`
  - [ ] `Serilog.Sinks.ApplicationInsights`
  - [ ] `Serilog.Sinks.File` (fallback)
- [ ] Configurar Serilog en `Program.cs`
- [ ] Configurar niveles de log:
  - [ ] Development: Debug y superior
  - [ ] Production: Information y superior
- [ ] Implementar logging estructurado:
  ```csharp
  _logger.LogInformation("Ruta calculada {RutaId} para bus {BusId} con {CantidadParadas} paradas",
      ruta.Id, ruta.IdBus, ruta.Paradas.Count);
  ```
- [ ] Configurar filtros para no loggear datos sensibles (contraseñas, tokens)
- [ ] Crear queries útiles en Application Insights
- [ ] Configurar alertas de logs críticos

---

### 4. Refactoring: Extraer Lógica a Services
**Prioridad:** 🟡 Media  
**Estimación:** 20 horas

**Justificación:**
Actualmente algunos PageModels tienen lógica de negocio que debería estar en Services.

**Tareas:**
- [ ] Crear `IPagoService` e implementar `PagoService`
  - [ ] Mover lógica de validación de pagos
  - [ ] Mover lógica de confirmación/rechazo
- [ ] Crear `IAsistenciaService` (si no existe)
  - [ ] Mover lógica de validación de horarios
  - [ ] Mover lógica de procesamiento automático
- [ ] Crear `IRecogidaService`
  - [ ] Mover lógica de registro de recogidas
  - [ ] Mover lógica de prevención de duplicados
- [ ] Actualizar PageModels para usar Services
- [ ] Actualizar tests para usar mocks de Services

**Patrón:**
```
PageModel → Service → Repository → Database
```

---

### 5. Implementar CI/CD para Testing Automático
**Prioridad:** 🟢 Baja  
**Estimación:** 8 horas

**Tareas:**
- [ ] Agregar step de testing en GitHub Actions workflow
- [ ] Configurar code coverage report con Coverlet
- [ ] Configurar quality gate: build falla si coverage < 60%
- [ ] Integrar con SonarCloud (análisis de calidad)
- [ ] Configurar notificaciones de fallos en tests

---

### 6. Documentación API con Swagger
**Prioridad:** 🟢 Baja  
**Estimación:** 6 horas

**Tareas:**
- [ ] Instalar NuGet package `Swashbuckle.AspNetCore`
- [ ] Configurar Swagger en `Program.cs`
- [ ] Agregar XML comments a controllers y DTOs
- [ ] Configurar autenticación en Swagger (Bearer token)
- [ ] Probar todos los endpoints desde Swagger UI
- [ ] Generar documentación estática (opcional)

**URL:** `https://transportesgenesis.azurewebsites.net/swagger`

---

## 🚀 FUNCIONALIDADES v2.0 (BACKLOG FUTURO)

### Estado: ⏳ **PLANIFICADO - NO INICIADO**
### Prioridad: 🟢 Baja
### Estimación Total: 200+ horas

---

### 1. Notificaciones Push (Firebase/OneSignal)
**Prioridad:** 🔴 Alta (para v2.0)  
**Estimación:** 60 horas

**Funcionalidades:**
- Notificar a padres cuando el bus está a 5 min de la parada
- Notificar a padres sobre retrasos
- Notificar a admin sobre pagos pendientes
- Notificar a pilotos sobre cambios en ruta
- Notificar a monitores sobre alumnos sin confirmar asistencia

**Tecnologías:**
- Firebase Cloud Messaging (FCM) para web/móvil
- OneSignal como alternativa (más fácil)

---

### 2. Reportes Avanzados (Excel, PDF)
**Prioridad:** 🟡 Media  
**Estimación:** 30 horas

**Reportes a Implementar:**
- Reporte de pagos por rango de fechas (Excel/PDF)
- Reporte de asistencia por alumno (Excel/PDF)
- Reporte de recogidas por bus y fecha (Excel/PDF)
- Reporte de eficiencia de rutas (km recorridos, tiempo)
- Reporte de alertas y retrasos (estadísticas)

**Librerías:**
- `EPPlus` para Excel
- `iTextSharp` o `QuestPDF` para PDF

---

### 3. Dashboard con Gráficos (Chart.js)
**Prioridad:** 🟡 Media  
**Estimación:** 25 horas

**Gráficos a Implementar:**
- Gráfico de pagos mensuales (barras)
- Gráfico de asistencia semanal (líneas)
- Gráfico de alertas por tipo (pie chart)
- Gráfico de eficiencia de rutas (líneas)
- Mapa de calor de zonas con más alumnos

---

### 4. Búsqueda y Filtros Avanzados
**Prioridad:** 🟢 Baja  
**Estimación:** 20 horas

**Funcionalidades:**
- Búsqueda global (alumnos, buses, rutas, pagos)
- Filtros avanzados en listados (por fecha, estado, tipo)
- Ordenamiento por columnas
- Paginación optimizada (query sin traer todos los datos)

---

### 5. Histórico de Ubicaciones (Consulta)
**Prioridad:** 🟢 Baja  
**Estimación:** 25 horas

**Funcionalidades:**
- Guardar histórico de ubicaciones en BD
- Consultar histórico de un bus en un rango de fechas
- Reproducir ruta histórica en mapa (playback)
- Generar reporte de desvíos de ruta

**Impacto en BD:**
- Tabla `Ubicaciones` crecerá rápidamente (1 registro cada 10s por bus)
- Considerar particionamiento o archivado de datos antiguos

---

### 6. Multi-Idioma (i18n)
**Prioridad:** 🟢 Baja  
**Estimación:** 30 horas

**Idiomas a Soportar:**
- Español (actual)
- Inglés (internacional)

**Tareas:**
- Configurar recursos `.resx` para cada idioma
- Crear archivo `Resources/Strings.es.resx` y `Strings.en.resx`
- Actualizar todas las vistas para usar recursos
- Agregar selector de idioma en layout
- Guardar preferencia de idioma en cookie/perfil de usuario

---

### 7. App Móvil Nativa (.NET MAUI)
**Prioridad:** 🟢 Baja  
**Estimación:** 200+ horas

**Funcionalidades:**
- Login/Logout
- Ver mapa en tiempo real (nativo)
- Confirmar asistencia (calendario nativo)
- Registrar pagos (cámara para comprobante)
- Notificaciones push nativas
- Modo offline (sincronizar cuando hay internet)

**Plataformas:**
- Android (prioridad)
- iOS (si hay presupuesto para licencia Apple Developer)

---

## 📊 ESTIMACIÓN DE TIEMPO TOTAL

| Categoría | Horas | Responsable |
|-----------|-------|-------------|
| **Sprint 5 - Bloqueadores** | 18-26 | Jonathan |
| **Sprint 8 - Testing** | 80 | David + Jonathan |
| **Sprint 8 - Deploy** | 30 | David |
| **Technical Debt** | 100 | Equipo |
| **Funcionalidades v2.0** | 390 | Futuro |
| **TOTAL PENDIENTE** | **128-136 horas** (para MVP Production) |
| **TOTAL v2.0** | **+390 horas** |

---

## 🎯 PRIORIZACIÓN RECOMENDADA

### Fase 1: Desbloqueadores (Semana 1)
1. ✅ **Migraciones de Pagos** (Jonathan) - 12 horas
2. ✅ **Validaciones de Pagos** (Jonathan) - 6 horas
3. ✅ **Integración Login con Pagos** (Jonathan) - 8 horas

**Total Fase 1:** 26 horas (~3-4 días de trabajo)

---

### Fase 2: Testing (Semana 2-3)
1. ✅ **Testing Unitario** (David + Jonathan) - 40 horas
2. ✅ **Testing de Integración** (David) - 20 horas
3. ✅ **Testing Manual** (Equipo) - 8 horas
4. ✅ **Corrección de Bugs** (Equipo) - 12 horas

**Total Fase 2:** 80 horas (~2 semanas)

---

### Fase 3: Deploy a Producción (Semana 4)
1. ✅ **Configuración Azure** (David) - 12 horas
2. ✅ **CI/CD GitHub Actions** (David) - 8 horas
3. ✅ **Backups y Monitoreo** (David) - 6 horas
4. ✅ **Documentación Deploy** (David) - 4 horas

**Total Fase 3:** 30 horas (~1 semana)

---

### Fase 4: Mejoras de Calidad (Post-Deploy, bajo demanda)
- Implementar según prioridad de negocio
- Estimación: 10-20 horas/semana durante 2-3 meses

---

### Fase 5: Funcionalidades v2.0 (3-6 meses)
- Planificar roadmap con stakeholders
- Priorizar según feedback de usuarios
- Estimación: 390+ horas (~2-3 meses de trabajo full-time)

---

## 📅 CRONOGRAMA SUGERIDO

```
┌─────────────┬───────────────────────────────────────┬──────────┐
│   Semana    │              Actividad                │   Hrs    │
├─────────────┼───────────────────────────────────────┼──────────┤
│  Semana 1   │ Desbloqueadores Sprint 5 (Jonathan)   │   26     │
├─────────────┼───────────────────────────────────────┼──────────┤
│ Semana 2-3  │ Testing Completo (Equipo)             │   80     │
├─────────────┼───────────────────────────────────────┼──────────┤
│  Semana 4   │ Deploy a Producción (David)           │   30     │
├─────────────┼───────────────────────────────────────┼──────────┤
│ Semana 5+   │ Mejoras de Calidad (bajo demanda)     │   100    │
├─────────────┼───────────────────────────────────────┼──────────┤
│ 3-6 meses   │ Funcionalidades v2.0 (planificado)    │   390+   │
└─────────────┴───────────────────────────────────────┴──────────┘

🎯 MVP en Producción: 4 semanas (136 horas)
🚀 v2.0 Completa: 6 meses adicionales
```

---

## 🚨 RIESGOS Y MITIGACIÓN

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|--------------|---------|------------|
| Migraciones de pagos fallan al aplicar | 🟡 Media | 🔴 Alto | Probar en BD de desarrollo primero; tener backup |
| Tests descubren bugs críticos | 🟡 Media | 🟡 Medio | Resolver antes de deploy; extender timeline si es necesario |
| Deploy a Azure falla por configuración incorrecta | 🟢 Baja | 🔴 Alto | Probar en staging environment primero |
| Costos de Azure superan presupuesto | 🟢 Baja | 🟡 Medio | Monitorear costos semanalmente; optimizar recursos |
| Falta de tiempo para completar testing | 🟡 Media | 🔴 Alto | Priorizar tests críticos; diferir tests de v2.0 |

---

## 📞 CONTACTO Y RESPONSABILIDADES

| Responsable | Área | Tareas Pendientes |
|-------------|------|-------------------|
| **Jonathan Villeda** | Backend & Pagos | Sprint 5 Bloqueadores (26h), Testing Unitario Pagos (8h) |
| **David** | Geolocalización & Testing | Testing Completo (60h), Deploy Azure (30h) |
| **Equipo Completo** | QA & Deploy | Testing Manual (8h), Corrección Bugs (12h) |

---

**📅 Última Actualización:** Enero 2025  
**📊 Estado del Proyecto:** 85% Completado  
**🎯 Próximo Hito:** MVP en Producción (4 semanas)  
**🚀 Siguiente Versión:** v2.0 (6 meses)

---

**FIN DEL DOCUMENTO: TAREAS_PENDIENTES.md**
