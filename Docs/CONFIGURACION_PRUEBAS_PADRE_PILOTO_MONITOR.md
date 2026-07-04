# Configuración de Padre, Piloto y Monitor para pruebas

Guía práctica para preparar el entorno de **Transportes Génesis** y ejecutar demos (mapa en vivo, rutas, asistencia, pagos).

---

## Requisitos previos

| Elemento | Valor |
|----------|--------|
| URL local | `https://localhost:7241` |
| Login | `/Auth/Login` |
| Base de datos | `TransportesGenesis` en `(local)` |
| Admin por defecto | `admin` / `admin@transportesgenesis.com` |
| Contraseña admin | `Admin123!` |

Al **primer arranque** de la aplicación, `Startup.cs` crea usuarios de prueba con contraseña **`Admin123!`** (mínimo 6 caracteres, al menos 1 dígito).

---

## Escenario recomendado para demo en vivo

Para que **padre**, **monitor/piloto** y **admin** vean el **mismo bus** en el mapa:

| Rol | Usuario | Contraseña | Bus demo |
|-----|---------|------------|----------|
| **Padre** | `padre1@gmail.com` | `Admin123!` | Escucha el bus de sus hijos (**BUS-001**, IdBus **4**) |
| **Piloto o Monitor** | `piloto1@…` o `monitor1@…` | `Admin123!` | Deben transmitir el **mismo IdBus** que los hijos del padre |

> **Sobre `piloto1`:** el piloto **sí funciona** en el sistema. En muchos entornos de prueba viene asignado al **Bus #1** (P-001GT), mientras los hijos de `padre1` están en **BUS-001 (IdBus 4)**. SignalR transmite al grupo `Bus_{idBus}`; si el piloto manda `Bus_1` y el padre escucha `Bus_4`, **no se verá movimiento** — no es un bug del rol Piloto, es desalineación de datos.
>
> **Para usar `piloto1` con `padre1`:** Admin → **Gestionar Asignaciones** → finalizar asignación previa del bus → asignar **`piloto1` → BUS-001**. Alternativa: usar `monitor1` si ya está en ese bus.

---

## Scripts SQL útiles (opcional)

Ejecutar en SSMS sobre `TransportesGenesis` **antes** de la demo si faltan datos:

| Script | Propósito |
|--------|-----------|
| `Scripts/Seed_Escenarios_Completos.sql` | Asistencias, config colegio, desactiva rutas viejas |
| `Scripts/EJECUTAR_SIMPLE_DashboardMonitor.sql` | Bus BUS-001, padres, alumnos, rutas base |
| `Scripts/VERIFICAR_Demo_Padre_Piloto.sql` | Verifica cadena padre → hijo → bus → piloto |
| `Scripts/IGUALAR_Contraseña_Padres.sql` | Iguala contraseña de padres a `Admin123!` |

**Después del seed**, recalcular rutas desde la app:

1. Admin → **Calcular Rutas Escolares** (`/Admin/CalcularRutas`)
2. Elegir fecha (día hábil) y turno **Mañana**, luego **Tarde**

---

## Parte 1 — Configuración desde Admin

### 1.1 Crear usuarios (si no existen)

**Ruta:** `/Admin/Usuarios`

| Campo | Padre | Piloto | Monitor |
|-------|-------|--------|---------|
| Rol | `PadreDeFamilia` | `Piloto` | `Monitor` |
| Contraseña inicial | mín. 6 chars + 1 dígito | igual | igual |
| `IsFirstLogin` | `true` → pedirá cambio de clave al entrar | | |

Usuarios ya creados por seed (contraseña `Admin123!`, **sin** cambio obligatorio de clave):

- Padres: `padre1` … `padre5` (`padre1@gmail.com`, etc.)
- Pilotos: `piloto2` … `piloto5`
- Monitores: `monitor2` … `monitor5`

### 1.2 Buses

**Ruta:** `/Geolocalizacion/BusesIndex`

- Verificar que exista **BUS-001** (capacidad, estado activo).
- Anotar el **IdBus** (suele ser `4` en entornos de demo).

### 1.3 Alumnos y vínculo con el padre

**Ruta:** `/Admin/GestionarAlumnos`

Cada padre necesita:

1. Un registro en **`genesis.Padres`** con `UsuarioId` = Id del usuario Identity.
2. Al menos un **alumno** con:
   - `IdPadre` correcto
   - `IdBusAsignado` = bus de la demo
   - `Latitud` / `Longitud` (o el padre los guardará en el primer login)

> Si el padre entra y no ve hijos, casi siempre falta el enlace `Padres.UsuarioId` ↔ `AspNetUsers.Id`.

### 1.4 Paradas y rutas

**Ruta:** `/Admin/GestionarParadas`

- Filtrar por bus **BUS-001** y ruta activa.
- Cada parada debe tener coordenadas (excepto colegio).

**Ruta:** `/Admin/CalcularRutas`

- Generar rutas **Mañana** y **Tarde** para la fecha de la demo.

### 1.5 Asignar piloto o monitor al bus

**Ruta:** `/Admin/GestionarAsignaciones`

1. Elegir usuario (**monitor1** o un piloto de prueba).
2. Elegir bus **BUS-001**.
3. Guardar asignación activa (`EsActual = true`).

Reglas:

- Un usuario solo puede tener **una** asignación activa.
- Un bus solo puede tener **un** piloto/monitor activo a la vez.
- Si hay conflicto, **finalizar** la asignación anterior antes de crear otra.

---

## Parte 2 — Configurar y probar el **Padre**

### Credenciales demo

```
Email:    padre1@gmail.com
Usuario:  padre1
Clave:    Admin123!
```

### Flujo al iniciar sesión

```
Login
  → ¿IsFirstLogin?        → /Auth/ForceChangePassword
  → ¿Hijos sin GPS?       → /Padre/ConfiguracionInicial
  → Panel Padre           → /PagosPadresFamilia
```

### ConfiguracionInicial (ubicación)

Si algún hijo no tiene `Latitud`/`Longitud`:

1. Se muestra el mapa para buscar la dirección.
2. El padre confirma y guarda.
3. Opcional: botón *Configurar después* (volverá a pedirlo en el próximo login si siguen sin coordenadas).

### Pantallas del padre

| Pantalla | URL |
|----------|-----|
| Panel principal | `/PagosPadresFamilia` |
| Ruta del bus (mapa hijo) | `/Padres/DashboardRutaBusAsignado` |
| Asistencia | `/Padres/ConfirmarAsistencia` |
| Traslados | `/Padres/Traslados` |

### Checklist padre

- [ ] Login correcto
- [ ] Cambio de clave (solo si `IsFirstLogin = true`)
- [ ] Ubicación guardada para todos los hijos
- [ ] Llega al **Panel Padre** con las tarjetas (Pagos, Ruta del Bus, etc.)
- [ ] En **Ruta del Bus** ve la ruta del **mismo bus** que transmitirá el monitor
- [ ] **No** debe aparecer “Mapa General” en el panel

---

## Parte 3 — Configurar y probar el **Piloto**

### Credenciales (alternativas seed)

```
piloto2@transportesgenesis.com  / Admin123!
piloto3@transportesgenesis.com  / Admin123!
...
```

Para demo alineada con `padre1`, el piloto/monitor debe estar en **BUS-001** (Admin → Gestionar Asignaciones). Tanto `piloto1` como `monitor1` sirven si transmiten el mismo bus.

### Flujo al iniciar sesión

```
Login → /Piloto/MiRuta
```

### Requisitos

- Asignación activa en **Gestionar Asignaciones** (usuario ↔ bus).
- Ruta activa para el turno actual (**Mañana** antes de 12:00, **Tarde** después).
- Paradas cargadas en esa ruta.

### Simular recorrido (GPS en vivo)

En `/Piloto/MiRuta`:

1. Verificar que muestre el bus y la ruta correctos.
2. Pulsar **Simular ruta** (envía `POST /api/ubicaciones` cada ~2 s).
3. SignalR emite al grupo `Bus_{idBus}`.

URL con turno fijo (demo):

```
/Piloto/MiRuta?turno=Mañana
/Piloto/MiRuta?turno=Tarde
```

### Checklist piloto

- [ ] Muestra bus asignado (placa correcta)
- [ ] Lista de paradas visible
- [ ] Simulación mueve el marcador
- [ ] Consola: SignalR conectado y unido a `Bus_X`

---

## Parte 4 — Configurar y probar el **Monitor**

### Credenciales demo (recomendadas)

```
Email: monitor1@transportesgenesis.com
Clave: Admin123!
```

> `monitor1` debe estar asignado al **mismo bus** que los hijos de `padre1` (**BUS-001 / IdBus 4**).

### Pantallas

| Pantalla | URL |
|----------|-----|
| Mi ruta (simulación) | `/Monitor/MiRuta` |
| Listado niños | `/Monitor/ListadoNinos` |

El monitor también puede usar `/Piloto/MiRuta` (rol **Monitor** permitido).

### Simular recorrido

Igual que el piloto: botón **Simular ruta** en `/Monitor/MiRuta`.

Para demo de tarde:

```
/Monitor/MiRuta?turno=Tarde
```

### Checklist monitor

- [ ] Asignación activa al bus de la demo
- [ ] Ruta del turno cargada
- [ ] Simulación transmite ubicaciones
- [ ] Admin en Mapa en Tiempo Real ve el bus si está en horario

---

## Parte 5 — Demo completa (3 navegadores)

Orden sugerido:

| Paso | Rol | Acción |
|------|-----|--------|
| 1 | **Admin** | `/Geolocalizacion/MapaEnTiempoReal` → elegir **BUS-001** y ruta (Mañana/Tarde) |
| 2 | **Padre** | `/Padres/DashboardRutaBusAsignado?turno=Mañana` (o Tarde) |
| 3 | **Monitor** | `/Monitor/MiRuta?turno=Mañana` → **Simular ruta** |
| 4 | Todos | Verificar movimiento en mapa padre y admin (SignalR `Bus_4`) |

### Horario (mapa admin)

El mapa admin muestra alerta *“fuera de horario”* si la ruta no corresponde al turno actual:

- **Mañana:** desde `HoraInicio` hasta 12:00
- **Tarde:** desde `HoraInicio` (o 14:30) hasta 19:00

Para probar fuera de horario, usar `?turno=Mañana` o `?turno=Tarde` en piloto/monitor/padre.

---

## Verificación rápida en SQL

```sql
-- Cadena padre → bus → asignación
SELECT
    uPadre.Email       AS EmailPadre,
    a.Nombre + ' ' + a.Apellido AS Hijo,
    a.IdBusAsignado,
    b.Placa,
    uMon.Email         AS EmailMonitorPiloto
FROM genesis.Padres p
INNER JOIN AspNetUsers uPadre ON p.UsuarioId = uPadre.Id
INNER JOIN genesis.Alumnos a  ON a.IdPadre = p.IdPadre
LEFT  JOIN genesis.Buses b    ON b.IdBus = a.IdBusAsignado
LEFT  JOIN genesis.AsignacionPilotoBus ap ON ap.IdBus = a.IdBusAsignado AND ap.EsActual = 1
LEFT  JOIN AspNetUsers uMon   ON uMon.Id = ap.IdUsuarioPiloto
WHERE uPadre.Email = 'padre1@gmail.com';
```

Script completo: `Scripts/VERIFICAR_Demo_Padre_Piloto.sql`

---

## Problemas frecuentes

| Síntoma | Causa probable | Solución |
|---------|----------------|----------|
| Padre sin hijos | `Padres.UsuarioId` no enlazado | Vincular padre en BD o scripts seed |
| Piloto “sin bus asignado” | Sin fila en `AsignacionPilotoBus` | Admin → Gestionar Asignaciones |
| Padre no ve movimiento | Piloto/monitor en **otro bus** que los hijos del padre | Reasignar piloto a **BUS-001** o alinear padre y bus |
| Mapa admin sin movimiento | Fuera de horario o sin simulación | Simular ruta + turno/hora correctos |
| Pide cambio de clave siempre | `IsFirstLogin = 1` | Completar cambio o poner `IsFirstLogin = 0` en BD |
| Pide ubicación siempre | Hijos sin lat/lng | Completar ConfiguracionInicial |
| Contraseña no funciona | Hash distinto al admin | Ejecutar `Scripts/IGUALAR_Contraseña_Padres.sql` |

---

## Resumen de URLs

| Rol | Entrada principal |
|-----|-------------------|
| Admin | `/Admin` |
| Padre | `/PagosPadresFamilia` |
| Piloto | `/Piloto/MiRuta` |
| Monitor | `/Monitor/MiRuta` |
| Login | `/Auth/Login` |

---

*Última actualización: junio 2026 — Transportes Génesis*
