---
titulo: Implementación de una plataforma web integrada de geolocalización, registro de abordaje y gestión digital de pagos para Transportes Génesis
universidad: Universidad Galileo
facultad: "[Nombre de la facultad o escuela]"
carrera: "Ingeniería en Ciencias y Sistemas"
autor: "Jonathan Samuel Villeda Pérez y José David Florián Secaida"
carne: "[Número de carné]"
asesor: "[Nombre completo del asesor]"
lugar: Guatemala
fecha: Agosto de 2026
estado: Borrador integral con evidencia audiovisual; cierre UAT pendiente
---

# CARÁTULA

**UNIVERSIDAD GALILEO**

**[NOMBRE DE LA FACULTAD O ESCUELA]**

**INGENIERÍA EN CIENCIAS Y SISTEMAS**

**IMPLEMENTACIÓN DE UNA PLATAFORMA WEB INTEGRADA DE GEOLOCALIZACIÓN, REGISTRO DE ABORDAJE Y GESTIÓN DIGITAL DE PAGOS PARA TRANSPORTES GÉNESIS**

Trabajo de graduación presentado por:

**JONATHAN SAMUEL VILLEDA PÉREZ**

**JOSÉ DAVID FLORIÁN SECAIDA**

Carné: **[NÚMERO DE CARNÉ]**

Previo a optar al grado académico de:

**[GRADO O TÍTULO ACADÉMICO]**

Guatemala, agosto de 2026

<!-- PAGE_BREAK -->

# DECLARACIÓN DEL TRABAJO

Esta tesis fue elaborada por **Jonathan Samuel Villeda Pérez** y **José David Florián Secaida** como requisito para obtener el grado o título de **[GRADO O TÍTULO ACADÉMICO]**.

Guatemala, agosto de 2026.

<!-- PAGE_BREAK -->

# DEDICATORIA

Sección opcional. El autor podrá completar esta página antes de la entrega final.

<!-- PAGE_BREAK -->

# CARTA DE APROBACIÓN DEL ASESOR

**Estado:** pendiente de insertar la carta oficial firmada por el asesor.

<!-- PAGE_BREAK -->

# DICTAMEN DEL DIRECTOR DE PROGRAMAS

**Estado:** pendiente de insertar el dictamen oficial firmado por el Director de Programas.

<!-- PAGE_BREAK -->

# AUTORIZACIÓN DEL DECANO

**Estado:** pendiente de insertar la autorización oficial para publicar el trabajo de graduación.

<!-- PAGE_BREAK -->

# PREFACIO Y AGRADECIMIENTOS

Sección opcional. El autor podrá reconocer la colaboración de Transportes Génesis, del centro educativo, de los padres participantes, del asesor y de las personas que contribuyeron a la investigación.

<!-- PAGE_BREAK -->

# RESUMEN

El presente trabajo documenta el desarrollo y la validación planificada de una plataforma web integrada para Transportes Génesis, empresa dedicada al transporte escolar en San Cristóbal, departamento de San Marcos, Guatemala. El problema se relaciona con la dispersión de información sobre rutas, asistencia, abordaje y pagos, situación que dificulta la trazabilidad operativa y administrativa.

La solución reúne autenticación por roles, administración de buses y rutas, geolocalización en tiempo casi real, confirmación de asistencia, registro operativo de recogidas, alertas dentro del navegador y gestión digital de comprobantes e historial de pagos. La plataforma utiliza ASP.NET Core 8, SQL Server, Entity Framework Core, ASP.NET Core Identity, SignalR, Leaflet, OpenStreetMap y OSRM.

La investigación es aplicada, con alcance descriptivo y evaluativo y enfoque mixto de predominio técnico. La evidencia revisada comprende fotografías y videos de demostraciones controladas con pantallas de administrador, padre de familia y piloto o monitor. El material respalda de forma funcional la gestión de paradas, la visualización cartográfica, el calendario de asistencia y la solicitud de traslados. No contiene un flujo audiovisual de pagos, mediciones antes y después, una escala de satisfacción, una bitácora formal de casos UAT ni un acta de aceptación. Por ello, la hipótesis queda respaldada parcialmente en su dimensión funcional y cualitativa, pero no confirmada en cuanto a reducción de tiempos ni mejora cuantificada de la percepción de seguridad.

**Palabras clave:** transporte escolar, geolocalización, pagos digitales, UAT, trazabilidad, SignalR, pruebas de software.

<!-- PAGE_BREAK -->

# ÍNDICE GENERAL

El índice general se genera automáticamente en la versión Word. Debe actualizarse antes de imprimir o convertir el documento a PDF.

<!-- PAGE_BREAK -->

# ÍNDICE DE CUADROS

El índice de cuadros se genera automáticamente en la versión Word. La numeración sigue el esquema capítulo.secuencia.

<!-- PAGE_BREAK -->

# ÍNDICE DE FIGURAS

El índice de figuras se genera automáticamente en la versión Word. La numeración sigue el esquema capítulo.secuencia.

<!-- PAGE_BREAK -->

<!-- BODY_START -->

# CAPÍTULO 1

# INTRODUCCIÓN

## 1.1 Tema principal

Implementación de una plataforma web integrada de geolocalización, registro de abordaje y gestión digital de pagos para Transportes Génesis, orientada a padres de familia con hijos en primer grado de un colegio en San Cristóbal, departamento de San Marcos.

## 1.2 Contexto de la investigación

El transporte escolar constituye un servicio de apoyo para las familias que necesitan trasladar diariamente a sus hijos entre el hogar y el centro educativo. Su operación requiere coordinar horarios, rutas, paradas, estudiantes, pilotos, monitores y pagos. Cuando estos procesos se administran mediante llamadas telefónicas, mensajes instantáneos, documentos impresos y registros separados, la información puede llegar tarde, duplicarse o resultar difícil de verificar.

Transportes Génesis presta este servicio en San Cristóbal, departamento de San Marcos. Antes de la propuesta tecnológica, la comunicación sobre recorridos y pagos dependía principalmente de procedimientos manuales o canales no integrados. El padre de familia debía consultar por mensaje o llamada la ubicación del bus; la empresa debía revisar información distribuida para determinar asistencias y cobros; y el comprobante de un pago podía quedar separado del historial administrativo.

La necesidad adquiere especial importancia en familias con hijos en primer grado. Durante esta etapa, los padres pueden requerir mayor información sobre el momento de recogida, el traslado y la llegada del estudiante. Sin embargo, una plataforma tecnológica no garantiza por sí misma la seguridad física del niño. Su aporte consiste en ofrecer información oportuna, trazabilidad y registros que puedan aumentar la confianza percibida y apoyar la coordinación del servicio.

La plataforma Transportes Génesis centraliza las funciones principales en un sistema web con acceso por roles. El administrador gestiona usuarios, buses, rutas, paradas y pagos; el piloto y el monitor consultan el recorrido y registran acciones operativas; y el padre visualiza el bus asignado, confirma asistencia, consulta pagos y recibe avisos dentro del navegador. La investigación evaluará si esta integración produce beneficios perceptibles y medibles frente a los procedimientos anteriores.

## 1.3 Hipótesis

**Hipótesis de investigación (H1):**

La integración de los módulos de geolocalización, asistencia y abordaje y pagos, acompañada de pruebas de aceptación de usuario, mejora la eficiencia operativa del transporte escolar y la percepción de seguridad de los padres de familia.

**Hipótesis nula (H0):**

La integración de los módulos de geolocalización, asistencia y abordaje y pagos, acompañada de pruebas de aceptación de usuario, no produce mejoras observables en la eficiencia operativa ni en la percepción de seguridad de los padres de familia.

La eficiencia se entiende como la capacidad de completar tareas operativas dentro de una plataforma integrada, con menor fragmentación de información. La seguridad se interpreta como percepción de información y control, no como garantía de seguridad física. Para confirmar una mejora se requieren mediciones comparables o evidencia de aceptación estructurada. Las fotografías, videos y comentarios cualitativos permiten valorar funcionamiento y utilidad aparente, pero no sustituyen tiempos antes y después, escalas de satisfacción ni un acta UAT.

**Cuadro 1.1. Criterios de evaluación de la hipótesis**

| Componente | Evidencia principal | Criterio propuesto |
|---|---|---|
| Geolocalización | Video, fotografías y registro técnico | Mapa, ruta y actualización de ubicación observables |
| Asistencia y abordaje | Video, fotografías y persistencia | Confirmación asociada a fecha, turno y estudiante |
| Pagos | Flujo UAT, historial y reporte | Registro, validación e historial sin defecto crítico |
| UAT | Casos, participantes, resultados y acta | Ejecución por usuarios representativos con evidencia trazable |
| Eficiencia | Tiempos o pasos antes y después | Reducción observable y documentada |
| Seguridad percibida | Instrumento o retroalimentación estructurada | Mejora reportada sin presentarla como seguridad física |

## 1.4 Objetivos

### 1.4.1 Objetivo general

Evaluar si la integración de geolocalización, asistencia y abordaje, pagos y pruebas UAT mejora la eficiencia operativa del transporte escolar y la percepción de seguridad de los padres de familia.

### 1.4.2 Objetivos específicos

1. Implementar geolocalización en tiempo casi real para el monitoreo de buses y rutas.
2. Desarrollar el módulo de asistencia y abordaje para el control operativo de estudiantes.
3. Integrar el sistema de pagos con el modelo de datos, sus migraciones y la trazabilidad administrativa.
4. Validar el sistema mediante pruebas UAT con padres de familia, pilotos, monitores y personal administrativo.

## 1.5 Planteamiento del problema

La operación de un transporte escolar produce información que cambia durante el día. El bus inicia un recorrido, visita paradas, recoge o deja estudiantes y puede experimentar retrasos. Paralelamente, la administración necesita conocer quién utilizará el servicio y mantener control de pagos. Cuando la información se distribuye entre llamadas, mensajes y registros separados, resulta difícil reconstruir el estado de un recorrido o de un pago.

La falta de una fuente centralizada limita la trazabilidad. Un padre puede desconocer la ubicación reportada del bus; el personal puede tener dificultades para confirmar asistencia o abordaje; y un comprobante puede quedar separado de su validación e historial. La existencia de un prototipo no demuestra por sí sola que los flujos funcionen de manera aceptable en condiciones representativas.

Por lo anterior, la investigación responde la siguiente pregunta:

**¿En qué medida la integración de geolocalización, asistencia y abordaje, pagos y pruebas UAT mejora la eficiencia operativa del transporte escolar y la percepción de seguridad de los padres de familia?**

## 1.6 Justificación

### 1.6.1 Justificación social

La información sobre el recorrido puede contribuir a que las familias organicen mejor los horarios de recogida y entrega. El registro de asistencia y abordaje proporciona evidencia operativa adicional. El beneficio esperado no debe describirse como garantía absoluta de seguridad, sino como mejora en información, confianza percibida y coordinación.

### 1.6.2 Justificación económica

La posibilidad de registrar y consultar pagos de forma remota puede reducir actividades administrativas y ciertos desplazamientos. Esta tesis no cuantificará ahorros económicos sin datos verificables; validará que el flujo digital permita registrar, revisar y consultar el estado de un pago.

### 1.6.3 Justificación operativa

La centralización reduce la fragmentación de información entre llamadas, mensajes, documentos y hojas separadas. Los roles permiten que cada participante consulte o registre la información necesaria. La trazabilidad de ubicaciones, recogidas y pagos apoya el seguimiento administrativo.

### 1.6.4 Justificación tecnológica

El proyecto demuestra la aplicación de tecnologías web disponibles para construir una solución local: ASP.NET Core para el servidor, SQL Server para persistencia, SignalR para comunicación en tiempo real y herramientas cartográficas abiertas para visualizar rutas y ubicaciones.

### 1.6.5 Justificación académica

La investigación vincula una solución de ingeniería de software con un protocolo de evaluación. La hipótesis, las variables, los instrumentos y los criterios de éxito se definen antes de observar los resultados. Esto permite presentar evidencia favorable, desfavorable o mixta con el mismo criterio de honestidad académica.

## 1.7 Delimitación

**Cuadro 1.2. Delimitación del estudio**

| Dimensión | Delimitación |
|---|---|
| Geográfica | San Cristóbal, departamento de San Marcos, Guatemala |
| Poblacional | Padres con hijos en primer grado de un colegio atendido por Transportes Génesis |
| Temporal | Desarrollo y validación durante 2026 |
| Tecnológica | Plataforma web adaptable; no incluye aplicación móvil nativa |
| Funcional | Geolocalización, rutas, asistencia, abordaje, pagos, alertas web y administración |
| Institucional | Transportes Génesis y el colegio participante, cuyo nombre puede reservarse |

## 1.8 Alcance

El trabajo comprende el análisis, diseño, implementación y validación inicial de una plataforma web. Incluye autenticación por roles, administración de buses y rutas, visualización cartográfica, recepción y almacenamiento de ubicaciones, comunicación SignalR, confirmación de asistencia, registro operativo de recogidas, gestión de comprobantes e historial de pagos y pruebas con usuarios.

La evaluación incluye casos funcionales, pruebas UAT, observación de tareas, automatización con Selenium y pruebas de carga y rendimiento en ambiente controlado. Las evidencias audiovisuales y técnicas se referenciarán mediante enlaces.

## 1.9 Limitaciones

1. La versión evaluada es web y no incluye una aplicación nativa para Android o iOS.
2. Las alertas funcionan dentro del navegador y no constituyen notificaciones push, SMS o WhatsApp.
3. La transmisión demostrable depende del navegador del piloto o monitor y del entorno de simulación; no utiliza un dispositivo GPS dedicado instalado permanentemente en el bus.
4. El registro de abordaje es efectuado por el personal; no utiliza QR, NFC, RFID ni biometría.
5. El pago con pasarela es complementario. La evaluación principal se enfoca en comprobantes e historial.
6. La muestra puede ser pequeña y no permite generalizar automáticamente los resultados a toda Guatemala.
7. La conectividad, precisión del dispositivo y disponibilidad de servicios cartográficos externos pueden afectar la experiencia.
8. Los resultados definitivos dependen de ejecutar el protocolo con usuarios reales y no pueden inferirse solo del funcionamiento técnico.

## 1.10 Resumen del capítulo

El capítulo presentó el contexto, el problema, la hipótesis verificable y los objetivos. La propuesta evaluará cumplimiento funcional, aceptación operativa y rendimiento sin afirmar que la tecnología garantiza la seguridad ni atribuir beneficios no medidos.

# CAPÍTULO 2

# MARCO TEÓRICO

## 2.1 Sistemas de información y digitalización de procesos

Un sistema de información integra personas, procedimientos, datos y tecnología para apoyar operaciones y decisiones. La calidad de la solución no depende únicamente del software: también requiere procesos definidos, datos confiables y usuarios capaces de utilizarla. En Transportes Génesis, la plataforma sustituye parcialmente la dispersión de información por un punto común de consulta y registro.

La digitalización consiste en rediseñar actividades para que la información pueda capturarse, almacenarse, procesarse y recuperarse de manera sistemática. En un servicio de transporte escolar, esto abarca asignaciones, rutas, paradas, ubicaciones, asistencia, recogidas y pagos. La integración evita que cada módulo sea una aplicación aislada, aunque internamente conserve responsabilidades separadas.

## 2.2 Transporte escolar y confianza de los padres

La confianza es una expectativa sobre el comportamiento de un servicio y sobre la calidad de la información recibida. En este estudio se entiende como percepción del padre respecto de la oportunidad, claridad y utilidad de la información que proporciona Transportes Génesis. No equivale a ausencia de riesgo ni reemplaza los protocolos físicos de seguridad.

La incertidumbre aparece cuando el usuario no dispone de información suficiente para anticipar o comprender una situación. Durante un recorrido escolar puede expresarse en dudas sobre la ubicación del bus, el retraso o el abordaje. Un mapa actualizado y un registro operativo pueden reducir la necesidad de consultas, siempre que los datos sean comprensibles y recientes.

## 2.3 Geolocalización

La geolocalización permite determinar la posición de un dispositivo mediante coordenadas. La API de geolocalización del navegador expone esta capacidad con autorización del usuario y en un contexto seguro. La precisión depende del hardware, la red y el entorno; por ello, una coordenada debe acompañarse de fecha y hora y no considerarse exacta en todo momento (World Wide Web Consortium, 2024).

El término «tiempo real» suele utilizarse en aplicaciones web para describir actualizaciones recibidas sin recargar la pantalla. En este proyecto es más preciso hablar de tiempo casi real: el dispositivo envía ubicaciones periódicas al servidor, este las almacena y comunica a los clientes conectados. Siempre existe una latencia producida por captura, red, procesamiento y representación.

### 2.3.1 Representación cartográfica

Leaflet es una biblioteca de JavaScript para mapas interactivos. OpenStreetMap aporta datos cartográficos abiertos; OSRM calcula recorridos sobre la red vial; y Nominatim permite búsquedas geográficas. Estas piezas cumplen funciones distintas. El mapa base no calcula por sí mismo la ruta, y una ruta sugerida no reemplaza el criterio del piloto ni las condiciones reales de tránsito.

### 2.3.2 Distancia y alertas de proximidad

La distancia entre dos coordenadas puede aproximarse con la fórmula de Haversine. Transportes Génesis utiliza radios de referencia para generar avisos cuando el bus se aproxima a la casa del alumno o al colegio. Estas alertas dependen de la ubicación reportada y de que el padre mantenga una sesión web activa.

### 2.3.3 Comunicación en tiempo real

SignalR facilita la comunicación bidireccional entre servidor y clientes web. El servidor puede enviar eventos a conexiones o grupos sin que el navegador consulte repetidamente. En Génesis, la ubicación se recibe mediante una API REST, se almacena y luego se distribuye por SignalR. Esta separación permite conservar trazabilidad y actualizar el mapa.

## 2.4 Registro de asistencia y abordaje

La asistencia anticipada informa si el estudiante utilizará el servicio en un turno. El abordaje registra una acción observada durante el recorrido. Son datos relacionados pero distintos: una confirmación previa no prueba que el estudiante haya subido, y un registro de recogida depende de la actuación del monitor o piloto.

La trazabilidad requiere identificar fecha, estudiante, parada, estado y responsable del registro. También demanda controles de acceso, porque la información de un menor no debe mostrarse a usuarios ajenos a su relación autorizada.

## 2.5 Gestión digital de pagos

La gestión de pagos comprende asignación de montos, registro, evidencia, validación e historial. En un flujo con comprobante, el padre carga una imagen o documento; la administración verifica la información y establece un estado. La trazabilidad permite consultar el mes, monto, fecha, tipo y resultado de validación.

Un pago digital no elimina automáticamente todos los costos. Puede reducir desplazamientos cuando el procedimiento se completa de forma remota, pero todavía puede existir el costo de conectividad, de una transferencia bancaria o de una comisión. Por esa razón, este estudio compara el costo declarado del proceso anterior con el costo asociado al uso del sistema.

### 2.5.1 Seguridad del flujo de pagos

La aplicación debe validar archivos, restringir tipos y tamaños, proteger credenciales y aplicar autorización. Cuando interviene una pasarela, los datos sensibles de tarjeta deben procesarse mediante componentes del proveedor y bajo prácticas como las del PCI Security Standards Council (2024). La tesis no debe presentar el almacenamiento local de comprobantes como equivalente a una certificación de seguridad.

## 2.6 Integración de módulos y arquitectura web

La arquitectura de Génesis separa presentación, controladores o páginas, servicios, acceso a datos y comunicación en tiempo real. La modularidad facilita asignar responsabilidades y probar componentes, mientras que la integración ofrece una sesión y una experiencia unificadas.

ASP.NET Core Identity administra usuarios, autenticación y roles. Entity Framework Core relaciona objetos con tablas de SQL Server. Las API reciben datos estructurados; Razor Pages y MVC generan interfaces; SignalR envía eventos a clientes conectados.

## 2.7 Usabilidad y aceptación operativa

La usabilidad comprende efectividad, eficiencia y satisfacción en un contexto de uso. En este trabajo se observarán especialmente la finalización de tareas, los errores, la ayuda requerida y el tiempo empleado. Una función puede ser técnicamente correcta y, aun así, impedir que el usuario complete un flujo.

La UAT permite que los usuarios finales ejecuten escenarios representativos y determinen si el resultado es aceptable para su operación. El investigador registra hechos observables y comentarios espontáneos, pero no sustituye la decisión del usuario ni ejecuta las tareas en su lugar.

## 2.8 Selección de métodos sin encuesta

La guía de validación consultada establece que no es necesario aplicar todos los métodos disponibles y recomienda seleccionar dos o tres que se complementen, incluyendo uno técnico y uno con usuarios reales. Por ello, la estrategia combina UAT y observación directa con pruebas funcionales automatizadas y carga-rendimiento.

La encuesta es útil cuando se busca cuantificar percepciones en una muestra amplia. En esta tesis se omite porque la disponibilidad prevista de participantes es reducida y el objetivo reformulado se concentra en cumplimiento funcional y aceptación operativa. Esta decisión evita presentar promedios de satisfacción con alcance estadístico insuficiente. Los comentarios de usuarios se conservarán como evidencia cualitativa anonimizada y no se convertirán en porcentajes de opinión.

## 2.9 Pruebas de software

Las pruebas funcionales verifican que las acciones produzcan el resultado esperado. Las pruebas de integración comprueban la interacción entre componentes. Las pruebas UAT permiten que usuarios representativos determinen si el sistema satisface necesidades operativas. Las pruebas de carga examinan el comportamiento ante concurrencia, y las de rendimiento miden latencia, tasa de errores y estabilidad.

La calidad debe evaluarse con criterios definidos antes de ejecutar. Para Génesis se proponen tiempos de respuesta, estabilidad de actualización, éxito de tareas, claridad de pagos y percepción de utilidad.

## 2.10 Protección de datos y ética

La geolocalización y los datos de estudiantes requieren precaución. Los participantes deben conocer qué información se recolectará, para qué se utilizará, por cuánto tiempo se conservará y quién podrá acceder. La investigación utilizará códigos en lugar de nombres en la base de análisis.

El consentimiento debe ser voluntario. Un padre puede retirarse sin afectar el servicio. Las capturas utilizadas como evidencia deben ocultar nombres, direcciones, credenciales y datos financieros. Las cuentas de demostración no deben exponer secretos reales.

## 2.11 Definiciones

**Cuadro 2.1. Términos principales**

| Término | Definición operacional |
|---|---|
| Geolocalización | Obtención de latitud y longitud desde un dispositivo autorizado |
| Tiempo casi real | Actualización con una latencia breve y medible, sin promesa de inmediatez absoluta |
| Abordaje | Registro manual de presencia o recogida efectuado por personal autorizado |
| Trazabilidad | Capacidad de reconstruir eventos mediante registros con fecha, estado y responsable |
| Pago digital | Registro o procesamiento de un pago mediante la plataforma |
| Confianza percibida | Valoración del padre sobre oportunidad y utilidad de la información |
| Incertidumbre | Falta de información suficiente respecto del traslado |
| UAT | Prueba de aceptación ejecutada por usuarios representativos |
| SignalR | Biblioteca para comunicación en tiempo real entre servidor y cliente |
| Rol | Conjunto de permisos asignado a un tipo de usuario |

## 2.12 Resumen del capítulo

El marco teórico relaciona sistemas de información, geolocalización, trazabilidad, pagos, usabilidad y evaluación. La tecnología aporta datos y canales de consulta, pero sus beneficios deben comprobarse con usuarios y métricas.

# CAPÍTULO 3

# MARCO METODOLÓGICO

## 3.1 Enfoque y tipo de investigación

La investigación es aplicada porque desarrolla y evalúa una solución para una necesidad concreta de Transportes Génesis. Tiene alcance descriptivo y evaluativo y un enfoque mixto de predominio técnico. Los casos UAT, tiempos, defectos y métricas de rendimiento aportan datos cuantitativos; las observaciones de tareas y comentarios espontáneos aportan evidencia cualitativa.

El diseño corresponde a un estudio de caso tecnológico con validación en ambiente controlado cercano al uso real. No se emplea un diseño preexperimental de percepción ni se aplican encuestas. La unidad evaluada es el sistema en interacción con usuarios representativos y con una carga simulada definida.

## 3.2 Unidades de análisis y participantes

La unidad técnica de análisis es la versión del sistema desplegada para pruebas. Las unidades de observación humana son representantes de los roles padre de familia, administrador, piloto y monitor. Los usuarios finales ejecutarán tareas UAT; el equipo técnico preparará el ambiente y registrará evidencia sin tomar el control de las acciones del participante.

La cantidad final de participantes, sus roles, las fechas y el lugar permanecen pendientes. La guía orientativa sugiere de cinco a diez participantes para pruebas cualitativas; si no se alcanza ese número, se declarará como limitación y no se generalizarán los resultados.

## 3.3 Categorías de evaluación

**Cuadro 3.1. Operacionalización de la evaluación**

| Categoría | Dimensión | Indicador | Instrumento |
|---|---|---|---|
| Cumplimiento funcional | Requisitos críticos | Casos aprobados, con observaciones y rechazados | Matriz UAT |
| Usabilidad operativa | Ejecución de tareas | Éxito, tiempo, errores y ayuda requerida | Hoja de observación |
| Trazabilidad | Integridad del registro | Correspondencia entre requisito, prueba, defecto y evidencia | Matriz de trazabilidad |
| Control por roles | Autorización funcional | Accesos permitidos y denegados | Casos positivos y negativos |
| Rendimiento | Latencia y capacidad | Promedio, percentil 95, throughput y tasa de error | Script y reporte de carga |
| Estabilidad | Continuidad del mapa | Duración sin caída y pérdidas observadas | Logs, video y reporte |
| Calidad de corrección | Gestión de defectos | Severidad, estado y regresión | Bitácora de defectos |

## 3.4 Criterios de éxito

1. Al menos 80 % de tareas UAT completadas sin ayuda crítica.
2. Ningún defecto crítico abierto al momento del cierre.
3. Acceso correcto por rol en todos los casos de autorización ejecutados.
4. Persistencia correcta de asistencia por fecha, turno y estudiante en los casos aprobados.
5. Flujo de pago completo: registro, validación e historial sin defecto crítico.
6. Mapa estable durante treinta minutos de simulación controlada.
7. Percentil 95 menor de tres segundos y tasa de error inferior al 1 % en el escenario de carga aprobado.
8. Scripts automatizados críticos aprobados en tres ejecuciones consecutivas.
9. Cada resultado reportado debe contar con evidencia enlazada y fecha de ejecución.

Los umbrales son criterios de proyecto sujetos a aprobación del asesor y deben fijarse antes de analizar los resultados.

## 3.5 Técnicas e instrumentos

### 3.5.1 Revisión documental

Se revisarán requisitos, código, modelo de datos, manuales, bitácoras y documentación operativa para describir la solución y construir la trazabilidad.

### 3.5.2 Pruebas de aceptación de usuario

Cada usuario ejecutará escenarios definidos con precondiciones, pasos y resultado esperado. El caso se clasificará como aprobado, aprobado con observaciones, rechazado o bloqueado. Se registrarán tiempo, ayuda, resultado observado y enlace a evidencia anonimizada.

### 3.5.3 Observación directa

El observador anotará errores, dudas, pausas, necesidad de ayuda y comentarios espontáneos. No utilizará preguntas de escala ni convertirá comentarios aislados en resultados estadísticos.

### 3.5.4 Automatización y pruebas técnicas

Los flujos repetibles podrán automatizarse con Selenium. Las pruebas de carga usarán la herramienta que entregue el responsable, por ejemplo JMeter o k6. Se conservarán scripts, configuración, versión del sistema, hardware, red, fecha y reportes.

## 3.6 Validez y trazabilidad

El protocolo será revisado por el asesor y por un representante operativo antes de ejecutarse. Cada objetivo se relacionará con requisitos, casos de prueba, criterios y evidencias. La misma versión del sistema y los mismos datos controlados deberán utilizarse en las repeticiones comparables.

Una evidencia será admisible cuando identifique fecha, ambiente, versión, caso y resultado; cuando no exponga datos personales; y cuando el enlace permita al revisor acceder al archivo autorizado. Los videos y fotografías serán evidencia de apoyo, no sustitutos del registro estructurado.

## 3.7 Procedimiento

1. Obtener autorización institucional y consentimiento de los participantes.
2. Confirmar versión, ambiente, roles, datos ficticios, responsables y fechas.
3. Aprobar los escenarios y criterios antes de ejecutar.
4. Realizar una prueba seca con uno o dos usuarios.
5. Ejecutar pruebas internas y corregir defectos críticos.
6. Ejecutar UAT con usuarios finales sin que el desarrollador controle el dispositivo.
7. Registrar resultado, tiempo, ayuda, observaciones y evidencia de cada caso.
8. Ejecutar scripts automatizados y pruebas de carga-rendimiento.
9. Corregir defectos y repetir las pruebas de regresión necesarias.
10. Consolidar enlaces, bitácora y acta de aceptación.
11. Analizar resultados contra los criterios definidos.
12. Redactar conclusiones que respondan a cada objetivo sin inventar datos faltantes.

## 3.8 Escenarios UAT

**Cuadro 3.2. Escenarios de aceptación**

| ID | Rol | Tarea | Resultado esperado | Evidencia |
|---|---|---|---|---|
| UAT-01 | Padre | Iniciar sesión | Acceso solo a funciones autorizadas | Video o capturas |
| UAT-02 | Padre | Abrir mapa y usar controles | Ruta, ubicación y zoom visibles | Video |
| UAT-03 | Padre | Confirmar asistencia para una fecha | Registro limitado a fecha y turno elegidos | Video y consulta |
| UAT-04 | Monitor | Registrar recogida o presencia | Registro asociado al estudiante correcto | Video y consulta |
| UAT-05 | Piloto/monitor | Iniciar, pausar y reanudar ruta | Conserva posición y motivo operativo | Video y logs |
| UAT-06 | Padre | Registrar comprobante | Pago queda pendiente de validación | Video y registro |
| UAT-07 | Administrador | Validar o rechazar pago | Estado actualizado con trazabilidad | Video y registro |
| UAT-08 | Padre | Consultar historial | Mes, monto y estado identificables | Video |
| UAT-09 | Administrador | Gestionar bus, ruta y asignación | Cambios persistidos correctamente | Video y consulta |
| UAT-10 | Todos | Ejecutar flujo integrado | Módulos operan con roles correctos | Video de cierre |

## 3.9 Plan de análisis

La tasa de éxito UAT se calculará como casos aprobados divididos entre casos ejecutados, multiplicado por cien. Los casos bloqueados se reportarán por separado y no se ocultarán. También se presentarán tiempos por tarea, cantidad de ayudas, defectos por severidad y estado de regresión.

Las pruebas de carga reportarán usuarios virtuales, duración, rampa, throughput, latencia promedio, percentil 95, tasa de error y recursos del servidor. Cada métrica se comparará con el criterio fijado antes de ejecutar.

Los comentarios se agruparán cualitativamente por módulo y tipo de problema. No se informará “porcentaje de usuarios satisfechos” ni otra inferencia de percepción, porque no se aplicará encuesta.

## 3.10 Ética

Se solicitará consentimiento informado. No se publicarán nombres, direcciones, coordenadas del hogar, teléfonos, credenciales ni comprobantes reales. Los participantes se identificarán mediante códigos. Las capturas, fotografías y videos deberán anonimizarse antes de compartir sus enlaces.

## 3.11 Amenazas a la validez

La disponibilidad reducida de usuarios puede limitar la diversidad de escenarios. El ambiente controlado no reproduce todas las condiciones de una ruta real. La presencia del observador puede modificar el comportamiento. La simulación de ubicación no equivale a un GPS dedicado. Los enlaces externos pueden perder disponibilidad. Estas amenazas se describirán junto con los resultados.

## 3.12 Resumen del capítulo

La metodología combina un método con usuarios reales —UAT y observación— con métodos técnicos —automatización, carga y rendimiento—. Esta combinación permite omitir encuestas sin dejar la tesis sin validación empírica, siempre que las pruebas se ejecuten y la evidencia sea verificable.

# CAPÍTULO 4

# DISEÑO E IMPLEMENTACIÓN DE LA SOLUCIÓN

## 4.1 Descripción general

Transportes Génesis es una aplicación web desarrollada sobre .NET 8. Reúne funciones administrativas y operativas con interfaces diferenciadas. La solución utiliza una base de datos relacional y comunicación en tiempo real para el mapa.

## 4.2 Usuarios y roles

**Cuadro 4.1. Roles del sistema**

| Rol | Responsabilidades |
|---|---|
| Administrador | Gestionar usuarios, buses, rutas, paradas, asignaciones, pagos y reportes |
| Piloto | Consultar ruta asignada y transmitir o simular ubicación |
| Monitor | Consultar ruta, revisar estudiantes y registrar recogidas |
| Padre de familia | Consultar mapa, confirmar asistencia, gestionar pagos y solicitudes |

ASP.NET Core Identity autentica a los usuarios. Las políticas por rol reducen la exposición de pantallas, aunque deben complementarse con validaciones sobre la propiedad de los datos.

## 4.3 Arquitectura

**Figura 4.1. Flujo general de la plataforma**

```text
Usuarios web
    │
    ▼
Razor Pages / MVC / API REST
    │
    ├── Servicios de negocio
    ├── ASP.NET Core Identity
    ├── SignalR
    ▼
Entity Framework Core
    ▼
SQL Server

Servicios cartográficos externos: OpenStreetMap, OSRM y Nominatim
```

La arquitectura combina Razor Pages y MVC por la evolución del proyecto. Las API atienden ubicaciones y rutas. Los servicios encapsulan reglas de negocio y Entity Framework Core gestiona persistencia.

## 4.4 Tecnologías

**Cuadro 4.2. Tecnologías principales**

| Capa | Tecnología | Propósito |
|---|---|---|
| Servidor | ASP.NET Core / .NET 8.0 | Aplicación, páginas, controladores y API |
| Persistencia | SQL Server (versión del entorno pendiente) | Almacenamiento relacional |
| Acceso a datos | Entity Framework Core 8.0.10 | Mapeo y consultas |
| Seguridad | ASP.NET Core Identity 8.0.10 | Usuarios y roles |
| Tiempo real | SignalR para ASP.NET Core 8 | Eventos hacia navegadores |
| Mapa | Leaflet 1.9.4 y OpenStreetMap | Visualización |
| Rutas | OSRM, servicio público consultado | Trazado sobre calles |
| Geocodificación | Nominatim, servicio público consultado | Búsqueda de direcciones |
| Pagos en línea | Stripe.net 51.1.0 | Flujo complementario sujeto a configuración |
| Interfaz | Bootstrap 5.3.0, JavaScript y jQuery 3.7.0 | Presentación adaptable |

## 4.5 Módulo de geolocalización

El piloto o monitor utiliza una pantalla web que puede capturar ubicación continua mediante la API de geolocalización del navegador con `watchPosition`. El proyecto también conserva una modalidad de simulación que envía una coordenada aproximadamente cada dos segundos. La API recibe identificador de bus, latitud, longitud y datos relacionados; el servicio almacena el registro y emite un evento SignalR.

**Figura 4.2. Flujo de una ubicación**

```text
Piloto o monitor
      │ POST /api/ubicaciones
      ▼
UbicacionesController
      ▼
UbicacionBusService
      ├── Guarda en SQL Server
      ├── Calcula proximidad
      └── Emite SignalR
                  ▼
          Mapa del padre
```

El sistema identifica una ubicación como reciente según reglas temporales. Puede clasificar el bus como detenido, en movimiento o sin señal. Las alertas se calculan por distancia aproximada. No se utilizan geocercas poligonales.

## 4.6 Rutas y paradas

Las rutas se organizan por bus y turno. El sistema ordena paradas mediante una heurística de vecino más cercano. Este procedimiento busca una solución práctica, pero no garantiza la ruta globalmente óptima. OSRM dibuja el trayecto sobre calles disponibles.

El administrador mantiene paradas y asignaciones. El piloto y monitor consultan el recorrido. La información de asistencia puede modificar la lista de estudiantes esperados, con comportamientos de respaldo para ambientes de demostración.

## 4.7 Asistencia y abordaje

El proyecto contiene la interfaz, el modelo y la API para confirmar si el estudiante utilizará el transporte por la mañana o por la tarde. Sin embargo, la vista actual incluye comportamiento de demostración y la API puede devolver respuestas simuladas ante determinados errores. Por ello, la confirmación no se considerará funcionalmente validada hasta comprobar que el dato se persiste y se recupera desde SQL Server durante UAT. Al completar una parada se crea un registro de recogida; el monitor también dispone de una interfaz para marcar presencia o ausencia.

El término «registro de abordaje» se utiliza como concepto operativo. El sistema no verifica identidad mediante hardware y depende de la acción del usuario autorizado. Esta limitación será comunicada a los participantes.

## 4.8 Alertas

SignalR distribuye eventos de ubicación, proximidad, parada completada y retraso. El padre debe mantener el navegador conectado. Algunas alertas automáticas se muestran en tiempo real, pero no todas se almacenan en el historial. Por ello, la tesis distinguirá entre recepción en pantalla y persistencia.

## 4.9 Gestión de pagos

El padre consulta meses y montos, registra un pago o carga una boleta y revisa el historial. La administración puede validar o rechazar registros según el flujo activo. El proyecto contiene modelos de pagos de distintas etapas, por lo que la validación se enfocará en el flujo visible y utilizado durante UAT. La lógica actual de meses deberá probarse para los doce meses del año, pues la lista configurada no cubre de forma segura todos los casos; no se afirmará gestión mensual completa mientras esta prueba no sea aprobada.

La pasarela Stripe se considera complementaria. Si no se encuentra habilitada con credenciales de prueba, se documentará su exclusión y no se presentará como resultado validado.

## 4.10 Datos principales

**Cuadro 4.3. Entidades relevantes**

| Entidad | Finalidad |
|---|---|
| Bus | Vehículo y capacidad |
| Ruta | Recorrido por bus y turno |
| Parada | Punto, orden y estudiante relacionado |
| UbicacionBusEnTiempoReal | Coordenada y fecha reportada |
| Alumno | Estudiante asociado a responsable |
| Padre | Responsable del estudiante |
| AsistenciaAlumno | Confirmación diaria por turno |
| RegistroRecogida | Evidencia operativa de parada |
| PagoPadre | Pago web, mes, monto, tipo y comprobante |
| Usuario y rol | Autenticación y autorización |

## 4.11 Controles de seguridad

La solución utiliza autenticación y restricciones por rol en varias páginas, pero la cobertura de autorización no es completa en todas las API, acciones administrativas y grupos SignalR. En consecuencia, el control de acceso se tratará como requisito pendiente de endurecimiento y deberá someterse a casos negativos antes de aprobarse. También se requiere conexión segura, validación de entrada y control de archivos. Las contraseñas no deben aparecer en la tesis. Los datos de demostración deben ser ficticios. Los secretos de Stripe o conexión no deben quedar en el repositorio ni en capturas.

## 4.12 Restricciones observadas

1. La captura con `watchPosition` depende del permiso, precisión y conectividad del dispositivo; la simulación permanece como modalidad alternativa de demostración y debe distinguirse de una ruta real.
2. Algunas rutas de demostración contienen valores de bus preconfigurados.
3. Existen respuestas de respaldo o simulación en determinados flujos.
4. Las notificaciones no salen del navegador.
5. Algunos modelos y pantallas de pagos reflejan etapas diferentes del desarrollo.
6. La integración se da en una plataforma y sesión comunes; geolocalización y pagos conservan flujos funcionales separados.
7. La persistencia de asistencia debe verificarse porque la interfaz y la API contienen comportamiento de simulación.
8. Determinadas API, acciones y suscripciones SignalR requieren controles de autorización adicionales.
9. El cálculo de meses del módulo de pagos debe corregirse o validarse para enero, noviembre y diciembre.

Estas restricciones no invalidan el prototipo, pero delimitan las afirmaciones y el diseño de prueba.

## 4.13 Resumen del capítulo

El capítulo documentó la arquitectura y el funcionamiento realmente implementado. Se evitó presentar simulaciones, funciones complementarias o trabajos futuros como capacidades plenamente validadas.

# CAPÍTULO 5

# VALIDACIÓN Y RESULTADOS

## 5.1 Propósito y criterio de interpretación

Este capítulo presenta los resultados que pueden comprobarse mediante el material audiovisual entregado para la investigación. La revisión distingue entre funcionamiento visible, retroalimentación cualitativa reportada y métricas que todavía no están disponibles. No se asignan porcentajes de aprobación ni reducciones de tiempo que no hayan sido medidos.

La evidencia demuestra pantallas y flujos en un ambiente controlado. Por sí sola no acredita una UAT completa, porque no incluye matriz firmada de casos, cantidad e identificación anonimizada de participantes, tiempos antes y después, bitácora de defectos ni acta de aceptación.

## 5.2 Repositorio de evidencia audiovisual

**REPOSITORIO AUDIOVISUAL INSTITUCIONAL — ACCESO PARA EL GRUPO DE LA UNIVERSIDAD:**

**https://drive.google.com/drive/folders/1d1_k0cRqK9md96MxalGPg3LT3fl4RPZg?usp=sharing**

La copia de trabajo se conserva en `Documentos/Tesis/imagenes _documentos_transporte_genesis`. Se revisaron diez archivos de imagen y cinco videos. La fotografía y el video de piloto y monitor están duplicados en ambas carpetas; para evitar doble conteo se consideraron ocho fotografías y cuatro videos únicos. Las duraciones aproximadas de los videos únicos son 15.81, 7.40, 33.67 y 37.97 segundos.

**Cuadro 5.1. Inventario de evidencia revisada**

| Código | Rol o módulo | Evidencia observable | Resultado de revisión |
|---|---|---|---|
| ADM-01 | Administrador | Panel con mapa, rutas, traslados, usuarios, asignaciones y paradas | Pantalla disponible |
| ADM-02 | Administrador | Formulario para crear parada con dirección, coordenadas, orden y estado | Flujo visible |
| ADM-03 | Administrador | Mapa con paradas registradas y acciones de edición | Flujo visible |
| PM-01 | Piloto/monitor | Mapa con ruta, paradas ordenadas y destino | Pantalla disponible |
| PM-02 | Piloto/monitor | Video de marcador de bus desplazándose sobre el mapa | Actualización visual demostrada |
| PAD-01 | Padre | Inicio de sesión | Pantalla disponible |
| PAD-02 | Padre | Panel con pagos, historial, ruta, asistencia y traslados | Integración de accesos visible |
| PAD-03 | Padre | Calendario con estados por fecha y turno | Flujo visible |
| PAD-04 | Padre | Solicitud de traslado temporal | Flujo visible |
| PAD-05 | Padre | Mapa de ruta y controles de visualización | Flujo visible |
| PAD-06 | Padre | Video de mapa con cambios de vista | Interacción cartográfica demostrada |
| PAD-07 | Padre | Video de calendario y traslado | Selección y formulario demostrados |
| PAD-08 | Padre | Video de confirmación de asistencia | Estados por fecha y turno visibles |

## 5.3 Resultados de geolocalización y rutas

El panel administrativo evidencia acceso a mapas, rutas, asignaciones y gestión de paradas. Las capturas ADM-02 y ADM-03 muestran que una parada puede definirse mediante dirección, latitud, longitud, orden y estado activo, y que las paradas registradas se representan sobre el mapa.

La evidencia PM-01 presenta una ruta con paradas numeradas y destino final. El video PM-02, de aproximadamente 15.81 segundos, muestra el cambio de posición del marcador del bus sobre el mapa. Este comportamiento respalda funcionalmente la representación dinámica de la ubicación en una demostración controlada.

El video PAD-06, de aproximadamente 7.40 segundos, muestra la pantalla cartográfica del padre y cambios en el área visible del mapa. Esto confirma la disponibilidad de interacción cartográfica. Sin embargo, los videos no muestran simultáneamente las pantallas de administrador, padre y piloto o monitor; por ello, no permiten comprobar por sí solos la sincronización SignalR entre los tres roles, la precisión geográfica ni la latencia de actualización.

## 5.4 Resultados de asistencia, abordaje y traslados

La evidencia PAD-03 muestra un calendario con estados diferenciados por fecha y por ruta matutina o vespertina. Los videos PAD-07 y PAD-08 muestran selección de fechas, confirmación por turno, cambio de mes y visualización de estados. La evidencia es consistente con la corrección solicitada para que una confirmación se asocie al día seleccionado y no a toda la semana.

El video PAD-07 también muestra el formulario de traslado temporal, con fecha, turno, bus de destino y motivo. Esto respalda la disponibilidad del flujo de solicitud desde el perfil del padre.

El material no muestra una consulta directa de SQL Server ni una recarga controlada seguida de verificación del mismo registro. En consecuencia, la persistencia definitiva por fecha y turno debe considerarse parcialmente evidenciada. Tampoco se incluyó un video específico del monitor registrando el abordaje o la recogida de un estudiante; esa parte del objetivo continúa pendiente de evidencia audiovisual o de un caso UAT estructurado.

## 5.5 Resultados del módulo de pagos

El panel PAD-02 muestra accesos a Pagos e Historial de Pagos, lo cual acredita que el módulo forma parte de la interfaz integrada. El código y el modelo de datos documentan registro de comprobantes, validación administrativa e historial.

No se encontraron fotografías o videos que muestren un flujo completo de pago: asignación de monto, carga de comprobante, validación por el administrador y consulta posterior del estado. Tampoco se proporcionaron tiempos del proceso anterior y del proceso digital. Por ello, no puede afirmarse con esta evidencia que el módulo haya reducido tiempos de gestión ni aumentado la confianza de los padres.

## 5.6 Retroalimentación cualitativa y alcance UAT

La retroalimentación entregada reporta los siguientes hallazgos:

**Cuadro 5.2. Retroalimentación y grado de respaldo**

| Rol | Retroalimentación reportada | Respaldo disponible |
|---|---|---|
| Administrador | Crear y gestionar paradas | Respaldado por ADM-01 a ADM-03 |
| Administrador | Exigir piloto y monitor al asignar un bus | No visible en el material; requiere caso negativo |
| Administrador | Enlace SignalR entre roles | Parcial; no hay grabación simultánea |
| Piloto/monitor | Pausa de ruta por incidente o espera | Implementada en el sistema; no visible en el video entregado |
| Piloto/monitor | Geolocalización activa | Respaldada funcionalmente por PM-01 y PM-02 |
| Padre | Asistencia limitada al día confirmado | Respaldada visualmente; persistencia pendiente de prueba |
| Padre | Mayor percepción de seguridad por geolocalización | Comentario cualitativo reportado; no cuantificado |
| Pagos | Reducción de tiempos y mayor confianza | Sin medición ni evidencia audiovisual del flujo |

Las imágenes muestran interacción humana con las pantallas, pero no identifican de forma metodológica el número de participantes, sus códigos, el guion aplicado o el estado de cada caso. El material se clasifica como evidencia de demostración y retroalimentación cualitativa, no como acta definitiva de aceptación UAT.

## 5.7 Métricas disponibles y faltantes

**Cuadro 5.3. Estado de las métricas**

| Métrica | Evidencia disponible | Resultado |
|---|---|---|
| Módulos con evidencia audiovisual funcional | Geolocalización y asistencia | Dos módulos principales respaldados |
| Flujo de pagos completo | No disponible | No evaluado |
| Tiempo de gestión antes y después | No disponible | No calculable |
| Tiempo de espera del transporte | No disponible | No calculable |
| Satisfacción de padres | Sin escala ni instrumento | No cuantificable |
| Seguridad percibida | Comentario cualitativo | Indicio favorable, no generalizable |
| Casos UAT aprobados o rechazados | Sin matriz formal | No calculable |
| Defectos y regresión | Sin bitácora consolidada | No calculable |
| Rendimiento, latencia y errores | Sin reporte técnico | No calculable |

Los videos permiten registrar duración del material, pero esa duración no equivale al tiempo que un usuario necesitó para completar una tarea. No se utilizará como indicador de eficiencia.

## 5.8 Cumplimiento de objetivos específicos

**Cuadro 5.4. Evaluación de objetivos**

| Objetivo | Evidencia | Evaluación |
|---|---|---|
| Geolocalización en tiempo casi real | Mapas, ruta y movimiento del marcador | Cumplimiento funcional parcial |
| Asistencia y abordaje | Calendario, confirmación y traslados | Asistencia respaldada; abordaje pendiente |
| Integración de pagos | Acceso visible y respaldo en código | Implementado, sin validación audiovisual completa |
| Validación UAT con usuarios | Fotografías, videos y retroalimentación | Evidencia cualitativa parcial; cierre formal pendiente |

## 5.9 Evaluación de la hipótesis

La evidencia respalda que la plataforma integra visualmente funciones de administración, geolocalización, asistencia y traslados, y que usuarios pueden interactuar con ellas en un ambiente controlado. También existe retroalimentación favorable respecto de la información geográfica y de las correcciones al calendario.

No obstante, la hipótesis incluye una mejora en eficiencia y seguridad. La reducción de tiempos no fue medida contra un proceso anterior; el flujo de pagos no cuenta con evidencia audiovisual completa; y la percepción de seguridad no fue recolectada mediante un instrumento estructurado. Asimismo, el material no constituye una UAT cerrada con resultados por caso y acta de aceptación.

Por lo anterior, **la hipótesis queda parcialmente respaldada en el nivel funcional y cualitativo, pero no puede confirmarse de manera definitiva**. No corresponde rechazar la hipótesis nula con la evidencia disponible. La confirmación global requiere completar el flujo de pagos, la matriz UAT, mediciones comparables de eficiencia y un registro estructurado de percepción de seguridad.

## 5.10 Discusión

La integración en una sola interfaz reduce la fragmentación visible de accesos: el padre dispone de pagos, historial, ruta, calendario y traslados desde su panel, mientras el administrador dispone de rutas, paradas, usuarios y asignaciones. Esta centralización representa una condición favorable para la eficiencia, pero no demuestra por sí sola una reducción temporal.

La geolocalización aporta información visual sobre rutas y ubicación. Su contribución se limita a información y coordinación; no constituye una garantía de seguridad física. La seguridad percibida debe interpretarse como mayor disponibilidad de información sobre el recorrido.

El calendario muestra mejoras asociadas a fechas y turnos. La falta de evidencia específica de abordaje, pagos y sincronización simultánea impide extender el resultado favorable a toda la plataforma.

## 5.11 Limitaciones y amenazas a la validez

1. No se documentó el número ni el perfil anonimizado de participantes.
2. Las grabaciones corresponden a demostraciones breves en ambiente controlado.
3. Parte del mapa utiliza simulación; no se documentó una ruta real completa.
4. No existe medición antes y después de tiempos de espera o gestión.
5. No se aplicó una escala de satisfacción o percepción de seguridad.
6. No se entregó evidencia audiovisual del flujo completo de pagos.
7. No se adjuntó matriz UAT firmada, bitácora de defectos ni acta de aceptación.
8. Las pantallas de los distintos roles no se grabaron simultáneamente.
9. La fotografía y el video de piloto y monitor están duplicados y se trataron como una sola evidencia.
10. El repositorio de Google Drive está restringido al grupo universitario; su disponibilidad depende de los permisos institucionales.

## 5.12 Resumen del capítulo

La evidencia permite documentar avances funcionales verificables en administración de paradas, geolocalización, calendario de asistencia y traslados. El abordaje, los pagos, las métricas de eficiencia y el cierre formal UAT requieren evidencia adicional. En consecuencia, la validación de la hipótesis es parcial y no definitiva.

# CAPÍTULO 6

# CONCLUSIONES Y TRABAJO FUTURO

## 6.1 Estado de las conclusiones

Las conclusiones se formulan únicamente a partir del código documentado, las fotografías, los videos y la retroalimentación proporcionada. Se distinguen resultados funcionales de impactos que aún no fueron medidos.

## 6.2 Respuesta a los objetivos específicos

**Cuadro 6.1. Relación entre objetivos, evidencia y conclusión**

| Objetivo | Evidencia | Conclusión |
|---|---|---|
| Implementar geolocalización | Mapas de administrador, padre y piloto o monitor; video de marcador | Implementado y demostrado en ambiente controlado |
| Desarrollar asistencia y abordaje | Calendario, confirmaciones y traslado temporal | Asistencia demostrada; abordaje requiere evidencia específica |
| Integrar pagos y trazabilidad | Accesos en panel, código y modelo de datos | Integrado técnicamente; validación del flujo completo pendiente |
| Validar mediante UAT | Fotografías, videos y retroalimentación | Validación cualitativa parcial; acta y matriz UAT pendientes |

## 6.3 Conclusiones principales

1. La plataforma centraliza funciones administrativas y de consulta que antes podían encontrarse dispersas. La evidencia muestra paneles diferenciados para administrador, padre y piloto o monitor.
2. La geolocalización se encuentra implementada funcionalmente. El material muestra mapas, paradas, rutas y desplazamiento visual del marcador del bus en una demostración controlada.
3. El calendario permite representar estados de asistencia por fecha y turno. La evidencia es consistente con la corrección que limita la confirmación al día seleccionado, aunque la persistencia debe verificarse mediante consulta y recarga controlada.
4. No se presentó evidencia específica del registro de abordaje por parte del monitor. Por ello, este componente no puede considerarse validado solo con el material entregado.
5. El módulo de pagos aparece integrado en la navegación y está respaldado por código y modelo de datos. Sin un video del flujo completo ni registros de tiempo, no puede concluirse que haya reducido la duración de la gestión o aumentado la confianza.
6. Las fotografías y videos constituyen evidencia útil de demostración y retroalimentación, pero no sustituyen una UAT formal con participantes identificados mediante códigos, casos ejecutados, tiempos, defectos y acta de aceptación.

## 6.4 Conclusión sobre la hipótesis

La hipótesis sostiene que la integración de geolocalización, asistencia y abordaje, pagos y UAT mejora la eficiencia operativa y la percepción de seguridad. El material respalda parcialmente la integración y el funcionamiento de geolocalización y asistencia, además de registrar un indicio cualitativo favorable sobre la información disponible para los padres.

La ausencia de mediciones antes y después, evidencia completa de pagos, resultados estructurados de satisfacción y cierre UAT impide confirmar la mejora global. Por tanto, **la hipótesis se considera parcialmente respaldada, pero no confirmada de manera definitiva**. La evidencia disponible tampoco permite rechazar formalmente la hipótesis nula.

## 6.5 Recomendaciones

1. Ejecutar y registrar casos UAT para cada objetivo con resultado esperado, obtenido, tiempo, estado y evidencia.
2. Grabar simultáneamente administrador, padre y piloto o monitor para comprobar la comunicación SignalR.
3. Incorporar un video del flujo completo de pagos y su historial.
4. Verificar la persistencia de asistencia mediante recarga y consulta de base de datos.
5. Documentar el registro de abordaje desde el perfil del monitor.
6. Medir el proceso anterior y el digital con el mismo criterio si se desea afirmar reducción de tiempos.
7. Utilizar preguntas estructuradas o una escala breve si se mantiene la afirmación sobre percepción de seguridad.
8. Incorporar bitácora de defectos, regresión y acta de aceptación.
9. Mantener visible y vigente el enlace institucional de evidencias para los revisores autorizados.

## 6.6 Trabajo futuro

1. Completar la UAT formal con usuarios representativos y trazabilidad por requisito.
2. Ampliar la evidencia a rutas reales y períodos de uso más prolongados.
3. Integrar pruebas automatizadas y de carga con reportes reproducibles.
4. Unificar y auditar el modelo de pagos y sus migraciones.
5. Evaluar notificaciones fuera del navegador y alternativas móviles.
6. Fortalecer autorización de API, SignalR y protección de datos.
7. Medir eficiencia y percepción de seguridad con instrumentos aprobados por el asesor.

## 6.7 Cierre

Transportes Génesis presenta una integración funcional verificable en varios de sus módulos. El principal aporte del material revisado consiste en demostrar visualmente el funcionamiento de geolocalización, gestión de paradas, asistencia y traslados. La validez académica de la afirmación de impacto dependerá de completar las mediciones y documentos UAT que aún faltan.

# GLOSARIO

**API:** interfaz que permite comunicación estructurada entre componentes.

**ASP.NET Core:** plataforma de Microsoft para aplicaciones web.

**Abordaje:** registro operativo de que un estudiante fue recogido o marcado como presente.

**Entity Framework Core:** herramienta de acceso a datos para .NET.

**Geolocalización:** determinación de coordenadas geográficas de un dispositivo.

**Haversine:** fórmula para aproximar distancia entre puntos geográficos.

**Identity:** componente de ASP.NET Core para usuarios, autenticación y roles.

**Leaflet:** biblioteca para mapas interactivos.

**OSRM:** servicio de cálculo de rutas sobre una red vial.

**OpenStreetMap:** proyecto colaborativo de datos cartográficos abiertos.

**SignalR:** biblioteca para comunicación en tiempo real.

**SQL Server:** sistema de gestión de bases de datos relacionales.

**Trazabilidad:** capacidad de reconstruir eventos mediante registros.

**UAT:** pruebas de aceptación realizadas por usuarios.

# REFERENCIAS

Del Valle, J. (2019). *Redes neuronales y transferencia de aprendizaje aplicado a la tarea de perfilamiento de autores de textos anónimos* [Trabajo de graduación, Universidad Galileo].

International Organization for Standardization. (2023). *ISO/IEC 25010:2023 Systems and software engineering—Systems and software Quality Requirements and Evaluation (SQuaRE)—Product quality model*. https://www.iso.org/standard/78176.html

Microsoft. (2024). *Introduction to ASP.NET Core SignalR*. Microsoft Learn. https://learn.microsoft.com/aspnet/core/signalr/introduction

OpenStreetMap contributors. (2026). *OpenStreetMap*. https://www.openstreetmap.org/

OWASP Foundation. (2021). *OWASP Application Security Verification Standard 4.0.3*. https://owasp.org/www-project-application-security-verification-standard/

PCI Security Standards Council. (2024). *Payment Card Industry Data Security Standard: Requirements and testing procedures, version 4.0.1*. https://www.pcisecuritystandards.org/

Project OSRM. (2026). *Open Source Routing Machine*. https://project-osrm.org/

Selenium Project. (2026). *Selenium documentation*. https://www.selenium.dev/documentation/

Universidad Galileo. (s. f.). *Guía para la preparación de trabajo de graduación*.

Universidad Galileo, FISICC. (2026). *Guía de validación en ambiente real: checklist para probar un proyecto funcional en la tesis* [Documento orientativo].

World Wide Web Consortium. (2024). *Geolocation: W3C Recommendation 14 August 2024*. https://www.w3.org/TR/2024/REC-geolocation-20240814/

# APÉNDICE A

# FORMATO DE CASO DE PRUEBA UAT

| Campo | Contenido |
|---|---|
| Identificador | UAT-__ |
| Requisito relacionado | |
| Fecha y versión | |
| Rol | |
| Precondiciones | |
| Datos de prueba | |
| Pasos | |
| Resultado esperado | |
| Resultado obtenido | |
| Tiempo | |
| Ayuda requerida | |
| Estado | Aprobado / con observaciones / rechazado / bloqueado |
| Defecto relacionado | |
| Enlace a evidencia anonimizada | [PEGAR URL] |
| Observaciones | |

# APÉNDICE B

# CONSENTIMIENTO INFORMADO PARA UAT

Se invita a participar en una evaluación académica de la plataforma Transportes Génesis. La sesión incluye tareas de uso, observación, registro de tiempos y, con autorización, fotografías o grabación de pantalla. No se utilizarán pagos, credenciales ni datos bancarios reales. La participación es voluntaria y puede finalizarse en cualquier momento.

Los resultados se presentarán de forma agrupada y anonimizada. No se publicarán nombres, rostros de menores, direcciones, coordenadas del hogar ni información que identifique al estudiante. Al firmar, el participante confirma que recibió una explicación, tuvo oportunidad de preguntar y acepta participar.

Nombre o código: ____________________

Autoriza grabación de pantalla: Sí / No

Autoriza fotografía anonimizada del ambiente: Sí / No

Firma: ____________________

Fecha: ____________________

Firma del investigador: ____________________

# APÉNDICE C

# MATRIZ DE EVIDENCIAS Y ENLACES

**Repositorio audiovisual institucional — acceso para el grupo de la universidad:**

**https://drive.google.com/drive/folders/1d1_k0cRqK9md96MxalGPg3LT3fl4RPZg?usp=sharing**

| ID | Tipo | Descripción | Caso o requisito | Fecha | Responsable | Enlace |
|---|---|---|---|---|---|---|
| SCR-01 | Script | Automatización Selenium | UAT-__ | Pendiente | Pendiente | [PEGAR URL] |
| REP-01 | Reporte | Ejecución automatizada | UAT-__ | Pendiente | Pendiente | [PEGAR URL] |
| VID-01 | Video | Rutas de piloto o monitor | UAT geolocalización | Evidencia recibida | Equipo | Repositorio audiovisual institucional |
| VID-02 | Video | Mapa del padre | UAT geolocalización | Evidencia recibida | Equipo | Repositorio audiovisual institucional |
| VID-03 | Video | Calendario y traslado | UAT asistencia | Evidencia recibida | Equipo | Repositorio audiovisual institucional |
| VID-04 | Video | Confirmación de asistencia | UAT asistencia | Evidencia recibida | Equipo | Repositorio audiovisual institucional |
| FOTO-01 | Fotografías | Administrador, padre y piloto o monitor | Demostraciones por rol | Evidencia recibida | Equipo | Repositorio audiovisual institucional |
| LOG-01 | Logs | Evidencia técnica | Prueba técnica | Pendiente | Pendiente | [PEGAR URL] |
| ACT-01 | Acta | Aceptación de usuario | Cierre UAT | Pendiente | Pendiente | [PEGAR URL] |

# APÉNDICE D

# BITÁCORA DE DEFECTOS

| ID | Módulo | Descripción | Severidad | Pasos | Estado | Responsable | Evidencia |
|---|---|---|---|---|---|---|---|
| DEF-001 | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | Pendiente | [PEGAR URL] |

# APÉNDICE E

# PROTOCOLO DE PRUEBAS TÉCNICAS

1. Registrar versión, fecha, servidor, hardware y red.
2. Verificar que el ambiente no contenga datos reales.
3. Medir tiempo de respuesta de páginas y API principales.
4. Mantener una simulación de mapa durante treinta minutos.
5. Simular usuarios concurrentes según el escenario aprobado.
6. Registrar promedio, percentil 95, throughput, errores, CPU y memoria.
7. Repetir la medición al menos tres veces.
8. Conservar scripts, configuración, logs y reportes mediante enlaces.
9. Identificar el commit o versión exacta sometida a prueba.

# APÉNDICE F

# LISTA DE INFORMACIÓN PENDIENTE

1. Nombre oficial de facultad o escuela.
2. Grado académico, números de carné y nombre del asesor.
3. Nombre público o reservado del colegio.
4. Autorizaciones institucionales y consentimientos.
5. Cantidad y roles de participantes.
6. Fechas, lugar, versión y ambiente de ejecución.
7. Matriz formal de resultados UAT y pruebas técnicas.
8. Enlaces a scripts y reportes del compañero responsable.
9. Confirmación periódica de permisos del repositorio audiovisual institucional.
10. Bitácora de defectos, regresión y acta de aceptación.
11. Revisión del asesor sobre la omisión de encuestas.
12. Cartas de aprobación y autorización para publicación.

# APÉNDICE G

# GUÍA DE ENTREVISTA SOBRE EL PROCESO ACTUAL

Esta guía se aplicará al administrador de Transportes Génesis y, cuando sea posible, a un piloto o monitor. Es una entrevista operativa para describir el proceso y los requisitos; no es una encuesta de percepción. Las respuestas se registrarán sin credenciales ni datos personales de estudiantes.

1. ¿Cómo se informa actualmente a los padres sobre ubicación o retrasos?
2. ¿Cuáles son las consultas más frecuentes durante una ruta?
3. ¿Cómo se confirma si un estudiante utilizará el transporte?
4. ¿Cómo se registra que un estudiante fue recogido o entregado?
5. ¿Cómo se reciben, verifican y archivan los comprobantes de pago?
6. ¿Qué información se pierde, duplica o resulta difícil de consultar?
7. ¿Qué fallos impedirían utilizar el sistema en la operación diaria?
8. ¿Qué funciones deben aprobarse obligatoriamente antes de la entrega?

Las respuestas se codificarán por proceso, requisito, riesgo y criterio de aceptación.
