"""Genera la tesis final del 11 de agosto de 2026.

La fuente histórica v4 se conserva sin cambios. Este script construye una
fuente final alineada con la evidencia disponible, genera el DOCX con el
formato institucional y exporta el PDF mediante Microsoft Word.
"""

from __future__ import annotations

import importlib.util
import re
import subprocess
from pathlib import Path
from urllib.parse import quote

from docx.enum.style import WD_STYLE_TYPE
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.enum.text import (
    WD_ALIGN_PARAGRAPH,
    WD_LINE_SPACING,
    WD_TAB_ALIGNMENT,
    WD_TAB_LEADER,
)
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.opc.constants import RELATIONSHIP_TYPE
from docx.shared import Cm, Pt
from docx.text.paragraph import Paragraph


ROOT = Path(__file__).resolve().parents[3]
BASE = Path(__file__).resolve().parent
SOURCE_BASE = BASE / "TESIS_TRANSPORTES_GENESIS_v4.md"
SOURCE_OUT = BASE / "TESIS_TRANSPORTES_GENESIS_FINAL_11_AGOSTO.md"
DOCX_OUT = BASE / "TESIS_TRANSPORTES_GENESIS_FINAL_11_AGOSTO.docx"
PDF_OUT = BASE / "TESIS_TRANSPORTES_GENESIS_FINAL_11_AGOSTO.pdf"
README_OUT = BASE / "README.md"
SUMMARY_OUT = BASE / "RESUMEN_ENTREGA_FINAL.md"
CHECKLIST_OUT = BASE / "CHECKLIST_FINAL.md"
GENERATOR_BASE = (
    ROOT / "Documentos" / "Tesis" / "versioniniicial_tesis" / "generar_tesis.py"
)
DIAGRAMS_DIR = BASE / "diagramas_integrados"
DIAGRAM_FILES = {
    "arquitectura_general": DIAGRAMS_DIR / "figura_4_2_arquitectura_general.png",
    "geolocalizacion": DIAGRAMS_DIR / "figura_4_4_geolocalizacion.png",
    "asistencia_abordaje": DIAGRAMS_DIR / "figura_4_5_asistencia_abordaje.png",
    "pagos_conciliacion": DIAGRAMS_DIR / "figura_4_6_pagos_conciliacion.png",
}

GITHUB_FOLDER = (
    "https://github.com/johnsvill/TransportesGenesis/tree/"
    "dev_intermedia_merge/Documentos/Tesis/"
    "imagenes%20_documentos_transporte_genesis"
)
GITHUB_BLOB_BASE = (
    "https://github.com/johnsvill/TransportesGenesis/blob/"
    "dev_intermedia_merge/Documentos/Tesis/"
    "imagenes%20_documentos_transporte_genesis/"
)


def evidence_url(relative_path: str) -> str:
    return GITHUB_BLOB_BASE + quote(relative_path.replace("\\", "/"), safe="/")


VIDEO_ROUTE = evidence_url("piloto/video/1) Uso_rutas_piloto_monitor.mp4")
VIDEO_PARENT_MAP = evidence_url(
    "padres_de_familia/videos_pantalla_padres_familia/"
    "1) Video_uso_del_mapa_padres_familia.mp4"
)
VIDEO_PARENT_CALENDAR = evidence_url(
    "padres_de_familia/videos_pantalla_padres_familia/"
    "2) Uso_del_calendraio_traslado_padres_familia.mp4"
)
VIDEO_PARENT_ATTENDANCE = evidence_url(
    "padres_de_familia/videos_pantalla_padres_familia/"
    "3) confirmaciones_asistencia_Alumnos_padre_familia.mp4"
)


def replace_section(text: str, start: str, end: str, replacement: str) -> str:
    start_index = text.index(start)
    end_index = text.index(end, start_index)
    return text[:start_index] + replacement.rstrip() + "\n\n" + text[end_index:]


def build_final_markdown() -> str:
    text = SOURCE_BASE.read_text(encoding="utf-8")
    old_title = (
        "Implementación de una plataforma web integrada de geolocalización, "
        "registro de abordaje y gestión digital de pagos para Transportes Génesis"
    )
    new_title = (
        "Implementación y validación funcional de una plataforma web de "
        "geolocalización, asistencia y abordaje para Transportes Génesis"
    )
    text = text.replace(old_title, new_title)
    text = text.replace(old_title.upper(), new_title.upper())
    text = text.replace("# CARÁTULA\n\n", "")
    text = text.replace(
        'facultad: "[Nombre de la facultad o escuela]"',
        'facultad: "PENDIENTE DE COMPLETAR POR LOS AUTORES"',
    )
    text = text.replace(
        'carne: "[Número de carné]"',
        'carne: "PENDIENTE DE COMPLETAR POR LOS AUTORES"',
    )
    text = text.replace(
        'asesor: "[Nombre completo del asesor]"',
        'asesor: "PENDIENTE DE COMPLETAR POR LOS AUTORES"',
    )
    text = text.replace(
        "estado: Borrador integral con evidencia audiovisual; cierre UAT pendiente",
        "estado: Versión final de contenido; datos administrativos pendientes",
    )
    text = text.replace(
        "**[NOMBRE DE LA FACULTAD O ESCUELA]**",
        "**PENDIENTE DE COMPLETAR: FACULTAD O ESCUELA**",
    )
    text = text.replace(
        "Carné: **[NÚMERO DE CARNÉ]**",
        "Carné(s): **PENDIENTE DE COMPLETAR POR LOS AUTORES**",
    )
    text = text.replace(
        "**[GRADO O TÍTULO ACADÉMICO]**",
        "**PENDIENTE DE COMPLETAR: GRADO O TÍTULO ACADÉMICO**",
    )
    text = text.replace(
        "el grado o título de **[GRADO O TÍTULO ACADÉMICO]**",
        "el grado o título académico pendiente de completar por los autores",
    )
    text = text.replace(
        "# DEDICATORIA\n\n"
        "Sección opcional. El autor podrá completar esta página antes de la entrega final.\n\n"
        "<!-- PAGE_BREAK -->\n\n",
        "",
    )
    text = text.replace(
        "# PREFACIO Y AGRADECIMIENTOS\n\n"
        "Sección opcional. El autor podrá reconocer la colaboración de Transportes "
        "Génesis, del centro educativo, de los padres participantes, del asesor y "
        "de las personas que contribuyeron a la investigación.\n\n"
        "<!-- PAGE_BREAK -->\n\n",
        "",
    )
    text = text.replace(
        "# AUTORIZACIÓN DEL DECANO\n\n"
        "**Estado:** pendiente de insertar la autorización oficial para publicar "
        "el trabajo de graduación.\n\n"
        "<!-- PAGE_BREAK -->",
        "# AUTORIZACIÓN DEL DECANO\n\n"
        "**Estado:** pendiente de insertar la autorización oficial para publicar "
        "el trabajo de graduación.\n\n"
        "<!-- PAGE_BREAK -->\n\n"
        "# ACTA DE ACEPTACIÓN DE USUARIOS\n\n"
        "**Proyecto:** Implementación y validación funcional de una plataforma web "
        "de geolocalización, asistencia y abordaje para Transportes Génesis.\n\n"
        "**Lugar:** ____________________________________    "
        "**Fecha:** ____ / ____ / ______\n\n"
        "Por medio de la presente, las personas firmantes dejan constancia de que "
        "participaron en la revisión funcional de la plataforma Transportes Génesis. "
        "Durante la sesión se explicaron el propósito del sistema, las funciones "
        "presentadas y las limitaciones del ambiente de prueba.\n\n"
        "**Funciones revisadas:**\n\n"
        "[ ] Rutas y paradas    [ ] Geolocalización y mapa    "
        "[ ] Asistencia diaria    [ ] Abordaje\n\n"
        "**Resultado de la revisión:**\n\n"
        "[ ] Aceptado para el alcance funcional presentado.\n\n"
        "[ ] Aceptado con observaciones.\n\n"
        "[ ] Requiere correcciones antes de su aceptación.\n\n"
        "**Observaciones:**\n\n"
        "________________________________________________________________________\n\n"
        "________________________________________________________________________\n\n"
        "________________________________________________________________________\n\n"
        "**Firmas de usuarios representativos:**\n\n"
        "| Rol | Nombre completo | Firma |\n"
        "|---|---|---|\n"
        "| Padre, madre o encargado | ______________________________ | ____________________ |\n"
        "| Piloto | ______________________________ | ____________________ |\n"
        "| Monitor | ______________________________ | ____________________ |\n"
        "| Administrador | ______________________________ | ____________________ |\n\n"
        "**Responsables del proyecto:**\n\n"
        "| Nombre | Firma |\n"
        "|---|---|\n"
        "| Jonathan Samuel Villeda Pérez | ______________________________ |\n"
        "| José David Florián Secaida | ______________________________ |\n\n"
        "Las firmas consignadas certifican únicamente la revisión y aceptación del "
        "alcance funcional presentado; no constituyen certificación de producción, "
        "rendimiento ni seguridad física del servicio.\n\n"
        "<!-- PAGE_BREAK -->",
    )

    summary = """# RESUMEN

El presente trabajo documenta la implementación y la validación funcional de una plataforma web para Transportes Génesis, empresa dedicada al transporte escolar en San Cristóbal, departamento de San Marcos, Guatemala. El problema abordado es la dispersión de información sobre rutas, ubicaciones, asistencia y recogidas, que dificulta el seguimiento operativo del servicio.

La solución integra autenticación por roles, administración de buses, rutas y paradas, geolocalización en tiempo casi real, confirmación diaria de asistencia y registro operativo de recogidas. El sistema utiliza ASP.NET Core 8, SQL Server, Entity Framework Core, ASP.NET Core Identity, SignalR, Leaflet, OpenStreetMap y OSRM. El módulo de pagos se conserva como función complementaria y no forma parte de la hipótesis funcional evaluada.

La investigación es aplicada, descriptiva y evaluativa, bajo un estudio de caso tecnológico. La evidencia comprende un informe de pruebas fechado el 4 de agosto de 2026, que declara aprobados dos casos funcionales de rutas y paradas, y material audiovisual de los perfiles administrador, padre, piloto y monitor. Las capturas del informe respaldan los casos solo de forma parcial: una muestra un resultado de cálculo y otra un mapa de detalle, pero no demuestran por completo la edición y persistencia esperadas. También se revisaron pantallas de asistencia, geolocalización y pagos. Los resultados respaldan la integración funcional de rutas, mapas, ubicación observable y confirmación de asistencia en un ambiente controlado. El abordaje, la persistencia de algunos registros, el rendimiento y la aceptación formal por todos los perfiles no cuentan con evidencia suficiente para una afirmación general.

La hipótesis de investigación queda respaldada dentro del alcance funcional documentado: la plataforma centraliza la consulta de rutas y ubicación y permite representar la confirmación diaria de asistencia. Esta conclusión no equivale a certificar despliegue productivo, reducción de tiempos, seguridad física ni aceptación integral del sistema.

**Palabras clave:** transporte escolar, geolocalización, asistencia, abordaje, SignalR, pruebas funcionales, trazabilidad.

<!-- PAGE_BREAK -->"""
    text = replace_section(text, "# RESUMEN", "# ÍNDICE GENERAL", summary)

    chapter1 = """# CAPÍTULO 1

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
"""
    text = replace_section(text, "# CAPÍTULO 1", "# CAPÍTULO 2", chapter1)

    chapter2 = """# CAPÍTULO 2

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
"""
    text = replace_section(text, "# CAPÍTULO 2", "# CAPÍTULO 3", chapter2)

    chapter3 = f"""# CAPÍTULO 3

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

El material audiovisual publicado se encuentra en [Evidencias audiovisuales]({GITHUB_FOLDER}). El PDF UAT se conserva únicamente en el archivo local del proyecto y no se enlaza debido a su clasificación interna y a la presencia de una credencial que debe redactarse.

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
"""
    text = replace_section(text, "# CAPÍTULO 3", "# CAPÍTULO 4", chapter3)

    text = text.replace(
        "SQL Server (versión del entorno pendiente)",
        "SQL Server 2022 (ambiente de prueba documentado)",
    )
    text = text.replace(
        "El padre consulta meses y montos, registra un pago o carga una boleta y "
        "revisa el historial. La administración puede validar o rechazar registros "
        "según el flujo activo. El proyecto contiene modelos de pagos de distintas "
        "etapas, por lo que la validación se enfocará en el flujo visible y utilizado "
        "durante UAT. La lógica actual de meses deberá probarse para los doce meses "
        "del año, pues la lista configurada no cubre de forma segura todos los casos; "
        "no se afirmará gestión mensual completa mientras esta prueba no sea aprobada.",
        "El padre dispone de pantallas para registrar pagos, cargar comprobantes y "
        "consultar historial. La evidencia entregada permite observar el módulo, pero "
        "el informe UAT excluye integraciones externas y reportes financieros. Por "
        "ello, pagos se documenta como función complementaria y no como componente "
        "confirmado por la hipótesis.",
    )
    text = text.replace(
        "La pasarela Stripe se considera complementaria. Si no se encuentra habilitada "
        "con credenciales de prueba, se documentará su exclusión y no se presentará "
        "como resultado validado.",
        "La pasarela Stripe es complementaria. No se recibió un reporte transaccional "
        "ni una conciliación reproducible, por lo que no se presenta como resultado "
        "validado.",
    )
    text = text.replace(
        "La Figura 4.5 relaciona la confirmación diaria, la consulta de la ruta, la "
        "transmisión GPS y el registro de recogida efectuado por el monitor.",
        "La Figura 4.5 relaciona la confirmación diaria, la consulta de la ruta, la "
        "transmisión GPS y el registro de recogida previsto para el monitor.",
    )
    text = text.replace(
        "El capítulo documentó la arquitectura y el funcionamiento realmente "
        "implementado. Se evitó presentar simulaciones, funciones complementarias "
        "o trabajos futuros como capacidades plenamente validadas.",
        "El capítulo documentó la arquitectura y las funciones implementadas. Los "
        "resultados observados y sus límites se presentan por separado en el "
        "Capítulo 5.",
    )

    chapter5 = f"""# CAPÍTULO 5

# VALIDACIÓN Y RESULTADOS

## 5.1 Alcance de los resultados

Los resultados proceden de dos casos funcionales documentados y de evidencia audiovisual. Las afirmaciones se limitan al ambiente controlado y no equivalen a aceptación integral por usuarios finales ni certificación de producción.

## 5.2 Repositorio externo

La evidencia se consulta en [Repositorio de pruebas UAT, fotografías y videos]({GITHUB_FOLDER}). Los medios no se incrustan en la tesis para evitar exponer datos y aumentar el tamaño del documento.

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

Enlaces externos disponibles: [ruta de piloto o monitor]({VIDEO_ROUTE}) y [mapa del padre]({VIDEO_PARENT_MAP}). Los dos videos adicionales de geolocalización se revisaron desde la copia local, pero no se encuentran en la rama externa indicada.

## 5.5 Resultados de asistencia y abordaje

Los videos del padre muestran calendario, selección de fecha y estados por turno. Esta evidencia respalda la confirmación diaria observable y evita interpretar una selección como confirmación de toda la semana.

Enlaces: [calendario y traslado]({VIDEO_PARENT_CALENDAR}) y [confirmación de asistencia]({VIDEO_PARENT_ATTENDANCE}).

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
"""
    text = replace_section(text, "# CAPÍTULO 5", "# CAPÍTULO 6", chapter5)

    chapter6 = """# CAPÍTULO 6

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
"""
    text = replace_section(text, "# CAPÍTULO 6", "# GLOSARIO", chapter6)

    references = f"""# BIBLIOGRAFÍA

Equipo QA y validadores técnicos regionales. (2026). *Informe de pruebas UAT: TransportesGenesis, versión 1.0* [Documento interno conservado en el archivo local del proyecto].

International Organization for Standardization. (2023). *ISO/IEC 25010:2023 Systems and software engineering—Systems and software Quality Requirements and Evaluation (SQuaRE)—Product quality model*. [ISO]({quote("https://www.iso.org/standard/78176.html", safe=":/")})

Leaflet. (2026). *Leaflet documentation*. [Leaflet]({quote("https://leafletjs.com/reference.html", safe=":/")})

Microsoft. (2024). *Introduction to ASP.NET Core SignalR*. Microsoft Learn. [SignalR]({quote("https://learn.microsoft.com/aspnet/core/signalr/introduction", safe=":/")})

OpenStreetMap contributors. (2026). *OpenStreetMap*. [OpenStreetMap]({quote("https://www.openstreetmap.org/", safe=":/")})

OWASP Foundation. (2021). *OWASP Application Security Verification Standard 4.0.3*. [OWASP ASVS]({quote("https://owasp.org/www-project-application-security-verification-standard/", safe=":/")})

PCI Security Standards Council. (2024). *Payment Card Industry Data Security Standard: Requirements and testing procedures, version 4.0.1*. [PCI SSC]({quote("https://www.pcisecuritystandards.org/", safe=":/")})

Project OSRM. (2026). *Open Source Routing Machine*. [Project OSRM]({quote("https://project-osrm.org/", safe=":/")})

Universidad Galileo, FISICC. (2026a). *Guía para la elaboración del trabajo de graduación: Tesis checklist* [Documento institucional].

Universidad Galileo, FISICC. (2026b). *Guía de validación en ambiente real: checklist para probar un proyecto funcional en la tesis* [Documento orientativo].

World Wide Web Consortium. (2024). *Geolocation: W3C Recommendation 14 August 2024*. [W3C Geolocation]({quote("https://www.w3.org/TR/2024/REC-geolocation-20240814/", safe=":/")})

"""
    text = replace_section(text, "# REFERENCIAS", "# APÉNDICE A", references)

    appendices = f"""# APÉNDICE A

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
| VID-01 | Rutas | [Piloto o monitor]({VIDEO_ROUTE}) | Objetivo 1 |
| VID-02 | Mapa del padre | [Uso del mapa]({VIDEO_PARENT_MAP}) | Objetivo 1 |
| VID-03 | Asistencia | [Calendario y traslado]({VIDEO_PARENT_CALENDAR}) | Objetivo 2 |
| VID-04 | Asistencia | [Confirmaciones]({VIDEO_PARENT_ATTENDANCE}) | Objetivo 2 |
| LOC-01 | Geolocalización adicional | Dos videos locales sin enlace externo | Objetivo 1 |
| LOC-02 | Pagos complementarios | Un video y dos imágenes locales sin enlace externo | Alcance complementario |
| REP-01 | Pruebas | Informe local sin enlace directo | Objetivo 3 |
| REP-02 | Evidencia publicada | [Carpeta audiovisual en GitHub]({GITHUB_FOLDER}) | Objetivos 1–3 |

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
"""
    text = text[: text.index("# APÉNDICE A")] + appendices.rstrip() + "\n"
    return text


TOKEN_RE = re.compile(
    r"(\[[^\]]+\]\(https?://[^)]+\)|\*\*.*?\*\*|\*.*?\*)"
)


def add_hyperlink(paragraph, label: str, url: str) -> None:
    relationship_id = paragraph.part.relate_to(
        url, RELATIONSHIP_TYPE.HYPERLINK, is_external=True
    )
    hyperlink = OxmlElement("w:hyperlink")
    hyperlink.set(qn("r:id"), relationship_id)
    run = OxmlElement("w:r")
    properties = OxmlElement("w:rPr")
    fonts = OxmlElement("w:rFonts")
    fonts.set(qn("w:ascii"), "Times New Roman")
    fonts.set(qn("w:hAnsi"), "Times New Roman")
    fonts.set(qn("w:eastAsia"), "Times New Roman")
    color = OxmlElement("w:color")
    color.set(qn("w:val"), "0563C1")
    underline = OxmlElement("w:u")
    underline.set(qn("w:val"), "single")
    size = OxmlElement("w:sz")
    size.set(qn("w:val"), "24")
    properties.extend([fonts, color, underline, size])
    run.append(properties)
    text = OxmlElement("w:t")
    text.text = label
    run.append(text)
    hyperlink.append(run)
    paragraph._p.append(hyperlink)


def configure_inline_parser(module) -> None:
    def add_inline(paragraph, text: str, *, force_bold: bool = False) -> None:
        for part in TOKEN_RE.split(text):
            if not part:
                continue
            link = re.fullmatch(r"\[([^\]]+)\]\((https?://[^)]+)\)", part)
            if link:
                add_hyperlink(paragraph, link.group(1), link.group(2))
            elif part.startswith("**") and part.endswith("**"):
                run = paragraph.add_run(part[2:-2])
                module.set_run_font(run, bold=True)
            elif part.startswith("*") and part.endswith("*"):
                run = paragraph.add_run(part[1:-1])
                module.set_run_font(run, italic=True)
            else:
                run = paragraph.add_run(part)
                module.set_run_font(run, bold=True if force_bold else None)

    module.add_inline = add_inline


def add_bookmark(paragraph, name: str, bookmark_id: int) -> None:
    start = OxmlElement("w:bookmarkStart")
    start.set(qn("w:id"), str(bookmark_id))
    start.set(qn("w:name"), name)
    end = OxmlElement("w:bookmarkEnd")
    end.set(qn("w:id"), str(bookmark_id))
    insert_at = 1 if paragraph._p.pPr is not None else 0
    paragraph._p.insert(insert_at, start)
    paragraph._p.append(end)


def insert_paragraph_after(paragraph) -> Paragraph:
    element = OxmlElement("w:p")
    paragraph._p.addnext(element)
    return Paragraph(element, paragraph._parent)


def clear_paragraph(paragraph) -> None:
    for child in list(paragraph._p):
        if child.tag != qn("w:pPr"):
            paragraph._p.remove(child)


def write_index_entry(paragraph, caption: str, bookmark: str, module) -> None:
    paragraph.style = "Normal"
    paragraph.alignment = WD_ALIGN_PARAGRAPH.LEFT
    paragraph.paragraph_format.first_line_indent = Cm(0)
    paragraph.paragraph_format.line_spacing_rule = WD_LINE_SPACING.SINGLE
    paragraph.paragraph_format.space_before = Pt(0)
    paragraph.paragraph_format.space_after = Pt(0)
    paragraph.paragraph_format.tab_stops.add_tab_stop(
        Cm(15), WD_TAB_ALIGNMENT.RIGHT, WD_TAB_LEADER.DOTS
    )
    text_run = paragraph.add_run(caption)
    module.set_run_font(text_run, size=Pt(10))
    tab_run = paragraph.add_run("\t")
    module.set_run_font(tab_run, size=Pt(10))
    page_run = paragraph.add_run()
    module.set_run_font(page_run, size=Pt(10))
    module.set_field(page_run, f" PAGEREF {bookmark} \\h ", "0")


def build_manual_index(document, heading: str, entries, module) -> None:
    paragraphs = document.paragraphs
    heading_index = next(
        index for index, paragraph in enumerate(paragraphs)
        if paragraph.text.strip() == heading
    )
    field_paragraph = paragraphs[heading_index + 1]
    clear_paragraph(field_paragraph)
    current = field_paragraph
    for entry_index, (caption, bookmark) in enumerate(entries):
        if entry_index:
            current = insert_paragraph_after(current)
        write_index_entry(current, caption, bookmark, module)
    for paragraph in list(document.paragraphs):
        if paragraph.text.startswith(
            "El índice de cuadros se genera automáticamente"
        ) or paragraph.text.startswith(
            "El índice de figuras se genera automáticamente"
        ):
            paragraph._p.getparent().remove(paragraph._p)


def format_document(document, module) -> None:
    styles = document.styles
    styles["Normal"].paragraph_format.alignment = WD_ALIGN_PARAGRAPH.LEFT
    for style_name in ("List Bullet", "List Number"):
        styles[style_name].paragraph_format.alignment = WD_ALIGN_PARAGRAPH.LEFT
    for style_name in ("Tesis Cuadro", "Tesis Figura"):
        if style_name not in styles:
            style = styles.add_style(style_name, WD_STYLE_TYPE.PARAGRAPH)
        else:
            style = styles[style_name]
        style.font.name = "Times New Roman"
        style._element.get_or_add_rPr().rFonts.set(
            qn("w:eastAsia"), "Times New Roman"
        )
        style.font.size = Pt(10)
        style.font.bold = True
        style.paragraph_format.alignment = WD_ALIGN_PARAGRAPH.CENTER
        style.paragraph_format.first_line_indent = Cm(0)
        style.paragraph_format.line_spacing_rule = WD_LINE_SPACING.SINGLE
        style.paragraph_format.space_after = Pt(3)
        style.paragraph_format.keep_with_next = True

    if len(document.sections) > 1:
        module.set_page_number_format(document.sections[1], "lowerRoman", 8)
    for section in document.sections[3:]:
        page_number = section._sectPr.find(qn("w:pgNumType"))
        if page_number is not None and qn("w:start") in page_number.attrib:
            del page_number.attrib[qn("w:start")]

    for paragraph in document.paragraphs:
        paragraph.alignment = WD_ALIGN_PARAGRAPH.CENTER
        paragraph.paragraph_format.first_line_indent = Cm(0)
        paragraph.paragraph_format.line_spacing_rule = WD_LINE_SPACING.SINGLE
        if paragraph.text.strip().lower() == "guatemala, agosto de 2026":
            break

    for table in document.tables:
        table.alignment = WD_TABLE_ALIGNMENT.CENTER
        for row in table.rows:
            for cell in row.cells:
                for paragraph in cell.paragraphs:
                    paragraph.alignment = WD_ALIGN_PARAGRAPH.JUSTIFY
                    paragraph.paragraph_format.first_line_indent = Cm(0)
                    paragraph.paragraph_format.left_indent = Cm(0)
                    paragraph.paragraph_format.right_indent = Cm(0)
                    paragraph.paragraph_format.space_before = Pt(0)
                    paragraph.paragraph_format.space_after = Pt(0)
                    paragraph.paragraph_format.line_spacing_rule = (
                        WD_LINE_SPACING.SINGLE
                    )
                    for tab in paragraph._p.xpath(".//w:tab"):
                        tab.getparent().remove(tab)

    table_entries = []
    figure_entries = []
    bookmark_id = 1000
    for paragraph in document.paragraphs:
        caption = paragraph.text.strip()
        match = re.match(
            r"^(Cuadro|Figura)\s+(?:\d+|[A-Z])\.\d+\.",
            caption,
            re.IGNORECASE,
        )
        if not match:
            continue
        paragraph.style = (
            "Tesis Cuadro"
            if match.group(1).lower() == "cuadro"
            else "Tesis Figura"
        )
        paragraph.alignment = WD_ALIGN_PARAGRAPH.CENTER
        paragraph.paragraph_format.first_line_indent = Cm(0)
        prefix = "Cuadro" if match.group(1).lower() == "cuadro" else "Figura"
        bookmark = f"{prefix}_{bookmark_id}"
        add_bookmark(paragraph, bookmark, bookmark_id)
        (table_entries if prefix == "Cuadro" else figure_entries).append(
            (caption, bookmark)
        )
        bookmark_id += 1

    build_manual_index(document, "ÍNDICE DE CUADROS", table_entries, module)
    build_manual_index(document, "ÍNDICE DE FIGURAS", figure_entries, module)

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


def generate_docx() -> None:
    spec = importlib.util.spec_from_file_location("generador_base", GENERATOR_BASE)
    if spec is None or spec.loader is None:
        raise RuntimeError("No fue posible cargar el generador base.")
    module = importlib.util.module_from_spec(spec)
    spec.loader.exec_module(module)
    configure_inline_parser(module)
    module.SOURCE = SOURCE_OUT
    document = module.build_document()
    format_document(document, module)
    document.save(DOCX_OUT)


def export_pdf() -> None:
    if PDF_OUT.exists():
        PDF_OUT.unlink()
    docx = str(DOCX_OUT.resolve()).replace("'", "''")
    pdf = str(PDF_OUT.resolve()).replace("'", "''")
    command = (
        "$ErrorActionPreference='Stop'; "
        "$word=New-Object -ComObject Word.Application; "
        "$word.Visible=$false; $word.DisplayAlerts=0; "
        "try { "
        f"$doc=$word.Documents.Open('{docx}'); "
        "$doc.Fields.Update() | Out-Null; "
        "foreach($toc in $doc.TablesOfContents){$toc.Update() | Out-Null}; "
        "foreach($tof in $doc.TablesOfFigures){$tof.Update() | Out-Null}; "
        "$doc.Save(); "
        f"$doc.ExportAsFixedFormat('{pdf}',17,$false,0,0,1,1,0,$true,$true,1,$true,$true,$false); "
        "$doc.Close(); "
        "} finally { $word.Quit() }"
    )
    result = subprocess.run(
        ["powershell", "-NoProfile", "-ExecutionPolicy", "Bypass", "-Command", command],
        check=False,
        capture_output=True,
        text=True,
        timeout=180,
    )
    if result.returncode != 0:
        raise RuntimeError(
            "Word no pudo actualizar campos o exportar PDF:\n"
            + result.stdout
            + result.stderr
        )


def write_support_files() -> None:
    README_OUT.write_text(
        """# Tesis Transportes Génesis — entrega final de contenido

## Entregables

- `TESIS_TRANSPORTES_GENESIS_FINAL_11_AGOSTO.md`
- `TESIS_TRANSPORTES_GENESIS_FINAL_11_AGOSTO.docx`
- `TESIS_TRANSPORTES_GENESIS_FINAL_11_AGOSTO.pdf`
- `generar_tesis_final.py`
- `validar_tesis_final.py`
- `RESUMEN_ENTREGA_FINAL.md`
- `CHECKLIST_FINAL.md`

La versión v5 se conserva como respaldo. La hipótesis final evalúa funcionamiento
observable y no afirma reducción de tiempos, seguridad física ni aceptación
integral. Los medios UAT se referencian mediante enlaces y no se incrustan.

Antes de subir, los autores deben completar facultad, carnés, grado, asesor y
cartas institucionales indicadas en el Apéndice E.

## Regeneración y validación

Ejecutar `python generar_tesis_final.py` y luego
`python validar_tesis_final.py`. La validación revisa Markdown, Word, PDF,
márgenes, tablas, índices, figuras, hipervínculos y ausencia de credenciales.
""",
        encoding="utf-8",
    )
    SUMMARY_OUT.write_text(
        """# Resumen de entrega final

La tesis documenta la implementación y validación funcional de rutas, paradas,
geolocalización y asistencia diaria en Transportes Génesis.

## Resultado principal

- El informe declara aprobados dos de dos casos; sus capturas ofrecen respaldo parcial.
- La geolocalización y la asistencia son observables en evidencia audiovisual.
- El abordaje no cuenta con una prueba específica de ejecución y persistencia.
- Pagos se mantiene como módulo complementario fuera de la hipótesis.
- La hipótesis queda respaldada únicamente dentro del alcance funcional probado.

## Pendiente antes de subir

Completar facultad, carnés, grado, asesor y las tres cartas institucionales.
""",
        encoding="utf-8",
    )
    CHECKLIST_OUT.write_text(
        """# Checklist final

## Contenido académico

- [x] Hipótesis funcional evaluada con evidencia.
- [x] Objetivos respondidos en conclusiones.
- [x] Metodología consistente con el alcance real.
- [x] Dos casos funcionales documentados sin divulgar credenciales.
- [x] Enlaces de videos y repositorio incorporados.
- [x] Bibliografía y citas normalizadas.
- [x] Figuras y cuadros referenciados.
- [x] Evidencia audiovisual no incrustada.

## Formato

- [x] Tamaño carta.
- [x] Márgenes 4 cm superior/izquierdo y 2.5 cm inferior/derecho.
- [x] Times New Roman 12 y doble espacio en texto principal.
- [x] Capítulos y apéndices en página impar.
- [x] Tablas justificadas, sin sangría ni tabulaciones.
- [x] Texto normal alineado a la izquierda por instrucción del usuario.

## Pendientes administrativos

- [ ] Facultad o escuela.
- [ ] Carné de cada autor.
- [ ] Grado o título académico.
- [ ] Nombre del asesor.
- [ ] Carta del asesor.
- [ ] Dictamen del Director de Programas.
- [ ] Autorización del Decano.
- [ ] Carátula amarillo pálido y lomo, si se exige entrega impresa.

La carga institucional no debe realizarse hasta completar los elementos aplicables.
""",
        encoding="utf-8",
    )


def main() -> None:
    SOURCE_OUT.write_text(build_final_markdown(), encoding="utf-8")
    write_support_files()
    generate_docx()
    export_pdf()
    print(f"Markdown: {SOURCE_OUT}")
    print(f"Word: {DOCX_OUT}")
    print(f"PDF: {PDF_OUT}")
    print(f"Checklist: {CHECKLIST_OUT}")


if __name__ == "__main__":
    main()
