# Guía completa — Geolocalización en TransportesGenesis

Esta guía explica cómo hacer funcionar la geolocalización **en producción real** y en **modo simulación/demo**, paso a paso.

---

## 1. Resumen: dos modos que comparten el mismo canal

| Modo | Quién lo activa | Qué transmite | ¿Llega al padre/admin? |
|------|-----------------|---------------|-------------------------|
| **Ruta en vivo (GPS real)** | Monitor/Piloto → botón verde **Iniciar Ruta en Vivo** | GPS del dispositivo cada ~5 s | ✅ Sí (API + SignalR + BD) |
| **Simulación de ruta** | Monitor/Piloto → **Simular Ruta** | Posición animada del bus sobre OSRM | ✅ Sí (mismo canal) |
| **Simulación local padre** | Padre → botón simulación en dashboard | Solo en el navegador del padre | ❌ No (Plan B offline) |
| **Simulación admin mapa** | Admin → toggle en mapa | Animación local en admin | ❌ No (solo visual admin) |

**Canal oficial (vivo y simulación sincronizada):**

```
Monitor/Piloto → POST /api/ubicaciones → UbicacionBusService
  → Guarda en genesis.UbicacionBusEnTiempoReal
  → SignalR: Bus_{id}, Alumno_{id}, Administradores
Padre/Admin → reciben UbicacionBusActualizada
```

---

## 2. Requisitos previos

### 2.1 Software

- .NET 8 SDK
- SQL Server (local o remoto)
- Navegador Chrome/Edge con **HTTPS** (`https://localhost:7241`)
- Permiso de **ubicación GPS** en el navegador

### 2.2 Base de datos

1. Configura la cadena de conexión en `appsettings.json`:

```json
"ConnectionStrings": {
  "TransportesGenesisConnection": "Server=TU_SERVIDOR;Database=TransportesGenesis;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

2. Aplica migraciones:

```powershell
cd C:\Proyectos\TransportesGenesis
dotnet ef database update
```

3. **Arranca la app una vez** para que `Startup.cs` cree usuarios Identity:

| Usuario | Email | Rol | Contraseña |
|---------|-------|-----|------------|
| admin | admin@transportesgenesis.com | Administrador | Admin123! |
| padre1 | padre1@gmail.com | PadreDeFamilia | Admin123! |
| monitor2 | monitor2@transportesgenesis.com | Monitor | Admin123! |
| piloto2 | piloto2@transportesgenesis.com | Piloto | Admin123! |

4. Ejecuta el script de datos demo (ver sección 8):

```sql
-- En SSMS o Azure Data Studio
Scripts/seed_completo_demo_geolocalizacion.sql
```

### 2.3 Regla crítica: mismo bus

El padre escucha el grupo `Bus_{IdBus}` de **sus hijos**. El monitor/piloto debe estar asignado al **mismo IdBus**.

- Demo recomendada: **BUS-001** (Diego y Sofía Martínez → padre1@gmail.com)
- ❌ No uses `piloto1` en Bus #1 si el padre escucha Bus #4 (BUS-001)

Verificar en Admin → **Gestionar Asignaciones** o con:

```sql
-- Scripts/VERIFICAR_Demo_Padre_Piloto.sql
```

---

## 3. Configuración inicial (Admin)

### Paso 1 — Colegio

1. Login: `admin` / `Admin123!`
2. Ir a configuración del colegio (coordenadas Guatemala demo):
   - Lat: `14.6350`
   - Lng: `-90.5125`
   - Nombre: `Colegio Genesis`

> El script SQL también inserta esto en `genesis.ConfiguracionSistema`.

### Paso 2 — Bus y rutas

El script demo crea:

- Bus **BUS-001**
- Ruta **Mañana** y **Tarde** con 10 alumnos + parada colegio
- Coordenadas en Zona 1, 2, 4, 9, 10 y 11 (Guatemala)

### Paso 3 — Asignaciones

1. Admin → **Gestionar Asignaciones**
2. Asignar al **BUS-001**:
   - Un **Monitor** (ej. `monitor2@transportesgenesis.com`)
   - Un **Piloto** (ej. `piloto2@transportesgenesis.com`)

### Paso 4 — Vincular padre con usuario

El script SQL vincula **Carlos Martínez López** (padre de Diego/Sofía) con `padre1@gmail.com`.

Si no corre el script, hacer manualmente:

```sql
UPDATE genesis.Padres
SET UsuarioId = (SELECT Id FROM AspNetUsers WHERE Email = 'padre1@gmail.com')
WHERE Nombre = 'Carlos' AND Apellido = 'Martínez López';
```

---

## 4. Prueba end-to-end — Simulación (recomendada para demo)

### Ventana 1 — Monitor (transmisor)

1. Login: `monitor2@transportesgenesis.com` / `Admin123!`
2. Ir a: `/Monitor/MiRuta?turno=Mañana` o `?turno=Tarde`
3. Permitir **GPS** cuando el navegador lo pida
4. Verificar:
   - **Mañana:** la ruta inicia desde tu ubicación (predio/hogar)
   - **Tarde:** la ruta inicia desde el colegio
5. Clic en **Simular Ruta**
6. El bus naranja se mueve; cada ~2 s se envía ubicación al servidor

### Ventana 2 — Padre (receptor)

1. Login: `padre1@gmail.com` / `Admin123!`
2. Ir a: `/Padres/DashboardRutaBusAsignado?turno=Mañana`
3. Debe aparecer:
   - Marcador verde 🚌 moviéndose
   - Badge "Conectado" (SignalR)
   - Ruta OSRM en el mapa
4. Al abrir la página, si el bus ya transmitió antes, carga la **última ubicación** desde `/api/ubicaciones/{idBus}/ultima`

### Ventana 3 — Admin (opcional)

1. Login: `admin` / `Admin123!`
2. Ir a: `/Geolocalizacion/MapaEnTiempoReal`
3. Ver el bus en movimiento en el mapa general
4. Clic en un bus → vista enfocada con ruta real

### Pausar en parada (niño no llegó)

Con la ruta activa (simulación o vivo):

1. Panel **Control de Pausa de Ruta**
2. Clic en **Niño no está en parada**
3. El bus se detiene (icono rojo ⏸)
4. Padres reciben `RutaPausada` por SignalR
5. **Reanudar Ruta** para continuar

---

## 5. Prueba end-to-end — GPS real (ruta en vivo)

Para probar como si fuera el bus circulando de verdad:

1. Monitor/Piloto en un **celular o laptop con GPS**
2. Login y `/Monitor/MiRuta` o `/Piloto/MiRuta`
3. Clic en **Iniciar Ruta en Vivo (GPS real)** (botón verde)
4. Camina o conduce: el marcador verde 🚌 sigue tu posición
5. Cada ~5 s se reporta a `/api/ubicaciones`
6. Padre y admin ven el movimiento igual que en simulación

**Notas:**

- No puedes tener simulación y ruta en vivo al mismo tiempo
- La pausa funciona en ambos modos
- Si pierdes red, `ubicacion-offline-queue.js` encola puntos y sincroniza al reconectar

---

## 6. Turno Mañana vs Tarde — inicio de parada

| Turno | Punto de inicio de la ruta OSRM |
|-------|----------------------------------|
| **Mañana** | GPS actual (predio, garaje o ubicación del monitor/piloto) → casas → colegio |
| **Tarde** | Colegio → casas |

Implementado en `aplicarParadaInicioPorTurno()` en:

- `Pages/Monitor/MiRuta.cshtml`
- `Pages/Piloto/MiRuta.cshtml`

---

## 7. Solución de problemas

### El padre no ve movimiento

| Causa | Solución |
|-------|----------|
| Bus distinto | Verificar `IdBusAsignado` de hijos = bus del monitor |
| No se inició transmisión | Monitor debe pulsar **Simular Ruta** o **Ruta en Vivo** |
| SignalR desconectado | Recargar página; revisar consola F12 |
| GPS denegado | Permitir ubicación en el candado 🔒 de la URL |
| Padre sin hijos vinculados | Ejecutar script SQL de vinculación |

### Admin muestra "Sin señal"

- Última ubicación > 10 min → estado **SinSeñal** (normal si nadie transmite)
- Inicia simulación o ruta en vivo desde monitor

### API 401 al reportar ubicación

- Solo roles **Monitor** y **Piloto** pueden POST `/api/ubicaciones`
- Verificar sesión activa

### Simulación local del padre vs en vivo

- El botón simulación del **padre** es Plan B (solo su navegador)
- Para demo con compañero, usa siempre transmisión desde **Monitor/Piloto**

### Consola útil (F12)

```
✅ [Monitor] Unido a Bus_4
✅ [PadreDashboard] Unido a Bus_4
🔵 [PADRE] UbicacionBusActualizada recibida
[Monitor] Mañana: inicio desde GPS ...
[Monitor] Tarde: inicio desde colegio ...
```

---

## 8. Script SQL de datos demo

Archivo: **`Scripts/seed_completo_demo_geolocalizacion.sql`**

Incluye:

- Bus BUS-001, 8 padres, 10 alumnos
- Rutas Mañana y Tarde con paradas
- Vinculación padre1 ↔ Carlos Martínez
- Asignación monitor2 → BUS-001
- Configuración colegio (Guatemala)
- Ubicación inicial del bus (mapa admin al cargar)
- Consultas de verificación al final

**Orden de ejecución:**

1. `dotnet ef database update`
2. Arrancar app una vez (usuarios Identity)
3. `Scripts/seed_completo_demo_geolocalizacion.sql`
4. Admin → Gestionar Asignaciones (completar piloto si falta)
5. Probar flujo sección 4

---

## 9. Archivos clave del código

| Área | Archivo |
|------|---------|
| API ubicaciones | `Controllers/Api/UbicacionesController.cs` |
| Lógica + alertas | `Services/Implementations/UbicacionBusService.cs` |
| SignalR | `Hubs/NotificacionesHub.cs` |
| Cliente SignalR | `wwwroot/js/signalr-client.js` |
| Cola offline | `wwwroot/js/ubicacion-offline-queue.js` |
| Monitor | `Pages/Monitor/MiRuta.cshtml` |
| Piloto | `Pages/Piloto/MiRuta.cshtml` |
| Padre | `Pages/Padres/DashboardRutaBusAsignado.cshtml` |
| Admin mapa | `Pages/Geolocalizacion/MapaEnTiempoReal.cshtml` |
| Asignaciones | `Pages/Admin/GestionarAsignaciones.cshtml` |

---

## 10. Checklist rápido antes de la demo

- [ ] SQL demo ejecutado sin errores
- [ ] `padre1@gmail.com` vinculado a padre con Diego/Sofía
- [ ] Monitor asignado a **BUS-001** (mismo bus que hijos)
- [ ] Rutas Mañana/Tarde activas con paradas
- [ ] App en `https://localhost:7241`
- [ ] GPS permitido en monitor y padre
- [ ] Monitor: **Simular Ruta** o **Ruta en Vivo** iniciado
- [ ] Padre: dashboard abierto, marcador verde visible
- [ ] (Opcional) Admin: mapa en tiempo real abierto

---

*Última actualización: julio 2026 — incluye ruta en vivo GPS, inicio por turno y carga inicial de ubicación en dashboard padre.*
