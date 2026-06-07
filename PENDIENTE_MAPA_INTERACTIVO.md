# 🗺️ Mapa Interactivo de Paradas - ✅ COMPLETADO (100%)

## 📋 Estado Actual

### ✅ Completado (100%)
- Visualización de mapa en dashboards Piloto/Monitor
- Ubicación en tiempo real del bus
- Paradas mostradas en el mapa (marcadores fijos)
- Integración con base de datos `genesis.Paradas`
- Visualización de rutas activas
- **✅ Editor Interactivo de Paradas** (Recién implementado)
  - Página `/Admin/GestionarParadas` con mapa Leaflet
  - Crear paradas haciendo clic en el mapa
  - Editar paradas existentes (drag-and-drop y formulario)
  - Eliminar paradas (soft delete)
  - API REST completa en `/api/paradas`
  - Visualización en tabla con filtros
  - Integración con base de datos real

---

## 🎉 Funcionalidad Implementada

### 1. **Página Administrativa**: `/Admin/GestionarParadas`
- Mapa interactivo con Leaflet
- Tabla de paradas registradas
- Panel de control con botones "Agregar Parada" y "Refrescar"

### 2. **API REST**: `/api/paradas`
- `GET /api/paradas` - Listar todas las paradas
- `GET /api/paradas/{id}` - Obtener una parada
- `POST /api/paradas` - Crear nueva parada
- `PUT /api/paradas/{id}` - Actualizar parada
- `DELETE /api/paradas/{id}` - Eliminar (soft delete) parada

### 3. **Funcionalidades del Mapa**
- **Agregar**: Click en "Agregar Parada" → Click en el mapa → Modal con coordenadas capturadas
- **Editar**: Click en marcador o botón de tabla → Modal con datos precargados
- **Mover**: Drag-and-drop de marcadores → Actualización automática de coordenadas
- **Eliminar**: Botón eliminar → Confirmación → Soft delete (marca como inactivo)

### 4. **Archivos Creados**
```
Pages/Admin/GestionarParadas.cshtml       # Vista Razor con mapa
Pages/Admin/GestionarParadas.cshtml.cs    # PageModel
Controllers/Api/ParadasController.cs       # API REST
wwwroot/js/gestionarParadas.js            # Lógica JavaScript del mapa
```

### 5. **Tarjeta en Dashboard Admin**
- Agregada tarjeta "📍 Gestionar Paradas" en `Views/Admin/Index.cshtml`
- Enlace directo: `/Admin/GestionarParadas`

---

## 🔧 Detalles Técnicos

### Base de Datos
- Tabla: `genesis.Paradas`
- Campos:
  - `IdParada` (PK)
  - `IdRuta` (FK) - Relación con ruta
  - `IdAlumno` (FK, nullable) - Alumno asociado
  - `Latitud`, `Longitud` (decimal 10,7)
  - `Direccion` (string 250)
  - `Orden` (int) - Orden en la ruta
  - `HoraEstimada` (TimeSpan?)
  - `Completada` (bool)
  - `Activo` (int - 1/0) - Soft delete
  - `FechaRegistro` (DateTime) - Heredad de `Auditoria`

### Seguridad
- Toda la funcionalidad está protegida con `[Authorize(Roles = "Administrador")]`
- Solo usuarios con rol Administrador pueden acceder

### Características Especiales
- **Soft Delete**: Las paradas no se eliminan físicamente, se marcan como inactivas (`Activo = 0`)
- **Captura Automática de Coordenadas**: Al hacer clic en el mapa, se capturan lat/lng automáticamente
- **Validación**: No permite crear paradas sin dirección
- **Drag-and-Drop**: Los marcadores son movibles y actualizan automáticamente la BD

---

## ✨ Próximos Pasos Opcionales (Mejoras Futuras)

1. **Filtros Avanzados**
   - Filtrar paradas por ruta
   - Filtrar paradas activas/inactivas
   - Búsqueda por dirección

2. **Asignación Automática**
   - Sugerir paradas cercanas al crear una nueva
   - Geocodificación inversa (dirección desde coordenadas)

3. **Historial de Cambios**
   - Auditoría de movimientos de paradas
   - Registro de quién modificó qué

4. **Importación Masiva**
   - Cargar paradas desde archivo CSV/Excel
   - Exportar paradas a Excel

---

## 📝 Conclusión

El **Mapa Interactivo de Paradas** está ahora **100% funcional** y cumple con todos los requisitos iniciales. Los administradores pueden gestionar paradas de manera visual e intuitiva desde `/Admin/GestionarParadas`.

**Fecha de Finalización**: 2025-01-XX  
**Estado**: ✅ **COMPLETADO**
                            <input type="number" class="form-control" id="parada-lat" step="0.000001" readonly />
                        </div>
                        <div class="col-md-6 mb-3">
                            <label for="parada-lng" class="form-label">Longitud</label>
                            <input type="number" class="form-control" id="parada-lng" step="0.000001" readonly />
                        </div>
                    </div>
                    <div class="mb-3">
                        <label for="parada-orden" class="form-label">Orden</label>
                        <input type="number" class="form-control" id="parada-orden" />
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                <button type="button" class="btn btn-primary" id="btn-guardar-parada">Guardar</button>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script src="~/js/gestionarParadas.js"></script>
}
```

---

### JavaScript (gestionarParadas.js)

```javascript
let mapa;
let marcadores = [];
let paradaEnEdicion = null;
let modoAgregar = false;

// Inicializar mapa
function inicializarMapa() {
    mapa = L.map('mapa-paradas').setView([14.6349, -90.5069], 13); // Guatemala

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
    }).addTo(mapa);

    // Cargar paradas existentes
    cargarParadas();

    // Evento click en el mapa (cuando está en modo agregar)
    mapa.on('click', function(e) {
        if (modoAgregar) {
            agregarParadaTemporal(e.latlng);
        }
    });
}

// Cargar paradas desde la BD
async function cargarParadas() {
    try {
        const response = await fetch('/api/paradas/obtener-todas');
        const paradas = await response.json();

        paradas.forEach(parada => {
            agregarMarcador(parada);
        });

        actualizarTablaParadas(paradas);
    } catch (error) {
        console.error('Error al cargar paradas:', error);
    }
}

// Agregar marcador al mapa
function agregarMarcador(parada) {
    const marcador = L.marker([parada.latitud, parada.longitud], {
        draggable: true,
        icon: L.icon({
            iconUrl: '/images/parada-icon.png',
            iconSize: [32, 32]
        })
    }).addTo(mapa);

    marcador.paradaId = parada.idParada;

    // Popup con información
    marcador.bindPopup(`
        <b>${parada.nombre}</b><br>
        Orden: ${parada.orden}<br>
        <button onclick="editarParada(${parada.idParada})" class="btn btn-sm btn-primary">Editar</button>
        <button onclick="eliminarParada(${parada.idParada})" class="btn btn-sm btn-danger">Eliminar</button>
    `);

    // Evento drag
    marcador.on('dragend', function(e) {
        const nuevaPos = e.target.getLatLng();
        actualizarCoordenadas(parada.idParada, nuevaPos.lat, nuevaPos.lng);
    });

    marcadores.push(marcador);
}

// Agregar parada temporal (nueva)
function agregarParadaTemporal(latlng) {
    document.getElementById('parada-id').value = '';
    document.getElementById('parada-nombre').value = '';
    document.getElementById('parada-lat').value = latlng.lat;
    document.getElementById('parada-lng').value = latlng.lng;
    document.getElementById('parada-orden').value = marcadores.length + 1;

    $('#modal-editar-parada').modal('show');
    modoAgregar = false;
    document.getElementById('btn-agregar-parada').classList.remove('active');
}

// Activar modo agregar
document.getElementById('btn-agregar-parada').addEventListener('click', function() {
    modoAgregar = !modoAgregar;
    this.classList.toggle('active');

    if (modoAgregar) {
        mapa.getContainer().style.cursor = 'crosshair';
        alert('Haz clic en el mapa para agregar una parada');
    } else {
        mapa.getContainer().style.cursor = '';
    }
});

// Guardar parada (crear o actualizar)
document.getElementById('btn-guardar-parada').addEventListener('click', async function() {
    const parada = {
        idParada: document.getElementById('parada-id').value || null,
        nombre: document.getElementById('parada-nombre').value,
        latitud: parseFloat(document.getElementById('parada-lat').value),
        longitud: parseFloat(document.getElementById('parada-lng').value),
        orden: parseInt(document.getElementById('parada-orden').value)
    };

    try {
        const url = parada.idParada ? '/api/paradas/actualizar' : '/api/paradas/crear';
        const method = 'POST';

        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(parada)
        });

        if (response.ok) {
            $('#modal-editar-parada').modal('hide');
            location.reload(); // Recargar para actualizar el mapa
        } else {
            alert('Error al guardar la parada');
        }
    } catch (error) {
        console.error('Error:', error);
    }
});

// Editar parada
function editarParada(idParada) {
    fetch(`/api/paradas/obtener/${idParada}`)
        .then(response => response.json())
        .then(parada => {
            document.getElementById('parada-id').value = parada.idParada;
            document.getElementById('parada-nombre').value = parada.nombre;
            document.getElementById('parada-lat').value = parada.latitud;
            document.getElementById('parada-lng').value = parada.longitud;
            document.getElementById('parada-orden').value = parada.orden;

            $('#modal-editar-parada').modal('show');
        });
}

// Eliminar parada
async function eliminarParada(idParada) {
    if (!confirm('¿Está seguro de eliminar esta parada?')) return;

    try {
        const response = await fetch(`/api/paradas/eliminar/${idParada}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            location.reload();
        } else {
            alert('Error al eliminar la parada');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

// Actualizar coordenadas después de drag
async function actualizarCoordenadas(idParada, lat, lng) {
    try {
        await fetch('/api/paradas/actualizar-coordenadas', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ idParada, latitud: lat, longitud: lng })
        });
    } catch (error) {
        console.error('Error al actualizar coordenadas:', error);
    }
}

// Inicializar al cargar la página
document.addEventListener('DOMContentLoaded', inicializarMapa);
```

---

### Backend (API Controller)

#### ParadasApiController.cs

```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.Models.DB.Negocio;

namespace TransportesGenesis.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/paradas")]
    [ApiController]
    public class ParadasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ParadasApiController> _logger;

        public ParadasApiController(ApplicationDbContext context, ILogger<ParadasApiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("obtener-todas")]
        public async Task<IActionResult> ObtenerTodas()
        {
            var paradas = await _context.ParadasDb.ToListAsync();
            return Ok(paradas);
        }

        [HttpGet("obtener/{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var parada = await _context.ParadasDb.FindAsync(id);
            if (parada == null)
                return NotFound();

            return Ok(parada);
        }

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] Parada parada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            parada.FechaRegistro = DateTime.Now;
            parada.Activo = 1;

            _context.ParadasDb.Add(parada);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Parada creada: {parada.Nombre} (Lat: {parada.Latitud}, Lng: {parada.Longitud})");

            return CreatedAtAction(nameof(ObtenerPorId), new { id = parada.IdParada }, parada);
        }

        [HttpPost("actualizar")]
        public async Task<IActionResult> Actualizar([FromBody] Parada parada)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paradaExistente = await _context.ParadasDb.FindAsync(parada.IdParada);
            if (paradaExistente == null)
                return NotFound();

            paradaExistente.Nombre = parada.Nombre;
            paradaExistente.Latitud = parada.Latitud;
            paradaExistente.Longitud = parada.Longitud;
            paradaExistente.Orden = parada.Orden;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Parada actualizada: {parada.Nombre}");

            return Ok(paradaExistente);
        }

        [HttpPost("actualizar-coordenadas")]
        public async Task<IActionResult> ActualizarCoordenadas([FromBody] ActualizarCoordenadasRequest request)
        {
            var parada = await _context.ParadasDb.FindAsync(request.IdParada);
            if (parada == null)
                return NotFound();

            parada.Latitud = request.Latitud;
            parada.Longitud = request.Longitud;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var parada = await _context.ParadasDb.FindAsync(id);
            if (parada == null)
                return NotFound();

            // Verificar si la parada está en uso
            var estaEnUso = await _context.RutasParadasLinkDb.AnyAsync(rp => rp.IdParada == id);
            if (estaEnUso)
                return BadRequest(new { mensaje = "No se puede eliminar la parada porque está asociada a una ruta activa." });

            _context.ParadasDb.Remove(parada);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Parada eliminada: {parada.Nombre}");

            return Ok();
        }

        public class ActualizarCoordenadasRequest
        {
            public int IdParada { get; set; }
            public double Latitud { get; set; }
            public double Longitud { get; set; }
        }
    }
}
```

---

## 📦 Recursos Necesarios

### Imágenes
- `wwwroot/images/parada-icon.png` (ícono de parada)
- `wwwroot/images/parada-icon-selected.png` (ícono de parada seleccionada)

### Librerías JavaScript
- **Leaflet.js** (ya integrado)
- **Bootstrap 5** (ya integrado)

---

## ✅ Criterios de Aceptación

1. ✅ El administrador puede hacer clic en el mapa para agregar una nueva parada
2. ✅ Las coordenadas lat/lng se capturan automáticamente
3. ✅ Se muestra un modal para ingresar nombre y orden de la parada
4. ✅ Las paradas existentes son arrastrables (drag-and-drop)
5. ✅ Al arrastrar una parada, las coordenadas se actualizan en la BD
6. ✅ Al hacer clic en un marcador, se puede editar o eliminar la parada
7. ✅ No se pueden eliminar paradas asociadas a rutas activas
8. ✅ La tabla de paradas se actualiza en tiempo real

---

## 🎨 UI/UX Recomendaciones

1. **Botón "Agregar Parada":** Cambiar color a verde cuando está activo
2. **Cursor:** Cambiar a crosshair cuando está en modo agregar
3. **Confirmación:** Diálogo de confirmación al eliminar paradas
4. **Feedback visual:** Animación al guardar/actualizar paradas
5. **Validaciones:** Campos requeridos marcados en rojo si están vacíos

---

## 📝 Notas Técnicas

- **Seguridad:** Todos los endpoints de la API deben estar protegidos con `[Authorize(Roles = "Administrador")]`
- **Validaciones:** Verificar que las coordenadas estén dentro de rangos válidos (lat: -90 a 90, lng: -180 a 180)
- **Performance:** Considerar paginación si hay más de 100 paradas
- **Responsividad:** El mapa debe ajustarse en dispositivos móviles

---

## 🚀 Estimación de Tiempo

| Tarea | Tiempo Estimado |
|-------|-----------------|
| Crear página `/Admin/GestionarParadas` | 2 horas |
| Implementar JavaScript del mapa interactivo | 4 horas |
| Crear API Controller `ParadasApiController` | 2 horas |
| Testing y correcciones | 2 horas |
| **Total** | **10 horas** |

---

**Prioridad:** 🔴 Alta  
**Impacto:** Alto (mejora significativa la experiencia del administrador)  
**Complejidad:** Media

---

**Última actualización:** 2025-01-XX
