# Guía de Pruebas de Sincronización de Rutas en Tiempo Real

## Descripción General
Este documento proporciona instrucciones paso a paso para probar la sincronización en tiempo real entre los tres roles del sistema: **Monitor**, **Padre** y **Administrador**.

## Cambios Implementados

### 1. NotificacionesHub (Backend)
- **ReportarUbicacionBus** ahora emite actualizaciones al grupo `Administradores` además del grupo `Bus_{idBus}`
- Todos los roles reciben la misma información en tiempo real desde la misma fuente

### 2. Dashboard del Padre
- Ya estaba correctamente conectado a SignalR
- Recibe actualizaciones mediante el evento `UbicacionBusActualizada`
- Muestra solo la ruta del hijo asignado, pero sincronizada con el Monitor

### 3. Tracking en Vivo del Administrador (NUEVO)
- **Nueva página**: `/Admin/TrackingEnVivo`
- Muestra todos los buses activos en un mapa
- Se conecta al grupo `Administradores` en SignalR
- Recibe actualizaciones en tiempo real de todos los buses

## Requisitos Previos

1. Tener al menos un bus configurado con rutas activas
2. Tener usuarios con los siguientes roles:
   - **Monitor** (asignado a un bus)
   - **Padre** (con hijo asignado a un bus)
   - **Administrador**

## Escenario de Prueba

### Paso 1: Preparar el entorno
1. Iniciar sesión con diferentes usuarios en navegadores/pestañas separadas:
   - **Navegador 1**: Usuario Monitor
   - **Navegador 2**: Usuario Padre
   - **Navegador 3**: Usuario Administrador

### Paso 2: Abrir las páginas de tracking

**Monitor:**
- Ir a `/Monitor/MiRuta`
- Verificar que se muestre el bus asignado y la ruta del día
- Confirmar que el badge de GPS muestre "Detectando GPS..."

**Padre:**
- Ir a `/Padres/DashboardRutaBusAsignado`
- Verificar que se muestre el bus asignado al hijo
- Confirmar que el badge de SignalR cambie a "En Vivo" (verde)

**Administrador:**
- Ir a `/Admin/TrackingEnVivo` (nueva página)
- Verificar que se muestren todos los buses activos
- Confirmar que el badge de SignalR cambie a "En Vivo" (verde)

### Paso 3: Iniciar simulación desde el Monitor

**Como Monitor:**
1. En la página `/Monitor/MiRuta`, ubicar el panel "Control de Simulación de Ruta"
2. Hacer clic en el botón **"Simular Ruta en Tiempo Real"**
3. La simulación comenzará y el bus se moverá automáticamente por la ruta
4. Observar que el marcador verde (bus) se mueve en el mapa del Monitor

### Paso 4: Verificar sincronización en el Padre

**Como Padre (en otro navegador):**
1. Observar el mapa en `/Padres/DashboardRutaBusAsignado`
2. **VERIFICAR**: El marcador verde del bus debe aparecer y moverse en tiempo real
3. **VERIFICAR**: El marcador debe seguir la misma ruta que ve el Monitor
4. **VERIFICAR**: Las alertas deben aparecer cuando el bus se acerque a la casa del hijo:
   - "⚠️ El bus está a menos de 300 m de la casa..."
   - "✅ El bus ha llegado al colegio. [Nombre del hijo] ya está en el colegio."
5. **VERIFICAR**: El badge "En Vivo" debe permanecer verde durante toda la simulación

### Paso 5: Verificar sincronización en el Administrador

**Como Administrador (en otro navegador):**
1. Observar el mapa en `/Admin/TrackingEnVivo`
2. **VERIFICAR**: El marcador verde del bus debe aparecer y moverse en tiempo real
3. **VERIFICAR**: El marcador debe seguir la misma ruta que ven el Monitor y el Padre
4. **VERIFICAR**: El badge del bus en la lista lateral debe cambiar de "Offline" a "Online" (verde)
5. **VERIFICAR**: Las estadísticas deben actualizarse:
   - "Buses En Línea" debe incrementar a 1
   - "Última Actualización" debe mostrar la hora actual
6. **VERIFICAR**: Al hacer clic en la tarjeta del bus en la lista lateral, el mapa debe centrarse en ese bus

### Paso 6: Verificar consistencia de datos

**IMPORTANTE**: Los tres roles deben mostrar:
- ✅ La misma posición del bus en tiempo real
- ✅ El mismo movimiento sincronizado
- ✅ Actualizaciones simultáneas (dentro de 1-2 segundos de diferencia)

## Validaciones Específicas

### Monitor (Fuente de verdad)
- [x] El Monitor puede iniciar la simulación de ruta
- [x] El marcador del bus se mueve correctamente en su mapa
- [x] Reporta la ubicación al servidor mediante SignalR

### Padre (Vista filtrada)
- [x] Ve únicamente la ruta de su hijo
- [x] Recibe actualizaciones de posición en tiempo real
- [x] El bus se mueve sincronizado con el Monitor
- [x] Recibe alertas de proximidad cuando corresponde

### Administrador (Vista global)
- [x] Ve todos los buses activos en el sistema
- [x] Recibe actualizaciones en tiempo real de todos los buses
- [x] Puede ver la posición de cada bus sincronizada con el Monitor
- [x] Las estadísticas se actualizan correctamente

## Estados del Bus

Durante la simulación, verificar que los tres roles reflejen correctamente estos estados:

1. **En Movimiento** (🚌 verde):
   - Monitor: Marcador moviéndose
   - Padre: Marcador moviéndose
   - Administrador: Badge "Online" verde

2. **Parada Completada**:
   - Monitor: Alerta de parada completada
   - Padre: Notificación de hijo recogido/dejado
   - Administrador: (sin alerta, solo tracking)

3. **Llegada al Colegio** (solo turno Mañana):
   - Monitor: Bus llega a la última parada
   - Padre: Alerta "El bus ha llegado al colegio"
   - Administrador: Bus se detiene en coordenadas del colegio

## Problemas Comunes y Soluciones

### Problema: El Padre no ve el bus moviéndose
**Solución:**
1. Verificar que el badge de SignalR esté en verde "En Vivo"
2. Abrir la consola del navegador (F12) y buscar errores
3. Verificar que el hijo esté asignado al mismo bus que el Monitor está simulando

### Problema: El Administrador no recibe actualizaciones
**Solución:**
1. Verificar que el badge de SignalR esté en verde "En Vivo"
2. Confirmar que el usuario tenga el rol "Administrador"
3. Verificar en la consola: `✅ [TrackingAdmin] Unido al grupo Administradores`

### Problema: Las actualizaciones tienen mucho retraso
**Solución:**
1. Verificar la conexión a internet
2. Revisar si hay errores en la consola del servidor
3. Confirmar que SignalR esté configurado correctamente en `Startup.cs`

## Logs de Consola Esperados

### Monitor
```
[SIGNALR] Bus 1 reporta ubicación: Lat 14.6380, Lon -90.5240
✅ [SignalR] Ubicación reportada: Bus 1
```

### Padre
```
✅ [PadreDashboard] Unido a Bus_1
✅ [PadreDashboard] Unido a Alumno_5
📍 [SignalR] Ubicación actualizada: {IdBus: 1, Latitud: 14.6380, Longitud: -90.5240}
[PadreDashboard] Bus 1 -> (14.63800, -90.52400)
```

### Administrador
```
✅ [TrackingAdmin] Unido al grupo Administradores
📍 [SignalR] Ubicación actualizada: {IdBus: 1, Latitud: 14.6380, Longitud: -90.5240}
[TrackingAdmin] Bus 1 -> (14.63800, -90.52400)
```

## Conclusión

Si todos los pasos se verifican correctamente, la sincronización entre los tres roles está funcionando como se esperaba:

- ✅ **Monitor**: Fuente principal de la ruta en tiempo real
- ✅ **Padre**: Ve la ruta de su hijo sincronizada con el Monitor
- ✅ **Administrador**: Ve todas las rutas dinámicas sincronizadas con los Monitores

La información fluye correctamente desde el Monitor a través de SignalR hacia el Padre y el Administrador, garantizando consistencia de datos en tiempo real.
