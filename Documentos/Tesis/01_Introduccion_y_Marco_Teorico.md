# Transportes Génesis
## Sistema web integrado de geolocalización y gestión de pagos para transporte escolar

---

# Capítulo 1. Introducción

## 1.1. Tema principal

**Implementación de un sistema web integrado de geolocalización en tiempo real y gestión digital de pagos para Transportes Génesis, orientado a padres de familia con hijos en primer grado de un colegio en San Cristóbal de las Casas.**

El presente trabajo aborda el diseño, desarrollo e implementación de una plataforma tecnológica que unifica dos necesidades centrales del servicio de transporte escolar: conocer en todo momento la ubicación del bus que transporta a los estudiantes, y gestionar de forma transparente los pagos mensuales del servicio. El segmento de estudio se centra en familias con niños en primer grado, etapa en la que la confianza y la seguridad en el transporte adquieren especial relevancia para los padres.

---

## 1.2. Introducción

El transporte escolar privado desempeña un papel fundamental en la vida diaria de miles de familias guatemaltecas. Empresas como **Transportes Génesis**, con sede en San Cristóbal de las Casas, prestan el servicio de traslado de estudiantes hacia colegios de la región. Sin embargo, la operación diaria de estas empresas suele apoyarse en procesos manuales o dispersos: la comunicación con los padres se realiza por mensajes de texto o llamadas telefónicas, los pagos se registran en formatos físicos o hojas de cálculo, y la ubicación del bus rara vez es visible para las familias en tiempo real.

Esta situación genera incertidumbre en los padres, especialmente cuando sus hijos cursan los primeros años de educación primaria. Un padre de familia con un niño en primer grado no solo necesita saber que el bus llegará a recoger a su hijo, sino también poder visualizar el recorrido, confirmar la asistencia y tener claridad sobre el estado de sus pagos mensuales, todo desde un mismo lugar.

Ante este contexto, surge la necesidad de desarrollar **Transportes Génesis**, un sistema web que integre dos módulos complementarios: uno de **geolocalización en tiempo real**, que permite a los padres ver la ruta y ubicación del bus asignado a sus hijos, y otro de **gestión digital de pagos**, que facilita el registro, validación y consulta de los pagos mensuales del servicio. La plataforma contempla distintos roles —administrador, piloto, monitor y padre de familia—, cada uno con acceso a las funcionalidades que su responsabilidad requiere.

El presente documento forma parte de la investigación orientada a demostrar que la integración de estos dos módulos en una sola plataforma mejora la experiencia de las familias y la eficiencia operativa de la empresa de transporte escolar.

---

## 1.3. Hipótesis

**Hipótesis general:**

Si se implementa y utiliza un sistema web integrado que combine el seguimiento en tiempo real de la geolocalización del transporte escolar con la gestión digital de pagos mensuales, entonces los padres de familia con hijos en primer grado de un colegio en San Cristóbal de las Casas atendido por Transportes Génesis percibirán mayor confianza, transparencia y satisfacción con el servicio, y la empresa reducirá la incertidumbre operativa y administrativa, en comparación con los métodos manuales actuales.

**Sub-hipótesis:**

| Código | Sub-hipótesis |
|--------|---------------|
| **H1** | El módulo de geolocalización en tiempo real reduce la incertidumbre de los padres sobre la ubicación del bus de sus hijos durante el recorrido escolar. |
| **H2** | El módulo de gestión digital de pagos mejora la transparencia y el control de los pagos mensuales del servicio de transporte. |
| **H3** | La integración de ambos módulos en una sola plataforma incrementa la satisfacción general de los padres del segmento de primer grado. |

---

## 1.4. Objetivo general

Desarrollar e implementar un sistema web integrado de geolocalización en tiempo real y gestión digital de pagos para Transportes Génesis, orientado a padres de familia con hijos en primer grado de un colegio en San Cristóbal de las Casas, que mejore la transparencia, el control y la satisfacción con el servicio de transporte escolar.

### 1.4.1. Objetivos específicos

1. Analizar las necesidades de los padres de familia, el personal operativo y la administración de Transportes Génesis respecto al seguimiento del bus y la gestión de pagos.
2. Diseñar e implementar el módulo de geolocalización en tiempo real, que permita visualizar la ruta y ubicación del bus asignado a cada estudiante.
3. Diseñar e implementar el módulo de gestión digital de pagos, que facilite el registro de comprobantes, la validación administrativa y la consulta del historial de pagos.
4. Integrar ambos módulos en la plataforma Transportes Génesis con acceso diferenciado por roles (administrador, piloto, monitor y padre de familia).
5. Validar el sistema mediante pruebas de aceptación de usuario (UAT) con representantes del colegio y padres del segmento definido.
6. Evaluar la percepción de confianza, transparencia y satisfacción de los padres participantes tras utilizar el sistema.

---

## 1.5. Planteamiento del problema

Transportes Génesis presta actualmente el servicio de transporte escolar a estudiantes de un colegio en San Cristóbal de las Casas. La operación diaria presenta las siguientes dificultades:

- **Falta de visibilidad del bus:** los padres no tienen forma de saber en tiempo real dónde se encuentra el bus que transporta a sus hijos. Ante retrasos o cambios de ruta, deben comunicarse por teléfono o mensajes, lo cual genera ansiedad, sobre todo en familias con niños en primer grado.
- **Gestión de pagos poco transparente:** el registro y seguimiento de los pagos mensuales se realiza de forma manual o dispersa. Los padres no siempre tienen claridad sobre si su pago fue recibido, validado o rechazado.
- **Procesos no integrados:** la información de rutas, asistencia y pagos no se encuentra centralizada en un solo sistema, lo que dificulta la operación tanto para la empresa como para las familias.
- **Comunicación limitada:** la coordinación entre administración, pilotos, monitores y padres depende de canales informales (WhatsApp, llamadas), propensos a errores y retrasos.

**Pregunta de investigación:**

¿La implementación de un sistema web que integre geolocalización en tiempo real y gestión digital de pagos mejora la confianza, la transparencia y la satisfacción de los padres de familia con hijos en primer grado que utilizan el servicio de Transportes Génesis en San Cristóbal de las Casas?

---

## 1.6. Resumen

Este trabajo propone el desarrollo e implementación de **Transportes Génesis**, una plataforma web para la gestión integral del transporte escolar que integra dos módulos centrales: geolocalización en tiempo real y gestión digital de pagos. El sistema está orientado a padres de familia con hijos en primer grado de un colegio en San Cristóbal de las Casas, segmento en el que la seguridad y la confianza en el servicio son prioritarias.

La investigación parte del problema identificado: procesos manuales, falta de visibilidad del bus y pagos poco transparentes. Como respuesta, se plantea una hipótesis compuesta que vincula ambos módulos con resultados medibles en confianza, transparencia y satisfacción. El objetivo general es desarrollar e implementar la plataforma; los objetivos específicos abarcan el análisis de necesidades, el diseño de cada módulo, su integración, la validación con pruebas UAT y la evaluación de la percepción de los usuarios.

El marco teórico que sustenta este trabajo aborda el transporte escolar, los sistemas de información web, la geolocalización, la gestión digital de pagos y la integración de módulos en plataformas orientadas al usuario. Se espera que los resultados aporten evidencia sobre la utilidad de centralizar estos servicios en una sola herramienta digital para empresas de transporte escolar en contextos similares al de San Cristóbal de las Casas.

---

# Capítulo 2. Marco teórico

## 2.1. Trasfondo del transporte escolar y los sistemas de información

El transporte escolar es un servicio esencial que garantiza el traslado seguro de estudiantes entre su hogar y el centro educativo. En Guatemala, numerosas familias dependen de empresas privadas de transporte escolar, especialmente en ciudades como San Cristóbal de las Casas, donde la dispersión geográfica y las condiciones del tráfico hacen necesario un servicio organizado y confiable.

Tradicionalmente, la gestión de estas empresas se ha apoyado en procesos manuales: hojas de ruta impresas, registros de asistencia en cuadernos, cobros en efectivo y comunicación telefónica con los padres. Estos métodos, aunque funcionales, presentan limitaciones en cuanto a trazabilidad, transparencia y capacidad de respuesta ante imprevistos.

Los **sistemas de información web** surgen como una alternativa que permite centralizar la operación en una plataforma accesible desde cualquier dispositivo con conexión a internet. Según Laudon y Laudon (2020), un sistema de información combina personas, tecnología y procedimientos para recopilar, procesar y distribuir información que apoye la toma de decisiones en una organización. En el contexto del transporte escolar, esto se traduce en una herramienta donde administradores, pilotos, monitores y padres acceden a la información que necesitan en el momento oportuno.

La digitalización del transporte escolar no es un fenómeno aislado. A nivel internacional, diversas plataformas han demostrado que la integración de rastreo GPS, gestión de rutas y comunicación con padres mejora la eficiencia operativa y la satisfacción de las familias (Bureau of Transportation Statistics, 2019). En el contexto local guatemalteco, la adopción de estas tecnologías aún es incipiente, lo que representa una oportunidad para empresas como Transportes Génesis.

---

## 2.2. Geolocalización y seguimiento en tiempo real

La **geolocalización** es la capacidad de determinar la posición geográfica de un objeto —en este caso, un bus escolar— mediante coordenadas de latitud y longitud obtenidas por un receptor GPS (Global Positioning System). Cuando esta información se transmite y actualiza de forma continua, se habla de **seguimiento en tiempo real**.

Para los padres de familia, especialmente aquellos con hijos en los primeros años de educación primaria, conocer la ubicación del bus representa un factor de tranquilidad y confianza. Un estudio de la National Association for Pupil Transportation (2018) señala que la visibilidad del transporte escolar reduce significativamente la ansiedad de los padres durante los horarios de recogida y entrega.

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

---

## 2.4. Integración de módulos en plataformas web

Uno de los retos centrales de este proyecto es que la geolocalización y los pagos no operen como sistemas aislados, sino como **módulos integrados** dentro de una misma plataforma. La integración aporta ventajas claras:

- **Experiencia unificada para el padre:** accede al mapa del bus, confirma asistencia y gestiona sus pagos desde un solo lugar, sin cambiar de aplicación o canal.
- **Datos centralizados para la administración:** usuarios, buses, rutas, pagos y ubicaciones se gestionan en un solo sistema, facilitando reportes y decisiones.
- **Roles diferenciados:** cada tipo de usuario (administrador, piloto, monitor, padre) ve solo lo que le corresponde, lo que refuerza la seguridad y la usabilidad.

La integración de módulos en plataformas web es un principio ampliamente reconocido en el diseño de sistemas de información. Según Sommerville (2016), la modularidad permite desarrollar, probar y mantener cada componente de forma independiente, mientras la integración garantiza que el conjunto funcione como un sistema coherente orientado al usuario.

En Transportes Génesis, esta integración se materializa en una plataforma ASP.NET Core donde el padre accede a su panel principal y desde ahí navega al mapa del bus, a la confirmación de asistencia o a la sección de pagos, todo con la misma sesión de usuario y el mismo diseño visual.

---

## 2.5. Proceso general de implementación y validación del sistema

La implementación de un sistema de información como Transportes Génesis sigue un proceso estructurado que abarca desde el análisis de necesidades hasta la validación con usuarios reales. De forma general, este proceso comprende las siguientes etapas:

| Etapa | Descripción |
|-------|-------------|
| **1. Análisis de necesidades** | Identificar qué necesitan padres, pilotos, monitores y administradores. |
| **2. Diseño** | Definir la arquitectura, los módulos, las pantallas y los flujos de cada rol. |
| **3. Desarrollo** | Construir el sistema con las tecnologías seleccionadas (ASP.NET Core, SQL Server, SignalR, Leaflet). |
| **4. Pruebas internas** | El equipo de desarrollo verifica que cada módulo funcione correctamente. |
| **5. Pruebas de aceptación (UAT)** | Usuarios reales —padres y representantes del colegio— prueban el sistema y dan su opinión. |
| **6. Corrección y cierre** | Se atienden las observaciones y se entrega el sistema validado. |

La **validación con usuarios reales** (UAT) es especialmente relevante en este proyecto, dado que el segmento de estudio son padres con hijos en primer grado, usuarios que no necesariamente tienen familiaridad con herramientas tecnológicas complejas. Las pruebas de aceptación permiten confirmar que el sistema es comprensible, útil y confiable antes de ponerlo en operación con las familias del colegio en San Cristóbal de las Casas.

Adicionalmente, se contemplan **pruebas de carga y rendimiento** para verificar que el sistema responda adecuadamente en momentos de alta demanda, como las horas pico de recogida escolar o los días de inicio de mes cuando muchos padres registran sus pagos simultáneamente.

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
| **San Cristóbal de las Casas** | Municipio del departamento de San Marcos, Guatemala; contexto geográfico del estudio. |
| **Transportes Génesis** | Empresa de transporte escolar para la cual se desarrolla el sistema objeto de esta investigación. |

---

## Referencias bibliográficas sugeridas (a completar con formato de tu universidad)

- Bureau of Transportation Statistics. (2019). *School Transportation Statistics*. U.S. Department of Transportation.
- Laudon, K. C., & Laudon, J. P. (2020). *Management Information Systems: Managing the Digital Firm* (16th ed.). Pearson.
- National Association for Pupil Transportation. (2018). *NAPT Key Industry Facts and Statistics*.
- Sommerville, I. (2016). *Software Engineering* (10th ed.). Pearson.

> **Nota:** Completar las referencias con el formato bibliográfico exigido por tu universidad (APA, Vancouver, etc.) y agregar fuentes locales guatemaltecas si las encuentras.

---

*Documento elaborado para la tesis de Transportes Génesis — julio 2026.*
