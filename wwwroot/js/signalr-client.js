/**
 * Cliente SignalR para Notificaciones en Tiempo Real
 * FASE 7 - TransportesGenesis
 * 
 * Uso:
 * <script src="~/js/signalr-client.js"></script>
 * <script>
 *   const cliente = new NotificacionesCliente();
 *   cliente.conectar();
 *   cliente.onBusCerca((data) => { console.log(data); });
 * </script>
 */

class NotificacionesCliente {
    constructor() {
        this.connection = null;
        this.estaConectado = false;
        this.callbacks = {};
        this.intentosReconexion = 0;
        this.maxIntentosReconexion = 5;
    }

    /**
     * Conectar al Hub de SignalR
     */
    async conectar() {
        try {
            // Crear conexión
            this.connection = new signalR.HubConnectionBuilder()
                .withUrl("/notificacionesHub")
                .withAutomaticReconnect([0, 2000, 5000, 10000, 30000]) // Intervalos de reconexión
                .configureLogging(signalR.LogLevel.Information)
                .build();

            // Registrar eventos de conexión
            this.connection.onreconnecting((error) => {
                console.warn('🔄 [SignalR] Reconectando...', error);
                this.estaConectado = false;
                this.notificarEstado('reconnecting');
            });

            this.connection.onreconnected((connectionId) => {
                console.log('✅ [SignalR] Reconectado exitosamente. Connection ID:', connectionId);
                this.estaConectado = true;
                this.intentosReconexion = 0;
                this.notificarEstado('connected');
                this.reUnirseAGrupos();
            });

            this.connection.onclose((error) => {
                console.error('❌ [SignalR] Conexión cerrada', error);
                this.estaConectado = false;
                this.notificarEstado('disconnected');
            });

            // Iniciar conexión
            await this.connection.start();
            this.estaConectado = true;
            console.log('✅ [SignalR] Conectado exitosamente. Connection ID:', this.connection.connectionId);
            this.notificarEstado('connected');

            // Registrar listeners
            this.registrarListeners();

            return true;
        } catch (error) {
            console.error('❌ [SignalR] Error al conectar:', error);
            this.estaConectado = false;
            this.notificarEstado('error');

            // Reintentar conexión
            if (this.intentosReconexion < this.maxIntentosReconexion) {
                this.intentosReconexion++;
                console.log(`🔄 [SignalR] Reintentando conexión (${this.intentosReconexion}/${this.maxIntentosReconexion})...`);
                setTimeout(() => this.conectar(), 5000);
            }

            return false;
        }
    }

    /**
     * Desconectar del Hub
     */
    async desconectar() {
        if (this.connection) {
            try {
                await this.connection.stop();
                console.log('🔌 [SignalR] Desconectado');
                this.estaConectado = false;
                this.notificarEstado('disconnected');
            } catch (error) {
                console.error('❌ [SignalR] Error al desconectar:', error);
            }
        }
    }

    /**
     * Registrar todos los listeners de eventos del servidor
     */
    registrarListeners() {
        // Bus cercano a parada
        this.connection.on('BusCerca', (data) => {
            console.log('🚌 [SignalR] Bus cerca:', data);
            this.ejecutarCallback('busCerca', data);
        });

        // Parada completada
        this.connection.on('ParadaCompletada', (data) => {
            console.log('✅ [SignalR] Parada completada:', data);
            this.ejecutarCallback('paradaCompletada', data);
        });

        // Retraso reportado
        this.connection.on('RetrasoReportado', (data) => {
            console.log('⚠️ [SignalR] Retraso reportado:', data);
            this.ejecutarCallback('retraso', data);
        });

        // Mensaje broadcast
        this.connection.on('MensajeBroadcast', (data) => {
            console.log('📢 [SignalR] Mensaje broadcast:', data);
            this.ejecutarCallback('broadcast', data);
        });

        // Mensaje recibido
        this.connection.on('MensajeRecibido', (data) => {
            console.log('💬 [SignalR] Mensaje recibido:', data);
            this.ejecutarCallback('mensaje', data);
        });

        // Ubicación bus actualizada
        this.connection.on('UbicacionBusActualizada', (data) => {
            console.log('📍 [SignalR] Ubicación actualizada:', data);
            this.ejecutarCallback('ubicacionActualizada', data);
        });

        // Alertas de proximidad o llegada
        this.connection.on('AlertaRecibida', (data) => {
            console.log('🔔 [SignalR] Alerta recibida (grupo bus):', data);
            this.ejecutarCallback('alerta', data);
        });

        // Alertas personales dirigidas a un alumno/padre
        this.connection.on('AlertaPersonal', (data) => {
            console.log('🔔 [SignalR] Alerta personal:', data);
            this.ejecutarCallback('alertaPersonal', data);
        });

        // Echo response (testing)
        this.connection.on('EchoResponse', (data) => {
            console.log('🔊 [SignalR] Echo response:', data);
            this.ejecutarCallback('echo', data);
        });

        // Pong (testing)
        this.connection.on('Pong', (data) => {
            console.log('🏓 [SignalR] Pong:', data);
            this.ejecutarCallback('pong', data);
        });
    }

    /**
     * Unirse a un grupo (por IdBus, IdAlumno, etc.)
     */
    async unirseAGrupo(nombreGrupo) {
        if (!this.estaConectado) {
            console.warn('⚠️ [SignalR] No conectado, no se puede unir a grupo');
            return false;
        }

        try {
            await this.connection.invoke('UnirseAGrupo', nombreGrupo);
            console.log(`✅ [SignalR] Unido al grupo: ${nombreGrupo}`);

            // Guardar grupo para re-unirse después de reconexión
            if (!this.gruposUnidos) this.gruposUnidos = [];
            if (!this.gruposUnidos.includes(nombreGrupo)) {
                this.gruposUnidos.push(nombreGrupo);
            }

            return true;
        } catch (error) {
            console.error(`❌ [SignalR] Error al unirse al grupo ${nombreGrupo}:`, error);
            return false;
        }
    }

    /**
     * Salir de un grupo
     */
    async salirDeGrupo(nombreGrupo) {
        if (!this.estaConectado) {
            console.warn('⚠️ [SignalR] No conectado');
            return false;
        }

        try {
            await this.connection.invoke('SalirDeGrupo', nombreGrupo);
            console.log(`🚪 [SignalR] Salió del grupo: ${nombreGrupo}`);

            // Remover de lista de grupos
            if (this.gruposUnidos) {
                this.gruposUnidos = this.gruposUnidos.filter(g => g !== nombreGrupo);
            }

            return true;
        } catch (error) {
            console.error(`❌ [SignalR] Error al salir del grupo ${nombreGrupo}:`, error);
            return false;
        }
    }

    /**
     * Re-unirse a grupos después de reconexión
     */
    async reUnirseAGrupos() {
        if (this.gruposUnidos && this.gruposUnidos.length > 0) {
            console.log(`🔄 [SignalR] Re-uniéndose a ${this.gruposUnidos.length} grupo(s)...`);
            for (const grupo of this.gruposUnidos) {
                await this.unirseAGrupo(grupo);
            }
        }
    }

    /**
     * Reportar ubicación del bus (Piloto)
     */
    async reportarUbicacionBus(idBus, latitud, longitud) {
        if (!this.estaConectado) return false;

        try {
            await this.connection.invoke('ReportarUbicacionBus', idBus, latitud, longitud);
            return true;
        } catch (error) {
            console.error('❌ [SignalR] Error al reportar ubicación:', error);
            return false;
        }
    }

    /**
     * Notificar parada completada (Piloto)
     */
    async notificarParadaCompletada(idParada, idAlumno, nombreAlumno) {
        if (!this.estaConectado) return false;

        try {
            await this.connection.invoke('NotificarParadaCompletada', idParada, idAlumno, nombreAlumno);
            console.log(`✅ [SignalR] Parada ${idParada} notificada como completada`);
            return true;
        } catch (error) {
            console.error('❌ [SignalR] Error al notificar parada:', error);
            return false;
        }
    }

    /**
     * Reportar retraso (Piloto)
     */
    async reportarRetraso(idBus, minutosRetraso, motivo) {
        if (!this.estaConectado) return false;

        try {
            await this.connection.invoke('ReportarRetraso', idBus, minutosRetraso, motivo);
            console.log(`⚠️ [SignalR] Retraso reportado: ${minutosRetraso} min`);
            return true;
        } catch (error) {
            console.error('❌ [SignalR] Error al reportar retraso:', error);
            return false;
        }
    }

    /**
     * Enviar mensaje broadcast (Admin)
     */
    async enviarBroadcast(titulo, mensaje, tipo = 'info') {
        if (!this.estaConectado) return false;

        try {
            await this.connection.invoke('EnviarMensajeBroadcast', titulo, mensaje, tipo);
            console.log(`📢 [SignalR] Broadcast enviado: ${titulo}`);
            return true;
        } catch (error) {
            console.error('❌ [SignalR] Error al enviar broadcast:', error);
            return false;
        }
    }

    /**
     * Enviar mensaje a un bus (Admin)
     */
    async enviarMensajeABus(idBus, mensaje) {
        if (!this.estaConectado) return false;

        try {
            await this.connection.invoke('EnviarMensajeABus', idBus, mensaje);
            console.log(`💬 [SignalR] Mensaje enviado al Bus ${idBus}`);
            return true;
        } catch (error) {
            console.error('❌ [SignalR] Error al enviar mensaje:', error);
            return false;
        }
    }

    /**
     * Test Echo
     */
    async echo(mensaje) {
        if (!this.estaConectado) return false;

        try {
            await this.connection.invoke('Echo', mensaje);
            return true;
        } catch (error) {
            console.error('❌ [SignalR] Error en echo:', error);
            return false;
        }
    }

    /**
     * Test Ping
     */
    async ping() {
        if (!this.estaConectado) return false;

        try {
            await this.connection.invoke('Ping');
            return true;
        } catch (error) {
            console.error('❌ [SignalR] Error en ping:', error);
            return false;
        }
    }

    // ==================== CALLBACKS ====================

    onBusCerca(callback) {
        this.callbacks['busCerca'] = callback;
    }

    onParadaCompletada(callback) {
        this.callbacks['paradaCompletada'] = callback;
    }

    onRetraso(callback) {
        this.callbacks['retraso'] = callback;
    }

    onBroadcast(callback) {
        this.callbacks['broadcast'] = callback;
    }

    onMensaje(callback) {
        this.callbacks['mensaje'] = callback;
    }

    onUbicacionActualizada(callback) {
        this.callbacks['ubicacionActualizada'] = callback;
    }

    onAlerta(callback) {
        this.callbacks['alerta'] = callback;
    }

    onAlertaPersonal(callback) {
        this.callbacks['alertaPersonal'] = callback;
    }

    onEstadoConexion(callback) {
        this.callbacks['estado'] = callback;
    }

    onEcho(callback) {
        this.callbacks['echo'] = callback;
    }

    onPong(callback) {
        this.callbacks['pong'] = callback;
    }

    // ==================== HELPERS ====================

    ejecutarCallback(evento, data) {
        if (this.callbacks[evento]) {
            try {
                this.callbacks[evento](data);
            } catch (error) {
                console.error(`❌ [SignalR] Error en callback '${evento}':`, error);
            }
        }
    }

    notificarEstado(estado) {
        if (this.callbacks['estado']) {
            this.callbacks['estado'](estado);
        }
    }

    obtenerEstadoConexion() {
        return {
            conectado: this.estaConectado,
            connectionId: this.connection?.connectionId,
            estado: this.connection?.state,
            gruposUnidos: this.gruposUnidos || []
        };
    }
}

// Exportar para uso global
window.NotificacionesCliente = NotificacionesCliente;
