# 🔍 Solución al Problema de Geocodificación

## 🐛 Problema Reportado

**Síntoma**: Al buscar "Centro Comercial Pradera, Guatemala" no sale nada y no cambian los valores de latitud/longitud.

---

## ✅ Solución Implementada

### 1. **Código Mejorado con Debug Completo**

Se actualizó `wwwroot/js/gestionarParadas.js` con:
- ✅ **Logs detallados** en consola del navegador
- ✅ **Alertas visuales** para el usuario
- ✅ **Mejor manejo de errores**
- ✅ **Múltiples resultados** (limit=3 en lugar de 1)

### 2. **Página de Prueba Independiente**

Se creó `wwwroot/test-geocoding.html` para probar la API sin depender del sistema completo .

---

## 🔧 Cómo Diagnosticar el Problema

### Paso 1: Probar con la Página de Test

1. **Ejecutar el proyecto**:
   ```bash
   dotnet run
   ```

2. **Abrir en el navegador**:
   ```
   https://localhost:7XXX/test-geocoding.html
   ```

3. **Probar búsqueda**:
   - Ya viene prellenado con "Centro Comercial Pradera, Guatemala"
   - Hacer clic en "🔍 Buscar Ubicación"
   - Observar los logs en la página

4. **Resultados esperados**:
   - ✅ Si funciona: Verás coordenadas y dirección completa
   - ❌ Si no funciona: Verás el error específico en los logs

---

### Paso 2: Abrir la Consola del Navegador

1. **En Google Chrome/Edge**:
   - Presiona `F12` o `Ctrl + Shift + I`
   - Ve a la pestaña "Console"

2. **En Firefox**:
   - Presiona `F12` o `Ctrl + Shift + K`
   - Ve a "Consola"

3. **Busca logs** que comiencen con:
   ```
   === INICIO GEOCODIFICACIÓN ===
   ```

---

### Paso 3: Verificar la Red

En la consola del navegador (F12):

1. **Ve a la pestaña "Network" (Red)**
2. **Haz la búsqueda** de nuevo
3. **Busca la petición** a `nominatim.openstreetmap.org`
4. **Revisa**:
   - Status Code: ¿Es 200?
   - Response: ¿Tiene datos JSON?
   - Headers: ¿Hay errores de CORS?

---

## 🚨 Posibles Causas del Error

### Causa 1: Bloqueo de CORS
**Síntoma**: Error en consola `CORS policy` o `Access-Control-Allow-Origin`

**Solución**: No se puede solucionar desde el cliente. Opciones:
- Usar un proxy backend
- Usar otra API de geocodificación
- Instalar extensión de navegador para deshabilitar CORS (solo desarrollo)

**Verificar**: Abre la página de test y revisa si aparece este error.

---

### Causa 2: Sin Conexión a Internet
**Síntoma**: Error `Failed to fetch` o `Network Error`

**Solución**:
- Verificar conexión WiFi/Ethernet
- Verificar que no haya firewall bloqueando
- Probar con otro navegador

**Verificar**: Intenta abrir https://nominatim.openstreetmap.org/ en el navegador.

---

### Causa 3: Rate Limiting de Nominatim
**Síntoma**: Error 429 (Too Many Requests)

**Solución**:
- Esperar 1-2 minutos
- Nominatim tiene límite de 1 petición por segundo

**Verificar**: Revisa el Status Code en la pestaña Network.

---

### Causa 4: Dirección No Encontrada
**Síntoma**: La API responde pero con array vacío `[]`

**Solución**:
- Probar con direcciones más específicas
- Agregar "Guatemala" al final
- Usar nombres de lugares conocidos

**Verificar**: En la página de test verás "0 ubicaciones encontradas".

---

### Causa 5: JavaScript No se Está Ejecutando
**Síntoma**: No aparece ningún log en consola

**Solución**:
- Verificar que el archivo `.js` esté cargado
- Revisar si hay errores de sintaxis
- Verificar que el botón tenga el evento configurado

**Verificar**: En consola escribe `typeof geocodificarDireccion` → debe decir `"function"`

---

### Causa 6: Modal No Está Abierto Correctamente
**Síntoma**: Los elementos no se encuentran (getElementById retorna null)

**Solución**:
- Asegurarse de abrir el modal ANTES de hacer clic en "Buscar en Mapa"
- Verificar que los IDs coincidan: `parada-nombre`, `parada-lat`, `parada-lng`

**Verificar**: En consola escribe:
```javascript
console.log(document.getElementById('parada-nombre'));
// Debe mostrar el elemento <input>, NO null
```

---

## 🎯 Pruebas para Hacer AHORA

### Test 1: Página de Prueba Independiente

```bash
# 1. Ejecutar proyecto
dotnet run

# 2. Abrir navegador
https://localhost:7XXX/test-geocoding.html

# 3. Hacer clic en "Buscar"
# 4. Ver resultado en pantalla
```

**Resultado esperado**: Deberías ver algo como:
```
✅ Ubicación encontrada
Dirección: Centro Comercial Pradera Concepción, Diagonal 6, Zona 10...
Coordenadas:
• Latitud: 14.598765
• Longitud: -90.512345
```

---

### Test 2: Desde la Consola del Navegador

Abre la consola (F12) y ejecuta:

```javascript
// Test manual de la API
fetch('https://nominatim.openstreetmap.org/search?format=json&q=Centro%20Comercial%20Pradera,%20Guatemala&limit=1', {
	headers: {
		'User-Agent': 'TransportesGenesis/1.0'
	}
})
.then(r => r.json())
.then(data => {
	console.log('Resultados:', data);
	if (data.length > 0) {
		console.log('Lat:', data[0].lat);
		console.log('Lon:', data[0].lon);
		console.log('Nombre:', data[0].display_name);
	}
})
.catch(err => console.error('Error:', err));
```

**Resultado esperado**: Deberías ver un objeto JSON con coordenadas.

---

### Test 3: Verificar Elementos del DOM

En la página "Gestionar Paradas", abre el modal de editar y ejecuta en consola:

```javascript
// Verificar que los elementos existen
console.log('Input dirección:', document.getElementById('parada-nombre'));
console.log('Input latitud:', document.getElementById('parada-lat'));
console.log('Input longitud:', document.getElementById('parada-lng'));
console.log('Botón geocodificar:', document.getElementById('btn-geocodificar'));
console.log('Info geocoding:', document.getElementById('geocoding-info'));

// Verificar función
console.log('Función existe:', typeof geocodificarDireccion);
```

**Resultado esperado**: Todos deben mostrar elementos HTML, NO `null`.

---

## 🛠️ Soluciones Alternativas

### Alternativa 1: Usar Google Maps API (Requiere API Key)

Si Nominatim no funciona por CORS, podemos usar Google:

```javascript
// Requiere API Key de Google Cloud
const url = `https://maps.googleapis.com/maps/api/geocode/json?address=${direccion}&key=YOUR_API_KEY`;
```

**Ventajas**: Más preciso, mejor cobertura  
**Desventajas**: Requiere tarjeta de crédito y API Key

---

### Alternativa 2: Backend Proxy

Crear un endpoint en el backend que haga la petición:

```csharp
// Controllers/Api/GeocodingController.cs
[HttpGet("geocode")]
public async Task<IActionResult> Geocode(string address)
{
	using var httpClient = new HttpClient();
	var url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(address)}&limit=1";
	var response = await httpClient.GetStringAsync(url);
	return Ok(response);
}
```

**Ventajas**: Sin problemas de CORS  
**Desventajas**: Más carga en el servidor

---

### Alternativa 3: Método Manual (Ya Existe)

Usar el método existente de "Click en el Mapa":

1. Click en botón "Agregar Parada"
2. Click en el mapa donde quieres la parada
3. Se llenan las coordenadas automáticamente
4. Escribir dirección manualmente

---

## 📋 Checklist de Diagnóstico

- [ ] Ejecutar el proyecto y abrir `test-geocoding.html`
- [ ] Hacer búsqueda y ver si funciona
- [ ] Abrir consola del navegador (F12)
- [ ] Buscar errores en la pestaña "Console"
- [ ] Revisar la pestaña "Network" para ver la petición HTTP
- [ ] Verificar Status Code de la petición
- [ ] Probar el test manual desde la consola (código arriba)
- [ ] Verificar que los elementos del DOM existen
- [ ] Probar con diferentes direcciones
- [ ] Probar en otro navegador (Chrome, Firefox, Edge)

---

## 📞 Reportar Resultados

### Si la Página de Test Funciona:
✅ El problema está en la integración con el modal.  
**Solución**: Verificar que el modal esté abierto y los IDs coincidan.

### Si la Página de Test NO Funciona:
❌ El problema es con la API o conexión.  
**Solución**: Probar alternativas (Google Maps API o Backend Proxy).

### Si Aparece Error de CORS:
⚠️ El navegador está bloqueando la petición.  
**Solución**: Implementar Backend Proxy.

---

## 🎓 Código Mejorado Instalado

El archivo `gestionarParadas.js` ahora tiene:

1. **Logs detallados**:
   ```javascript
   console.log('=== INICIO GEOCODIFICACIÓN ===');
   console.log('Dirección ingresada:', direccion);
   ```

2. **Alertas para el usuario**:
   ```javascript
   alert('✅ Ubicación encontrada!\n\n...');
   ```

3. **Manejo de múltiples resultados**:
   ```javascript
   const url = `...&limit=3`; // Antes era limit=1
   ```

4. **Mejor manejo de errores**:
   ```javascript
   console.error('Tipo de error:', error.name);
   console.error('Mensaje:', error.message);
   console.error('Stack:', error.stack);
   ```

---

## ✅ Próximos Pasos

1. **Ejecutar el proyecto**
2. **Abrir** `https://localhost:XXXX/test-geocoding.html`
3. **Hacer clic** en "Buscar"
4. **Reportar** qué mensaje aparece:
   - ¿Funciona? → Entonces el problema es en el modal
   - ¿Error de CORS? → Necesitamos backend proxy
   - ¿No se encuentra? → Probar con otra dirección
   - ¿Otro error? → Compartir el mensaje exacto

---

**Última actualización**: Mayo 2026  
**Estado**: 🔧 Debugging en progreso
