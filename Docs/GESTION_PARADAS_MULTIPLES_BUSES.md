# 🚌 Gestión de Paradas con Múltiples Buses

## 📋 ¿Cómo funciona con múltiples buses?

### 🔑 Conceptos Clave

La pantalla `/Admin/GestionarParadas` gestiona **todas las paradas del sistema**, independientemente del bus. Cada parada está asociada a una **ruta específica**, y cada ruta está asociada a un **bus específico**.

```
Bus #1 → Ruta "Mañana Zona 10" → Paradas (1, 2, 3, 4...)
Bus #2 → Ruta "Tarde Zona 15" → Paradas (5, 6, 7, 8...)
Bus #3 → Ruta "Mañana Centro" → Paradas (9, 10, 11...)
```

---

## 🗺️ Visualización en el Mapa

### Escenario con 3 Buses Activos

Cuando abres `/Admin/GestionarParadas`, el mapa muestra:

```
┌─────────────────────────────────────────────────┐
│  📍 Mapa de Todas las Paradas                   │
│                                                  │
│    🔴 Parada 1 (Bus #1 - Zona 10)              │
│    🔴 Parada 2 (Bus #1 - Zona 10)              │
│    🔴 Parada 3 (Bus #1 - Zona 10)              │
│                                                  │
│    🔵 Parada 4 (Bus #2 - Zona 15)              │
│    🔵 Parada 5 (Bus #2 - Zona 15)              │
│                                                  │
│    🟢 Parada 6 (Bus #3 - Centro)               │
│    🟢 Parada 7 (Bus #3 - Centro)               │
│                                                  │
└─────────────────────────────────────────────────┘
```

**Todos los marcadores se muestran juntos** en el mismo mapa.

---

## 📊 Tabla de Paradas

La tabla muestra todas las paradas con información de su ruta:

| ID | Dirección | Latitud | Longitud | Orden | Ruta | Bus | Activo |
|----|-----------|---------|----------|-------|------|-----|--------|
| 1  | 5ta Av 12-34 | 14.6234 | -90.5123 | 1 | Mañana Zona 10 | Bus #1 | ✅ Activo |
| 2  | 7ma Calle 8-45 | 14.6345 | -90.5234 | 2 | Mañana Zona 10 | Bus #1 | ✅ Activo |
| 3  | 10ma Av 15-20 | 14.6456 | -90.5345 | 3 | Mañana Zona 10 | Bus #1 | ✅ Activo |
| 4  | Blvd Los Próceres | 14.5678 | -90.4567 | 1 | Tarde Zona 15 | Bus #2 | ✅ Activo |
| 5  | 12 Calle 3-45 | 14.5789 | -90.4678 | 2 | Tarde Zona 15 | Bus #2 | ✅ Activo |

---

## 🎨 Mejoras Sugeridas para Diferenciar Buses

### Opción 1: Filtros por Ruta/Bus

Agregar controles de filtro en la parte superior:

```html
<div class="filtros mb-3">
    <select id="filtro-bus" class="form-select">
        <option value="">Todos los buses</option>
        <option value="1">Bus #1 - Placa ABC123</option>
        <option value="2">Bus #2 - Placa DEF456</option>
        <option value="3">Bus #3 - Placa GHI789</option>
    </select>

    <select id="filtro-ruta" class="form-select">
        <option value="">Todas las rutas</option>
        <option value="1">Mañana Zona 10</option>
        <option value="2">Tarde Zona 15</option>
        <option value="3">Mañana Centro</option>
    </select>
</div>
```

**JavaScript para filtrar:**
```javascript
document.getElementById('filtro-bus').addEventListener('change', function() {
    const idBus = this.value;
    filtrarParadasPorBus(idBus);
});

function filtrarParadasPorBus(idBus) {
    // Ocultar/mostrar marcadores según el bus seleccionado
    marcadores.forEach(marcador => {
        if (!idBus || marcador.idBus == idBus) {
            marcador.addTo(mapa);
        } else {
            mapa.removeLayer(marcador);
        }
    });
}
```

---

### Opción 2: Colores Diferentes por Bus

Usar iconos de colores distintos para cada bus:

```javascript
function agregarMarcador(parada) {
    // Determinar color según el bus
    const colores = {
        1: 'red',
        2: 'blue',
        3: 'green',
        4: 'orange',
        5: 'purple'
    };

    const color = colores[parada.idBus] || 'gray';

    const icono = L.icon({
        iconUrl: `https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-${color}.png`,
        shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-shadow.png',
        iconSize: [25, 41],
        iconAnchor: [12, 41],
        popupAnchor: [1, -34],
        shadowSize: [41, 41]
    });

    const marcador = L.marker([parada.latitud, parada.longitud], {
        icon: icono,
        draggable: true
    }).addTo(mapa);

    // Guardar referencia del bus
    marcador.idBus = parada.ruta.idBus;
}
```

---

### Opción 3: Agrupación por Capas (Layer Groups)

Usar capas de Leaflet para agrupar paradas por bus:

```javascript
const capas = {};

function cargarParadas() {
    // Limpiar capas anteriores
    Object.values(capas).forEach(capa => mapa.removeLayer(capa));

    // Crear capas por bus
    paradas.forEach(parada => {
        const idBus = parada.ruta.idBus;

        if (!capas[idBus]) {
            capas[idBus] = L.layerGroup().addTo(mapa);
        }

        const marcador = L.marker([parada.latitud, parada.longitud]);
        marcador.addTo(capas[idBus]);
    });

    // Agregar control de capas
    L.control.layers(null, {
        'Bus #1': capas[1],
        'Bus #2': capas[2],
        'Bus #3': capas[3]
    }).addTo(mapa);
}
```

---

## 🔧 Implementación Recomendada

### Paso 1: Agregar Campo `NombreRuta` a la Tabla

Ya está incluido en el DTO `ParadaDto.cs`:
```csharp
public string? NombreRuta { get; set; }
```

### Paso 2: Mostrar Información de la Ruta en la Tabla

Modificar `gestionarParadas.js`:
```javascript
function actualizarTablaParadas(paradas) {
    paradas.forEach(parada => {
        const row = `
            <tr>
                <td>${parada.idParada}</td>
                <td>${parada.direccion || `Parada #${parada.idParada}`}</td>
                <td>${parseFloat(parada.latitud).toFixed(6)}</td>
                <td>${parseFloat(parada.longitud).toFixed(6)}</td>
                <td>${parada.orden || 'N/A'}</td>
                <td><span class="badge bg-info">${parada.nombreRuta || 'Sin ruta'}</span></td>
                <td>${parada.activo === 1 ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-secondary">Inactivo</span>'}</td>
                <td>...</td>
            </tr>
        `;
    });
}
```

### Paso 3: Agregar Filtros (Opcional)

Si tienes muchas paradas (>50), agregar filtros es recomendable.

---

## 📝 Respuesta a tu Pregunta

> **"Si son varios buses, ¿cómo se vería esta pantalla o cómo se gestionan?"**

### Respuesta:

1. **Vista Unificada:** Todas las paradas de todos los buses se muestran en el mismo mapa y tabla.

2. **Diferenciación:** Se puede diferenciar mediante:
   - Columna "Ruta" en la tabla
   - Colores distintos en los marcadores
   - Filtros dropdown para mostrar solo un bus/ruta

3. **Gestión Individual:** Cada parada se edita/elimina individualmente, sin importar a qué bus pertenezca.

4. **Creación:** Al crear una parada, debes especificar a qué ruta pertenece (y esa ruta está asociada a un bus).

---

## 🎨 Ejemplo Visual de Implementación con Colores

```
Leyenda:
🔴 Bus #1 (Placa ABC123) - Ruta Mañana Zona 10
🔵 Bus #2 (Placa DEF456) - Ruta Tarde Zona 15
🟢 Bus #3 (Placa GHI789) - Ruta Mañana Centro

Mapa:
    🔴 ← Parada "5ta Avenida"
    🔴 ← Parada "7ma Calle"
    🔵 ← Parada "Blvd Los Próceres"
    🟢 ← Parada "Centro Histórico"
    🔴 ← Parada "Zona 10 Mall"
```

---

## ✨ Conclusión

**La pantalla actual funciona para múltiples buses**, pero se puede mejorar agregando:
1. ✅ Columna de "Ruta" en la tabla (ya incluido en el DTO)
2. 🎨 Colores diferentes por bus (mejora visual)
3. 🔍 Filtros por bus/ruta (si hay muchas paradas)

**No necesitas pantallas separadas** por cada bus, la gestión centralizada es más eficiente.