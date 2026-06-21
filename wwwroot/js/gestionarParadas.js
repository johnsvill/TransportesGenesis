// Gestión de Paradas con Mapa Interactivo
let mapa;
let marcadores = [];
let capasRuta = [];
let modoAgregar = false;
let todasLasParadas = [];
let rutaSeleccionadaId = null;
let geocodificacionExitosa = false;
let configParadas = { buses: [], rutas: [], alumnosSinAsignar: [] };

document.addEventListener('DOMContentLoaded', function () {
    cargarConfigParadas();
    inicializarFiltros();
    inicializarMapa();
    configurarEventos();
    cargarParadas();
});

function cargarConfigParadas() {
    const dataEl = document.getElementById('config-paradas-data');
    let raw = null;

    if (dataEl && dataEl.textContent.trim()) {
        try {
            raw = JSON.parse(dataEl.textContent);
        } catch (error) {
            console.error('Error al parsear config-paradas-data:', error);
        }
    }

    if (!raw && window.configParadas) {
        raw = window.configParadas;
    }

    if (!raw) {
        console.warn('GestionarParadas: no se encontró configuración de buses/rutas.');
        return;
    }

    configParadas = {
        buses: normalizarLista(raw.buses || raw.Buses, normalizarBus),
        rutas: normalizarLista(raw.rutas || raw.Rutas, normalizarRuta),
        alumnosSinAsignar: normalizarLista(raw.alumnosSinAsignar || raw.AlumnosSinAsignar, normalizarAlumno),
        idRutaPreseleccionada: raw.idRutaPreseleccionada ?? raw.IdRutaPreseleccionada ?? null,
        idBusPreseleccionado: raw.idBusPreseleccionado ?? raw.IdBusPreseleccionado ?? null
    };
}

function normalizarLista(lista, fn) {
    if (!Array.isArray(lista)) return [];
    return lista.map(fn);
}

function normalizarBus(bus) {
    const capacidad = bus.capacidad ?? bus.Capacidad ?? 0;
    const alumnosAsignados = bus.alumnosAsignados ?? bus.AlumnosAsignados ?? 0;
    const cuposDisponibles = bus.cuposDisponibles ?? bus.CuposDisponibles ?? (capacidad - alumnosAsignados);

    return {
        idBus: bus.idBus ?? bus.IdBus,
        placa: bus.placa ?? bus.Placa ?? 'Sin placa',
        capacidad,
        alumnosAsignados,
        cuposDisponibles
    };
}

function normalizarRuta(ruta) {
    return {
        idRuta: ruta.idRuta ?? ruta.IdRuta,
        idBus: ruta.idBus ?? ruta.IdBus,
        nombre: ruta.nombre ?? ruta.Nombre ?? 'Ruta',
        tipoRuta: ruta.tipoRuta ?? ruta.TipoRuta ?? '',
        fecha: ruta.fecha ?? ruta.Fecha ?? ''
    };
}

function normalizarAlumno(alumno) {
    return {
        idAlumno: alumno.idAlumno ?? alumno.IdAlumno,
        nombreCompleto: alumno.nombreCompleto ?? alumno.NombreCompleto ?? `Alumno #${alumno.idAlumno ?? alumno.IdAlumno}`
    };
}

function obtenerRutas() {
    return configParadas.rutas || [];
}

function obtenerBusesFiltro() {
    return configParadas.buses || [];
}

function obtenerBusesConCupo() {
    return obtenerBusesFiltro().filter(bus => (bus.cuposDisponibles ?? 0) > 0);
}

function textoOpcionBus(bus, incluirCupos) {
    if (incluirCupos) {
        const libres = bus.cuposDisponibles ?? (bus.capacidad - (bus.alumnosAsignados || 0));
        return `${bus.placa} — ${libres} cupo(s) libre(s) de ${bus.capacidad}`;
    }
    return bus.placa;
}

function inicializarFiltros() {
    const selectBus = document.getElementById('filtro-bus');
    const selectRuta = document.getElementById('filtro-ruta');

    selectBus.innerHTML = '<option value="">-- Seleccione un bus --</option>';
    obtenerBusesFiltro().forEach(bus => {
        const opt = document.createElement('option');
        opt.value = bus.idBus;
        opt.textContent = textoOpcionBus(bus, true);
        selectBus.appendChild(opt);
    });

    selectBus.addEventListener('change', function () {
        actualizarRutasPorBus(this.value, selectRuta);
        rutaSeleccionadaId = null;
        actualizarBadgeRuta();
        refrescarVistaParadas();
    });

    selectRuta.addEventListener('change', function () {
        rutaSeleccionadaId = this.value ? parseInt(this.value) : null;
        actualizarBadgeRuta();
        refrescarVistaParadas();
    });

    if (configParadas.idBusPreseleccionado) {
        selectBus.value = configParadas.idBusPreseleccionado;
        actualizarRutasPorBus(configParadas.idBusPreseleccionado, selectRuta);
    }

    if (configParadas.idRutaPreseleccionada) {
        selectRuta.value = configParadas.idRutaPreseleccionada;
        rutaSeleccionadaId = parseInt(configParadas.idRutaPreseleccionada);
        actualizarBadgeRuta();
    }
}

function actualizarRutasPorBus(idBus, selectElement) {
    const selectRuta = selectElement || document.getElementById('filtro-ruta');
    selectRuta.innerHTML = '<option value="">-- Seleccione una ruta --</option>';

    if (!idBus) {
        selectRuta.disabled = true;
        return;
    }

    const rutasDelBus = obtenerRutas().filter(r => r.idBus == idBus);
    rutasDelBus.forEach(ruta => {
        const opt = document.createElement('option');
        opt.value = ruta.idRuta;
        opt.textContent = `${ruta.nombre} (${ruta.tipoRuta}) - ${ruta.fecha}`;
        selectRuta.appendChild(opt);
    });

    selectRuta.disabled = rutasDelBus.length === 0;
}

function actualizarBadgeRuta() {
    const badge = document.getElementById('badge-ruta-seleccionada');
    if (!rutaSeleccionadaId) {
        badge.className = 'badge bg-secondary fs-6 w-100 py-2';
        badge.textContent = 'Sin ruta seleccionada';
        return;
    }

    const ruta = obtenerRutas().find(r => r.idRuta == rutaSeleccionadaId);
    if (ruta) {
        badge.className = 'badge bg-primary fs-6 w-100 py-2';
        badge.textContent = `Ruta: ${ruta.nombre} (${ruta.tipoRuta})`;
    }
}

function obtenerParadasFiltradas() {
    if (!rutaSeleccionadaId) return todasLasParadas;
    return todasLasParadas.filter(p => p.idRuta == rutaSeleccionadaId);
}

function calcularSiguienteOrden() {
    const idRuta = parseInt(document.getElementById('parada-ruta-id').value) || rutaSeleccionadaId;
    const paradasRuta = idRuta
        ? todasLasParadas.filter(p => p.idRuta == idRuta)
        : obtenerParadasFiltradas();
    if (paradasRuta.length === 0) return 1;
    return Math.max(...paradasRuta.map(p => p.orden || 0)) + 1;
}

function coordenadasValidas(latitud, longitud) {
    return !isNaN(latitud) && !isNaN(longitud) && latitud !== 0 && longitud !== 0;
}

function obtenerCoordenadasFormulario() {
    return {
        latitud: parseFloat(document.getElementById('parada-lat').value),
        longitud: parseFloat(document.getElementById('parada-lng').value)
    };
}

function actualizarRequerimientoDireccion() {
    const { latitud, longitud } = obtenerCoordenadasFormulario();
    const tieneCoords = coordenadasValidas(latitud, longitud);
    const label = document.getElementById('label-parada-direccion');
    if (label) {
        label.textContent = tieneCoords
            ? 'Dirección de la Parada (opcional)'
            : 'Dirección de la Parada *';
    }
}

function poblarBusesModal(idBusSeleccionado) {
    const selectBus = document.getElementById('parada-bus');
    selectBus.innerHTML = '<option value="">-- Seleccione un bus --</option>';

    obtenerBusesConCupo().forEach(bus => {
        const opt = document.createElement('option');
        opt.value = bus.idBus;
        opt.textContent = textoOpcionBus(bus, true);
        selectBus.appendChild(opt);
    });

    if (idBusSeleccionado) {
        selectBus.value = idBusSeleccionado;
    }
}

function poblarRutasModal(idBus, idRutaSeleccionada) {
    const selectRuta = document.getElementById('parada-ruta-modal');
    const hiddenRuta = document.getElementById('parada-ruta-id');

    selectRuta.innerHTML = '<option value="">-- Seleccione una ruta --</option>';
    hiddenRuta.value = '';

    if (!idBus) {
        selectRuta.disabled = true;
        return;
    }

    const rutasDelBus = obtenerRutas().filter(r => r.idBus == idBus);
    rutasDelBus.forEach(ruta => {
        const opt = document.createElement('option');
        opt.value = ruta.idRuta;
        opt.textContent = `${ruta.nombre} (${ruta.tipoRuta}) - ${ruta.fecha}`;
        selectRuta.appendChild(opt);
    });

    selectRuta.disabled = rutasDelBus.length === 0;

    if (idRutaSeleccionada && rutasDelBus.some(r => r.idRuta == idRutaSeleccionada)) {
        selectRuta.value = idRutaSeleccionada;
        hiddenRuta.value = idRutaSeleccionada;
    } else if (rutasDelBus.length === 1) {
        selectRuta.value = rutasDelBus[0].idRuta;
        hiddenRuta.value = rutasDelBus[0].idRuta;
    }
}

function poblarAlumnosModal(idAlumnoActual) {
    const selectAlumno = document.getElementById('parada-alumno');
    selectAlumno.innerHTML = '<option value="">Parada del colegio (sin alumno)</option>';

    (configParadas.alumnosSinAsignar || []).forEach(alumno => {
        const opt = document.createElement('option');
        opt.value = alumno.idAlumno;
        opt.textContent = alumno.nombreCompleto;
        selectAlumno.appendChild(opt);
    });

    if (idAlumnoActual) {
        const yaListado = (configParadas.alumnosSinAsignar || []).some(a => a.idAlumno == idAlumnoActual);
        if (!yaListado) {
            const paradaActual = todasLasParadas.find(p => p.idAlumno == idAlumnoActual);
            const opt = document.createElement('option');
            opt.value = idAlumnoActual;
            opt.textContent = paradaActual?.nombreAlumno || `Alumno #${idAlumnoActual}`;
            selectAlumno.appendChild(opt);
        }
        selectAlumno.value = idAlumnoActual;
    }
}

function sincronizarModalBusRuta(idBus, idRuta) {
    poblarBusesModal(idBus);
    poblarRutasModal(idBus, idRuta);
}

function limpiarRutaEnMapa() {
    capasRuta.forEach(capa => mapa.removeLayer(capa));
    capasRuta = [];
}

function obtenerCoordsOrdenadas(paradas) {
    return paradas
        .filter(p => {
            const lat = parseFloat(p.latitud);
            const lng = parseFloat(p.longitud);
            return !isNaN(lat) && !isNaN(lng) && lat !== 0 && lng !== 0;
        })
        .sort((a, b) => (a.orden || 0) - (b.orden || 0))
        .map(p => [parseFloat(p.latitud), parseFloat(p.longitud)]);
}

function agregarPolylineRuta(coords) {
    if (coords.length < 2) return;

    const borde = L.polyline(coords, {
        color: '#FFFFFF',
        weight: 9,
        opacity: 0.95,
        lineJoin: 'round',
        lineCap: 'round'
    }).addTo(mapa);

    const linea = L.polyline(coords, {
        color: '#C1121F',
        weight: 5,
        opacity: 0.95,
        lineJoin: 'round',
        lineCap: 'round'
    }).addTo(mapa);

    capasRuta.push(borde, linea);
}

function dibujarRutaEnMapa(paradas) {
    limpiarRutaEnMapa();

    // Solo dibujar la línea cuando hay una ruta seleccionada
    if (!rutaSeleccionadaId) return;

    agregarPolylineRuta(obtenerCoordsOrdenadas(paradas));
}

function obtenerExtensionKm(coords) {
    if (coords.length < 2) return 0;
    const bounds = L.latLngBounds(coords);
    return bounds.getNorthEast().distanceTo(bounds.getSouthWest()) / 1000;
}

function calcularZoomObjetivo(cantidad, extensionKm, rutaFiltrada) {
    let maxZoom = 17;

    if (extensionKm > 10) maxZoom = 11;
    else if (extensionKm > 6) maxZoom = 12;
    else if (extensionKm > 4) maxZoom = 13;
    else if (extensionKm > 2.5) maxZoom = 14;
    else if (extensionKm > 1.5) maxZoom = 15;
    else if (extensionKm > 0.8) maxZoom = 16;

    if (!rutaFiltrada) {
        maxZoom = Math.min(maxZoom, 13);
    }

    if (cantidad <= 3 && extensionKm < 2 && rutaFiltrada) {
        maxZoom = Math.max(maxZoom, 16);
    }

    return maxZoom;
}

function calcularPadding(cantidad, extensionKm) {
    if (cantidad <= 2) return [80, 80];
    if (cantidad <= 4) return [65, 65];
    if (cantidad <= 8) return [55, 55];
    if (extensionKm > 6) return [35, 35];
    return [45, 45];
}

function ajustarVistaMapa(paradas) {
    const coords = obtenerCoordsOrdenadas(paradas);
    const centroCiudad = [14.6349, -90.5069];

    if (coords.length === 0) {
        mapa.setView(centroCiudad, 15);
        return;
    }

    if (coords.length === 1) {
        mapa.setView(coords[0], 16);
        return;
    }

    const extensionKm = obtenerExtensionKm(coords);
    const cantidad = coords.length;
    const rutaFiltrada = Boolean(rutaSeleccionadaId);
    const maxZoom = calcularZoomObjetivo(cantidad, extensionKm, rutaFiltrada);
    const padding = calcularPadding(cantidad, extensionKm);
    const bounds = L.latLngBounds(coords);

    mapa.fitBounds(bounds, { padding, maxZoom });

    // Rutas compactas: acercar lo suficiente para leer la línea
    if (rutaFiltrada && extensionKm < 2 && mapa.getZoom() < 14) {
        mapa.setZoom(14);
    }

    // Sin filtro y paradas muy dispersas: no alejar más de lo necesario
    if (!rutaFiltrada && mapa.getZoom() < 11) {
        mapa.setZoom(11);
    }

    setTimeout(() => mapa.invalidateSize(), 100);
}

function inicializarMapa() {
    mapa = L.map('mapa-paradas').setView([14.6349, -90.5069], 15);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap contributors'
    }).addTo(mapa);

    mapa.on('click', function (e) {
        if (modoAgregar) {
            agregarParadaTemporal(e.latlng);
        }
    });
}

function configurarEventos() {
    document.getElementById('btn-agregar-parada').addEventListener('click', function () {
        if (!rutaSeleccionadaId) {
            mostrarNotificacion('Seleccione un bus y una ruta antes de agregar paradas', 'warning');
            return;
        }

        modoAgregar = !modoAgregar;
        this.classList.toggle('active');

        const container = mapa.getContainer();
        if (modoAgregar) {
            container.classList.add('crosshair');
            mostrarNotificacion('Haz clic en el mapa para agregar una parada', 'info');
        } else {
            container.classList.remove('crosshair');
        }
    });

    document.getElementById('btn-refrescar').addEventListener('click', cargarParadas);
    document.getElementById('btn-guardar-parada').addEventListener('click', guardarParada);

    document.getElementById('parada-bus').addEventListener('change', function () {
        poblarRutasModal(this.value, null);
    });

    document.getElementById('parada-ruta-modal').addEventListener('change', function () {
        document.getElementById('parada-ruta-id').value = this.value || '';
        document.getElementById('parada-orden').value = calcularSiguienteOrden();
    });

    document.getElementById('parada-lat').addEventListener('input', actualizarRequerimientoDireccion);
    document.getElementById('parada-lng').addEventListener('input', actualizarRequerimientoDireccion);

    document.body.addEventListener('click', function (e) {
        const target = e.target.closest('#btn-geocodificar');
        if (target) {
            e.preventDefault();
            e.stopPropagation();
            geocodificarDireccion();
        }
    });
}

async function cargarParadas() {
    try {
        const response = await fetch('/api/paradas');
        if (!response.ok) throw new Error('Error al cargar paradas');

        todasLasParadas = await response.json();
        refrescarVistaParadas();
        mostrarNotificacion(`${todasLasParadas.length} paradas cargadas`, 'success');
    } catch (error) {
        console.error('Error al cargar paradas:', error);
        mostrarNotificacion('Error al cargar las paradas', 'danger');
    }
}

function refrescarVistaParadas() {
    marcadores.forEach(marcador => mapa.removeLayer(marcador));
    marcadores = [];

    const paradas = obtenerParadasFiltradas();
    paradas.forEach(parada => agregarMarcador(parada));
    dibujarRutaEnMapa(paradas);
    actualizarTablaParadas(paradas);
    ajustarVistaMapa(paradas);
}

function agregarMarcador(parada) {
    const lat = Number(parada.latitud);
    const lng = Number(parada.longitud);

    if (isNaN(lat) || isNaN(lng) || lat === 0 || lng === 0) return;

    const marcador = L.marker([lat, lng], { draggable: true }).addTo(mapa);
    marcador.paradaId = parada.idParada;

    const nombre = parada.direccion || `Parada #${parada.idParada}`;
    const nombreRuta = parada.nombreRuta || 'Sin ruta';
    const nombreAlumno = parada.nombreAlumno || (parada.idAlumno ? `Alumno #${parada.idAlumno}` : 'Colegio');

    const popupContent = `
        <div>
            <b>${nombre}</b><br>
            <small><i class="bi bi-person"></i> ${nombreAlumno}</small><br>
            <small><i class="bi bi-arrow-down-up"></i> Orden: ${parada.orden || 'N/A'}</small><br>
            <small><i class="bi bi-bus-front"></i> Ruta: <span class="badge bg-info">${nombreRuta}</span></small><br>
            <small>Estado: ${parada.activo === 1 ? '✅ Activo' : '❌ Inactivo'}</small>
            <div class="popup-actions">
                <button class="btn btn-sm btn-primary" onclick="editarParada(${parada.idParada})">
                    <i class="bi bi-pencil"></i> Editar
                </button>
                <button class="btn btn-sm btn-danger" onclick="eliminarParada(${parada.idParada}, '${nombre.replace(/'/g, "\\'")}')">
                    <i class="bi bi-trash"></i> Eliminar
                </button>
            </div>
        </div>
    `;
    marcador.bindPopup(popupContent);

    marcador.on('dragend', function (e) {
        const nuevaPos = e.target.getLatLng();
        actualizarCoordenadas(parada.idParada, nuevaPos.lat, nuevaPos.lng);
    });

    marcadores.push(marcador);
}

function abrirModalParada(opciones) {
    const {
        titulo,
        idParada = '',
        idBus = '',
        idRuta = '',
        direccion = '',
        latitud = '',
        longitud = '',
        orden = null,
        activo = true,
        idAlumno = null,
        coordsDesdeMapa = false
    } = opciones;

    document.getElementById('modal-title').textContent = titulo;
    document.getElementById('parada-id').value = idParada;
    document.getElementById('parada-nombre').value = direccion;
    document.getElementById('parada-lat').value = latitud;
    document.getElementById('parada-lng').value = longitud;
    document.getElementById('parada-orden').value = orden ?? calcularSiguienteOrden();
    document.getElementById('parada-activo').checked = activo;

    geocodificacionExitosa = coordsDesdeMapa && coordenadasValidas(parseFloat(latitud), parseFloat(longitud));

    sincronizarModalBusRuta(idBus, idRuta);
    poblarAlumnosModal(idAlumno);

    document.getElementById('geocoding-info').style.display = 'none';
    document.getElementById('geocoding-info-text').textContent = '';
    actualizarRequerimientoDireccion();

    const modal = new bootstrap.Modal(document.getElementById('modal-editar-parada'));
    modal.show();
}

function agregarParadaTemporal(latlng) {
    const ruta = obtenerRutas().find(r => r.idRuta == rutaSeleccionadaId);
    if (!ruta) {
        mostrarNotificacion('Seleccione una ruta primero', 'warning');
        return;
    }

    modoAgregar = false;
    document.getElementById('btn-agregar-parada').classList.remove('active');
    mapa.getContainer().classList.remove('crosshair');

    abrirModalParada({
        titulo: 'Crear Nueva Parada',
        idBus: ruta.idBus,
        idRuta: rutaSeleccionadaId,
        latitud: latlng.lat.toFixed(6),
        longitud: latlng.lng.toFixed(6),
        coordsDesdeMapa: true
    });
}

window.editarParada = async function (idParada) {
    try {
        const response = await fetch(`/api/paradas/${idParada}`);
        if (!response.ok) throw new Error('Error al obtener parada');

        const parada = await response.json();
        const ruta = obtenerRutas().find(r => r.idRuta == parada.idRuta);

        abrirModalParada({
            titulo: 'Editar Parada',
            idParada: parada.idParada,
            idBus: ruta?.idBus || '',
            idRuta: parada.idRuta,
            direccion: parada.direccion || '',
            latitud: parada.latitud,
            longitud: parada.longitud,
            orden: parada.orden || '',
            activo: parada.activo === 1,
            idAlumno: parada.idAlumno || null,
            coordsDesdeMapa: true
        });
    } catch (error) {
        console.error('Error al editar parada:', error);
        mostrarNotificacion('Error al cargar datos de la parada', 'danger');
    }
};

function validarFormularioParada() {
    const idRuta = parseInt(document.getElementById('parada-ruta-id').value);
    const idBus = document.getElementById('parada-bus').value;
    const direccion = document.getElementById('parada-nombre').value.trim();
    const { latitud, longitud } = obtenerCoordenadasFormulario();
    const tieneCoords = coordenadasValidas(latitud, longitud);

    if (!idBus) {
        mostrarNotificacion('Seleccione un bus con cupo disponible', 'warning');
        return false;
    }

    if (!idRuta) {
        mostrarNotificacion('Seleccione una ruta activa para la parada', 'warning');
        return false;
    }

    if (!tieneCoords) {
        if (!direccion) {
            mostrarNotificacion('Ingrese la dirección de la parada', 'warning');
            return false;
        }
        if (!geocodificacionExitosa) {
            mostrarNotificacion('Debe buscar la dirección en el mapa antes de guardar', 'warning');
            return false;
        }
    }

    if (isNaN(latitud) || isNaN(longitud)) {
        mostrarNotificacion('Las coordenadas son inválidas', 'warning');
        return false;
    }

    return true;
}

async function guardarParada() {
    if (!validarFormularioParada()) return;

    const idParada = document.getElementById('parada-id').value;
    const idRuta = parseInt(document.getElementById('parada-ruta-id').value);
    const direccion = document.getElementById('parada-nombre').value.trim();
    const latitud = parseFloat(document.getElementById('parada-lat').value);
    const longitud = parseFloat(document.getElementById('parada-lng').value);
    const orden = parseInt(document.getElementById('parada-orden').value) || calcularSiguienteOrden();
    const activo = document.getElementById('parada-activo').checked ? 1 : 0;
    const alumnoVal = document.getElementById('parada-alumno').value;
    const idAlumno = alumnoVal ? parseInt(alumnoVal) : null;

    const parada = {
        idParada: idParada ? parseInt(idParada) : 0,
        idRuta: idRuta,
        idAlumno: idAlumno,
        direccion: direccion || null,
        latitud: latitud,
        longitud: longitud,
        orden: orden,
        activo: activo
    };

    try {
        const url = idParada ? `/api/paradas/${idParada}` : '/api/paradas';
        const method = idParada ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(parada)
        });

        if (response.ok) {
            const modal = bootstrap.Modal.getInstance(document.getElementById('modal-editar-parada'));
            modal.hide();

            if (window.marcadorTemporal) {
                mapa.removeLayer(window.marcadorTemporal);
                window.marcadorTemporal = null;
            }

            mostrarNotificacion(
                idParada ? 'Parada actualizada correctamente' : 'Parada creada correctamente',
                'success'
            );

            setTimeout(() => location.reload(), 600);
        } else {
            const errorText = await response.text();
            let mensaje = errorText;
            try {
                const errJson = JSON.parse(errorText);
                mensaje = errJson.message || errJson.title || errorText;
            } catch (_) { /* usar texto plano */ }
            throw new Error(mensaje);
        }
    } catch (error) {
        console.error('Error al guardar parada:', error);
        mostrarNotificacion('Error al guardar la parada: ' + error.message, 'danger');
    }
}

window.eliminarParada = async function (idParada, nombre) {
    if (!confirm(`¿Estás seguro de eliminar la parada "${nombre}"?`)) return;

    try {
        const response = await fetch(`/api/paradas/${idParada}`, { method: 'DELETE' });

        if (response.ok) {
            mostrarNotificacion('Parada eliminada correctamente', 'success');
            setTimeout(() => cargarParadas(), 500);
        } else {
            const error = await response.text();
            throw new Error(error);
        }
    } catch (error) {
        console.error('Error al eliminar parada:', error);
        mostrarNotificacion('Error al eliminar la parada: ' + error.message, 'danger');
    }
};

async function actualizarCoordenadas(idParada, lat, lng) {
    try {
        const response = await fetch(`/api/paradas/${idParada}`);
        if (!response.ok) throw new Error('Error al obtener parada');

        const parada = await response.json();
        parada.latitud = lat;
        parada.longitud = lng;

        const updateResponse = await fetch(`/api/paradas/${idParada}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(parada)
        });

        if (updateResponse.ok) {
            mostrarNotificacion('Coordenadas actualizadas correctamente', 'success');
        } else {
            throw new Error('Error al actualizar coordenadas');
        }
    } catch (error) {
        console.error('Error al actualizar coordenadas:', error);
        mostrarNotificacion('Error al actualizar coordenadas', 'danger');
    }
}

function actualizarTablaParadas(paradas) {
    const tbody = document.getElementById('paradas-table-body');
    tbody.innerHTML = '';

    if (paradas.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="9" class="text-center text-muted">
                    ${rutaSeleccionadaId ? 'No hay paradas para la ruta seleccionada' : 'Seleccione una ruta o no hay paradas registradas'}
                </td>
            </tr>
        `;
        return;
    }

    paradas.forEach(parada => {
        const nombre = parada.direccion || `Parada #${parada.idParada}`;
        const nombreRuta = parada.nombreRuta || 'Sin ruta';
        const nombreAlumno = parada.nombreAlumno || (parada.idAlumno ? `Alumno #${parada.idAlumno}` : 'Colegio');
        const row = `
            <tr>
                <td>${parada.idParada}</td>
                <td>${nombre}</td>
                <td>${parseFloat(parada.latitud).toFixed(6)}</td>
                <td>${parseFloat(parada.longitud).toFixed(6)}</td>
                <td>${parada.orden || 'N/A'}</td>
                <td><span class="badge bg-info">${nombreRuta}</span></td>
                <td>${nombreAlumno}</td>
                <td>${parada.activo === 1 ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-secondary">Inactivo</span>'}</td>
                <td>
                    <button class="btn btn-sm btn-primary" onclick="editarParada(${parada.idParada})">
                        <i class="bi bi-pencil"></i>
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="eliminarParada(${parada.idParada}, '${nombre.replace(/'/g, "\\'")}')">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            </tr>
        `;
        tbody.innerHTML += row;
    });
}

function mostrarNotificacion(mensaje, tipo) {
    console.log(`[${tipo.toUpperCase()}] ${mensaje}`);
    if (typeof showToast === 'function') {
        showToast(mensaje, tipo);
    } else {
        alert(mensaje);
    }
}

window.geocodificarDireccion = async function () {
    const inputDireccion = document.getElementById('parada-nombre');
    if (!inputDireccion) return;

    const direccion = inputDireccion.value.trim();
    if (!direccion) {
        alert('Por favor ingresa una dirección');
        return;
    }

    geocodificacionExitosa = false;

    const btn = document.getElementById('btn-geocodificar');
    const btnOriginalText = btn.innerHTML;
    btn.disabled = true;
    btn.innerHTML = '<i class="bi bi-hourglass-split"></i> Buscando...';

    try {
        const queryDireccion = direccion.toLowerCase().includes('guatemala')
            ? direccion
            : `${direccion}, Guatemala`;

        const url = `https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(queryDireccion)}&limit=3`;
        const response = await fetch(url, {
            headers: { 'User-Agent': 'TransportesGenesis/1.0' }
        });

        if (!response.ok) throw new Error(`Error HTTP: ${response.status}`);

        const resultados = await response.json();
        if (resultados.length === 0) {
            document.getElementById('geocoding-info').style.display = 'block';
            document.getElementById('geocoding-info-text').textContent =
                '❌ Dirección no encontrada. Intente agregar Zona o Ciudad.';
            return;
        }

        const ubicacion = resultados[0];
        const lat = parseFloat(ubicacion.lat);
        const lng = parseFloat(ubicacion.lon);

        document.getElementById('parada-lat').value = lat.toFixed(6);
        document.getElementById('parada-lng').value = lng.toFixed(6);
        geocodificacionExitosa = true;
        actualizarRequerimientoDireccion();

        document.getElementById('geocoding-info').style.display = 'block';
        document.getElementById('geocoding-info-text').textContent =
            `✅ Ubicación encontrada: ${ubicacion.display_name}`;

        if (window.marcadorTemporal) mapa.removeLayer(window.marcadorTemporal);

        window.marcadorTemporal = L.marker([lat, lng], {
            icon: L.icon({
                iconUrl: 'https://raw.githubusercontent.com/pointhi/leaflet-color-markers/master/img/marker-icon-2x-red.png',
                shadowUrl: 'https://cdnjs.cloudflare.com/ajax/libs/leaflet/1.9.4/images/marker-shadow.png',
                iconSize: [25, 41],
                iconAnchor: [12, 41],
                popupAnchor: [1, -34],
                shadowSize: [41, 41]
            })
        }).addTo(mapa);

        window.marcadorTemporal.bindPopup(`<b>Nueva Ubicación</b><br>${direccion}`).openPopup();
        mapa.setView([lat, lng], 15);
    } catch (error) {
        document.getElementById('geocoding-info').style.display = 'block';
        document.getElementById('geocoding-info-text').textContent = `❌ Error: ${error.message}`;
    } finally {
        btn.disabled = false;
        btn.innerHTML = btnOriginalText;
    }
};
