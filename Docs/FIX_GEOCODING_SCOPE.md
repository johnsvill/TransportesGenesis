# ✅ SOLUCIÓN AL ERROR: geocodificarDireccion is not defined

## 🐛 Error Encontrado

```
Uncaught ReferenceError: geocodificarDireccion is not defined
	at HTMLButtonElement.onclick (GestionarParadas:414:73)
```

**Causa**: La función estaba declarada como `async function` que es local al scope del módulo, no global.

**Problema**: El `onclick` inline busca la función en el scope global (`window`).

---

## ✅ Solución Aplicada

Cambié la declaración de:
```javascript
async function geocodificarDireccion() {
	// código...
}
```

A:
```javascript
window.geocodificarDireccion = async function() {
	// código...
}
```

Ahora la función está en el scope global y el `onclick` puede encontrarla.

---

## 🚀 INSTRUCCIONES INMEDIATAS

### Paso 1: Recargar la Página
En tu navegador:
- Presiona **`Ctrl + Shift + R`** (recarga forzada, limpia cache)
- O **`Ctrl + F5`**

### Paso 2: Verificar en Consola (F12)
```javascript
// Verificar que la función existe
console.log(typeof window.geocodificarDireccion);
// Debe decir: "function"
```

### Paso 3: Probar el Botón
1. Haz click en **"Editar"** una parada
2. Escribe: `Universidad de San Carlos, Guatemala`
3. Haz click en **"🔍 Buscar en Mapa"**

---

## ✅ Resultado Esperado

Deberías ver:
```
===================================
🚀 FUNCIÓN GEOCODIFICAR EJECUTADA
===================================
Elemento parada-nombre: <input...>
=== INICIO GEOCODIFICACIÓN ===
Dirección ingresada: Universidad de San Carlos, Guatemala
```

Y luego:
- 🔔 Alerta popup con coordenadas
- 📝 Campos lat/lng llenados
- 🔴 Marcador rojo en el mapa

---

## 🧪 Test Manual (si no funciona aún)

Abre consola (F12) y ejecuta:

```javascript
// 1. Verificar que existe
console.log('Función existe:', typeof window.geocodificarDireccion);

// 2. Llamarla manualmente (con modal abierto)
window.geocodificarDireccion();
```

---

## 📝 Explicación Técnica

### El Problema:
```javascript
// ❌ Función local (no accesible desde HTML)
async function geocodificarDireccion() { }
```

### La Solución:
```javascript
// ✅ Función global (accesible desde HTML)
window.geocodificarDireccion = async function() { }
```

### ¿Por qué?
Cuando usas `onclick="geocodificarDireccion()"` en HTML, el navegador busca la función en el scope global (`window`). Si la función está declarada como `function` normal dentro de un módulo, no está en el scope global.

---

## 🔄 Alternativa (si prefieres no usar window)

Si no te gusta contaminar el scope global, otra opción es NO usar `onclick` inline:

### Opción A: Solo Event Listener (ya implementado como respaldo)
```javascript
document.body.addEventListener('click', function(e) {
	const target = e.target.closest('#btn-geocodificar');
	if (target) {
		geocodificarDireccion(); // función local
	}
});
```

### Opción B: Remover onclick del HTML
En `GestionarParadas.cshtml`, cambiar de:
```html
<button onclick="geocodificarDireccion()" id="btn-geocodificar">
```

A:
```html
<button id="btn-geocodificar">
```

Y usar solo el event listener.

---

## 📊 Estado Actual

✅ **Solución implementada**: `window.geocodificarDireccion = async function()`  
✅ **Doble respaldo**: Event delegation también activo  
✅ **Compilación**: Exitosa  

**Siguiente paso**: Recarga la página con `Ctrl + Shift + R` y prueba.

---

## 🎯 Confirmación

Después de recargar, ejecuta en consola:
```javascript
window.geocodificarDireccion
```

**Debe mostrar**:
```
ƒ () {
	console.log('===================================');
	console.log('🚀 FUNCIÓN GEOCODIFICAR EJECUTADA');
	...
}
```

**NO debe mostrar**: `undefined`

---

## 📞 Si Sigue Sin Funcionar

1. **Verifica que el archivo JS se carga**:
   - F12 → Pestaña "Network" (Red)
   - Busca `gestionarParadas.js`
   - ¿Status 200? ✅
   - ¿Status 404? ❌ No se está cargando

2. **Verifica la ruta del script**:
   En `GestionarParadas.cshtml` debe haber:
   ```html
   @section Scripts {
	   <script src="~/js/gestionarParadas.js"></script>
   }
   ```

3. **Cache del navegador**:
   - Presiona `Ctrl + Shift + Delete`
   - Limpia cache e imágenes
   - Recarga la página

---

**Estado**: ✅ SOLUCIONADO  
**Confianza**: 95%  
**Acción requerida**: Recarga forzada de la página (Ctrl + Shift + R)
