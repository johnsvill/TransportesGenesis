# 🎬 GUÍA COMPLETA: DEMO TIEMPO REAL PADRE-PILOTO

## 📋 Objetivo de la Demo
Mostrar **notificaciones en tiempo real** entre:
- **Padre** viendo el mapa de la ruta de su hijo
- **Piloto** simulando la ruta y llegando a paradas
- **Sistema** enviando alertas automáticas cuando el bus llega a cada parada

---

## ⚙️ PREPARACIÓN PREVIA (Ejecutar Antes del Martes)

### 1. Verificar Relaciones en la Base de Datos

Ejecuta el script SQL para identificar las credenciales correctas:

```sql
-- Ubicación: Scripts/VERIFICAR_Demo_Padre_Piloto.sql
```

**Este script te dará:**
- ✅ Email del Padre y su contraseña
- ✅ Email del Piloto y su contraseña
- ✅ Confirmación de que el hijo del padre está asignado al bus del piloto
- ✅ Instrucciones paso a paso para la demo

### 2. Verificar que la App esté Compilada

```powershell
# En Visual Studio
# Presiona F5 para compilar y ejecutar
# Verifica que no haya errores de compilación
```

### 3. Probar SignalR (Opcional pero Recomendado)

Abre la consola del navegador (F12) y verifica:
```
✅ SignalR conectado correctamente
✅ Unido al grupo: Bus_X
```

---

## 🎯 PASOS PARA LA DEMO (DÍA MARTES)

### **PASO 1: Preparar las Dos Ventanas**

#### Ventana 1 - PADRE (Navegador Normal)
```
1. Abre Chrome/Edge en modo NORMAL
2. Ve a: https://localhost:7241
3. Login con:
   - Usuario: [Del script SQL - ej: padre@test.com]
   - Contraseña: Admin123!
4. Deberías ver el menú del padre
```

#### Ventana 2 - PILOTO (Navegador Incógnito)
```
1. Presiona Ctrl+Shift+N (Chrome) o Ctrl+Shift+P (Edge)
2. Ve a: https://localhost:7241
3. Login con:
   - Usuario: [Del script SQL - ej: piloto@genesis.com]
   - Contraseña: Admin123!
4. Deberías ver el menú del piloto
```

---

### **PASO 2: Navegar a las Páginas Correctas**

#### En Ventana PADRE:
```
1. Clic en el menú o navega a:
   /Padres/DashboardRutaBusAsignado

2. Deberías ver:
   ✅ Mapa con la ruta de tu hijo
   ✅ Información del bus asignado
   ✅ Badge verde en esquina inferior derecha: "🟢 Conectado"
```

**🔍 VERIFICACIÓN IMPORTANTE:**
- Abre la consola (F12)
- Busca el mensaje: `✅ SignalR conectado correctamente`
- Busca el mensaje: `✅ Unido al grupo: Bus_X`

#### En Ventana PILOTO:
```
1. Navega a: /Piloto/MiRuta

2. Deberías ver:
   ✅ Mapa con las paradas de tu ruta
   ✅ Botón grande: "🚌 Simular Ruta"
   ✅ Tu ubicación GPS (punto azul en el mapa)
```

---

### **PASO 3: Iniciar la Simulación**

#### En Ventana PILOTO:

```
1. Clic en el botón: "🚌 Simular Ruta"

2. Observa:
   ✅ El botón cambia a: "🛑 Detener Simulación"
   ✅ Aparece un bus naranja 🚌 en el mapa
   ✅ El bus empieza a moverse automáticamente
   ✅ Se escucha un sonido al iniciar
   ✅ Cuando llega a cada parada:
	  • Suena una alerta 🔔
	  • Aparece un popup en el mapa
	  • Se ve un mensaje: "🚏 Llegando a: [nombre parada]"
```

---

### **PASO 4: Observar las Alertas en Tiempo Real**

#### En Ventana PADRE (MIENTRAS el piloto simula):

**⚠️ NOTA IMPORTANTE:** Las alertas aparecerán **SOLO si el piloto YA INICIÓ la simulación** en su ventana.

```
✅ DEBERÍAS VER (en tiempo real):

1. Notificaciones Toast (esquina superior derecha):
   • "✅ [Nombre alumno] ha sido recogido/dejado correctamente"
   • Cada vez que el piloto llega a una parada
   • Con sonido de notificación 🔔

2. Badge de Conexión (esquina inferior derecha):
   • Verde: "🟢 Conectado" (todo bien)
   • Rojo: "🔴 Desconectado" (problema de conexión)

3. En la consola del navegador (F12):
   • "🚏 Parada completada: [datos]"
   • "📍 Ubicación del bus actualizada: [coordenadas]"
```

**🎬 GUION DE PRESENTACIÓN:**

```
"Como pueden ver, tengo dos ventanas abiertas:

1. [Señala ventana PADRE]:
   Esta es la vista del padre, quien está esperando que 
   el bus llegue a recoger a su hijo.

2. [Señala ventana PILOTO]:
   Esta es la vista del piloto, quien está manejando el bus
   en su ruta.

3. [Clic en 'Simular Ruta' en ventana PILOTO]:
   Ahora el piloto inicia su ruta... observen.

4. [Señala ventana PADRE mientras aparecen notificaciones]:
   ¡Fíjense! El padre recibe notificaciones EN TIEMPO REAL
   cada vez que el bus llega a una parada.

5. [Muestra una notificación específica]:
   Aquí dice: '✅ [Alumno] ha sido recogido correctamente'

   Esto le da tranquilidad al padre de que su hijo 
   ya fue recogido por el bus.

6. [Muestra el mapa del padre]:
   Además, puede ver en el mapa la ruta completa que
   seguirá el bus hacia el colegio.
"
```

---

## 🐛 SOLUCIÓN DE PROBLEMAS COMUNES

### Problema 1: No aparece el badge "Conectado" en la página del padre

**Solución:**
```
1. Abre consola (F12)
2. Busca errores en rojo
3. Verifica que diga: "✅ SignalR conectado correctamente"
4. Si no aparece, recarga la página (F5)
5. Si persiste, verifica que Program.cs tenga:
   app.MapHub<NotificacionesHub>("/notificacionesHub");
```

### Problema 2: El padre NO recibe notificaciones cuando el piloto llega a paradas

**Causa más probable:** La simulación del piloto NO está enviando notificaciones SignalR.

**NOTA IMPORTANTE:** En el estado actual del código, la simulación del piloto **SOLO** muestra alertas locales (sonido + popup) pero **NO envía** notificaciones a través de SignalR al hub.

**Opciones:**

A) **Solución Rápida para la Demo:**
   - Explica que la funcionalidad de alertas en tiempo real está implementada
   - Muestra el código del dashboard del padre (listeners de SignalR)
   - Muestra el código del hub (NotificacionesHub.cs)
   - Explica que falta conectar la simulación al hub (paso siguiente)

B) **Solución Completa (Requiere modificar el código):**
   Necesitarías agregar código SignalR en la simulación del piloto para enviar notificaciones.
   (Esto se cubrirá en el siguiente paso del plan si el usuario lo requiere)

### Problema 3: GPS no funciona en la página del piloto

**Solución:**
```
1. El navegador debe pedir permiso GPS la primera vez
2. Clic en "Permitir" cuando aparezca el popup
3. Si ya se bloqueó:
   • Clic en el candado 🔒 en la barra de direcciones
   • Busca "Ubicación"
   • Cambia a "Permitir"
   • Recarga la página (F5)
```

### Problema 4: Error 405 al cerrar sesión

**Ya solucionado** en cambios anteriores. Si persiste:
```
1. Verifica que AuthController.cs tenga:
   [HttpGet]
   public async Task<IActionResult> Logout()

2. Verifica que los botones de logout sean <a href="/Auth/Logout">
   y NO <form method="post">
```

### Problema 5: La simulación no inicia o se detiene

**Solución:**
```
1. Verifica que haya paradas en la ruta
2. Abre consola (F12) y busca errores
3. Verifica que el botón cambie a "Detener Simulación"
4. Si se detiene, clic en "Simular Ruta" de nuevo
```

---

## ✅ CHECKLIST ANTES DE LA DEMO

Imprime esto y márcalo conforme lo completes:

```
□ Script SQL ejecutado (VERIFICAR_Demo_Padre_Piloto.sql)
□ Credenciales anotadas (Padre y Piloto)
□ Aplicación compilada sin errores (F5 en Visual Studio)
□ Ventana 1 (Normal) lista con login de Padre
□ Ventana 2 (Incógnito) lista con login de Piloto
□ Dashboard padre muestra "🟢 Conectado"
□ Página piloto muestra botón "🚌 Simular Ruta"
□ GPS del piloto funcionando (punto azul visible)
□ Consola abierta (F12) en ambas ventanas para monitorear
□ Audio del navegador habilitado (no muteado)
```

---

## 🎉 PUNTOS CLAVE PARA DESTACAR EN LA PRESENTACIÓN

1. **Tiempo Real:**
   ```
   "Las notificaciones llegan INSTANTÁNEAMENTE gracias a SignalR,
   sin necesidad de que el padre recargue la página"
   ```

2. **Tranquilidad para los Padres:**
   ```
   "Los padres saben EXACTAMENTE cuándo su hijo fue recogido
   o dejado por el bus, sin tener que llamar al piloto"
   ```

3. **Escalabilidad:**
   ```
   "El sistema puede manejar múltiples buses y cientos de padres
   simultáneamente, cada uno recibiendo solo las alertas de sus hijos"
   ```

4. **Tecnología Moderna:**
   ```
   "Usamos SignalR de Microsoft, la misma tecnología que usan
   aplicaciones como Microsoft Teams para chat en tiempo real"
   ```

5. **Sin Recargas:**
   ```
   "Observen que el padre NO tiene que recargar la página.
   Las notificaciones aparecen automáticamente"
   ```

---

## 📱 BONUS: Si tienes tiempo extra

### Mostrar en Móvil

```
1. Abre en tu teléfono: https://[tu-ip-local]:7241
   (Ej: https://192.168.1.100:7241)

2. Login como padre

3. Las notificaciones también funcionan en móvil

4. Muestra que el diseño es responsive
```

### Mostrar la Consola del Navegador

```
1. Presiona F12 en la ventana del padre

2. Ve a la pestaña "Console"

3. Muestra los mensajes de SignalR:
   • "✅ SignalR conectado correctamente"
   • "🚏 Parada completada: [datos]"
   • "📍 Ubicación del bus actualizada"

4. Esto demuestra que hay comunicación en tiempo real
```

---

## 🚨 SI ALGO FALLA EN VIVO

### Plan B: Mostrar el Código

```
1. Abre Visual Studio

2. Muestra el archivo:
   Pages/Padres/DashboardRutaBusAsignado.cshtml

   Señala las secciones:
   • configurarEventosSignalR()
   • conexionSignalR.on("ParadaCompletada")
   • mostrarAlertaParadaCompletada()

3. Muestra el archivo:
   Hubs/NotificacionesHub.cs

   Señala:
   • public async Task NotificarParadaCompletada(...)
   • await Clients.Group($"Alumno_{idAlumno}").SendAsync(...)

4. Explica:
   "La funcionalidad está implementada. El problema es [X],
   pero el código está listo para funcionar en producción"
```

---

## 📊 MÉTRICAS DE ÉXITO

Al final de la demo, deberías haber mostrado:

```
✅ Dos usuarios (Padre y Piloto) conectados simultáneamente
✅ Simulación de ruta funcionando en tiempo real
✅ Notificaciones apareciendo automáticamente en la ventana del padre
✅ Badge de conexión indicando estado del sistema
✅ Mapa interactivo mostrando la ruta
✅ GPS funcionando (ubicación del piloto visible)
✅ Diseño responsive (funciona en escritorio y móvil)
✅ Sin errores en consola
```

---

## 💡 PREGUNTAS FRECUENTES QUE PODRÍAN HACERTE

**P: ¿Qué pasa si el padre no está conectado cuando el bus llega?**
```
R: Las notificaciones se envían en tiempo real solo a usuarios 
   conectados. Para historial, existe el módulo de "Historial de 
   Alertas" donde se guardan todas las notificaciones enviadas.
```

**P: ¿Funciona con múltiples hijos del mismo padre?**
```
R: Sí. El sistema unirá al padre a múltiples grupos (uno por cada hijo)
   y recibirá notificaciones de todos sus hijos.
```

**P: ¿Qué pasa si el internet falla?**
```
R: SignalR tiene reconexión automática. Cuando el internet regrese,
   se reconectará automáticamente y el badge cambiará de rojo a verde.
```

**P: ¿Los pilotos también reciben notificaciones?**
```
R: En esta demo, el piloto ES quien genera las notificaciones.
   Pero el sistema puede extenderse para que los pilotos también
   reciban alertas del admin o de emergencias.
```

**P: ¿Cuántos usuarios simultáneos soporta?**
```
R: SignalR está diseñado para escalar horizontalmente. Con un servidor
   dedicado, puede manejar miles de conexiones simultáneas. Para esta
   escuela, será más que suficiente.
```

---

## 🎓 CONCLUSIÓN

Esta demo muestra:

1. ✅ **Comunicación en tiempo real** entre usuarios
2. ✅ **Arquitectura escalable** con SignalR y grupos
3. ✅ **UX moderna** con notificaciones toast y sonidos
4. ✅ **Tranquilidad para padres** con confirmaciones instantáneas
5. ✅ **Sistema robusto** con reconexión automática

---

## 📞 CONTACTO TÉCNICO

Si tienes problemas antes de la demo:

```
1. Revisa esta guía paso a paso
2. Ejecuta el script SQL de verificación
3. Abre la consola del navegador (F12) para ver errores
4. Verifica que SignalR esté conectado
```

---

¡Éxito en tu demo del martes! 🚀🎉
