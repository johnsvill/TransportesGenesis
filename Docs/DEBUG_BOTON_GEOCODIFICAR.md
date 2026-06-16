# 🔧 Script de Debug para Consola del Navegador

## Abre la consola (F12) y ejecuta estos comandos uno por uno:

### 1. Verificar que el botón existe
```javascript
console.log('Botón geocodificar:', document.getElementById('btn-geocodificar'));
```
**Esperado**: Debe mostrar `<button id="btn-geocodificar"...>`  
**Si es null**: El modal no está abierto

---

### 2. Verificar que la función existe
```javascript
console.log('Función geocodificarDireccion:', typeof geocodificarDireccion);
```
**Esperado**: `"function"`  
**Si es undefined**: El archivo JS no se cargó

---

### 3. Probar la función manualmente
```javascript
// Primero, abre el modal de editar parada
// Luego ejecuta:
geocodificarDireccion();
```
**Esperado**: Debe ejecutarse y mostrar logs

---

### 4. Simular click en el botón
```javascript
const btn = document.getElementById('btn-geocodificar');
if (btn) {
	btn.click();
	console.log('✅ Click simulado exitoso');
} else {
	console.log('❌ Botón no encontrado - ¿Modal abierto?');
}
```

---

### 5. Verificar eventos registrados
```javascript
// Ver qué eventos tiene el botón
const btn = document.getElementById('btn-geocodificar');
console.log('Eventos en el botón:', getEventListeners(btn));
```
**Nota**: `getEventListeners` solo funciona en Chrome/Edge

---

### 6. Test completo
```javascript
console.log('=== TEST COMPLETO ===');
console.log('1. Modal existe:', !!document.getElementById('modal-editar-parada'));
console.log('2. Botón existe:', !!document.getElementById('btn-geocodificar'));
console.log('3. Input existe:', !!document.getElementById('parada-nombre'));
console.log('4. Función existe:', typeof geocodificarDireccion);
console.log('5. Mapa existe:', typeof mapa !== 'undefined');
```

---

## 🎯 Procedimiento Completo

1. **Recarga la página** (Ctrl + R o F5)
2. **Abre la consola** (F12)
3. **Ve a la pestaña Console**
4. **Ejecuta** el Test completo (#6)
5. **Abre el modal** (click en editar una parada)
6. **Ejecuta** el Test #1 y #2
7. **Escribe** una dirección en el campo
8. **Click** en "Buscar en Mapa"
9. **Revisa** los logs en consola

---

## 📋 Checklist

- [ ] Recargué la página (F5)
- [ ] Abrí la consola (F12)
- [ ] El Test #6 muestra todo en `true` o `"function"`
- [ ] Abrí el modal de editar
- [ ] El botón existe (Test #1 no es null)
- [ ] Hice click en "Buscar en Mapa"
- [ ] Aparecieron logs en consola que empiezan con "🚀 FUNCIÓN GEOCODIFICAR EJECUTADA"
- [ ] Vi una alerta o cambio en los campos lat/lng

---

## 🐛 Si el botón NO responde:

### Prueba este workaround temporal:
```javascript
// Forzar registro del evento (ejecuta en consola)
document.getElementById('btn-geocodificar').addEventListener('click', function() {
	console.log('🔥 Evento temporal registrado');
	geocodificarDireccion();
});

console.log('✅ Evento temporal agregado. Prueba el botón ahora.');
```

Luego haz click en "Buscar en Mapa".

---

## 🎯 Si TODO funciona en consola pero NO con el botón:

Significa que hay un conflicto de eventos. Prueba:

1. **Revisar si hay otro script** que esté capturando el evento
2. **Verificar en Network** (pestaña Red) si el archivo JS se carga
3. **Buscar errores** en la pestaña Console

---

## ✅ Reporta:

1. ¿Qué dice el Test #6?
2. ¿El Test #1 muestra el botón o null?
3. ¿Aparece "🚀 FUNCIÓN GEOCODIFICAR EJECUTADA" al hacer click?
4. ¿El workaround temporal funciona?
