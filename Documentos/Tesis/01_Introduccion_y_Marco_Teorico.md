---
documento: Tesis — Capítulos 1 y 2
proyecto: Transportes Génesis
tipo: Introducción y Marco Teórico
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

# Capítulo 1. Introducción

## 1.1. Tema principal

**Implementación de un sistema web integrado de geolocalización en tiempo real y gestión digital de pagos para Transportes Génesis, orientado a padres de familia con hijos en primer grado de un colegio en San Cristóbal, departamento de San Marcos.**

El presente trabajo aborda el diseño, desarrollo e implementación de una plataforma tecnológica que unifica dos necesidades centrales del servicio de transporte escolar: conocer en todo momento la ubicación del bus que transporta a los estudiantes, y gestionar de forma transparente los pagos mensuales del servicio. El segmento de estudio se centra en familias con niños en primer grado, etapa en la que la confianza y la seguridad en el transporte adquieren especial relevancia para los padres.

---

## 1.2. Contexto y antecedentes

El transporte escolar privado desempeña un papel fundamental en la vida diaria de miles de familias guatemaltecas. Empresas como **Transportes Génesis**, con sede en San Cristóbal, departamento de San Marcos, prestan el servicio de traslado de estudiantes hacia colegios de la región. Sin embargo, la operación diaria de estas empresas suele apoyarse en procesos manuales o dispersos: la comunicación con los padres se realiza por mensajes de texto o llamadas telefónicas, los pagos se registran en formatos físicos o hojas de cálculo, y la ubicación del bus rara vez es visible para las familias en tiempo real.

Esta situación genera incertidumbre en los padres cuyos hijos cursan **primer grado** de educación primaria. Un padre de familia con un niño en ese grado no solo necesita saber que el bus llegará a recoger a su hijo, sino también poder visualizar el recorrido, confirmar la asistencia y tener claridad sobre el estado de sus pagos mensuales, todo desde un mismo lugar.

Ante este contexto, surge la necesidad de desarrollar **Transportes Génesis**, un sistema web que integre dos módulos complementarios: uno de **geolocalización en tiempo real**, que permite a los padres ver la ruta y ubicación del bus asignado a sus hijos, y otro de **gestión digital de pagos**, que facilita el registro, validación y consulta de los pagos mensuales del servicio. La plataforma contempla distintos roles —administrador, piloto, monitor y padre de familia—, cada uno con acceso a las funcionalidades que su responsabilidad requiere.

El presente documento forma parte de la investigación orientada a demostrar que la integración de estos dos módulos en una sola plataforma mejora la experiencia de las familias y la eficiencia operativa de la empresa de transporte escolar.

---

## 1.3. Hipótesis

**Hipótesis general:**

Si se implementa y utiliza un sistema web integrado que combine el seguimiento en tiempo real de la geolocalización del transporte escolar con la gestión digital de pagos mensuales, entonces los padres de familia con hijos en primer grado de un colegio en San Cristóbal, departamento de San Marcos atendido por Transportes Génesis percibirán mayor confianza, transparencia y satisfacción con el servicio, y la empresa reducirá la incertidumbre operativa y administrativa, en comparación con los métodos manuales actuales.

**Sub-hipótesis:**

| Código | Sub-hipótesis |
|--------|---------------|
| **H1** | El módulo de geolocalización en tiempo real reduce la incertidumbre de los padres sobre la ubicación del bus de sus hijos, medido mediante una percepción de seguridad igual o superior a 4 sobre 5 en escala Likert. |
| **H2** | El módulo de gestión digital de pagos mejora la transparencia del servicio, medido mediante una percepción de claridad en el estado de pagos igual o superior a 4 sobre 5 en escala Likert. |
| **H3** | La integración de ambos módulos en una sola plataforma incrementa la satisfacción general de los padres del segmento de primer grado, medido mediante una satisfacción global igual o superior a 4 sobre 5 en escala Likert. |

---

## 1.4. Objetivo general

Desarrollar e implementar un sistema web integrado de geolocalización en tiempo real y gestión digital de pagos para Transportes Génesis, orientado a padres de familia con hijos en primer grado de un colegio en San Cristóbal, departamento de San Marcos, que mejore la transparencia, el control y la satisfacción con el servicio de transporte escolar.

### 1.4.1. Objetivos específicos

1. Analizar las necesidades de los padres de familia, el personal operativo y la administración de Transportes Génesis respecto al seguimiento del bus y la gestión de pagos.
2. Diseñar e implementar el módulo de geolocalización en tiempo real, que permita visualizar la ruta y ubicación del bus asignado a cada estudiante.
3. Diseñar e implementar el módulo de gestión digital de pagos, que facilite el registro de comprobantes, la validación administrativa y la consulta del historial de pagos.
4. Integrar ambos módulos en la plataforma Transportes Génesis con acceso diferenciado por roles (administrador, piloto, monitor y padre de familia).
5. Validar el sistema mediante pruebas de aceptación de usuario (UAT) con representantes del colegio y padres del segmento definido, documentadas en el anexo *Plan de Pruebas UAT, Carga y Rendimiento*.
6. Evaluar la percepción de confianza, transparencia y satisfacción de los padres participantes tras utilizar el sistema, mediante un instrumento de encuesta post-UAT.

### 1.4.2. Instrumento de evaluación de percepción

Para el objetivo específico 6 se aplicará un **cuestionario post-UAT** a padres de familia con hijos en primer grado que participen en las pruebas de aceptación. El instrumento evaluará tres dimensiones vinculadas a las sub-hipótesis H1, H2 y H3:

| Dimensión | Variable | Escala | Vinculación |
|-----------|----------|--------|-------------|
| Seguridad / ubicación del bus | Percepción de reducción de incertidumbre | Likert 1–5 | H1 |
| Transparencia en pagos | Claridad del estado de pagos y comprobantes | Likert 1–5 | H2 |
| Satisfacción global | Satisfacción con el servicio integrado | Likert 1–5 | H3 |

**Muestra mínima sugerida:** al menos 5 padres del segmento de primer grado. **Criterio de éxito:** promedio ≥ 4.0 en cada dimensión. El detalle operativo de las pruebas UAT y la aplicación del cuestionario se documenta en el anexo ubicado en `Documentos/Tesis/pruebas_tesis/PLAN_PRUEBAS_UAT_CARGA_RENDIMIENTO.md`.

---

## 1.5. Planteamiento del problema

Transportes Génesis presta actualmente el servicio de transporte escolar a estudiantes de un colegio en San Cristóbal, departamento de San Marcos. La operación diaria presenta las siguientes dificultades:

- **Falta de visibilidad del bus:** los padres no tienen forma de saber en tiempo real dónde se encuentra el bus que transporta a sus hijos. Ante retrasos o cambios de ruta, deben comunicarse por teléfono o mensajes, lo cual genera ansiedad, sobre todo en familias con niños en primer grado.
- **Gestión de pagos poco transparente:** el registro y seguimiento de los pagos mensuales se realiza de forma manual o dispersa. Los padres no siempre tienen claridad sobre si su pago fue recibido, validado o rechazado.
- **Procesos no integrados:** la información de rutas, asistencia y pagos no se encuentra centralizada en un solo sistema, lo que dificulta la operación tanto para la empresa como para las familias.
- **Comunicación limitada:** la coordinación entre administración, pilotos, monitores y padres depende de canales informales (WhatsApp, llamadas), propensos a errores y retrasos.

**Pregunta de investigación:**

¿La implementación de un sistema web que integre geolocalización en tiempo real y gestión digital de pagos mejora la confianza, la transparencia y la satisfacción de los padres de familia con hijos en primer grado que utilizan el servicio de Transportes Génesis en San Cristóbal, departamento de San Marcos?

---

## 1.6. Resumen

Este trabajo propone el desarrollo e implementación de **Transportes Génesis**, una plataforma web para la gestión integral del transporte escolar que integra dos módulos centrales: geolocalización en tiempo real y gestión digital de pagos. El sistema está orientado a padres de familia con hijos en primer grado de un colegio en San Cristóbal, departamento de San Marcos, segmento en el que la seguridad y la confianza en el servicio son prioritarias.

La investigación parte del problema identificado: procesos manuales, falta de visibilidad del bus y pagos poco transparentes. Como respuesta, se plantea una hipótesis compuesta que vincula ambos módulos con resultados medibles en confianza, transparencia y satisfacción. El objetivo general es desarrollar e implementar la plataforma; los objetivos específicos abarcan el análisis de necesidades, el diseño de cada módulo, su integración, la validación con pruebas UAT y la evaluación de la percepción de los usuarios.

El marco teórico que sustenta este trabajo aborda el transporte escolar, los sistemas de información web, la geolocalización, la gestión digital de pagos y la integración de módulos en plataformas orientadas al usuario. Se espera que los resultados aporten evidencia sobre la utilidad de centralizar estos servicios en una sola herramienta digital para empresas de transporte escolar en contextos similares al de San Cristóbal, departamento de San Marcos.

---

## 1.7. Justificación

**Justificación social:** Las familias con hijos en primer grado requieren mayor tranquilidad respecto al transporte escolar. Un sistema que muestre la ubicación del bus y centralice los pagos responde a una necesidad real de seguridad y transparencia.

**Justificación operativa:** Transportes Génesis reduce la dependencia de WhatsApp, llamadas y registros en papel, lo que disminuye errores administrativos y mejora la trazabilidad de rutas, asistencias y cobros.

**Justificación tecnológica:** La integración de geolocalización en tiempo real y pagos digitales en una plataforma web demuestra la viabilidad de digitalizar servicios locales con tecnologías accesibles (ASP.NET Core, SignalR, mapas web).

**Justificación académica:** El trabajo aporta evidencia empírica sobre la aceptación de un sistema integrado por usuarios reales del segmento definido, vinculando hipótesis, pruebas UAT y evaluación de percepción.

---

## 1.8. Delimitación del tema

| Tipo de delimitación | Alcance |
|----------------------|---------|
| **Geográfica** | San Cristóbal, departamento de San Marcos, Guatemala |
| **Poblacional** | Padres de familia con hijos en **primer grado** de un colegio atendido por Transportes Génesis |
| **Temporal** | Desarrollo y validación durante el año 2026 |
| **Tecnológica** | Plataforma web responsive; no incluye aplicación nativa móvil |
| **Institucional** | Colegio atendido por Transportes Génesis (nombre reservado por confidencialidad si la universidad lo requiere) |

---

## 1.9. Alcance y limitaciones

**Dentro del alcance:**

- Módulos de geolocalización en tiempo real, gestión de pagos (comprobante y validación), asistencia, rutas, administración y roles.
- Pruebas internas, UAT, automatización con Selenium y pruebas de carga/rendimiento.
- Evaluación de percepción con cuestionario post-UAT.

**Limitaciones:**

- No se incluye aplicación nativa para iOS o Android.
- Las notificaciones push al teléfono no forman parte de la versión evaluada; las alertas operan dentro del navegador web.
- El pago en línea con pasarela (Stripe) se considera funcionalidad complementaria; el flujo principal evaluado es registro de comprobante y validación administrativa.
- La muestra de padres en UAT puede ser reducida según disponibilidad del colegio (mínimo 5 participantes del segmento de primer grado).
- Las pruebas de carga se realizan en entorno de prueba, por lo que los resultados deben interpretarse como referencia, no como garantía absoluta en producción.

---

# Capítulo 2. Marco teórico

## 2.1. Trasfondo del transporte escolar y los sistemas de información

### 2.1.1. Antecedentes y trabajos relacionados

A nivel internacional existen plataformas de transporte escolar que combinan rastreo GPS, gestión de rutas y comunicación con padres. Estas soluciones han demostrado reducción de llamadas de consulta, mejor puntualidad percibida y mayor satisfacción familiar. En Guatemala, la adopción de este tipo de sistemas en empresas de transporte escolar de mediano tamaño aún es limitada, lo que deja una brecha que Transportes Génesis busca cerrar al integrar **geolocalización** y **pagos** en un solo producto web.

| Referencia / enfoque | Aporte relevante | Brecha que cubre este proyecto |
|----------------------|------------------|--------------------------------|
| Sistemas de rastreo escolar (GPS + mapa) | Visibilidad del bus en tiempo real | Integración con pagos y roles locales |
| Portales de pago escolar | Registro digital de cobros | Falta de vínculo con ubicación del bus |
| Transportes Génesis (propuesta) | Plataforma unificada web | Validación con padres de primer grado en San Marcos |

El transporte escolar es un servicio esencial que garantiza el traslado seguro de estudiantes entre su hogar y el centro educativo. En Guatemala, numerosas familias dependen de empresas privadas de transporte escolar, especialmente en municipios del departamento de San Marcos —como San Cristóbal—, donde la dispersión geográfica y las condiciones del tráfico hacen necesario un servicio organizado y confiable.

Tradicionalmente, la gestión de estas empresas se ha apoyado en procesos manuales: hojas de ruta impresas, registros de asistencia en cuadernos, cobros en efectivo y comunicación telefónica con los padres. Estos métodos, aunque funcionales, presentan limitaciones en cuanto a trazabilidad, transparencia y capacidad de respuesta ante imprevistos.

Los **sistemas de información web** surgen como una alternativa que permite centralizar la operación en una plataforma accesible desde cualquier dispositivo con conexión a internet. Según Laudon y Laudon (2020), un sistema de información combina personas, tecnología y procedimientos para recopilar, procesar y distribuir información que apoye la toma de decisiones en una organización. En el contexto del transporte escolar, esto se traduce en una herramienta donde administradores, pilotos, monitores y padres acceden a la información que necesitan en el momento oportuno.

La digitalización del transporte escolar no es un fenómeno aislado. A nivel internacional, diversas plataformas han demostrado que la integración de rastreo GPS, gestión de rutas y comunicación con padres mejora la eficiencia operativa y la satisfacción de las familias (Bureau of Transportation Statistics, 2019). En el contexto local guatemalteco, la adopción de estas tecnologías aún es incipiente, lo que representa una oportunidad para empresas como Transportes Génesis.

---

## 2.2. Geolocalización y seguimiento en tiempo real

La **geolocalización** es la capacidad de determinar la posición geográfica de un objeto —en este caso, un bus escolar— mediante coordenadas de latitud y longitud obtenidas por un receptor GPS (Global Positioning System). Cuando esta información se transmite y actualiza de forma continua, se habla de **seguimiento en tiempo real**.

Para los padres de familia con hijos en **primer grado**, conocer la ubicación del bus representa un factor de tranquilidad y confianza. Un estudio de la National Association for Pupil Transportation (2018) señala que la visibilidad del transporte escolar reduce significativamente la ansiedad de los padres durante los horarios de recogida y entrega.

En sistemas web, la geolocalización se implementa mediante:

- **Captura de coordenadas GPS** desde el dispositivo del piloto o monitor durante el recorrido.
- **Transmisión al servidor** para almacenar y procesar la ubicación.
- **Actualización en el navegador del padre** mediante tecnologías de comunicación en tiempo real (como SignalR en plataformas .NET), de modo que el mapa refleje el movimiento del bus sin necesidad de recargar la página.
- **Visualización en mapas web** mediante herramientas como Leaflet.js y OpenStreetMap, que permiten mostrar la ruta, las paradas y la posición actual del bus.

Las **alertas de proximidad** complementan el seguimiento: cuando el bus se acerca a la parada del estudiante o al colegio, el sistema puede notificar al padre, reduciendo tiempos de espera y mejorando la coordinación.

---

## 2.3. Gestión digital de pagos en servicios escolares

La **gestión de pagos** en el contexto del transporte escolar implica el registro, seguimiento y validación de los cobros mensuales que las familias realizan por el servicio. Tradicionalmente, este proceso se lleva a cabo con recibos físicos, transferencias bancarias sin confirmación automatizada o pagos en efectivo directamente al conductor, lo que dificulta la trazabilidad y genera conflictos por pagos no registrados o no validados.

La **gestión digital de pagos** digitaliza este flujo en una plataforma web donde:

- Cada padre consulta el **monto asignado** a su cuenta.
- Puede **registrar su pago** subiendo un comprobante (captura de transferencia, depósito, etc.).
- El **administrador valida** el comprobante, marcándolo como aprobado, pendiente o rechazado.
- Existe un **historial consultable** de todos los pagos realizados.

### 2.3.1. Pagos en servicios de transporte escolar

En el sector de transporte escolar, los pagos suelen ser **mensuales** y están vinculados al servicio contratado por cada familia. La falta de un sistema centralizado genera problemas recurrentes: padres que no saben si su pago fue recibido, administradores que pierden tiempo verificando comprobantes en papel, y dificultad para generar reportes financieros.

Un sistema digital resuelve estos problemas al ofrecer un canal único donde el padre registra su pago y la administración lo valida en el mismo lugar, con registro automático de fechas, montos y estados. Esto es especialmente relevante para familias con niños en primer grado, cuyos padres valoran la claridad y la facilidad de uso de las herramientas que utilizan.

### 2.3.2. Validación y trazabilidad de pagos

La **validación** es el paso en el que un administrador revisa el comprobante enviado por el padre y confirma que el pago es correcto. La **trazabilidad** garantiza que cada transacción quede registrada con su historial completo: quién pagó, cuándo, cuánto, en qué estado quedó y quién lo validó.

Estos conceptos son fundamentales para generar **confianza** entre la empresa y las familias. Un padre que puede consultar en cualquier momento el estado de sus pagos y ver el historial completo percibe mayor transparencia que uno que depende de confirmaciones verbales o mensajes informales.

Adicionalmente, el sistema contempla la posibilidad de **pago en línea** mediante pasarela (Stripe) como complemento al flujo principal de comprobante y validación. Esta opción amplía las formas de pago, aunque la evaluación principal del proyecto se centra en el registro de comprobante y la validación administrativa, por ser el flujo más utilizado en el contexto local.

---

## 2.4. Integración de módulos en plataformas web

Uno de los retos centrales de este proyecto es que la geolocalización y los pagos no operen como sistemas aislados, sino como **módulos integrados** dentro de una misma plataforma. La integración aporta ventajas claras:

- **Experiencia unificada para el padre:** accede al mapa del bus, confirma asistencia y gestiona sus pagos desde un solo lugar, sin cambiar de aplicación o canal.
- **Datos centralizados para la administración:** usuarios, buses, rutas, pagos y ubicaciones se gestionan en un solo sistema, facilitando reportes y decisiones.
- **Roles diferenciados:** cada tipo de usuario (administrador, piloto, monitor, padre) ve solo lo que le corresponde, lo que refuerza la seguridad y la usabilidad.

La integración de módulos en plataformas web es un principio ampliamente reconocido en el diseño de sistemas de información. Según Sommerville (2016), la modularidad permite desarrollar, probar y mantener cada componente de forma independiente, mientras la integración garantiza que el conjunto funcione como un sistema coherente orientado al usuario.

En Transportes Génesis, esta integración se materializa en una plataforma ASP.NET Core donde el padre accede a su panel principal y desde ahí navega al mapa del bus, a la confirmación de asistencia o a la sección de pagos, todo con la misma sesión de usuario y el mismo diseño visual.

---

## 2.5. Fundamentos de pruebas de software y validación con usuarios

Desde el punto de vista teórico, la **ingeniería de software** distingue pruebas unitarias, de integración, de aceptación y no funcionales (carga y rendimiento). Las **pruebas de aceptación de usuario (UAT)** validan que el producto satisface las necesidades del cliente final, no solo que cumple especificaciones técnicas (Sommerville, 2016).

La **automatización de pruebas** con herramientas como Selenium permite repetir flujos críticos de forma consistente; en este proyecto, un tester prepara y ejecuta scripts que simulan la interacción humana en el navegador, complementando —sin reemplazar— la evaluación subjetiva de los usuarios.

Las **pruebas de carga y rendimiento** verifican el comportamiento del sistema bajo demanda concurrente, relevante en horarios pico de recogida escolar y en fechas de concentración de pagos mensuales.

El detalle operativo de rondas de prueba, UAT, Selenium y carga se documenta en el anexo: `Documentos/Tesis/pruebas_tesis/PLAN_PRUEBAS_UAT_CARGA_RENDIMIENTO.md`.

---

## 2.6. Definiciones varias

| Término | Definición |
|---------|------------|
| **Transporte escolar** | Servicio de traslado de estudiantes entre su hogar y el centro educativo, prestado por una empresa privada o institución. |
| **Geolocalización** | Determinación de la posición geográfica de un objeto mediante coordenadas GPS. |
| **Seguimiento en tiempo real** | Transmisión y visualización continua de la ubicación de un vehículo mientras se desplaza. |
| **SignalR** | Tecnología de Microsoft que permite comunicación bidireccional en tiempo real entre el servidor y el navegador del usuario. |
| **Sistema de información web** | Plataforma accesible por internet que centraliza datos y procesos de una organización. |
| **Gestión digital de pagos** | Registro, validación y consulta de pagos realizados a través de una plataforma en línea. |
| **Comprobante de pago** | Documento (captura, foto o archivo) que acredita que el padre realizó el pago del servicio. |
| **Validación administrativa** | Revisión por parte del administrador de un comprobante de pago, marcándolo como aprobado, pendiente o rechazado. |
| **UAT (User Acceptance Testing)** | Pruebas de aceptación de usuario: evaluación del sistema por parte de los usuarios finales para confirmar que cumple sus necesidades. |
| **Módulo** | Componente funcional del sistema que agrupa un conjunto de funcionalidades relacionadas (ejemplo: módulo de geolocalización, módulo de pagos). |
| **Rol de usuario** | Conjunto de permisos y pantallas asignados a un tipo de usuario (administrador, piloto, monitor, padre de familia). |
| **Parada** | Punto geográfico donde el bus recoge o deja a un estudiante durante su ruta. |
| **Ruta escolar** | Secuencia ordenada de paradas que el bus recorre en un turno (mañana o tarde). |
| **Primer grado** | Primer año de educación primaria; segmento de estudio de esta investigación. |
| **Asistencia escolar** | Confirmación por parte del padre de si el estudiante utilizará el transporte en un turno determinado. |
| **Traslado** | Solicitud de cambio temporal o permanente de bus o ruta asignada al estudiante. |
| **Leaflet.js** | Biblioteca JavaScript para mapas interactivos en el navegador web. |
| **OpenStreetMap** | Proyecto de mapas libres usado como capa base del sistema. |
| **San Cristóbal, departamento de San Marcos** | Municipio del departamento de San Marcos, Guatemala; contexto geográfico del estudio. |
| **Transportes Génesis** | Empresa de transporte escolar para la cual se desarrolla el sistema objeto de esta investigación. |

---

## Referencias bibliográficas sugeridas (a completar con formato de tu universidad)

- Bureau of Transportation Statistics. (2019). *School Transportation Statistics*. U.S. Department of Transportation.
- Laudon, K. C., & Laudon, J. P. (2020). *Management Information Systems: Managing the Digital Firm* (16th ed.). Pearson.
- National Association for Pupil Transportation. (2018). *NAPT Key Industry Facts and Statistics*.
- Sommerville, I. (2016). *Software Engineering* (10th ed.). Pearson.

> **Nota:** Completar las referencias con el formato bibliográfico exigido por tu universidad (APA, Vancouver, etc.) y agregar fuentes locales guatemaltecas si las encuentras.

---

*Documento formal de tesis — Transportes Génesis — Julio 2026*
