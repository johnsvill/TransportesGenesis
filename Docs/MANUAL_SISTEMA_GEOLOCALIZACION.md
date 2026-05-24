# 📘 Manual Completo del Sistema de Geolocalización

## 🎯 Transportes Genesis - Cómo Funciona Todo el Sistema

---

## 📋 Tabla de Contenidos

1. [Introducción](#introducción)
2. [Estructura de Datos](#estructura-de-datos)
3. [Flujo Completo del Sistema](#flujo-completo-del-sistema)
4. [Roles y Permisos](#roles-y-permisos)
5. [Proceso de Registro](#proceso-de-registro)
6. [Gestión de Paradas](#gestión-de-paradas)
7. [Rutas y Optimización](#rutas-y-optimización)
8. [Seguimiento en Tiempo Real](#seguimiento-en-tiempo-real)
9. [Notificaciones y Alertas](#notificaciones-y-alertas)
10. [Casos de Uso Detallados](#casos-de-uso-detallados)

---

## 1. Introducción

### ¿Qué es Transportes Genesis?

Es un sistema integral para gestionar transporte escolar que permite:
- 🚌 Rastrear buses en tiempo real
- 📍 Gestionar paradas y rutas
- 👨‍👩‍👧 Registro de padres y alumnos
- 💰 Gestión de pagos
- 📊 Reportes y análisis
- 🔔 Notificaciones automáticas

### Tecnologías Clave:
- **Backend**: ASP.NET Core 8 (C#)
- **Frontend**: Razor Pages + Bootstrap 5
- **Mapas**: Leaflet.js + OpenStreetMap
- **Geocodificación**: Nominatim API
- **Base de Datos**: SQL Server
- **Tiempo Real**: SignalR

---

## 2. Estructura de Datos

### 2.1 Entidades Principales

#### 👨‍👩‍👧 **Padres** (Tabla: `genesis.Padres`)
```sql
IdPadre      INT PRIMARY KEY
Nombre       VARCHAR(100)
Apellido     VARCHAR(100)
-- Relacionado con AspNetUsers para login
```

#### 👦 **Alumnos** (Tabla: `genesis.Alumnos`)
```sql
IdAlumno         INT PRIMARY KEY
IdPadre          INT FOREIGN KEY → Padres (¿De quién es hijo?)
Nombre           VARCHAR(100)
Apellido         VARCHAR(100)
IdBusAsignado    INT FOREIGN KEY → Bus (¿En qué bus va?)
Latitud          DECIMAL(10,7) (¿Dónde vive? - Coordenada Y)
Longitud         DECIMAL(10,7) (¿Dónde vive? - Coordenada X)
```

#### 📍 **Paradas** (Tabla: `genesis.Paradas`)
```sql
IdParada     INT PRIMARY KEY
IdRuta       INT FOREIGN KEY → Rutas
IdAlumno     INT NULLABLE FOREIGN KEY → Alumnos (puede ser NULL si es parada del colegio)
Latitud      DECIMAL(10,7)
Longitud     DECIMAL(10,7)
Direccion    VARCHAR(250)
Orden        INT (1, 2, 3... orden de recogida)
HoraEstimada TIME (ej: 07:15 AM)
Completada   BIT (¿Ya pasó el bus?)
```

#### 🚌 **Bus** (Tabla: `genesis.Buses`)
```sql
IdBus          INT PRIMARY KEY
Placa          VARCHAR(20)
LatitudActual  DECIMAL(10,7) (¿Dónde está AHORA?)
LongitudActual DECIMAL(10,7) (¿Dónde está AHORA?)
Activo         BIT
```

#### 🛣️ **Rutas** (Tabla: `genesis.Rutas`)
```sql
IdRuta       INT PRIMARY KEY
Nombre       VARCHAR(100)
IdBus        INT FOREIGN KEY → Bus
TipoRuta     VARCHAR(20) (Ida/Regreso)
FechaAsignada DATE
```

---

### 2.2 Relación Entre Entidades

```
Padre (👨‍👩‍👧)
  │
  └─── tiene uno o más ──→ Alumno (👦)
							   │
							   │ vive en coordenadas (lat, lng)
							   │
							   ├─── se le crea una ──→ Parada (📍)
							   │                          │
							   │                          └─── pertenece a ──→ Ruta (🛣️)
							   │                                                   │
							   │                                                   └─── asignada a ──→ Bus (🚌)
							   │
							   └─── asignado a ──→ Bus (🚌)
```

---

## 3. Flujo Completo del Sistema

### 📊 Diagrama de Flujo General

```
1. REGISTRO
   │
   ├─→ Padre crea cuenta (Login)
   │
   └─→ Administrador registra al Alumno
	   └─→ Asigna coordenadas (dirección de casa)

2. CONFIGURACIÓN
   │
   ├─→ Admin crea Paradas
   │   └─→ Parada del Alumno (casa)
   │   └─→ Parada del Colegio
   │
   └─→ Admin crea Rutas
	   └─→ Asigna Bus a la Ruta
	   └─→ Optimiza orden de paradas (algoritmo TSP)

3. OPERACIÓN DIARIA
   │
   ├─→ Padre confirma asistencia del alumno
   │   └─→ Sistema actualiza lista del día
   │
   ├─→ Piloto/Monitor inicia recorrido
   │   └─→ GPS del bus se actualiza cada X segundos
   │
   ├─→ Sistema calcula proximidad a paradas
   │   └─→ Envía notificación al padre
   │       "El bus está a 5 minutos"
   │
   └─→ Monitor marca asistencia
	   └─→ Confirma que el alumno subió al bus

4. MONITOREO
   │
   ├─→ Padre ve en mapa dónde está el bus
   │
   ├─→ Admin ve todos los buses activos
   │
   └─→ Sistema registra alertas de retraso
```

---

## 4. Roles y Permisos

### 👑 **Administrador** (Role: `Admin`)
- ✅ Crear/editar/eliminar paradas
- ✅ Crear/editar/eliminar rutas
- ✅ Asignar buses a rutas
- ✅ Gestionar usuarios (pilotos, monitores, padres)
- ✅ Ver todos los buses en tiempo real
- ✅ Generar reportes
- ✅ Gestionar pagos

### 🚗 **Piloto** (Role: `Piloto`)
- ✅ Ver su ruta asignada del día
- ✅ Ver mapa con paradas
- ✅ Actualizar ubicación del bus
- ⚠️ NO puede editar rutas

### 👀 **Monitor** (Role: `Monitor`)
- ✅ Ver lista de alumnos de su bus
- ✅ Marcar asistencia (presente/ausente)
- ✅ Ver ruta del día
- ⚠️ NO puede cambiar la ruta

### 👨‍👩‍👧 **Padre de Familia** (Role: `Padre`)
- ✅ Ver ubicación del bus de su hijo
- ✅ Confirmar asistencia diaria
- ✅ Ver historial de recorridos
- ✅ Recibir notificaciones
- ✅ Gestionar sus pagos
- ⚠️ NO puede ver otros buses

---

## 5. Proceso de Registro

### 5.1 Registro de Padre de Familia

#### Paso 1: Crear Usuario de Login
El padre se registra en el sistema creando una cuenta:

```
URL: /Account/Register

Formulario:
- Email: padre@gmail.com
- Contraseña: ********
- Confirmar Contraseña: ********
```

Esto crea:
- ✅ Registro en `AspNetUsers`
- ✅ Asignación del rol `Padre`

#### Paso 2: Administrador Vincula Padre con Entidad Padre
El administrador debe:

```sql
INSERT INTO genesis.Padres (Nombre, Apellido, UsuarioId)
VALUES ('Juan', 'Pérez', '[UsuarioId del login]');
```

**Importante**: El vínculo entre `AspNetUsers` y `genesis.Padres` se hace por el campo `UsuarioId`.

---

### 5.2 Registro de Alumno

#### ¿Quién lo hace?
👑 **Solo el Administrador**

#### Paso 1: Admin Crea el Alumno
```sql
INSERT INTO genesis.Alumnos 
(IdPadre, Nombre, Apellido, Latitud, Longitud, IdBusAsignado)
VALUES 
(1,                    -- ID del padre (Juan Pérez)
 'María',              -- Nombre del alumno
 'Pérez',              -- Apellido
 14.590843,            -- Latitud de la casa (Guatemala)
 -90.551780,           -- Longitud de la casa
 1                     -- ID del bus asignado (BUS-001)
);
```

#### Paso 2: Sistema Crea la Parada Automáticamente

Cuando el admin registra un alumno con coordenadas, el sistema **puede crear automáticamente** una parada en esas coordenadas, o el admin puede crearla manualmente.

**Opción A: Crear Parada Manualmente**
```sql
INSERT INTO genesis.Paradas
(IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden)
VALUES
(1,                    -- Ruta de la mañana
 1,                    -- ID del alumno (María Pérez)
 14.590843,            -- Misma latitud que el alumno
 -90.551780,           -- Misma longitud que el alumno
 '5ta Avenida 12-34 Zona 10',
 2                     -- Segunda parada de la ruta
);
```

**Opción B: Usar la Interfaz Web**
1. Admin va a `/Admin/GestionarParadas`
2. Hace clic en "Agregar Parada"
3. Escribe la dirección del alumno
4. Hace clic en "🔍 Buscar en Mapa"
5. El sistema geocodifica y obtiene lat/lng automáticamente
6. Guarda la parada

---

### 5.3 ¿Cómo Sabe el Sistema Dónde Vive el Alumno?

#### Forma 1: Administrador Ingresa Dirección

1. **Admin abre formulario** de crear alumno
2. **Escribe dirección**: `"5ta Avenida 12-34 Zona 10, Guatemala"`
3. **Hace clic en "Buscar en Mapa"**
4. **Sistema geocodifica**:
   - Llama a API de Nominatim
   - Obtiene: `Lat: 14.590843, Lng: -90.551780`
5. **Guarda en tabla Alumnos**:
   ```sql
   UPDATE Alumnos 
   SET Latitud = 14.590843, 
	   Longitud = -90.551780
   WHERE IdAlumno = 1;
   ```

#### Forma 2: Administrador Hace Click en el Mapa

1. **Admin abre el mapa**
2. **Hace clic en la ubicación de la casa** del alumno
3. **Sistema captura las coordenadas** del click
4. **Guarda automáticamente**

## Forma 3: Padre Proporciona su Ubicación (✅ IMPLEMENTADO)

**Descripción**: El padre, después de cambiar su contraseña en el primer login, indica su dirección y el sistema la geocodifica automáticamente.

### Flujo:
1. **Padre inicia sesión por primera vez**
   ```
   Usuario: juan.perez@gmail.com
   Contraseña: Temp123 (temporal)
   ```

2. **Sistema detecta primer login**
   - Redirige a cambio de contraseña
   - Usuario cambia a contraseña personal

3. **Sistema detecta que falta dirección de recogida**
   - Verifica si el alumno tiene coordenadas (Latitud/Longitud)
   - Si no tiene → Redirige a `/Padre/ConfiguracionInicial`

4. **Padre configura dirección de recogida**
   ```
   ┌────────────────────────────────────────────┐
   │  Configuración Inicial Requerida           │
   ├────────────────────────────────────────────┤
   │  👤 Alumno: María Pérez                    │
   │                                             │
   │  📍 Dirección: [_________________]          │
   │     Ej: 5ta Avenida 12-34 Zona 10         │
   │                                             │
   │  [🔍 Buscar en Mapa]                       │
   │                                             │
   │  🗺️ [Mapa con marcador verde]             │
   │                                             │
   │  [Guardar y Continuar]                     │
   └────────────────────────────────────────────┘
   ```

5. **Padre escribe dirección y busca en el mapa**
   - Escribe: "5ta Avenida 12-34 Zona 10, Guatemala"
   - Hace clic en "🔍 Buscar en Mapa"
   - Sistema geocodifica usando Nominatim API
   - Obtiene coordenadas: `(14.590843, -90.551780)`
   - Muestra marcador verde en el mapa
   - Padre verifica visualmente

6. **Padre guarda la configuración**
   - Sistema actualiza `genesis.Alumnos`:
     ```sql
     UPDATE genesis.Alumnos
     SET Latitud = 14.590843,
         Longitud = -90.551780
     WHERE IdAlumno = 1;
     ```

   - Si el alumno tiene bus asignado, crea parada automáticamente:
     ```sql
     INSERT INTO genesis.Paradas
     (IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, Activo)
     VALUES
     ([IdRuta], 1, 14.590843, -90.551780, '5ta Avenida...', [Orden], 1);
     ```

7. **Padre es redirigido al Dashboard**
   - Ya puede ver el bus en tiempo real
   - Ya puede recibir notificaciones
   - Ya puede ver ETA (tiempo de llegada)

### Ventajas:
- ✅ **Más preciso**: Geocodificación directa de la dirección
- ✅ **Menos carga para el admin**: El padre proporciona su propia dirección
- ✅ **Verificación visual**: Padre puede ver el mapa antes de guardar
- ✅ **Automatizado**: Crea parada automáticamente si hay bus asignado
- ✅ **Experiencia guiada**: Paso a paso en el primer login

### Archivos Involucrados:
- `Pages/Padre/ConfiguracionInicial.cshtml` (Vista con mapa)
- `Pages/Padre/ConfiguracionInicial.cshtml.cs` (Lógica)
- `Repositories/Interfaces/IAlumnoRepository.cs` (Contrato)
- `Repositories/Implementations/AlumnoRepository.cs` (Implementación)
- `Models/DB/Negocio/Padres.cs` (Campo UsuarioId agregado)
- `Scripts/Add_UsuarioId_To_Padres.sql` (Migración de BD)

### Configuración Necesaria:
```sql
-- Ejecutar en SQL Server
USE TransportesGenesisDB;
GO

ALTER TABLE genesis.Padres
ADD UsuarioId NVARCHAR(450) NULL;
GO

CREATE INDEX IX_Padres_UsuarioId ON genesis.Padres(UsuarioId);
GO
```

### Cómo se vincula Padre con Usuario:
```sql
-- Al crear un padre, vincular con AspNetUsers
UPDATE genesis.Padres
SET UsuarioId = (SELECT Id FROM AspNetUsers WHERE Email = 'juan.perez@gmail.com')
WHERE Nombre = 'Juan' AND Apellido = 'Pérez';
```

---

## 6. Gestión de Paradas

### 6.1 Tipos de Paradas

#### 📍 **Parada de Alumno**
- Asociada a un alumno específico (`IdAlumno` NOT NULL)
- Ubicada en la casa del alumno
- Ejemplo: Casa de María Pérez

#### 🏫 **Parada del Colegio**
- NO asociada a un alumno (`IdAlumno` IS NULL)
- Ubicación fija
- Ejemplo: Entrada principal del colegio

---

### 6.2 Crear Parada con Geocodificación

#### Interfaz Web: `/Admin/GestionarParadas`

**Flujo Paso a Paso**:

```
1. Admin abre "Gestionar Paradas"
   └─→ Ve mapa interactivo con paradas existentes

2. Hace clic en "Agregar Parada"
   └─→ Se abre modal

3. Escribe dirección en el campo
   Ejemplo: "Centro Comercial Oakland, Guatemala"

4. Hace clic en "🔍 Buscar en Mapa"
   │
   ├─→ JavaScript ejecuta geocodificarDireccion()
   │
   ├─→ Llama a API Nominatim:
   │   https://nominatim.openstreetmap.org/search
   │   ?format=json
   │   &q=Centro+Comercial+Oakland,+Guatemala
   │
   └─→ Recibe respuesta:
	   {
		 "lat": "14.598765",
		 "lon": "-90.512345",
		 "display_name": "Oakland Mall, Diagonal 6, Zona 10..."
	   }

5. Sistema llena automáticamente:
   ├─→ Latitud: 14.598765
   └─→ Longitud: -90.512345

6. Aparece marcador ROJO en el mapa
   └─→ Admin verifica que sea la ubicación correcta

7. Admin asigna:
   ├─→ Orden: 3 (tercera parada)
   ├─→ Ruta: Ruta Mañana - Bus 001
   └─→ (Opcional) IdAlumno: si es casa de un alumno

8. Hace clic en "Guardar"
   └─→ Se guarda en BD:
	   INSERT INTO Paradas 
	   (IdRuta, Latitud, Longitud, Direccion, Orden, IdAlumno)
	   VALUES (1, 14.598765, -90.512345, 'Oakland Mall', 3, NULL)
```

---

### 6.3 Flujo Técnico de Geocodificación

```javascript
// Código simplificado
async function geocodificarDireccion() {
	// 1. Obtener dirección del input
	const direccion = document.getElementById('parada-nombre').value;
	// "Centro Comercial Oakland, Guatemala"

	// 2. Construir URL de la API
	const url = `https://nominatim.openstreetmap.org/search
				 ?format=json
				 &q=${encodeURIComponent(direccion)}
				 &limit=3`;

	// 3. Hacer petición HTTP
	const response = await fetch(url);
	const resultados = await response.json();

	// 4. Tomar primer resultado
	const ubicacion = resultados[0];

	// 5. Actualizar campos del formulario
	document.getElementById('parada-lat').value = ubicacion.lat;
	document.getElementById('parada-lng').value = ubicacion.lon;

	// 6. Agregar marcador rojo en el mapa
	L.marker([ubicacion.lat, ubicacion.lon]).addTo(mapa);
}
```

---

## 7. Rutas y Optimización

### 7.1 Crear una Ruta

#### Ejemplo: Ruta de la Mañana

```sql
-- 1. Crear la ruta
INSERT INTO genesis.Rutas (Nombre, IdBus, TipoRuta, FechaAsignada)
VALUES ('Ruta Mañana - Zona 10', 1, 'Ida', '2026-05-20');

-- 2. Asignar paradas a la ruta (en orden)
INSERT INTO genesis.Paradas (IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden)
VALUES
-- Parada 1: Casa de Juan (primera recogida)
(1, 1, 14.590843, -90.551780, '5ta Avenida 12-34 Zona 10', 1),

-- Parada 2: Casa de María (segunda recogida)
(1, 2, 14.598765, -90.512345, 'Oakland Mall Zona 10', 2),

-- Parada 3: Casa de Pedro (tercera recogida)
(1, 3, 14.601234, -90.505678, '12 Calle 3-45 Zona 10', 3),

-- Parada 4: Colegio (destino final) - NO tiene IdAlumno
(1, NULL, 14.610000, -90.500000, 'Colegio Ejemplo, Zona 10', 4);
```

---

### 7.2 Optimización Automática de Rutas (Algoritmo TSP)

El sistema incluye un **algoritmo de optimización** que calcula la ruta más eficiente para recoger a todos los alumnos.

#### ¿Cómo funciona?

```
Problema:
- Tienes 10 alumnos en diferentes puntos de la ciudad
- El bus debe recogerlos a todos
- ¿En qué orden para minimizar distancia/tiempo?

Solución: Traveling Salesman Problem (TSP)
```

#### Interfaz: `/Admin/CalcularRutas`

```
1. Admin selecciona:
   ├─→ Bus: BUS-001
   ├─→ Fecha: 2026-05-20
   └─→ Tipo: Ida (mañana)

2. Sistema lista todos los alumnos de ese bus

3. Admin hace clic en "Calcular Ruta Óptima"

4. Sistema ejecuta algoritmo TSP:
   ├─→ Calcula distancia entre todas las casas
   ├─→ Encuentra el orden más corto
   └─→ Actualiza el campo "Orden" de cada parada

5. Resultado:
   Orden Original:        Orden Optimizado:
   1. Juan (km 0)         1. Pedro (km 0)
   2. María (km 5)        2. Juan (km 2)
   3. Pedro (km 10)       3. María (km 3)
   4. Colegio (km 15)     4. Colegio (km 5)

   Ahorro: 10 km menos
```

---

## 8. Seguimiento en Tiempo Real

### 8.1 Actualización de Ubicación del Bus

#### ¿Cómo se actualiza?

**Opción A: Manualmente (Demo/Pruebas)**
```javascript
// JavaScript en el navegador del piloto
function actualizarUbicacion(lat, lng) {
	fetch('/api/buses/1/ubicacion', {
		method: 'PUT',
		body: JSON.stringify({ latitud: lat, longitud: lng })
	});
}
```

**Opción B: Automático con GPS del Navegador**
```javascript
// Geolocation API del navegador
navigator.geolocation.watchPosition(function(position) {
	const lat = position.coords.latitude;
	const lng = position.coords.longitude;

	actualizarUbicacion(lat, lng);
}, null, {
	enableHighAccuracy: true,
	timeout: 5000,
	maximumAge: 0
});
```

**Opción C: Dispositivo GPS Físico (Hardware)**
- Dispositivo GPS en el bus
- Envía coordenadas por 4G/5G
- Endpoint API: `/api/buses/{idBus}/ubicacion`

---

### 8.2 Notificaciones de Proximidad

#### ¿Cómo Funciona?

```
1. Bus se mueve por la ciudad
   └─→ Ubicación se actualiza cada 10 segundos

2. Sistema calcula distancia a cada parada
   Formula: distancia = √((lat1-lat2)² + (lng1-lng2)²)

3. Si distancia < 500 metros:
   ├─→ Sistema crea NotificacionProximidad
   │
   ├─→ SignalR envía notificación en tiempo real
   │   └─→ Grupo: "Padre_{IdPadre}"
   │
   └─→ Padre recibe:
	   "🚌 El bus está a 5 minutos de tu parada"
```

#### Código Backend (Simplificado):
```csharp
// Services/NotificacionService.cs
public async Task VerificarProximidad(int idBus)
{
	var bus = await _busRepository.GetByIdAsync(idBus);
	var paradas = await _paradaRepository.GetParadasActivasByBusAsync(idBus);

	foreach (var parada in paradas)
	{
		var distancia = CalcularDistancia(
			bus.LatitudActual, bus.LongitudActual,
			parada.Latitud, parada.Longitud
		);

		if (distancia < 0.5) // 500 metros
		{
			// Crear notificación
			var notificacion = new NotificacionProximidad
			{
				IdBus = idBus,
				IdParada = parada.IdParada,
				DistanciaMetros = distancia * 1000
			};

			await _notificacionRepository.AddAsync(notificacion);

			// Enviar por SignalR
			await _hubContext.Clients
				.Group($"Padre_{parada.Alumno.IdPadre}")
				.SendAsync("BusProximo", new {
					mensaje = "El bus está a 5 minutos",
					distancia = distancia,
					tiempoEstimado = 5 // minutos
				});
		}
	}
}
```

---

### 8.3 Vista del Padre

#### Página: `/Padre/SeguirBus`

```
1. Padre inicia sesión

2. Sistema identifica a sus hijos:
   SELECT * FROM Alumnos WHERE IdPadre = @IdPadre

3. Obtiene el bus de cada hijo:
   SELECT IdBusAsignado FROM Alumnos WHERE IdAlumno = @IdAlumno

4. Muestra mapa con:
   ├─→ Ubicación actual del bus (marcador azul)
   ├─→ Parada de su hijo (marcador verde)
   ├─→ Ruta trazada (línea azul)
   └─→ ETA (tiempo estimado de llegada): "5 minutos"

5. Actualización en tiempo real con SignalR:
   - Cada 10 segundos
   - Bus se mueve en el mapa
   - ETA se actualiza
```

---

## 9. Notificaciones y Alertas

### 9.1 Tipos de Notificaciones

#### 🚌 **Notificación de Proximidad**
```
"El bus está a 5 minutos de tu parada"
Enviada cuando: Distancia < 500 metros
Destinatario: Padre del alumno
```

#### ⏰ **Notificación de Retraso**
```
"El bus tiene un retraso de 10 minutos"
Enviada cuando: HoraActual > HoraEstimada + 5 minutos
Destinatario: Todos los padres de la ruta
```

#### ✅ **Confirmación de Recogida**
```
"Juan fue recogido a las 7:15 AM"
Enviada cuando: Monitor marca asistencia
Destinatario: Padre del alumno
```

#### 🏫 **Llegada al Destino**
```
"El bus llegó al colegio a las 7:45 AM"
Enviada cuando: Bus llega a la última parada
Destinatario: Todos los padres de la ruta
```

---

### 9.2 Sistema de Alertas (Admin)

#### Dashboard de Alertas: `/Admin/Alertas`

```
Panel de Control
├─→ Alertas Activas (🔴 5)
│   ├─→ Bus 001: Retraso de 15 minutos
│   ├─→ Bus 003: Sin señal GPS
│   └─→ Bus 005: Ruta desviada
│
├─→ Historial de Alertas
│   └─→ Últimos 30 días
│
└─→ Estadísticas
	├─→ Promedio de retrasos: 8 minutos
	├─→ Total de alertas: 120
	└─→ Tasa de puntualidad: 92%
```

---

## 10. Casos de Uso Detallados

### 10.1 Caso de Uso 1: Registro de Nuevo Alumno

#### 👨‍👩‍👧 Personajes:
- **Juan Pérez** (Padre)
- **María Pérez** (Hija, 8 años)
- **Admin** (Administrador del sistema)

#### 📝 Historia:
Juan quiere inscribir a su hija María en el transporte escolar.

#### 🎬 Flujo:

**Día 1: Registro del Padre**

1. Juan va a `https://transportesgenesis.com/Account/Register`
2. Llena formulario:
   - Email: `juan.perez@gmail.com`
   - Contraseña: `********`
3. Recibe email de confirmación
4. Confirma su cuenta
5. ✅ Juan ahora puede iniciar sesión

**Día 2: Admin Registra a María**

1. Admin inicia sesión
2. Va a `/Admin/Alumnos/Crear`
3. Llena formulario:
   - **Nombre**: María
   - **Apellido**: Pérez
   - **Padre**: Juan Pérez (selecciona de lista)
   - **Dirección**: "5ta Avenida 12-34 Zona 10, Guatemala"
4. Hace clic en "🔍 Buscar en Mapa"
5. Sistema geocodifica:
   - Latitud: `14.590843`
   - Longitud: `-90.551780`
6. Admin asigna:
   - **Bus**: BUS-001
   - **Grado**: 3ro Primaria
7. Hace clic en "Guardar"

**Día 2: Admin Crea la Parada de María**

1. Admin va a `/Admin/GestionarParadas`
2. Hace clic en "Agregar Parada"
3. Llena formulario:
   - **Dirección**: "5ta Avenida 12-34 Zona 10" (copia del registro)
   - **Ruta**: Ruta Mañana - BUS-001
   - **Orden**: 3 (tercera parada)
   - **Alumno**: María Pérez
4. Sistema ya tiene las coordenadas de María:
   - Latitud: `14.590843`
   - Longitud: `-90.551780`
5. Hace clic en "Guardar"
6. ✅ Parada creada y vinculada a María

**Día 3: Primer Día de Uso**

1. Juan inicia sesión en su app/web
2. Ve dashboard con:
   - Foto de María
   - Bus asignado: BUS-001
   - Parada: 5ta Avenida 12-34
   - Estado: "Bus en camino"
3. A las 7:05 AM recibe notificación:
   - "🚌 El bus está a 10 minutos"
4. A las 7:12 AM recibe:
   - "🚌 El bus está a 2 minutos"
5. A las 7:15 AM recibe:
   - "✅ María fue recogida"
6. A las 7:45 AM recibe:
   - "🏫 María llegó al colegio"

---

### 10.2 Caso de Uso 2: Día Típico del Sistema

#### 🌅 Mañana (6:00 AM - 8:00 AM)

**6:00 AM - Padres Confirman Asistencia**
```
Padre 1: Confirma que Juan irá hoy ✅
Padre 2: Confirma que María irá hoy ✅
Padre 3: Cancela a Pedro por enfermedad ❌
```

Sistema actualiza lista del día:
```sql
UPDATE AsistenciaAlumno 
SET Confirmada = 1 
WHERE IdAlumno = 1 AND Fecha = '2026-05-20';

UPDATE AsistenciaAlumno 
SET Confirmada = 0 
WHERE IdAlumno = 3 AND Fecha = '2026-05-20';
```

**6:30 AM - Piloto Inicia Recorrido**
```
1. Piloto abre app en su celular
2. Va a "Mi Ruta"
3. Ve lista de alumnos:
   ✅ Juan - 5ta Avenida 12-34
   ✅ María - Oakland Mall
   ❌ Pedro - CANCELADO (no se muestra)
4. Sistema activa GPS
5. Comienza rastreo en tiempo real
```

**6:45 AM - Bus en Movimiento**
```
Sistema actualiza ubicación cada 10 segundos:
- 6:45:00 → Lat: 14.580000, Lng: -90.560000
- 6:45:10 → Lat: 14.581000, Lng: -90.559000
- 6:45:20 → Lat: 14.582000, Lng: -90.558000
...
```

**7:00 AM - Primera Parada (Casa de Juan)**
```
Sistema detecta:
- Bus está a 400 metros de parada de Juan
- Envía notificación a Padre de Juan:
  "🚌 El bus está a 5 minutos"

7:05 AM:
- Bus llega a la parada
- Monitor marca en app: "Juan subió al bus" ✅
- Sistema envía: "✅ Juan fue recogido a las 7:05 AM"
```

**7:15 AM - Segunda Parada (Casa de María)**
```
- Notificación a Padre de María: "🚌 Bus a 5 minutos"
- Bus llega
- Monitor marca: "María subió al bus" ✅
- Notificación: "✅ María fue recogida a las 7:15 AM"
```

**7:45 AM - Llegada al Colegio**
```
- Bus completa ruta
- Sistema registra hora de llegada
- Notificación a TODOS los padres:
  "🏫 Bus llegó al colegio a las 7:45 AM"
- Monitor confirma que todos bajaron del bus
```

---

### 10.3 Caso de Uso 3: Gestión de Paradas con Geocodificación

#### 📍 Escenario:
El colegio tiene nuevo alumno y el admin debe crear su parada.

**Alumno**: Carlos López  
**Dirección**: "Universidad Rafael Landívar, Vista Hermosa, Guatemala"

#### 🎬 Flujo Detallado:

**Paso 1: Admin Abre Gestión de Paradas**
```
URL: /Admin/GestionarParadas
```

**Paso 2: Ve el Mapa Interactivo**
```
[Mapa de Guatemala]
- Marcador azul: Parada 1 (Juan)
- Marcador azul: Parada 2 (María)
- Marcador verde: Colegio
```

**Paso 3: Hace Click en "Agregar Parada"**
```
[Modal]
┌────────────────────────────────────┐
│  Crear Nueva Parada                │
├────────────────────────────────────┤
│ Dirección: [____________]          │
│            [🔍 Buscar en Mapa]     │
│                                    │
│ Latitud:  [____________] (readonly)│
│ Longitud: [____________] (readonly)│
│                                    │
│ Orden: [3]                         │
│ Ruta: [Ruta Mañana - BUS-001 ▼]   │
│ Alumno: [Carlos López ▼]           │
│                                    │
│ [✓] Parada Activa                  │
│                                    │
│ [Cancelar]  [Guardar]              │
└────────────────────────────────────┘
```

**Paso 4: Admin Escribe Dirección**
```
Dirección: [Universidad Rafael Landívar, Guatemala]
```

**Paso 5: Hace Click en "🔍 Buscar en Mapa"**

**Paso 5.1: JavaScript Ejecuta Geocodificación**
```javascript
console.log('=== INICIO GEOCODIFICACIÓN ===');
console.log('Dirección: Universidad Rafael Landívar, Guatemala');

// Llamada a API
fetch('https://nominatim.openstreetmap.org/search?format=json&q=Universidad+Rafael+Landívar,+Guatemala&limit=3')
```

**Paso 5.2: API Responde**
```json
[
  {
	"place_id": 123456,
	"lat": "14.6049",
	"lon": "-90.4889",
	"display_name": "Universidad Rafael Landívar, Vista Hermosa III, Guatemala",
	"type": "university",
	"importance": 0.8
  }
]
```

**Paso 5.3: Sistema Actualiza Formulario**
```
Latitud:  [14.6049] (lleno automáticamente)
Longitud: [-90.4889] (lleno automáticamente)
```

**Paso 5.4: Aparece Marcador Rojo en el Mapa**
```
[Mapa]
- Marcador azul: Parada 1 (Juan)
- Marcador azul: Parada 2 (María)
- Marcador ROJO: Nueva ubicación (URL) ← ¡NUEVO!
- Marcador verde: Colegio
```

**Paso 5.5: Muestra Información**
```
ℹ️ ✅ Ubicación encontrada:
   Universidad Rafael Landívar, Vista Hermosa III, Guatemala
```

**Paso 6: Admin Configura Detalles**
```
Orden: [3]
Ruta: [Ruta Mañana - BUS-001]
Alumno: [Carlos López]
[✓] Parada Activa
```

**Paso 7: Hace Click en "Guardar"**

**Paso 7.1: Sistema Valida**
```javascript
✅ Dirección: OK
✅ Latitud: OK (14.6049)
✅ Longitud: OK (-90.4889)
✅ Orden: OK (3)
✅ Ruta: OK (1)
```

**Paso 7.2: Sistema Guarda en BD**
```sql
INSERT INTO genesis.Paradas 
(IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden, Activo)
VALUES 
(1,         -- Ruta Mañana
 3,         -- Carlos López
 14.6049,   -- Latitud
 -90.4889,  -- Longitud
 'Universidad Rafael Landívar, Vista Hermosa',
 3,         -- Tercera parada
 1          -- Activa
);
```

**Paso 7.3: Sistema Responde**
```
✅ Parada creada correctamente
```

**Paso 8: Modal se Cierra**

**Paso 9: Mapa se Actualiza**
```
[Mapa actualizado]
- Marcador azul: Parada 1 (Juan) - Orden 1
- Marcador azul: Parada 2 (María) - Orden 2
- Marcador azul: Parada 3 (Carlos) - Orden 3 ← ¡NUEVO!
- Marcador verde: Colegio - Orden 4
```

**Paso 10: Tabla se Actualiza**
```
📋 Paradas Registradas
┌────┬─────────────────────────────┬──────────┬───────────┬───────┬───────┬────────┐
│ ID │ Dirección                   │ Latitud  │ Longitud  │ Orden │ Ruta  │ Activo │
├────┼─────────────────────────────┼──────────┼───────────┼───────┼───────┼────────┤
│ 1  │ 5ta Avenida 12-34 Zona 10  │ 14.5908  │ -90.5518  │   1   │ RM-01 │   ✅   │
│ 2  │ Oakland Mall Zona 10        │ 14.5988  │ -90.5123  │   2   │ RM-01 │   ✅   │
│ 3  │ URL Vista Hermosa          │ 14.6049  │ -90.4889  │   3   │ RM-01 │   ✅   │ ← NUEVO
│ 4  │ Colegio Ejemplo Zona 10     │ 14.6100  │ -90.5000  │   4   │ RM-01 │   ✅   │
└────┴─────────────────────────────┴──────────┴───────────┴───────┴───────┴────────┘
```

✅ **Parada Creada Exitosamente**

---

## 11. Preguntas Frecuentes (FAQ)

### ❓ ¿Cómo sabe el sistema dónde vive el alumno?

**R**: El administrador ingresa la dirección al registrar al alumno. Puede:
1. Escribir la dirección y hacer clic en "Buscar en Mapa" (geocodificación automática)
2. Hacer clic directamente en el mapa en la ubicación
3. El sistema guarda las coordenadas (latitud, longitud) en la tabla `Alumnos`

---

### ❓ ¿El padre puede cambiar la dirección de su hijo?

**R**: No directamente. El padre debe:
1. Contactar al administrador
2. El admin actualiza la dirección
3. El admin actualiza la parada asociada

*Opción futura*: Permitir que el padre solicite cambio de dirección desde su perfil.

---

### ❓ ¿Qué pasa si el alumno no tiene dirección registrada?

**R**: 
- No se puede crear una parada para ese alumno
- El alumno no aparecerá en la ruta
- El sistema mostrará advertencia al admin
- El padre no podrá ver el bus en el mapa

**Solución**: Admin debe registrar la dirección del alumno.

---

### ❓ ¿Cómo se vincula un alumno con una parada?

**R**: Hay dos formas:

**Forma 1: Manual**
```sql
INSERT INTO Paradas (IdRuta, IdAlumno, Latitud, Longitud, ...)
VALUES (1, 3, 14.6049, -90.4889, ...);
				↑
			ID del Alumno
```

**Forma 2: Automática (recomendada)**
- Al crear parada desde la interfaz
- Seleccionar alumno de la lista desplegable
- Sistema vincula automáticamente

---

### ❓ ¿Puede un alumno tener múltiples paradas?

**R**: Técnicamente sí, pero generalmente:
- **Ruta de Ida** (mañana): Parada en casa del alumno
- **Ruta de Regreso** (tarde): Parada en la casa (misma ubicación)

Caso especial:
- Lunes a Viernes: Casa de mamá
- Solo viernes PM: Casa de papá (diferente dirección)

*Esto requeriría lógica adicional para seleccionar parada según día.*

---

### ❓ ¿Qué pasa si el GPS del bus falla?

**R**: 
1. Sistema detecta falta de actualización (> 2 minutos)
2. Marca bus como "Sin señal"
3. Envía alerta al administrador
4. Padres ven mensaje: "Bus sin señal GPS (última actualización: 7:15 AM)"
5. Se usa última ubicación conocida

---

### ❓ ¿Cómo se calcula el tiempo estimado de llegada (ETA)?

**R**: 
```
ETA = DistanciaRestante / VelocidadPromedio

Ejemplo:
- Distancia al siguiente punto: 2 km
- Velocidad promedio del bus: 30 km/h
- ETA = 2 / 30 = 0.0667 horas = 4 minutos
```

El sistema considera:
- Tráfico (basado en histórico)
- Paradas intermedias
- Hora del día

---

### ❓ ¿Se puede ver el historial de recorridos?

**R**: Sí. Roles con acceso:

**Administrador**:
- Ve todos los recorridos de todos los buses
- Filtrar por fecha, bus, ruta
- Exportar a Excel/PDF

**Padre**:
- Ve solo recorridos donde estuvo su hijo
- Últimos 30 días
- Puede ver mapa de recorrido pasado

---

### ❓ ¿Qué tan preciso es el sistema de geocodificación?

**R**: 
- **API Nominatim**: Precisión variable
  - Lugares conocidos: ✅ Muy precisa (< 10 metros)
  - Direcciones específicas: ⚠️ Buena (< 50 metros)
  - Direcciones vagas: ❌ Puede fallar

**Recomendación**:
- Siempre verificar en el mapa
- Si es impreciso, usar método de "Click en el Mapa"

---

## 12. Arquitectura Técnica

### 12.1 Stack Tecnológico

```
Frontend
├─→ Razor Pages (HTML + C#)
├─→ Bootstrap 5 (CSS)
├─→ Leaflet.js (Mapas)
├─→ SignalR Client (Tiempo Real)
└─→ JavaScript ES6+

Backend
├─→ ASP.NET Core 8 (C#)
├─→ Entity Framework Core (ORM)
├─→ SignalR (WebSockets)
├─→ Identity (Autenticación)
└─→ API REST Controllers

Base de Datos
├─→ SQL Server 2022
├─→ Schema: genesis
└─→ Tablas: 20+

APIs Externas
├─→ Nominatim (Geocodificación)
└─→ OpenStreetMap (Tiles del mapa)
```

---

### 12.2 Flujo de Datos en Tiempo Real

```
GPS del Bus (Navegador/Dispositivo)
	│
	├─→ JavaScript captura ubicación cada 10s
	│   navigator.geolocation.watchPosition()
	│
	└─→ HTTP PUT /api/buses/{id}/ubicacion
			│
			└─→ Controller actualiza BD
					│
					├─→ UPDATE Buses SET LatitudActual=..., LongitudActual=...
					│
					└─→ SignalR envía broadcast
							│
							├─→ Grupo "Administradores" (todos los buses)
							│
							└─→ Grupo "Padre_{IdPadre}" (solo su bus)
									│
									└─→ JavaScript del cliente recibe
											│
											└─→ Actualiza marcador en el mapa
```

---

## 13. Conclusión

### ✅ Resumen del Flujo Completo

```
1. REGISTRO
   Padre → Crea cuenta de login
   Admin → Registra Alumno con dirección (geocodifica)
   Admin → Crea Parada en esa dirección

2. CONFIGURACIÓN
   Admin → Crea Ruta
   Admin → Asigna Paradas a la Ruta
   Admin → Asigna Bus a la Ruta
   Admin → (Opcional) Optimiza orden con TSP

3. OPERACIÓN
   Padre → Confirma asistencia del alumno
   Piloto → Inicia recorrido
   Sistema → Rastrea bus en tiempo real
   Sistema → Calcula proximidad a paradas
   Sistema → Envía notificaciones
   Monitor → Marca asistencia

4. MONITOREO
   Padre → Ve bus en mapa
   Admin → Ve todos los buses
   Sistema → Genera reportes
```

---

### 📚 Documentos Relacionados

- `GEOCODIFICACION_PARADAS.md` - Guía de uso de geocodificación
- `DEBUG_GEOCODIFICACION.md` - Solución de problemas
- `FIX_GEOCODING_SCOPE.md` - Fix técnico de JavaScript
- `SOLUCION_FINAL_GEOCODIFICACION.md` - Solución implementada

---

**Última Actualización**: Mayo 2026  
**Versión**: 1.0  
**Autores**: David & Jonathan - Transportes Genesis  
**Estado**: ✅ Documentación Completa
