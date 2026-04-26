# FASE 7+: Funcionalidades Avanzadas - Rutas Dinámicas

## 🚦 Verificación de Tráfico y Recalculo Automático

### Objetivo
Permitir al piloto verificar el estado del tráfico en tiempo real y recalcular la ruta automáticamente para optimizar tiempos de recogida considerando condiciones actuales del tráfico.

---

## 📋 Funcionalidades Planeadas

### 1️⃣ **Verificar Tráfico en Tiempo Real**

**Descripción:**
- Integración con Google Maps Traffic API o Waze API
- Mostrar condiciones de tráfico en el mapa (verde/amarillo/rojo)
- Alertas de incidentes (accidentes, construcciones, cierres)
- Tiempo estimado actualizado según tráfico actual

**Implementación:**
```csharp
// Services/Interfaces/ITraficoService.cs
public interface ITraficoService
{
    Task<TraficoDto> ObtenerTraficoRutaAsync(int idRuta);
    Task<bool> HayAlertasEnRutaAsync(int idRuta);
    Task<TimeSpan> CalcularTiempoConTraficoAsync(decimal latOrigen, decimal lonOrigen, decimal latDestino, decimal lonDestino);
}
```

**API Externa:**
- Google Maps Directions API con `traffic_model=best_guess`
- Endpoint: `https://maps.googleapis.com/maps/api/directions/json`
- Costo: ~$5 USD por cada 1000 peticiones

---

### 2️⃣ **Recalcular Ruta Automáticamente**

**Descripción:**
- Botón "Recalcular Ruta" en interfaz del piloto
- Algoritmo considera:
  * Tráfico actual
  * Paradas ya completadas (no las incluye)
  * Paradas pendientes
  * Ubicación actual del bus (GPS en tiempo real)
- Genera nueva ruta optimizada desde posición actual

**Implementación:**
```csharp
// Services/Implementations/RutaService.cs
public async Task<RutaDto> RecalcularRutaConTraficoAsync(RecalcularRutaDto dto)
{
    // 1. Obtener paradas pendientes
    var paradasPendientes = await _rutaRepository
        .GetParadasPendientesAsync(dto.IdRuta);

    // 2. Obtener ubicación actual del bus
    var ubicacionActual = await _ubicacionBusService
        .GetUltimaUbicacionAsync(dto.IdBus);

    // 3. Consultar tráfico para cada segmento
    var tiemposConTrafico = await _traficoService
        .CalcularTiemposConTraficoAsync(paradasPendientes);

    // 4. Ejecutar algoritmo de optimización (TSP con pesos de tráfico)
    var rutaOptimizada = OptimizarConTrafico(
        ubicacionActual, 
        paradasPendientes, 
        tiemposConTrafico
    );

    // 5. Actualizar ruta en BD
    await ActualizarOrdenParadasAsync(dto.IdRuta, rutaOptimizada);

    return rutaOptimizada;
}
```

---

### 3️⃣ **Notificaciones Automáticas**

**Cuando se detecta tráfico pesado:**
- Notificación al piloto: "⚠️ Tráfico pesado detectado. ¿Recalcular ruta?"
- Notificación a padres: "🚌 El bus puede llegar 15 min tarde debido al tráfico"
- Notificación al admin: "📊 Bus 4 con retraso estimado de 20 min"

**Implementación con SignalR:**
```csharp
// Hubs/RutaHub.cs
public async Task NotificarTrafico(int idBus, string mensaje, int minutosRetraso)
{
    await Clients.Group($"bus-{idBus}").SendAsync("TraficoDetectado", new {
        mensaje = mensaje,
        minutosRetraso = minutosRetraso,
        timestamp = DateTime.Now
    });
}
```

---

## 🛠️ Requisitos Técnicos

### APIs Externas Necesarias
1. **Google Maps Directions API**
   - Traffic data en tiempo real
   - Cálculo de rutas alternativas
   - Costo: $5/1000 requests

2. **Google Maps Distance Matrix API**
   - Calcular tiempos entre múltiples puntos
   - Considera tráfico actual
   - Costo: $5/1000 requests

3. **Alternativa Open Source:**
   - OSRM (Open Source Routing Machine)
   - GraphHopper con datos de OpenStreetMap
   - **Gratis** pero sin datos de tráfico en tiempo real

### Cambios en Base de Datos
```sql
-- Tabla para guardar historial de recálculos
CREATE TABLE genesis.HistorialRecalculoRutas (
    IdHistorial INT PRIMARY KEY IDENTITY(1,1),
    IdRuta INT NOT NULL,
    FechaRecalculo DATETIME NOT NULL,
    MotivoRecalculo NVARCHAR(200), -- 'Tráfico pesado', 'Incidente reportado', etc.
    RutaAnterior NVARCHAR(MAX), -- JSON con orden anterior de paradas
    RutaNueva NVARCHAR(MAX), -- JSON con nuevo orden
    MinutosAhorrados INT, -- Tiempo ahorrado estimado
    Activo BIT DEFAULT 1,
    FechaRegistro DATETIME DEFAULT GETDATE()
);

-- Tabla para guardar alertas de tráfico
CREATE TABLE genesis.AlertasTrafico (
    IdAlerta INT PRIMARY KEY IDENTITY(1,1),
    IdRuta INT NOT NULL,
    TipoAlerta NVARCHAR(50), -- 'Accidente', 'Construcción', 'Cierre', 'Tráfico pesado'
    Descripcion NVARCHAR(500),
    Latitud DECIMAL(10,7),
    Longitud DECIMAL(10,7),
    FechaInicio DATETIME NOT NULL,
    FechaFin DATETIME,
    SeveridadAlerta NVARCHAR(20), -- 'Baja', 'Media', 'Alta', 'Crítica'
    Activo BIT DEFAULT 1,
    FechaRegistro DATETIME DEFAULT GETDATE()
);
```

---

## 📊 Flujo de Usuario (Piloto)

```
1. Piloto abre "Mi Ruta del Día"
   └─> Sistema muestra ruta calculada previamente

2. Piloto hace clic en "Verificar Tráfico"
   └─> Sistema consulta Google Maps Traffic API
   └─> Mapa se actualiza con colores de tráfico
   └─> Aparece alerta: "⚠️ Tráfico pesado en Calle 26"

3. Sistema detecta retraso estimado > 10 minutos
   └─> Muestra modal: "¿Deseas recalcular la ruta?"
   └─> Piloto hace clic en "Sí, recalcular"

4. Sistema ejecuta algoritmo de recálculo
   └─> Considera: Ubicación actual + Paradas pendientes + Tráfico
   └─> Genera nueva ruta optimizada
   └─> Actualiza mapa con nueva polyline
   └─> Actualiza orden de paradas en lista

5. Sistema envía notificaciones
   └─> A padres: "El orden de las paradas cambió. Nueva hora estimada: 7:15 AM"
   └─> Al admin: "Bus 4 recalculó ruta por tráfico"

6. Piloto confirma y continúa ruta optimizada
```

---

## 🎯 Prioridad de Implementación

### Fase 7 (Corto Plazo - 2-3 semanas)
- [x] ✅ Algoritmo básico de cálculo de rutas (COMPLETADO)
- [ ] ⏳ Integración con Google Maps Directions API
- [ ] ⏳ Botón "Verificar Tráfico" funcional
- [ ] ⏳ Visualización de tráfico en mapa

### Fase 8 (Mediano Plazo - 1 mes)
- [ ] ⏳ Algoritmo de recálculo con tráfico
- [ ] ⏳ Notificaciones en tiempo real (SignalR)
- [ ] ⏳ Historial de recálculos
- [ ] ⏳ Panel de alertas de tráfico

### Fase 9 (Largo Plazo - 2-3 meses)
- [ ] ⏳ Machine Learning para predecir tráfico
- [ ] ⏳ Sugerencias automáticas de recálculo
- [ ] ⏳ Integración con Waze para reportes comunitarios
- [ ] ⏳ Estadísticas de rendimiento de rutas

---

## 💰 Estimación de Costos

### Google Maps APIs (Mensual)
- **Escenario conservador** (50 buses, 2 turnos/día, 20 días/mes):
  * Directions API: 50 × 2 × 20 = 2,000 requests/mes
  * Costo: 2,000 × $0.005 = **$10 USD/mes**

- **Escenario medio** (100 buses, recálculos frecuentes):
  * ~10,000 requests/mes
  * Costo: **$50 USD/mes**

- **Escenario alto** (verificaciones cada 5 min):
  * ~50,000 requests/mes
  * Costo: **$250 USD/mes**

### Alternativa GRATIS
- Usar **OSRM** (Open Source)
- Instalar servidor propio
- **Sin costos de API**
- **Desventaja:** No tiene datos de tráfico en tiempo real

---

## 📝 Notas de Desarrollo

1. **Por ahora:** Botones deshabilitados con mensaje "Próximamente"
2. **Para habilitar:** Necesita decisión de presupuesto para APIs
3. **Alternativa MVP:** Mostrar alertas manuales ingresadas por admin
4. **Testing:** Crear simulador de tráfico con datos ficticios

---

**Estado actual:** ⏸️ PAUSADO - Pendiente de aprobación de presupuesto  
**Responsable:** Equipo de Desarrollo  
**Fecha de revisión:** Al completar FASE 6 completa  

---

Generado: @DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
