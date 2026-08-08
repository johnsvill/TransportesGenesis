---
titulo: Implementación y validación funcional de una plataforma web de geolocalización, asistencia y abordaje para Transportes Génesis
universidad: Universidad Galileo
facultad: "PENDIENTE DE COMPLETAR POR LOS AUTORES"
carrera: "Ingeniería en Ciencias y Sistemas"
autor: "Jonathan Samuel Villeda Pérez y José David Florián Secaida"
carne: "PENDIENTE DE COMPLETAR POR LOS AUTORES"
asesor: "PENDIENTE DE COMPLETAR POR LOS AUTORES"
lugar: Guatemala
fecha: Agosto de 2026
estado: Versión final de contenido; datos administrativos pendientes
---

**UNIVERSIDAD GALILEO**

**PENDIENTE DE COMPLETAR: FACULTAD O ESCUELA**

**INGENIERÍA EN CIENCIAS Y SISTEMAS**

**IMPLEMENTACIÓN Y VALIDACIÓN FUNCIONAL DE UNA PLATAFORMA WEB DE GEOLOCALIZACIÓN, ASISTENCIA Y ABORDAJE PARA TRANSPORTES GÉNESIS**

Trabajo de graduación presentado por:

**JONATHAN SAMUEL VILLEDA PÉREZ**

**JOSÉ DAVID FLORIÁN SECAIDA**

Carné(s): **PENDIENTE DE COMPLETAR POR LOS AUTORES**

Previo a optar al grado académico de:

**PENDIENTE DE COMPLETAR: GRADO O TÍTULO ACADÉMICO**

Guatemala, agosto de 2026

<!-- PAGE_BREAK -->

# DECLARACIÓN DEL TRABAJO

Esta tesis fue elaborada por **Jonathan Samuel Villeda Pérez** y **José David Florián Secaida** como requisito para obtener el grado o título de **PENDIENTE DE COMPLETAR: GRADO O TÍTULO ACADÉMICO**.

Guatemala, agosto de 2026.

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

# ACTA DE ACEPTACIÓN DE USUARIOS

**Proyecto:** Implementación y validación funcional de una plataforma web de geolocalización, asistencia y abordaje para Transportes Génesis.

**Lugar:** ____________________________________    **Fecha:** ____ / ____ / ______

Por medio de la presente, las personas firmantes dejan constancia de que participaron en la revisión funcional de la plataforma Transportes Génesis. Durante la sesión se explicaron el propósito del sistema, las funciones presentadas y las limitaciones del ambiente de prueba.

**Funciones revisadas:**

[ ] Rutas y paradas    [ ] Geolocalización y mapa    [ ] Asistencia diaria    [ ] Abordaje

**Resultado de la revisión:**

[ ] Aceptado para el alcance funcional presentado.

[ ] Aceptado con observaciones.

[ ] Requiere correcciones antes de su aceptación.

**Observaciones:**

________________________________________________________________________

________________________________________________________________________

________________________________________________________________________

**Firmas de usuarios representativos:**

| Rol | Nombre completo | Firma |
|---|---|---|
| Padre, madre o encargado | ______________________________ | ____________________ |
| Piloto | ______________________________ | ____________________ |
| Monitor | ______________________________ | ____________________ |
| Administrador | ______________________________ | ____________________ |

**Responsables del proyecto:**

| Nombre | Firma |
|---|---|
| Jonathan Samuel Villeda Pérez | ______________________________ |
| José David Florián Secaida | ______________________________ |

Las firmas consignadas certifican únicamente la revisión y aceptación del alcance funcional presentado; no constituyen certificación de producción, rendimiento ni seguridad física del servicio.

<!-- PAGE_BREAK -->

# RESUMEN

El presente trabajo documenta la implementación y la validación funcional de una plataforma web para Transportes Génesis, empresa dedicada al transporte escolar en San Cristóbal, departamento de San Marcos, Guatemala. El problema abordado es la dispersión de información sobre rutas, ubicaciones, asistencia y recogidas, que dificulta el seguimiento operativo del servicio.

La solución integra autenticación por roles, administración de buses, rutas y paradas, geolocalización en tiempo casi real, confirmación diaria de asistencia y registro operativo de recogidas. El sistema utiliza ASP.NET Core 8, SQL Server, Entity Framework Core, ASP.NET Core Identity, SignalR, Leaflet, OpenStreetMap y OSRM. El módulo de pagos se conserva como función complementaria y no forma parte de la hipótesis funcional evaluada.

La investigación es aplicada, descriptiva y evaluativa, bajo un estudio de caso tecnológico. La evidencia comprende un informe de pruebas fechado el 4 de agosto de 2026, que declara aprobados dos casos funcionales de rutas y paradas, y material audiovisual de los perfiles administrador, padre, piloto y monitor. Las capturas del informe respaldan los casos solo de forma parcial: una muestra un resultado de cálculo y otra un mapa de detalle, pero no demuestran por completo la edición y persistencia esperadas. También se revisaron pantallas de asistencia, geolocalización y pagos. Los resultados respaldan la integración funcional de rutas, mapas, ubicación observable y confirmación de asistencia en un ambiente controlado. El abordaje, la persistencia de algunos registros, el rendimiento y la aceptación formal por todos los perfiles no cuentan con evidencia suficiente para una afirmación general.

La hipótesis de investigación queda respaldada dentro del alcance funcional documentado: la plataforma centraliza la consulta de rutas y ubicación y permite representar la confirmación diaria de asistencia. Esta conclusión no equivale a certificar despliegue productivo, reducción de tiempos, seguridad física ni aceptación integral del sistema.

**Palabras clave:** transporte escolar, geolocalización, asistencia, abordaje, SignalR, pruebas funcionales, trazabilidad.

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

Implementación y validación funcional de una plataforma web de geolocalización, asistencia y abordaje para Transportes Génesis, orientada a centralizar información operativa del transporte escolar.

## 1.2 Contexto de la investigación

El transporte escolar requiere coordinar buses, rutas, paradas, estudiantes, pilotos, monitores y responsables. Cuando la información se distribuye entre llamadas, mensajes y registros separados, la ubicación del bus y el estado diario de los estudiantes resultan difíciles de consultar y reconstruir.

Transportes Génesis presta el servicio en San Cristóbal, departamento de San Marcos. La plataforma propuesta centraliza funciones administrativas y operativas mediante perfiles diferenciados. El administrador gestiona buses, rutas y paradas; el piloto o monitor consulta el recorrido y transmite ubicaciones; y el padre visualiza el mapa y confirma la asistencia diaria.

El sistema también incluye funciones complementarias de pagos y traslados. Estas se describen para documentar el producto, pero no se utilizan para confirmar la hipótesis principal porque la evidencia entregada no cubre su validación completa.

## 1.3 Hipótesis

**Hipótesis de investigación (H1):**

La integración funcional de la geolocalización y la confirmación diaria de asistencia permite centralizar el monitoreo de buses y rutas y la consulta de confirmaciones diarias en la plataforma Transportes Génesis.

**Hipótesis nula (H0):**

La integración funcional de la geolocalización y la confirmación diaria de asistencia no permite centralizar el monitoreo de buses y rutas ni la consulta de confirmaciones diarias en la plataforma Transportes Génesis.

La hipótesis se limita al funcionamiento observable. No afirma reducción de tiempos, mejora estadística de satisfacción, seguridad física ni adopción productiva. Se considera respaldada cuando existe evidencia trazable de rutas y paradas, visualización o actualización de ubicación y confirmación de asistencia por fecha o turno.

**Cuadro 1.1. Criterios de evaluación de la hipótesis**

| Componente | Evidencia utilizada | Criterio de respaldo |
|---|---|---|
| Rutas y paradas | Informe de pruebas y capturas | Cálculo, detalle y edición observables |
| Geolocalización | Videos de piloto, monitor y padre | Ruta, mapa o cambio de ubicación observable |
| Asistencia diaria | Videos del calendario del padre | Confirmación asociada a fecha y turno |
| Integración | Navegación por roles y arquitectura | Funciones disponibles en una plataforma común |

## 1.4 Objetivos

### 1.4.1 Objetivo general

Validar funcionalmente la integración de geolocalización y confirmación diaria de asistencia para centralizar el monitoreo de buses y rutas y la consulta de confirmaciones diarias en Transportes Génesis.

### 1.4.2 Objetivos específicos

1. Implementar geolocalización en tiempo casi real para el monitoreo de buses y rutas.
2. Desarrollar el módulo de asistencia y abordaje para el control operativo de estudiantes.
3. Documentar mediante pruebas funcionales y evidencia audiovisual el comportamiento observable de rutas, paradas, geolocalización y asistencia.

## 1.5 Planteamiento del problema

La operación del transporte escolar genera información que cambia durante el día. El bus recorre paradas, recoge estudiantes y reporta posiciones, mientras las familias necesitan conocer la ruta y confirmar si el estudiante utilizará el servicio. La fragmentación de estos datos dificulta su consulta y trazabilidad.

La existencia de un prototipo no demuestra por sí sola que los componentes funcionen de forma integrada. Por ello, la pregunta de investigación es:

**¿La integración funcional de geolocalización y confirmación diaria de asistencia permite centralizar el monitoreo de buses y rutas y la consulta de confirmaciones diarias en Transportes Génesis?**

## 1.6 Justificación

### 1.6.1 Justificación social

La disponibilidad de información sobre rutas, ubicación y asistencia puede apoyar la coordinación entre la empresa y las familias. Este aporte se limita a información y trazabilidad; no constituye una garantía de seguridad física.

### 1.6.2 Justificación operativa

Una plataforma común reduce la dispersión visible de accesos y registros. La administración dispone de rutas y paradas; el personal operativo consulta recorridos; y los padres acceden a mapas y confirmaciones diarias.

### 1.6.3 Justificación tecnológica

El proyecto aplica tecnologías web y comunicación en tiempo casi real para integrar datos geográficos y operativos. La arquitectura permite conservar ubicaciones en una base relacional y distribuir actualizaciones a clientes conectados.

### 1.6.4 Justificación académica

La investigación relaciona un artefacto de software con evidencia funcional documentada. La hipótesis se evalúa únicamente contra resultados observables y se mantienen explícitas las funciones que no fueron verificadas.

## 1.7 Delimitación

**Cuadro 1.2. Delimitación del estudio**

| Dimensión | Delimitación |
|---|---|
| Geográfica | San Cristóbal, departamento de San Marcos, Guatemala |
| Temporal | Desarrollo y evidencia recopilada durante 2026 |
| Tecnológica | Plataforma web; no incluye aplicación móvil nativa |
| Funcional principal | Rutas, paradas, geolocalización y asistencia |
| Funcional complementaria | Abordaje, traslados y pagos, con validación limitada |
| Empírica | Dos casos funcionales documentados y evidencia audiovisual controlada |

## 1.8 Alcance

El trabajo comprende análisis, diseño, implementación y validación funcional inicial. Incluye autenticación por roles, administración de buses, rutas y paradas, visualización cartográfica, recepción de ubicaciones mediante el navegador, comunicación SignalR y confirmación diaria de asistencia.

La evaluación utiliza revisión documental, dos casos funcionales registrados en el informe UAT y observación de videos y fotografías. No se recibieron scripts Selenium, reportes de carga, la versión firmada del acta formal de aceptación ni métricas de uso productivo. Se incluye un machote para completar y firmar el acta cuando esté disponible.

## 1.9 Limitaciones

1. Los casos documentados se ejecutaron con perfil administrador y cubren rutas y paradas.
2. No se identificó cantidad ni perfil anonimizado de usuarios finales.
3. Las capturas del informe UAT no demuestran por completo los resultados esperados de ambos casos.
4. La evidencia audiovisual corresponde a demostraciones breves en ambiente controlado.
5. No existe evidencia específica y estructurada del abordaje registrado por el monitor.
6. No se comprobó persistencia de asistencia mediante consulta de base de datos.
7. No se documentaron tiempos comparativos, rendimiento ni uso sostenido.
8. El módulo de pagos es complementario y no integra la decisión de la hipótesis.
9. La precisión depende del dispositivo, permiso, conectividad y servicios externos.

## 1.10 Resumen del capítulo

El capítulo delimitó una hipótesis funcional verificable y tres objetivos relacionados con implementación y documentación. Las conclusiones se restringen al alcance de la evidencia entregada.

# CAPÍTULO 2

# MARCO TEÓRICO

## 2.1 Sistemas de información y calidad del software

Un sistema de información integra personas, procedimientos, datos y tecnología para apoyar operaciones y decisiones. En Transportes Génesis, la plataforma reúne consultas y registros antes distribuidos entre canales separados. La calidad del producto debe valorarse por características verificables y por el contexto de uso, no únicamente porque el código compile (International Organization for Standardization, 2023).

## 2.2 Transporte escolar y trazabilidad

La trazabilidad es la capacidad de reconstruir un evento mediante datos relacionados. En transporte escolar requiere asociar buses, rutas, paradas, estudiantes, fecha, turno y responsable. Un registro de asistencia anticipada y un registro de recogida representan hechos distintos: la confirmación informa intención de uso, mientras la recogida documenta una acción operativa.

## 2.3 Geolocalización

La geolocalización del navegador permite obtener coordenadas con autorización del usuario y en un contexto seguro. Su precisión depende del dispositivo, la red y el entorno; por ello, cada coordenada debe acompañarse de fecha y hora (World Wide Web Consortium, 2024).

En aplicaciones web, «tiempo real» describe actualizaciones recibidas sin recargar la pantalla. En este proyecto se utiliza «tiempo casi real» porque siempre existe latencia entre captura, transmisión, almacenamiento y representación.

### 2.3.1 Representación cartográfica

Leaflet proporciona componentes para mapas interactivos; OpenStreetMap aporta datos cartográficos; OSRM calcula trayectos sobre la red vial; y Nominatim permite búsquedas geográficas. Estas herramientas cumplen responsabilidades diferentes y una ruta sugerida no reemplaza las decisiones operativas del piloto (Leaflet, 2026; OpenStreetMap contributors, 2026; Project OSRM, 2026).

### 2.3.2 Comunicación con SignalR

SignalR permite que el servidor envíe eventos a navegadores conectados. Transportes Génesis recibe ubicaciones mediante una API, las almacena y distribuye actualizaciones a grupos asociados con buses o usuarios. La reconexión y los mecanismos de transporte dependen de las capacidades del cliente y del servidor (Microsoft, 2024).

## 2.4 Asistencia y abordaje

La asistencia diaria registra si el estudiante utilizará el servicio en una fecha y turno. El abordaje registra presencia o recogida durante la ruta. La evidencia de una confirmación previa no permite inferir que el estudiante abordó; ambos eventos deben evaluarse por separado.

## 2.5 Arquitectura web y control por roles

La solución separa presentación, páginas o controladores, servicios y acceso a datos. ASP.NET Core Identity gestiona autenticación y roles; Entity Framework Core relaciona entidades con SQL Server; y Razor Pages, MVC y API proporcionan interfaces de interacción.

Los controles de autorización deben aplicarse en interfaz, API y canales de tiempo real. OWASP recomienda verificar autenticación, control de acceso, validación y protección de datos de manera sistemática (OWASP Foundation, 2021).

## 2.6 Pagos como módulo complementario

El sistema contiene registro de pagos, carga de comprobantes, historial y una integración complementaria con Stripe. Los datos sensibles deben procesarse mediante componentes del proveedor y prácticas apropiadas; la presencia del módulo no equivale a certificación PCI ni a conciliación financiera validada (PCI Security Standards Council, 2024).

## 2.7 Pruebas funcionales y UAT

Las pruebas funcionales comparan un resultado observado con uno esperado. La UAT, en sentido estricto, requiere que usuarios representativos ejecuten escenarios y decidan si el sistema satisface sus necesidades. El informe entregado se denomina UAT, pero identifica como responsables al equipo QA, administrador y validadores técnicos; por ello, esta tesis lo interpreta como evidencia de aceptación funcional limitada, no como aceptación integral por padres, pilotos y monitores.

La guía FISICC recomienda definir casos, condiciones, resultados esperados, resultados obtenidos, estado y evidencia, y relacionar los hallazgos con los objetivos y la hipótesis (Universidad Galileo, FISICC, 2026b).

## 2.8 Selección de métodos

La guía de validación indica que no es necesario aplicar todos los métodos disponibles y recomienda combinar métodos técnicos y evidencia con usuarios (Universidad Galileo, FISICC, 2026b). Debido a que no se entregaron encuestas ni datos suficientes de participantes, este estudio utiliza revisión documental, resultados funcionales y observación audiovisual, sin calcular satisfacción ni inferencias poblacionales.

## 2.9 Ética y protección de datos

La evidencia de menores, ubicaciones y pagos exige anonimización. No deben publicarse nombres, direcciones, coordenadas de hogares, comprobantes ni credenciales. El documento UAT se referencia únicamente mediante la carpeta de evidencias, porque contiene información interna que debe ser redactada antes de difundirse individualmente.

## 2.10 Definiciones operacionales

**Cuadro 2.1. Términos principales**

| Término | Definición operacional |
|---|---|
| Geolocalización | Obtención autorizada de latitud y longitud |
| Tiempo casi real | Actualización con latencia breve sin promesa de inmediatez absoluta |
| Asistencia | Confirmación de uso del servicio por fecha y turno |
| Abordaje | Registro operativo de presencia o recogida |
| Trazabilidad | Reconstrucción de eventos mediante datos relacionados |
| UAT | Prueba de aceptación por usuarios representativos |
| Prueba funcional | Comparación entre resultado esperado y observado |
| SignalR | Biblioteca para comunicación de eventos entre servidor y clientes |

## 2.11 Resumen del capítulo

El marco teórico define geolocalización, asistencia, abordaje, arquitectura, control de acceso y validación funcional. Estas definiciones permiten interpretar la evidencia sin extender sus resultados más allá del alcance documentado.

# CAPÍTULO 3

# MARCO METODOLÓGICO

## 3.1 Enfoque y tipo de investigación

La investigación es aplicada porque desarrolla y evalúa una solución para una necesidad concreta. Tiene alcance descriptivo y evaluativo y utiliza un estudio de caso tecnológico. El análisis se limita a evidencia documental y audiovisual disponible al 8 de agosto de 2026 y organiza sus secciones conforme al checklist institucional (Universidad Galileo, FISICC, 2026a).

## 3.2 Unidades de análisis

La unidad técnica es la versión 1.0.0 identificada por el informe de pruebas del 4 de agosto de 2026. Las unidades documentales son dos casos funcionales del perfil administrador, fotografías y videos de pantallas de administrador, padre, piloto y monitor.

El informe no identifica número, códigos ni características de participantes finales. En consecuencia, no se realizan inferencias sobre satisfacción, adopción o percepción colectiva.

## 3.3 Fuentes de evidencia

**Cuadro 3.1. Fuentes analizadas**

| Fuente | Contenido | Uso en el análisis |
|---|---|---|
| Informe UAT versión 1.0 (archivo local) | Dos casos de rutas y paradas | Resultado esperado, obtenido y estado |
| Repositorio audiovisual | Videos y fotografías por módulo | Funcionamiento visible |
| Código y documentación del proyecto | Arquitectura, tecnologías y restricciones | Descripción técnica |
| Checklist FISICC | Requisitos académicos y de validación | Estructura y límites de interpretación |

El material audiovisual publicado se encuentra en [Evidencias audiovisuales](https://github.com/johnsvill/TransportesGenesis/tree/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis). El PDF UAT se conserva únicamente en el archivo local del proyecto y no se enlaza debido a su clasificación interna y a la presencia de una credencial que debe redactarse.

## 3.4 Categorías y criterios

**Cuadro 3.2. Operacionalización funcional**

| Categoría | Indicador | Criterio |
|---|---|---|
| Rutas | Detalle del bus y mapa | Resultado aprobado en caso UAT-001 |
| Paradas | Pantalla con opciones de edición | Resultado aprobado en caso UAT-002 |
| Geolocalización | Ruta, mapa o actualización visible | Evidencia audiovisual identificable |
| Asistencia | Estado por fecha y turno | Evidencia audiovisual identificable |
| Integración | Acceso por roles a funciones relacionadas | Pantallas disponibles en plataforma común |

## 3.5 Técnicas e instrumentos

### 3.5.1 Revisión documental

Se extrajeron alcance, ambiente, pasos, resultados e incidencias del informe UAT. No se copiaron credenciales ni datos sensibles.

### 3.5.2 Análisis de casos funcionales

Cada caso se registró con identificador, objetivo, resultado esperado, resultado obtenido y estado. La tasa se calculó solo sobre los casos efectivamente documentados.

### 3.5.3 Observación audiovisual

Los videos y fotografías se clasificaron por rol y módulo. Se registró únicamente lo visible; la duración de un video no se interpretó como tiempo de tarea ni métrica de eficiencia.

### 3.5.4 Revisión técnica

La arquitectura y restricciones se contrastaron con la documentación del proyecto. No se recibieron scripts Selenium, reportes de carga, logs consolidados ni resultados reproducibles de rendimiento.

## 3.6 Procedimiento ejecutado

1. Inventariar los archivos de evidencia y detectar duplicados.
2. Extraer del informe UAT alcance, ambiente, casos y resultados sin divulgar credenciales.
3. Clasificar videos y fotografías por rol y función.
4. Comparar cada evidencia con los criterios de la hipótesis.
5. Separar funciones observadas de funciones no demostradas.
6. Relacionar objetivos, resultados, limitaciones y conclusiones.
7. Incorporar enlaces externos sin incrustar material audiovisual.

## 3.7 Plan de análisis

El estado reportado se resume como casos declarados aprobados entre casos documentados. Se presenta con su denominador para evitar generalización: el informe declara aprobados dos casos de dos. La cobertura de las capturas se analiza por separado y la evidencia audiovisual no se transforma en porcentajes de satisfacción.

## 3.8 Validez y trazabilidad

Cada resultado del Capítulo 5 se relaciona con el informe UAT o con un enlace del repositorio. La conclusión funcional se considera válida solo para las pantallas y casos observados. No se adopta la declaración amplia de «listo para producción» incluida en el informe, porque los dos casos excluyen integraciones externas, reportes financieros, perfiles operativos y pruebas no funcionales.

## 3.9 Ética

No se reproducen credenciales, nombres de menores, direcciones, coordenadas privadas ni comprobantes. Los enlaces dirigen a material administrado por el equipo del proyecto. Antes de cualquier publicación externa debe comprobarse la anonimización y rotarse toda credencial expuesta.

## 3.10 Amenazas a la validez

1. La muestra documental contiene únicamente dos casos del administrador.
2. Se incluye el machote del acta de aceptación; la versión completada y firmada por usuarios representativos permanece pendiente de adjuntar.
3. Los videos son demostraciones breves y no sesiones UAT completas.
4. Algunas funciones utilizan simulación o comportamiento de respaldo.
5. No se aportaron mediciones de latencia, carga, persistencia o uso sostenido.
6. Los enlaces dependen de permisos y conservación futura del repositorio.

## 3.11 Resumen del capítulo

La metodología permite verificar funcionamiento observable sin presentar demostraciones como aceptación integral. La combinación de casos documentados y evidencia audiovisual es suficiente para una conclusión funcional acotada.

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

La arquitectura combina Razor Pages y MVC por la evolución del proyecto. Las API atienden ubicaciones y rutas. Los servicios encapsulan reglas de negocio y Entity Framework Core gestiona persistencia. La Figura 4.2 presenta la arquitectura general y la relación entre presentación, API, servicios, datos y proveedores externos.

[IMAGEN:arquitectura_general]

**Figura 4.2. Arquitectura general del sistema Transportes Génesis**

## 4.4 Tecnologías

**Cuadro 4.2. Tecnologías principales**

| Capa | Tecnología | Propósito |
|---|---|---|
| Servidor | ASP.NET Core / .NET 8.0 | Aplicación, páginas, controladores y API |
| Persistencia | SQL Server 2022 (ambiente de prueba documentado) | Almacenamiento relacional |
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

**Figura 4.3. Flujo de una ubicación**

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

La Figura 4.4 resume el flujo de geolocalización desde la captura del GPS del navegador hasta la actualización de las pantallas mediante SignalR.

[IMAGEN:geolocalizacion]

**Figura 4.4. Geolocalización en tiempo real y distribución mediante SignalR**

El sistema identifica una ubicación como reciente según reglas temporales. Puede clasificar el bus como detenido, en movimiento o sin señal. Las alertas se calculan por distancia aproximada. No se utilizan geocercas poligonales.

## 4.6 Rutas y paradas

Las rutas se organizan por bus y turno. El sistema ordena paradas mediante una heurística de vecino más cercano. Este procedimiento busca una solución práctica, pero no garantiza la ruta globalmente óptima. OSRM dibuja el trayecto sobre calles disponibles.

El administrador mantiene paradas y asignaciones. El piloto y monitor consultan el recorrido. La información de asistencia puede modificar la lista de estudiantes esperados, con comportamientos de respaldo para ambientes de demostración.

## 4.7 Asistencia y abordaje

El proyecto contiene la interfaz, el modelo y la API para confirmar si el estudiante utilizará el transporte por la mañana o por la tarde. Sin embargo, la vista actual incluye comportamiento de demostración y la API puede devolver respuestas simuladas ante determinados errores. Por ello, la confirmación no se considerará funcionalmente validada hasta comprobar que el dato se persiste y se recupera desde SQL Server durante UAT. Al completar una parada se crea un registro de recogida; el monitor también dispone de una interfaz para marcar presencia o ausencia.

La Figura 4.5 relaciona la confirmación diaria, la consulta de la ruta, la transmisión GPS y el registro de recogida previsto para el monitor.

[IMAGEN:asistencia_abordaje]

**Figura 4.5. Flujo de asistencia y abordaje durante la operación diaria**

El término «registro de abordaje» se utiliza como concepto operativo. El sistema no verifica identidad mediante hardware y depende de la acción del usuario autorizado. Esta limitación será comunicada a los participantes.

## 4.8 Alertas

SignalR distribuye eventos de ubicación, proximidad, parada completada y retraso. El padre debe mantener el navegador conectado. Algunas alertas automáticas se muestran en tiempo real, pero no todas se almacenan en el historial. Por ello, la tesis distinguirá entre recepción en pantalla y persistencia.

## 4.9 Gestión de pagos

El padre dispone de pantallas para registrar pagos, cargar comprobantes y consultar historial. La evidencia entregada permite observar el módulo, pero el informe UAT excluye integraciones externas y reportes financieros. Por ello, pagos se documenta como función complementaria y no como componente confirmado por la hipótesis.

La Figura 4.6 documenta el flujo diseñado para registro, revisión y conciliación. La validación administrativa y los estados finales se presentan como diseño de referencia; no como resultado UAT aprobado.

[IMAGEN:pagos_conciliacion]

**Figura 4.6. Flujo propuesto de pagos y conciliación administrativa**

La pasarela Stripe es complementaria. No se recibió un reporte transaccional ni una conciliación reproducible, por lo que no se presenta como resultado validado.

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

El capítulo documentó la arquitectura y las funciones implementadas. Los resultados observados y sus límites se presentan por separado en el Capítulo 5.

# CAPÍTULO 5

# VALIDACIÓN Y RESULTADOS

## 5.1 Alcance de los resultados

Los resultados proceden de dos casos funcionales documentados y de evidencia audiovisual. Las afirmaciones se limitan al ambiente controlado y no equivalen a aceptación integral por usuarios finales ni certificación de producción.

## 5.2 Repositorio externo

La evidencia se consulta en [Repositorio de pruebas UAT, fotografías y videos](https://github.com/johnsvill/TransportesGenesis/tree/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis). Los medios no se incrustan en la tesis para evitar exponer datos y aumentar el tamaño del documento.

El inventario local contiene 21 archivos: 12 imágenes, 8 videos y 1 informe PDF. Una imagen y un video se encuentran duplicados entre las carpetas de piloto y monitor; por ello, se consideran 19 archivos únicos. La rama externa de GitHub contiene 15 archivos audiovisuales físicos y 13 únicos; no incluye el PDF UAT, los dos videos adicionales de geolocalización ni el material de pagos que sí existe localmente.

**Cuadro 5.1. Inventario de evidencia única**

| Grupo | Cantidad física | Duplicados | Cantidad única |
|---|---:|---:|---:|
| Imágenes | 12 | 1 | 11 |
| Videos | 8 | 1 | 7 |
| Informe PDF | 1 | 0 | 1 |
| Total | 21 | 2 | 19 |

**Cuadro 5.2. Disponibilidad externa en GitHub**

| Tipo | Archivos físicos | Archivos únicos |
|---|---:|---:|
| Imágenes | 10 | 9 |
| Videos | 5 | 4 |
| Total | 15 | 13 |

## 5.3 Resultados del informe UAT

El informe, fechado el 4 de agosto de 2026, identifica la aplicación versión 1.0.0, Microsoft Edge y SQL Server 2022. Su alcance es el cálculo de rutas, visualización del detalle y edición de paradas. Excluye integraciones externas y reportes financieros (Equipo QA y validadores técnicos regionales, 2026).

**Cuadro 5.3. Casos funcionales documentados**

| Caso | Función | Resultado esperado | Resultado obtenido | Estado |
|---|---|---|---|---|
| UAT-001 | Calcular y ver ruta | Datos del bus y mapa con paradas numeradas | El informe declara resultado correcto; la captura muestra cálculo para un bus y cinco paradas, sin el mapa de detalle esperado | Aprobado según informe; evidencia parcial |
| UAT-002 | Calcular y editar paradas | Gestión de paradas con opciones de edición | El informe declara pantalla correcta; la captura muestra detalle y mapa, sin controles de edición ni operación guardada | Aprobado según informe; evidencia parcial |

El informe declara los dos casos aprobados, equivalente a 2/2 o 100 % de los casos documentados. Este valor describe el estado asignado por el informe, no una comprobación independiente ni cobertura total del sistema. Las capturas no demuestran todos los pasos y resultados esperados. El informe no contiene casos de padre, piloto, monitor, asistencia, abordaje, geolocalización transmitida, pagos, autorización negativa ni rendimiento.

El informe indica que no se encontraron incidencias críticas en los casos ejecutados y recomienda vigilar la carga en buses con más de cincuenta paradas. Su conclusión de disponibilidad para producción no se adopta como conclusión de esta tesis debido al alcance reducido.

## 5.4 Resultados de geolocalización, rutas y paradas

Las fotografías administrativas muestran panel, formulario de creación y una tabla con acciones aparentes de edición de paradas. El informe UAT declara aprobados el cálculo y la edición, pero sus capturas solo respaldan parcialmente el cálculo y el mapa de detalle; no demuestran que una edición haya sido guardada o persistida.

Los videos del piloto o monitor muestran pantallas de ruta y geolocalización; el material del padre muestra acceso al mapa. Dos videos locales adicionales documentan el uso de la pantalla geográfica en un teléfono dentro de vehículos durante un recorrido. Esta evidencia fortalece la demostración en contexto operativo, pero no mide precisión GPS, latencia, persistencia de cada coordenada ni sincronización simultánea entre todos los perfiles.

La operación segura requiere que el teléfono permanezca en un soporte y que cualquier interacción durante el movimiento sea realizada por el monitor o con el vehículo detenido. La grabación no constituye evidencia de un protocolo formal de seguridad vial.

Enlaces externos disponibles: [ruta de piloto o monitor](https://github.com/johnsvill/TransportesGenesis/blob/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis/piloto/video/1%29%20Uso_rutas_piloto_monitor.mp4) y [mapa del padre](https://github.com/johnsvill/TransportesGenesis/blob/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis/padres_de_familia/videos_pantalla_padres_familia/1%29%20Video_uso_del_mapa_padres_familia.mp4). Los dos videos adicionales de geolocalización se revisaron desde la copia local, pero no se encuentran en la rama externa indicada.

## 5.5 Resultados de asistencia y abordaje

Los videos del padre muestran calendario, selección de fecha y estados por turno. Esta evidencia respalda la confirmación diaria observable y evita interpretar una selección como confirmación de toda la semana.

Enlaces: [calendario y traslado](https://github.com/johnsvill/TransportesGenesis/blob/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis/padres_de_familia/videos_pantalla_padres_familia/2%29%20Uso_del_calendraio_traslado_padres_familia.mp4) y [confirmación de asistencia](https://github.com/johnsvill/TransportesGenesis/blob/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis/padres_de_familia/videos_pantalla_padres_familia/3%29%20confirmaciones_asistencia_Alumnos_padre_familia.mp4).

No se recibió un caso estructurado ni un video inequívoco del monitor guardando el abordaje o la recogida y verificando posteriormente su persistencia. Por ello, el componente de abordaje se considera implementado en diseño, pero no validado con la evidencia disponible.

## 5.6 Resultado del módulo complementario de pagos

Existen dos imágenes y un video del módulo de pagos. El video local muestra el panel administrativo con pagos de distintos meses, estados pendiente y validado, acceso al comprobante y un cuadro de revisión con acciones de aprobación o rechazo. Esto respalda la existencia del flujo administrativo visible.

El material no muestra de forma continua el registro inicial efectuado por el padre, la comprobación bancaria, la persistencia posterior ni un reporte financiero. Además, el informe UAT excluye integraciones externas y reportes financieros. El video y las dos imágenes se revisaron desde la copia local; no se proporciona enlace individual porque esos archivos no se encuentran en la rama externa indicada y las fotografías del cuaderno contienen datos personales que deben anonimizarse.

En consecuencia, pagos se documenta como función complementaria observada y no forma parte de la decisión de la hipótesis funcional.

## 5.7 Evaluación de objetivos

**Cuadro 5.4. Cumplimiento de objetivos específicos**

| Objetivo | Evidencia | Evaluación |
|---|---|---|
| Implementar geolocalización | Mapas y videos de piloto, monitor y padre | Cumplido funcionalmente en demostración controlada |
| Desarrollar asistencia y abordaje | Calendario y confirmación; diseño de recogida | Asistencia cumplida; abordaje sin validación específica |
| Documentar funcionamiento | Informe con dos casos declarados aprobados y repositorio audiovisual | Cumplido, diferenciando estado reportado y evidencia visible |

## 5.8 Evaluación de la hipótesis

La plataforma reúne rutas, paradas, mapas y confirmación diaria de asistencia en una solución común. El informe declara aprobados dos casos de cálculo y edición de rutas o paradas, aunque sus capturas los respaldan solo parcialmente. Las fotografías administrativas y los videos aportan evidencia adicional de interfaces de rutas, geolocalización y asistencia.

Por tanto, **la hipótesis de investigación queda respaldada dentro del alcance funcional documentado**: la integración permite centralizar el monitoreo observable de buses y rutas y la consulta de confirmaciones diarias de asistencia. Este respaldo no incluye abordaje persistente, rendimiento, producción, reducción de tiempos, satisfacción ni seguridad física. La hipótesis nula no se sostiene para las funciones y el ambiente examinados; esta conclusión no se extiende a otros contextos de operación.

## 5.9 Limitaciones y amenazas

1. Solo se documentaron dos casos funcionales, ambos del administrador, y sus capturas no cubren por completo los resultados esperados.
2. No existe matriz firmada por padres, pilotos o monitores.
3. No se midieron tiempos, errores de usuario, latencia ni carga.
4. El abordaje y la persistencia de asistencia no se verificaron directamente.
5. Los videos no muestran pantallas sincronizadas de varios perfiles.
6. El módulo de pagos no fue incluido en el informe UAT.
7. El acceso futuro a enlaces depende del repositorio.

## 5.10 Resumen del capítulo

La evidencia respalda rutas, paradas, mapas y confirmación diaria en un alcance funcional controlado. Los resultados no justifican una conclusión de aceptación integral o producción.

# CAPÍTULO 6

# CONCLUSIONES Y TRABAJO FUTURO

## 6.1 Respuesta a los objetivos

**Cuadro 6.1. Objetivos y conclusiones**

| Objetivo | Conclusión |
|---|---|
| Implementar geolocalización | Las interfaces de mapa, rutas y ubicación se encuentran implementadas y son observables en videos controlados. |
| Desarrollar asistencia y abordaje | La asistencia por fecha y turno está demostrada; el abordaje carece de evidencia específica de ejecución y persistencia. |
| Documentar funcionamiento | Se consolidaron dos casos declarados aprobados, inventario audiovisual, enlaces y límites de interpretación. |

## 6.2 Conclusiones principales

1. Transportes Génesis centraliza funciones administrativas y operativas en una plataforma con perfiles diferenciados.
2. El informe declara aprobados UAT-001 y UAT-002; las capturas demuestran parcialmente el cálculo y el mapa de detalle, pero no una edición persistida de paradas.
3. La evidencia audiovisual respalda la disponibilidad de mapas y pantallas de geolocalización para perfiles operativos y padres.
4. El calendario permite representar confirmaciones de asistencia por fecha y turno en el perfil del padre.
5. La existencia del diseño y la interfaz de abordaje no sustituye una prueba del monitor que guarde y recupere el registro.
6. El módulo de pagos es complementario y cuenta con evidencia visual, pero no con validación financiera o UAT dentro del informe recibido.
7. El 100 % corresponde al estado declarado en dos casos y no debe interpretarse como verificación independiente ni cobertura completa.

## 6.3 Conclusión sobre la hipótesis

La hipótesis funcional se considera respaldada para el alcance evaluado. Rutas, paradas, mapas y confirmaciones diarias se encuentran centralizados y son observables en una misma plataforma. La hipótesis nula no se sostiene para ese alcance funcional.

La conclusión no demuestra reducción cuantitativa de tiempo, satisfacción, seguridad física, abordaje persistente ni preparación general para producción. Estas dimensiones requerirían casos adicionales, usuarios representativos, métricas y acta formal de aceptación.

## 6.4 Recomendaciones

1. Ejecutar casos de padre, piloto y monitor con participantes identificados mediante códigos.
2. Documentar el abordaje completo, incluida persistencia y consulta posterior.
3. Probar autorización negativa en páginas, API y grupos SignalR.
4. Medir latencia, estabilidad y carga con scripts reproducibles.
5. Redactar o reemplazar el informe UAT antes de publicarlo y rotar toda credencial expuesta.
6. Mantener enlaces con permisos de solo lectura y evidencia anonimizada.
7. Completar datos de portada y adjuntar cartas institucionales antes de subir la entrega.
8. Establecer un protocolo que prohíba al piloto manipular el teléfono con el vehículo en movimiento.

## 6.5 Trabajo futuro

1. Ampliar pruebas a recorridos reales y períodos de uso sostenido.
2. Incorporar notificaciones fuera del navegador cuando exista autorización.
3. Evaluar un mecanismo verificable de abordaje considerando privacidad y costo.
4. Completar validación del módulo de pagos y conciliación.
5. Fortalecer monitoreo, auditoría, respaldo y protección de datos.

## 6.6 Cierre

El aporte comprobable consiste en una plataforma integrada con rutas, paradas, mapas y asistencia diaria, acompañada de evidencia funcional trazable. La tesis conserva como limitaciones todas las capacidades que no fueron demostradas.

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

# BIBLIOGRAFÍA

Equipo QA y validadores técnicos regionales. (2026). *Informe de pruebas UAT: TransportesGenesis, versión 1.0* [Documento interno conservado en el archivo local del proyecto].

International Organization for Standardization. (2023). *ISO/IEC 25010:2023 Systems and software engineering—Systems and software Quality Requirements and Evaluation (SQuaRE)—Product quality model*. [ISO](https://www.iso.org/standard/78176.html)

Leaflet. (2026). *Leaflet documentation*. [Leaflet](https://leafletjs.com/reference.html)

Microsoft. (2024). *Introduction to ASP.NET Core SignalR*. Microsoft Learn. [SignalR](https://learn.microsoft.com/aspnet/core/signalr/introduction)

OpenStreetMap contributors. (2026). *OpenStreetMap*. [OpenStreetMap](https://www.openstreetmap.org/)

OWASP Foundation. (2021). *OWASP Application Security Verification Standard 4.0.3*. [OWASP ASVS](https://owasp.org/www-project-application-security-verification-standard/)

PCI Security Standards Council. (2024). *Payment Card Industry Data Security Standard: Requirements and testing procedures, version 4.0.1*. [PCI SSC](https://www.pcisecuritystandards.org/)

Project OSRM. (2026). *Open Source Routing Machine*. [Project OSRM](https://project-osrm.org/)

Universidad Galileo, FISICC. (2026a). *Guía para la elaboración del trabajo de graduación: Tesis checklist* [Documento institucional].

Universidad Galileo, FISICC. (2026b). *Guía de validación en ambiente real: checklist para probar un proyecto funcional en la tesis* [Documento orientativo].

World Wide Web Consortium. (2024). *Geolocation: W3C Recommendation 14 August 2024*. [W3C Geolocation](https://www.w3.org/TR/2024/REC-geolocation-20240814/)

# APÉNDICE A

# RESUMEN DEL INFORME DE PRUEBAS

**Cuadro A.1. Ambiente documentado**

| Campo | Valor |
|---|---|
| Fecha | 4 de agosto de 2026 |
| Aplicación | TransportesGenesis SaaS |
| Versión | 1.0.0 |
| Navegador | Microsoft Edge |
| Base de datos | SQL Server 2022 |
| Alcance | Cálculo de rutas, detalle y edición de paradas |
| Exclusiones | Integraciones externas y reportes financieros |

**Cuadro A.2. Resultados documentados**

| Caso | Resultado | Estado | Incidencias |
|---|---|---|---|
| UAT-001 — Calcular y ver ruta | El informe declara aprobación; captura parcial sin mapa de detalle | Aprobado según informe | Ninguna declarada |
| UAT-002 — Calcular y editar paradas | El informe declara aprobación; captura sin edición guardada | Aprobado según informe | Ninguna declarada |

Por seguridad, el informe se conserva únicamente en `documento pruebas_UAT_selenium/Documento de Pruebas UAT.pdf`. No se proporciona enlace directo.

# APÉNDICE B

# MATRIZ DE EVIDENCIAS Y ENLACES

**Cuadro B.1. Matriz de evidencias y enlaces externos**

| ID | Módulo | Evidencia | Relación |
|---|---|---|---|
| VID-01 | Rutas | [Piloto o monitor](https://github.com/johnsvill/TransportesGenesis/blob/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis/piloto/video/1%29%20Uso_rutas_piloto_monitor.mp4) | Objetivo 1 |
| VID-02 | Mapa del padre | [Uso del mapa](https://github.com/johnsvill/TransportesGenesis/blob/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis/padres_de_familia/videos_pantalla_padres_familia/1%29%20Video_uso_del_mapa_padres_familia.mp4) | Objetivo 1 |
| VID-03 | Asistencia | [Calendario y traslado](https://github.com/johnsvill/TransportesGenesis/blob/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis/padres_de_familia/videos_pantalla_padres_familia/2%29%20Uso_del_calendraio_traslado_padres_familia.mp4) | Objetivo 2 |
| VID-04 | Asistencia | [Confirmaciones](https://github.com/johnsvill/TransportesGenesis/blob/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis/padres_de_familia/videos_pantalla_padres_familia/3%29%20confirmaciones_asistencia_Alumnos_padre_familia.mp4) | Objetivo 2 |
| LOC-01 | Geolocalización adicional | Dos videos locales sin enlace externo | Objetivo 1 |
| LOC-02 | Pagos complementarios | Un video y dos imágenes locales sin enlace externo | Alcance complementario |
| REP-01 | Pruebas | Informe local sin enlace directo | Objetivo 3 |
| REP-02 | Evidencia publicada | [Carpeta audiovisual en GitHub](https://github.com/johnsvill/TransportesGenesis/tree/dev_intermedia_merge/Documentos/Tesis/imagenes%20_documentos_transporte_genesis) | Objetivos 1–3 |

# APÉNDICE C

# LIMITACIONES Y HALLAZGOS PENDIENTES

**Cuadro C.1. Limitaciones derivadas de la evidencia**

| ID | Área | Hallazgo | Tratamiento en la tesis |
|---|---|---|---|
| LIM-01 | UAT | Solo dos casos del administrador | No generalizar el 100 % |
| LIM-02 | Abordaje | Sin ejecución y persistencia verificadas | Objetivo parcialmente cumplido |
| LIM-03 | Asistencia | Sin consulta directa de base de datos | Limitar conclusión a interfaz observable |
| LIM-04 | Rendimiento | Sin scripts ni reporte | No afirmar escalabilidad o latencia |
| LIM-05 | Pagos | Fuera del alcance del informe UAT | Tratar como módulo complementario |
| LIM-06 | Seguridad | Informe interno contiene una credencial | No enlazar directo; redactar y rotar |
| LIM-07 | Operación vial | No se documentó protocolo de manipulación del teléfono | Usar soporte y asignar interacción al monitor |
| LIM-08 | Evidencia UAT | Capturas no cubren por completo los resultados esperados | Separar estado declarado de verificación independiente |

# APÉNDICE D

# DIAGRAMAS DE ARQUITECTURA Y FLUJOS

Los diagramas se encuentran integrados y referenciados en el Capítulo 4:

**Cuadro D.1. Diagramas integrados en el documento**

| Figura | Contenido |
|---|---|
| Figura 4.2 | Arquitectura general |
| Figura 4.4 | Geolocalización y SignalR |
| Figura 4.5 | Asistencia y abordaje |
| Figura 4.6 | Diseño complementario de pagos |

# APÉNDICE E

# DATOS ADMINISTRATIVOS PENDIENTES

Los autores decidieron completar manualmente los siguientes elementos antes de la carga institucional:

1. Nombre oficial de facultad o escuela.
2. Número de carné de cada autor.
3. Grado o título académico exacto.
4. Nombre completo del asesor.
5. Carta de aprobación del asesor.
6. Dictamen del Director de Programas.
7. Autorización del Decano para publicación.
8. Carátula amarillo pálido y lomo, si la entrega se solicita impresa.

Estos pendientes no modifican el análisis técnico, pero impiden considerar completa la entrega administrativa hasta su incorporación.
