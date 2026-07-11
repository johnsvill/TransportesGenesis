# Seed demo Azure — datos para tu amigo

Script: [`Scripts/SEED_DEMO_AZURE_DatosAleatorios.sql`](SEED_DEMO_AZURE_DatosAleatorios.sql)

## Qué inserta

| Dato | Detalle |
|------|---------|
| Colegio | Nombre + coords Guatemala (parada fija) |
| 3 buses | `DEMO-001`, `DEMO-002`, `DEMO-003` |
| Padres | Vincula `padre1` / `padre2` de Identity si existen |
| Alumnos | 5 hijos con GPS (apellido `* Demo`) |
| Asignaciones | piloto2 → DEMO-001, monitor2 → DEMO-002 |
| Asistencias | 15 días hábiles confirmados |
| Ubicaciones | Puntos recientes para el mapa |

Re-ejecutable (no duplica por placa / nombre demo).

## Pasos para tu amigo (Azure)

1. Abrir **Azure Data Studio** o SSMS → BD `TransportesGenesis_db` (o el nombre real).
2. Si el script tiene `USE`, ajustar el nombre de la BD o ejecutarlo **ya conectado** a esa BD.
3. **Importante:** la app web debe haber arrancado al menos una vez para crear usuarios Identity (`admin`, `padre1`, `piloto2`, …).
4. Ejecutar el script completo.
5. En la app: **Admin → Calcular Rutas** (Mañana y Tarde).
6. Probar:
   - Admin: `/Geolocalizacion/BusesIndex` (debe listar DEMO-*)
   - Padre: `padre1@gmail.com` / `Admin123!` → Mi Ruta
   - Piloto: `piloto2@transportesgenesis.com` / `Admin123!` → Mi Ruta

## Bug “crear bus → dashboard”

En Azure antiguo, el botón **Crear Primer Bus** apuntaba a `/Geolocalizacion/BusCreate`, página que **no estaba publicada** (404) y terminabas en el panel admin.

En esta rama ya está corregido:

- Formulario de creación **dentro** de `/Geolocalizacion/BusesIndex` (también con lista vacía)
- Rutas explícitas: `BusCreate`, `BusEdit`, `BusDetails`, `BusesIndex`
- Tras crear, redirige a `/Geolocalizacion/BusesIndex` (no al dashboard)

Hay que **republicar** esta rama para que el fix llegue a Azure. Mientras tanto, el seed SQL permite seguir la demo sin depender de crear el primer bus por UI.
