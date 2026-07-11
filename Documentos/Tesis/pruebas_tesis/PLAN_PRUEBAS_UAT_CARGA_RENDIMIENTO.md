---
documento: Plan de Pruebas — Anexo de Tesis
proyecto: Transportes Génesis
tipo: UAT, Carga y Rendimiento
formato:
  fuente: Times New Roman
  tamano: 12 pt
  interlineado: 1.5
  margenes_cm:
    superior: 2.5
    inferior: 2.5
    izquierdo: 3.0
    derecho: 2.5
  alineacion: Justificado
  sangria_primera_linea: 0.63 cm
institucion: "[Nombre de la universidad]"
carrera: "[Nombre de la carrera]"
autor: "[Nombre del autor]"
asesor: "[Nombre del asesor]"
lugar: San Cristóbal, departamento de San Marcos, Guatemala
fecha: Julio 2026
---

---

## 1. Introducción y objetivo

**Transportes Génesis** es el sistema de transporte escolar que usarán cuatro tipos de usuarios:

| Rol | Qué hace en el sistema |
|-----|------------------------|
| **Padre de familia** | Ve la ruta del bus de sus hijos, confirma asistencia y realiza sus pagos. |
| **Piloto** | Ve su ruta asignada y transmite la ubicación del bus. |
| **Monitor** | Acompaña al piloto, ve el listado de niños y también transmite la ruta. |
| **Administrador** | Gestiona usuarios, buses, rutas, asignaciones y valida los pagos. |

**Objetivo de este documento:** establecer de forma formal y detallada cómo se ejecutarán las pruebas del sistema **Transportes Génesis** antes de entregarlo al colegio, validando los módulos de geolocalización, pagos y demás funcionalidades, con participación del segmento de **padres de familia con hijos en primer grado** en San Cristóbal, departamento de San Marcos.

**Vinculación con la tesis:** Este anexo sustenta los objetivos específicos 5 y 6 del Capítulo 1 y las sub-hipótesis H1 (geolocalización), H2 (pagos) e H3 (integración), mediante pruebas UAT, encuesta post-UAT y pruebas de carga.

---

## 1.2. Alcance de módulos a probar

| Módulo | Qué se prueba |
|--------|---------------|
| **Autenticación y roles** | Inicio de sesión, cambio de clave en el primer ingreso y que cada rol solo vea lo que le corresponde. |
| **Geolocalización / mapa en vivo** | Ver la ruta del bus, mapa en tiempo real, simulación de recorrido y alertas de proximidad. |
| **Rutas y paradas** | Cálculo de rutas de mañana y tarde, gestión de paradas y sus ubicaciones. |
| **Asistencia** | El padre confirma la asistencia y el monitor ve el listado de niños. |
| **Pagos (módulo principal)** | Monto asignado, comprobante mensual, bancos/cuentas, validación admin, dashboard, historial |
| **Pagos en línea (opcional v1.0)** | Pasarela Stripe — prueba complementaria si está habilitada en el entorno |
| **Reportes administrativos** | Reportes de rutas, asignaciones y pagos desde panel admin |
| **Registro de recogida** | Monitor registra recogida de estudiantes en paradas |
| **Traslados** | Solicitud y gestión de traslados. |
| **Alertas y notificaciones** | Avisos que recibe cada usuario. |
| **Administración** | Gestión de usuarios, buses y asignación de piloto/monitor a cada bus. |

> El alcance incluye geolocalización y pagos como módulos centrales de la investigación; ambos deben validarse con igual rigor.

### 1.2.1. Matriz de cobertura (módulo × ronda × tipo de prueba)

| Módulo | Ronda 1 | Ronda 2 | Ronda 3 UAT | Ronda 4 | Selenium | Carga |
|--------|---------|---------|-------------|---------|----------|-------|
| Autenticación y roles | Sí | Si hubo fallo | Sí | Sí | Sí | No |
| Geolocalización / mapa | Sí | Si hubo fallo | Sí | Sí | Sí | Sí |
| Rutas y paradas | Sí | Si hubo fallo | Parcial (admin) | Sí | No | No |
| Asistencia | Sí | Si hubo fallo | Sí | Sí | Sí | No |
| Pagos (comprobante) | Sí | Si hubo fallo | Sí | Sí | Sí | Sí |
| Pagos en línea (Stripe) | Opcional | Opcional | Opcional | Opcional | No | No |
| Traslados | Sí | Si hubo fallo | Parcial | Sí | No | No |
| Alertas | Sí | Si hubo fallo | Parcial | Sí | No | No |
| Administración | Sí | Si hubo fallo | Sí | Sí | Parcial | No |
| Reportes | Sí | Si hubo fallo | No | Sí | No | No |
| Encuesta post-UAT | No | No | Sí | Sí | No | No |

---

## 2. Rondas de prueba: cómo se organizarán

Una **ronda de prueba** es una "vuelta completa" en la que revisamos el sistema, anotamos lo que falla, lo corregimos y volvemos a revisar. Se hace en varias vueltas porque casi nunca todo sale perfecto a la primera: cada ronda deja el sistema más pulido.

| Ronda | Objetivo | Qué se valida |
|-------|----------|---------------|
| **Ronda 1 – Interna** | Detectar fallos evidentes | El equipo prueba todos los módulos por su cuenta (login, mapa, asistencia, pagos, administración). Se anotan los errores. |
| **Ronda 2 – Corrección** | Validar arreglos | Se corrigen los fallos de la ronda 1 y se vuelve a probar solo lo que se tocó y sus alrededores. |
| **Ronda 3 – UAT con el colegio** | Aceptación real | Usuarios reales (o representantes del colegio) usan el sistema como lo harían en el día a día. |
| **Ronda 4 – Final / cierre** | Confirmar que todo quedó bien | Se revisan las observaciones de la ronda 3 ya corregidas y se da el visto bueno final. |

**Criterio para pasar de una ronda a la siguiente:** no deben quedar errores **graves** abiertos (por ejemplo, que no se pueda pagar, que no cargue el mapa o que un rol vea información que no le toca). Los detalles menores se anotan y se resuelven, pero no frenan el avance.

---

## 3. Pruebas UAT (Aceptación de Usuario)

**UAT** significa *Pruebas de Aceptación de Usuario* (por sus siglas en inglés, *User Acceptance Testing*).

**¿Qué son?** Son las pruebas que hacen los **usuarios finales** —no los programadores— usando el sistema como lo usarían de verdad. En vez de preguntar "¿el sistema técnicamente funciona?", preguntan "¿este sistema me sirve para lo que necesito hacer?".

**¿Por qué las usamos en este proyecto?**

- Confirman que el sistema resuelve las necesidades reales del **colegio** y de las **familias**, no solo que "no tiene errores".
- Un padre puede darse cuenta de detalles que el equipo técnico no nota (por ejemplo, que un texto confunde o que un paso es incómodo).
- Dan **confianza y respaldo**: si los usuarios aprueban, hay acuerdo de que el sistema está listo.

**¿Quiénes participan?** Representantes del colegio y **padres de familia con hijos en primer grado** (mínimo 5 participantes del segmento de estudio), acompañados por el equipo de Transportes Génesis.

**Criterios de aceptación vinculados a la investigación:**

| Sub-hipótesis | Qué valida la prueba | Evidencia |
|---------------|----------------------|-----------|
| **H1** | Mapa en vivo reduce incertidumbre | Escenario UAT mapa + ítem encuesta de seguridad |
| **H2** | Pagos digitales mejoran transparencia | Escenario UAT pagos + ítem encuesta de claridad |
| **H3** | Integración mejora satisfacción | Demo 3 navegadores + ítem encuesta global |

**¿Cómo se registran los resultados?** Cada prueba se marca como:

- **Aprobada:** funcionó como se esperaba.
- **Aprobada con observaciones:** funciona, pero hay algo por mejorar.
- **Rechazada:** no funcionó o no cumple lo esperado (se corrige y se vuelve a probar).

---

## 4. Automatización de UAT con Selenium

**¿Qué es Selenium?** Es una herramienta para automatizar pruebas en el navegador.

> **Aclaración importante:** Selenium **no es un robot** que decide por sí solo. Es **una persona del equipo (un tester)** quien prepara y ejecuta un **script**. Ese script no es más que una lista de pasos escrita por adelantado que le indica al navegador qué hacer: abrir la página, escribir el usuario y la contraseña, hacer clic en un botón, revisar que aparezca lo esperado, etc. Es decir, reproduce los mismos pasos que haría un usuario real, pero de forma automática.

**¿Por qué elegimos Selenium?**

- Permite **repetir siempre los mismos pasos** de forma exacta, sin equivocaciones por cansancio o distracción.
- **Ahorra tiempo:** en cada ronda no hay que rehacer manualmente todas las pruebas; se ejecuta el script y listo.
- **Reduce el error humano** al repetir pruebas largas o repetitivas.
- Es una herramienta **conocida y ampliamente usada** para probar aplicaciones web como la nuestra.

**¿Qué flujos se automatizarán primero?** Los más importantes y repetitivos:

- Inicio de sesión de cada rol (padre, piloto, monitor, admin).
- Ver la ruta del bus en el mapa.
- Confirmar asistencia.
- **Flujo de pagos:** ver el monto asignado, subir un comprobante y la validación del pago por parte del administrador.

> **El script solo ejecuta los pasos.** Las personas siguen revisando los resultados y son quienes dan el **visto bueno final** de aceptación. La automatización acompaña a las personas, no las reemplaza.

---

## 5. Coordinación con el colegio (reunión)

**La situación:** el colegio trabaja de **lunes a viernes**, y nuestro equipo solo tiene disponibilidad los **sábados**. Esto hace necesario ponernos de acuerdo con anticipación.

**El plan:**

1. **Contactar al colegio** para acordar una reunión inicial y definir un canal de comunicación (correo, teléfono o grupo de mensajería).
2. **Realizar una reunión** para alinear expectativas, fechas y responsables.
3. **Dejar un calendario acordado** de las pruebas (especialmente las de aceptación, ronda 3).

**Puntos a acordar en la reunión:**

| Punto | Qué se busca definir |
|-------|----------------------|
| **Fecha y formato de la reunión** | Presencial o virtual, en un día hábil que le sirva al colegio. |
| **Horarios de las pruebas UAT** | Cómo combinamos la disponibilidad del colegio (lun–vie) con la nuestra (sábados). |
| **Responsables de cada lado** | Una persona de contacto del colegio y una de nuestro equipo. |
| **Participantes de las pruebas** | Qué padres o personal del colegio probarán el sistema. |
| **Calendario de sábados** | Qué sábados usaremos para ejecutar y corregir de nuestro lado. |
| **Canal de comunicación** | Cómo reportamos avances y dudas durante la semana. |

> Como el tiempo en común es limitado, la reunión sirve para **acordar todo por adelantado** y aprovechar bien cada sábado.

---

## 6. Pruebas de carga y rendimiento

Estas pruebas responden a dos preguntas distintas:

- **Carga:** ¿qué pasa cuando **muchas personas** usan el sistema **al mismo tiempo**?
- **Rendimiento:** ¿qué tan **rápido responde** el sistema?

**¿Por qué son importantes en este proyecto?** Porque hay momentos de mucha actividad simultánea:

- En la **mañana y la tarde**, varios padres abren el mapa en vivo al mismo tiempo mientras los buses transmiten su ubicación.
- A **inicio de mes**, muchos padres suben sus comprobantes de pago casi a la vez, y el administrador los valida.

Si el sistema se pusiera lento o se cayera en esos momentos, afectaría directamente a las familias.

**¿Cómo se implementarán (a alto nivel)?**

1. **Definir escenarios realistas**, por ejemplo:
   - Muchos padres viendo el mapa en vivo al mismo tiempo.
   - Varios buses transmitiendo su ubicación a la vez.
   - Varios padres subiendo comprobantes de pago simultáneamente.
2. **Simular ese volumen** de usuarios con una herramienta de pruebas de carga (por ejemplo, herramientas estándar como *Apache JMeter* o *k6*, mencionadas solo como referencia).
3. **Medir** cómo responde el sistema durante esa simulación.
4. **Comparar** los resultados contra metas acordadas y, si algo no cumple, mejorarlo.

**¿Qué se mide?**

| Indicador | Qué significa |
|-----------|---------------|
| **Tiempo de respuesta** | Cuánto tarda una pantalla o acción en responder. |
| **Usuarios simultáneos soportados** | Cuántas personas puede atender a la vez sin problemas. |
| **Estabilidad del mapa en vivo** | Que el mapa siga actualizándose sin trabarse ni caerse. |
| **Estabilidad de los pagos** | Que subir y validar comprobantes funcione incluso con mucha gente a la vez. |

**Metas de referencia (criterios de aprobación):**

| Indicador | Meta sugerida |
|-----------|---------------|
| Tiempo de respuesta de pantallas | Menos de 3 segundos |
| Mapa en vivo sin desconexiones | Sin caídas durante 30 min de simulación |
| Subida de comprobantes simultáneos | Sin errores con al menos 10 usuarios a la vez |

---

## 7. Cómo ejecutar el plan de pruebas (paso a paso)

Esta sección explica **qué haremos en la práctica** para llevar a cabo todo lo descrito arriba. Es la guía operativa del equipo.

### 7.1 Antes de empezar (preparación)

| Paso | Acción | Responsable |
|------|--------|-------------|
| 1 | Confirmar que el **entorno de prueba** está activo y accesible (no usar producción real). | Equipo técnico |
| 2 | Verificar la **base de datos de prueba** con usuarios, buses y rutas cargados. | Equipo técnico |
| 3 | Revisar la guía `Docs/CONFIGURACION_PRUEBAS_PADRE_PILOTO_MONITOR.md` y confirmar que padre, piloto/monitor y bus **BUS-001** están alineados. | Tester |
| 4 | Preparar la **bitácora de defectos** (hoja de cálculo o documento compartido). | Tester |
| 5 | Tener listos **3 navegadores** (o dispositivos) para la demo completa: admin, padre y monitor/piloto. | Tester |
| 6 | Agendar la **reunión con el colegio** antes de la ronda 3. | Coordinador |

**Checklist de preparación:**

| Ítem | Verificado (Sí/No) |
|------|-------------------|
| Entorno de prueba levantado | |
| Usuarios de prueba funcionando (login correcto) | |
| Rutas de mañana y tarde calculadas para la fecha de prueba | |
| Bitácora de defectos lista | |
| Reunión con colegio agendada (antes de ronda 3) | |
| Credenciales listas para padres de primer grado (mín. 5) | |

---

### 7.2 Ejecución de la Ronda 1 — Pruebas internas

**Cuándo:** primer sábado disponible.  
**Duración estimada:** 4–6 horas.  
**Quién:** todo el equipo de desarrollo/pruebas.

**Orden sugerido de ejecución:**

| Orden | Módulo | Qué hacer | Resultado esperado |
|-------|--------|-----------|-------------------|
| 1 | Login | Entrar con cada rol (admin, padre, piloto, monitor). | Cada uno llega a su pantalla principal. |
| 2 | Admin – configuración | Verificar buses, paradas, rutas y asignaciones. | BUS-001 con piloto/monitor asignado y ruta activa. |
| 3 | Padre – configuración | Login como padre de prueba; confirmar que ve a sus hijos. | Panel padre visible con tarjetas (Pagos, Ruta, Asistencia). |
| 4 | Padre – mapa | Abrir ruta del bus y dejar la pantalla abierta. | Mapa carga con ruta y paradas. |
| 5 | Piloto/Monitor – simulación | Iniciar sesión y pulsar **Simular ruta**. | El bus se mueve en el mapa del padre y del admin. |
| 6 | Padre – asistencia | Confirmar asistencia del hijo para mañana o tarde. | Confirmación guardada sin error. |
| 7 | Padre – pagos | Ver monto, subir comprobante de prueba. | Comprobante queda en estado pendiente de validar. |
| 8 | Admin – pagos | Revisar dashboard de pagos y validar el comprobante. | Pago marcado como aprobado o rechazado. |
| 9 | Traslados | Solicitar traslado desde panel padre. | Solicitud registrada sin error grave. |
| 10 | Alertas | Verificar alertas de proximidad durante simulación. | Alertas visibles en pantallas correspondientes. |
| 11 | Primer ingreso | Usuario con `IsFirstLogin` debe cambiar contraseña. | Redirección a cambio de clave obligatorio. |
| 12 | Piloto (rol separado) | Login piloto, ver ruta y simular recorrido. | Mismo bus que padre de prueba (BUS-001). |
| 13 | Monitor (rol separado) | Login monitor, listado de niños y simulación. | Listado y mapa operativos. |
| 14 | Admin – reportes | Generar reporte de rutas o pagos. | Reporte descargable o visible sin error. |
| 15 | Pagos en línea (opcional) | Probar pasarela Stripe si está habilitada. | Transacción de prueba o documentar exclusión. |

**Al terminar la ronda 1:**

1. Reunir todos los hallazgos en la bitácora.
2. Clasificar cada error: grave, media o menor.
3. Priorizar correcciones (primero los graves).
4. Definir qué se corrige antes de la ronda 2.

---

### 7.3 Ejecución de la Ronda 2 — Validación de correcciones

**Cuándo:** sábado siguiente a la ronda 1.  
**Duración estimada:** 2–4 horas.

**Pasos:**

1. Revisar la bitácora: solo probar lo que se corrigió **y** los módulos relacionados.
2. Repetir el flujo completo si hubo errores graves en login, mapa o pagos.
3. Ejecutar los **scripts de Selenium** (si ya están listos) para confirmar que los flujos automatizados pasan.
4. Si no quedan errores graves abiertos → **autorizar paso a ronda 3**.

---

### 7.4 Ejecución de la Ronda 3 — UAT con el colegio

**Cuándo:** según fecha acordada en la reunión con el colegio.  
**Participantes:** representantes del colegio, **padres con hijos en primer grado** (mínimo 5) y el equipo de Transportes Génesis.

**Antes de la sesión UAT:**

| Paso | Acción |
|------|--------|
| 1 | Enviar al colegio un **recordatorio** con fecha, hora y enlace de acceso. |
| 2 | Tener credenciales de prueba listas para cada participante. |
| 3 | Preparar una **lista de escenarios** que deben probar (no improvisar en vivo). |
| 4 | Designar una persona del equipo que **tome notas** de observaciones. |

**Escenarios UAT para el colegio (lista de ejecución):**

| # | Escenario | Rol | Pregunta al usuario | Hipótesis |
|---|-----------|-----|---------------------|-----------|
| 1 | Iniciar sesión | Padre (1er grado) | ¿Pudo entrar sin ayuda? | H3 |
| 2 | Ver ruta del bus en el mapa | Padre (1er grado) | ¿Entiende dónde va el bus? | H1 |
| 3 | Confirmar asistencia del hijo | Padre (1er grado) | ¿Le resultó claro el proceso? | H3 |
| 4 | Consultar monto y subir comprobante | Padre (1er grado) | ¿Pudo registrar su pago? | H2 |
| 5 | Consultar historial de pagos | Padre (1er grado) | ¿Ve claramente el estado de sus pagos? | H2 |
| 6 | Simular recorrido del bus | Monitor | ¿La pantalla muestra la ruta correcta? | H1 |
| 7 | Simular recorrido del bus | Piloto | ¿Puede transmitir ubicación sin error? | H1 |
| 8 | Validar un pago | Admin | ¿Puede aprobar o rechazar comprobantes? | H2 |
| 9 | Gestionar asignación piloto-bus | Admin | ¿La asignación queda activa correctamente? | — |
| 10 | Solicitar traslado | Padre | ¿El formulario es comprensible? | H3 |
| 11 | Demo integrada (3 navegadores) | Todos | ¿El bus se mueve en padre y admin al simular? | H1, H3 |
| 12 | Encuesta post-UAT | Padre (1er grado) | Escala Likert seguridad, transparencia, satisfacción | H1, H2, H3 |

**Durante la sesión:**

1. Explicar en 5 minutos qué es el sistema (sin tecnicismos).
2. Dar credenciales y dejar que **el usuario haga los pasos** (no hacerlo por ellos).
3. Anotar cada observación tal como la dicen.
4. Al final, marcar cada escenario: aprobado, con observaciones o rechazado.

**Después de la sesión:**

1. Consolidar observaciones en la bitácora.
2. Enviar al colegio un **resumen** de lo probado y lo pendiente.
3. Programar correcciones para el sábado siguiente.

---

### 7.5 Ejecución de pruebas con Selenium

**Cuándo:** a partir de la ronda 2; se repiten en cada ronda siguiente.  
**Quién:** tester del equipo.

**Pasos para ejecutar (sin entrar en código):**

| Paso | Acción |
|------|--------|
| 1 | El tester **prepara el script** con los pasos de cada flujo (login, mapa, asistencia, pagos). |
| 2 | Verifica entorno activo en `https://localhost:7241` (o URL acordada). |
| 3 | Confirma credenciales de prueba (padre1, monitor1, admin) según `Docs/CONFIGURACION_PRUEBAS_PADRE_PILOTO_MONITOR.md`. |
| 4 | **Ejecuta el script** desde la herramienta Selenium. |
| 5 | Revisa el **reporte de resultados**: qué pasó y qué falló. |
| 6 | Si algo falla, lo anota en la bitácora y lo reporta al equipo de desarrollo. |
| 7 | Tras corregir el error, **vuelve a ejecutar** solo ese script. |

**Criterio de script aprobado:** el script pasa 3 ejecuciones consecutivas sin fallos en el entorno de prueba.

**Flujos a ejecutar en cada ronda con Selenium:**

- Login por rol (4 scripts: padre, piloto, monitor, admin).
- Ver mapa del bus (padre).
- Confirmar asistencia (padre).
- Flujo de pago completo: ver monto → subir comprobante → validar (padre + admin).

> Selenium acelera la repetición; el visto bueno final sigue siendo responsabilidad de las personas en la UAT.

---

### 7.6 Ejecución de pruebas de carga y rendimiento

**Cuándo:** sábado después de la ronda 2 (entorno estable).  
**Duración estimada:** 2–3 horas.

**Pasos:**

| Paso | Acción |
|------|--------|
| 1 | Definir cuántos usuarios simultáneos simular (ejemplo: 20 padres viendo mapa, 5 buses transmitiendo, 10 subiendo pagos). |
| 2 | Configurar la herramienta de carga (JMeter o k6) con esos escenarios. |
| 3 | Ejecutar la simulación en el **entorno de prueba** (nunca en producción). |
| 4 | Registrar: tiempo de respuesta, errores, caídas del mapa o de pagos. |
| 5 | Comparar resultados con las **metas de la sección 6**. |
| 6 | Si no cumple → reportar al equipo técnico para optimizar antes de la ronda 4. |

---

### 7.7 Ejecución de la Ronda 4 — Cierre y visto bueno

**Cuándo:** tras corregir todas las observaciones de la ronda 3.

**Pasos:**

1. Repetir los escenarios UAT que quedaron con observaciones o rechazados.
2. Ejecutar scripts Selenium de los flujos corregidos.
3. Confirmar con el colegio (breve sesión o por escrito) que los puntos pendientes quedaron resueltos.
4. Elaborar **acta de cierre** (ver plantilla sección 9.1).
5. Ejecutar **demo integrada 7.8** como prueba de cierre obligatoria.
6. Marcar el plan como **completado** si no hay errores graves abiertos.

---

### 7.8 Demo completa recomendada (3 navegadores)

Para validar geolocalización + pagos en una sola sesión, seguir este orden (detalle en `Docs/CONFIGURACION_PRUEBAS_PADRE_PILOTO_MONITOR.md`):

| Paso | Rol | Acción |
|------|-----|--------|
| 1 | Admin | Abrir mapa en tiempo real → elegir BUS-001 |
| 2 | Padre | Abrir ruta del bus asignado a su hijo |
| 3 | Monitor | Iniciar **Simular ruta** |
| 4 | Todos | Verificar que el bus se mueve en los 3 pantallas |
| 5 | Padre | Subir comprobante de pago |
| 6 | Admin | Validar el pago en el dashboard |

---

## 8. Entorno de pruebas

Todas las pruebas se realizan en un **ambiente separado** (de prueba), **no** en el sistema real que usarán las familias.

| Elemento | Especificación |
|----------|----------------|
| **URL de prueba** | `https://localhost:7241` (desarrollo) o servidor acordado |
| **Base de datos** | `TransportesGenesis` en entorno local o de prueba |
| **Sistema operativo** | Windows 10/11 (equipo de desarrollo) |
| **Navegadores** | Chrome, Edge o Firefox (versiones actuales) |
| **Usuarios seed** | padre1, monitor1, piloto1, admin — contraseña `Admin123!` |
| **Bus de demo** | BUS-001 (IdBus 4) alineado con hijos de padre1 |
| **Documentación de apoyo** | `Docs/CONFIGURACION_PRUEBAS_PADRE_PILOTO_MONITOR.md` |

Esto asegura que ningún experimento afecte los datos reales del colegio ni de los padres, y que se puedan usar datos de prueba con libertad.

---

## 9. Registro de errores (bitácora de defectos)

Cada problema se registra en la bitácora con los siguientes campos:

| Campo | Descripción | Ejemplo |
|-------|-------------|---------|
| **ID** | Identificador único | DEF-001 |
| **Qué falló** | Descripción breve | Error al subir comprobante |
| **Módulo / Rol** | Área afectada | Pagos / Padre de familia |
| **Pasos para reproducir** | Secuencia | 1. Login padre 2. Pagos 3. Subir archivo |
| **Gravedad** | Grave / Media / Menor | Media |
| **Estado** | Abierto / En corrección / Cerrado | Abierto |
| **Evidencia** | Captura o referencia | captura_pago_error.png |
| **Responsable** | Quién corrige | Equipo desarrollo |
| **Fecha cierre** | Cuándo se resolvió | — |

### 9.1. Plantilla de acta de cierre

| Campo | Contenido |
|-------|-----------|
| **Fecha** | |
| **Participantes** | Equipo Génesis + representantes colegio |
| **Rondas completadas** | 1, 2, 3, 4 |
| **Módulos probados** | Listado de módulos de sección 1.2 |
| **Escenarios UAT** | Aprobados / Con observaciones / Rechazados |
| **Encuesta post-UAT** | Promedio H1, H2, H3 (escala 1–5) |
| **Errores graves abiertos** | Ninguno / Listado |
| **Resultado final** | Aprobado para entrega / Pendiente |
| **Firmas** | Responsable equipo / Representante colegio |

---

## 10. Riesgos y plan B

| Riesgo | Plan B |
|--------|--------|
| El colegio no está disponible un sábado acordado | Reprogramar y aprovechar ese sábado para pruebas internas y correcciones de nuestro lado. |
| Falla el internet durante una prueba | Continuar con pruebas que no dependan de conexión y reagendar las que sí. |
| Aparecen muchos errores en una ronda | Priorizar los graves, corregir y repetir la ronda antes de involucrar al colegio. |
| Poca disponibilidad de tiempo en común | Automatizar con Selenium lo más posible para aprovechar mejor las horas con el colegio. |

---

## 11. Calendario y responsables (resumen)

| Actividad | Cuándo | Responsable |
|-----------|--------|-------------|
| Contactar al colegio y agendar reunión | Cuanto antes (día hábil) | Equipo Transportes Génesis |
| Reunión de coordinación | Día hábil acordado | Equipo + Colegio |
| Rondas internas (1 y 2) | Sábados | Equipo Transportes Génesis |
| Automatización con Selenium | Sábados / durante la semana | Tester del equipo |
| Pruebas UAT (ronda 3) | Según acuerdo con el colegio | Colegio + Equipo |
| Pruebas de carga y rendimiento | Sábados | Equipo Transportes Génesis |
| Ronda final y visto bueno | Tras corregir observaciones | Colegio + Equipo |

---

## 12. Glosario

| Término | Significado sencillo |
|---------|----------------------|
| **UAT** | Pruebas de Aceptación de Usuario: los usuarios reales confirman que el sistema les sirve. |
| **Ronda de prueba** | Una vuelta completa de revisar, anotar fallos, corregir y volver a revisar. |
| **Selenium** | Herramienta con la que un tester ejecuta un script que repite pasos en el navegador de forma automática. |
| **Script** | Lista de pasos escrita por adelantado que indica qué hacer en cada prueba. |
| **Carga** | Muchos usuarios usando el sistema al mismo tiempo. |
| **Rendimiento** | Qué tan rápido responde el sistema. |
| **Bitácora de defectos** | Registro formal de errores con seguimiento hasta cierre. |
| **Acta de cierre** | Documento firmado que certifica fin de pruebas y resultado. |
| **Encuesta post-UAT** | Cuestionario Likert para medir H1, H2 y H3. |
| **Entorno de prueba** | Instancia del sistema separada de producción. |

---

## 13. Resumen final

Usaremos **tres pilares** para tener confianza en el sistema antes de entregarlo:

1. **Pruebas UAT** (con apoyo de **Selenium**): confirman, con usuarios reales, que **todos los módulos** —incluyendo geolocalización **y pagos**— funcionan y son fáciles de usar.
2. **Coordinación con el colegio**: una reunión previa nos permite acordar fechas y aprovechar el tiempo en común, dado que el colegio trabaja de lunes a viernes y nosotros tenemos disponibilidad los sábados.
3. **Pruebas de carga y rendimiento**: aseguran que el sistema aguante los momentos de mayor uso (mapa en vivo en horas pico y pagos a inicio de mes) sin ponerse lento ni fallar.

Con estos tres pilares buscamos entregar al colegio un sistema **probado, estable y confiable** para las familias.

---

*Documento formal de tesis — Transportes Génesis — Julio 2026*
