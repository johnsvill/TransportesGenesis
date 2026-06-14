# Geolocalización en vivo — Sesión demo (14 jun 2026)

Resumen de cambios y comportamiento implementado en **Transportes Génesis** para la tubería **monitor/piloto → padre** y la demo solicitada por ingeniería.

---

## Objetivo general

Tubería en tiempo real:

```
Monitor/Piloto (simulación o GPS)
    → POST /api/ubicaciones
    → UbicacionBusService
    → SignalR (grupo Bus_{id})
    → Dashboard del padre (mapa en vivo)
```

El botón **Plan B** del padre sigue existiendo como respaldo local si no hay señal.

---

## Problemas encontrados y soluciones

### 1. El padre no veía moverse el mapa

| Causa | Detalle |
|-------|---------|
| **Bus incorrecto** | `piloto1` transmite **Bus #1**; `padre1` escucha **Bus #4** (hijos Diego/Sofía en BUS-001). |
| **Página equivocada** | `/Monitor/MiRuta` **no enviaba** ubicaciones; solo animaba el mapa localmente. |

**Solución:**

- Usar **`monitor1@transportesgenesis.com`** (Bus #4) en `/Monitor/MiRuta` o `/Piloto/MiRuta`.
- Se añadió en Monitor: `POST /api/ubicaciones`, SignalR, fallback al hub, transmisión cada 2 s durante simulación.
- `/Piloto/MiRuta` ahora acepta rol **Monitor** además de **Piloto**.
- Banner de advertencia con bus y grupo SignalR (`Bus_4`).

**Credenciales demo:**

| Rol | Usuario | Bus |
|-----|---------|-----|
| Padre | `padre1@gmail.com` | Escucha Bus #4 |
| Monitor | `monitor1@transportesgenesis.com` | Transmite Bus #4 |
| ❌ No usar | `piloto1@transportesgenesis.com` | Bus #1 (no coincide) |

Contraseña: `Admin123!` · URL: `https://localhost:7241`

---

### 2. Mapa del padre — ruta Plan B vs ruta en vivo

**Antes:** La línea naranja punteada (Plan B) se dibujaba siempre al cargar.

**Ahora:**

| Modo | Línea en mapa |
|------|----------------|
| **En vivo** (default) | Ruta completa del bus en **azul sólido** (misma lógica que monitor, sin números de todas las paradas) |
| **Plan B** | Línea **naranja punteada** solo al pulsar *Iniciar Simulación (Plan B)* |

**Marcadores del padre:** solo colegio 🏫 y paradas/casas de **sus hijos** (Diego, Sofía) con icono amarillo pulsante → verde ✓ al pasar el bus.

---

### 3. Color verde de la ruta (Mañana vs Tarde)

| Turno | Tramo que se pone verde |
|-------|-------------------------|
| **Mañana** | Desde el avance del bus **hasta el colegio** (parada final) |
| **Tarde** | Desde el **colegio** (parada 1) **hasta la última parada de los hijos de este padre** |

El resto de la ruta del bus permanece azul. Leyenda en el encabezado del mapa.

**Selección de turno:**

- Automático: antes de 12:00 → Mañana; después → Tarde.
- Demo manual: `?turno=Mañana` o `?turno=Tarde` en padre y monitor.

---

### 4. Rutas en BD — Mañana vs Tarde (Bus #4, 15/06/2026)

Consultado en SQL:

| Turno | IdRuta | Paradas | Colegio |
|-------|--------|---------|---------|
| **Mañana** | 8 | 18 | Parada **18** (destino final) |
| **Tarde** | 10 | 18 | Parada **1** (punto de salida) |

Diego Martínez: parada **#15** (Mañana) / **#16** (Tarde).

En **Tarde** el bus **no debe terminar en el colegio**; sale de ahí y deja alumnos en casa.

---

### 5. Simulador aleatorio en Monitor (pedido ingeniería)

En `/Monitor/MiRuta` → panel **Demo aleatoria**:

| Control | Descripción |
|---------|-------------|
| Checkbox *Generar ruta aleatoria al simular* | Activo por defecto; mezcla antes de cada simulación |
| **Orden aleatorio** | Baraja paradas; respeta Mañana (→ colegio) / Tarde (colegio →) |
| **Con ausencias** | Subconjunto aleatorio de alumnos (simula no asistencia) |
| **Casa de amigo** | Un alumno con GPS desplazado |
| **Mixto** | Ausencias + casa amigo + orden aleatorio |
| **Generar** | Previsualiza ruta **morada** en mapa |
| **↺** | Restaura ruta de Admin/Calcular Rutas |

Sigue transmitiendo en vivo al padre durante la simulación.

---

## Archivos modificados (principales)

| Archivo | Cambio |
|---------|--------|
| `Pages/Monitor/MiRuta.cshtml` | Transmisión en vivo, datos desde BD, simulador aleatorio, completar paradas vía API |
| `Pages/Monitor/MiRuta.cshtml.cs` | `IPilotoService`, IdBus desde BD, parámetro `?turno=` |
| `Pages/Piloto/MiRuta.cshtml` | Banner bus, fix `fetch` API, rol Monitor |
| `Pages/Piloto/MiRuta.cshtml.cs` | Monitor autorizado, `?turno=`, placa bus |
| `Pages/Padres/DashboardRutaBusAsignado.cshtml` | Ruta vivo vs Plan B, colores Mañana/Tarde, marcadores hijos |
| `Pages/Padres/DashboardRutaBusAsignado.cshtml.cs` | `RutaCompletaJson`, `ParadasHijosJson`, `?turno=`, descripción turno |
| `Scripts/Seed_Escenarios_Completos.sql` | Notas credenciales demo Bus #4 |

---

## Cómo probar el flujo completo

1. **Admin** (si hace falta): Calcular rutas para Bus 4, fecha hábil, Mañana y Tarde.
2. **Padre:** login `padre1@gmail.com` → `/Padres/DashboardRutaBusAsignado?turno=Mañana`
3. **Monitor:** login `monitor1@transportesgenesis.com` → `/Monitor/MiRuta?turno=Mañana`
4. Verificar banner **Bus #4 — BUS-001** y grupo `Bus_4`.
5. Elegir escenario demo → **Generar** (opcional) → **Simular Ruta**.
6. En consola del padre: `📍 [SignalR] Ubicación actualizada` y mapa con bus verde moviéndose.
7. Repetir con `?turno=Tarde` para escenario de regreso.

---

## Preguntas frecuentes (comportamiento actual)

### ¿Las rutas en Admin son aleatorias?

**No.** Se calculan con **vecino más cercano** según GPS de alumnos con asistencia confirmada. Mañana termina en colegio; Tarde empieza en colegio.

### ¿Sofía en casa de amigo y Diego en su casa?

- **Parcialmente soportado:** asistencia (`AsisteMañana`/`AsisteTarde`), bus temporal (`IdBusTemporalMañana/Tarde`), traslados aprobados.
- La ruta usa GPS del alumno en BD; cambiar dirección = actualizar coordenadas del alumno.
- El escenario **casa de amigo** del simulador desplaza un punto al azar **solo para la demo en vivo** (no persiste en BD).

### ¿Hijos en buses distintos — el padre ve 2 rutas?

**Hoy no.** El dashboard muestra **un bus** (el del primer hijo con bus asignado). SignalR sí se une por `Alumno_{id}` para alertas. **Pendiente UI:** pestañas o selector por hijo/bus.

---

## Limitaciones conocidas / pendientes

- [ ] Padre con hijos en **buses diferentes**: una sola vista de mapa.
- [ ] Pantalla padre para “recoger en otra dirección hoy” sin editar alumno en admin.
- [ ] `/Piloto/MiRuta` — mismo simulador aleatorio que Monitor (solo implementado en Monitor).
- [ ] Mensajes *Tracking Prevention* en Edge: ruido del navegador (Leaflet/OSRM); no bloquean SignalR.

---

## Referencia SignalR

| Evento | Origen |
|--------|--------|
| `UbicacionBusActualizada` | POST `/api/ubicaciones` |
| `ParadaCompletada` | PUT `/api/rutas/{id}/parada/{id}/completar` |
| `AlertaRecibida` / `AlertaPersonal` | Proximidad / llegada colegio |

Grupos padre: `Bus_{idBus}`, `Alumno_{idAlumno}`.

---

*Documento generado al cierre de la sesión de geolocalización — Transportes Génesis.*
