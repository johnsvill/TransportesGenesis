# Manual — Simulación en vivo (BD compañero)

Demo en **`TransportesGenesis2`** con bus **BUS-001 (IdBus 4)**.

**URL base:** `https://localhost:7241`  
**Contraseña de todos:** `Admin123!`  
**BD:** `TransportesGenesis2` (connection string en `appsettings.json`)

---

## Quién usa qué (diferente a la máquina de David)

| Ventana | Usuario | Email / login | Página | Bus |
|---------|---------|---------------|--------|-----|
| 1 — Padre | padre1 | `padre1@gmail.com` | `/Padres/DashboardRutaBusAsignado` | Escucha **Bus 4** |
| 2 — Simulación | **piloto1** | `piloto1@transportesgenesis.com` | `/Piloto/MiRuta?turno=Mañana` | **BUS-001 (4)** |
| 3 — Admin | admin | `admin` o `admin@transportesgenesis.com` | `/Geolocalizacion/MapaEnTiempoReal` | Elegir **BUS-001 / Bus 4** |

> En su BD, **piloto1** está asignado a **BUS-001**. Por eso él simula con **piloto1**, no con monitor1.  
> David simula con **monitor1** porque en su BD monitor1 es quien está en el bus 4.

---

## Pasos (3 ventanas)

### 1) Arrancar la app

Connection string → `TransportesGenesis2`. Reiniciar app.

```powershell
dotnet run
```

---

### 2) Ventana PADRE (navegador normal)

1. Login: `padre1@gmail.com` / `Admin123!`
2. Ir a: **`/Padres/DashboardRutaBusAsignado`**
3. F12 → consola: `Unido a Bus_4`

Si el mapa está vacío, los hijos de padre1 deben estar en **bus 4** (mismo bus que simula piloto1).

---

### 3) Ventana SIMULACIÓN (incógnito)

1. Login: **`piloto1@transportesgenesis.com`** / `Admin123!`
2. Ir a: **`/Piloto/MiRuta?turno=Mañana`**
3. Confirmar: **Bus #4 — BUS-001**
4. Clic **`Simular Ruta`**

Transmite en grupo SignalR **`Bus_4`**.

---

### 4) Ventana ADMIN (incógnito 2)

1. Login: **`admin`** / `Admin123!`
2. Ir a: **`/Geolocalizacion/MapaEnTiempoReal`**
3. Selector **Bus** → **BUS-001** (Bus **4**)
4. Ver el bus moverse al simular en piloto1

---

## Orden al demo

1. padre1 → dashboard ruta  
2. piloto1 → Mi Ruta  
3. admin → mapa en vivo, BUS-001  
4. **Simular Ruta** en piloto1  

---

## Checklist si no funciona

| Revisar | Debe ser |
|---------|----------|
| BD en appsettings | `TransportesGenesis2` |
| piloto1 asignado | BUS-001, IdBus **4** |
| Usuario que simula | **piloto1** (en su caso) |
| Admin — bus | **BUS-001 / 4**, no Bus 1 |
| Ruta en Mi Ruta | Mapa con paradas antes de simular |
| Admin Calcular Rutas | **BUS-001 = Exitoso** (lo rojo en otros buses ignorar) |

---

## Preparación BD (si aún no tiene datos)

1. App arrancada una vez (usuarios Identity)  
2. `Scripts/seed_completo_demo_geolocalizacion.sql` con `USE TransportesGenesis2`  
3. Si falla `IdAlumno`: fix alumnos + re-ejecutar sección paradas  
4. piloto1 asignado a BUS-001  

Detalle completo: `Docs/DEMO_Companero_TransportesGenesis2.md`

---

## Resumen vs máquina de David

| | David | Compañero |
|---|--------|-----------|
| Quien simula | **monitor1** | **piloto1** |
| Bus demo | BUS-001 (4) | BUS-001 (4) |
| Padre | padre1@gmail.com | padre1@gmail.com |
| Admin mapa | BUS-001 | BUS-001 |

## Selenium / pruebas automatizadas

Si al pulsar **Simular Ruta** sale en consola **`Invalid LatLng object`**:

1. **Datos:** paradas en BD sin `Latitud`/`Longitud` (común si el seed de alumnos falló). Verificar:
   ```sql
   SELECT p.IdParada, p.Latitud, p.Longitud
   FROM genesis.Paradas p
   JOIN genesis.Rutas r ON r.IdRuta = p.IdRuta
   WHERE r.IdBus = 4 AND r.EsActiva = 1 AND (p.Latitud IS NULL OR p.Longitud IS NULL);
   ```
2. **URL de prueba:** usar turno Tarde si el navegador headless no da GPS:  
   `/Piloto/MiRuta?turno=Tarde`
3. **Mock GPS en Selenium** (Chrome), si prueban turno Mañana:
   ```java
   Map<String, Object> coords = new HashMap<>();
   coords.put("latitude", 14.6349);
   coords.put("longitude", -90.5069);
   coords.put("accuracy", 1);
   ((ChromeDriver) driver).executeCdpCommand("Emulation.setGeolocationOverride", coords);
   ```
4. Actualizar código: la página Piloto ya valida coordenadas antes de `L.polyline` y muestra alerta si no hay GPS válido en paradas.

La lógica es la misma: **los 3 roles deben mirar el mismo bus (4 / BUS-001)**. Solo cambia el usuario que inicia la simulación.

---
