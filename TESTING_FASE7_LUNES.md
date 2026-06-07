# 🧪 TESTING FASE 7 - Notificaciones SignalR (Lunes 27/04/2026)

## ✅ Estado Actual (Domingo 26/04 - Tarde)
- **FASE 7 infraestructura implementada al 100%**
- **Hub SignalR configurado** (`NotificacionesHub.cs`)
- **Servicio de notificaciones** (`INotificacionService`)
- **Cliente JavaScript** (`signalr-client.js`)
- **Compilación exitosa** sin errores
- **Pendiente:** Testing funcional en día hábil

---

## 🎯 Objetivos del Testing

Validar todos los flujos de notificaciones en tiempo real:
1. ✅ Conexión al Hub de SignalR
2. ✅ Unirse a grupos (Bus, Alumno)
3. ✅ Notificación: Bus cerca de parada
4. ✅ Notificación: Parada completada
5. ✅ Notificación: Retraso reportado
6. ✅ Mensaje broadcast (Admin → Todos)
7. ✅ Actualización ubicación en tiempo real

---

## 📋 Plan de Testing (45-60 minutos)

### PREREQUISITO: SignalR Client Library

Antes de comenzar, agregar la librería SignalR en `_Layout.cshtml` o en páginas específicas:

```html
<!-- En Pages/Shared/_Layout.cshtml, antes de cerrar </body> -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.0/signalr.min.js"></script>
<script src="~/js/signalr-client.js"></script>
```

---

### PASO 1: Test de Conexión Básica (10 min)

#### 1.1 Crear Página de Testing SignalR

**Ubicación:** `Pages/Test/TestSignalR.cshtml`

```html
@page
@model TransportesGenesis.Pages.Test.TestSignalRModel
@{
    ViewData["Title"] = "Test SignalR";
}

<div class="container py-4">
    <h2>🧪 Test de Notificaciones SignalR</h2>
    <hr>

    <!-- Estado de Conexión -->
    <div class="card mb-4">
        <div class="card-header bg-info text-white">
            <h5>📡 Estado de Conexión</h5>
        </div>
        <div class="card-body">
            <div id="estadoConexion" class="alert alert-secondary">
                ⏳ No conectado
            </div>
            <button id="btnConectar" class="btn btn-success me-2">
                Conectar
            </button>
            <button id="btnDesconectar" class="btn btn-danger" disabled>
                Desconectar
            </button>
        </div>
    </div>

    <!-- Test 1: Echo/Ping -->
    <div class="card mb-4">
        <div class="card-header bg-primary text-white">
            <h5>🔊 TEST 1: Echo y Ping</h5>
        </div>
        <div class="card-body">
            <div class="mb-3">
                <label>Mensaje Echo:</label>
                <input type="text" id="txtEcho" class="form-control" value="Hola SignalR!" />
            </div>
            <button id="btnEcho" class="btn btn-primary me-2" disabled>
                Enviar Echo
            </button>
            <button id="btnPing" class="btn btn-info" disabled>
                Enviar Ping
            </button>
            <div id="resultadoEcho" class="mt-3"></div>
        </div>
    </div>

    <!-- Test 2: Unirse a Grupos -->
    <div class="card mb-4">
        <div class="card-header bg-success text-white">
            <h5>👥 TEST 2: Grupos</h5>
        </div>
        <div class="card-body">
            <div class="mb-3">
                <label>Nombre del Grupo:</label>
                <input type="text" id="txtGrupo" class="form-control" value="Bus_4" 
                       placeholder="Ej: Bus_4, Alumno_1" />
            </div>
            <button id="btnUnirGrupo" class="btn btn-success me-2" disabled>
                Unirse a Grupo
            </button>
            <button id="btnSalirGrupo" class="btn btn-warning" disabled>
                Salir de Grupo
            </button>
            <div id="gruposActuales" class="mt-3">
                <strong>Grupos actuales:</strong> <span id="listaGrupos">Ninguno</span>
            </div>
        </div>
    </div>

    <!-- Test 3: Notificaciones (Simuladas desde Cliente) -->
    <div class="card mb-4">
        <div class="card-header bg-warning text-dark">
            <h5>📢 TEST 3: Enviar Notificaciones</h5>
        </div>
        <div class="card-body">
            <div class="row mb-3">
                <div class="col-md-6">
                    <label>ID Bus:</label>
                    <input type="number" id="txtIdBus" class="form-control" value="4" />
                </div>
                <div class="col-md-6">
                    <label>Latitud:</label>
                    <input type="text" id="txtLat" class="form-control" value="4.6935" />
                </div>
            </div>
            <div class="row mb-3">
                <div class="col-md-6">
                    <label>Longitud:</label>
                    <input type="text" id="txtLon" class="form-control" value="-74.0612" />
                </div>
                <div class="col-md-6">
                    <label>Minutos Retraso:</label>
                    <input type="number" id="txtMinutosRetraso" class="form-control" value="10" />
                </div>
            </div>
            <button id="btnReportarUbicacion" class="btn btn-primary me-2" disabled>
                Reportar Ubicación
            </button>
            <button id="btnReportarRetraso" class="btn btn-warning me-2" disabled>
                Reportar Retraso
            </button>
            <button id="btnBroadcast" class="btn btn-danger" disabled>
                Enviar Broadcast
            </button>
        </div>
    </div>

    <!-- Panel de Notificaciones Recibidas -->
    <div class="card">
        <div class="card-header bg-dark text-white">
            <h5>📬 Notificaciones Recibidas</h5>
        </div>
        <div class="card-body">
            <div id="notificaciones" style="max-height: 400px; overflow-y: auto;">
                <p class="text-muted">No hay notificaciones aún...</p>
            </div>
            <button id="btnLimpiarNotif" class="btn btn-sm btn-secondary mt-2">
                Limpiar
            </button>
        </div>
    </div>
</div>

@section Scripts {
<script>
    let cliente = null;

    // Inicializar al cargar página
    document.addEventListener('DOMContentLoaded', () => {
        cliente = new NotificacionesCliente();
        configurarEventos();
    });

    function configurarEventos() {
        // Botones de conexión
        document.getElementById('btnConectar').addEventListener('click', conectar);
        document.getElementById('btnDesconectar').addEventListener('click', desconectar);

        // Test 1: Echo/Ping
        document.getElementById('btnEcho').addEventListener('click', enviarEcho);
        document.getElementById('btnPing').addEventListener('click', enviarPing);

        // Test 2: Grupos
        document.getElementById('btnUnirGrupo').addEventListener('click', unirseAGrupo);
        document.getElementById('btnSalirGrupo').addEventListener('click', salirDeGrupo);

        // Test 3: Notificaciones
        document.getElementById('btnReportarUbicacion').addEventListener('click', reportarUbicacion);
        document.getElementById('btnReportarRetraso').addEventListener('click', reportarRetraso);
        document.getElementById('btnBroadcast').addEventListener('click', enviarBroadcast);

        // Limpiar notificaciones
        document.getElementById('btnLimpiarNotif').addEventListener('click', limpiarNotificaciones);

        // Configurar callbacks de cliente SignalR
        cliente.onEstadoConexion((estado) => {
            actualizarEstadoUI(estado);
        });

        cliente.onEcho((data) => {
            agregarNotificacion('🔊 Echo', data, 'primary');
        });

        cliente.onPong((data) => {
            agregarNotificacion('🏓 Pong', `Respuesta recibida: ${data}`, 'info');
        });

        cliente.onBusCerca((data) => {
            agregarNotificacion('🚌 Bus Cerca', JSON.stringify(data, null, 2), 'warning');
        });

        cliente.onParadaCompletada((data) => {
            agregarNotificacion('✅ Parada Completada', JSON.stringify(data, null, 2), 'success');
        });

        cliente.onRetraso((data) => {
            agregarNotificacion('⚠️ Retraso', JSON.stringify(data, null, 2), 'warning');
        });

        cliente.onBroadcast((data) => {
            agregarNotificacion('📢 Broadcast', JSON.stringify(data, null, 2), 'danger');
        });

        cliente.onUbicacionActualizada((data) => {
            agregarNotificacion('📍 Ubicación', `Bus ${data.IdBus}: ${data.Latitud}, ${data.Longitud}`, 'info');
        });
    }

    async function conectar() {
        const exito = await cliente.conectar();
        if (exito) {
            habilitarBotones(true);
        }
    }

    async function desconectar() {
        await cliente.desconectar();
        habilitarBotones(false);
    }

    async function enviarEcho() {
        const mensaje = document.getElementById('txtEcho').value;
        await cliente.echo(mensaje);
    }

    async function enviarPing() {
        await cliente.ping();
    }

    async function unirseAGrupo() {
        const grupo = document.getElementById('txtGrupo').value;
        await cliente.unirseAGrupo(grupo);
        actualizarListaGrupos();
    }

    async function salirDeGrupo() {
        const grupo = document.getElementById('txtGrupo').value;
        await cliente.salirDeGrupo(grupo);
        actualizarListaGrupos();
    }

    async function reportarUbicacion() {
        const idBus = parseInt(document.getElementById('txtIdBus').value);
        const lat = parseFloat(document.getElementById('txtLat').value);
        const lon = parseFloat(document.getElementById('txtLon').value);
        await cliente.reportarUbicacionBus(idBus, lat, lon);
    }

    async function reportarRetraso() {
        const idBus = parseInt(document.getElementById('txtIdBus').value);
        const minutos = parseInt(document.getElementById('txtMinutosRetraso').value);
        await cliente.reportarRetraso(idBus, minutos, 'Tráfico pesado en Calle 72');
    }

    async function enviarBroadcast() {
        await cliente.enviarBroadcast(
            'Mantenimiento Programado',
            'El sistema estará en mantenimiento mañana de 2-4 AM',
            'warning'
        );
    }

    function habilitarBotones(habilitar) {
        document.getElementById('btnConectar').disabled = habilitar;
        document.getElementById('btnDesconectar').disabled = !habilitar;
        document.getElementById('btnEcho').disabled = !habilitar;
        document.getElementById('btnPing').disabled = !habilitar;
        document.getElementById('btnUnirGrupo').disabled = !habilitar;
        document.getElementById('btnSalirGrupo').disabled = !habilitar;
        document.getElementById('btnReportarUbicacion').disabled = !habilitar;
        document.getElementById('btnReportarRetraso').disabled = !habilitar;
        document.getElementById('btnBroadcast').disabled = !habilitar;
    }

    function actualizarEstadoUI(estado) {
        const divEstado = document.getElementById('estadoConexion');
        if (estado === 'connected') {
            divEstado.className = 'alert alert-success';
            divEstado.innerHTML = '✅ Conectado - Connection ID: ' + cliente.connection.connectionId;
        } else if (estado === 'reconnecting') {
            divEstado.className = 'alert alert-warning';
            divEstado.innerHTML = '🔄 Reconectando...';
        } else if (estado === 'disconnected') {
            divEstado.className = 'alert alert-danger';
            divEstado.innerHTML = '❌ Desconectado';
        } else {
            divEstado.className = 'alert alert-secondary';
            divEstado.innerHTML = '⏳ ' + estado;
        }
    }

    function actualizarListaGrupos() {
        const estado = cliente.obtenerEstadoConexion();
        const spanGrupos = document.getElementById('listaGrupos');
        if (estado.gruposUnidos && estado.gruposUnidos.length > 0) {
            spanGrupos.innerHTML = estado.gruposUnidos.join(', ');
        } else {
            spanGrupos.innerHTML = 'Ninguno';
        }
    }

    function agregarNotificacion(titulo, mensaje, tipo) {
        const divNotif = document.getElementById('notificaciones');
        const timestamp = new Date().toLocaleTimeString();

        // Remover mensaje "No hay notificaciones"
        if (divNotif.querySelector('.text-muted')) {
            divNotif.innerHTML = '';
        }

        const html = `
            <div class="alert alert-${tipo} alert-dismissible fade show mb-2" role="alert">
                <strong>${titulo}</strong> <small class="text-muted">(${timestamp})</small>
                <br>
                <pre class="mb-0 mt-2" style="font-size: 12px;">${mensaje}</pre>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `;
        divNotif.insertAdjacentHTML('afterbegin', html);
    }

    function limpiarNotificaciones() {
        document.getElementById('notificaciones').innerHTML = '<p class="text-muted">No hay notificaciones aún...</p>';
    }
</script>
}
```

#### 1.2 Crear PageModel Stub

**Ubicación:** `Pages/Test/TestSignalR.cshtml.cs`

```csharp
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TransportesGenesis.Pages.Test
{
    public class TestSignalRModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
```

#### 1.3 Ejecutar Test de Conexión

1. Compilar proyecto
2. Ejecutar aplicación (F5)
3. Navegar a: `https://localhost:XXXXX/Test/TestSignalR`
4. Click **"Conectar"**
5. **Verificar:**
   - ✅ Estado cambia a "✅ Conectado"
   - ✅ Connection ID aparece
   - ✅ Botones se habilitan
   - ✅ Console del navegador (F12) muestra logs de SignalR

#### 1.4 Test Echo y Ping

1. En campo "Mensaje Echo", escribir: `Hola desde el cliente`
2. Click **"Enviar Echo"**
3. **Verificar:**
   - ✅ Aparece notificación "🔊 Echo: Hola desde el cliente"

4. Click **"Enviar Ping"**
5. **Verificar:**
   - ✅ Aparece notificación "🏓 Pong" con timestamp

---

### PASO 2: Test de Grupos (15 min)

#### 2.1 Unirse a Grupo Bus

1. En campo "Nombre del Grupo", escribir: `Bus_4`
2. Click **"Unirse a Grupo"**
3. **Verificar:**
   - ✅ Console muestra "Unido al grupo: Bus_4"
   - ✅ "Grupos actuales" muestra "Bus_4"

#### 2.2 Abrir Segunda Pestaña (Simular Otro Usuario)

1. Abrir nueva pestaña en el mismo navegador
2. Ir a: `https://localhost:XXXXX/Test/TestSignalR`
3. Click **"Conectar"** (segundo cliente)
4. Unirse al mismo grupo: `Bus_4`
5. **Verificar:**
   - ✅ Ambos clientes conectados con ConnectionId diferentes
   - ✅ Ambos en grupo "Bus_4"

#### 2.3 Test de Comunicación Grupo

**En Cliente 1:**
1. Click **"Reportar Ubicación"**
2. **Verificar en Cliente 2:**
   - ✅ Aparece notificación "📍 Ubicación: Bus 4: 4.6935, -74.0612"

**En Cliente 2:**
1. Click **"Reportar Retraso"**
2. **Verificar en Cliente 1:**
   - ✅ Aparece notificación "⚠️ Retraso: 10 minutos - Tráfico pesado"

---

### PASO 3: Test Broadcast (10 min)

#### 3.1 Enviar Mensaje a Todos

1. En cualquier cliente, click **"Enviar Broadcast"**
2. **Verificar:**
   - ✅ **AMBOS** clientes reciben notificación "📢 Broadcast"
   - ✅ Mensaje: "Mantenimiento Programado..."

#### 3.2 Abrir Tercera Pestaña (Sin Unirse a Grupo)

1. Nueva pestaña → `/Test/TestSignalR`
2. Solo click **"Conectar"** (NO unirse a ningún grupo)
3. Desde Cliente 1, click **"Enviar Broadcast"**
4. **Verificar:**
   - ✅ Cliente 3 **también recibe** el broadcast
   - ✅ Broadcast llega a TODOS, estén o no en grupos

---

### PASO 4: Test Página Piloto Integrada (15 min)

#### 4.1 Integrar SignalR en Página Piloto

**Modificar:** `Pages/Piloto/MiRuta.cshtml`

Agregar al final del archivo, antes de `@section Scripts`:

```html
<!-- Indicador de conexión SignalR -->
<div class="position-fixed bottom-0 start-0 m-3">
    <div id="signalrStatus" class="badge bg-secondary">
        <i class="bi bi-wifi-off"></i> SignalR: Desconectado
    </div>
</div>
```

En la sección `@section Scripts`, agregar al inicio:

```javascript
// ============ SIGNALR INTEGRATION ============
let clienteSignalR = null;

async function inicializarSignalR() {
    clienteSignalR = new NotificacionesCliente();

    // Callbacks
    clienteSignalR.onEstadoConexion((estado) => {
        const badge = document.getElementById('signalrStatus');
        if (estado === 'connected') {
            badge.className = 'badge bg-success';
            badge.innerHTML = '<i class="bi bi-wifi"></i> SignalR: Conectado';
        } else if (estado === 'reconnecting') {
            badge.className = 'badge bg-warning';
            badge.innerHTML = '<i class="bi bi-wifi"></i> SignalR: Reconectando...';
        } else {
            badge.className = 'badge bg-danger';
            badge.innerHTML = '<i class="bi bi-wifi-off"></i> SignalR: Desconectado';
        }
    });

    // Listener: Parada completada (actualizar UI automáticamente)
    clienteSignalR.onParadaCompletada((data) => {
        console.log('[PILOTO] Parada completada via SignalR:', data);
        // Actualizar UI automáticamente
        actualizarParadaUI(data.IdParada, true);
        mostrarToast(`✅ ${data.Mensaje}`);
    });

    // Listener: Mensaje recibido
    clienteSignalR.onMensaje((data) => {
        mostrarToast(`💬 Mensaje del admin: ${data.Mensaje}`, 'info');
    });

    // Conectar
    const conectado = await clienteSignalR.conectar();
    if (conectado) {
        // Unirse al grupo del bus
        await clienteSignalR.unirseAGrupo('Bus_@Model.IdBus');
        console.log('[PILOTO] Unido al grupo Bus_@Model.IdBus');
    }
}

// Modificar función marcarCompletada para enviar notificación SignalR
async function marcarCompletada(idParada, completada) {
    try {
        console.log(`Marcando parada ${idParada} como ${completada ? 'completada' : 'pendiente'}`);

        // Llamada a API (existente)
        const response = await fetch(`/api/rutas/@rutaActiva.IdRuta/parada/${idParada}/completar`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Completada: completada })
        });

        if (!response.ok) throw new Error('Error al marcar parada');

        const resultado = await response.json();
        console.log('Resultado API:', resultado);

        // Actualizar UI
        actualizarParadaUI(idParada, completada);
        actualizarEstadisticas();
        mostrarToast(completada ? '✅ Parada marcada como completada' : '⏳ Parada marcada como pendiente');

        // NUEVO: Enviar notificación SignalR si está conectado
        if (clienteSignalR && clienteSignalR.estaConectado && completada) {
            const parada = rutaActiva.Paradas.find(p => p.IdParada === idParada);
            if (parada && parada.IdAlumno) {
                await clienteSignalR.notificarParadaCompletada(
                    idParada,
                    parada.IdAlumno,
                    parada.NombreAlumno
                );
                console.log('[PILOTO] Notificación SignalR enviada');
            }
        }

    } catch (error) {
        console.error('Error:', error);
        mostrarToast('❌ Error al marcar parada', 'error');
    }
}

// Inicializar SignalR al cargar página
window.addEventListener('DOMContentLoaded', () => {
    inicializarSignalR();
});
```

#### 4.2 Test Integración Piloto

1. Abrir `/Piloto/MiRuta` en navegador
2. **Verificar:**
   - ✅ Badge "SignalR: Conectado" en esquina inferior izquierda
   - ✅ Console muestra "Unido al grupo Bus_4"

3. Marcar una parada como completada
4. Abrir `/Test/TestSignalR` en otra pestaña
5. Unirse al grupo `Alumno_1` (o el IdAlumno de la parada marcada)
6. Marcar parada en Piloto
7. **Verificar:**
   - ✅ Notificación aparece en página de test
   - ✅ Mensaje: "✅ [Nombre] ha sido recogido/dejado"

---

### PASO 5: Test Página Padre (15 min)

#### 5.1 Crear Página Padre Simple

**Ubicación:** `Pages/Padre/Notificaciones.cshtml`

```html
@page
@{
    ViewData["Title"] = "Mis Notificaciones";
    var idAlumno = 1; // TODO: Obtener desde sesión
}

<div class="container py-4">
    <h2>🔔 Notificaciones de @(ViewData["NombreAlumno"] ?? "Mi Hijo")</h2>
    <hr>

    <!-- Estado SignalR -->
    <div id="signalrStatus" class="alert alert-info">
        ⏳ Conectando a notificaciones...
    </div>

    <!-- Notificaciones Recientes -->
    <div id="notificaciones">
        <p class="text-muted">No hay notificaciones recientes</p>
    </div>
</div>

@section Scripts {
<script>
    const idAlumno = @idAlumno;
    let cliente = null;

    async function inicializar() {
        cliente = new NotificacionesCliente();

        // Estado
        cliente.onEstadoConexion((estado) => {
            const div = document.getElementById('signalrStatus');
            if (estado === 'connected') {
                div.className = 'alert alert-success';
                div.innerHTML = '✅ Conectado a notificaciones en tiempo real';
            } else {
                div.className = 'alert alert-warning';
                div.innerHTML = '⚠️ ' + estado;
            }
        });

        // Parada completada
        cliente.onParadaCompletada((data) => {
            agregarNotificacion(
                '✅ Recogida Confirmada',
                data.Mensaje,
                'success'
            );
        });

        // Bus cerca
        cliente.onBusCerca((data) => {
            agregarNotificacion(
                '🚌 Bus Cerca',
                data.Mensaje,
                'warning'
            );
        });

        // Retraso
        cliente.onRetraso((data) => {
            agregarNotificacion(
                '⚠️ Retraso',
                data.Mensaje,
                'warning'
            );
        });

        // Broadcast
        cliente.onBroadcast((data) => {
            agregarNotificacion(
                '📢 ' + data.Titulo,
                data.Mensaje,
                data.Tipo
            );
        });

        // Conectar y unirse a grupo del alumno
        await cliente.conectar();
        await cliente.unirseAGrupo(`Alumno_${idAlumno}`);
        await cliente.unirseAGrupo('Bus_4'); // TODO: Obtener IdBus del alumno
    }

    function agregarNotificacion(titulo, mensaje, tipo) {
        const div = document.getElementById('notificaciones');

        if (div.querySelector('.text-muted')) {
            div.innerHTML = '';
        }

        const tipoClase = {
            'success': 'alert-success',
            'warning': 'alert-warning',
            'error': 'alert-danger',
            'info': 'alert-info'
        }[tipo] || 'alert-info';

        const html = `
            <div class="${tipoClase} alert alert-dismissible fade show">
                <strong>${titulo}</strong>
                <p class="mb-0">${mensaje}</p>
                <small class="text-muted">${new Date().toLocaleTimeString()}</small>
                <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
            </div>
        `;
        div.insertAdjacentHTML('afterbegin', html);
    }

    window.addEventListener('DOMContentLoaded', inicializar);
</script>
}
```

#### 5.2 Test Flujo Completo Padre-Piloto

**Setup:**
1. Pestaña 1: `/Piloto/MiRuta` (piloto del Bus 4)
2. Pestaña 2: `/Padre/Notificaciones` (padre de alumno ID 1)

**Flujo:**
1. Piloto marca parada de Alumno 1 como completada
2. **Verificar en Padre:**
   - ✅ Notificación aparece instantáneamente
   - ✅ Mensaje: "✅ [Nombre Alumno] ha sido recogido/dejado"
   - ✅ Sin recargar página

---

### PASO 6: Test Performance y Reconexión (10 min)

#### 6.1 Test Reconexión Automática

1. Abrir `/Test/TestSignalR`
2. Conectar y unirse a grupo "Bus_4"
3. **Detener el servidor** (Stop Debugging en Visual Studio)
4. **Verificar:**
   - ✅ Badge cambia a "🔄 Reconectando..."
   - ✅ Console muestra intentos de reconexión

5. **Reiniciar servidor** (F5)
6. **Verificar:**
   - ✅ Cliente se reconecta automáticamente
   - ✅ Badge cambia a "✅ Conectado"
   - ✅ Se re-une al grupo "Bus_4" automáticamente

#### 6.2 Test Múltiples Clientes

1. Abrir 5 pestañas con `/Test/TestSignalR`
2. Conectar todas al grupo "Bus_4"
3. Desde una pestaña, enviar broadcast
4. **Verificar:**
   - ✅ **Las 5 pestañas** reciben el mensaje
   - ✅ Sin lag perceptible (<500ms)

#### 6.3 Test Actualizaciones Rápidas (Ubicación)

1. En `/Test/TestSignalR`, conectar y unirse a "Bus_4"
2. Abrir Console del navegador (F12)
3. Ejecutar script para simular actualizaciones cada segundo:

```javascript
let intervalo = setInterval(async () => {
    const lat = 4.6935 + (Math.random() - 0.5) * 0.01;
    const lon = -74.0612 + (Math.random() - 0.5) * 0.01;
    await cliente.reportarUbicacionBus(4, lat, lon);
}, 1000);

// Para detener: clearInterval(intervalo);
```

4. **Verificar:**
   - ✅ Notificaciones de ubicación aparecen cada segundo
   - ✅ Sin errores en console
   - ✅ Sistema responde correctamente

---

## ✅ Checklist de Validación

### Conexión
- [ ] Conexión exitosa al Hub
- [ ] Connection ID asignado
- [ ] Reconexión automática funciona
- [ ] Desconexión limpia sin errores

### Grupos
- [ ] Unirse a grupo funciona
- [ ] Salir de grupo funciona
- [ ] Re-unirse después de reconexión
- [ ] Mensajes llegan solo a miembros del grupo

### Notificaciones
- [ ] Bus cerca: Notificación recibida
- [ ] Parada completada: Notificación recibida
- [ ] Retraso: Notificación recibida
- [ ] Broadcast: Llega a TODOS los clientes
- [ ] Mensaje a bus: Llega solo al grupo del bus

### Integración Páginas
- [ ] Página Piloto: Badge de conexión visible
- [ ] Página Piloto: Marcar parada envía notificación
- [ ] Página Padre: Recibe notificación parada completada
- [ ] Página Padre: Recibe notificación bus cerca
- [ ] Página Test: Todos los tests pasan

### Performance
- [ ] Reconexión automática <5 segundos
- [ ] Latencia de notificaciones <500ms
- [ ] Múltiples clientes (5+) sin problemas
- [ ] Actualizaciones rápidas (1/segundo) sin lag

---

## 🐛 Troubleshooting

### Problema 1: "Failed to connect to SignalR hub"
**Causa:** Hub no mapeado correctamente  
**Solución:**
- Verificar en `Startup.cs`: `app.MapHub<NotificacionesHub>("/notificacionesHub")`
- URL debe ser `/notificacionesHub` (mismo path)

### Problema 2: Notificaciones no llegan
**Causa:** No unido al grupo correcto  
**Solución:**
- Verificar nombre del grupo: `Bus_4` (case-sensitive)
- Console del cliente debe mostrar "Unido al grupo: Bus_4"
- Verificar logs del servidor: `[SIGNALR] Cliente ... unido a grupo`

### Problema 3: "SignalR is not defined"
**Causa:** Librería SignalR no cargada  
**Solución:**
- Agregar en `_Layout.cshtml`:
```html
<script src="https://cdnjs.cloudflare.com/ajax/libs/microsoft-signalr/8.0.0/signalr.min.js"></script>
```
- Verificar que carga antes de `signalr-client.js`

### Problema 4: Reconexión no funciona
**Causa:** Configuración de reconexión no habilitada  
**Solución:**
- Verificar en `signalr-client.js`: `.withAutomaticReconnect()`
- Verificar intervalos: `[0, 2000, 5000, 10000, 30000]`

### Problema 5: Errores CORS
**Causa:** Aplicación en diferentes dominios  
**Solución:**
- En `Startup.cs`, agregar antes de `AddSignalR()`:
```csharp
builder.Services.AddCors(options => {
    options.AddPolicy("SignalRCors", policy => {
        policy.WithOrigins("https://localhost:5001")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
```
- En `Configure()`: `app.UseCors("SignalRCors");`

---

## 📊 Resultados Esperados

Al finalizar el testing:

✅ **Conexión estable** con reconexión automática  
✅ **8 tipos de notificaciones** funcionando  
✅ **3 páginas integradas** (Piloto, Padre, Test)  
✅ **Grupos funcionando** correctamente  
✅ **Performance adecuado** (<500ms latencia)  
✅ **Sin errores** en console del navegador  
✅ **Sin errores** en logs del servidor  

**Tiempo invertido:** 45-60 minutos  
**Issues encontrados:** Documentar en GitHub Issues  

---

## 🚀 Después del Testing

Una vez validado:

1. **Commit:**
```bash
git add .
git commit -m "✅ FASE 7 validada - SignalR notificaciones funcionando"
git push origin dev_david
```

2. **Siguiente fase:** Integrar notificaciones automáticas basadas en GPS
   - Detectar cuando bus está <1km de parada
   - Enviar notificación "Bus cerca" automáticamente
   - Requiere servicio background con IHostedService

3. **Mejoras opcionales:**
   - Panel admin para enviar broadcasts
   - Historial de notificaciones en BD
   - Notificaciones push (PWA)
   - Sonido/vibración en notificaciones

---

**Preparado por:** David  
**Fecha:** 26/04/2026 - Tarde  
**Para ejecutar el:** Lunes 27/04/2026  
**Duración estimada:** 45-60 minutos  
**Prerequisito:** FASE 7 implementada al 100% ✅
