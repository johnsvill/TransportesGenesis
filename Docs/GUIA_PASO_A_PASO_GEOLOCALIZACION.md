# Guía paso a paso — Geolocalización (desde cero hasta en vivo)

Manual para quien **no conoce el sistema**: cómo llenar datos, generar rutas y ver el bus en tiempo real.

**Importante**

- Esta guía **no cambia** la demo existente (simulación en Mi Ruta, Plan B del padre, etc.).
- Si ya hay data de demo **y** configuras un bus real, **pueden convivir**: la demo sigue disponible; el bus con datos reales usa rutas/GPS de la BD.
- Contraseña típica de usuarios seed: `Admin123!`

---

## 1. URLs y credenciales

### Entorno

| Entorno | URL base |
|----------|----------|
| **Azure (publicado)** | `https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net` |
| **Local** | `https://localhost:7241` (o el puerto de tu `launchSettings`) |

Login: `{URL}/Auth/Login`

### Usuarios seed (tras el primer arranque de la app)

| Rol | Usuario / correo | Contraseña |
|-----|------------------|------------|
| Administrador | `admin` o `admin@transportesgenesis.com` | `Admin123!` |
| Piloto | `piloto2@transportesgenesis.com` | `Admin123!` |
| Monitor | `monitor2@transportesgenesis.com` | `Admin123!` |
| Padre | `padre1@gmail.com` | `Admin123!` |

> La app crea estos usuarios al **primer arranque**. Si la BD está vacía de Identity, abre la web una vez y espera a que cargue.

### Ventanas recomendadas

Usa **2 o 3 ventanas** (una normal + incógnito):

1. **Admin** — configura todo  
2. **Piloto** — transmite ubicación  
3. **Padre** — ve la ruta en vivo  

---

## 2. Mapa mental del flujo (qué depende de qué)

```
1. Usuarios Identity (app arranca)
2. Configurar COLEGIO (parada fija)
3. Crear BUSES
4. Padres + ALUMNOS con GPS y bus asignado
5. Asignar PILOTO / MONITOR al mismo bus
6. (Recomendado) Confirmar ASISTENCIA
7. Admin → CALCULAR RUTAS (Mañana y Tarde)
8. Piloto/Monitor → transmitir GPS (o Simular Ruta)
9. Admin → Mapa en vivo  |  Padre → Mi Ruta
```

Si saltas un paso, lo típico que falla:

| Si falta… | Qué verás |
|-----------|-----------|
| Buses | Lista vacía; no hay flota |
| Alumnos sin GPS o sin bus | Calcular rutas = 0 paradas útiles |
| Piloto sin asignación | “No tienes un bus asignado” |
| Bus del piloto ≠ bus del hijo | El padre no ve el movimiento |
| Sin calcular rutas | Mapas con poca info / fallback de demo |
| Sin transmitir GPS | Admin/padre: “Sin señal” o bus quieto |

---

## 3. Dos caminos para llenar datos

### Camino A — Rápido (SQL + calcular rutas)

Ideal para Azure vacío o demo de tesis.

1. Arranca la app una vez (crea usuarios).
2. En Azure SQL / SSMS ejecuta:  
   [`Scripts/SEED_DEMO_AZURE_DatosAleatorios.sql`](../Scripts/SEED_DEMO_AZURE_DatosAleatorios.sql)  
   (Instrucciones: [`Scripts/README_SEED_DEMO_AZURE.md`](../Scripts/README_SEED_DEMO_AZURE.md))
3. Login como **admin**.
4. Ve a **Calcular Rutas** (sección 8 de esta guía) — Mañana y Tarde.
5. Salta a la sección 9–11 (piloto + padre + mapa).

Eso crea buses `DEMO-001`…`003`, alumnos con GPS, asignaciones y asistencias.

### Camino B — Manual (pantallas admin)

Sigue las secciones 4 → 11 en orden. Úsalo cuando quieras aprender el flujo completo o datos propios del colegio.

---

## 4. Paso 1 — Entrar como administrador

1. Abre `/Auth/Login`.
2. Ingresa admin / `Admin123!`.
3. Debes llegar a `/Admin/Index` (Panel de Administrador).

**Qué debes ver:** tarjetas de mapa, buses, colegio, asignaciones, calcular rutas, alumnos, etc.

---

## 5. Paso 2 — Configurar el colegio (parada fija)

**URL:** `/Admin/ConfigurarColegio`  
Panel → sección **Configurar flota** → **Configurar Colegio**

1. Completa **Nombre**, **Dirección**.
2. Ajusta **Latitud / Longitud** (mapa: clic o arrastra el marcador 🏫).
3. Horas de clases (ej. `07:00` / `14:30`).
4. Guarda.

**Para qué sirve:** es la parada fija del colegio en rutas Mañana (última) y Tarde (primera), y el marcador en los mapas.

**Si no configuras:** el sistema usa un default Guatemala; mejor configurarlo para la demo real.

---

## 6. Paso 3 — Crear buses (flota)

**URL:** `/Geolocalizacion/BusesIndex`  
Panel → **Gestión de Buses** / **Configurar flota → Buses**

### Si no hay buses (pantalla vacía)

1. Verás “¡No hay buses registrados!” y un **formulario** (Placa, Modelo, Capacidad).
2. Ejemplo:
   - Placa: `BUS-001`
   - Modelo: `Mercedes-Benz Sprinter`
   - Capacidad: `30`
3. Clic en **Crear Primer Bus**.
4. Debes **quedarte en Gestión de Buses** y ver la tarjeta del bus.

> **Si en Azure te manda al dashboard:** esa versión publicada es antigua (el botón iba a una página que no existía).  
> Solución: republicar esta rama **o** usar el script SQL del Camino A mientras tanto.

### Si ya hay buses

Usa el formulario **Agregar bus** arriba de la lista.

Crea al menos **1 bus** para una demo mínima (o 2–3 para flota).

---

## 7. Paso 4 — Usuarios (si faltan roles)

**URL:** `/Admin/Usuarios`

Crea o verifica:

- Un **Piloto**
- Un **Monitor** (opcional pero útil)
- Un **Padre de Familia**

Anota correo y contraseña.

**Nota:** crear el usuario Identity **no** crea solo el registro de “Padre” en negocio ni los alumnos. Eso sigue en el siguiente paso.

---

## 8. Paso 5 — Alumnos con GPS y bus

**URL:** `/Admin/GestionarAlumnos`

Para **cada alumno** que quieras en la ruta:

1. Debe existir un **padre** vinculado (entidad `Padres` + `UsuarioId` al login del padre).
2. Asigna **bus** (`IdBusAsignado`) — el mismo que usará el piloto.
3. Configura **coordenadas** (lat/lng) o dirección geocodificada.
4. Guarda.

**Checklist mínimo para 1 hijo:**

- [ ] Padre puede iniciar sesión  
- [ ] Alumno con lat/lng  
- [ ] Alumno con bus asignado (ej. `BUS-001` / `DEMO-001`)

Sin GPS o sin bus → **Calcular rutas** no genera paradas útiles para ese niño.

---

## 9. Paso 6 — Asignar piloto (y monitor) al bus

**URL:** `/Admin/GestionarAsignaciones`

1. Selecciona el **piloto** (ej. `piloto2`).
2. Selecciona el **mismo bus** donde están los alumnos.
3. Guarda.
4. (Opcional) Asigna un **monitor** a otro bus o al mismo, según tu demo.

**Qué debes ver:** fila de asignación activa.

**Regla de oro:**  

`Bus del hijo` = `Bus del piloto que transmite`  

Si no coinciden, el padre no verá el movimiento en vivo.

---

## 10. Paso 7 — Asistencia (recomendado)

**Padre:** `/Padres/ConfirmarAsistencia`  
**Monitor:** `/Monitor/ListadoNinos`

1. Login como padre.
2. Confirma asistencia **Mañana** y/o **Tarde** para un **día hábil**.

El cálculo de rutas **prioriza** alumnos con asistencia confirmada.  
Si no confirmas, el sistema puede usar un **fallback** (alumnos asignados al bus), útil para demo, pero en operación real conviene confirmar.

---

## 11. Paso 8 — Calcular rutas (generar paradas)

**URL:** `/Admin/CalcularRutas`

1. Elige **fecha** (día hábil: lun–vie).
2. Ejecuta turno **Mañana**.
3. Ejecuta turno **Tarde**.

**Qué debes ver:** por cada bus activo, mensaje de éxito y cantidad de paradas.

**Cómo se arma la ruta (comportamiento del sistema):**

| Turno | Orden |
|-------|--------|
| **Mañana** | Casas (orden cercano) → **Colegio al final** |
| **Tarde** | **Colegio primero** → casas |

**Opcional después:**

- `/Admin/ReporteRutas` — listado  
- `/Admin/VerRuta?idRuta=…` — detalle  
- `/Admin/GestionarParadas` — ajustar paradas en el mapa  

Sin este paso, piloto/padre pueden caer en **datos de respaldo/demo**, no en la ruta real de BD.

---

## 12. Paso 9 — Ver en tiempo real (Admin)

**URL:** `/Geolocalizacion/MapaEnTiempoReal`  
Roles: Administrador (y también Piloto/Monitor).

1. En **Bus**, elige el bus que tiene ruta y alumnos.
2. Elige la **ruta** (turno).
3. Clic en **Ver ruta en mapa**.

**Qué debes ver:**

- Paradas de la ruta  
- Marcador del **colegio** (desde configuración)  
- Posición del bus cuando hay GPS reciente  

**Estados útiles:**

| Indicador | Significado |
|-----------|-------------|
| **EN VIVO** | Llegan actualizaciones SignalR |
| **Última posición conocida** | Hubo GPS, pero hace rato no llega nada |
| **SinSeñal** | Última ubicación en BD tiene más de ~10 minutos |

### Demo en el mapa admin (no sustituye GPS)

El botón tipo **Simular Rutas** en el mapa admin dibuja rutas hacia el colegio en el mapa.  
**No graba** ubicaciones en BD ni mueve el bus “de verdad” para el padre.  
Sirve para visualización; para demo en vivo usa el **piloto** (siguiente sección).

---

## 13. Paso 10 — Piloto / Monitor: transmitir ubicación

### Piloto

**URL:** `/Piloto/MiRuta`  
Login: `piloto2@…` / `Admin123!`

1. Debe aparecer el bus asignado y (si calculaste) la ruta del día.
2. Usa **Simular Ruta** (o GPS real si está disponible en el navegador).
3. Deja la pestaña abierta: cada pocos segundos envía posición a `/api/ubicaciones`.

**Qué debes ver:**

- Badge de GPS / simulación activa  
- Si pierdes red: badge **Sin conexión – cola (N)** y al volver **Sincronizando…** (cola offline)

### Monitor

**URL:** `/Monitor/MiRuta` (si entras por esa ruta)  
También puede usarse el flujo de piloto según el menú.

El monitor puede:

- Transmitir ubicación igual que el piloto  
- En modos demo, **broadcast** de ruta aleatoria hacia el grupo del bus (el padre puede redibujar la ruta)

**Convivencia con demo:**  
La simulación de Mi Ruta **sí escribe** ubicaciones reales en BD + SignalR. Por eso el admin y el padre ven el bus “de verdad” aunque el piloto use el botón Simular.  
Eso es intencional para demos sin GPS de vehículo físico.

---

## 14. Paso 11 — Padre: ver la ruta del bus

**URL:** `/Padres/DashboardRutaBusAsignado`  
Login: `padre1@gmail.com` / `Admin123!`  
Menú: **Mi Ruta**

1. Debe mostrar el bus de sus hijos, paradas / segmento y colegio.
2. Con el piloto simulando, el marcador del bus se mueve (**En vivo**).
3. Query opcional para forzar turno: `?turno=Mañana` o `?turno=Tarde`.

**Si tiene hijos en buses distintos:** verás un aviso; se muestra el bus más frecuente.

### Plan B (demo local del padre)

Si no hay señal en vivo, el padre puede usar la **simulación local (Plan B)** en su pantalla.  
Eso **no** escribe en la BD: solo anima el mapa para demostración cuando el piloto no está transmitiendo.

**Orden recomendado en una demo frente al cliente:**

1. Admin calcula rutas  
2. Piloto abre Mi Ruta y pulsa Simular  
3. Padre abre Mi Ruta → ve movimiento  
4. Admin abre Mapa en vivo → elige el mismo bus  

---

## 15. Checklist de prueba completa (vacío → vivo)

Marca en orden:

- [ ] App arrancó (usuarios Identity existen)  
- [ ] Login admin OK  
- [ ] Colegio configurado  
- [ ] Al menos 1 bus creado (UI o SQL)  
- [ ] Alumno con GPS + bus  
- [ ] Padre vinculado y puede entrar  
- [ ] Piloto asignado al **mismo** bus  
- [ ] (Opcional) Asistencia confirmada  
- [ ] Calcular Rutas Mañana  
- [ ] Calcular Rutas Tarde  
- [ ] Piloto: Simular Ruta (pestaña abierta)  
- [ ] Padre: Mi Ruta ve el bus moverse  
- [ ] Admin: Mapa en vivo ve el mismo bus  

---

## 16. Cómo conviven “demo” y “datos reales”

| Modo | Dónde | ¿Guarda en BD? | ¿Afecta al padre/admin? |
|------|--------|----------------|-------------------------|
| Simular Ruta (piloto/monitor) | MiRuta | **Sí** (ubicaciones) | **Sí** — se ve en vivo |
| Ruta demo aleatoria (monitor) | Monitor MiRuta | Evento SignalR + ubicaciones | **Sí** — padre puede actualizar ruta |
| Plan B local | Dashboard padre | **No** | Solo esa ventana del padre |
| Simular Rutas (mapa admin) | MapaEnTiempoReal | **No** | Solo overlay visual |
| Rutas calculadas | CalcularRutas | **Sí** (Rutas/Paradas) | Base “oficial” del día |
| Seed SQL DEMO-* | Script | **Sí** | Datos listos para calcular |

**Regla práctica:**

- Para **mostrar tracking real** al cliente: piloto Simular + padre Mi Ruta + admin mapa.  
- Para **rellenar vacío rápido**: script `SEED_DEMO_AZURE_DatosAleatorios.sql` + Calcular Rutas.  
- No hace falta apagar la demo: si hay bus con ruta y GPS, el flujo real manda; los fallbacks solo aparecen cuando falta data.

---

## 17. Problemas frecuentes

| Problema | Qué revisar |
|----------|-------------|
| Crear primer bus → dashboard | Azure sin redeploy; usa SQL o publica esta rama |
| Calcular rutas = 0 paradas | Alumnos sin lat/lng o sin bus; fecha fin de semana |
| Piloto sin bus | Gestionar Asignaciones |
| Padre no ve movimiento | Mismo IdBus; piloto transmitiendo; SignalR; no solo Plan B |
| SinSeñal en mapa | Piloto cerró la pestaña o >10 min sin POST |
| Colegio en sitio raro | `/Admin/ConfigurarColegio` y guardar de nuevo |
| Padre sin ruta | Calcular rutas del día; `?turno=Mañana` |

---

## 18. Script SQL de apoyo

| Archivo | Cuándo usarlo |
|---------|----------------|
| `Scripts/SEED_DEMO_AZURE_DatosAleatorios.sql` | BD vacía / Azure: buses + alumnos + asignaciones + asistencias |
| `Scripts/README_SEED_DEMO_AZURE.md` | Cómo ejecutarlo |
| `Scripts/Setup_ConfiguracionColegio.sql` | Solo si falta la tabla de configuración (luego edita coords en UI; el insert viejo puede traer Bogotá) |
| `Scripts/Seed_Escenarios_Completos.sql` | Extender asistencias cuando ya hay alumnos con GPS |

Tras el seed Azure: **siempre** Admin → Calcular Rutas (Mañana y Tarde).

---

## 19. Secuencia sugerida para una demo de 15 minutos

1. **Admin** — mostrar colegio + buses + calcular rutas (1–2 min).  
2. **Piloto** (incógnito) — Simular Ruta (1 min).  
3. **Padre** (otra ventana) — Mi Ruta: bus en movimiento (2 min).  
4. **Admin** — Mapa en vivo, seleccionar el mismo bus (2 min).  
5. Preguntas: pérdida de señal (cerrar pestaña piloto → badge / última posición).  

---

## 20. Referencias rápidas de pantallas

| Acción | URL |
|--------|-----|
| Login | `/Auth/Login` |
| Panel admin | `/Admin/Index` |
| Colegio | `/Admin/ConfigurarColegio` |
| Buses | `/Geolocalizacion/BusesIndex` |
| Alumnos | `/Admin/GestionarAlumnos` |
| Asignaciones | `/Admin/GestionarAsignaciones` |
| Calcular rutas | `/Admin/CalcularRutas` |
| Paradas | `/Admin/GestionarParadas` |
| Mapa admin | `/Geolocalizacion/MapaEnTiempoReal` |
| Padre Mi Ruta | `/Padres/DashboardRutaBusAsignado` |
| Confirmar asistencia | `/Padres/ConfirmarAsistencia` |
| Piloto | `/Piloto/MiRuta` |
| Monitor | `/Monitor/MiRuta` |

---

**Última actualización:** alineada con colegio configurable, creación de buses en `BusesIndex`, seed Azure DEMO y cola offline GPS.  
Para que Azure tenga estos comportamientos, hay que **publicar la rama actual** después de mergear.
