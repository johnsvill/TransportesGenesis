# ✅ FASE 6 - Parte B: Indicadores Visuales en Calendario

## 📅 Fecha: Enero 2025

## 🎯 **Objetivo**
Agregar indicadores visuales en el calendario para días con traslados temporales aprobados.

---

## 🎨 **Características Implementadas**

### **1. Indicadores en las Celdas del Calendario**

#### **Borde Naranja + Sombra:**
```javascript
if (traslado) {
    celda.style.border = '3px solid #ff9800'; // Borde naranja
    celda.style.boxShadow = '0 0 8px rgba(255, 152, 0, 0.5)'; // Brillo naranja
}
```

**Resultado visual:**
```
Día NORMAL:
┌────┐
│ 25 │ Verde (borde estándar)
│ ✓  │
└────┘

Día con TRASLADO:
┏━━━━┓ ← Borde naranja grueso con brillo
┃ 26 ┃ Verde
┃ ✓  ┃
┃ 🚌 ┃ ← Icono de bus
┗━━━━┛
```

---

#### **Tooltip (hover):**
Cuando pasas el mouse sobre un día con traslado:
```
Tooltip: "Traslado aprobado a BUS-003"
```

---

#### **Icono de Bus 🚌:**
Se agrega debajo del check de asistencia:
```html
<div class="fw-bold">26</div>
<div class="mt-1">✓</div>
<div style="font-size: 0.9em;">🚌</div> ← NUEVO
```

---

### **2. Panel Lateral - Alerta de Traslado**

Cuando seleccionas un día con traslado aprobado, aparece una alerta azul:

```
┌────────────────────────────────────────┐
│ 🚌 Traslado Temporal Activo           │
├────────────────────────────────────────┤
│ Este día tiene un traslado aprobado:  │
│                                        │
│ 🚌 Bus temporal: BUS-003               │
│ ⏰ Turno: ambas rutas                  │
│ 📝 Motivo: Se quedará en casa de...   │
└────────────────────────────────────────┘
```

**Ubicación**: Entre la advertencia de desmarca y los botones de acción.

---

## 🔧 **Cambios en el Código**

### **Archivo Modificado:**
`Pages/Padres/ConfirmarAsistencia.cshtml`

---

### **1. Variable Global para Traslados**

```javascript
let trasladosDelMes = []; // Almacenar traslados aprobados del mes
```

---

### **2. Cargar Traslados en `cargarCalendario()`**

```javascript
// Cargar traslados aprobados del mes
try {
    const responseTras = await fetch(`/api/traslados/alumno/${idAlumno}`);
    const todosTras = await responseTras.json();

    // Filtrar solo los aprobados del mes actual
    trasladosDelMes = todosTras.filter(t => {
        if (t.estado !== 'Aprobado') return false;
        const fechaTras = new Date(t.fechaTraslado);
        return fechaTras.getFullYear() === anioActual 
            && fechaTras.getMonth() === mesActual;
    });

    console.log(`[CALENDARIO] Traslados aprobados del mes: ${trasladosDelMes.length}`);
} catch (error) {
    console.error('Error al cargar traslados:', error);
    trasladosDelMes = [];
}
```

---

### **3. Función `buscarTraslado(fecha)`**

```javascript
function buscarTraslado(fecha) {
    return trasladosDelMes.find(t => {
        const fechaTras = new Date(t.fechaTraslado);
        return fechaTras.toDateString() === fecha.toDateString();
    });
}
```

---

### **4. Modificar `generarCalendario()` - Agregar Indicadores**

```javascript
const traslado = buscarTraslado(fechaActual);

// Agregar borde si hay traslado aprobado
if (traslado) {
    celda.style.border = '3px solid #ff9800';
    celda.style.boxShadow = '0 0 8px rgba(255, 152, 0, 0.5)';
    celda.title = `Traslado aprobado a ${traslado.placaBusDestino || 'bus temporal'}`;
}

// Agregar icono de bus si hay traslado
const iconoTraslado = traslado ? '<div style="font-size: 0.9em;">🚌</div>' : '';

celda.innerHTML = `
    <div class="fw-bold">${fecha}</div>
    <div class="mt-1">${icono}</div>
    ${iconoTraslado}
`;
```

---

### **5. Agregar Alerta en HTML del Panel**

```html
<!-- Indicador de traslado -->
<div id="alertaTraslado" class="alert alert-info" style="display: none;">
    <h6 class="alert-heading">
        <i class="bi bi-bus-front"></i> Traslado Temporal Activo
    </h6>
    <p id="textoTraslado" class="mb-0"></p>
</div>
```

---

### **6. Modificar `seleccionarDia()` - Mostrar Alerta**

```javascript
const traslado = buscarTraslado(fecha);

// Mostrar alerta si hay traslado aprobado
const alertaTraslado = document.getElementById('alertaTraslado');
const textoTraslado = document.getElementById('textoTraslado');

if (traslado) {
    alertaTraslado.style.display = 'block';
    const turnoTexto = traslado.turno === 'Ambos' ? 'ambas rutas' : 
                      traslado.turno === 'Mañana' ? 'ruta matutina' : 'ruta vespertina';
    textoTraslado.innerHTML = `
        <strong>Este día tiene un traslado aprobado:</strong><br>
        🚌 Bus temporal: <strong>${traslado.placaBusDestino || 'N/A'}</strong><br>
        ⏰ Turno: <strong>${turnoTexto}</strong><br>
        ${traslado.motivo ? `📝 Motivo: ${traslado.motivo}` : ''}
    `;
} else {
    alertaTraslado.style.display = 'none';
}
```

---

## 🧪 **Cómo Probar**

### **Setup:**
1. Necesitas tener al menos un traslado **aprobado** en la BD
2. Si no tienes, puedes crear uno y luego actualizar su estado:

```sql
-- Ver traslados actuales
SELECT * FROM genesis.SolicitudTraslado;

-- Aprobar una solicitud existente
UPDATE genesis.SolicitudTraslado
SET Estado = 'Aprobado',
    IdBusDestino = 5, -- BUS-002
    FechaRespuesta = GETDATE(),
    AprobadoPor = 'Admin',
    ComentarioAdmin = 'Aprobado para prueba'
WHERE IdSolicitud = 1; -- Cambia por ID real
```

---

### **Test Visual:**

1. **Ejecuta el proyecto** (`F5`)
2. **Ve a**: `/Padres/ConfirmarAsistencia`
3. **Busca el día con traslado aprobado**:
   - Debe tener **borde naranja** ✅
   - Debe tener **brillo naranja** ✅
   - Debe tener **icono 🚌** ✅
4. **Pasa el mouse** sobre ese día:
   - Debe aparecer tooltip: "Traslado aprobado a BUS-XXX" ✅
5. **Click en ese día**:
   - Panel lateral se abre ✅
   - Aparece **alerta azul** con info del traslado ✅
   - Muestra: Bus, Turno, Motivo ✅

---

## 📊 **Comparación: Antes vs Después**

### **ANTES:**
```
Calendario:
┌────┬────┬────┬────┬────┐
│ 25 │ 26 │ 27 │ 28 │ 29 │ Verde (todos iguales)
│ ✓  │ ✓  │ ✓  │ ✓  │ ✓  │
└────┴────┴────┴────┴────┘

Panel lateral:
[Sin indicador de traslado]
```

### **DESPUÉS:**
```
Calendario:
┌────┬━━━━┬────┬────┬────┐
│ 25 │ 26 │ 27 │ 28 │ 29 │
│ ✓  │ ✓  │ ✓  │ ✓  │ ✓  │
│    │ 🚌 │    │    │    │ ← Bus visible
└────┗━━━━┛────┴────┴────┘
      ↑ Borde naranja + brillo

Panel lateral (día 26):
┌──────────────────────────────┐
│ 🚌 Traslado Temporal Activo  │
│ Bus: BUS-003, Turno: Ambos   │
└──────────────────────────────┘
```

---

## ✅ **Checklist de Funcionalidades**

- [x] Variable global `trasladosDelMes[]`
- [x] Carga traslados del mes desde API
- [x] Función `buscarTraslado(fecha)`
- [x] Borde naranja en celdas con traslado
- [x] Sombra naranja (glow effect)
- [x] Tooltip con info del bus
- [x] Icono 🚌 visible en celda
- [x] Alerta azul en panel lateral
- [x] Muestra: Bus, Turno, Motivo
- [x] Solo muestra traslados **Aprobados**
- [x] Filtra por mes actual
- [x] Código compila sin errores

---

## 🎨 **Personalización Opcional**

### **Cambiar color del borde:**
```javascript
celda.style.border = '3px solid #2196F3'; // Azul
celda.style.border = '3px solid #4CAF50'; // Verde
celda.style.border = '3px solid #9C27B0'; // Morado
```

### **Cambiar icono:**
```javascript
const iconoTraslado = traslado ? '<div>🚐</div>' : ''; // Van
const iconoTraslado = traslado ? '<div>🚍</div>' : ''; // Bus frontal
const iconoTraslado = traslado ? '<div>🔄</div>' : ''; // Flechas
```

### **Agregar animación:**
```css
@keyframes pulse {
    0%, 100% { box-shadow: 0 0 8px rgba(255, 152, 0, 0.5); }
    50% { box-shadow: 0 0 16px rgba(255, 152, 0, 0.8); }
}

celda.style.animation = 'pulse 2s infinite';
```

---

## 🎯 **Próximo Paso: Parte A**

Ahora que los padres pueden **ver** los traslados aprobados, necesitamos que el admin pueda **aprobarlos**.

**Parte A - Panel de Admin:**
- Página: `/Admin/GestionarTraslados`
- Tabla de solicitudes pendientes
- Botones Aprobar/Rechazar
- Modal para comentario

**Tiempo estimado**: 1-2 horas

---

**Estado**: ✅ **PARTE B COMPLETADA**  
**Compilación**: ✅ Exitosa  
**Siguiente**: Parte A - Panel de Admin

---

**Última actualización**: Enero 2025
