// Gestión de Paradas con Mapa Interactivo
let mapa;
let marcadores = [];
let paradaEnEdicion = null;
let modoAgregar = false;

// Inicializar cuando el DOM esté listo
document.addEventListener('DOMContentLoaded', function() {
    inicializarMapa();
    configurarEventos();
    cargarParadas();
});

// Inicializar mapa con Leaflet
function inicializarMapa() {
    // Coordenadas de Guatemala (centro)
    mapa = L.map('mapa-paradas').setView([14.6349, -90.5069], 13);

    // Capa de OpenStreetMap
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        attribution: '© OpenStreetMap contributors'
    }).addTo(mapa);

    // Evento click en el mapa (cuando está en modo agregar)
    mapa.on('click', function(e) {
        if (modoAgregar) {
            agregarParadaTemporal(e.latlng);
        }
    });
}

// Configurar eventos de botones
function configurarEventos() {
    // Botón Agregar Parada
    document.getElementById('btn-agregar-parada').addEventListener('click', function() {
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

    // Botón Refrescar
    document.getElementById('btn-refrescar').addEventListener('click', function() {
        cargarParadas();
    });

    // Botón Guardar en el modal
    document.getElementById('btn-guardar-parada').addEventListener('click', function() {
        guardarParada();
    });
}

// Cargar paradas desde la API
async function cargarParadas() {
    try {
        const response = await fetch('/api/paradas');
        if (!response.ok) {
            throw new Error('Error al cargar paradas');
        }

        const paradas = await response.json();

        console.log('Paradas recibidas:', paradas);

        // Limpiar marcadores existentes
        marcadores.forEach(marcador => mapa.removeLayer(marcador));
        marcadores = [];

        // Agregar marcadores al mapa
        paradas.forEach(parada => {
            // Validar que las coordenadas sean válidas
            const lat = parseFloat(parada.latitud);
            const lng = parseFloat(parada.longitud);

            if (isNaN(lat) || isNaN(lng)) {
                console.error(`Parada ${parada.idParada} tiene coordenadas inválidas:`, parada);
                return; // Saltar esta parada
            }

            agregarMarcador(parada);
        });

        // Actualizar tabla
        actualizarTablaParadas(paradas);

        mostrarNotificacion(`${paradas.length} paradas cargadas correctamente`, 'success');
    } catch (error) {
        console.error('Error al cargar paradas:', error);
        mostrarNotificacion('Error al cargar las paradas', 'danger');
    }
}

// Agregar marcador al mapa
function agregarMarcador(parada) {
    // Validar y convertir coordenadas
    const lat = Number(parada.latitud);
    const lng = Number(parada.longitud);

    console.log(`Agregando parada ${parada.idParada}:`, { lat, lng, original: { latitud: parada.latitud, longitud: parada.longitud } });

    // Verificar que las coordenadas sean válidas
    if (isNaN(lat) || isNaN(lng) || lat === 0 || lng === 0) {
        console.error(`Coordenadas inválidas para parada ${parada.idParada}:`, parada);
        return;
    }

    const marcador = L.marker([lat, lng], {
        draggable: true
    }).addTo(mapa);

    marcador.paradaId = parada.idParada;

    // Determinar el nombre a mostrar
    const nombre = parada.direccion || `Parada #${parada.idParada}`;
    const nombreRuta = parada.nombreRuta || 'Sin ruta';

    // Popup con información y acciones
    const popupContent = `
        <div>
            <b>${nombre}</b><br>
            <small><i class="bi bi-geo-alt"></i> Lat: ${lat.toFixed(6)}, Lng: ${lng.toFixed(6)}</small><br>
            <small><i class="bi bi-arrow-down-up"></i> Orden: ${parada.orden || 'N/A'}</small><br>
            <small><i class="bi bi-bus-front"></i> Ruta: <span class="badge bg-info">${nombreRuta}</span></small><br>
            <small>Estado: ${parada.activo === 1 ? '✅ Activo' : '❌ Inactivo'}</small>
            <div class="popup-actions">
                <button class="btn btn-sm btn-primary" onclick="editarParada(${parada.idParada})">
                    <i class="bi bi-pencil"></i> Editar
                </button>
                <button class="btn btn-sm btn-danger" onclick="eliminarParada(${parada.idParada}, '${nombre}')">
                    <i class="bi bi-trash"></i> Eliminar
                </button>
            </div>
        </div>
    `;
    marcador.bindPopup(popupContent);

    // Evento drag para actualizar coordenadas
    marcador.on('dragend', function(e) {
        const nuevaPos = e.target.getLatLng();
        actualizarCoordenadas(parada.idParada, nuevaPos.lat, nuevaPos.lng);
    });

    marcadores.push(marcador);
}

// Agregar parada temporal (nueva)
function agregarParadaTemporal(latlng) {
    document.getElementById('modal-title').textContent = 'Crear Nueva Parada';
    document.getElementById('parada-id').value = '';
    document.getElementById('parada-nombre').value = '';
    document.getElementById('parada-lat').value = latlng.lat.toFixed(6);
    document.getElementById('parada-lng').value = latlng.lng.toFixed(6);
    document.getElementById('parada-orden').value = marcadores.length + 1;
    document.getElementById('parada-activo').checked = true;

    // Cerrar modo agregar
    modoAgregar = false;
    document.getElementById('btn-agregar-parada').classList.remove('active');
    mapa.getContainer().classList.remove('crosshair');

    // Mostrar modal
    const modal = new bootstrap.Modal(document.getElementById('modal-editar-parada'));
    modal.show();
}

// Editar parada existente
window.editarParada = async function(idParada) {
    try {
        const response = await fetch(`/api/paradas/${idParada}`);
        if (!response.ok) {
            throw new Error('Error al obtener parada');
        }

        const parada = await response.json();

        document.getElementById('modal-title').textContent = 'Editar Parada';
        document.getElementById('parada-id').value = parada.idParada;
        document.getElementById('parada-nombre').value = parada.direccion || '';
        document.getElementById('parada-lat').value = parada.latitud;
        document.getElementById('parada-lng').value = parada.longitud;
        document.getElementById('parada-orden').value = parada.orden || '';
        document.getElementById('parada-activo').checked = parada.activo === 1;

        const modal = new bootstrap.Modal(document.getElementById('modal-editar-parada'));
        modal.show();
    } catch (error) {
        console.error('Error al editar parada:', error);
        mostrarNotificacion('Error al cargar datos de la parada', 'danger');
    }
}

// Guardar parada (crear o actualizar)
async function guardarParada() {
    const idParada = document.getElementById('parada-id').value;
    const direccion = document.getElementById('parada-nombre').value.trim();
    const latitud = parseFloat(document.getElementById('parada-lat').value);
    const longitud = parseFloat(document.getElementById('parada-lng').value);
    const orden = parseInt(document.getElementById('parada-orden').value) || 1;
    const activo = document.getElementById('parada-activo').checked ? 1 : 0;

    if (!direccion) {
        mostrarNotificacion('La dirección es obligatoria', 'warning');
        return;
    }

    const parada = {
        idParada: idParada ? parseInt(idParada) : 0,
        idRuta: 1, // Valor temporal - ajustar según lógica de negocio
        direccion: direccion,
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
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(parada)
        });

        if (response.ok) {
            const modal = bootstrap.Modal.getInstance(document.getElementById('modal-editar-parada'));
            modal.hide();

            mostrarNotificacion(
                idParada ? 'Parada actualizada correctamente' : 'Parada creada correctamente',
                'success'
            );

            // Recargar paradas
            setTimeout(() => cargarParadas(), 500);
        } else {
            const error = await response.text();
            throw new Error(error);
        }
    } catch (error) {
        console.error('Error al guardar parada:', error);
        mostrarNotificacion('Error al guardar la parada: ' + error.message, 'danger');
    }
}

// Eliminar parada
window.eliminarParada = async function(idParada, nombre) {
    if (!confirm(`¿Estás seguro de eliminar la parada "${nombre}"?`)) {
        return;
    }

    try {
        const response = await fetch(`/api/paradas/${idParada}`, {
            method: 'DELETE'
        });

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
}

// Actualizar coordenadas después de arrastrar
async function actualizarCoordenadas(idParada, lat, lng) {
    try {
        const response = await fetch(`/api/paradas/${idParada}`, {
            method: 'GET'
        });

        if (!response.ok) {
            throw new Error('Error al obtener parada');
        }

        const parada = await response.json();
        parada.latitud = lat;
        parada.longitud = lng;

        const updateResponse = await fetch(`/api/paradas/${idParada}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
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

// Actualizar tabla de paradas
function actualizarTablaParadas(paradas) {
    const tbody = document.getElementById('paradas-table-body');
    tbody.innerHTML = '';

    if (paradas.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center text-muted">No hay paradas registradas</td>
            </tr>
        `;
        return;
    }

    paradas.forEach(parada => {
        const nombre = parada.direccion || `Parada #${parada.idParada}`;
        const nombreRuta = parada.nombreRuta || 'Sin ruta';
        const row = `
            <tr>
                <td>${parada.idParada}</td>
                <td>${nombre}</td>
                <td>${parseFloat(parada.latitud).toFixed(6)}</td>
                <td>${parseFloat(parada.longitud).toFixed(6)}</td>
                <td>${parada.orden || 'N/A'}</td>
                <td><span class="badge bg-info">${nombreRuta}</span></td>
                <td>${parada.activo === 1 ? '<span class="badge bg-success">Activo</span>' : '<span class="badge bg-secondary">Inactivo</span>'}</td>
                <td>
                    <button class="btn btn-sm btn-primary" onclick="editarParada(${parada.idParada})">
                        <i class="bi bi-pencil"></i>
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="eliminarParada(${parada.idParada}, '${nombre}')">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            </tr>
        `;
        tbody.innerHTML += row;
    });
}

// Mostrar notificación temporal
function mostrarNotificacion(mensaje, tipo) {
    console.log(`[${tipo.toUpperCase()}] ${mensaje}`);
    // Aquí podrías implementar un toast de Bootstrap si lo deseas
}
