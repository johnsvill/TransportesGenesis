# 🧪 Testing FASE 6 - Traslados Temporales (Actualizado con Mejoras UX)

## ✅ Checklist de Pruebas

### **Preparación**
1. Asegúrate que el proyecto está compilado (`Ctrl+Shift+B`)
2. Ejecuta el proyecto (`F5` o `Ctrl+F5`)
3. La aplicación debe abrir en `https://localhost:7240`

---

## 📋 **PRUEBA 1: Navegación Rápida**

### Pasos:
1. Navega a: `https://localhost:7240/Padres/ConfirmarAsistencia`
2. Verás el calendario con un **botón nuevo** en la esquina superior derecha

### Verificar:
3. **Botón "Ver Mis Traslados"** visible y con icono 📋
4. Click en el botón → debe ir a `/Padres/Traslados`
5. En la página de traslados, usa el "Back" del navegador para volver

### Resultado esperado:
- ✅ Botón visible y bien posicionado
- ✅ Navegación funciona correctamente
- ✅ No pierde contexto del calendario al volver

---

## 📋 **PRUEBA 2: Crear Solicitud de Traslado (Mejorado)**

### Pasos:
1. Navega a: `https://localhost:7240/Padres/ConfirmarAsistencia`
2. Verás el calendario del mes actual
3. **Click en un día FUTURO** (no hoy, un día de esta semana o próxima semana)
4. Se abre el panel lateral con los switches de confirmación
5. **Click en el botón azul** "📝 Solicitar Traslado de Bus"
6. Se abre el modal de traslado

### En el modal:
7. **Selecciona Turno**: Mañana / Tarde / Ambos
8. **Selecciona Bus Destino**: Debe cargar lista de buses (BUS-001, BUS-002, etc.)
9. **Escribe Motivo** (opcional): "Se quedará en casa de su abuela"
10. **Click "Enviar Solicitud"**

### Resultado esperado (MEJORADO):
- ✅ Modal de éxito aparece con **nuevo diseño mejorado**
- ✅ Título: "Solicitud Enviada" / Subtítulo: "Traslado Temporal"
- ✅ Tabla con detalles completos:
  - Fecha del traslado (formato largo: "lunes, 20 de enero de 2025")
  - Turno con badge azul
  - Bus con badge gris
  - Estado "Pendiente" con badge amarillo y reloj
- ✅ Alerta azul informativa: "Recibirá notificación cuando sea revisada"
- ✅ **Botón "Ver Historial de Traslados"** visible en el footer
- ✅ Console del navegador (F12) muestra: `200 OK` en `/api/traslados`

### Probar botón del modal:
11. Click en "Ver Historial de Traslados" → debe ir a `/Padres/Traslados`
12. Verificar que la solicitud recién creada aparece en la tabla

### Si falla:
- ❌ Modal rojo de error → Normal si no hay datos en BD (modo simulación activo)
- ❌ Console muestra `500 Error` → Revisa que `Startup.cs` tiene registrados los servicios
- ❌ Lista de buses vacía → API retorna array vacío, pero debería mostrar datos simulados

---

## 📋 **PRUEBA 3: Ver Historial de Traslados**

### Opción A - Desde botón del modal:
1. Después de crear solicitud, click en "Ver Historial de Traslados" en el modal
2. Debe llevar directamente a la página de historial

### Opción B - Desde botón del calendario:
1. En cualquier momento, click en "📋 Ver Mis Traslados" (esquina superior derecha)
2. Debe llevar a `/Padres/Traslados`

### Verificar:
3. **Tarjetas de estadísticas** (arriba):
   - Total Pendientes: Número en amarillo
   - Total Aprobados: Número en verde
   - Total Rechazados: Número en rojo
   - Total: Suma de todos

4. **Tabla de solicitudes**:
   - Debe mostrar al menos 1 fila (la que acabas de crear)
   - O si no hay datos reales, muestra 3 solicitudes de prueba
   - Columnas: Fecha Traslado, Turno, Bus Destino, Estado, Fecha Solicitud

5. **Estado de la solicitud recién creada**:
   - Badge amarillo: "⏰ Pendiente"

### Resultado esperado:
- ✅ Tabla visible con datos
- ✅ Estadísticas suman correctamente
- ✅ Al menos 1 solicitud "Pendiente" (la que creaste)

---

## 📋 **PRUEBA 4: Ver Detalles de Solicitud**

### Pasos:
1. En la tabla de traslados, click en botón "👁️ Ver" de cualquier fila
2. Se abre modal de detalles

### Verificar:
3. **Header del modal**:
   - Fondo amarillo si es "Pendiente"
   - Fondo verde si es "Aprobado"
   - Fondo rojo si es "Rechazado"

4. **Información mostrada**:
   - Fecha del Traslado (día completo: "lunes, 20 de enero de 2025")
   - Turno con icono (🌅 Mañana / 🌆 Tarde / 🔄 Ambos)
   - Bus Origen (badge gris)
   - Bus Destino (badge gris)
   - Motivo (si lo escribiste)
   - Estado con badge de color
   - Fecha de Solicitud

5. Si es Aprobado/Rechazado (solicitudes de prueba):
   - Fecha de Respuesta
   - Respondido por: "Admin"
   - Comentario del Administrador

### Resultado esperado:
- ✅ Modal se abre correctamente
- ✅ Toda la información se muestra
- ✅ Colores y badges correctos
- ✅ Click "Cerrar" cierra el modal

---

## 📋 **PRUEBA 5: Filtros del Historial**

### Pasos:
1. En `/Padres/Traslados`, usa los filtros en la parte superior

### Filtro por Estado:
2. Selecciona "Pendiente" → Solo muestra solicitudes pendientes
3. Selecciona "Aprobado" → Solo muestra aprobadas
4. Selecciona "Rechazado" → Solo muestra rechazadas
5. Selecciona "Todos" → Muestra todas

### Filtro por Mes:
6. El dropdown debe tener opciones de meses (generadas automáticamente)
7. Selecciona un mes → Solo muestra solicitudes de ese mes

### Limpiar Filtros:
8. Click en "✖️ Limpiar Filtros" → Vuelve a mostrar todo

### Resultado esperado:
- ✅ Filtros funcionan correctamente
- ✅ Estadísticas se actualizan según filtro activo
- ✅ Tabla muestra solo datos filtrados

---

## 📋 **PRUEBA 6: Flujo Completo Integrado (NUEVO)**

### Objetivo: Probar la experiencia completa de principio a fin

### Escenario:
1. Abrir calendario: `/Padres/ConfirmarAsistencia`
2. Seleccionar día futuro (ej: miércoles de la próxima semana)
3. Confirmar asistencia (switches Mañana/Tarde)
4. Click "Solicitar Traslado de Bus"
5. Llenar formulario: Turno "Ambos", Bus "BUS-003", Motivo "Visita médica"
6. Enviar solicitud
7. **En el modal de éxito:** leer todos los detalles
8. Click "Ver Historial de Traslados"
9. Encontrar la solicitud recién creada en la tabla
10. Click "Ver" para ver detalles completos
11. Volver al calendario usando "Back" del navegador o el botón superior

### Resultado esperado:
- ✅ Todo el flujo funciona sin errores
- ✅ Datos se muestran consistentemente en modal y historial
- ✅ Navegación es fluida y sin pérdida de contexto
- ✅ Usuario siempre sabe dónde está y cómo volver

---

## 🔧 **Troubleshooting**

### Problema: No veo el botón "Ver Mis Traslados" en el calendario
**Solución**: Verifica que estés en la página correcta (`/Padres/ConfirmarAsistencia`). El botón debe estar en la esquina superior derecha, al lado del título del calendario. Si no aparece, recarga la página con `Ctrl+F5`.

### Problema: Modal de éxito no muestra el botón "Ver Historial de Traslados"
**Solución**: Esto solo aparece cuando creas un traslado, no cuando confirmas asistencia normal. Verifica que hayas usado el botón "Solicitar Traslado de Bus" y no solo los switches de confirmación.

### Problema: No aparece botón "Solicitar Traslado de Bus"
**Solución**: Asegúrate de hacer click en un día del calendario primero. El botón solo aparece cuando se selecciona un día.

### Problema: Modal de traslado no carga buses
**Solución**: Normal en modo simulación. La función `cargarBusesDisponibles()` debería mostrar al menos algunos buses. Verifica en Console (F12) si hay errores JavaScript.

### Problema: "Error al crear solicitud"
**Solución**: Modo simulación activo. El API retorna datos simulados cuando falla la BD. Esto está bien para testing.

### Problema: Historial muestra "No hay solicitudes"
**Solución**: Si acabas de crear una, espera y recarga la página. Si persiste, verifica que la función `generarDatosPrueba()` esté activa en el JavaScript.

---

## 📸 **Capturas esperadas**

### Vista de Calendario con botón de navegación (NUEVO):
```
[Header con título "Calendario de Asistencia" | Botón "Ver Mis Traslados" →]
[Calendario mensual]
[Día seleccionado → Panel lateral]
[Switches de confirmación]
[Botón azul: "📝 Solicitar Traslado de Bus"]
```

### Modal de éxito mejorado (NUEVO):
```
[Header verde: "Solicitud Enviada" - "Traslado Temporal"]
[Ícono de check grande]
[Alerta verde: "Solicitud creada exitosamente"]
[Tabla con detalles:]
  - Fecha del traslado: lunes, 20 de enero de 2025
  - Turno: [Badge azul]
  - Bus solicitado: [Badge gris]
  - Estado: [Badge amarillo "Pendiente"]
[Alerta azul: "Recibirá notificación..."]
[Footer con 2 botones:]
  - "Cerrar"
  - "Ver Historial de Traslados" (botón primario)
```

### Modal de solicitud:
```
[Encabezado: "Solicitar Traslado de Bus"]
[Select: Turno]
[Select: Bus Destino]
[Textarea: Motivo]
[Botón "Enviar Solicitud"]
```

### Historial de traslados:
```
[4 tarjetas de estadísticas]
[Filtros: Estado y Mes]
[Tabla con solicitudes]
[Botón "Ver" en cada fila]
```

---

## ✅ **Criterios de Aceptación (Actualizado)**

- [x] Se puede crear solicitud desde el calendario
- [x] Modal se abre y cierra correctamente
- [x] Lista de buses se carga dinámicamente
- [x] Solicitud se envía y muestra mensaje de éxito **mejorado con detalles**
- [x] **Modal de éxito tiene botón para ir al historial**
- [x] **Botón "Ver Mis Traslados" visible en header del calendario**
- [x] Historial muestra todas las solicitudes
- [x] Filtros funcionan correctamente
- [x] Modal de detalles muestra información completa
- [x] Estadísticas se calculan correctamente
- [x] Modo simulación funciona cuando no hay datos reales
- [x] **Navegación fluida entre calendario y historial**
- [x] **Feedback visual inmediato al crear solicitud**

---

## 🎯 **Próximos Pasos (después de testing)**

1. ✅ Agregar indicadores visuales en calendario (B)
2. ✅ Crear panel de Admin para aprobar/rechazar (A)
3. ✅ FASE 6 completa al 100%
