# 🔧 Solución al Error: Invalid LatLng object (NaN, NaN)

## 🔴 Problema Detectado

Al cargar la pantalla `/Admin/GestionarParadas`, la consola del navegador mostraba:

```
Error al cargar paradas: Error: Invalid LatLng object: (NaN, NaN)
    at new v (LatLng.js:32:9)
    at w (LatLng.js:123:11)
    at e.initialize (Marker.js:112:18)
```

---

## 🔍 Diagnóstico

### Causa Raíz: Inconsistencia en Naming Convention de JSON

El problema era una **discrepancia entre el formato JSON del servidor y el esperado por JavaScript**:

**Servidor (.NET):**
```json
{
  "Latitud": 14.6234,    // ❌ PascalCase
  "Longitud": -90.5123   // ❌ PascalCase
}
```

**JavaScript esperaba:**
```javascript
parada.latitud  // ❌ undefined (porque la propiedad es "Latitud")
parada.longitud // ❌ undefined (porque la propiedad es "Longitud")
```

Al intentar convertir `undefined` a número:
```javascript
parseFloat(undefined) // → NaN
Number(undefined)     // → NaN
```

Leaflet recibía `L.marker([NaN, NaN])` y lanzaba el error.

---

## ✅ Solución Implementada

### Paso 1: Cambiar JSON Serialization a camelCase

Se modificó `Startup.cs` para usar **camelCase** en las APIs:

**ANTES:**
```csharp
services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // ❌ PascalCase
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
```

**DESPUÉS:**
```csharp
services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase; // ✅ camelCase
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
```

**Ahora la API devuelve:**
```json
{
  "idParada": 1,
  "latitud": 14.6234,      // ✅ camelCase
  "longitud": -90.5123,    // ✅ camelCase
  "direccion": "5ta Av",
  "nombreRuta": "Mañana Zona 10"
}
```

---

### Paso 2: Agregar Validación en JavaScript

Se mejoró `gestionarParadas.js` para validar coordenadas antes de crear marcadores:

**En `cargarParadas()`:**
```javascript
async function cargarParadas() {
    try {
        const response = await fetch('/api/paradas');
        const paradas = await response.json();

        console.log('Paradas recibidas:', paradas); // ✅ Debug

        paradas.forEach(parada => {
            // ✅ Validar coordenadas antes de agregar
            const lat = parseFloat(parada.latitud);
            const lng = parseFloat(parada.longitud);

            if (isNaN(lat) || isNaN(lng)) {
                console.error(`Parada ${parada.idParada} tiene coordenadas inválidas:`, parada);
                return; // Saltar esta parada
            }

            agregarMarcador(parada);
        });

        // ...
    }
}
```

**En `agregarMarcador()`:**
```javascript
function agregarMarcador(parada) {
    // ✅ Convertir y validar coordenadas
    const lat = Number(parada.latitud);
    const lng = Number(parada.longitud);

    console.log(`Agregando parada ${parada.idParada}:`, { 
        lat, 
        lng, 
        original: { 
            latitud: parada.latitud, 
            longitud: parada.longitud 
        } 
    });

    // ✅ Verificación adicional
    if (isNaN(lat) || isNaN(lng) || lat === 0 || lng === 0) {
        console.error(`Coordenadas inválidas para parada ${parada.idParada}:`, parada);
        return;
    }

    const marcador = L.marker([lat, lng], {
        draggable: true
    }).addTo(mapa);

    // ...
}
```

---

## 🎯 Resultado Final

### ✅ Antes (Error)
```
GET /api/paradas → 200 OK
{
  "Latitud": 14.6234,  // ❌ PascalCase
  "Longitud": -90.5123
}

JavaScript:
parada.latitud → undefined → NaN → ❌ Error
```

### ✅ Después (Correcto)
```
GET /api/paradas → 200 OK
{
  "latitud": 14.6234,   // ✅ camelCase
  "longitud": -90.5123
}

JavaScript:
parada.latitud → 14.6234 → parseFloat → 14.6234 → ✅ Marcador creado
```

---

## 🔍 Script de Diagnóstico SQL

Se creó `Scripts/VerificarParadas.sql` para verificar los datos en la base de datos:

```sql
-- Ver todas las paradas con sus coordenadas
SELECT 
    IdParada,
    Latitud,
    Longitud,
    Direccion,
    Orden,
    Activo
FROM genesis.Paradas
ORDER BY Orden;

-- Verificar tipos de datos
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    NUMERIC_PRECISION,
    NUMERIC_SCALE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'genesis' 
  AND TABLE_NAME = 'Paradas'
  AND COLUMN_NAME IN ('Latitud', 'Longitud');
```

---

## 📚 Conceptos Clave

### camelCase vs PascalCase en APIs

**PascalCase (C# convención):**
```csharp
public class ParadaDto
{
    public int IdParada { get; set; }
    public decimal Latitud { get; set; }  // PascalCase
}
```

**JSON serializado (camelCase para JavaScript):**
```json
{
  "idParada": 1,
  "latitud": 14.6234  // camelCase
}
```

**Configuración recomendada:**
```csharp
.AddJsonOptions(options =>
{
    // ✅ Para APIs consumidas por JavaScript
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

    // ✅ Permite que el servidor acepte ambos formatos (envío del cliente)
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});
```

---

## ⚠️ Consideraciones Importantes

### 1. Hot Reload
Si estás debuggeando la aplicación:
- **Opción A:** Detener y reiniciar el debugger
- **Opción B:** Usar Hot Reload (puede no aplicar cambios en `Startup.cs`)

### 2. Otras APIs
Este cambio afecta **todas las APIs** del proyecto. Si otras APIs esperaban PascalCase, deberás:
- Actualizar el JavaScript correspondiente a camelCase, o
- Usar `[JsonPropertyName("NombreEspecifico")]` en propiedades específicas

### 3. Validación de Datos
Siempre valida coordenadas antes de usarlas:
```javascript
const lat = Number(valor);
if (isNaN(lat) || lat === 0) {
    console.error('Coordenada inválida');
    return;
}
```

---

## ✅ Estado Final

- ✅ JSON serializado en camelCase
- ✅ Validación de coordenadas en JavaScript
- ✅ Logging de diagnóstico agregado
- ✅ Script SQL para verificar datos
- ✅ Compilación exitosa

**Pasos para probar:**
1. Detén el debugger
2. Reinicia la aplicación
3. Abre `/Admin/GestionarParadas`
4. Abre la consola del navegador (F12)
5. Verifica que se muestre: `"Paradas recibidas: [...]"`
6. Los marcadores deben aparecer en el mapa

---

## 🔧 Archivos Modificados

1. ✅ **Modificado:** `Startup.cs` (cambio a camelCase)
2. ✅ **Modificado:** `wwwroot/js/gestionarParadas.js` (validación y logging)
3. ✅ **Creado:** `Scripts/VerificarParadas.sql` (diagnóstico SQL)

---

## 📖 Documentación Relacionada

- Ver: `Docs/SOLUCION_ERROR_500_API_PARADAS.md` (error anterior de ciclo JSON)
- Ver: `Docs/GESTION_PARADAS_MULTIPLES_BUSES.md` (funcionalidad con múltiples buses)