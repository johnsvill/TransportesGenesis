# Demo geolocalización — Mi BD local (`TransportesGenesis`)

Guía rápida para correr la demo en **tu máquina** (David).

> **Simulación en vivo (3 ventanas):** ver **`SIMULACION_MiMaquina.md`** — monitor1 + padre1 + admin BUS-001.

---

## Configuración

| Item | Valor |
|------|--------|
| Base de datos | `TransportesGenesis` |
| Servidor | `(local)` |
| Connection string | `appsettings.Development.json` → `TransportesGenesisConnection` |
| URL app | `https://localhost:7241` |
| Entorno | `Development` |

```json
"TransportesGenesisConnection": "Server=(local);Database=TransportesGenesis;Trusted_Connection=True;TrustServerCertificate=True"
```

---

## Asignaciones actuales (referencia)

| Usuario | Email | Bus | Placa | Uso en demo |
|---------|-------|-----|-------|-------------|
| **monitor1** | monitor1@transportesgenesis.com | **4** | **BUS-001** | **Simulación en vivo** (Mi Ruta) |
| piloto1 | piloto1@transportesgenesis.com | 1 | P-001GT | Otro bus; no usar para demo padre↔bus |
| padre1 | padre1@gmail.com | — | — | Ve el bus de sus hijos (BUS-001) |
| admin | admin@transportesgenesis.com | — | — | Calcular rutas, mapa admin |

**Contraseña demo:** `Admin123!`

---

## Preparación (solo si la BD está vacía o quieres resetear demo)

### 1. Migraciones

```powershell
dotnet ef database update
```

### 2. Arrancar la app una vez

Crea usuarios Identity (`admin`, `piloto2`, `padre1`, etc.) vía `Startup.cs`.

```powershell
dotnet run
```

Cierra y vuelve a abrir cuando termines los scripts SQL.

### 3. Seed principal — BUS-001

En SSMS / Azure Data Studio, ejecutar completo (F5):

```
Scripts/seed_completo_demo_geolocalizacion.sql
```

(Línea 17: `USE TransportesGenesis;`)

**Incluye:** colegio, BUS-001, padres, alumnos, rutas Mañana/Tarde, paradas, monitor1→BUS-001, GPS inicial.

### 4. (Opcional) Asistencias para Admin/CalcularRutas

```
Scripts/Seed_Escenarios_Completos.sql
```

Inserta asistencias confirmadas 20 días hábiles. **Requiere alumnos con bus + GPS.**

---

## Demo en vivo (padre + monitor + admin)

### Ventana 1 — Padre

1. Login: `padre1@gmail.com` / `Admin123!`
2. Ir a: `/Padres/DashboardRutaBusAsignado`
3. Consola (F12): debe aparecer `Unido a Bus_4`

### Ventana 2 — Simulación (incógnito)

1. Login: **`monitor1@transportesgenesis.com`** / `Admin123!`
2. Ir a: `/Piloto/MiRuta?turno=Mañana`
3. Iniciar **Simular Ruta**
4. SignalR transmite en grupo `Bus_4`

> No uses **piloto1** para esta demo: está en **Bus 1 (P-001GT)**. El padre escucha **Bus 4 (BUS-001)**.

### Ventana 3 — Admin (opcional)

1. Login: `admin` / `Admin123!`
2. Mapa / geolocalización: seleccionar **Bus 4 (BUS-001)**, no Bus 1
3. Calcular rutas: `/Admin/CalcularRutas` — fecha día hábil, turno Mañana y luego Tarde

---

## Verificación SQL

```sql
USE TransportesGenesis;

-- Asignaciones
SELECT u.UserName, u.Email, apb.IdBus, b.Placa
FROM genesis.AsignacionPilotoBus apb
JOIN dbo.AspNetUsers u ON u.Id = apb.IdUsuarioPiloto
JOIN genesis.Buses b ON b.IdBus = apb.IdBus
WHERE apb.EsActual = 1;

-- Alumnos en BUS-001
SELECT COUNT(*) AS AlumnosBus4
FROM genesis.Alumnos WHERE IdBusAsignado = 4 AND Activo = 1;

-- Rutas activas BUS-001
SELECT r.TipoRuta, COUNT(*) AS Rutas, SUM(p.cnt) AS Paradas
FROM genesis.Rutas r
LEFT JOIN (
    SELECT IdRuta, COUNT(*) AS cnt FROM genesis.Paradas GROUP BY IdRuta
) p ON p.IdRuta = r.IdRuta
WHERE r.IdBus = 4 AND r.EsActiva = 1
GROUP BY r.TipoRuta;
```

---

## Fin de semana

- Hoy sábado/domingo: Mi Ruta puede mostrar mensaje de fin de semana si no hay ruta activa.
- Probar igual con: `/Piloto/MiRuta?turno=Mañana`
- Admin no calcula rutas para sábado/domingo.

---

## Scripts útiles

| Script | Para qué |
|--------|----------|
| `Scripts/VERIFICAR_Demo_Padre_Piloto.sql` | Credenciales y vínculo padre–bus |
| `Scripts/seed_completo_demo_geolocalizacion.sql` | Demo BUS-001 completa |
| `Scripts/Seed_Escenarios_Completos.sql` | Asistencias + recalcular en Admin |
| `GUIA_DEMO_Padre_Piloto_Tiempo_Real.md` | Guía extendida tiempo real |
| `GUIA_GEOLOCALIZACION_COMPLETA.md` | Geolocalización detallada |

---

## Problemas frecuentes

| Síntoma | Causa | Solución |
|---------|--------|----------|
| Padre no ve bus moverse | Simulaste con piloto1 (bus 1) | Usar **monitor1** (bus 4) |
| Admin bus 1 sin movimiento | Mismo motivo | Elegir **Bus 4** |
| Calcular rutas: filas rojas | Buses sin alumnos | Normal; importa BUS-001 en verde |
| `IdAlumno` NULL al seed | BD sin IDENTITY en Alumnos | Ver guía del compañero (fix IdAlumno) |
