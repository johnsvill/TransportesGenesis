"""Genera la tesis v4 sin encuestas y con evidencias UAT enlazadas.

Parte de la fuente editable de la versión inicial y conserva el generador
institucional: papel carta, Times New Roman 12, doble espacio, texto
justificado, sangría de 0.63 cm y márgenes 4/4/2.5/2.5 cm.
"""

from __future__ import annotations

import importlib.util
from pathlib import Path

from docx.enum.text import WD_ALIGN_PARAGRAPH, WD_LINE_SPACING
from docx.shared import Cm, Pt


ROOT = Path(__file__).resolve().parents[3]
BASE = Path(__file__).resolve().parent
SOURCE_BASE = (
    ROOT
    / "Documentos"
    / "Tesis"
    / "versioniniicial_tesis"
    / "TESIS_TRANSPORTES_GENESIS.md"
)
GENERATOR_BASE = (
    ROOT
    / "Documentos"
    / "Tesis"
    / "versioniniicial_tesis"
    / "generar_tesis.py"
)
SOURCE_OUT = BASE / "TESIS_TRANSPORTES_GENESIS_v4.md"
DOCX_OUT = BASE / "TESIS_TRANSPORTES_GENESIS_v4.docx"
README_OUT = BASE / "README.md"
DIAGRAMS_DIR = BASE / "diagramas_integrados"
SOURCE_DIAGRAM_DOCX = (
    ROOT
    / "Documentos"
    / "Proyecto"
    / "final"
    / "Presentacion_final_20062026"
    / "Proyecto_Transportes_Genesis_v4.docx"
)
DIAGRAM_FILES = {
    "arquitectura_general": DIAGRAMS_DIR / "figura_4_2_arquitectura_general.png",
    "geolocalizacion": DIAGRAMS_DIR / "figura_4_4_geolocalizacion.png",
    "asistencia_abordaje": DIAGRAMS_DIR / "figura_4_5_asistencia_abordaje.png",
    "pagos_conciliacion": DIAGRAMS_DIR / "figura_4_6_pagos_conciliacion.png",
}


def replace_section(text: str, start: str, end: str, replacement: str) -> str:
    start_index = text.index(start)
    end_index = text.index(end, start_index)
    return text[:start_index] + replacement.rstrip() + "\n\n" + text[end_index:]


def build_markdown() -> str:
    text = SOURCE_BASE.read_text(encoding="utf-8")
    text = text.replace("fecha: Julio de 2026", "fecha: Agosto de 2026")
    text = text.replace(
        'carrera: "[Nombre de la carrera]"',
        'carrera: "Ingeniería en Ciencias y Sistemas"',
    )
    text = text.replace(
        'autor: "[Nombre completo del autor]"',
        'autor: "Jonathan Samuel Villeda Pérez y José David Florián Secaida"',
    )
    text = text.replace(
        "estado: Borrador inicial sujeto a validación empírica y revisión del asesor",
        "estado: Borrador integral con evidencia audiovisual; cierre UAT pendiente",
    )
    text = text.replace("Guatemala, julio de 2026", "Guatemala, agosto de 2026")
    text = text.replace("Guatemala, julio de 2026.", "Guatemala, agosto de 2026.")
    text = text.replace(
        "**[NOMBRE DE LA CARRERA]**",
        "**INGENIERÍA EN CIENCIAS Y SISTEMAS**",
    )
    text = text.replace(
        "**[NOMBRE COMPLETO DEL AUTOR]**",
        "**JONATHAN SAMUEL VILLEDA PÉREZ**\n\n"
        "**JOSÉ DAVID FLORIÁN SECAIDA**",
    )
    text = text.replace(
        "Esta tesis fue elaborada por **JONATHAN SAMUEL VILLEDA PÉREZ**\n\n"
        "**JOSÉ DAVID FLORIÁN SECAIDA** como requisito",
        "Esta tesis fue elaborada por **Jonathan Samuel Villeda Pérez** y "
        "**José David Florián Secaida** como requisito",
    )

    text = replace_section(
        text,
        "# RESUMEN",
        "<!-- BODY_START -->",
        """# RESUMEN

El presente trabajo documenta el desarrollo y la validación planificada de una plataforma web integrada para Transportes Génesis, empresa dedicada al transporte escolar en San Cristóbal, departamento de San Marcos, Guatemala. El problema se relaciona con la dispersión de información sobre rutas, asistencia, abordaje y pagos, situación que dificulta la trazabilidad operativa y administrativa.

La solución reúne autenticación por roles, administración de buses y rutas, geolocalización en tiempo casi real, confirmación de asistencia, registro operativo de recogidas, alertas dentro del navegador y gestión digital de comprobantes e historial de pagos. La plataforma utiliza ASP.NET Core 8, SQL Server, Entity Framework Core, ASP.NET Core Identity, SignalR, Leaflet, OpenStreetMap y OSRM.

La investigación es aplicada, con alcance descriptivo y evaluativo y enfoque mixto de predominio técnico. La evidencia revisada comprende fotografías y videos de demostraciones controladas con pantallas de administrador, padre de familia y piloto o monitor. El material respalda de forma funcional la gestión de paradas, la visualización cartográfica, el calendario de asistencia y la solicitud de traslados. No contiene un flujo audiovisual de pagos, mediciones antes y después, una escala de satisfacción, una bitácora formal de casos UAT ni un acta de aceptación. Por ello, la hipótesis queda respaldada parcialmente en su dimensión funcional y cualitativa, pero no confirmada en cuanto a reducción de tiempos ni mejora cuantificada de la percepción de seguridad.

**Palabras clave:** transporte escolar, geolocalización, pagos digitales, UAT, trazabilidad, SignalR, pruebas de software.

""",
    )
    summary_start = text.index("# RESUMEN")
    summary_end = text.index("<!-- BODY_START -->", summary_start)
    summary_block = text[summary_start:summary_end].strip()
    text = text[:summary_start] + text[summary_end:]
    index_start = text.index("# ÍNDICE GENERAL")
    text = (
        text[:index_start]
        + summary_block
        + "\n\n<!-- PAGE_BREAK -->\n\n"
        + text[index_start:]
    )

    text = replace_section(
        text,
        "## 1.3 Hipótesis",
        "## 1.4 Objetivos",
        """## 1.3 Hipótesis

**Hipótesis de investigación (H1):**

La integración de la geolocalización y del módulo de asistencia y abordaje mejora la eficiencia operativa del transporte escolar y la percepción de seguridad de los padres de familia.

**Hipótesis nula (H0):**

La integración de la geolocalización y del módulo de asistencia y abordaje no produce mejoras observables en la eficiencia operativa ni en la percepción de seguridad de los padres de familia.

La eficiencia se entiende como la capacidad de completar tareas operativas dentro de una plataforma integrada, con menor fragmentación de información. La seguridad se interpreta como percepción de información y control, no como garantía de seguridad física. Para confirmar una mejora se requieren mediciones comparables o evidencia de aceptación estructurada. Las fotografías, videos y comentarios cualitativos permiten valorar funcionamiento y utilidad aparente, pero no sustituyen tiempos antes y después, escalas de satisfacción ni un acta UAT.

**Cuadro 1.1. Criterios de evaluación de la hipótesis**

| Componente | Evidencia principal | Criterio propuesto |
|---|---|---|
| Geolocalización | Video, fotografías y registro técnico | Mapa, ruta y actualización de ubicación observables |
| Asistencia y abordaje | Video, fotografías y persistencia | Confirmación asociada a fecha, turno y estudiante |
| UAT | Casos, participantes, resultados y acta | Ejecución por usuarios representativos con evidencia trazable |
| Eficiencia | Tiempos o pasos antes y después | Reducción observable y documentada |
| Seguridad percibida | Instrumento o retroalimentación estructurada | Mejora reportada sin presentarla como seguridad física |
""",
    )

    text = replace_section(
        text,
        "## 1.4 Objetivos",
        "## 1.5 Planteamiento del problema",
        """## 1.4 Objetivos

### 1.4.1 Objetivo general

Validar si la integración de la geolocalización y del módulo de asistencia y abordaje mejora la eficiencia operativa del transporte escolar y la percepción de seguridad de los padres de familia.

### 1.4.2 Objetivos específicos

1. Implementar geolocalización en tiempo real para el monitoreo de buses y rutas.
2. Desarrollar el módulo de asistencia y abordaje para el control operativo de estudiantes.
""",
    )

    text = replace_section(
        text,
        "## 1.5 Planteamiento del problema",
        "## 1.6 Justificación",
        """## 1.5 Planteamiento del problema

La operación de un transporte escolar produce información que cambia durante el día. El bus inicia un recorrido, visita paradas, recoge o deja estudiantes y puede experimentar retrasos. Paralelamente, la administración necesita conocer quién utilizará el servicio y mantener control de pagos. Cuando la información se distribuye entre llamadas, mensajes y registros separados, resulta difícil reconstruir el estado de un recorrido o de un pago.

La falta de una fuente centralizada limita la trazabilidad. Un padre puede desconocer la ubicación reportada del bus; el personal puede tener dificultades para confirmar asistencia o abordaje; y un comprobante puede quedar separado de su validación e historial. La existencia de un prototipo no demuestra por sí sola que los flujos funcionen de manera aceptable en condiciones representativas.

Por lo anterior, la investigación responde la siguiente pregunta:

**¿En qué medida la integración de la geolocalización y del módulo de asistencia y abordaje mejora la eficiencia operativa del transporte escolar y la percepción de seguridad de los padres de familia?**
""",
    )

    text = text.replace(
        "La posibilidad de registrar y consultar pagos de forma remota puede evitar ciertos desplazamientos destinados únicamente a entregar o confirmar un comprobante. La magnitud del ahorro no se asumirá de antemano: se calculará a partir del tiempo y del costo declarado por los participantes para el procedimiento anterior y el procedimiento digital.",
        "La posibilidad de registrar y consultar pagos de forma remota puede reducir actividades administrativas y ciertos desplazamientos. Esta tesis no cuantificará ahorros económicos sin datos verificables; validará que el flujo digital permita registrar, revisar y consultar el estado de un pago.",
    )
    text = text.replace(
        "La evaluación incluye casos funcionales, pruebas UAT, encuesta de percepción, medición de tiempo y costo del proceso de pago, y pruebas de carga y rendimiento en ambiente controlado.",
        "La evaluación incluye casos funcionales, pruebas UAT, observación de tareas, automatización con Selenium y pruebas de carga y rendimiento en ambiente controlado. Las evidencias audiovisuales y técnicas se referenciarán mediante enlaces.",
    )
    text = text.replace(
        "El capítulo presentó el contexto, el problema, la hipótesis compuesta y los objetivos. La propuesta busca evaluar beneficios concretos sin afirmar que la tecnología garantiza la seguridad. La investigación medirá confianza, incertidumbre, tiempo, costo y satisfacción, además del funcionamiento técnico de la plataforma.",
        "El capítulo presentó el contexto, el problema, la hipótesis verificable y los objetivos. La propuesta evaluará cumplimiento funcional, aceptación operativa y rendimiento sin afirmar que la tecnología garantiza la seguridad ni atribuir beneficios no medidos.",
    )

    text = replace_section(
        text,
        "## 2.7 Usabilidad y aceptación tecnológica",
        "## 2.9 Pruebas de software",
        """## 2.7 Usabilidad y aceptación operativa

La usabilidad comprende efectividad, eficiencia y satisfacción en un contexto de uso. En este trabajo se observarán especialmente la finalización de tareas, los errores, la ayuda requerida y el tiempo empleado. Una función puede ser técnicamente correcta y, aun así, impedir que el usuario complete un flujo.

La UAT permite que los usuarios finales ejecuten escenarios representativos y determinen si el resultado es aceptable para su operación. El investigador registra hechos observables y comentarios espontáneos, pero no sustituye la decisión del usuario ni ejecuta las tareas en su lugar.

## 2.8 Selección de métodos sin encuesta

La guía de validación consultada establece que no es necesario aplicar todos los métodos disponibles y recomienda seleccionar dos o tres que se complementen, incluyendo uno técnico y uno con usuarios reales. Por ello, la estrategia combina UAT y observación directa con pruebas funcionales automatizadas y carga-rendimiento.

La encuesta es útil cuando se busca cuantificar percepciones en una muestra amplia. En esta tesis se omite porque la disponibilidad prevista de participantes es reducida y el objetivo reformulado se concentra en cumplimiento funcional y aceptación operativa. Esta decisión evita presentar promedios de satisfacción con alcance estadístico insuficiente. Los comentarios de usuarios se conservarán como evidencia cualitativa anonimizada y no se convertirán en porcentajes de opinión.
""",
    )

    chapter3 = """# CAPÍTULO 3

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

"""
    text = replace_section(text, "# CAPÍTULO 3", "# CAPÍTULO 4", chapter3)

    text = text.replace(
        "| Servidor | ASP.NET Core 8 | Aplicación, páginas, controladores y API |\n"
        "| Persistencia | SQL Server | Almacenamiento relacional |\n"
        "| Acceso a datos | Entity Framework Core | Mapeo y consultas |\n"
        "| Seguridad | ASP.NET Core Identity | Usuarios y roles |\n"
        "| Tiempo real | SignalR | Eventos hacia navegadores |\n"
        "| Mapa | Leaflet y OpenStreetMap | Visualización |\n"
        "| Rutas | OSRM | Trazado sobre calles |\n"
        "| Geocodificación | Nominatim | Búsqueda de direcciones |\n"
        "| Interfaz | Bootstrap, JavaScript y jQuery | Presentación adaptable |",
        "| Servidor | ASP.NET Core / .NET 8.0 | Aplicación, páginas, controladores y API |\n"
        "| Persistencia | SQL Server (versión del entorno pendiente) | Almacenamiento relacional |\n"
        "| Acceso a datos | Entity Framework Core 8.0.10 | Mapeo y consultas |\n"
        "| Seguridad | ASP.NET Core Identity 8.0.10 | Usuarios y roles |\n"
        "| Tiempo real | SignalR para ASP.NET Core 8 | Eventos hacia navegadores |\n"
        "| Mapa | Leaflet 1.9.4 y OpenStreetMap | Visualización |\n"
        "| Rutas | OSRM, servicio público consultado | Trazado sobre calles |\n"
        "| Geocodificación | Nominatim, servicio público consultado | Búsqueda de direcciones |\n"
        "| Pagos en línea | Stripe.net 51.1.0 | Flujo complementario sujeto a configuración |\n"
        "| Interfaz | Bootstrap 5.3.0, JavaScript y jQuery 3.7.0 | Presentación adaptable |",
    )
    text = text.replace(
        "El piloto o monitor utiliza una pantalla web. Durante la demostración, "
        "la ruta puede simularse y enviar una coordenada aproximadamente cada dos "
        "segundos. La API recibe identificador de bus, latitud, longitud y datos "
        "relacionados. El servicio almacena el registro y emite un evento SignalR.",
        "El piloto o monitor utiliza una pantalla web que puede capturar ubicación "
        "continua mediante la API de geolocalización del navegador con "
        "`watchPosition`. El proyecto también conserva una modalidad de simulación "
        "que envía una coordenada aproximadamente cada dos segundos. La API recibe "
        "identificador de bus, latitud, longitud y datos relacionados; el servicio "
        "almacena el registro y emite un evento SignalR.",
    )
    text = text.replace(
        "Una cola local permite conservar temporalmente puntos cuando se interrumpe "
        "la conexión y enviarlos en lote al restablecerse. Este mecanismo mejora "
        "continuidad, pero no garantiza que todos los puntos se reciban.\n\n",
        "",
    )
    text = text.replace(
        "1. El GPS real continuo no está implementado con `watchPosition`; la "
        "demostración utiliza simulación o envío periódico desde la vista.",
        "1. La captura con `watchPosition` depende del permiso, precisión y "
        "conectividad del dispositivo; la simulación permanece como modalidad "
        "alternativa de demostración y debe distinguirse de una ruta real.",
    )
    text = text.replace(
        "9. El cálculo de meses del módulo de pagos debe corregirse o validarse "
        "para noviembre y diciembre.",
        "9. El cálculo de meses del módulo de pagos debe corregirse o validarse "
        "para enero, noviembre y diciembre.",
    )
    text = text.replace(
        "**Figura 4.2. Flujo de una ubicación**",
        "**Figura 4.3. Flujo de una ubicación**",
    )
    text = text.replace(
        "La arquitectura combina Razor Pages y MVC por la evolución del proyecto. "
        "Las API atienden ubicaciones y rutas. Los servicios encapsulan reglas de "
        "negocio y Entity Framework Core gestiona persistencia.",
        "La arquitectura combina Razor Pages y MVC por la evolución del proyecto. "
        "Las API atienden ubicaciones y rutas. Los servicios encapsulan reglas de "
        "negocio y Entity Framework Core gestiona persistencia. La Figura 4.2 "
        "presenta la arquitectura general y la relación entre presentación, API, "
        "servicios, datos y proveedores externos.\n\n"
        "[IMAGEN:arquitectura_general]\n\n"
        "**Figura 4.2. Arquitectura general del sistema Transportes Génesis**",
    )
    text = text.replace(
        "El sistema identifica una ubicación como reciente según reglas temporales. "
        "Puede clasificar el bus como detenido, en movimiento o sin señal. Las alertas "
        "se calculan por distancia aproximada. No se utilizan geocercas poligonales.",
        "La Figura 4.4 resume el flujo de geolocalización desde la captura del GPS "
        "del navegador hasta la actualización de las pantallas mediante SignalR.\n\n"
        "[IMAGEN:geolocalizacion]\n\n"
        "**Figura 4.4. Geolocalización en tiempo real y distribución mediante SignalR**\n\n"
        "El sistema identifica una ubicación como reciente según reglas temporales. "
        "Puede clasificar el bus como detenido, en movimiento o sin señal. Las alertas "
        "se calculan por distancia aproximada. No se utilizan geocercas poligonales.",
    )
    text = text.replace(
        "El término «registro de abordaje» se utiliza como concepto operativo. "
        "El sistema no verifica identidad mediante hardware y depende de la acción "
        "del usuario autorizado. Esta limitación será comunicada a los participantes.",
        "La Figura 4.5 relaciona la confirmación diaria, la consulta de la ruta, la "
        "transmisión GPS y el registro de recogida efectuado por el monitor.\n\n"
        "[IMAGEN:asistencia_abordaje]\n\n"
        "**Figura 4.5. Flujo de asistencia y abordaje durante la operación diaria**\n\n"
        "El término «registro de abordaje» se utiliza como concepto operativo. "
        "El sistema no verifica identidad mediante hardware y depende de la acción "
        "del usuario autorizado. Esta limitación será comunicada a los participantes.",
    )
    text = text.replace(
        "La pasarela Stripe se considera complementaria. Si no se encuentra habilitada "
        "con credenciales de prueba, se documentará su exclusión y no se presentará "
        "como resultado validado.",
        "La Figura 4.6 documenta el flujo diseñado para registro, revisión y "
        "conciliación. La validación administrativa y los estados finales se presentan "
        "como diseño de referencia; no como resultado UAT aprobado.\n\n"
        "[IMAGEN:pagos_conciliacion]\n\n"
        "**Figura 4.6. Flujo propuesto de pagos y conciliación administrativa**\n\n"
        "La pasarela Stripe se considera complementaria. Si no se encuentra habilitada "
        "con credenciales de prueba, se documentará su exclusión y no se presentará "
        "como resultado validado.",
    )

    chapter5_pending = """# CAPÍTULO 5

# VALIDACIÓN Y RESULTADOS

## 5.1 Propósito y estado

Este capítulo está preparado para recibir la evidencia que genere el responsable de pruebas UAT. A la fecha de esta versión, no se han recibido scripts, reportes consolidados, videos, fotografías ni acta de aceptación. Por ética académica, todos los resultados permanecen como pendientes y no se incluyen porcentajes ni testimonios ficticios.

## 5.2 Preparación del ambiente

**Cuadro 5.1. Datos del ambiente**

| Campo | Valor |
|---|---|
| Fecha y lugar | Pendiente |
| Versión o commit probado | Pendiente |
| URL del ambiente | Pendiente |
| Servidor y base de datos | Pendiente |
| Navegadores y dispositivos | Pendiente |
| Datos ficticios preparados | Pendiente |
| Responsable de ejecución | Pendiente |
| Participantes por rol | Pendiente |
| Consentimientos y autorizaciones | Pendiente |

## 5.3 Registro de ejecución UAT

**Cuadro 5.2. Resultados de escenarios**

| Caso | Rol | Estado | Tiempo | Defecto relacionado | Enlace de evidencia |
|---|---|---|---|---|---|
| UAT-01 Inicio de sesión | Padre | Pendiente | Pendiente | Pendiente | [PEGAR URL] |
| UAT-02 Mapa y controles | Padre | Pendiente | Pendiente | Pendiente | [PEGAR URL] |
| UAT-03 Asistencia por fecha | Padre | Pendiente | Pendiente | Pendiente | [PEGAR URL] |
| UAT-04 Registro de recogida | Monitor | Pendiente | Pendiente | Pendiente | [PEGAR URL] |
| UAT-05 Pausa y reanudación | Piloto/monitor | Pendiente | Pendiente | Pendiente | [PEGAR URL] |
| UAT-06 Registro de pago | Padre | Pendiente | Pendiente | Pendiente | [PEGAR URL] |
| UAT-07 Validación de pago | Administrador | Pendiente | Pendiente | Pendiente | [PEGAR URL] |
| UAT-08 Historial de pagos | Padre | Pendiente | Pendiente | Pendiente | [PEGAR URL] |
| UAT-09 Gestión administrativa | Administrador | Pendiente | Pendiente | Pendiente | [PEGAR URL] |
| UAT-10 Flujo integrado | Todos | Pendiente | Pendiente | Pendiente | [PEGAR URL] |

La tasa de éxito solo se calculará cuando los casos tengan resultado verificable. Los casos bloqueados se separarán de los rechazados y se explicará la causa.

## 5.4 Observaciones cualitativas

Se resumirán comentarios espontáneos y dificultades observadas por módulo. Cada comentario se anonimizará y se asociará al caso correspondiente. No se presentará como resultado de encuesta ni se calcularán promedios de opinión.

**Cuadro 5.3. Registro de observaciones**

| Código | Caso | Observación | Impacto | Acción | Evidencia |
|---|---|---|---|---|---|
| OBS-01 | Pendiente | Pendiente | Pendiente | Pendiente | [PEGAR URL] |

## 5.5 Scripts y reportes automatizados

Esta sección se completará cuando el compañero responsable entregue los archivos. Los enlaces deberán apuntar a una versión identificable y accesible para el asesor.

**Cuadro 5.4. Enlaces de automatización**

| Artefacto | Herramienta | Versión/fecha | Estado | Enlace |
|---|---|---|---|---|
| Scripts UAT automatizados | Selenium | Pendiente | Pendiente | [PEGAR URL AL REPOSITORIO O CARPETA] |
| Reporte de ejecución Selenium | Pendiente | Pendiente | Pendiente | [PEGAR URL AL REPORTE] |
| Scripts de carga | JMeter, k6 u otra | Pendiente | Pendiente | [PEGAR URL AL SCRIPT] |
| Reporte de carga y rendimiento | Pendiente | Pendiente | Pendiente | [PEGAR URL AL REPORTE] |
| Logs técnicos anonimizados | Pendiente | Pendiente | Pendiente | [PEGAR URL A LOS LOGS] |

## 5.6 Evidencia audiovisual

Los videos y fotografías se almacenarán fuera del documento para evitar un archivo Word pesado. En la tesis se incluirán únicamente enlaces, descripción, fecha y caso relacionado. Antes de publicar, se ocultarán nombres, rostros de menores, direcciones, coordenadas, credenciales y comprobantes.

**Cuadro 5.5. Enlaces de videos y fotografías**

| ID | Tipo | Descripción | Caso relacionado | Fecha | Enlace |
|---|---|---|---|---|---|
| VID-01 | Video | Ejecución UAT completa | UAT-__ | Pendiente | [PEGAR URL] |
| VID-02 | Video | Flujo integrado de cierre | UAT-10 | Pendiente | [PEGAR URL] |
| FOTO-01 | Fotografía | Ambiente y participantes anonimizados | Sesión UAT | Pendiente | [PEGAR URL] |
| FOTO-02 | Fotografía | Evidencia adicional | Pendiente | Pendiente | [PEGAR URL] |

## 5.7 Bitácora y aceptación

**Cuadro 5.6. Documentos de cierre**

| Documento | Contenido mínimo | Estado | Enlace |
|---|---|---|---|
| Bitácora de defectos | ID, severidad, pasos, estado, responsable y evidencia | Pendiente | [PEGAR URL] |
| Matriz requisito-caso | Requisito, caso, resultado y evidencia | Pendiente | [PEGAR URL] |
| Acta de aceptación UAT | Fecha, participantes, alcance, pendientes y firmas | Pendiente | [PEGAR URL] |
| Resumen de regresión | Defectos corregidos y casos repetidos | Pendiente | [PEGAR URL] |

## 5.8 Resultados técnicos

**Cuadro 5.7. Pruebas de carga y rendimiento**

| Indicador | Criterio | Resultado | Estado | Evidencia |
|---|---|---|---|---|
| Percentil 95 | Menor de 3 segundos | Pendiente | Pendiente | [PEGAR URL] |
| Tasa de error | Menor de 1 % | Pendiente | Pendiente | [PEGAR URL] |
| Estabilidad del mapa | 30 minutos sin caída crítica | Pendiente | Pendiente | [PEGAR URL] |
| Carga de comprobantes | Diez usuarios concurrentes sin error crítico | Pendiente | Pendiente | [PEGAR URL] |
| Uso de CPU y memoria | Registrar durante la prueba | Pendiente | Pendiente | [PEGAR URL] |

Se documentarán herramienta, configuración, número de iteraciones, hardware, red y versión del sistema para permitir reproducción.

## 5.9 Evaluación de la hipótesis

**Cuadro 5.8. Matriz de decisión**

| Componente | Evidencia requerida | Resultado | Decisión |
|---|---|---|---|
| Cumplimiento funcional | Matriz UAT | Pendiente | Pendiente |
| Control por roles | Casos positivos y negativos | Pendiente | Pendiente |
| Geolocalización estable | UAT, logs y video | Pendiente | Pendiente |
| Asistencia y abordaje persistentes | UAT y consulta de datos | Pendiente | Pendiente |
| Flujo de pagos trazable | UAT y registros | Pendiente | Pendiente |
| Rendimiento | Reporte de carga | Pendiente | Pendiente |

La hipótesis podrá quedar respaldada, respaldada parcialmente o no respaldada. La decisión se explicará por componente.

## 5.10 Discusión

La discusión se completará con evidencia real y analizará la relación entre resultados UAT y técnicos, defectos críticos, efecto de la conectividad, diferencias entre roles, limitaciones del ambiente y cambios realizados después de la retroalimentación.

## 5.11 Amenazas y limitaciones observadas

Esta sección se completará con incidentes reales. Como mínimo se revisarán cantidad y diversidad de participantes, ambiente controlado, uso de simulación, duración, conectividad, intervención del observador y disponibilidad futura de los enlaces.

## 5.12 Resumen del capítulo

El capítulo queda estructurado para incorporar resultados y enlaces sin alterar la lógica metodológica. Mientras los campos permanezcan pendientes, no deberá afirmarse que la UAT fue aprobada ni que la hipótesis fue respaldada.

"""
    chapter5 = """# CAPÍTULO 5

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
| Piloto/monitor | Geolocalización activa; equilibrar la visualización entre ruta y alumnos | Geolocalización respaldada por PM-01 y PM-02; ajuste de interfaz pendiente de prueba |
| Padre | Asistencia limitada al día confirmado | Respaldada visualmente; persistencia pendiente de prueba |
| Padre | Mayor percepción de seguridad por geolocalización | Comentario cualitativo reportado; no cuantificado |

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

## 5.9 Evaluación de la hipótesis

La evidencia respalda que la plataforma integra visualmente funciones de administración, geolocalización, asistencia y traslados, y que usuarios pueden interactuar con ellas en un ambiente controlado. También existe retroalimentación favorable respecto de la información geográfica y de las correcciones al calendario.

No obstante, la hipótesis incluye una mejora en eficiencia y seguridad. La reducción de tiempos no fue medida contra un proceso anterior; la percepción de seguridad no fue recolectada mediante un instrumento estructurado; y el abordaje no dispone de evidencia específica. Asimismo, el material no constituye una UAT cerrada con resultados por caso y acta de aceptación.

Por lo anterior, **la hipótesis queda parcialmente respaldada en el nivel funcional y cualitativo, pero no puede confirmarse de manera definitiva**. No corresponde rechazar la hipótesis nula con la evidencia disponible. La confirmación global requiere documentar el abordaje, completar la matriz UAT, obtener mediciones comparables de eficiencia y registrar de forma estructurada la percepción de seguridad.

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

"""
    text = replace_section(text, "# CAPÍTULO 5", "# CAPÍTULO 6", chapter5)

    chapter6_pending = """# CAPÍTULO 6

# CONCLUSIONES Y TRABAJO FUTURO

## 6.1 Estado de las conclusiones

Las conclusiones definitivas se redactarán después de ejecutar UAT y pruebas técnicas. En esta versión no corresponde afirmar que la hipótesis fue comprobada.

## 6.2 Matriz para conclusiones

**Cuadro 6.1. Relación entre objetivos y conclusiones**

| Objetivo | Evidencia | Conclusión |
|---|---|---|
| Analizar procesos y requisitos | Revisión documental y entrevistas operativas | Pendiente |
| Implementar geolocalización | Código, UAT, logs y video | Pendiente |
| Implementar asistencia y abordaje | Código, UAT y persistencia | Pendiente |
| Implementar pagos | Código, UAT e historial | Pendiente |
| Integrar roles y módulos | Flujo UAT integrado | Pendiente |
| Validar aceptación operativa | Matriz UAT y acta | Pendiente |
| Evaluar rendimiento | Scripts y reportes técnicos | Pendiente |
| Organizar evidencia | Matriz de enlaces | Pendiente |

Cada conclusión deberá responder a un objetivo y citar resultados del Capítulo 5.

## 6.3 Conclusiones técnicas preliminares

La revisión del artefacto permite describir que la plataforma integra componentes de autenticación por roles, gestión operativa, ubicación, asistencia, recogidas y pagos. Esta conclusión se refiere a la existencia de componentes en el código, no a su aceptación por usuarios.

También se identifican restricciones que deben verificarse: uso de simulación para parte del seguimiento, persistencia de asistencia, cobertura de autorización, carácter manual del abordaje y dependencia del navegador para alertas. Ninguna de estas observaciones sustituye la ejecución UAT.

## 6.4 Recomendaciones

1. No completar estados, porcentajes o conclusiones hasta recibir evidencia verificable.
2. Ejecutar UAT con usuarios finales y no únicamente con desarrolladores.
3. Conservar una relación uno a uno entre requisito, caso, defecto y enlace.
4. Anonimizar videos y fotografías antes de compartirlos.
5. Corregir defectos críticos y ejecutar regresión antes del acta de cierre.
6. Solicitar al asesor aprobación explícita de la omisión de encuestas y de los criterios técnicos.
7. Verificar periódicamente que los enlaces sean accesibles con permisos de solo lectura.
8. Actualizar todos los índices y campos de Word antes de entregar.

## 6.5 Trabajo futuro

1. Sustituir o complementar la simulación con captura GPS continua y un dispositivo controlado.
2. Implementar aplicación móvil o notificaciones push con consentimiento.
3. Incorporar confirmación de abordaje mediante un mecanismo verificable, evaluando privacidad y costo.
4. Unificar el modelo de pagos y fortalecer el flujo administrativo.
5. Persistir de forma consistente las alertas relevantes.
6. Evaluar rutas con datos de tráfico y métodos de optimización adicionales.
7. Ampliar la validación a más rutas, grados y períodos de uso.
8. Aplicar auditorías de seguridad y protección de datos antes de producción.

## 6.6 Cierre

Transportes Génesis constituye una propuesta tecnológica para centralizar información del transporte escolar. Su valor académico dependerá de la correspondencia entre lo implementado, lo probado y lo concluido. La versión final deberá incorporar evidencia UAT y técnica real, así como reconocer defectos y limitaciones.

"""
    chapter6 = """# CAPÍTULO 6

# CONCLUSIONES Y TRABAJO FUTURO

## 6.1 Estado de las conclusiones

Las conclusiones se formulan únicamente a partir del código documentado, las fotografías, los videos y la retroalimentación proporcionada. Se distinguen resultados funcionales de impactos que aún no fueron medidos.

## 6.2 Respuesta a los objetivos específicos

**Cuadro 6.1. Relación entre objetivos, evidencia y conclusión**

| Objetivo | Evidencia | Conclusión |
|---|---|---|
| Implementar geolocalización | Mapas de administrador, padre y piloto o monitor; video de marcador | Implementado y demostrado en ambiente controlado |
| Desarrollar asistencia y abordaje | Calendario, confirmaciones y traslado temporal | Asistencia demostrada; abordaje requiere evidencia específica |

## 6.3 Conclusiones principales

1. La plataforma centraliza funciones administrativas y de consulta que antes podían encontrarse dispersas. La evidencia muestra paneles diferenciados para administrador, padre y piloto o monitor.
2. La geolocalización se encuentra implementada funcionalmente. El material muestra mapas, paradas, rutas y desplazamiento visual del marcador del bus en una demostración controlada.
3. El calendario permite representar estados de asistencia por fecha y turno. La evidencia es consistente con la corrección que limita la confirmación al día seleccionado, aunque la persistencia debe verificarse mediante consulta y recarga controlada.
4. No se presentó evidencia específica del registro de abordaje por parte del monitor. Por ello, este componente no puede considerarse validado solo con el material entregado.
5. El módulo de pagos aparece integrado en la navegación y está respaldado por código y modelo de datos. Sin un video del flujo completo ni registros de tiempo, no puede concluirse que haya reducido la duración de la gestión o aumentado la confianza.
6. Las fotografías y videos constituyen evidencia útil de demostración y retroalimentación, pero no sustituyen una UAT formal con participantes identificados mediante códigos, casos ejecutados, tiempos, defectos y acta de aceptación.

## 6.4 Conclusión sobre la hipótesis

La hipótesis sostiene que la integración de la geolocalización y del módulo de asistencia y abordaje mejora la eficiencia operativa y la percepción de seguridad. El material respalda parcialmente la integración y el funcionamiento de geolocalización y asistencia, además de registrar un indicio cualitativo favorable sobre la información disponible para los padres.

La ausencia de mediciones antes y después, evidencia específica de abordaje, resultados estructurados de percepción y cierre UAT impide confirmar la mejora global. Por tanto, **la hipótesis se considera parcialmente respaldada, pero no confirmada de manera definitiva**. La evidencia disponible tampoco permite rechazar formalmente la hipótesis nula.

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

"""
    text = replace_section(text, "# CAPÍTULO 6", "# GLOSARIO", chapter6)

    references = """# REFERENCIAS

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

"""
    text = replace_section(text, "# REFERENCIAS", "# APÉNDICE A", references)

    appendices = """# APÉNDICE A

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

# APÉNDICE H

# DIAGRAMAS DE ARQUITECTURA Y FLUJOS

Los diagramas se extrajeron de los documentos ubicados en `Documentos/Proyecto/final/Presentacion_final_20062026`. Se integraron y referenciaron en el Capítulo 4 para evitar duplicarlos:

| Figura | Contenido | Archivo local |
|---|---|---|
| Figura 4.2 | Arquitectura general del sistema | `diagramas_integrados/figura_4_2_arquitectura_general.png` |
| Figura 4.4 | Geolocalización en tiempo real | `diagramas_integrados/figura_4_4_geolocalizacion.png` |
| Figura 4.5 | Asistencia y abordaje | `diagramas_integrados/figura_4_5_asistencia_abordaje.png` |
| Figura 4.6 | Pagos y conciliación propuestos | `diagramas_integrados/figura_4_6_pagos_conciliacion.png` |

El flujo de pagos se conserva como documentación de diseño complementaria. Su inclusión no equivale a validación UAT del proceso completo.
"""
    text = text[: text.index("# APÉNDICE A")] + appendices.rstrip() + "\n"

    text = text.replace(
        "**Likert:** escala de respuesta ordenada utilizada para medir percepción o acuerdo.\n\n",
        "",
    )
    return text


def write_readme() -> None:
    README_OUT.write_text(
        """# Tesis Transportes Génesis — versión 4

Esta versión conserva el formato institucional de la tesis anterior e incorpora
el análisis verificable de las fotografías y videos entregados.

## Decisión metodológica

La encuesta no es obligatoria en la guía de validación: se deben seleccionar dos
o tres métodos complementarios. Se eligieron:

- UAT y observación directa con usuarios finales.
- Pruebas funcionales automatizadas con Selenium.
- Pruebas de carga y rendimiento.

La hipótesis, objetivos, resultados y conclusiones distinguen entre
funcionamiento observado e impacto medido. La evidencia respalda parcialmente
la hipótesis en el nivel funcional y cualitativo. No se afirma reducción de
tiempos, satisfacción cuantificada ni seguridad física sin medición específica.

## Repositorio audiovisual institucional

Acceso para el grupo de la universidad:

`https://drive.google.com/drive/folders/1d1_k0cRqK9md96MxalGPg3LT3fl4RPZg?usp=sharing`

La copia local está en:

`Documentos/Tesis/imagenes _documentos_transporte_genesis`

## Archivos

- `TESIS_TRANSPORTES_GENESIS_v4.docx`: documento Word.
- `TESIS_TRANSPORTES_GENESIS_v4.md`: fuente editable.
- `generar_tesis_v4.py`: regenera ambos documentos.
- `diagramas_integrados/`: figuras extraídas de la presentación y el Word del proyecto.

## Evidencias pendientes

Todavía faltan scripts, reportes técnicos, logs, bitácora, matriz formal UAT,
evidencia completa de pagos y acta de aceptación. Anonimizar toda evidencia
antes de compartirla.

## Antes de entregar

1. Completar datos de portada y cartas.
2. Obtener visto bueno del asesor para omitir encuestas.
3. Completar la matriz UAT y el flujo audiovisual de pagos.
4. Incorporar mediciones antes/después si se afirmará mejora de eficiencia.
5. En Word, presionar `Ctrl + A`, `F9` y actualizar toda la tabla.
6. Revisar índices, páginas impares, ortografía y permisos de los enlaces.
""",
        encoding="utf-8",
    )


def generate_docx() -> None:
    spec = importlib.util.spec_from_file_location("generador_base", GENERATOR_BASE)
    if spec is None or spec.loader is None:
        raise RuntimeError("No fue posible cargar el generador base.")
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    module.SOURCE = SOURCE_OUT
    document = module.build_document()
    for paragraph in document.paragraphs:
        marker = paragraph.text.strip()
        if not marker.startswith("[IMAGEN:") or not marker.endswith("]"):
            continue
        key = marker[8:-1]
        image_path = DIAGRAM_FILES.get(key)
        if image_path is None or not image_path.exists():
            raise FileNotFoundError(f"No existe el diagrama requerido: {image_path}")
        paragraph.clear()
        paragraph.alignment = WD_ALIGN_PARAGRAPH.CENTER
        paragraph.paragraph_format.first_line_indent = Cm(0)
        paragraph.paragraph_format.line_spacing_rule = WD_LINE_SPACING.SINGLE
        paragraph.paragraph_format.space_after = Pt(3)
        paragraph.add_run().add_picture(str(image_path), width=Cm(15))
    document.save(DOCX_OUT)


def extract_word_diagrams() -> None:
    from docx import Document

    DIAGRAMS_DIR.mkdir(parents=True, exist_ok=True)
    source = Document(SOURCE_DIAGRAM_DOCX)
    relation_map = {
        "rId36": DIAGRAM_FILES["arquitectura_general"],
        "rId25": DIAGRAM_FILES["asistencia_abordaje"],
        "rId26": DIAGRAM_FILES["pagos_conciliacion"],
    }
    for relation_id, output_path in relation_map.items():
        output_path.write_bytes(source.part.rels[relation_id].target_part.blob)

    geolocation_export = DIAGRAMS_DIR / "figura_4_3_geolocalizacion.png"
    if geolocation_export.exists():
        geolocation_export.replace(DIAGRAM_FILES["geolocalizacion"])
    if not DIAGRAM_FILES["geolocalizacion"].exists():
        raise FileNotFoundError(
            "Falta exportar la diapositiva 6 como figura de geolocalización."
        )


def main() -> None:
    extract_word_diagrams()
    SOURCE_OUT.write_text(build_markdown(), encoding="utf-8")
    write_readme()
    generate_docx()
    print(f"Fuente: {SOURCE_OUT}")
    print(f"Word: {DOCX_OUT}")
    print(f"Guía: {README_OUT}")


if __name__ == "__main__":
    main()
