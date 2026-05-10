# 📘 ESPECIFICACIÓN FUNCIONAL - TRANSPORTES GENESIS
## Sistema de Gestión de Transporte Escolar

---

**Proyecto:** Transportes Genesis  
**Tipo de Documento:** Especificación Funcional  
**Versión:** 1.0  
**Fecha:** 2025  
**Autor:** Equipo de Desarrollo TransportesGenesis  
**Repositorio:** https://github.com/johnsvill/TransportesGenesis  
**Rama:** dev_david  

---

## 📑 ÍNDICE

1. [Introducción y alcance del sistema](#11-introducción-y-alcance-del-sistema)
2. [Contexto organizacional y stakeholders](#12-contexto-organizacional-y-stakeholders)
3. [Análisis de Requerimientos Funcionales](#13-análisis-de-requerimientos-funcionales)
4. [Análisis de Requerimientos No Funcionales](#14-análisis-de-requerimientos-no-funcionales)
5. [Análisis de Dominio](#15-análisis-de-dominio)
6. [Supuestos, restricciones y dependencias funcionales](#16-supuestos-restricciones-y-dependencias-funcionales)

---

## 1.1 Introducción y alcance del sistema

### 1.1.1 Descripción General

**Transportes Genesis** es un sistema integral de gestión de transporte escolar desarrollado en **.NET 8** con **Razor Pages** que digitaliza y optimiza las operaciones diarias de empresas de transporte escolar. El sistema proporciona una plataforma centralizada que conecta a todos los actores involucrados en el servicio de transporte: administradores, pilotos, monitores y padres de familia.

### 1.1.2 Problema que Resuelve

Las empresas de transporte escolar tradicionalmente operan con procesos manuales que generan:

- **Ineficiencia operativa**: Planificación de rutas manual y sin optimización
- **Falta de visibilidad**: Los padres no saben dónde está el bus en tiempo real
- **Desorganización**: Registros en papel de recogidas, asistencias y pagos
- **Baja satisfacción del cliente**: Poca comunicación y transparencia
- **Riesgos de seguridad**: Falta de trazabilidad de quién recoge a cada alumno
- **Costos elevados**: Rutas no optimizadas consumen más combustible
- **Errores humanos**: Pérdida de registros, pagos no confirmados, asistencias mal registradas

### 1.1.3 Objetivos del Proyecto

#### Objetivos Generales

1. **Digitalizar** todas las operaciones de transporte escolar en una plataforma web centralizada
2. **Optimizar** las rutas mediante algoritmos de cálculo automático (TSP - Traveling Salesman Problem)
3. **Proporcionar visibilidad en tiempo real** de la ubicación de los buses mediante geolocalización
4. **Mejorar la comunicación** entre la empresa de transporte y los padres de familia
5. **Aumentar la seguridad** con registro detallado de recogidas y asistencias
6. **Reducir costos operativos** mediante rutas optimizadas y procesos automatizados

#### Objetivos Específicos

1. Implementar un sistema de **autenticación y autorización** basado en roles
2. Crear un módulo de **cálculo automático de rutas** optimizadas
3. Desarrollar un sistema de **geolocalización en tiempo real** con SignalR
4. Proveer un módulo de **gestión de pagos** para padres de familia
5. Implementar un sistema de **confirmación de asistencia** diaria
6. Crear un módulo de **registro de recogidas** para monitores
7. Proporcionar dashboards y reportes para administradores

### 1.1.4 Alcance del Sistema

#### ✅ Dentro del Alcance (v1.0)

El sistema **SÍ incluye**:

1. **Gestión de Usuarios y Roles**
   - Login/Logout con ASP.NET Core Identity
   - Roles: Administrador, Piloto, Monitor, Padre de Familia
   - Gestión de perfiles de usuario
   - Cambio de contraseña (obligatorio en primer login)

2. **Gestión de Buses y Asignaciones**
   - CRUD de buses (placa, modelo, capacidad)
   - Asignación de pilotos a buses
   - Asignación de monitores a buses
   - Historial de asignaciones

3. **Gestión de Alumnos**
   - CRUD de alumnos
   - Vinculación con padres de familia
   - Información básica: nombre, fecha de nacimiento, grado

4. **Gestión de Paradas**
   - CRUD de paradas
   - Geolocalización de paradas (latitud, longitud)
   - Direcciones descriptivas

5. **Cálculo y Gestión de Rutas**
   - Cálculo automático de rutas optimizadas (algoritmo TSP)
   - Rutas diferenciadas por turno: Mañana y Tarde
   - Asignación de paradas a rutas con orden secuencial
   - Estimación de horarios por parada
   - Visualización de rutas para pilotos y monitores

6. **Geolocalización en Tiempo Real**
   - Envío de ubicación del bus cada X segundos (SignalR)
   - Mapa en tiempo real para visualizar buses
   - Histórico de ubicaciones (opcional en v1.0)

7. **Módulo de Pagos**
   - Registro de pagos realizados por padres
   - Historial de pagos
   - Estado de pagos (Pendiente, Confirmado)
   - Métodos de pago (efectivo, transferencia, etc.)

8. **Confirmación de Asistencia**
   - Los padres pueden confirmar si el alumno asistirá o no
   - Confirmación diaria
   - Visualización del estado de confirmación

9. **Registro de Recogidas (Monitor)**
   - El monitor registra qué alumnos fueron recogidos en cada parada
   - Prevención de duplicados
   - Registro de fecha/hora y ubicación de recogida
   - Visualización del estado de recogidas

10. **Dashboards y Reportes Básicos**
    - Panel de administración con estadísticas básicas
    - Reportes de pagos
    - Reportes de asistencia

#### ❌ Fuera del Alcance (v1.0)

El sistema **NO incluye** en la versión inicial:

1. **Aplicación móvil nativa** (iOS/Android)
   - Solo interfaz web responsive (funciona en móviles vía navegador)

2. **Notificaciones push**
   - No se envían notificaciones automáticas a dispositivos móviles
   - Posible en versiones futuras con Firebase/OneSignal

3. **Integración con pasarelas de pago en línea**
   - Los pagos se registran manualmente (no hay integración con Stripe, PayPal, etc.)
   - Posible en v2.0

4. **Chat en tiempo real** entre usuarios
   - No hay mensajería interna en el sistema

5. **Machine Learning** para predicción de tiempos
   - Predicción de tiempos de llegada con ML no está implementada
   - Posible en versiones futuras

6. **Integración con sistemas contables externos**
   - No hay conexión con QuickBooks, SAP, etc.

7. **Gestión de mantenimiento de buses**
   - Registro de mantenimientos preventivos/correctivos no incluido en v1.0

8. **Módulo de quejas y reclamos formal**
   - No hay sistema de tickets de soporte en v1.0

9. **Análisis avanzado de datos (BI)**
   - Dashboards básicos únicamente; no hay herramientas de Business Intelligence

10. **Multi-idioma**
    - El sistema está únicamente en español

### 1.1.5 Beneficios Esperados

#### Para la Empresa de Transporte

- ✅ **Reducción del 20-30% en costos de combustible** gracias a rutas optimizadas
- ✅ **Ahorro de 10-15 horas/semana** en tareas administrativas
- ✅ **Mayor control operativo** con visibilidad en tiempo real de toda la flota
- ✅ **Reducción de errores** en registros de pagos y asistencias
- ✅ **Mejora en la imagen corporativa** al ofrecer tecnología moderna

#### Para los Pilotos

- ✅ **Rutas claras y optimizadas** sin necesidad de planificación manual
- ✅ **Información actualizada** de alumnos que asistirán cada día
- ✅ **Interfaz simple** para ver su ruta diaria

#### Para los Monitores

- ✅ **Proceso digitalizado** de registro de recogidas (adiós al papel)
- ✅ **Prevención de errores** con validaciones automáticas
- ✅ **Interfaz intuitiva** para marcar asistencias en tiempo real

#### Para los Padres de Familia

- ✅ **Tranquilidad** al ver dónde está el bus en tiempo real
- ✅ **Comunicación efectiva** al confirmar asistencia de su hijo
- ✅ **Transparencia** en el estado de pagos
- ✅ **Accesibilidad** desde cualquier dispositivo con navegador

### 1.1.6 Usuarios del Sistema

| Rol | Descripción | Cantidad Estimada | Funcionalidades Principales |
|-----|-------------|-------------------|----------------------------|
| **Administrador** | Dueño o gerente de la empresa de transporte | 1-3 por empresa | Gestión completa del sistema, reportes, configuración |
| **Piloto** | Conductor del bus | 5-50 por empresa | Ver su ruta diaria, actualizar ubicación |
| **Monitor** | Acompañante en el bus que supervisa a los alumnos | 5-50 por empresa | Ver ruta, registrar recogidas, marcar asistencias |
| **Padre de Familia** | Tutor del alumno | 50-500 por empresa | Ver ubicación del bus, confirmar asistencia, ver pagos |

---

✅ **1.1 INTRODUCCIÓN Y ALCANCE DEL SISTEMA - COMPLETADO**

---

## 1.2 Contexto organizacional y stakeholders

### 1.2.1 Modelo Organizacional de Empresa de Transporte Escolar

**[INSTRUCCIONES PARA DIAGRAMA ORGANIZACIONAL]**

```
Título: Estructura Organizacional Típica - Empresa de Transporte Escolar

Nivel 1: Dirección General / Dueño
  |
  ├── Nivel 2: Administrador / Gerente de Operaciones
  |     |
  |     ├── Nivel 3: Coordinador de Rutas
  |     ├── Nivel 3: Encargado de Finanzas/Pagos
  |     └── Nivel 3: Supervisor de Flota
  |
  ├── Nivel 2: Pilotos (Conductores)
  |     └── Asignados a buses específicos
  |
  └── Nivel 2: Monitores
        └── Asignados a buses específicos

Relación externa:
- Padres de Familia → Clientes del servicio
- Alumnos → Usuarios finales del transporte

Interacción con el sistema:
- Administrador gestiona todo
- Pilotos y Monitores operan el servicio diario
- Padres consultan información y realizan pagos
```

---

### 1.2.2 Stakeholders del Proyecto

#### Stakeholders Primarios (Usuarios Directos)

| Stakeholder | Rol en el Negocio | Interés en el Sistema | Influencia | Expectativas |
|-------------|-------------------|----------------------|------------|--------------|
| **Dueño de la empresa** | Inversionista principal | Alto | 🔴 Crítica | ROI, eficiencia, reducción de costos |
| **Administrador** | Gerente de operaciones | Alto | 🔴 Crítica | Sistema completo, confiable, reportes precisos |
| **Pilotos** | Operadores de buses | Medio | 🟡 Media | Interfaz simple, rutas claras |
| **Monitores** | Supervisores de alumnos | Medio | 🟡 Media | Registro rápido, fácil de usar en movimiento |
| **Padres de Familia** | Clientes pagadores | Alto | 🟢 Baja | Visibilidad, transparencia, seguridad de sus hijos |
| **Alumnos** | Usuarios finales (pasivos) | Bajo | 🟢 Baja | No interactúan directamente con el sistema |

---

#### Stakeholders Secundarios

| Stakeholder | Interés | Influencia | Expectativas |
|-------------|---------|------------|--------------|
| **Instituciones Educativas** | Coordinación con servicio de transporte | 🟡 Media | Puntualidad, comunicación con padres |
| **Autoridades de Tránsito** | Regulación y permisos | 🟢 Baja | Cumplimiento de normativas (no aplica al software directamente) |
| **Proveedores de Tecnología** | Azure, Google Maps API | 🟡 Media | Pago puntual de servicios, uso correcto de APIs |
| **Equipo de Desarrollo** | Implementadores | 🔴 Crítica | Requerimientos claros, feedback oportuno |

---

### 1.2.3 Flujo Operativo Actual vs Propuesto

#### Proceso Actual (Manual)

**Flujo: Planificación de Rutas**

```
[Administrador]
   ↓
1. Recibe lista de alumnos y direcciones en Excel
   ↓
2. Imprime mapa y marca ubicaciones manualmente
   ↓
3. Traza rutas "a ojo" intentando optimizar
   ↓
4. Comunica rutas a pilotos por WhatsApp o papel
   ↓
5. Pilotos memorizan o anotan en papel
```

**Problemas:**
- ❌ Rutas no optimizadas (más tiempo y combustible)
- ❌ Cambios no se comunican eficientemente
- ❌ Sin visibilidad de quién recoge a quién

---

**Flujo: Registro de Recogidas**

```
[Monitor]
   ↓
1. Lleva lista impresa de alumnos por parada
   ↓
2. Marca con lápiz quién subió al bus
   ↓
3. Al final del día, entrega lista al administrador
   ↓
4. Administrador transcribe a Excel (doble trabajo)
```

**Problemas:**
- ❌ Papeles se pierden o mojan
- ❌ Caligrafía ilegible
- ❌ Retraso en información (no es en tiempo real)

---

**Flujo: Control de Pagos**

```
[Padre de Familia]
   ↓
1. Hace depósito/transferencia bancaria
   ↓
2. Toma foto del comprobante, envía por WhatsApp al administrador
   ↓
3. Administrador anota en Excel manualmente
   ↓
4. Padre pregunta si se confirmó el pago (días después)
```

**Problemas:**
- ❌ Desorganización en WhatsApp (mensajes se pierden)
- ❌ No hay trazabilidad clara
- ❌ Padre no tiene acceso a su historial de pagos

---

#### Proceso Propuesto (Digitalizado con Transportes Genesis)

**Flujo: Planificación de Rutas**

```
[Administrador en el sistema]
   ↓
1. Ingresa alumnos y paradas con geolocalización
   ↓
2. Sistema calcula ruta óptima automáticamente (algoritmo TSP)
   ↓
3. Asigna ruta al bus y al piloto
   ↓
4. Piloto/Monitor acceden a su ruta desde su panel
   ↓
5. Ven mapa con orden de paradas y horarios estimados
```

**Beneficios:**
- ✅ Rutas optimizadas (ahorro 20-30% combustible)
- ✅ Cambios instantáneos (todos actualizados)
- ✅ Visibilidad total

---

**Flujo: Registro de Recogidas**

```
[Monitor en el sistema (tablet/móvil)]
   ↓
1. Ve su ruta con lista de alumnos por parada
   ↓
2. En cada parada, marca checkboxes de quién subió
   ↓
3. Sistema registra automáticamente:
   - Fecha/hora
   - Ubicación GPS
   - Usuario que registró
   ↓
4. Administrador ve reportes en tiempo real
```

**Beneficios:**
- ✅ Digitalizado, sin papel
- ✅ Información en tiempo real
- ✅ Prevención de duplicados

---

**Flujo: Control de Pagos**

```
[Padre de Familia en el sistema]
   ↓
1. Hace depósito/transferencia bancaria
   ↓
2. Entra al sistema, sección "Mis Pagos"
   ↓
3. Ingresa:
   - Monto
   - Método (transferencia/efectivo)
   - Comprobante (sube foto)
   ↓
4. Administrador recibe notificación, valida y confirma
   ↓
5. Padre ve estado actualizado: "Confirmado" ✅
   ↓
6. Historial de pagos siempre accesible
```

**Beneficios:**
- ✅ Padre tiene acceso a su historial
- ✅ Trazabilidad completa
- ✅ Organización centralizada (no WhatsApp)

---

### 1.2.4 Análisis de Impacto en Stakeholders

| Stakeholder | Impacto del Sistema | Cambios en su Trabajo | Resistencia Esperada | Estrategia de Mitigación |
|-------------|---------------------|----------------------|---------------------|--------------------------|
| **Administrador** | 🟢 Positivo Alto | Digitalización total, más tiempo para estrategia | 🟡 Media (curva de aprendizaje) | Capacitación dedicada, soporte 1:1 |
| **Pilotos** | 🟢 Positivo Medio | Menos confusión, rutas claras | 🟡 Media (algunos no son tech-savvy) | Interfaz muy simple, entrenamiento práctico |
| **Monitores** | 🟢 Positivo Alto | Adiós al papel, proceso más rápido | 🟡 Media (acostumbrados al papel) | Onboarding con tablet, práctica guiada |
| **Padres** | 🟢 Positivo Alto | Mayor tranquilidad y control | 🟢 Baja (la mayoría usa smartphones) | Tutorial en video, soporte por WhatsApp |

---

### 1.2.5 Mapa de Empatía por Stakeholder

#### Administrador

**¿Qué piensa y siente?**
- "Necesito controlar todo desde un solo lugar"
- "No puedo perder tiempo transcribiendo datos manualmente"
- "Quiero que los padres estén satisfechos para retenerlos como clientes"

**¿Qué ve?**
- Excel desorganizado
- WhatsApp lleno de mensajes de padres y pilotos
- Papeles con listas de recogidas acumuladas

**¿Qué dice y hace?**
- "¿Ya registraron las recogidas de hoy?"
- "¿Confirmaron los pagos de esta semana?"
- Pasa horas consolidando información

**Pains (Dolores):**
- ❌ Pérdida de información
- ❌ Errores humanos
- ❌ Falta de reportes consolidados

**Gains (Ganancias esperadas):**
- ✅ Ahorro de 10+ horas/semana
- ✅ Reportes automáticos
- ✅ Control total en un solo sistema

---

#### Padre de Familia

**¿Qué piensa y siente?**
- "¿Dónde está el bus? ¿Ya pasó?"
- "¿Recogieron a mi hijo?"
- "¿Por qué no me confirman si mi pago fue recibido?"

**¿Qué ve?**
- Su hijo esperando en la calle
- Falta de comunicación de la empresa

**¿Qué dice y hace?**
- Llama a la empresa preguntando por el bus
- Envía comprobantes de pago por WhatsApp
- Pregunta varias veces si el pago fue confirmado

**Pains (Dolores):**
- ❌ Ansiedad por no saber dónde está el bus
- ❌ Falta de transparencia en pagos

**Gains (Ganancias esperadas):**
- ✅ Ver el bus en tiempo real (tranquilidad)
- ✅ Historial de pagos siempre accesible
- ✅ Confirmar asistencia de su hijo fácilmente

---

✅ **1.2 CONTEXTO ORGANIZACIONAL Y STAKEHOLDERS - COMPLETADO**

---

## 1.3 Análisis de Requerimientos Funcionales

### 1.3.1 Requerimientos Funcionales por Módulo

#### RF-01: Módulo de Autenticación y Autorización

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-01.1** | El sistema debe permitir login con email y contraseña | 🔴 Crítica | Todos | Usuario accede con credenciales válidas |
| **RF-01.2** | El sistema debe soportar 4 roles: Administrador, Piloto, Monitor, Padre de Familia | 🔴 Crítica | Sistema | Cada usuario solo ve funcionalidades de su rol |
| **RF-01.3** | El sistema debe forzar cambio de contraseña en primer login | 🟡 Alta | Todos | Usuario no puede acceder hasta cambiar contraseña |
| **RF-01.4** | El sistema debe permitir logout | 🔴 Crítica | Todos | Sesión se cierra correctamente |
| **RF-01.5** | El sistema debe redirigir a cada rol a su página principal después del login | 🟡 Alta | Sistema | Administrador → Admin panel, Piloto → MiRuta, Monitor → MiRuta, Padre → Mapa |
| **RF-01.6** | El sistema debe mostrar mensaje de error si credenciales son inválidas | 🟡 Alta | Todos | Mensaje: "Email o contraseña incorrectos" |

---

#### RF-02: Módulo de Gestión de Buses

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-02.1** | El administrador debe poder crear un bus con: placa, modelo, capacidad | 🔴 Crítica | Administrador | Bus se guarda en BD con validaciones |
| **RF-02.2** | El administrador debe poder listar todos los buses | 🔴 Crítica | Administrador | Tabla con todos los buses y acciones (editar, eliminar) |
| **RF-02.3** | El administrador debe poder editar un bus | 🟡 Alta | Administrador | Cambios se reflejan en BD |
| **RF-02.4** | El administrador debe poder eliminar un bus (soft delete) | 🟡 Alta | Administrador | Bus no se muestra en listados pero se mantiene en BD |
| **RF-02.5** | El sistema debe validar que la placa sea única | 🟡 Alta | Sistema | Error si placa ya existe |
| **RF-02.6** | El administrador debe poder asignar un piloto a un bus | 🔴 Crítica | Administrador | Registro en tabla `AsignacionPilotoBus` |
| **RF-02.7** | El administrador debe poder ver historial de asignaciones de un bus | 🟢 Media | Administrador | Listado de pilotos que han operado el bus |

---

#### RF-03: Módulo de Gestión de Alumnos

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-03.1** | El administrador debe poder crear un alumno con: nombre, apellido, fecha de nacimiento, grado | 🔴 Crítica | Administrador | Alumno se guarda en BD |
| **RF-03.2** | El administrador debe poder asignar un alumno a un padre de familia | 🔴 Crítica | Administrador | Relación en tabla `AlumnoPadreFamilia` |
| **RF-03.3** | El administrador debe poder asignar un alumno a una parada | 🔴 Crítica | Administrador | Registro en `AsignacionAlumnoParada` |
| **RF-03.4** | El administrador debe poder listar todos los alumnos | 🔴 Crítica | Administrador | Tabla con alumnos y acciones |
| **RF-03.5** | El administrador debe poder editar un alumno | 🟡 Alta | Administrador | Cambios se reflejan en BD |
| **RF-03.6** | El administrador debe poder eliminar un alumno (soft delete) | 🟡 Alta | Administrador | Alumno no se muestra pero se mantiene en BD |
| **RF-03.7** | El sistema debe validar que la fecha de nacimiento sea coherente (alumno entre 3-18 años) | 🟢 Media | Sistema | Error si fecha no es válida |

---

#### RF-04: Módulo de Gestión de Paradas

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-04.1** | El administrador debe poder crear una parada con: nombre, dirección, latitud, longitud | 🔴 Crítica | Administrador | Parada se guarda en BD |
| **RF-04.2** | El sistema debe permitir seleccionar la ubicación en un mapa interactivo | 🟡 Alta | Administrador | Click en mapa rellena latitud/longitud automáticamente |
| **RF-04.3** | El administrador debe poder listar todas las paradas | 🔴 Crítica | Administrador | Tabla con paradas y acciones |
| **RF-04.4** | El administrador debe poder editar una parada | 🟡 Alta | Administrador | Cambios se reflejan en BD |
| **RF-04.5** | El administrador debe poder eliminar una parada (soft delete) | 🟡 Alta | Administrador | Parada no se muestra pero se mantiene en BD |
| **RF-04.6** | El sistema debe validar que latitud esté entre -90 y 90 | 🟡 Alta | Sistema | Error si latitud inválida |
| **RF-04.7** | El sistema debe validar que longitud esté entre -180 y 180 | 🟡 Alta | Sistema | Error si longitud inválida |

---

#### RF-05: Módulo de Cálculo de Rutas

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-05.1** | El administrador debe poder calcular una ruta automáticamente para un bus y turno específico | 🔴 Crítica | Administrador | Sistema ejecuta algoritmo TSP y genera ruta óptima |
| **RF-05.2** | El sistema debe calcular rutas diferenciadas para turno Mañana y Tarde | 🔴 Crítica | Sistema | Dos rutas independientes por bus |
| **RF-05.3** | El sistema debe asignar un orden secuencial a cada parada en la ruta | 🔴 Crítica | Sistema | Campo `Orden` en `ParadaRuta` |
| **RF-05.4** | El sistema debe calcular horario estimado para cada parada | 🟡 Alta | Sistema | Campo `HorarioEstimado` basado en distancia y velocidad promedio |
| **RF-05.5** | El sistema debe permitir al administrador editar manualmente el orden de paradas | 🟢 Media | Administrador | Drag-and-drop o campo numérico editable |
| **RF-05.6** | El sistema debe recalcular la ruta si se agregan o eliminan paradas | 🟢 Media | Sistema | Ruta se actualiza automáticamente |
| **RF-05.7** | El sistema debe mostrar la ruta calculada en un mapa | 🟡 Alta | Administrador | Mapa con línea que conecta paradas en orden |

---

#### RF-06: Módulo de Geolocalización en Tiempo Real

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-06.1** | El piloto debe poder enviar su ubicación GPS en tiempo real | 🔴 Crítica | Piloto | Ubicación se envía vía SignalR cada 10 segundos |
| **RF-06.2** | El padre de familia debe poder ver el bus en un mapa en tiempo real | 🔴 Crítica | Padre | Mapa muestra ícono del bus en posición actual |
| **RF-06.3** | El sistema debe mostrar la ruta completa con paradas en el mapa | 🟡 Alta | Padre, Piloto | Mapa con marcadores en cada parada |
| **RF-06.4** | El sistema debe actualizar la posición del bus sin recargar la página | 🔴 Crítica | Sistema | SignalR actualiza mapa dinámicamente |
| **RF-06.5** | El sistema debe mostrar mensaje si el bus no está transmitiendo ubicación | 🟡 Alta | Padre | Mensaje: "El bus no está en operación actualmente" |
| **RF-06.6** | El sistema debe registrar historial de ubicaciones (opcional) | 🟢 Baja | Sistema | Tabla `HistorialUbicaciones` (futuro) |

---

#### RF-07: Módulo de Pagos

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-07.1** | El padre debe poder registrar un pago con: monto, fecha, método, comprobante | 🔴 Crítica | Padre | Pago se guarda con estado "Pendiente" |
| **RF-07.2** | El padre debe poder subir una imagen del comprobante de pago | 🟡 Alta | Padre | Imagen se guarda en servidor o Azure Blob |
| **RF-07.3** | El padre debe poder ver su historial de pagos | 🔴 Crítica | Padre | Tabla con todos sus pagos y estados |
| **RF-07.4** | El administrador debe poder listar todos los pagos pendientes | 🔴 Crítica | Administrador | Tabla filtrada por estado "Pendiente" |
| **RF-07.5** | El administrador debe poder confirmar un pago | 🔴 Crítica | Administrador | Estado cambia a "Confirmado" |
| **RF-07.6** | El administrador debe poder rechazar un pago con motivo | 🟡 Alta | Administrador | Estado cambia a "Rechazado", se registra motivo |
| **RF-07.7** | El sistema debe permitir filtrar pagos por alumno, fecha, estado | 🟢 Media | Administrador | Filtros funcionales en listado |
| **RF-07.8** | El sistema debe calcular el total de ingresos en un rango de fechas | 🟢 Media | Administrador | Reporte con suma de pagos confirmados |

---

#### RF-08: Módulo de Confirmación de Asistencia

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-08.1** | El padre debe poder confirmar diariamente si su hijo asistirá | 🔴 Crítica | Padre | Registro en tabla `ConfirmacionAsistencia` |
| **RF-08.2** | El sistema debe mostrar el estado de confirmación para el día actual | 🟡 Alta | Padre | Indicador visual: "Confirmado" / "No confirmado" |
| **RF-08.3** | El piloto/monitor debe poder ver qué alumnos confirmaron asistencia | 🔴 Crítica | Piloto, Monitor | Lista con checkmarks de confirmaciones |
| **RF-08.4** | El sistema debe permitir cambiar la confirmación hasta 1 hora antes del inicio de ruta | 🟢 Media | Padre | Después de la hora límite, no permite cambios |
| **RF-08.5** | El sistema debe enviar recordatorio si el padre no ha confirmado (futuro) | 🟢 Baja | Sistema | Notificación o email automático |

---

#### RF-09: Módulo de Registro de Recogidas (Monitor)

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-09.1** | El monitor debe poder ver su ruta diaria con lista de alumnos por parada | 🔴 Crítica | Monitor | Página `RegistrarRecogidas` con ruta y alumnos |
| **RF-09.2** | El monitor debe poder marcar qué alumnos fueron recogidos en cada parada | 🔴 Crítica | Monitor | Checkboxes por alumno, botón "Guardar" |
| **RF-09.3** | El sistema debe registrar fecha/hora y ubicación GPS al guardar recogidas | 🟡 Alta | Sistema | Campos automáticos en `RegistroRecogida` |
| **RF-09.4** | El sistema debe prevenir registros duplicados del mismo alumno en el mismo día | 🔴 Crítica | Sistema | Validación: error si ya existe registro |
| **RF-09.5** | El monitor debe poder ver qué alumnos ya fueron registrados | 🟡 Alta | Monitor | Checkboxes pre-marcados si ya hay registro |
| **RF-09.6** | El administrador debe poder ver reportes de recogidas por fecha y bus | 🟡 Alta | Administrador | Reporte con filtros |

---

#### RF-10: Dashboard y Reportes

| ID | Requerimiento | Prioridad | Actor | Criterio de Aceptación |
|----|---------------|-----------|-------|------------------------|
| **RF-10.1** | El administrador debe ver un dashboard con estadísticas generales | 🟡 Alta | Administrador | Tarjetas con: total de buses, alumnos, pagos pendientes, etc. |
| **RF-10.2** | El administrador debe poder generar reporte de pagos por rango de fechas | 🟡 Alta | Administrador | PDF o Excel con detalle de pagos |
| **RF-10.3** | El administrador debe poder generar reporte de asistencia por alumno | 🟢 Media | Administrador | Reporte con % de asistencia |
| **RF-10.4** | El administrador debe poder generar reporte de recogidas por bus | 🟢 Media | Administrador | Reporte con detalle de recogidas diarias |
| **RF-10.5** | El sistema debe permitir exportar reportes a Excel | 🟢 Media | Administrador | Botón "Exportar a Excel" funcional |

---

### 1.3.2 Matriz de Trazabilidad (Requerimientos ↔ Casos de Uso)

| Caso de Uso | Requerimientos Relacionados | Prioridad | Complejidad |
|-------------|----------------------------|-----------|-------------|
| **CU-01: Login** | RF-01.1, RF-01.2, RF-01.5, RF-01.6 | 🔴 Crítica | 🟢 Baja |
| **CU-02: Cambiar contraseña** | RF-01.3 | 🟡 Alta | 🟢 Baja |
| **CU-03: Crear bus** | RF-02.1, RF-02.5 | 🔴 Crítica | 🟢 Baja |
| **CU-04: Asignar piloto a bus** | RF-02.6 | 🔴 Crítica | 🟡 Media |
| **CU-05: Crear alumno** | RF-03.1, RF-03.7 | 🔴 Crítica | 🟢 Baja |
| **CU-06: Asignar alumno a parada** | RF-03.3 | 🔴 Crítica | 🟡 Media |
| **CU-07: Crear parada** | RF-04.1, RF-04.6, RF-04.7 | 🔴 Crítica | 🟡 Media |
| **CU-08: Seleccionar ubicación en mapa** | RF-04.2 | 🟡 Alta | 🟡 Media |
| **CU-09: Calcular ruta automáticamente** | RF-05.1, RF-05.2, RF-05.3, RF-05.4 | 🔴 Crítica | 🔴 Alta |
| **CU-10: Ver ruta en mapa** | RF-05.7 | 🟡 Alta | 🟡 Media |
| **CU-11: Enviar ubicación GPS (piloto)** | RF-06.1 | 🔴 Crítica | 🔴 Alta |
| **CU-12: Ver bus en tiempo real (padre)** | RF-06.2, RF-06.3, RF-06.4 | 🔴 Crítica | 🔴 Alta |
| **CU-13: Registrar pago** | RF-07.1, RF-07.2 | 🔴 Crítica | 🟡 Media |
| **CU-14: Confirmar pago** | RF-07.5 | 🔴 Crítica | 🟢 Baja |
| **CU-15: Ver historial de pagos** | RF-07.3 | 🔴 Crítica | 🟢 Baja |
| **CU-16: Confirmar asistencia** | RF-08.1, RF-08.2 | 🔴 Crítica | 🟢 Baja |
| **CU-17: Registrar recogidas** | RF-09.1, RF-09.2, RF-09.3, RF-09.4 | 🔴 Crítica | 🟡 Media |
| **CU-18: Ver dashboard** | RF-10.1 | 🟡 Alta | 🟡 Media |
| **CU-19: Generar reporte de pagos** | RF-10.2 | 🟡 Alta | 🟡 Media |

---

✅ **1.3 ANÁLISIS DE REQUERIMIENTOS FUNCIONALES - COMPLETADO**

---

## 1.4 Análisis de Requerimientos No Funcionales

### 1.4.1 Requerimientos de Rendimiento (Performance)

| ID | Requerimiento | Métrica | Prioridad | Justificación |
|----|---------------|---------|-----------|---------------|
| **RNF-01.1** | El tiempo de carga de la página principal no debe exceder 2 segundos | < 2s | 🟡 Alta | Experiencia de usuario fluida |
| **RNF-01.2** | El cálculo de una ruta con hasta 30 paradas no debe tomar más de 5 segundos | < 5s | 🟡 Alta | Evitar frustración del administrador |
| **RNF-01.3** | El sistema debe soportar hasta 100 usuarios concurrentes sin degradación | 100 usuarios | 🟡 Alta | Empresas medianas con muchos padres |
| **RNF-01.4** | La actualización de ubicación GPS debe ocurrir cada 10 segundos máximo | ≤ 10s | 🔴 Crítica | Geolocalización en "tiempo real" |
| **RNF-01.5** | Las consultas a base de datos deben responder en menos de 500ms | < 500ms | 🟡 Alta | Fluidez en operaciones CRUD |
| **RNF-01.6** | El sistema debe soportar hasta 50 buses transmitiendo ubicación simultáneamente | 50 buses | 🟢 Media | Escalabilidad para empresas grandes |

---

### 1.4.2 Requerimientos de Usabilidad

| ID | Requerimiento | Criterio de Aceptación | Prioridad | Target |
|----|---------------|------------------------|-----------|--------|
| **RNF-02.1** | La interfaz debe ser responsive y funcionar en móviles, tablets y desktops | Bootstrap 5 responsive | 🔴 Crítica | Padres usan móviles |
| **RNF-02.2** | El sistema debe ser intuitivo: un usuario promedio debe poder usarlo sin capacitación extensa | Usuario completa tarea en < 5 min | 🟡 Alta | Pilotos/monitores no son expertos en tecnología |
| **RNF-02.3** | Los mensajes de error deben ser claros y en español | Mensajes descriptivos (no códigos técnicos) | 🟡 Alta | Evitar confusión |
| **RNF-02.4** | Los botones y enlaces deben tener tamaños adecuados para touch (mínimo 44x44px) | Guías de accesibilidad móvil | 🟡 Alta | Usabilidad en tablets para monitores |
| **RNF-02.5** | El sistema debe mostrar loaders/spinners mientras procesa operaciones largas | Indicador visual de carga | 🟡 Alta | Feedback al usuario |
| **RNF-02.6** | Los formularios deben tener validaciones en tiempo real | Validaciones client-side con JavaScript | 🟢 Media | Mejor UX |

---

### 1.4.3 Requerimientos de Seguridad

| ID | Requerimiento | Método de Implementación | Prioridad | Riesgo Mitigado |
|----|---------------|-------------------------|-----------|-----------------|
| **RNF-03.1** | Todas las contraseñas deben ser hasheadas (nunca plain text) | ASP.NET Core Identity (bcrypt/PBKDF2) | 🔴 Crítica | Robo de credenciales |
| **RNF-03.2** | Las conexiones deben ser HTTPS (certificado SSL) | SSL/TLS en Azure | 🔴 Crítica | Interceptación de datos (MITM) |
| **RNF-03.3** | Las sesiones deben expirar después de 30 minutos de inactividad | Configuración de cookies de sesión | 🟡 Alta | Sesiones abiertas en computadoras compartidas |
| **RNF-03.4** | Los archivos subidos (comprobantes) deben validarse (tipo, tamaño) | Validación: JPEG/PNG, máx 5MB | 🟡 Alta | Subida de archivos maliciosos |
| **RNF-03.5** | El sistema debe prevenir SQL Injection | Entity Framework Core (ORM) | 🔴 Crítica | Ataques de inyección |
| **RNF-03.6** | El sistema debe prevenir XSS (Cross-Site Scripting) | Razor Pages sanitiza inputs automáticamente | 🔴 Crítica | Inyección de scripts maliciosos |
| **RNF-03.7** | El acceso a funcionalidades debe estar restringido por rol | `[Authorize(Roles = "...")]` en cada página | 🔴 Crítica | Acceso no autorizado |
| **RNF-03.8** | Los tokens de API (Google Maps) deben estar en variables de entorno, no hardcodeados | Azure Key Vault o `appsettings.json` no versionado | 🟡 Alta | Exposición de secretos |

---

### 1.4.4 Requerimientos de Disponibilidad (Availability)

| ID | Requerimiento | Métrica | Prioridad | SLA |
|----|---------------|---------|-----------|-----|
| **RNF-04.1** | El sistema debe estar disponible 99.5% del tiempo mensual | Uptime ≥ 99.5% | 🟡 Alta | ~3.6 horas de downtime permitido/mes |
| **RNF-04.2** | Las actualizaciones de mantenimiento deben hacerse en horarios de baja actividad | Domingos 2-5 AM | 🟢 Media | Minimizar impacto en usuarios |
| **RNF-04.3** | El sistema debe tener backups automáticos diarios de la base de datos | Backup diario a las 3 AM | 🔴 Crítica | Recuperación ante desastres |
| **RNF-04.4** | Los backups deben retenerse por al menos 30 días | Retention: 30 días | 🟡 Alta | Recuperación de datos históricos |
| **RNF-04.5** | En caso de caída, el sistema debe recuperarse en menos de 15 minutos | RTO < 15 min | 🟢 Media | Minimizar interrupción del servicio |

---

### 1.4.5 Requerimientos de Escalabilidad

| ID | Requerimiento | Criterio | Prioridad | Justificación |
|----|---------------|----------|-----------|---------------|
| **RNF-05.1** | El sistema debe soportar crecimiento de hasta 500 alumnos por empresa | 500 alumnos | 🟡 Alta | Empresas grandes |
| **RNF-05.2** | La base de datos debe poder almacenar al menos 1 año de historial de ubicaciones | 365 días de registros GPS | 🟢 Media | Análisis histórico |
| **RNF-05.3** | La arquitectura debe permitir agregar nuevos módulos sin refactoring mayor | Arquitectura modular (áreas Razor Pages) | 🟡 Alta | Extensibilidad futura |
| **RNF-05.4** | El sistema debe permitir multi-tenancy (múltiples empresas en el futuro) | Diseño con `IdEmpresa` en tablas | 🟢 Baja (v2.0) | Escalabilidad del negocio |

---

### 1.4.6 Requerimientos de Mantenibilidad

| ID | Requerimiento | Método | Prioridad | Beneficio |
|----|---------------|--------|-----------|-----------|
| **RNF-06.1** | El código debe seguir convenciones de C# y Razor Pages | Estándares de Microsoft | 🟡 Alta | Legibilidad y mantenimiento |
| **RNF-06.2** | Las funciones complejas deben estar documentadas con comentarios XML | `/// <summary>` en métodos públicos | 🟢 Media | Facilitar onboarding de nuevos devs |
| **RNF-06.3** | El sistema debe tener logging de errores centralizado | Serilog o Azure Application Insights | 🟡 Alta | Debugging y monitoreo |
| **RNF-06.4** | Los cambios en base de datos deben gestionarse con migraciones de EF Core | `dotnet ef migrations add` | 🔴 Crítica | Control de versiones de BD |
| **RNF-06.5** | El sistema debe tener unit tests para la lógica crítica (TSP, validaciones) | xUnit, mínimo 60% code coverage | 🟢 Media | Prevenir regresiones |

---

### 1.4.7 Requerimientos de Compatibilidad

| ID | Requerimiento | Detalle | Prioridad |
|----|---------------|---------|-----------|
| **RNF-07.1** | El sistema debe funcionar en navegadores modernos: Chrome, Firefox, Edge, Safari | Últimas 2 versiones de cada navegador | 🔴 Crítica |
| **RNF-07.2** | El sistema debe funcionar en dispositivos Android e iOS (vía navegador) | Responsive web, no app nativa | 🔴 Crítica |
| **RNF-07.3** | El sistema debe ser compatible con resoluciones desde 320px (móviles) hasta 1920px (desktops) | Bootstrap breakpoints | 🟡 Alta |
| **RNF-07.4** | El sistema debe funcionar con conexiones de internet moderadas (3G mínimo) | Optimización de recursos, caching | 🟢 Media |

---

### 1.4.8 Requerimientos de Cumplimiento Legal

| ID | Requerimiento | Normativa | Prioridad | Acción Requerida |
|----|---------------|-----------|-----------|------------------|
| **RNF-08.1** | El sistema debe cumplir con leyes de protección de datos personales (si aplican en Honduras) | GDPR (referencia), ley local | 🟡 Alta | Política de privacidad, consentimiento de padres |
| **RNF-08.2** | Los datos de menores (alumnos) deben manejarse con protección especial | COPPA (referencia) | 🟡 Alta | No recopilar datos innecesarios de alumnos |
| **RNF-08.3** | El sistema debe permitir a los usuarios exportar o eliminar sus datos (derecho al olvido) | GDPR Art. 17 | 🟢 Media | Funcionalidad "Exportar mis datos" / "Eliminar mi cuenta" |
| **RNF-08.4** | El sistema debe tener términos y condiciones y política de privacidad | Legal | 🟡 Alta | Páginas estáticas con documentos legales |

---

✅ **1.4 ANÁLISIS DE REQUERIMIENTOS NO FUNCIONALES - COMPLETADO**

---

## 1.5 Análisis de Dominio

### 1.5.1 Modelo Conceptual del Negocio

**[INSTRUCCIONES PARA DIAGRAMA DE DOMINIO]**

```
Título: Modelo de Dominio - Transportes Genesis

Entidades Principales y Relaciones:

1. **Empresa** (entidad raíz)
   - Atributos: RUC, NombreComercial, Teléfono, Dirección
   - Relaciones:
     * Tiene muchos Buses (1:N)
     * Tiene muchos Usuarios (1:N)
     * Tiene muchos Alumnos (1:N)

2. **Bus**
   - Atributos: Placa (único), Modelo, Capacidad, AñoFabricación
   - Relaciones:
     * Pertenece a una Empresa (N:1)
     * Tiene AsignacionesPilotoBus (1:N)
     * Tiene Rutas (1:N por turno)
     * Transmite Ubicaciones (1:N)

3. **Usuario** (AppUser)
   - Atributos: Email, Nombre, Apellido, Rol (Administrador, Piloto, Monitor, PadreDeFamilia)
   - Relaciones:
     * Si es Piloto/Monitor → Tiene AsignacionesPilotoBus (1:N)
     * Si es Padre → Tiene Alumnos (1:N)

4. **Alumno**
   - Atributos: Nombre, Apellido, FechaNacimiento, Grado
   - Relaciones:
     * Pertenece a Padres de Familia (N:N) vía AlumnoPadreFamilia
     * Asignado a Paradas (N:N) vía AsignacionAlumnoParada
     * Tiene ConfirmacionesAsistencia (1:N)
     * Tiene RegistrosRecogida (1:N)
     * Tiene Pagos asociados (1:N)

5. **Parada**
   - Atributos: Nombre, Dirección, Latitud, Longitud
   - Relaciones:
     * Pertenece a Rutas (N:N) vía ParadaRuta
     * Tiene Alumnos asignados (N:N) vía AsignacionAlumnoParada
     * Tiene RegistrosRecogida (1:N)

6. **Ruta**
   - Atributos: FechaInicio, FechaFin, Turno (Mañana/Tarde), EsActiva
   - Relaciones:
     * Asignada a un Bus (N:1)
     * Tiene Paradas (N:N) vía ParadaRuta con atributo Orden

7. **ParadaRuta** (tabla intermedia con datos)
   - Atributos: Orden, HorarioEstimado, DistanciaKm
   - Relaciona: Ruta ↔ Parada

8. **AsignacionPilotoBus** (tabla de relación histórica)
   - Atributos: FechaAsignacion, FechaFinAsignacion, EsActual
   - Relaciona: Usuario (Piloto/Monitor) ↔ Bus

9. **AsignacionAlumnoParada** (tabla de relación histórica)
   - Atributos: FechaAsignacion, FechaFinAsignacion, EsActual
   - Relaciona: Alumno ↔ Parada

10. **ConfirmacionAsistencia**
    - Atributos: Fecha, AsistiraManana, AsistiraTarde
    - Relaciona: Alumno + Fecha (confirmación diaria)

11. **RegistroRecogida**
    - Atributos: FechaHoraRecogida, AlumnoPresente, Latitud, Longitud, ConfirmadoPor
    - Relaciona: Alumno + Parada + Fecha + Usuario (Monitor)

12. **Pago**
    - Atributos: Monto, FechaPago, MetodoPago, Estado, Comprobante
    - Relaciona: Alumno + Usuario (Padre) + Usuario (Administrador que confirma)

13. **Ubicacion** (transmisión GPS en tiempo real)
    - Atributos: Latitud, Longitud, FechaHora
    - Relaciona: Bus (transmisión continua vía SignalR)

---

Conceptos de Dominio (Glosario):

- **Turno**: Periodo del día (Mañana o Tarde) en que opera una ruta
- **Ruta Activa**: Ruta vigente y en operación
- **Orden de Parada**: Secuencia numérica en que se visitan las paradas en una ruta
- **TSP (Traveling Salesman Problem)**: Algoritmo que calcula la ruta más corta visitando todas las paradas
- **Geolocalización**: Ubicación geográfica expresada en latitud y longitud
- **SignalR**: Tecnología de comunicación en tiempo real (WebSockets)
- **Rol**: Perfil de usuario con permisos específicos
- **Soft Delete**: Eliminación lógica (registro marcado como eliminado pero no borrado físicamente)
```

---

### 1.5.2 Reglas de Negocio

| ID | Regla de Negocio | Justificación | Implementación |
|----|------------------|---------------|----------------|
| **RN-01** | Un bus solo puede tener un piloto activo a la vez | Evitar confusión sobre quién opera el bus | Campo `EsActual` en `AsignacionPilotoBus` |
| **RN-02** | Un alumno solo puede estar asignado a una parada activa a la vez | Un alumno solo puede ser recogido en una ubicación por turno | Campo `EsActual` en `AsignacionAlumnoParada` |
| **RN-03** | Un bus solo puede tener una ruta activa por turno (Mañana y Tarde) | Evitar ambigüedad sobre qué ruta seguir | Campo `Turno` + `EsActiva` en `Ruta` |
| **RN-04** | Una confirmación de asistencia solo es válida para el día actual | Confirmaciones son diarias y no se arrastran | Campo `Fecha` en `ConfirmacionAsistencia` |
| **RN-05** | No se puede registrar la misma recogida dos veces (mismo alumno, misma parada, mismo día) | Evitar duplicados | Validación en controller/service |
| **RN-06** | Un pago en estado "Pendiente" puede ser editado por el padre; uno "Confirmado" no | Integridad de registros contables | Validación de estado antes de editar |
| **RN-07** | La fecha de nacimiento de un alumno debe estar en el rango de 3-18 años | Alumnos de transporte escolar son niños/adolescentes | Validación en modelo |
| **RN-08** | La placa de un bus debe ser única en el sistema | Identificación inequívoca de buses | Índice único en BD + validación |
| **RN-09** | Un usuario con rol "Piloto" o "Monitor" debe estar asignado a un bus para acceder a su ruta | Sin asignación, no hay ruta que mostrar | Validación en página `MiRuta.cshtml.cs` |
| **RN-10** | El cálculo de ruta debe considerar solo paradas con alumnos asignados | No tiene sentido visitar paradas vacías | Filtro en servicio de cálculo de rutas |
| **RN-11** | Los horarios estimados de paradas se calculan asumiendo velocidad promedio de 30 km/h | Estimación realista para zonas urbanas | Fórmula: distancia / velocidad |
| **RN-12** | Un padre solo puede ver información de sus propios hijos | Privacidad y seguridad | Filtro por `IdUsuarioPadre` en consultas |
| **RN-13** | Al eliminar un bus (soft delete), las rutas asociadas se marcan como inactivas | Consistencia de datos | Lógica en servicio de eliminación |
| **RN-14** | La ubicación GPS debe actualizarse máximo cada 10 segundos para considerarse "tiempo real" | Balance entre precisión y consumo de datos | Timer en cliente SignalR |
| **RN-15** | Solo el administrador puede confirmar o rechazar pagos | Control financiero centralizado | `[Authorize(Roles = "Administrador")]` |

---

### 1.5.3 Procesos de Negocio Clave

#### Proceso 1: Onboarding de Nuevo Alumno

```
[Administrador]
   ↓
1. Crea usuario "Padre de Familia" (email + contraseña temporal)
   ↓
2. Crea registro de Alumno (nombre, apellido, fecha nacimiento, grado)
   ↓
3. Vincula Alumno con Padre (tabla AlumnoPadreFamilia)
   ↓
4. Asigna Alumno a una Parada existente (tabla AsignacionAlumnoParada, EsActual = true)
   ↓
5. Recalcula rutas afectadas (servicio TSP)
   ↓
6. Notifica a Padre (por correo o WhatsApp) con credenciales de acceso
   ↓
[Padre recibe email, accede al sistema, cambia contraseña obligatoriamente]
```

---

#### Proceso 2: Operación Diaria de Ruta (Turno Mañana)

```
**Día Anterior (Noche):**
[Padre de Familia]
   ↓
Confirma asistencia de su hijo para mañana (página "Confirmar Asistencia")
   ↓
Sistema registra en `ConfirmacionAsistencia` (Fecha = mañana, AsistiraManana = true)

---

**Día Actual (Mañana):**

[Piloto]
   ↓
1. Inicia sesión, ve su ruta diaria en `MiRuta`
   ↓
2. Revisa lista de alumnos confirmados
   ↓
3. Inicia transmisión de ubicación GPS (activa botón "Iniciar Ruta")
   ↓
4. SignalR envía ubicación cada 10 segundos

[Monitor] (en paralelo)
   ↓
1. Abre página `RegistrarRecogidas` en tablet
   ↓
2. Ve ruta con paradas y alumnos
   ↓
3. En cada parada:
   - Marca checkboxes de alumnos que subieron al bus
   - Presiona "Guardar"
   - Sistema registra en `RegistroRecogida` (FechaHora, Lat, Lon automáticos)
   ↓
4. Continúa hasta completar todas las paradas

[Padre de Familia] (en paralelo)
   ↓
1. Abre el sistema desde su móvil
   ↓
2. Ve mapa con bus en tiempo real
   ↓
3. Observa cuándo el bus está cerca de su parada
   ↓
4. Confirma que su hijo fue recogido (ve checkbox marcado en su panel)

[Administrador]
   ↓
Al final del día, revisa dashboard:
   - Alumnos confirmados vs recogidos
   - Rutas completadas
   - Incidencias (alumnos confirmados pero no recogidos)
```

---

#### Proceso 3: Ciclo de Pago Mensual

```
**Inicio de Mes:**

[Administrador]
   ↓
Genera factura/recibo mensual para cada alumno (puede ser manual o futura feature)
   ↓
Envía por email o WhatsApp a padres

---

**Durante el Mes:**

[Padre de Familia]
   ↓
1. Hace depósito bancario o transferencia
   ↓
2. Accede al sistema, sección "Mis Pagos"
   ↓
3. Clic en "Registrar Pago"
   ↓
4. Completa formulario:
   - Alumno
   - Monto
   - Fecha de pago
   - Método (Transferencia/Efectivo/Tarjeta)
   - Sube imagen del comprobante
   ↓
5. Presiona "Guardar"
   ↓
Sistema guarda pago con Estado = "Pendiente"

---

**Validación:**

[Administrador]
   ↓
1. Ve listado de "Pagos Pendientes"
   ↓
2. Revisa comprobante subido
   ↓
3. Valida contra registros bancarios
   ↓
4. Opciones:
   a) Confirmar pago → Estado = "Confirmado"
   b) Rechazar pago → Estado = "Rechazado", ingresa motivo ("Monto incorrecto", "Comprobante ilegible", etc.)
   ↓
Sistema actualiza estado

---

**Consulta:**

[Padre de Familia]
   ↓
Entra a "Historial de Pagos"
   ↓
Ve tabla con todos sus pagos:
   - Fecha
   - Monto
   - Estado (Pendiente ⏳ / Confirmado ✅ / Rechazado ❌)
   - Motivo (si fue rechazado)
```

---

### 1.5.4 Eventos de Dominio

| Evento | Desencadenante | Efecto en el Sistema | Actores Notificados |
|--------|----------------|---------------------|---------------------|
| **AlumnoAsignadoAParada** | Administrador asigna alumno a parada | Recalculo automático de ruta del bus correspondiente | Piloto, Monitor (ven ruta actualizada) |
| **RutaCalculada** | Administrador ejecuta "Calcular Ruta" | Se genera `Ruta` con `ParadaRuta` ordenadas | Piloto, Monitor |
| **UbicacionActualizada** | Piloto envía GPS cada 10s | Mapa de padres se actualiza en tiempo real | Padres de alumnos en esa ruta |
| **AsistenciaConfirmada** | Padre confirma asistencia | Piloto/Monitor ven lista actualizada de confirmaciones | Piloto, Monitor |
| **RecogidaRegistrada** | Monitor marca alumno como recogido | Padre ve notificación visual de que su hijo subió al bus | Padre de ese alumno |
| **PagoRegistrado** | Padre sube pago | Aparece en lista de pagos pendientes del administrador | Administrador |
| **PagoConfirmado** | Administrador confirma pago | Estado del pago cambia a "Confirmado" | Padre ve confirmación en su historial |
| **PilotoAsignadoABus** | Administrador asigna piloto | Piloto puede acceder a su ruta | Piloto |

---

✅ **1.5 ANÁLISIS DE DOMINIO - COMPLETADO**

---

## 1.6 Supuestos, restricciones y dependencias funcionales

### 1.6.1 Supuestos del Proyecto

| ID | Supuesto | Implicación | Riesgo si es Falso |
|----|----------|-------------|--------------------|
| **SUP-01** | Los usuarios (pilotos, monitores, padres) tienen acceso a smartphones o computadoras con navegador web | El sistema es 100% web; no hay app nativa | 🟡 Medio: Si muchos usuarios no tienen dispositivos, adopción será baja |
| **SUP-02** | Los usuarios tienen conexión a internet (3G mínimo) durante la operación | Geolocalización y actualización en tiempo real dependen de conectividad | 🔴 Alto: Sin internet, features críticas no funcionan |
| **SUP-03** | Los navegadores de los usuarios son modernos (últimas 2 versiones de Chrome, Firefox, Edge, Safari) | Se usan features modernas de HTML5, CSS3, JavaScript | 🟢 Bajo: Navegadores antiguos son minoría |
| **SUP-04** | La empresa de transporte ya tiene definidas sus paradas y rutas operativas | El sistema digitaliza rutas existentes | 🟡 Medio: Si no hay rutas establecidas, requiere trabajo previo de campo |
| **SUP-05** | Los pilotos y monitores tienen dispositivos (tablets o móviles) para usar durante la operación | Registro de recogidas y envío de GPS ocurren en movimiento | 🟡 Medio: Si no tienen dispositivos, la empresa debe proveerlos |
| **SUP-06** | Los padres están dispuestos a usar una plataforma digital | Cambio cultural de WhatsApp/llamadas a sistema web | 🟡 Medio: Resistencia al cambio puede frenar adopción |
| **SUP-07** | Google Maps API está disponible y accesible en Honduras | Geolocalización y mapas dependen de Google | 🟢 Bajo: Google Maps funciona globalmente |
| **SUP-08** | La empresa tiene capacidad de invertir en infraestructura cloud (Azure) | Costos operativos de $150-$300/mes | 🟡 Medio: Si no puede pagar, el sistema no puede operarse |
| **SUP-09** | Los datos de ubicación GPS de los dispositivos son precisos (margen de error ≤ 10 metros) | Mapas en tiempo real muestran posición real del bus | 🟢 Bajo: GPS moderno es generalmente preciso |
| **SUP-10** | La empresa tiene personal (administrador) con conocimientos básicos de computación | Gestión del sistema requiere uso de interfaces CRUD | 🟡 Medio: Si el administrador no sabe usar computadoras, requiere capacitación intensiva |

---

### 1.6.2 Restricciones del Proyecto

#### Restricciones Técnicas

| ID | Restricción | Tipo | Impacto |
|----|-------------|------|---------|
| **RES-TEC-01** | El sistema DEBE desarrollarse en **.NET 8** con **Razor Pages** | Tecnología | No se puede usar Blazor, Angular, React, etc. |
| **RES-TEC-02** | La base de datos DEBE ser **SQL Server** | Infraestructura | No se puede usar MySQL, PostgreSQL, MongoDB, etc. |
| **RES-TEC-03** | El sistema DEBE desplegarse en **Microsoft Azure** | Infraestructura | No se puede usar AWS, Google Cloud, DigitalOcean, etc. |
| **RES-TEC-04** | La geolocalización DEBE usar **Google Maps API** | API Externa | No se puede usar OpenStreetMap, Mapbox (al menos en v1.0) |
| **RES-TEC-05** | El tiempo real DEBE implementarse con **SignalR** | Protocolo | No se puede usar polling, long-polling, otros frameworks WebSocket |
| **RES-TEC-06** | El sistema DEBE ser una aplicación web (NO app móvil nativa) | Plataforma | No hay versiones iOS/Android nativas en v1.0 |

---

#### Restricciones de Presupuesto

| ID | Restricción | Límite | Impacto |
|----|-------------|--------|---------|
| **RES-PRE-01** | Inversión inicial máxima: **$25,000 USD** | Desarrollo + capital de trabajo | No se pueden contratar más de 2-3 desarrolladores o extender timeline excesivamente |
| **RES-PRE-02** | Costos operativos mensuales máximos: **$300 USD** | Azure + Google Maps + otros | Debe optimizarse el uso de recursos cloud |
| **RES-PRE-03** | Precio de venta por cliente: **$50/bus/mes** | Modelo de negocio | No se puede cobrar más sin justificación de valor |

---

#### Restricciones de Tiempo

| ID | Restricción | Plazo | Impacto |
|----|-------------|-------|---------|
| **RES-TIE-01** | El MVP debe estar listo en **3 meses (12 semanas)** | 537 horas de desarrollo | Features no críticas deben diferirse a v2.0 |
| **RES-TIE-02** | El sistema debe estar operativo para el **inicio del ciclo escolar** | Típicamente Febrero en Honduras | Deadline fijo; retrasos pueden perder oportunidad de mercado |

---

#### Restricciones Legales y de Cumplimiento

| ID | Restricción | Normativa | Impacto |
|----|-------------|-----------|---------|
| **RES-LEG-01** | El sistema debe cumplir con leyes de protección de datos personales | Ley hondureña (si existe) o GDPR como referencia | Debe incluirse política de privacidad y consentimiento |
| **RES-LEG-02** | Los datos de menores (alumnos) deben manejarse con protección especial | COPPA (referencia) | No recopilar datos sensibles innecesarios |
| **RES-LEG-03** | El sistema debe tener términos y condiciones | Legal | Página estática obligatoria |

---

### 1.6.3 Dependencias Funcionales

#### Dependencias Externas (Third-Party)

| Dependencia | Proveedor | Criticidad | Función | Riesgo de Indisponibilidad | Mitigación |
|-------------|-----------|------------|---------|----------------------------|------------|
| **Google Maps API** | Google | 🔴 Crítica | Geolocalización, mapas, cálculo de distancias | 🟢 Baja (99.9% uptime) | Cache de mapas estáticos, fallback a OpenStreetMap en v2.0 |
| **Microsoft Azure** | Microsoft | 🔴 Crítica | Hosting, base de datos, storage | 🟢 Baja (SLA 99.95%) | Backups regulares, plan de recuperación ante desastres |
| **SignalR** | Microsoft | 🔴 Crítica | Tiempo real (WebSockets) | 🟢 Baja (parte de .NET) | Fallback a long-polling si WebSockets falla |
| **Bootstrap 5** | Open Source (CDN) | 🟡 Alta | UI responsive | 🟢 Baja (múltiples CDNs) | Servir Bootstrap desde servidor propio si CDN falla |
| **jQuery** | Open Source (CDN) | 🟡 Alta | Interactividad frontend | 🟢 Baja | Servir desde servidor propio |

---

#### Dependencias Internas (Entre Módulos)

| Módulo Dependiente | Módulo del que Depende | Tipo de Dependencia | Impacto si el Módulo Base Falla |
|--------------------|------------------------|---------------------|--------------------------------|
| **Cálculo de Rutas** | Gestión de Paradas + Gestión de Alumnos | Datos | ❌ No se puede calcular ruta sin paradas y alumnos |
| **Geolocalización en Tiempo Real** | Gestión de Buses + Gestión de Rutas | Datos | ❌ No se puede mostrar bus en mapa sin bus y ruta asignados |
| **Registro de Recogidas** | Gestión de Rutas + Gestión de Alumnos | Datos | ❌ Monitor no puede registrar recogidas sin ruta y alumnos |
| **Confirmación de Asistencia** | Gestión de Alumnos + Auth (Padres) | Datos + Auth | ❌ Padre no puede confirmar sin alumno vinculado |
| **Gestión de Pagos** | Gestión de Alumnos + Auth (Padres, Administrador) | Datos + Auth | ❌ No se puede registrar pago sin alumno y usuarios |
| **Dashboard de Administrador** | Todos los módulos | Datos agregados | ⚠️ Dashboard incompleto si algún módulo no tiene datos, pero sistema sigue operativo |

---

#### Dependencias de Datos (Secuencia de Implementación)

**Orden obligatorio de configuración inicial:**

```
1. Crear Empresa
   ↓
2. Crear Usuarios (Administrador, Pilotos, Monitores, Padres)
   ↓
3. Crear Buses
   ↓
4. Asignar Pilotos a Buses (AsignacionPilotoBus)
   ↓
5. Crear Paradas (con geolocalización)
   ↓
6. Crear Alumnos
   ↓
7. Vincular Alumnos con Padres (AlumnoPadreFamilia)
   ↓
8. Asignar Alumnos a Paradas (AsignacionAlumnoParada)
   ↓
9. Calcular Rutas (TSP automático)
   ↓
10. Iniciar Operación Diaria (confirmaciones, registro de recogidas, geolocalización)
```

**Implicación:** No se puede "saltar" pasos; cada módulo depende del anterior.

---

### 1.6.4 Riesgos Funcionales

| ID | Riesgo | Probabilidad | Impacto | Mitigación |
|----|--------|--------------|---------|------------|
| **RF-RIESGO-01** | El algoritmo TSP no optimiza rutas adecuadamente para casos complejos (50+ paradas) | 🟡 Media | 🟡 Medio | Implementar versión simplificada (greedy algorithm) + permitir ajuste manual |
| **RF-RIESGO-02** | Los pilotos olvidan activar la transmisión de ubicación GPS | 🟡 Media | 🔴 Alto | Mostrar alerta prominente en su dashboard + notificación si no transmite en horario de ruta |
| **RF-RIESGO-03** | Los monitores registran recogidas incorrectamente (marcan alumnos que no subieron) | 🟡 Media | 🟡 Medio | Validación cruzada con confirmaciones de asistencia + revisión de administrador |
| **RF-RIESGO-04** | Los padres no confirman asistencia (sistema asume que todos asistirán) | 🔴 Alta | 🟢 Bajo | Configuración: "Por defecto, todos asisten si no se confirma lo contrario" |
| **RF-RIESGO-05** | La conexión a internet del bus es intermitente, ubicación GPS se actualiza con retraso | 🟡 Media | 🟡 Medio | Buffer de ubicaciones en cliente, enviar cuando reconecta + mostrar "última ubicación conocida" en mapa |
| **RF-RIESGO-06** | Los padres suben comprobantes de pago falsos o editados | 🟢 Baja | 🔴 Alto | Validación manual por administrador + verificación cruzada con extractos bancarios |
| **RF-RIESGO-07** | El sistema se usa para un solo bus (no escala a múltiples buses) | 🟢 Baja | 🟢 Bajo | Arquitectura ya soporta múltiples buses; solo es cuestión de configuración |

---

✅ **1.6 SUPUESTOS, RESTRICCIONES Y DEPENDENCIAS - COMPLETADO**

---

## 🎉 ESPECIFICACIÓN FUNCIONAL COMPLETADA AL 100%

### Resumen Final del Documento

✅ **1.1** Introducción y alcance del sistema (problema, objetivos, beneficios, alcance in/out)  
✅ **1.2** Contexto organizacional y stakeholders (estructura, stakeholders, flujos actual vs propuesto, mapa de empatía)  
✅ **1.3** Análisis de requerimientos funcionales (RF por módulo, matriz de trazabilidad)  
✅ **1.4** Análisis de requerimientos no funcionales (rendimiento, usabilidad, seguridad, disponibilidad, escalabilidad, compatibilidad, legal)  
✅ **1.5** Análisis de dominio (modelo conceptual, reglas de negocio, procesos clave, eventos)  
✅ **1.6** Supuestos, restricciones y dependencias (supuestos, restricciones técnicas/presupuesto/tiempo/legales, dependencias externas/internas, riesgos)

---

**FIN DEL DOCUMENTO: 1_Fase_inicial_ESPECIFICACION_FUNCIONAL_v1.md**
   - No hay integración con software contable (SAP, QuickBooks, etc.)

7. **Generación avanzada de reportes** (BI)
   - No hay integración con Power BI o Tableau
   - Reportes básicos incluidos en HTML

8. **Multi-tenancy** (múltiples empresas en una sola instancia)
   - El sistema está diseñado para una sola empresa
   - Escalable a multi-tenancy en versiones futuras

9. **Gestión de mantenimiento de buses**
   - No hay módulo de mantenimiento preventivo/correctivo

10. **Sistema de facturación electrónica**
    - No genera facturas electrónicas (solo recibos simples)

### 1.1.5 Usuarios Finales del Sistema

El sistema está diseñado para cuatro tipos principales de usuarios:

1. **Administradores** (Gerentes/Dueños de la empresa)
   - Gestionan usuarios, buses, rutas, alumnos
   - Calculan rutas optimizadas
   - Visualizan reportes y estadísticas
   - Supervisan operaciones en tiempo real

2. **Pilotos** (Conductores de buses)
   - Visualizan su ruta asignada del día
   - Ven la secuencia de paradas y horarios
   - Consultan información de alumnos en cada parada
   - Acceden al mapa de su ruta

3. **Monitores** (Asistentes en el bus)
   - Visualizan la ruta del bus asignado
   - Registran qué alumnos fueron recogidos en cada parada
   - Confirman presencia o ausencia de alumnos
   - Acceden al mapa en tiempo real

4. **Padres de Familia** (Clientes)
   - Confirman asistencia diaria de sus hijos
   - Visualizan el bus en tiempo real
   - Registran pagos realizados
   - Consultan historial de pagos y asistencias

### 1.1.6 Beneficios Esperados

#### Para la Empresa de Transporte:
- ✅ **Reducción de costos** operativos (combustible) mediante rutas optimizadas
- ✅ **Mejora en eficiencia** operativa (procesos automatizados)
- ✅ **Mayor control** y trazabilidad de operaciones
- ✅ **Reducción de errores** humanos en registros
- ✅ **Mejor imagen corporativa** (tecnología moderna)
- ✅ **Escalabilidad** del negocio (más fácil gestionar más buses)

#### Para los Pilotos y Monitores:
- ✅ **Claridad en rutas** a seguir cada día
- ✅ **Reducción de tiempo** en planificación manual
- ✅ **Facilidad de registro** de recogidas (digital vs papel)
- ✅ **Acceso desde cualquier dispositivo** con internet

#### Para los Padres de Familia:
- ✅ **Tranquilidad** al ver ubicación del bus en tiempo real
- ✅ **Mejor comunicación** con la empresa (confirmaciones, pagos)
- ✅ **Transparencia** en registros y pagos
- ✅ **Facilidad de uso** (interfaz web responsive)

---

✅ **1.1 INTRODUCCIÓN Y ALCANCE COMPLETADO**

---

## 1.2 Contexto organizacional y stakeholders

*[Pendiente: Sección 1.2]*

---

## 1.3 Análisis de Requerimientos Funcionales

*[Pendiente: Sección 1.3]*

---

## 1.4 Análisis de Requerimientos No Funcionales

*[Pendiente: Sección 1.4]*

---

## 1.5 Análisis de Dominio

*[Pendiente: Sección 1.5]*

---

## 1.6 Supuestos, restricciones y dependencias funcionales

*[Pendiente: Sección 1.6]*

---

**FIN DEL DOCUMENTO (Versión parcial - Solo sección 1.1 completada)**
