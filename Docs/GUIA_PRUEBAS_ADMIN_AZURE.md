# Guía de pruebas — Rol Administrador (Azure)

Manual paso a paso para probar **todas las funcionalidades del administrador** en TransportesGenesis desplegado en Azure.

**Versión:** 1.0  
**Uso:** tesis, demo con cliente, piloto de 2 meses  
**Orden recomendado:** seguir las secciones en secuencia la primera vez; después puede usarse como checklist.

---

## 1. Antes de empezar

### 1.1 Datos del entorno Azure

| Campo | Valor |
|-------|--------|
| **URL de la aplicación** | `https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net` |
| **Login** | [https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net/Auth/Login](https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net/Auth/Login) |
| **Panel admin** | [https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net/Admin/Index](https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net/Admin/Index) |
| **Base de datos** | `TransportesGenesis_db` @ `transportesgenesisdbserver` (Azure SQL) |
| **Región** | Central US (`centralus-01`) |
| **Estado** | Publicada en línea — acceso con usuario admin confirmado |

### 1.2 Credenciales de administrador

Probar login con lo que esté configurado en Azure. En desarrollo local el seed usa:

| Campo | Valor típico (confirmar en Azure) |
|-------|-------------------------------------|
| Usuario o correo | `admin` o `admin@transportesgenesis.com` |
| Contraseña | La definida al desplegar (en local era `Admin123!`) |

> Si no entran, pedir al compañero la contraseña del admin en Azure o resetearla desde el portal / Identity.

### 1.3 Usuarios de apoyo para pruebas cruzadas

Para probar traslados, asignaciones y pagos necesitarás también:

| Rol | Usuario ejemplo | Contraseña (seed local) |
|-----|-----------------|-------------------------|
| Padre | `padre1@gmail.com` | `Admin123!` |
| Piloto | `piloto2@transportesgenesis.com` | `Admin123!` |
| Monitor | `monitor2@transportesgenesis.com` | `Admin123!` |

### 1.4 Navegador y herramientas

- Chrome o Edge actualizado
- Ventana normal + ventana incógnito (para probar varios roles a la vez)
- Opcional: Excel/LibreOffice para validar exportaciones
- Anotar resultados en la tabla de la sección 10

### 1.5 Orden lógico de datos

Muchas funciones del admin **dependen de datos previos**:

```
Usuarios → Buses → Alumnos (bus + coordenadas) → Asignaciones (piloto/monitor → bus)
    → Asistencia confirmada (padre) → Calcular rutas → Paradas → Mapa en vivo / Alertas
    → Montos → Pago padre → Validar pago admin
```

Si la BD en Azure está vacía o recién reseteada, ejecutar primero los scripts SQL del repo (ver sección 11) o crear datos manualmente siguiendo las secciones 3–6.

---

## 2. Acceso y panel principal

### Prueba 2.1 — Login administrador

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir [Login](https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net/Auth/Login) | Formulario de login |
| 2 | Ingresar credenciales admin | Redirección a [Panel Admin](https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net/Admin/Index) |
| 3 | Verificar nombre en pantalla | Muestra "Bienvenido, {usuario}" |
| 4 | Cerrar sesión (`/Auth/Logout`) | Vuelve al login |
| 5 | Intentar `/Admin/Index` sin login | Redirección a login o acceso denegado |

**URL:** `/Admin/Index`  
**Archivo:** `Views/Admin/Index.cshtml`

---

### Prueba 2.2 — Control de acceso (otros roles)

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Login como `padre1@gmail.com` | Redirección a panel de padre |
| 2 | Abrir manualmente [Panel Admin](https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net/Admin/Index) sin login | Acceso denegado o redirect |
| 3 | Repetir con piloto/monitor | No debe ver panel admin |

---

## 3. Usuarios del sistema

Hay **dos pantallas** de usuarios; probar ambas si están en el menú.

### Prueba 3.1 — Usuarios (Razor) — recomendada

**URL:** `/Admin/Usuarios`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir la página | Lista de usuarios y formulario de creación |
| 2 | Crear usuario: username único, email, contraseña, rol **Piloto** | Mensaje de éxito; usuario en la lista |
| 3 | Crear otro con rol **PadreDeFamilia** | Usuario creado |
| 4 | Intentar username duplicado | Error claro |
| 5 | Eliminar un usuario de prueba (no admin) | Usuario eliminado |
| 6 | Intentar eliminar `admin` | Debe bloquearse o no permitirse |

---

### Prueba 3.2 — Gestión de usuarios (MVC)

**URL:** `/Admin/GestionUsuarios`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir la página | Tabla de usuarios |
| 2 | Crear usuario desde modal | Usuario creado (contraseña por defecto suele ser `Temp123!`) |
| 3 | Verificar que puede iniciar sesión con el nuevo usuario | Login OK según rol |

---

## 4. Flota de buses

**URL base:** `/Geolocalizacion/BusesIndex`

### Prueba 4.1 — Listar buses

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir BusesIndex | Lista de buses con placa, modelo, capacidad |
| 2 | Anotar **IdBus** de un bus activo (ej. BUS-001) | Para usar en asignaciones y rutas |

---

### Prueba 4.2 — Crear bus

**URL:** `/Geolocalizacion/BusCreate`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Completar placa, modelo, capacidad | Formulario válido |
| 2 | Guardar | Redirección a listado; bus visible |
| 3 | Abrir detalle `/Geolocalizacion/BusDetails?id={id}` | Muestra datos del bus |

---

### Prueba 4.3 — Editar bus

**URL:** `/Geolocalizacion/BusEdit?id={id}`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Cambiar modelo o capacidad | Guardado correcto |
| 2 | Verificar en listado | Cambio reflejado |

---

## 5. Alumnos

**URL:** `/Admin/GestionarAlumnos`

### Prueba 5.1 — Asignar bus a alumno

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir gestión de alumnos | Tabla con alumnos, padres, bus |
| 2 | Filtrar por "sin bus" o similar | Lista filtrada |
| 3 | Asignar un bus activo a un alumno | `IdBusAsignado` actualizado |
| 4 | Si el alumno tiene coordenadas | Puede crearse parada automática en ruta existente |

---

### Prueba 5.2 — Dirección y coordenadas

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Editar dirección de un alumno | Campo de dirección |
| 2 | Usar geocodificación / mapa (si aplica) | Latitud y longitud guardadas |
| 3 | Verificar en mapa o en paradas | Coordenadas coherentes (Guatemala) |

---

### Prueba 5.3 — Quitar bus

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Dejar alumno sin bus asignado | Bus removido |
| 2 | Verificar paradas relacionadas | Paradas asociadas actualizadas o eliminadas según reglas del sistema |

---

## 6. Asignaciones de personal (piloto / monitor → bus)

**URL:** `/Admin/GestionarAsignaciones`

### Prueba 6.1 — Crear asignación

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir página | Listas de pilotos, monitores y buses disponibles |
| 2 | Asignar **piloto2** (o piloto de prueba) a un bus **sin** asignación activa | Mensaje de éxito |
| 3 | Ver fila en asignaciones activas | `EsActual = true`, fecha inicio visible |

**Importante para mapa en vivo:** el piloto/monitor debe estar asignado al **mismo IdBus** que los alumnos del padre que observará el mapa.

---

### Prueba 6.2 — Finalizar asignación

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Clic en **Finalizar** en una asignación activa | Asignación cerrada con fecha fin |
| 2 | Intentar reasignar mismo piloto a otro bus | Debe permitirse tras finalizar |

---

### Prueba 6.3 — Reporte de asignaciones

**URL:** `/Admin/ReporteAsignaciones`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir reporte | Historial de asignaciones |
| 2 | Verificar activas e históricas | Datos coinciden con prueba 6.1–6.2 |

---

## 7. Rutas escolares

### Prueba 7.1 — Confirmar asistencia (como padre, prerequisito)

**URL (padre):** `/Padres/ConfirmarAsistencia` o ruta equivalente en el menú padre

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Login como padre vinculado a un alumno con bus | Panel de asistencia |
| 2 | Confirmar asistencia mañana y/o tarde para **hoy** o fecha de prueba | Guardado exitoso |

> Sin asistencia confirmada, **Calcular Rutas** puede no generar paradas para ese día.

---

### Prueba 7.2 — Calcular rutas masivas

**URL:** `/Admin/CalcularRutas`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Elegir **fecha en día hábil** (lunes–viernes) | Formulario acepta fecha |
| 2 | Elegir turno **Mañana** | — |
| 3 | Clic en calcular | Resultados por bus: IdRuta, cantidad de paradas |
| 4 | Repetir turno **Tarde** | Segunda ruta si aplica |
| 5 | Probar con **sábado/domingo** | Mensaje de error o sin rutas (comportamiento esperado) |
| 6 | Anotar **IdRuta** generado | Para paradas y ver ruta |

---

### Prueba 7.3 — Ver ruta en mapa

**URL:** `/Admin/VerRuta?idRuta={id}`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir con IdRuta de prueba 7.2 | Mapa Leaflet con paradas ordenadas |
| 2 | Ver listado de alumnos / paradas | Coincide con alumnos del bus |
| 3 | Ver info de piloto asignado | Nombre coherente con asignación |

---

### Prueba 7.4 — Editor de paradas

**URL:** `/Admin/GestionarParadas?idRuta={id}`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir editor con idRuta | Mapa + selector de bus/ruta |
| 2 | **Crear** parada (clic en mapa o formulario) | Nueva parada al final del orden |
| 3 | **Mover** parada (drag) | Coordenadas actualizadas |
| 4 | **Editar** datos de parada | Cambios guardados |
| 5 | **Eliminar** parada de prueba | Parada inactiva o eliminada (soft delete) |
| 6 | Asignar alumno a parada (si la UI lo permite) | Alumno vinculado |

**API usada:** `/api/paradas` (requiere sesión admin)

---

### Prueba 7.5 — Reporte de rutas

**URL:** `/Admin/ReporteRutas`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Ver listado | Rutas con bus, tipo, paradas, estado |
| 2 | Exportar Excel (`?handler=ExportarExcel` o botón en UI) | Descarga `.xlsx` válido |
| 3 | (Opcional) Generar datos de ejemplo si BD vacía | Rutas/buses de demo creados |

---

## 8. Asistencias (reporte admin)

**URL:** `/Admin/ReporteAsistencias`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir reporte (mes actual) | Estadísticas mañana/tarde |
| 2 | Filtrar por alumno | Subconjunto correcto |
| 3 | Filtrar por rango de fechas | Datos acotados |
| 4 | Exportar Excel | Archivo descargado con totales |

---

## 9. Traslados

### Prueba 9.1 — Crear solicitud (como padre)

**URL padre:** `/Padres/Traslados` (o equivalente)

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Login como padre | — |
| 2 | Crear solicitud de traslado (fecha futura, turno, motivo) | Estado **Pendiente** |

---

### Prueba 9.2 — Aprobar / rechazar (admin)

**URL:** `/Admin/GestionarTraslados`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir página admin | Lista de pendientes |
| 2 | **Aprobar** solicitud; elegir bus destino si pide | Estado **Aprobado** |
| 3 | Crear otra solicitud y **Rechazar** con comentario | Estado **Rechazado** + comentario admin |
| 4 | Filtrar por estado | Listas coherentes |

**API:** `GET /api/traslados/pendientes`, `POST /api/traslados/{id}/responder`

---

## 10. Alertas

**URL:** `/Admin/AlertasHistorial`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir historial | Tabla de alertas (proximidad / retraso) |
| 2 | Filtrar por bus, tipo, fecha | Resultados filtrados |
| 3 | **Marcar como resuelta** una alerta activa | Estado resuelto |
| 4 | **Generar datos de ejemplo** (si hay botón) | Alertas de prueba en BD |
| 5 | **Exportar Excel** | Descarga correcta |
| 6 | (Opcional) Limpiar alertas antiguas | Registros >30 días eliminados |

**Generación en vivo:** cuando un piloto transmite ubicación cerca de una parada, pueden generarse alertas SignalR (probar con piloto en otra pestaña).

---

## 11. Operaciones en vivo (mapa y GPS)

### Prueba 11.1 — Mapa en tiempo real

**URL:** `/Geolocalizacion/MapaEnTiempoReal`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir mapa como admin | Selector de buses elegibles |
| 2 | Elegir bus con ruta activa | Paradas dibujadas |
| 3 | En otra pestaña: login **piloto/monitor** asignado al mismo bus | — |
| 4 | Iniciar simulación / reporte de ubicación en Mi Ruta | — |
| 5 | Volver al mapa admin | Marcador del bus se mueve o actualiza posición |
| 6 | Esperar >10 min sin GPS | Estado "Sin señal" o última posición conocida |

**APIs:** `/api/mapa-tiempo-real/buses-eligibles`, `/api/mapa-tiempo-real/vista?idBus=`

---

### Prueba 11.2 — Simulación de mapa (demo)

**URL:** `/Mapa/Simulacion`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir página | Buses simulados en movimiento |
| 2 | Verificar animación | Marcadores se mueven (demo visual) |

---

## 12. Pagos y mensualidades

### Prueba 12.1 — Asignar montos a padres

**URL:** `/MontosPadres/Index`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir listado | Montos por usuario padre |
| 2 | **Create:** asignar monto mensual a `padre1` (usuario del sistema) | Registro activo |
| 3 | **Edit** monto | Valor actualizado |
| 4 | (Opcional) **Delete** monto de prueba | Soft delete o inactivo |

---

### Prueba 12.2 — Padre registra pago

**URL (padre):** `/PagosPadresFamilia/Pagos`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Login padre con monto asignado | Pantalla de pagos |
| 2 | **Subir boleta** (imagen JPG/PNG) o **pago en línea Stripe** | Pago en estado **Pendiente** |
| 3 | Ver historial padre | Comprobante / referencia visible |

> Stripe en Azure requiere HTTPS y claves configuradas en variables de entorno.

---

### Prueba 12.3 — Validar pagos (admin)

**URL:** `/Admin/DashboardPagos`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir dashboard | Pagos pendientes listados |
| 2 | Filtrar por usuario / mes | Filtro funciona |
| 3 | **Aprobar** pago pendiente | `EstadoAdmin = Validado` |
| 4 | **Rechazar** otro pago | `EstadoAdmin = Rechazado` |
| 5 | **Deshacer** validación | Vuelve a Pendiente |

---

### Prueba 12.4 — Calendario de pagos

**URL:** `/Admin/Calendario`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir calendario | Eventos de pagos por fecha |
| 2 | Clic en evento | Detalle (usuario, monto, tipo) |
| 3 | Validar desde calendario (si hay enlace) | Pago marcado validado |
| 4 | Exportar Excel/PDF desde admin | `/Admin/ExportarPagosExcel`, `/Admin/ExportarPagosPdf` |

**Nota conocida:** rechazar desde calendario puede fallar si solo existe acción POST; usar DashboardPagos para rechazar.

---

## 13. Dashboard analítico

**URL:** `/Admin/Dashboard`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir dashboard | Tarjetas KPI: buses, rutas, alumnos, alertas |
| 2 | Revisar gráficos Chart.js | Se renderizan sin error en consola |
| 3 | Sección actividad reciente | Última ruta / asistencias del día |

---

## 14. Planner del proyecto (interno)

**URL:** `/Planner`

| Paso | Acción | Resultado esperado |
|------|--------|-------------------|
| 1 | Abrir planner | Tablero de sprints/tareas |
| 2 | Exportar Excel (si hay botón) | Descarga planificación |

> Herramienta interna de documentación del proyecto; no es operación de transporte.

---

## 15. Checklist resumido (todas las URLs admin)

Marca ✅ / ❌ / ⚠️ al probar:

| # | Módulo | URL | ✅ |
|---|--------|-----|----|
| 1 | Login | `/Auth/Login` | |
| 2 | Panel principal | `/Admin/Index` | |
| 3 | Usuarios Razor | `/Admin/Usuarios` | |
| 4 | Usuarios MVC | `/Admin/GestionUsuarios` | |
| 5 | Buses | `/Geolocalizacion/BusesIndex` | |
| 6 | Crear bus | `/Geolocalizacion/BusCreate` | |
| 7 | Alumnos | `/Admin/GestionarAlumnos` | |
| 8 | Asignaciones | `/Admin/GestionarAsignaciones` | |
| 9 | Reporte asignaciones | `/Admin/ReporteAsignaciones` | |
| 10 | Calcular rutas | `/Admin/CalcularRutas` | |
| 11 | Ver ruta | `/Admin/VerRuta?idRuta=` | |
| 12 | Paradas | `/Admin/GestionarParadas?idRuta=` | |
| 13 | Reporte rutas | `/Admin/ReporteRutas` | |
| 14 | Reporte asistencias | `/Admin/ReporteAsistencias` | |
| 15 | Traslados | `/Admin/GestionarTraslados` | |
| 16 | Alertas | `/Admin/AlertasHistorial` | |
| 17 | Mapa vivo | `/Geolocalizacion/MapaEnTiempoReal` | |
| 18 | Simulación mapa | `/Mapa/Simulacion` | |
| 19 | Montos padres | `/MontosPadres/Index` | |
| 20 | Validar pagos | `/Admin/DashboardPagos` | |
| 21 | Calendario pagos | `/Admin/Calendario` | |
| 22 | Export Excel pagos | `/Admin/ExportarPagosExcel` | |
| 23 | Export PDF pagos | `/Admin/ExportarPagosPdf` | |
| 24 | Dashboard KPIs | `/Admin/Dashboard` | |
| 25 | Planner | `/Planner` | |

---

## 16. Registro de pruebas (plantilla)

Copia esta tabla por sesión de prueba:

| ID | Fecha | Probador | Módulo | Resultado | Observaciones |
|----|-------|----------|--------|-----------|---------------|
| ADM-01 | | | Login admin | ✅ / ❌ | |
| ADM-02 | | | Usuarios | | |
| ADM-03 | | | Buses | | |
| … | | | | | |

**Criterio de aprobación sugerido para piloto:**
- Críticos (login, asignaciones, calcular rutas, mapa vivo, validar pagos): **100% OK**
- Reportes y exportaciones: **≥ 90% OK**
- Planner / datos de ejemplo: **informativo**

---

## 17. Scripts SQL útiles (Azure Query editor)

Si faltan datos en `TransportesGenesis_db`, ejecutar en el **Query editor** del portal Azure (como en tu captura):

| Script (en el repo) | Para qué |
|---------------------|----------|
| `Scripts/Seed_Escenarios_Completos.sql` | Asistencias, colegio, escenario completo |
| `Scripts/EJECUTAR_SIMPLE_DashboardMonitor.sql` | Bus BUS-001, padres, alumnos |
| `Scripts/InsertarDatosPrueba_Asistencia.sql` | Reporte de asistencias |
| `Scripts/SeedData_GestionarParadas.sql` | Editor de paradas |
| `Scripts/CorregirSolicitudes_Traslado.sql` | Traslados pendientes |

> **Cuidado:** no ejecutar `DROP TABLE` en producción sin respaldo. Si resetean migraciones, coordinar con quien desplegó la app.

---

## 18. Problemas frecuentes y qué revisar

| Síntoma | Causa probable | Qué hacer |
|---------|----------------|-----------|
| Calcular rutas sin paradas | Sin asistencia confirmada o alumnos sin coords | Padre confirma asistencia; admin geocodifica alumnos |
| Mapa sin movimiento | Piloto en **otro IdBus** que el padre | Alinear asignación piloto ↔ bus de alumnos |
| No hay buses elegibles en mapa | Sin ruta activa o sin alumnos en bus | Calcular rutas en día hábil |
| Pago Stripe falla | Claves Stripe o HTTPS en Azure | Revisar configuración de app en Azure |
| 401/403 en APIs | Sesión expirada | Volver a login admin |
| Paradas no guardan | Error JS o API | F12 → Consola; verificar `/api/paradas` |

---

## 19. Secuencia recomendada para demo completa (1–2 horas)

1. Login admin → `/Admin/Index`  
2. Verificar buses → `/Geolocalizacion/BusesIndex`  
3. Gestionar alumnos (bus + dirección) → `/Admin/GestionarAlumnos`  
4. Asignar piloto al bus → `/Admin/GestionarAsignaciones`  
5. Padre confirma asistencia (otra pestaña)  
6. Calcular rutas → `/Admin/CalcularRutas`  
7. Ajustar paradas → `/Admin/GestionarParadas?idRuta=X`  
8. Piloto inicia ruta/simulación → `/Piloto/MiRuta`  
9. Admin mapa en vivo → `/Geolocalizacion/MapaEnTiempoReal`  
10. Montos + pago padre + validar admin  
11. Traslado padre → aprobar admin  
12. Reportes (rutas, asistencias, alertas) + export Excel  
13. Dashboard analítico → `/Admin/Dashboard`  

---

## 20. Referencias en el repositorio

| Documento | Contenido |
|-----------|-----------|
| `Docs/CONFIGURACION_PRUEBAS_PADRE_PILOTO_MONITOR.md` | Pruebas cruzadas padre/piloto/monitor |
| `GUIA_DEMO_Padre_Piloto_Tiempo_Real.md` | Demo mapa en vivo |
| `CREDENCIALES_DEMO.md` | Listado de usuarios demo |
| `Views/Admin/Index.cshtml` | Panel con enlaces oficiales |

---

**Última actualización:** despliegue Azure Central US — app en línea.  
URL: https://transportes-genesis-web-htf0ajebcufbgzfr.centralus-01.azurewebsites.net
