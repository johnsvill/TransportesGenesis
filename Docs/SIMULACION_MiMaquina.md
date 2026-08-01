# Manual — Simulación en vivo (mi máquina)

Demo **padre ↔ monitor ↔ admin** con bus **BUS-001 (IdBus 4)**.

**URL base:** `https://localhost:7241`  
**Contraseña de todos:** `Admin123!`  
**BD:** `TransportesGenesis` (`appsettings.Development.json`)

---

## Quién usa qué

| Ventana | Usuario | Email / login | Página | Bus |
|---------|---------|---------------|--------|-----|
| 1 — Padre | padre1 | `padre1@gmail.com` | `/Padres/DashboardRutaBusAsignado` | Escucha **Bus 4** |
| 2 — Simulación | **monitor1** | `monitor1@transportesgenesis.com` | `/Piloto/MiRuta?turno=Mañana` | **BUS-001 (4)** |
| 3 — Admin | admin | `admin` o `admin@transportesgenesis.com` | `/Geolocalizacion/MapaEnTiempoReal` | Elegir **BUS-001 / Bus 4** |

> **Importante:** usa **monitor1**, no piloto1.  
> En tu BD, **piloto1** está en **Bus 1 (P-001GT)**. La simulación manda GPS al grupo `Bus_4`. Si simulas con piloto1, padre1 y admin no verán movimiento en BUS-001.

---

## Pasos (3 ventanas)

### 1) Arrancar la app

```powershell
dotnet run
```

Abrir `https://localhost:7241`.

---

### 2) Ventana PADRE (navegador normal)

1. Login: `padre1@gmail.com` / `Admin123!`
2. Ir a: **`/Padres/DashboardRutaBusAsignado`**
3. Debe verse el mapa de la ruta del hijo.
4. F12 → consola: buscar `Unido a Bus_4` o `Bus_4`.

---

### 3) Ventana SIMULACIÓN (incógnito: Ctrl+Shift+N)

1. Login: **`monitor1@transportesgenesis.com`** / `Admin123!`
2. Ir a: **`/Piloto/MiRuta?turno=Mañana`**
3. Confirmar arriba: **Bus #4 — BUS-001**
4. Clic en **`Simular Ruta`** (panel “Control de Simulación de Ruta”)
5. El bus naranja se mueve por las paradas; suena alerta en cada parada.

La transmisión va al grupo SignalR **`Bus_4`**.

---

### 4) Ventana ADMIN (otro incógnito o otro navegador)

1. Login: **`admin`** / `Admin123!`
2. Menú Admin → **Mapa en vivo** o ir a: **`/Geolocalizacion/MapaEnTiempoReal`**
3. En el selector **Bus**, elegir **BUS-001** (Bus **4**), no P-001GT ni Bus 1.
4. Turno/ruta: **Mañana** si aplica.
5. Debe verse el mismo bus moverse que en la ventana del monitor.

---

## Orden recomendado al demo

1. Abrir **padre1** y esperar mapa cargado.  
2. Abrir **monitor1** en Mi Ruta.  
3. Abrir **admin** en mapa en vivo con BUS-001.  
4. Pulsa **Simular Ruta** en monitor1.  
5. Mirar las 3 pantallas a la vez.

---

## Checklist rápido si no se mueve el mapa

| Revisar | Debe ser |
|---------|----------|
| Usuario que simula | **monitor1** (no piloto1) |
| Bus en Mi Ruta | **#4 — BUS-001** |
| Admin — bus seleccionado | **BUS-001 / 4** |
| Padre — consola SignalR | Conectado a **Bus_4** |
| Hay ruta activa | Paradas visibles en Mi Ruta antes de simular |

---

## Si falta ruta en Mi Ruta

Ejecutar antes (una vez): `Scripts/seed_completo_demo_geolocalizacion.sql`  
Detalle de BD: ver `Docs/DEMO_MiBD_TransportesGenesis.md`.
