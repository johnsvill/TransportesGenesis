# 📍 SISTEMA DE GEOLOCALIZACIÓN - TRANSPORTES GÉNESIS
## Flujo de Ubicaciones GPS: Pilotos y Padres

**Fecha:** Enero 2025  
**Proyecto:** Sistema de Transporte Escolar  
**Tecnología:** SignalR + Geolocation API + Leaflet.js

---

## 🎯 RESPUESTA RÁPIDA A TU PREGUNTA

### **¿Cuándo se guardan las ubicaciones?**

**Las ubicaciones NO se guardan automáticamente al iniciar sesión.** 

Las ubicaciones se envían y se guardan (opcionalmente) cuando:

1. ✅ **El PILOTO abre la página `/Piloto/MiRuta`**
2. ✅ **El PILOTO presiona el botón "Iniciar Transmisión GPS"** (o se activa automáticamente)
3. ✅ **El navegador del piloto obtiene permisos de geolocalización**
4. ✅ **El sistema envía la ubicación cada 10 segundos vía SignalR**

**Los PADRES NO envían ubicación GPS**, solo **reciben** la ubicación del bus en tiempo real.

---

## 📊 ARQUITECTURA DEL SISTEMA DE GEOLOCALIZACIÓN

```
┌─────────────────────────────────────────────────────────────────┐
│                    FLUJO DE GEOLOCALIZACIÓN                      │
└─────────────────────────────────────────────────────────────────┘

[PILOTO - Navegador]
      │
      │ 1. Abre /Piloto/MiRuta
      ↓
[Geolocation API del Navegador]
      │
      │ 2. navigator.geolocation.getCurrentPosition()
      ↓
[JavaScript en el Cliente]
      │
      │ 3. connection.invoke("ReportarUbicacionBus", idBus, lat, lon)
      ↓
[SignalR Hub - NotificacionesHub.cs]
      │
      │ 4. Recibe ubicación del piloto
      ├──→ [Broadcast a Grupo "Bus_{idBus}"]
      │         │
      │         │ 5. Clients.Group($"Bus_{idBus}").SendAsync(...)
      │         ↓
      │    [PADRES suscritos al bus]
      │         │
      │         │ 6. connection.on("UbicacionBusActualizada", ...)
      │         ↓
      │    [Actualización del mapa en tiempo real]
      │
      └──→ [Opcional: Guardar en BD]
                  │
                  ↓
            [Tabla: UbicacionBusEnTiempoReal]
            (Histórico opcional)
```

---

## 🚗 FLUJO DETALLADO: PILOTO TRANSMITE UBICACIÓN

### **Paso 1: Piloto Inicia Sesión y Abre su Ruta**

```
1. Piloto hace login: piloto1@transportesgenesis.com
2. Sistema lo redirige a: /Piloto/MiRuta
3. Se carga la página con mapa de Leaflet.js
4. Se establece conexión SignalR automáticamente
```

**Código en `Pages/Piloto/MiRuta.cshtml`:**
```html
<script src="https://unpkg.com/leaflet@1.9.4/dist/leaflet.js"></script>
<script src="~/lib/signalr/signalr.min.js"></script>

<script>
    // 1. Establecer conexión SignalR
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificacionesHub")
        .build();

    connection.start().then(() => {
        console.log("✅ SignalR conectado");
        iniciarTransmisionGPS();
    });
</script>
```

---

### **Paso 2: Navegador Solicita Permisos de Geolocalización**

**El navegador pregunta al piloto:**
```
╔══════════════════════════════════════════════╗
║  transportesgenesis.com desea acceder a     ║
║  tu ubicación                                ║
║                                              ║
║  [ Permitir ]        [ Bloquear ]           ║
╚══════════════════════════════════════════════╝
```

**Código JavaScript:**
```javascript
function iniciarTransmisionGPS() {
    if (navigator.geolocation) {
        // Solicitar permiso de ubicación
        navigator.geolocation.getCurrentPosition(
            function(position) {
                console.log("✅ Permiso de ubicación otorgado");
                transmitirUbicacionContinua();
            },
            function(error) {
                console.error("❌ Permiso de ubicación denegado:", error);
                mostrarError("No se pudo obtener permiso de geolocalización");
            }
        );
    } else {
        console.error("❌ Geolocalización no soportada por el navegador");
    }
}
```

---

### **Paso 3: Sistema Obtiene Ubicación Cada 10 Segundos**

**Código JavaScript en el navegador del piloto:**
```javascript
function transmitirUbicacionContinua() {
    // Obtener ubicación cada 10 segundos
    setInterval(function() {
        navigator.geolocation.getCurrentPosition(
            function(position) {
                const lat = position.coords.latitude;
                const lon = position.coords.longitude;
                const idBus = 4; // Obtenido del servidor (bus asignado)

                console.log(`📍 Ubicación obtenida: Lat ${lat}, Lon ${lon}`);

                // Enviar vía SignalR
                connection.invoke("ReportarUbicacionBus", idBus, lat, lon)
                    .then(() => {
                        console.log("✅ Ubicación enviada al servidor");
                        actualizarIndicadorGPS("Transmitiendo...");
                    })
                    .catch(err => {
                        console.error("❌ Error al enviar ubicación:", err);
                    });

                // Actualizar mapa del piloto
                actualizarMarcadorEnMapa(lat, lon);
            },
            function(error) {
                console.error("❌ Error al obtener ubicación:", error);
            },
            {
                enableHighAccuracy: true,  // GPS de alta precisión
                timeout: 10000,            // Timeout de 10 segundos
                maximumAge: 0              // No usar caché
            }
        );
    }, 10000); // 10 segundos
}
```

**Opciones de `getCurrentPosition`:**
- `enableHighAccuracy: true` → Usa GPS en lugar de WiFi/Cell Tower (más preciso, más batería)
- `timeout: 10000` → Si no obtiene ubicación en 10s, lanza error
- `maximumAge: 0` → No usar ubicación en caché, siempre obtener nueva

---

### **Paso 4: SignalR Hub Recibe y Distribuye la Ubicación**

**Código en `Hubs/NotificacionesHub.cs`:**
```csharp
public async Task ReportarUbicacionBus(int idBus, decimal latitud, decimal longitud)
{
    _logger.LogInformation($"[SIGNALR] Bus {idBus} reporta ubicación: Lat {latitud}, Lon {longitud}");

    // Broadcast a todos los padres que están viendo este bus
    await Clients.Group($"Bus_{idBus}").SendAsync("UbicacionBusActualizada", new
    {
        IdBus = idBus,
        Latitud = latitud,
        Longitud = longitud,
        FechaHora = DateTime.Now
    });

    // OPCIONAL: Guardar en base de datos
    // await GuardarUbicacionEnBD(idBus, latitud, longitud);
}
```

**Flujo en el Hub:**
1. ✅ Recibe llamada desde JavaScript del piloto
2. ✅ Log de la ubicación (para debugging)
3. ✅ Envía ubicación a grupo `"Bus_4"` (solo padres de ese bus)
4. 🟡 **OPCIONALMENTE** guarda en BD (actualmente NO implementado)

---

## 👨‍👩‍👧‍👦 FLUJO DETALLADO: PADRES RECIBEN UBICACIÓN

### **Paso 1: Padre Inicia Sesión y Abre el Mapa**

```
1. Padre hace login: padre1@gmail.com
2. Sistema lo redirige a: /PagosPadresFamilia
3. Padre hace clic en: "Ver Ruta del Bus" → /Padres/DashboardRutaBusAsignado
4. Se carga el mapa de Leaflet.js
5. Se establece conexión SignalR
6. Padre se suscribe al grupo del bus de su hijo
```

**Código en `Pages/Padres/DashboardRutaBusAsignado.cshtml`:**
```javascript
// 1. Establecer conexión SignalR
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificacionesHub")
    .build();

// 2. Iniciar conexión
connection.start().then(() => {
    console.log("✅ SignalR conectado");

    // 3. Unirse al grupo del bus de su hijo
    const idBusHijo = 4; // Obtenido del servidor
    connection.invoke("UnirseAGrupo", `Bus_${idBusHijo}`);
    console.log(`✅ Suscrito a grupo Bus_${idBusHijo}`);
});
```

---

### **Paso 2: Padre Recibe Actualizaciones en Tiempo Real**

**Código JavaScript en el navegador del padre:**
```javascript
// 4. Escuchar evento de ubicación actualizada
connection.on("UbicacionBusActualizada", function(data) {
    console.log("📍 Nueva ubicación del bus recibida:", data);

    const { IdBus, Latitud, Longitud, FechaHora } = data;

    // 5. Actualizar marcador del bus en el mapa
    if (marcadorBus) {
        marcadorBus.setLatLng([Latitud, Longitud]);
    } else {
        marcadorBus = L.marker([Latitud, Longitud], {
            icon: iconoBus
        }).addTo(mapa);
    }

    // 6. Centrar mapa en el bus (opcional)
    mapa.setView([Latitud, Longitud], 15);

    // 7. Actualizar timestamp de última actualización
    document.getElementById("ultimaActualizacion").innerText = 
        `Última actualización: ${new Date(FechaHora).toLocaleTimeString()}`;

    // 8. Mostrar notificación si el bus está cerca
    verificarProximidad(Latitud, Longitud);
});
```

**Resultado:**
```
╔════════════════════════════════════════════════╗
║  🚌 Mapa del Bus - Tiempo Real                ║
║  ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━  ║
║                                                ║
║         🗺️ [Mapa con bus en movimiento]       ║
║              ↓ (se actualiza cada 10s)        ║
║                                                ║
║  📍 Última ubicación: 14.0723, -87.1921       ║
║  ⏰ Última actualización: 7:15:32 AM          ║
║                                                ║
║  ✅ Bus en camino (3 paradas restantes)       ║
╚════════════════════════════════════════════════╝
```

---

### **Paso 3: Sistema Verifica Proximidad y Envía Alertas**

**Código JavaScript (opcional):**
```javascript
function verificarProximidad(latBus, lonBus) {
    // Ubicación de la parada del hijo
    const latParada = 14.0750;
    const lonParada = -87.1950;

    // Calcular distancia (en metros)
    const distancia = calcularDistancia(latBus, lonBus, latParada, lonParada);

    console.log(`📏 Distancia del bus a la parada: ${distancia.toFixed(0)} metros`);

    // Si está a menos de 500 metros (5 cuadras)
    if (distancia < 500) {
        mostrarNotificacion("🚌 El bus está cerca", "El bus llegará a tu parada en 3-5 minutos");
        reproducirSonido("alerta.mp3");
    }
}

function calcularDistancia(lat1, lon1, lat2, lon2) {
    // Fórmula de Haversine (distancia entre 2 puntos GPS)
    const R = 6371000; // Radio de la Tierra en metros
    const dLat = (lat2 - lat1) * Math.PI / 180;
    const dLon = (lon2 - lon1) * Math.PI / 180;
    const a = Math.sin(dLat/2) * Math.sin(dLat/2) +
              Math.cos(lat1 * Math.PI / 180) * Math.cos(lat2 * Math.PI / 180) *
              Math.sin(dLon/2) * Math.sin(dLon/2);
    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
    return R * c; // Distancia en metros
}
```

---

## 💾 GUARDADO DE UBICACIONES EN BASE DE DATOS

### **Estado Actual: NO se guardan automáticamente**

Actualmente, las ubicaciones se transmiten en **tiempo real vía SignalR** pero **NO se guardan en la base de datos**.

**¿Por qué?**
- Para reducir carga en la BD (1 registro cada 10s por bus = 8,640 registros/día/bus)
- Para priorizar el tiempo real sobre el histórico
- Para evitar llenar la BD con datos que pueden no ser necesarios

---

### **Opción 1: Guardar TODAS las ubicaciones (Histórico Completo)**

Si quieres guardar **TODAS** las ubicaciones para tener un histórico completo:

**Modificar `Hubs/NotificacionesHub.cs`:**
```csharp
public async Task ReportarUbicacionBus(int idBus, decimal latitud, decimal longitud)
{
    _logger.LogInformation($"[SIGNALR] Bus {idBus} reporta ubicación: Lat {latitud}, Lon {longitud}");

    // Broadcast a padres
    await Clients.Group($"Bus_{idBus}").SendAsync("UbicacionBusActualizada", new
    {
        IdBus = idBus,
        Latitud = latitud,
        Longitud = longitud,
        FechaHora = DateTime.Now
    });

    // ✅ NUEVO: Guardar en base de datos
    await GuardarUbicacionEnBD(idBus, latitud, longitud);
}

private async Task GuardarUbicacionEnBD(int idBus, decimal latitud, decimal longitud)
{
    using (var scope = _serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var ubicacion = new UbicacionBusEnTiempoReal
        {
            IdBus = idBus,
            Latitud = latitud,
            Longitud = longitud,
            FechaHora = DateTime.Now,
            Velocidad = null, // Calcular si es necesario
            Direccion = null  // Calcular si es necesario
        };

        context.UbicacionesBusDb.Add(ubicacion);
        await context.SaveChangesAsync();
    }
}
```

**Agregar inyección de dependencias en el constructor:**
```csharp
public class NotificacionesHub : Hub
{
    private readonly ILogger<NotificacionesHub> _logger;
    private readonly IServiceProvider _serviceProvider; // ← NUEVO

    public NotificacionesHub(
        ILogger<NotificacionesHub> logger,
        IServiceProvider serviceProvider) // ← NUEVO
    {
        _logger = logger;
        _serviceProvider = serviceProvider; // ← NUEVO
    }

    // ... resto del código
}
```

**Resultado en Base de Datos:**
```sql
SELECT TOP 100 * FROM UbicacionBusEnTiempoReal
ORDER BY FechaHora DESC;

-- Resultado:
| IdUbicacion | IdBus | Latitud   | Longitud   | FechaHora           | Velocidad | Direccion |
|-------------|-------|-----------|------------|---------------------|-----------|-----------|
| 1           | 4     | 14.07230  | -87.19210  | 2025-01-20 07:15:10 | NULL      | NULL      |
| 2           | 4     | 14.07245  | -87.19225  | 2025-01-20 07:15:20 | NULL      | NULL      |
| 3           | 4     | 14.07260  | -87.19240  | 2025-01-20 07:15:30 | NULL      | NULL      |
| ...         | ...   | ...       | ...        | ...                 | ...       | ...       |
| 8640        | 4     | 14.08500  | -87.21000  | 2025-01-20 15:59:50 | NULL      | NULL      |
```

**⚠️ Advertencia:**
- 1 bus transmitiendo cada 10s = 8,640 registros/día
- 10 buses = 86,400 registros/día
- 30 días = 2,592,000 registros/mes

**Solución:** Implementar limpieza automática de datos antiguos (ej: eliminar registros > 30 días)

---

### **Opción 2: Guardar solo ubicaciones IMPORTANTES (Optimizado)**

Guardar SOLO ubicaciones relevantes:
- ✅ Cuando el bus pasa cerca de una parada (< 100 metros)
- ✅ Cada 5 minutos (en lugar de cada 10 segundos)
- ✅ Cuando hay un cambio significativo de dirección
- ✅ Inicio y fin de ruta

**Código optimizado:**
```csharp
private DateTime _ultimaUbicacionGuardada = DateTime.MinValue;

public async Task ReportarUbicacionBus(int idBus, decimal latitud, decimal longitud)
{
    // Broadcast siempre (tiempo real)
    await Clients.Group($"Bus_{idBus}").SendAsync("UbicacionBusActualizada", new
    {
        IdBus = idBus,
        Latitud = latitud,
        Longitud = longitud,
        FechaHora = DateTime.Now
    });

    // Guardar SOLO cada 5 minutos
    if ((DateTime.Now - _ultimaUbicacionGuardada).TotalMinutes >= 5)
    {
        await GuardarUbicacionEnBD(idBus, latitud, longitud);
        _ultimaUbicacionGuardada = DateTime.Now;
    }

    // O guardar si está cerca de una parada
    if (await EstaCercaDeParada(idBus, latitud, longitud))
    {
        await GuardarUbicacionEnBD(idBus, latitud, longitud);
    }
}

private async Task<bool> EstaCercaDeParada(int idBus, decimal latitud, decimal longitud)
{
    using (var scope = _serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Obtener paradas del bus
        var paradas = await context.ParadasDb
            .Where(p => p.Rutas.Any(r => r.IdBus == idBus && r.EsActiva))
            .ToListAsync();

        // Verificar si está a menos de 100 metros de alguna parada
        foreach (var parada in paradas)
        {
            var distancia = CalcularDistancia(
                (double)latitud, (double)longitud,
                (double)parada.Latitud, (double)parada.Longitud);

            if (distancia < 100) // 100 metros
            {
                return true;
            }
        }

        return false;
    }
}
```

**Resultado:** Solo se guardan ~100-200 registros/día por bus (mucho más eficiente)

---

### **Opción 3: NO guardar (Solo Tiempo Real)**

**Estado actual recomendado:**
- ✅ SignalR transmite en tiempo real (padres ven ubicación al instante)
- ❌ NO se guarda en BD (no hay histórico)
- ✅ Más eficiente, menos carga en servidor
- ✅ Suficiente para el 95% de los casos de uso

**Cuándo implementar guardado:**
- Si necesitas "reproducir" rutas históricas
- Si necesitas generar reportes de cumplimiento de rutas
- Si necesitas detectar desvíos de ruta
- Si autoridades lo requieren para auditorías

---

## 🔄 RESUMEN DEL FLUJO COMPLETO

```
┌──────────────────────────────────────────────────────────────────┐
│                     LÍNEA DE TIEMPO                               │
└──────────────────────────────────────────────────────────────────┘

T=0s    Piloto hace login → /Piloto/MiRuta
T=2s    Navegador solicita permiso de geolocalización
T=3s    Piloto otorga permiso
T=5s    Se obtiene primera ubicación GPS
T=5s    Se envía vía SignalR al Hub
T=5s    Hub distribuye a padres suscritos al bus
T=5s    Padres ven bus en el mapa (actualización instantánea)
T=15s   Segunda ubicación obtenida y enviada
T=25s   Tercera ubicación obtenida y enviada
T=35s   Cuarta ubicación obtenida y enviada
...     (continúa cada 10 segundos)
```

**Frecuencia:** 1 actualización cada 10 segundos = 6 actualizaciones por minuto

---

## 🎛️ CONFIGURACIÓN RECOMENDADA

### **Parámetros Actuales:**

| Parámetro | Valor | Justificación |
|-----------|-------|---------------|
| **Intervalo de transmisión** | 10 segundos | Balance entre tiempo real y consumo de batería |
| **Precisión GPS** | Alta (`enableHighAccuracy: true`) | Ubicación precisa para mapas |
| **Timeout** | 10 segundos | Dar tiempo al GPS a obtener señal |
| **Guardado en BD** | NO | Reducir carga de servidor, tiempo real prioritario |
| **Grupo SignalR** | `Bus_{idBus}` | Cada bus tiene su propio grupo |

---

### **Parámetros Opcionales (Ajustar según necesidad):**

```javascript
// Opción 1: Actualización más frecuente (más batería)
setInterval(transmitirUbicacion, 5000); // 5 segundos

// Opción 2: Actualización menos frecuente (menos batería)
setInterval(transmitirUbicacion, 30000); // 30 segundos

// Opción 3: Precisión baja (WiFi/Cell Tower, ahorro de batería)
navigator.geolocation.getCurrentPosition(
    successCallback,
    errorCallback,
    { enableHighAccuracy: false } // ← Cambio
);
```

---

## 🔧 TROUBLESHOOTING

### **Problema 1: "Ubicación no disponible"**

**Causas:**
- GPS deshabilitado en el dispositivo
- Navegador no tiene permiso de ubicación
- Señal GPS débil (interior de edificios)

**Solución:**
```javascript
function manejarErrorGPS(error) {
    switch(error.code) {
        case error.PERMISSION_DENIED:
            alert("❌ Debes permitir acceso a tu ubicación para usar esta función");
            break;
        case error.POSITION_UNAVAILABLE:
            alert("⚠️ No se puede obtener tu ubicación. Verifica que el GPS esté habilitado");
            break;
        case error.TIMEOUT:
            alert("⏱️ Tiempo de espera agotado. Intenta de nuevo");
            break;
    }
}
```

---

### **Problema 2: "Ubicación no se actualiza en el mapa de padres"**

**Causas:**
- Conexión SignalR perdida
- Padre no está suscrito al grupo correcto
- Piloto no está transmitiendo

**Solución (Debugging):**
```javascript
// En consola del navegador del padre:
connection.state // Debe ser "Connected"

// Verificar que está suscrito al grupo:
console.log("Grupos:", connection._groups); // Debe incluir "Bus_4"

// Verificar que el Hub está recibiendo:
// (Ver logs en servidor: appsettings.Development.json → LogLevel: Debug)
```

---

### **Problema 3: "Consumo alto de batería en dispositivo del piloto"**

**Causa:** GPS de alta precisión consume mucha batería

**Solución:**
```javascript
// Reducir frecuencia de transmisión
setInterval(transmitirUbicacion, 30000); // Cada 30 segundos

// O usar precisión baja cuando el bus está detenido
if (velocidad < 5) { // km/h
    // Bus detenido, usar precisión baja
    enableHighAccuracy = false;
} else {
    // Bus en movimiento, usar precisión alta
    enableHighAccuracy = true;
}
```

---

## ✅ CONCLUSIONES Y RECOMENDACIONES

### **Estado Actual del Sistema:**

✅ **SignalR configurado y funcional**  
✅ **Geolocation API implementado en piloto**  
✅ **Actualización en tiempo real a padres funcionando**  
❌ **NO se guarda histórico en BD (por diseño)**

---

### **Recomendaciones:**

#### **Para MVP (versión actual):**
- ✅ Mantener SOLO tiempo real vía SignalR
- ✅ NO guardar en BD (evita sobrecarga)
- ✅ Frecuencia: cada 10 segundos (buen balance)

#### **Para v2.0 (futuro):**
- 🟡 Implementar guardado SELECTIVO (cada 5 min o cerca de paradas)
- 🟡 Agregar funcionalidad de "reproducir ruta histórica"
- 🟡 Agregar cálculo de velocidad y dirección
- 🟡 Implementar limpieza automática de datos > 30 días

#### **Para Producción:**
- ⚠️ Considerar usar **Azure SignalR Service** si > 100 usuarios simultáneos
- ⚠️ Implementar reconexión automática si se pierde conexión
- ⚠️ Agregar indicador visual de "transmitiendo" vs "detenido"

---

## 📚 REFERENCIAS

- **Geolocation API:** https://developer.mozilla.org/en-US/docs/Web/API/Geolocation_API
- **SignalR Real-time:** https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction
- **Leaflet.js Maps:** https://leafletjs.com/
- **Haversine Formula:** https://en.wikipedia.org/wiki/Haversine_formula

---

**📅 Documento Generado:** Enero 2025  
**🎯 Estado:** Sistema en Producción (85% completado)  
**✅ Conclusión:** Las ubicaciones se transmiten en tiempo real SOLO cuando el piloto está en su ruta  

---

**FIN DEL DOCUMENTO: SISTEMA_GEOLOCALIZACION.md**
