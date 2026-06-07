# ✅ FASE 6 - Parte A: Panel de Administrador

## 📅 Fecha: Enero 2025

## 🎯 **Objetivo**
Crear panel de administración para que los admins puedan aprobar o rechazar solicitudes de traslado temporal.

---

## 🎨 **Características Implementadas**

### **1. Página Principal: `/Admin/GestionarTraslados`**

#### **Estadísticas en Tiempo Real:**
```
┌──────────┬──────────┬──────────┬──────────┐
│    5     │    2     │    1     │    8     │
│ Pendient │ Aprobadas│Rechazadas│  Total   │
│   (⚠️)   │   (✅)   │   (❌)   │   (📊)   │
└──────────┴──────────┴──────────┴──────────┘
```

---

#### **Filtros Avanzados:**
- **Por Estado**: Pendiente / Aprobado / Rechazado / Todos
- **Por Orden**: Fecha traslado / Fecha solicitud (asc/desc)
- **Búsqueda**: Por nombre de alumno o motivo
- **Botón Actualizar**: Refrescar datos manualmente

---

#### **Tabla de Solicitudes:**

```
┌──────────┬────────┬───────┬─────────┬─────────┬────────┬──────────┬──────────┐
│ Alumno   │ Fecha  │ Turno │ Bus Ori │ Bus Dest│ Estado │Solicitud │ Acciones │
├──────────┼────────┼───────┼─────────┼─────────┼────────┼──────────┼──────────┤
│ Juan P.  │ Lun 27 │ 🔄 Am │ BUS-001 │Por asig │⏰ Pend │ 25/04/26 │ ✅ ❌    │
│          │¡1 día! │  bos  │         │         │        │          │          │
├──────────┼────────┼───────┼─────────┼─────────┼────────┼──────────┼──────────┤
│ María G. │ Mar 29 │🌅 Mañ │ BUS-002 │BUS-003  │✅ Apro │ 24/04/26 │Procesado │
└──────────┴────────┴───────┴─────────┴─────────┴────────┴──────────┴──────────┘
```

**Características especiales:**
- ⚠️ **Fila amarilla** para solicitudes urgentes (≤2 días)
- 🚨 **Alerta roja** mostrando días restantes
- 🔘 **Botones verdes/rojos** para aprobar/rechazar
- 📋 **"Procesado"** para solicitudes ya respondidas

---

### **2. Modal de Responder Solicitud**

```
┌─────────────────────────────────────────────┐
│ ✅ Responder Solicitud               [X]    │
├─────────────────────────────────────────────┤
│                                             │
│ Detalles de la Solicitud:                  │
│ ┌───────────────────────────────────────┐  │
│ │ Alumno: Juan Pérez                    │  │
│ │ Fecha: lunes, 27 de abril de 2026     │  │
│ │ Turno: Ambos                          │  │
│ │ Bus origen: BUS-001                   │  │
│ │ Bus solicitado: Por asignar           │  │
│ │ Motivo: Se quedará en casa de abuela │  │
│ └───────────────────────────────────────┘  │
│                                             │
│ Acción:                                     │
│ [✅ Aprobar] [❌ Rechazar]                  │
│                                             │
│ Bus destino: [Dropdown con buses]           │
│ BUS-002 - Volvo B7R (30 asientos)          │
│                                             │
│ Comentario (opcional):                      │
│ [___________________________________]       │
│ Ej: Aprobado - Bus con capacidad...        │
│                                             │
├─────────────────────────────────────────────┤
│         [Cancelar]  [📤 Enviar Respuesta]  │
└─────────────────────────────────────────────┘
```

**Funcionalidades:**
- 📋 Muestra **todos los detalles** de la solicitud
- 🔘 Toggle entre **Aprobar** y **Rechazar**
- 🚌 **Dropdown de buses** (solo visible al aprobar)
- 💬 **Textarea** para comentario del admin
- ✅ Validación: Bus destino obligatorio si aprueba
- 🎨 Header cambia de color según acción (verde/rojo)

---

### **3. Modal de Confirmación**

Después de aprobar/rechazar:
```
┌─────────────────────────────┐
│ ✅ ¡Éxito!             [X]  │
├─────────────────────────────┤
│                             │
│     ✓ (ícono grande)        │
│                             │
│ Solicitud aprobada          │
│ correctamente               │
│                             │
├─────────────────────────────┤
│         [Entendido]         │
└─────────────────────────────┘
```

**Auto-actualización**: La tabla se recarga automáticamente después de 1.5 segundos.

---

## 🔧 **Archivos Creados**

### **1. PageModel**
`Pages/Admin/GestionarTraslados.cshtml.cs`

```csharp
public class GestionarTrasladosModel : PageModel
{
    private readonly ITrasladoService _trasladoService;

    public GestionarTrasladosModel(ITrasladoService trasladoService)
    {
        _trasladoService = trasladoService;
    }

    public void OnGet()
    {
        // La página cargará datos vía JavaScript/API
    }
}
```

**Nota**: Toda la lógica está en JavaScript/APIs por rendimiento y UX fluida.

---

### **2. Página Razor**
`Pages/Admin/GestionarTraslados.cshtml`

**Secciones principales:**
1. **Header** con título e instrucciones
2. **Filtros** (estado, orden, búsqueda, actualizar)
3. **Estadísticas** (4 cards con contadores)
4. **Tabla** de solicitudes (responsiva)
5. **Modal Responder** (aprobar/rechazar)
6. **Modal Éxito** (confirmación)
7. **Scripts** (~450 líneas de JavaScript)

---

## 🔌 **Integración con APIs**

### **Endpoints utilizados:**

#### **1. GET `/api/traslados/pendientes`**
Obtiene solicitudes con estado "Pendiente"

```javascript
const response = await fetch('/api/traslados/pendientes');
const solicitudes = await response.json();
```

---

#### **2. GET `/api/traslados/buses-disponibles`**
Obtiene lista de buses para el dropdown

```javascript
const response = await fetch('/api/traslados/buses-disponibles');
const buses = await response.json();
// Retorna: [{ idBus, placa, modelo, asientosDisponibles }]
```

---

#### **3. POST `/api/traslados/{id}/responder`**
Envía la respuesta del admin

```javascript
const dto = {
    idSolicitud: 5,
    estado: 'Aprobado', // o 'Rechazado'
    idBusDestino: 5,    // Solo si aprueba
    comentarioAdmin: 'Aprobado - Bus con capacidad'
};

const response = await fetch('/api/traslados/5/responder', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto)
});
```

---

## 🎨 **Características de UX**

### **Urgencia Visual:**
```javascript
// Filas amarillas para solicitudes ≤2 días
const diffDias = Math.ceil((fechaTraslado - hoy) / (1000 * 60 * 60 * 24));
const urgente = diffDias <= 2 && estado === 'Pendiente';

if (urgente) {
    row.className = 'table-warning';
    // Mostrar: "¡1 día!" en rojo
}
```

---

### **Iconos Descriptivos:**
- 🌅 **Mañana** (solo ruta matutina)
- 🌆 **Tarde** (solo ruta vespertina)
- 🔄 **Ambos** (ambas rutas)
- ⏰ **Pendiente** (sin procesar)
- ✅ **Aprobado** (aprobado por admin)
- ❌ **Rechazado** (rechazado por admin)

---

### **Búsqueda en Tiempo Real:**
```javascript
function filtrarTabla() {
    const busqueda = document.getElementById('busqueda').value.toLowerCase();

    solicitudesFiltradas = solicitudesOriginales.filter(s => 
        (s.nombreAlumno || '').toLowerCase().includes(busqueda) ||
        (s.motivo || '').toLowerCase().includes(busqueda)
    );
}
```

**Busca en**: Nombre del alumno, Motivo de la solicitud

---

### **Estadísticas Dinámicas:**
```javascript
// Se actualizan automáticamente al filtrar
- Total Pendientes: Cuenta solicitudes con estado "Pendiente"
- Aprobadas Hoy: Cuenta aprobaciones de hoy (fechaRespuesta)
- Rechazadas Hoy: Cuenta rechazos de hoy
- Total: Cuenta solicitudes filtradas actualmente
```

---

## 🧪 **Cómo Probar**

### **Preparación:**
Ya tienes solicitudes pendientes en la BD (IDs: 1, 2, 3, 4, 5, etc.)

---

### **Test 1: Ver Panel de Admin**

1. **Ejecuta** el proyecto (`F5`)
2. **Ve a**: `https://localhost:7240/Admin/GestionarTraslados`
3. **Verifica**:
   - ✅ Estadísticas muestran números correctos
   - ✅ Tabla carga solicitudes pendientes
   - ✅ Botones ✅ y ❌ visibles en cada fila

---

### **Test 2: Aprobar Solicitud**

1. **Click** en botón verde ✅ de una solicitud
2. **Modal se abre** con detalles completos
3. **Acción** pre-seleccionada: "Aprobar"
4. **Selecciona** un bus del dropdown
5. **Escribe** comentario: "Aprobado - Capacidad disponible"
6. **Click** "Enviar Respuesta"
7. **Resultado**:
   - Modal de éxito aparece ✅
   - Tabla se actualiza automáticamente
   - Solicitud desaparece de pendientes

---

### **Test 3: Rechazar Solicitud**

1. **Click** en botón rojo ❌ de una solicitud
2. **Modal se abre** con header rojo
3. **Acción** pre-seleccionada: "Rechazar"
4. **Dropdown de bus** desaparece (no es necesario)
5. **Escribe** comentario: "Bus sin capacidad"
6. **Click** "Enviar Respuesta"
7. **Resultado**:
   - Modal de éxito aparece
   - Solicitud desaparece de pendientes

---

### **Test 4: Filtros**

**Filtro por Estado:**
1. Cambia dropdown "Estado" a "Aprobado"
2. Tabla muestra solo solicitudes aprobadas
3. Estadísticas se actualizan

**Filtro por Orden:**
1. Cambia "Ordenar por" a "Fecha (más próxima)"
2. Tabla se reordena por fecha de traslado

**Búsqueda:**
1. Escribe "Juan" en búsqueda
2. Tabla filtra solicitudes de alumnos con "Juan"

---

### **Test 5: Urgencia Visual**

1. **Crea solicitud** para **mañana**
2. **Recarga** panel de admin
3. **Verifica**:
   - Fila aparece con **fondo amarillo** ⚠️
   - Muestra "**¡1 día!**" en rojo
   - Destaca visualmente de las demás

---

## 📊 **Comparación: Antes vs Después**

### **ANTES (sin Panel Admin):**
```
❌ Admin tenía que:
1. Ir a SQL Server Management Studio
2. Ejecutar UPDATE manual
3. Copiar/pegar IDs
4. Recordar nombres de columnas
5. Sin validaciones
6. Sin historial visual
```

### **DESPUÉS (con Panel Admin):**
```
✅ Admin puede:
1. Ver tabla con todas las solicitudes
2. Filtrar por estado/fecha
3. Buscar por alumno
4. Ver detalles completos en modal
5. Aprobar con 2 clicks
6. Rechazar con comentario
7. Ver estadísticas en tiempo real
8. Identificar solicitudes urgentes
```

---

## 🎯 **Flujo Completo End-to-End**

### **1. Padre crea solicitud:**
```
/Padres/ConfirmarAsistencia
→ Click día → Solicitar Traslado
→ Llena formulario → Enviar
→ Estado: "Pendiente" ⏰
```

### **2. Admin revisa:**
```
/Admin/GestionarTraslados
→ Ve solicitud en tabla
→ Click botón ✅ (aprobar)
→ Selecciona bus
→ Escribe comentario
→ Enviar Respuesta
→ Estado cambia a: "Aprobado" ✅
```

### **3. Padre ve actualización:**
```
/Padres/Traslados (historial)
→ Solicitud ahora muestra: "Aprobado" ✅
→ Puede ver comentario del admin
→ Fecha de respuesta visible

/Padres/ConfirmarAsistencia (calendario)
→ Día del traslado muestra borde naranja 🟧
→ Icono 🚌 visible
→ Panel lateral muestra alerta azul con detalles
```

---

## ✅ **Checklist de Funcionalidades**

### **Visualización:**
- [x] Tabla responsiva con todas las solicitudes
- [x] Estadísticas en cards (Pendientes, Aprobadas hoy, Rechazadas hoy, Total)
- [x] Filtro por estado (Pendiente/Aprobado/Rechazado/Todos)
- [x] Filtro por orden (4 opciones)
- [x] Búsqueda por texto
- [x] Botón actualizar
- [x] Iconos descriptivos (turnos, estados)
- [x] Badges de colores

### **Interacción:**
- [x] Botones Aprobar/Rechazar en cada fila
- [x] Modal con detalles completos
- [x] Dropdown de buses (carga dinámica)
- [x] Toggle Aprobar/Rechazar
- [x] Textarea para comentario
- [x] Validación de campos requeridos
- [x] Modal de confirmación
- [x] Auto-recarga después de responder

### **UX Avanzada:**
- [x] Filas amarillas para solicitudes urgentes (≤2 días)
- [x] Contador de días restantes
- [x] Header del modal cambia de color (verde/rojo)
- [x] Dropdown de bus oculto si rechaza
- [x] Búsqueda en tiempo real
- [x] Estadísticas actualizadas dinámicamente
- [x] Mensajes de error/éxito claros

### **Integración:**
- [x] Consume API `/api/traslados/pendientes`
- [x] Consume API `/api/traslados/buses-disponibles`
- [x] POST a `/api/traslados/{id}/responder`
- [x] Manejo de errores con try-catch
- [x] Modo simulación del API funciona

---

## 🚀 **Optimizaciones Futuras (Opcionales)**

### **1. Notificaciones en Tiempo Real:**
```javascript
// SignalR para actualizar sin refrescar
connection.on("SolicitudCreada", (solicitud) => {
    // Agregar a la tabla automáticamente
});
```

### **2. Paginación:**
```javascript
// Si hay >50 solicitudes
- Mostrar 20 por página
- Botones Anterior/Siguiente
- Selector de items por página
```

### **3. Exportar a Excel:**
```javascript
// Botón para descargar reporte
function exportarExcel() {
    // Generar archivo con todas las solicitudes
}
```

### **4. Comentarios Pre-definidos:**
```javascript
// Dropdown con comentarios comunes
- "Aprobado - Capacidad disponible"
- "Rechazado - Bus sin cupo"
- "Rechazado - Fecha muy próxima"
```

### **5. Historial de Cambios:**
```javascript
// Ver quién aprobó/rechazó y cuándo
- Auditoría completa
- Timeline de eventos
```

---

## 📝 **Notas Importantes**

### **Seguridad (TODO):**
```csharp
// Agregar en el futuro:
[Authorize(Roles = "Admin")]
public class GestionarTrasladosModel : PageModel
```

**Pendiente**: Integración con módulo de Login del compañero.

---

### **Performance:**
- Carga solo solicitudes pendientes por defecto
- Filtrado en cliente para respuesta rápida
- Auto-recarga tras aprobar (1.5 seg delay)
- Sin polling constante (solo refresh manual)

---

## 🎉 **Resultado Final**

### **FASE 6 - 100% COMPLETADA**
✅ **Parte 1 - Backend**: DTOs, Services, Repositories
✅ **Parte 2 - Frontend Padre**: Modal, Historial, Filtros
✅ **Parte B - Indicadores**: Borde naranja, icono 🚌, alerta
✅ **Parte A - Panel Admin**: Aprobar/Rechazar con UI completa

---

**Estado**: ✅ **FASE 6 COMPLETADA**  
**Compilación**: ✅ Exitosa  
**Próximo**: Testing completo o siguiente fase

---

**Última actualización**: Abril 2026
