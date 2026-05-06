/**
 * AlertasSignalR.js - Cliente SignalR para alertas de proximidad
 * Módulo 3: Integración con Hub de Notificaciones
 * Sin afectar funcionalidad existente
 */

class AlertasSignalRClient {
    constructor() {
        this.connection = null;
        this.isConnected = false;
        this.alertasActivas = [];
        this.callbacks = {
            onAlertaRecibida: [],
            onAlertaPersonal: [],
            onAlertaResuelta: [],
            onAlertaConfirmada: []
        };

        // Auto-inicializar si hay un usuario logueado
        if (this.isUserLoggedIn()) {
            this.inicializar();
        }
    }

    /**
     * Inicializa la conexión SignalR para alertas
     */
    async inicializar() {
        try {
            // Usar la misma conexión que el Hub existente
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/notificacionesHub")
                .withAutomaticReconnect([0, 2000, 10000, 30000])
                .build();

            this.configurarEventos();
            await this.conectar();

            console.log("🔄 AlertasSignalR inicializado correctamente");
        } catch (error) {
            console.error("❌ Error inicializando AlertasSignalR:", error);
        }
    }

    /**
     * Configura los event listeners de SignalR para alertas
     */
    configurarEventos() {
        // Evento: Nueva alerta recibida (para todo el bus)
        this.connection.on("AlertaRecibida", (data) => {
            console.log("🚨 Nueva alerta recibida:", data);
            this.procesarAlertaRecibida(data);
        });

        // Evento: Alerta personal (específica para el alumno/padre)
        this.connection.on("AlertaPersonal", (data) => {
            console.log("🎯 Alerta personal recibida:", data);
            this.procesarAlertaPersonal(data);
        });

        // Evento: Alerta resuelta
        this.connection.on("AlertaResuelta", (data) => {
            console.log("✅ Alerta resuelta:", data);
            this.procesarAlertaResuelta(data);
        });

        // Evento: Confirmación de alerta procesada
        this.connection.on("AlertaConfirmada", (data) => {
            console.log("🔔 Confirmación de alerta:", data);
            this.procesarAlertaConfirmada(data);
        });

        // Eventos de conexión
        this.connection.onreconnecting((error) => {
            console.warn("⏳ Reconectando AlertasSignalR...", error);
            this.isConnected = false;
        });

        this.connection.onreconnected((connectionId) => {
            console.log("✅ AlertasSignalR reconectado:", connectionId);
            this.isConnected = true;
            this.unirseAGrupos();
        });

        this.connection.onclose((error) => {
            console.warn("❌ Conexión AlertasSignalR cerrada:", error);
            this.isConnected = false;
        });
    }

    /**
     * Establece la conexión con el Hub
     */
    async conectar() {
        try {
            await this.connection.start();
            this.isConnected = true;
            console.log("✅ Conectado a NotificacionesHub para alertas");

            // Unirse a grupos automáticamente
            await this.unirseAGrupos();
        } catch (error) {
            console.error("❌ Error conectando AlertasSignalR:", error);
            this.isConnected = false;
        }
    }

    /**
     * Se une automáticamente a grupos según el rol del usuario
     */
    async unirseAGrupos() {
        if (!this.isConnected) return;

        try {
            const userInfo = this.obtenerInfoUsuario();

            if (userInfo.rol === "PadreDeFamilia" && userInfo.idsBus?.length > 0) {
                // Unirse a grupos de buses de los hijos
                for (const idBus of userInfo.idsBus) {
                    await this.connection.invoke("UnirseAGrupo", `Bus_${idBus}`);
                    console.log(`👨‍👩‍👧‍👦 Unido al grupo Bus_${idBus}`);
                }

                // Unirse a grupos de alumnos
                if (userInfo.idsAlumno?.length > 0) {
                    for (const idAlumno of userInfo.idsAlumno) {
                        await this.connection.invoke("UnirseAGrupo", `Alumno_${idAlumno}`);
                        console.log(`👶 Unido al grupo Alumno_${idAlumno}`);
                    }
                }
            }

            if (userInfo.rol === "Administrador") {
                await this.connection.invoke("UnirseAGrupo", "Administradores");
                console.log("👨‍💼 Unido al grupo Administradores");
            }

            if (userInfo.rol === "Piloto") {
                await this.connection.invoke("UnirseAGrupo", "Pilotos");
                console.log("🚌 Unido al grupo Pilotos");
            }
        } catch (error) {
            console.error("❌ Error uniéndose a grupos:", error);
        }
    }

    /**
     * Procesa una alerta recibida (para todo el bus)
     */
    procesarAlertaRecibida(data) {
        this.alertasActivas.push(data);

        // Mostrar notificación visual
        this.mostrarNotificacionAlerta(data, 'general');

        // Llamar callbacks registrados
        this.callbacks.onAlertaRecibida.forEach(callback => {
            try { callback(data); } catch (e) { console.error(e); }
        });
    }

    /**
     * Procesa una alerta personal (específica para el usuario)
     */
    procesarAlertaPersonal(data) {
        // Alerta personal es más importante
        this.mostrarNotificacionAlerta(data, 'personal');

        // Si requiere confirmación, mostrar botón
        if (data.RequiereConfirmacion && data.TipoAlerta === 'proximidad') {
            this.mostrarBotonConfirmacion(data);
        }

        // Llamar callbacks registrados
        this.callbacks.onAlertaPersonal.forEach(callback => {
            try { callback(data); } catch (e) { console.error(e); }
        });
    }

    /**
     * Procesa resolución de alerta
     */
    procesarAlertaResuelta(data) {
        // Remover de alertas activas
        this.alertasActivas = this.alertasActivas.filter(a => a.IdAlerta !== data.IdAlerta);

        // Mostrar notificación de resolución
        this.mostrarNotificacionResolucion(data);

        // Llamar callbacks registrados
        this.callbacks.onAlertaResuelta.forEach(callback => {
            try { callback(data); } catch (e) { console.error(e); }
        });
    }

    /**
     * Procesa confirmación de alerta
     */
    procesarAlertaConfirmada(data) {
        this.callbacks.onAlertaConfirmada.forEach(callback => {
            try { callback(data); } catch (e) { console.error(e); }
        });
    }

    /**
     * Confirma una alerta enviando respuesta al servidor
     */
    async confirmarAlerta(idAlerta) {
        if (!this.isConnected) {
            console.warn("⚠️ No conectado - no se puede confirmar alerta");
            return false;
        }

        try {
            const userInfo = this.obtenerInfoUsuario();
            await this.connection.invoke("ConfirmarAlerta", idAlerta, userInfo.id);

            console.log(`✅ Alerta ${idAlerta} confirmada`);

            // Remover botón de confirmación si existe
            this.removerBotonConfirmacion(idAlerta);

            return true;
        } catch (error) {
            console.error("❌ Error confirmando alerta:", error);
            return false;
        }
    }

    /**
     * Muestra notificación visual de alerta
     */
    mostrarNotificacionAlerta(data, tipo) {
        // Usar Toastr, SweetAlert o sistema de notificaciones existente
        const mensaje = data.Mensaje;
        const tipoNotificacion = data.TipoAlerta === 'proximidad' ? 'info' : 'warning';
        const esPersonal = tipo === 'personal';

        // Si existe Toastr
        if (typeof toastr !== 'undefined') {
            const titulo = esPersonal ? `🎯 ${data.TipoAlerta.toUpperCase()}` : `🚨 ${data.TipoAlerta.toUpperCase()}`;

            if (tipoNotificacion === 'info') {
                toastr.info(mensaje, titulo, { 
                    timeOut: esPersonal ? 15000 : 8000,
                    extendedTimeOut: 3000,
                    closeButton: true,
                    progressBar: true
                });
            } else {
                toastr.warning(mensaje, titulo, { 
                    timeOut: esPersonal ? 20000 : 10000,
                    extendedTimeOut: 5000,
                    closeButton: true,
                    progressBar: true
                });
            }
        } else {
            // Fallback a alert del navegador
            alert(`${data.TipoAlerta.toUpperCase()}: ${mensaje}`);
        }

        // Reproducir sonido de notificación si es personal
        if (esPersonal && data.TipoAlerta === 'proximidad') {
            this.reproducirSonidoNotificacion();
        }
    }

    /**
     * Muestra botón de confirmación para alertas de proximidad
     */
    mostrarBotonConfirmacion(data) {
        // Crear botón flotante o modal de confirmación
        const alertaId = `alerta-confirmacion-${data.IdAlerta}`;

        // Evitar duplicados
        if (document.getElementById(alertaId)) return;

        const confirmacionDiv = document.createElement('div');
        confirmacionDiv.id = alertaId;
        confirmacionDiv.className = 'alerta-confirmacion-flotante';
        confirmacionDiv.innerHTML = `
            <div class="alerta-confirmacion-content">
                <div class="alerta-confirmacion-header">
                    <strong>🚌 El bus se acerca a tu parada</strong>
                </div>
                <div class="alerta-confirmacion-body">
                    <p>${data.Mensaje}</p>
                    ${data.ParadasRestantes ? `<p><small>Faltan ${data.ParadasRestantes} paradas</small></p>` : ''}
                </div>
                <div class="alerta-confirmacion-footer">
                    <button class="btn btn-success btn-sm" onclick="alertasSignalR.confirmarAlerta(${data.IdAlerta})">
                        ✅ Confirmar - Estaré esperando
                    </button>
                    <button class="btn btn-secondary btn-sm" onclick="alertasSignalR.removerBotonConfirmacion(${data.IdAlerta})">
                        ❌ Cerrar
                    </button>
                </div>
            </div>
        `;

        // Agregar estilos CSS inline para que funcione independientemente
        confirmacionDiv.style.cssText = `
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 9999;
            background: white;
            border: 2px solid #28a745;
            border-radius: 8px;
            padding: 15px;
            max-width: 350px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.3);
            animation: slideInRight 0.5s ease-out;
        `;

        document.body.appendChild(confirmacionDiv);

        // Auto-remover después de 30 segundos
        setTimeout(() => {
            this.removerBotonConfirmacion(data.IdAlerta);
        }, 30000);
    }

    /**
     * Remueve el botón de confirmación
     */
    removerBotonConfirmacion(idAlerta) {
        const elemento = document.getElementById(`alerta-confirmacion-${idAlerta}`);
        if (elemento) {
            elemento.remove();
        }
    }

    /**
     * Muestra notificación de resolución
     */
    mostrarNotificacionResolucion(data) {
        if (typeof toastr !== 'undefined') {
            toastr.success(data.Mensaje, "✅ RESUELTO", { 
                timeOut: 5000,
                closeButton: true,
                progressBar: true
            });
        }
    }

    /**
     * Reproduce sonido de notificación
     */
    reproducirSonidoNotificacion() {
        // Crear audio element con sonido de notificación
        try {
            const audio = new Audio('data:audio/wav;base64,UklGRnoGAABXQVZFZm10IBAAAAABAAEAQB8AAEAfAAABAAgAZGF0YQoGAACBhYqFbF1fdJivrJBhNjVgodDbq2EcBj+a2/LDciUFLIHO8tiJNwgZaLvt559NEAxQp+PwtmMcBjiR1/LMeSwFJHfH8N2QQAoUXrTp66hVFApGn+DyvmwhBie+2OvBcSAFLYDI9d+JOQcZXrjq5JJMEwxCpObyvWAhCCi60OyOOgAJZr3h8JJREwyKpuPrvo4sUyyK1+zJ');
            audio.volume = 0.3;
            audio.play().catch(e => console.log("No se pudo reproducir sonido:", e));
        } catch (error) {
            console.log("Error reproduciendo sonido:", error);
        }
    }

    /**
     * Registra callback para eventos específicos
     */
    on(evento, callback) {
        if (this.callbacks[evento]) {
            this.callbacks[evento].push(callback);
        }
    }

    /**
     * Obtiene información del usuario actual (implementar según tu sistema)
     */
    obtenerInfoUsuario() {
        // TODO: Implementar según el sistema de autenticación
        // Por ahora datos simulados para pruebas
        return {
            id: "user-123",
            rol: "PadreDeFamilia", // o "Administrador", "Piloto"
            idsBus: [1, 2], // IDs de buses de los hijos
            idsAlumno: [5, 8] // IDs de alumnos/hijos
        };
    }

    /**
     * Verifica si hay un usuario logueado
     */
    isUserLoggedIn() {
        // TODO: Implementar según tu sistema
        return true; // Por ahora siempre true para pruebas
    }

    /**
     * Desconecta el cliente SignalR
     */
    async desconectar() {
        if (this.connection) {
            await this.connection.stop();
            this.isConnected = false;
            console.log("🔌 AlertasSignalR desconectado");
        }
    }

    /**
     * Obtiene alertas activas
     */
    obtenerAlertasActivas() {
        return [...this.alertasActivas];
    }

    /**
     * Estado de conexión
     */
    estaConectado() {
        return this.isConnected;
    }
}

// Instancia global para usar en toda la aplicación
window.alertasSignalR = new AlertasSignalRClient();

// CSS adicional para botones de confirmación
const css = `
@keyframes slideInRight {
    from { transform: translateX(100%); opacity: 0; }
    to { transform: translateX(0); opacity: 1; }
}

.alerta-confirmacion-flotante {
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, sans-serif;
}

.alerta-confirmacion-header {
    color: #28a745;
    font-size: 14px;
    margin-bottom: 8px;
}

.alerta-confirmacion-body {
    color: #333;
    font-size: 13px;
    margin-bottom: 12px;
}

.alerta-confirmacion-footer button {
    margin: 2px;
    padding: 4px 8px;
    border: none;
    border-radius: 4px;
    font-size: 11px;
    cursor: pointer;
}
`;

// Agregar CSS al documento
const style = document.createElement('style');
style.textContent = css;
document.head.appendChild(style);

console.log("📡 AlertasSignalR Client cargado y listo");