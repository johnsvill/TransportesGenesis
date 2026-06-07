# ✅ Solución al Error 500 en `/api/paradas`

## 🔴 Problema Detectado

Al cargar la pantalla `/Admin/GestionarParadas`, la tabla de paradas se quedaba "cargando..." indefinidamente y la consola del navegador mostraba:

```
GET https://localhost:7241/api/paradas 500 (Internal Server Error)
Error al cargar paradas: Error: Error al cargar paradas
```

---

## 🔍 Diagnóstico

### Causa Raíz: Ciclo de Serialización JSON

El endpoint `/api/paradas` realizaba la consulta correctamente:

```csharp
var paradas = await _context.ParadasDb
    .Include(p => p.Ruta)
    .Include(p => p.Alumno)
    .OrderBy(p => p.Orden)
    .ToListAsync();

return Ok(paradas); // ❌ Esto causaba el error
```

**Problema:** Entity Framework crea navegaciones bidireccionales:

```
Parada → Ruta → ParadasLink (ICollection<Parada>) → Ruta → ParadasLink...
```

Esto provoca un **ciclo infinito** al serializar a JSON, generando:

```
System.Text.Json.JsonException: A possible object cycle was detected.
Path: $.Ruta.ParadasLink.Ruta.ParadasLink.Ruta.ParadasLink...
```

---

## ✅ Solución Implementada

### Paso 1: Crear DTO (Data Transfer Object)

Se creó `DTOs/Paradas/ParadaDto.cs`:

```csharp
namespace TransportesGenesis.DTOs.Paradas
{
    public class ParadaDto
    {
        public int IdParada { get; set; }
        public int IdRuta { get; set; }
        public int? IdAlumno { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public string? Direccion { get; set; }
        public int Orden { get; set; }
        public TimeSpan? HoraEstimada { get; set; }
        public bool Completada { get; set; }
        public int Activo { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Información adicional sin navegaciones circulares
        public string? NombreRuta { get; set; }
        public string? NombreAlumno { get; set; }
    }
}
```

**Ventajas:**
- ✅ No tiene navegaciones circulares
- ✅ Solo incluye las propiedades necesarias para la UI
- ✅ Agrega información calculada (`NombreRuta`, `NombreAlumno`)

---

### Paso 2: Refactorizar el Controller

Se actualizó `Controllers/Api/ParadasController.cs`:

**ANTES:**
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<Parada>>> GetParadas()
{
    var paradas = await _context.ParadasDb
        .Include(p => p.Ruta)
        .Include(p => p.Alumno)
        .OrderBy(p => p.Orden)
        .ToListAsync();

    return Ok(paradas); // ❌ Ciclo JSON
}
```

**DESPUÉS:**
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<ParadaDto>>> GetParadas()
{
    var paradas = await _context.ParadasDb
        .Include(p => p.Ruta)
        .Include(p => p.Alumno)
        .OrderBy(p => p.Orden)
        .Select(p => new ParadaDto
        {
            IdParada = p.IdParada,
            IdRuta = p.IdRuta,
            IdAlumno = p.IdAlumno,
            Latitud = p.Latitud,
            Longitud = p.Longitud,
            Direccion = p.Direccion,
            Orden = p.Orden,
            HoraEstimada = p.HoraEstimada,
            Completada = p.Completada,
            Activo = p.Activo,
            FechaRegistro = p.FechaRegistro,
            NombreRuta = p.Ruta != null ? p.Ruta.Nombre : null,
            NombreAlumno = p.Alumno != null ? p.Alumno.Nombre + " " + p.Alumno.Apellido : null
        })
        .ToListAsync();

    return Ok(paradas); // ✅ Sin ciclos
}
```

**Nota importante:** Se usa `.Select()` para proyectar directamente a DTO **antes** de `.ToListAsync()`. Esto evita que EF materialice las navegaciones circulares.

---

### Paso 3: Actualizar UI para Mostrar Información de Ruta

Se actualizó la tabla HTML para incluir la columna "Ruta":

**`Pages/Admin/GestionarParadas.cshtml`:**
```html
<thead>
    <tr>
        <th>ID</th>
        <th>Dirección</th>
        <th>Latitud</th>
        <th>Longitud</th>
        <th>Orden</th>
        <th>Ruta</th> <!-- ✅ Nueva columna -->
        <th>Activo</th>
        <th>Acciones</th>
    </tr>
</thead>
```

**`wwwroot/js/gestionarParadas.js`:**
```javascript
function actualizarTablaParadas(paradas) {
    paradas.forEach(parada => {
        const nombreRuta = parada.nombreRuta || 'Sin ruta';
        const row = `
            <tr>
                <td>${parada.idParada}</td>
                <td>${parada.direccion || `Parada #${parada.idParada}`}</td>
                <td>${parseFloat(parada.latitud).toFixed(6)}</td>
                <td>${parseFloat(parada.longitud).toFixed(6)}</td>
                <td>${parada.orden || 'N/A'}</td>
                <td><span class="badge bg-info">${nombreRuta}</span></td> <!-- ✅ -->
                <td>${parada.activo === 1 ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-secondary">Inactivo</span>'}</td>
                <td>...</td>
            </tr>
        `;
    });
}
```

También se actualizó el popup del mapa:
```javascript
const popupContent = `
    <div>
        <b>${nombre}</b><br>
        <small><i class="bi bi-geo-alt"></i> Lat: ${lat}, Lng: ${lng}</small><br>
        <small><i class="bi bi-arrow-down-up"></i> Orden: ${orden}</small><br>
        <small><i class="bi bi-bus-front"></i> Ruta: <span class="badge bg-info">${nombreRuta}</span></small><br>
        <small>Estado: ${activo}</small>
        ...
    </div>
`;
```

---

## 🎯 Resultado Final

### ✅ Antes (Error)
```
GET /api/paradas → 500 Internal Server Error
JsonException: A possible object cycle was detected
```

### ✅ Después (Correcto)
```
GET /api/paradas → 200 OK
[
    {
        "idParada": 1,
        "idRuta": 1,
        "latitud": 14.6234,
        "longitud": -90.5123,
        "direccion": "5ta Avenida 12-34",
        "orden": 1,
        "activo": 1,
        "nombreRuta": "Mañana Zona 10",  ← ✅ Sin ciclos
        "nombreAlumno": "Juan Pérez"
    },
    ...
]
```

---

## 📊 Mejoras Adicionales Implementadas

### 1. Columna "Ruta" en la Tabla
Ahora la tabla muestra a qué ruta pertenece cada parada:

| ID | Dirección | Latitud | Longitud | Orden | **Ruta** | Activo |
|----|-----------|---------|----------|-------|---------|--------|
| 1 | 5ta Av 12-34 | 14.6234 | -90.5123 | 1 | **Mañana Zona 10** | ✅ |
| 2 | 7ma Calle 8-45 | 14.6345 | -90.5234 | 2 | **Mañana Zona 10** | ✅ |

### 2. Información de Ruta en el Popup del Mapa
Los marcadores ahora muestran la ruta asociada en el popup.

### 3. Preparación para Múltiples Buses
Con la columna "Ruta" visible, es fácil distinguir paradas de diferentes buses.

---

## 📚 Lecciones Aprendidas

### ❌ Anti-Pattern: Devolver Entidades EF Directamente
```csharp
// NUNCA hacer esto con entidades que tienen navegaciones
return Ok(await _context.ParadasDb.Include(x => x.Ruta).ToListAsync());
```

**Problemas:**
- Ciclos de serialización JSON
- Expone toda la estructura interna de la base de datos
- Riesgo de over-fetching

### ✅ Best Practice: Usar DTOs
```csharp
// SIEMPRE proyectar a DTOs
return Ok(await _context.ParadasDb
    .Select(p => new ParadaDto { ... })
    .ToListAsync());
```

**Ventajas:**
- Control total sobre lo que se serializa
- No hay ciclos
- API más limpia y mantenible
- Mejor rendimiento (solo se transfieren datos necesarios)

---

## 🔧 Archivos Modificados

1. ✅ **Creado:** `DTOs/Paradas/ParadaDto.cs`
2. ✅ **Modificado:** `Controllers/Api/ParadasController.cs`
3. ✅ **Modificado:** `Pages/Admin/GestionarParadas.cshtml`
4. ✅ **Modificado:** `wwwroot/js/gestionarParadas.js`

---

## ✅ Estado Actual

- ✅ Compilación exitosa
- ✅ API `/api/paradas` devuelve 200 OK
- ✅ Tabla de paradas carga correctamente
- ✅ Mapa muestra marcadores con información completa
- ✅ Columna "Ruta" agregada
- ✅ Sin ciclos JSON
- ✅ Preparado para múltiples buses

---

## 📖 Documentación Relacionada

- Ver: `Docs/GESTION_PARADAS_MULTIPLES_BUSES.md` para entender cómo funciona con varios buses
- Ver: `Docs/PENDIENTE_MAPA_INTERACTIVO.md` (ahora marcado como ✅ COMPLETADO)