# 🔧 SOLUCIÓN JAVASCRIPT PARA FORMULARIO

Si el problema persiste, reemplaza el formulario con este código que usa JavaScript para enviar los datos:

## 📝 Código Completo

```html
<div class="card-body">
    <div class="row">
        <div class="col-md-5">
            <label for="IdUsuario" class="form-label">Seleccionar Personal:</label>
            <select class="form-select" id="IdUsuario" required>
                <option value="">-- Seleccione --</option>
                @if (Model.Pilotos.Any())
                {
                    <optgroup label="Pilotos">
                        @foreach (var piloto in Model.Pilotos)
                        {
                            <option value="@piloto.Id">
                                👨‍✈️ @piloto.UserName (@piloto.Email)
                            </option>
                        }
                    </optgroup>
                }
                @if (Model.Monitores.Any())
                {
                    <optgroup label="Monitores">
                        @foreach (var monitor in Model.Monitores)
                        {
                            <option value="@monitor.Id">
                                👨‍🏫 @monitor.UserName (@monitor.Email)
                            </option>
                        }
                    </optgroup>
                }
            </select>
        </div>

        <div class="col-md-5">
            <label for="IdBus" class="form-label">Seleccionar Bus:</label>
            <select class="form-select" id="IdBus" required>
                <option value="0">-- Seleccione --</option>
                @foreach (var bus in Model.BusesDisponibles)
                {
                    <option value="@bus.IdBus">
                        🚌 @bus.Placa - @bus.Modelo (Capacidad: @bus.Capacidad)
                    </option>
                }
            </select>
        </div>

        <div class="col-md-2 d-flex align-items-end">
            <button type="button" onclick="crearAsignacion()" class="btn btn-success w-100">
                <i class="bi bi-check-circle"></i> Asignar
            </button>
        </div>
    </div>
</div>

<script>
function crearAsignacion() {
    const idUsuario = document.getElementById('IdUsuario').value;
    const idBus = document.getElementById('IdBus').value;

    console.log('Valores a enviar:', { idUsuario, idBus });

    if (!idUsuario || idBus === '0') {
        alert('Debe seleccionar un usuario y un bus');
        return;
    }

    // Crear FormData para enviar como form post tradicional
    const formData = new FormData();
    formData.append('IdUsuario', idUsuario);
    formData.append('IdBus', idBus);

    console.log('Enviando POST a /Admin/GestionarAsignaciones');

    fetch('/Admin/GestionarAsignaciones', {
        method: 'POST',
        body: formData
    })
    .then(response => {
        console.log('Respuesta recibida:', response.status);
        if (response.ok) {
            window.location.reload();
        } else {
            return response.text().then(text => {
                console.error('Error del servidor:', text);
                alert('Error al crear asignación (Status: ' + response.status + ')');
            });
        }
    })
    .catch(error => {
        console.error('Error de red:', error);
        alert('Error de conexión: ' + error.message);
    });
}
</script>
```

## 🧪 Cómo Usarlo

1. Reemplaza el contenido del `<div class="card-body">` en `GestionarAsignaciones.cshtml`
2. Guarda el archivo
3. Recarga la página (Ctrl+F5)
4. Abre DevTools (F12) > Console
5. Intenta crear una asignación
6. Verás los logs en la consola indicando:
   - Los valores capturados
   - El momento del envío
   - La respuesta del servidor

## 🔍 Ventajas de esta Solución

- ✅ Control total sobre el envío de datos
- ✅ Logs en consola para diagnóstico
- ✅ Manejo de errores visible
- ✅ No depende del binding automático de Razor Pages

---

**Fecha:** 2025-01-XX  
**Estado:** 🔧 Solución alternativa
