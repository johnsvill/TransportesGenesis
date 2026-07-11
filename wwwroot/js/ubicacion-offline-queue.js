/**
 * Cola offline de ubicaciones GPS del bus.
 * Si falla el POST a /api/ubicaciones, encola en localStorage y sincroniza al recuperar red.
 */
(function (global) {
    'use strict';

    const STORAGE_KEY = 'tg_ubicacion_offline_queue';
    const MAX_POINTS = 300; // ~10 min a 1 punto / 2s

    function loadQueue() {
        try {
            const raw = localStorage.getItem(STORAGE_KEY);
            if (!raw) return [];
            const arr = JSON.parse(raw);
            return Array.isArray(arr) ? arr : [];
        } catch {
            return [];
        }
    }

    function saveQueue(queue) {
        try {
            localStorage.setItem(STORAGE_KEY, JSON.stringify(queue.slice(-MAX_POINTS)));
        } catch (e) {
            console.warn('[OfflineQueue] No se pudo guardar cola:', e);
        }
    }

    function enqueue(point) {
        const queue = loadQueue();
        queue.push({
            idBus: point.idBus,
            latitud: point.latitud,
            longitud: point.longitud,
            velocidad: point.velocidad ?? 10,
            direccion: point.direccion ?? 0,
            timestamp: point.timestamp || new Date().toISOString()
        });
        saveQueue(queue);
        notifyListeners();
        return queue.length;
    }

    function size() {
        return loadQueue().length;
    }

    function clear() {
        saveQueue([]);
        notifyListeners();
    }

    const listeners = [];
    function onChange(fn) {
        if (typeof fn === 'function') listeners.push(fn);
    }
    function notifyListeners() {
        const n = size();
        listeners.forEach(fn => {
            try { fn(n); } catch { /* ignore */ }
        });
    }

    /**
     * Envía un punto a la API. Si falla, lo encola.
     * También intenta fallback hub (broadcast efímero) sin dejar de encolar.
     */
    async function enviarUbicacion(point, options) {
        const opts = options || {};
        const payload = {
            idBus: parseInt(point.idBus, 10),
            latitud: point.latitud,
            longitud: point.longitud,
            velocidad: point.velocidad ?? 10,
            direccion: point.direccion ?? 0
        };

        if (!navigator.onLine) {
            enqueue({ ...payload, timestamp: new Date().toISOString() });
            if (typeof opts.onHubFallback === 'function') {
                try { opts.onHubFallback(payload); } catch { /* ignore */ }
            }
            return { ok: false, queued: true, reason: 'offline' };
        }

        try {
            const r = await fetch('/api/ubicaciones', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (!r.ok) throw new Error('API ' + r.status);
            return { ok: true, queued: false };
        } catch (err) {
            enqueue({ ...payload, timestamp: new Date().toISOString() });
            if (typeof opts.onHubFallback === 'function') {
                try { opts.onHubFallback(payload); } catch { /* ignore */ }
            }
            return { ok: false, queued: true, reason: err.message || 'error' };
        }
    }

    let flushing = false;

    /**
     * Vacía la cola en orden cronológico.
     * Preferencia: lote /api/ubicaciones/lote; fallback: POST unitario.
     */
    async function flush() {
        if (flushing || !navigator.onLine) return { flushed: 0, remaining: size() };
        flushing = true;
        notifyListeners();

        try {
            let queue = loadQueue();
            if (!queue.length) return { flushed: 0, remaining: 0 };

            queue = queue.slice().sort((a, b) =>
                String(a.timestamp).localeCompare(String(b.timestamp)));

            // Intentar lote primero
            try {
                const r = await fetch('/api/ubicaciones/lote', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(queue.map(p => ({
                        idBus: p.idBus,
                        latitud: p.latitud,
                        longitud: p.longitud,
                        velocidad: p.velocidad,
                        direccion: p.direccion
                    })))
                });
                if (r.ok) {
                    clear();
                    return { flushed: queue.length, remaining: 0 };
                }
            } catch {
                // continuar con unitario
            }

            let flushed = 0;
            let remaining = [];
            for (let i = 0; i < queue.length; i++) {
                const p = queue[i];
                try {
                    const r = await fetch('/api/ubicaciones', {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({
                            idBus: p.idBus,
                            latitud: p.latitud,
                            longitud: p.longitud,
                            velocidad: p.velocidad,
                            direccion: p.direccion
                        })
                    });
                    if (!r.ok) throw new Error('API ' + r.status);
                    flushed++;
                } catch {
                    remaining = queue.slice(i);
                    break;
                }
            }
            saveQueue(remaining);
            notifyListeners();
            return { flushed, remaining: remaining.length };
        } finally {
            flushing = false;
            notifyListeners();
        }
    }

    function isFlushing() {
        return flushing;
    }

    // Auto-sync al volver online
    if (typeof window !== 'undefined') {
        window.addEventListener('online', () => {
            flush().then(r => {
                if (r.flushed) console.log(`[OfflineQueue] Sincronizados ${r.flushed} puntos`);
            });
        });
    }

    global.UbicacionOfflineQueue = {
        enqueue,
        size,
        clear,
        flush,
        enviarUbicacion,
        onChange,
        isFlushing
    };
})(typeof window !== 'undefined' ? window : globalThis);
