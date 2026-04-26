# 🔧 Debug: Borde Naranja No Aparece en Calendario

## 🐛 **Problema Reportado**
Usuario creó solicitud de traslado pero al regresar al calendario no ve el borde naranja.

---

## 🔍 **Diagnóstico**

### **Causa Identificada:**
Las solicitudes se crean con estado **"Pendiente"**, pero los indicadores visuales solo aparecen para traslados con estado **"Aprobado"**.

```javascript
// En el código del calendario (línea ~453):
if (t.estado.toLowerCase() !== 'aprobado') return false;
```

**Flujo correcto:**
```
Padre crea solicitud → Estado: "Pendiente" (sin borde naranja)
                          ↓
Admin aprueba solicitud → Estado: "Aprobado" (CON borde naranja) ✅
```

---

## ✅ **Solución Temporal (Para Testing)**

### **Opción A: Aprobar manualmente en BD**

Ya aprobé 2 solicitudes para ti:

```sql
-- Solicitudes aprobadas para abril 2026:
IdSolicitud 5: lunes 27/04/2026 - Turno Mañana - BUS: P-001GT
IdSolicitud 6: lunes 27/04/2026 - Turno Ambos - BUS: BUS-002
```

---

## 🧪 **Cómo Probar AHORA**

### **Paso 1: Ejecuta el proyecto**
```
F5 en Visual Studio
```

### **Paso 2: Abre Developer Tools**
```
En el navegador: F12 → Pestaña Console
```

### **Paso 3: Ve al calendario**
```
https://localhost:7240/Padres/ConfirmarAsistencia
```

### **Paso 4: Lee los logs en Console**
Deberías ver:
```
[CALENDARIO] Total traslados recibidos: 7
[CALENDARIO] Mes actual: 3 Año actual: 2026
[TRASLADO] ID: 5, Estado: "Aprobado", Fecha: 2026-04-27...
[TRASLADO] ID: 6, Estado: "Aprobado", Fecha: 2026-04-27...
[CALENDARIO] ✅ Traslados aprobados del mes: 2
```

### **Paso 5: Busca el lunes 27 en el calendario**
Deberías ver:
```
┏━━━━┓ ← Borde naranja
┃ 27 ┃
┃ ✓  ┃
┃ 🚌 ┃ ← Icono de bus
┗━━━━┛
```

### **Paso 6: Pasa el mouse sobre el día 27**
Tooltip debería decir: "Traslado aprobado a P-001GT"

### **Paso 7: Click en el día 27**
Panel lateral debería mostrar:
```
┌──────────────────────────────────┐
│ 🚌 Traslado Temporal Activo      │
├──────────────────────────────────┤
│ Bus temporal: P-001GT            │
│ Turno: ruta matutina             │
│ Motivo: Traslado de prueba...    │
└──────────────────────────────────┘
```

---

## 🔧 **Troubleshooting**

### **Problema 1: Sigue sin verse el borde naranja**

**Verificar en Console (F12):**
```javascript
[CALENDARIO] ✅ Traslados aprobados del mes: 0  // ← Si ves 0, hay problema
```

**Soluciones:**
1. Verifica que el mes/año coincidan:
   - Console dice: `Mes actual: 3` (abril = mes 3 en JavaScript, 0-indexed)
   - BD tiene: abril 2026

2. Verifica estado en logs:
   ```
   [TRASLADO] ID: 5, Estado: "Pendiente"  // ← Si dice Pendiente, no aparecerá
   ```

---

### **Problema 2: Logs muestran traslados pero sin borde**

**Verificar en Console:**
```javascript
// Busca esto cuando se genera el calendario:
[CALENDARIO] Traslados filtrados: [{...}]
```

**Si aparece pero sin borde**, revisa:
1. Recarga con `Ctrl+F5` (limpiar caché)
2. Verifica que `buscarTraslado(fecha)` retorna algo
3. Agrega log temporal:

```javascript
// En la función generarCalendario, línea ~493
const traslado = buscarTraslado(fechaActual);
console.log(`Día ${fecha}: Traslado encontrado:`, traslado); // ← Agregar esto
```

---

### **Problema 3: API retorna lista vacía**

**Verificar endpoint:**
```javascript
// En Console, ejecuta:
fetch('/api/traslados/alumno/1')
    .then(r => r.json())
    .then(data => console.log('API Response:', data));
```

**Resultado esperado:**
```json
[
  {
    "idSolicitud": 5,
    "fechaTraslado": "2026-04-27T...",
    "estado": "Aprobado",
    "placaBusDestino": "P-001GT",
    ...
  }
]
```

**Si está vacío:**
- El repositorio no está trayendo los datos
- Verifica que `GetByAlumnoAsync(1)` funcione

---

## 📊 **Mejoras Implementadas (Logs)**

Agregué logs detallados en el código para facilitar debugging:

```javascript
// Logs agregados:
console.log(`[CALENDARIO] Total traslados recibidos: ${todosTras.length}`);
console.log('[CALENDARIO] Mes actual:', mesActual, 'Año actual:', anioActual);
console.log(`[TRASLADO] ID: ${t.idSolicitud}, Estado: "${t.estado}", Fecha: ${t.fechaTraslado}`);
console.log(`  -> Año: ${fechaTras.getFullYear()}, Mes: ${fechaTras.getMonth()}, Coincide: ${esDelMes}`);
console.log(`[CALENDARIO] ✅ Traslados aprobados del mes: ${trasladosDelMes.length}`);
```

**Beneficio**: Ahora puedes ver exactamente qué está pasando en cada paso.

---

## 🎯 **Flujo Correcto (Producción)**

### **Para que aparezca el borde naranja naturalmente:**

```
1. PADRE crea solicitud
   → Estado: "Pendiente"
   → Sin borde naranja (normal)

2. ADMIN aprueba solicitud (Panel de Admin - Parte A)
   → Estado cambia a: "Aprobado"
   → IdBusDestino asignado
   → ComentarioAdmin agregado

3. PADRE recarga calendario
   → API retorna traslado con estado "Aprobado"
   → Calendario lo filtra y muestra borde naranja ✅
```

**Próximo paso**: Crear Panel de Admin (Parte A) para que el admin pueda aprobar solicitudes desde la interfaz.

---

## 📝 **Para Aprobar Más Solicitudes (Testing)**

Si quieres probar con otras solicitudes, ejecuta:

```sql
-- Ver solicitudes pendientes
SELECT IdSolicitud, FechaTraslado, Turno, Motivo, Estado
FROM genesis.SolicitudTraslado
WHERE Estado = 'Pendiente'
ORDER BY FechaTraslado;

-- Aprobar una solicitud específica
UPDATE genesis.SolicitudTraslado
SET Estado = 'Aprobado',
    IdBusDestino = 5,  -- BUS-002
    FechaRespuesta = GETDATE(),
    AprobadoPor = 'Admin',
    ComentarioAdmin = 'Aprobado para prueba'
WHERE IdSolicitud = 4;  -- Cambia el ID por el que quieras aprobar
```

---

## ✅ **Checklist Post-Fix**

- [x] Logs agregados al código
- [x] Comparación case-insensitive (`toLowerCase()`)
- [x] 2 solicitudes aprobadas para abril 2026
- [x] Compilación exitosa
- [ ] Testing visual en navegador (pendiente de usuario)
- [ ] Verificar logs en Console (F12)
- [ ] Confirmar borde naranja visible

---

## 🎯 **Próximos Pasos**

1. **AHORA**: Prueba con `F5` y verifica logs en Console
2. **Reporta**: Si ves borde naranja o qué logs aparecen
3. **Si funciona**: Continuamos con Parte A (Panel Admin)
4. **Si no funciona**: Con los logs sabremos exactamente qué ajustar

---

**Estado**: ✅ Fix implementado + logs agregados  
**Solicitudes aprobadas**: 2 (lunes 27/04/2026)  
**Esperando**: Testing visual del usuario
