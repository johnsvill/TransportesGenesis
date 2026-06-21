"""
Aplica correcciones v2 -> v4 sobre Proyecto_Transportes_Genesis_v4.docx
Mantiene estructura y extensión del documento original.
"""
from pathlib import Path
from docx import Document
from docx.shared import Pt
from docx.enum.text import WD_PARAGRAPH_ALIGNMENT

DOCX = Path(__file__).parent / "Proyecto_Transportes_Genesis_v4.docx"

# Orden: cadenas más específicas primero
REPLACEMENTS = [
    # --- Hub y APIs ---
    ("/geolocalizacionHub", "/notificacionesHub"),
    ("Hubs/GeolocalizacionHub.cs", "Hubs/NotificacionesHub.cs"),
    ("GeolocalizacionHub", "NotificacionesHub"),
    # --- Stripe ---
    ("Stripe (No implementado en v1.0): diseñado para:", "Stripe (implementado en v1.0):"),
    ("Integración futura con pasarelas de pago (Stripe)", "Integración con Stripe para pagos en línea (implementado)"),
    ("[Futuro] Pasarela de Pagos (Stripe, PayPal)", "Stripe API (pagos en línea, implementado)"),
    ("[Futuro] Pasarela de Pagos", "Stripe API (pagos en línea)"),
    # --- Google Maps -> OSM stack ---
    ("Transportes Genesis → Google Maps API: \"Obtiene mapas y geocodificación\"",
     "Transportes Genesis → OpenStreetMap/OSRM/Nominatim: \"Mapas, rutas y geocodificación\""),
    ("- Google Maps API (Geolocalización y mapas)", "- OpenStreetMap + Leaflet + OSRM + Nominatim"),
    ("Google Maps API (HTTPS, puerto 443)", "OpenStreetMap / OSRM / Nominatim (HTTPS) + Stripe API"),
    ("Web Application → Google Maps API: \"HTTPS API calls\"", "Web Application → OSM/OSRM/Nominatim/Stripe: \"HTTPS API calls\""),
    ("Servidor Web → Google Maps API:", "Servidor Web → OpenStreetMap/OSRM/Nominatim/Stripe:"),
    ("Street Maps API: usado para:", "OpenStreetMap + Leaflet + OSRM + Nominatim: usado para:"),
    ("Google Maps API", "OpenStreetMap + Leaflet (+ OSRM/Nominatim)"),
    ("Cache de mapas estáticos, fallback a OpenStreetMap en v2.0", "Sin costo de API de mapas en v1.0; respetar políticas de uso OSM/Nominatim"),
    ("Azure + Google Maps + otros", "Azure PaaS (Q 210/mes) + Stripe (comisión por transacción)"),
    # --- RES-TEC ---
    ("El sistema DEBE desplegarse en Microsoft Azure", "El sistema puede desplegarse en Microsoft Azure o en servidores Windows propios (IIS + SQL Server)"),
    ("No se puede usar AWS, Google Cloud, DigitalOcean, etc.", "En cloud se recomienda Azure; también válido on-premise para demo/desarrollo"),
    ("La geolocalización DEBE usar Google Maps API", "Los mapas usan OpenStreetMap + Leaflet; rutas con OSRM; direcciones con Nominatim (sin API key Google en v1.0)"),
    ("No se puede usar OpenStreetMap, Mapbox (al menos en v1.0)", "No se usa Google Maps API en v1.0 (decisión de costo cero en mapas)"),
    # --- TSP -> vecino más cercano ---
    ("Optimizar las rutas mediante algoritmos de cálculo automático (TSP - Traveling Salesman Problem)",
     "Optimizar las rutas mediante cálculo automático con heurística del vecino más cercano (Nearest Neighbor)"),
    ("Cálculo automático de rutas optimizadas (algoritmo TSP)", "Cálculo automático de rutas optimizadas (heurística vecino más cercano)"),
    ("Sistema calcula ruta óptima automáticamente (algoritmo TSP)", "Sistema calcula ruta optimizada automáticamente (algoritmo vecino más cercano)"),
    ("Sistema ejecuta algoritmo TSP y genera ruta óptima", "Sistema ejecuta algoritmo vecino más cercano y genera ruta optimizada"),
    ("servicio TSP", "servicio de cálculo de rutas (vecino más cercano)"),
    ("algoritmo tipo TSP (Traveling Salesman Problem)", "heurística vecino más cercano (Nearest Neighbor)"),
    ("cálculo de rutas (TSP)", "cálculo de rutas (vecino más cercano)"),
    ("Bug en algoritmo TSP con muchas paradas (>30)", "Rendimiento con muchas paradas (>30) en heurística vecino más cercano"),
    ("El algoritmo TSP no optimiza rutas adecuadamente para casos complejos (50+ paradas)",
     "La heurística vecino más cercano no garantiza optimalidad global (casos 50+ paradas)"),
    ("Usar servicios externos (Google Directions API)", "Dividir ruta manualmente o usar instancia OSRM dedicada"),
    ("TSP (Traveling Salesman Problem): Algoritmo que calcula la ruta más corta visitando todas las paradas",
     "Vecino más cercano (Nearest Neighbor): Heurística que ordena paradas por proximidad geográfica"),
    # --- Geo flujo ---
    ("Envío de ubicación del bus cada X segundos (SignalR)", "Envío de ubicación vía POST /api/ubicaciones; notificación al padre con SignalR (/notificacionesHub)"),
    ("Ubicación se envía vía SignalR cada 10 segundos", "Ubicación se envía vía POST /api/ubicaciones; SignalR notifica al padre (intervalo demo ~2-10 s)"),
    ("SignalR envía ubicación cada 10 segundos", "Cliente envía GPS a POST /api/ubicaciones; servidor emite SignalR al padre"),
    ("SignalR actualiza mapa dinámicamente", "Tras persistir en BD, SignalR (/notificacionesHub) actualiza mapa Leaflet dinámicamente"),
    # --- Roles y pantallas ---
    ("Padre → Mapa", "Padre → PagosPadresFamilia (panel de pagos)"),
    ("Páginas: MiRuta.cshtml, RegistrarRecogidas.cshtml", "Páginas: MiRuta.cshtml (ruta y registro de recogidas)"),
    ("Página RegistrarRecogidas con ruta y alumnos", "Página Monitor/MiRuta con ruta y alumnos"),
    ("Abre página `RegistrarRecogidas` en tablet", "Abre página Monitor/MiRuta en tablet"),
    ("Controllers/PagosController.cs", "Controllers/PagosPadresFamilia.cs (MVC)"),
    ("PagoDto.cs", "PagoRequest.cs / PagoPadre"),
    # --- Tablas / entidades ---
    ("Relación en tabla AlumnoPadreFamilia", "Relación Padres + Alumnos.IdPadre (FK)"),
    ("tabla AlumnoPadreFamilia", "tabla Padres vinculada a Alumnos.IdPadre"),
    ("AlumnoPadreFamilia", "Padres / Alumnos.IdPadre"),
    ("AsignacionAlumnoParada", "Paradas.IdAlumno (asignación alumno-parada en ruta)"),
    ("Registro en tabla ConfirmacionAsistencia", "Registro en tabla AsistenciaAlumno"),
    ("ConfirmacionAsistencia", "AsistenciaAlumno"),
    ("ConfirmacionesAsistencia", "AsistenciaAlumno"),
    ("Campo Fecha en ConfirmacionAsistencia", "Campo Fecha en AsistenciaAlumno"),
    ("`ConfirmacionAsistencia`", "`AsistenciaAlumno`"),
    ("ParadaRuta", "Paradas (IdRuta, Orden)"),
    ("RutaParada", "Paradas (IdRuta, Orden, IdAlumno)"),
    # --- Pagos ---
    ("Pago se guarda con estado \"Pendiente\"", "Pago se registra en PagosPadres (TipoPago Boleta o Linea); validación admin Pendiente/Aprobado: v2.0"),
    ("Pasarela de pago en línea (Stripe, etc.) — diseñada a futuro.", "Webhooks Stripe y conciliación automática — previsto v2.0 (Stripe en línea ya implementado)."),
    ("Imagen se guarda en servidor o Azure Blob", "Imagen se guarda en wwwroot/BoletasPago/ (servidor)"),
    # --- Economía (presentación pagos) ---
    ("Se proyecta que la inversión podrá recuperarse en un periodo aproximado de 10 meses, debido a la reducción de tiempo administrativo, mejora en la organización y aumento en la satisfacción del cliente.",
     "Se proyecta recuperación en ~16 meses (modelo suscripción Q 68/alumno, 75 alumnos, flujo neto Q 4,890/mes tras Azure Q 210); escenario base sin crecimiento: ~10 meses."),
    ("Payback estimado: 10 meses, con base de 75 alumnos y cobrando Q68.00 por alumno.",
     "Payback estimado: 16 meses (presentación); base 75 alumnos × Q 68.00 = Q 5,100 bruto/mes; neto Q 4,890/mes (menos Azure Q 210). Escenario base: ~10 meses."),
    ("Subtotal\nQ750", "Subtotal\nQ210/mes (Azure PaaS recurrente)"),
    ("Técnico\n10 hrs\nQ75\nQ750", "Soporte preventivo\n6 meses\n—\nQ1,500"),
    ("Mantenimiento\nQ750.00", "Soporte preventivo (6 meses)\nQ1,500.00"),
    # --- Versión ---
    ("Versión: 1.0", "Versión: 4.0"),
    ("Fecha: 2025", "Fecha: 20/06/2026"),
]

ANEXO_PARAGRAPHS = [
    ("ANEXO A — Correcciones v4 respecto a v2 (junio 2026)", True, 16),
    ("Este anexo resume los ajustes aplicados en la versión 4.0 sin reducir el alcance documental de la v2.", False, 11),
    ("", False, 11),
    ("• Mapas: Google Maps → OpenStreetMap + Leaflet + OSRM + Nominatim", False, 11),
    ("• Pagos en línea: Stripe implementado (GTQ); boleta + historial en PagosPadres", False, 11),
    ("• Geolocalización: POST /api/ubicaciones → SQL Server → SignalR /notificacionesHub", False, 11),
    ("• Rutas: heurística vecino más cercano (no TSP óptimo exhaustivo)", False, 11),
    ("• Padre al login: /PagosPadresFamilia; mapa en /Padres/DashboardRutaBusAsignado", False, 11),
    ("• Conciliación admin Pendiente/Aprobado: diseño v2.0; v1.0 registra pago directo", False, 11),
    ("• Economía: inversión Q 48,950; suscripción Q 68/alumno; neto Q 4,890/mes; payback ~16 meses", False, 11),
    ("• Módulos añadidos en alcance: traslados, alertas proximidad (~250 m), config. inicial padre", False, 11),
]

NOTA_V4 = (
    "Nota v4.0: Documento corregido y alineado al código implementado (junio 2026). "
    "Base estructural: versión 2.0. Sin reducción de secciones académicas."
)


def replace_in_text(text: str) -> str:
    if not text:
        return text
    for old, new in REPLACEMENTS:
        if old in text:
            text = text.replace(old, new)
    return text


def iter_all_paragraphs(doc):
    for p in doc.paragraphs:
        yield p
    for table in doc.tables:
        for row in table.rows:
            for cell in row.cells:
                for p in cell.paragraphs:
                    yield p
    for section in doc.sections:
        header = section.header
        footer = section.footer
        for p in header.paragraphs:
            yield p
        for p in footer.paragraphs:
            yield p


def replace_paragraph_text(p, new_text: str):
    if not p.runs:
        p.text = new_text
        return
    p.runs[0].text = new_text
    for r in p.runs[1:]:
        r.text = ""


def apply_replacements(doc):
    count = 0
    for p in iter_all_paragraphs(doc):
        old = p.text
        new = replace_in_text(old)
        if new != old:
            replace_paragraph_text(p, new)
            count += 1
    return count


def remove_code_test_example(doc):
    """Reemplaza bloque de código de test por metodología sin código."""
    methodology = (
        "Metodología de pruebas (sin código en este documento): "
        "Unitarias con xUnit y Moq sobre servicios de rutas y validaciones; "
        "integración vía Postman contra /api/ubicaciones y /api/rutas; "
        "E2E con Playwright (login por rol, pago boleta, mapa en vivo); "
        "manual en demo con usuarios seed. Cobertura objetivo >70% en lógica crítica."
    )
    in_code = False
    removed = 0
    for p in doc.paragraphs:
        t = p.text.strip()
        if "public class MiRutaModelTests" in t or "OnGetAsync_ConAsignacionActiva" in t:
            in_code = True
            replace_paragraph_text(p, methodology)
            removed += 1
            continue
        if in_code:
            if t.startswith("Cobertura objetivo") or t.startswith("Testing End-to-End"):
                in_code = False
            elif t and ("{" in t or "}" in t or "Assert." in t or "var " in t or "await " in t):
                replace_paragraph_text(p, "")
                removed += 1
    return removed


def add_nota_after_title(doc):
    for i, p in enumerate(doc.paragraphs[:30]):
        if "Transportes Genesis" in p.text and "Especificación" in p.text:
            np = doc.paragraphs[i]._element
            # insert after current - use add_paragraph at doc level after index
            break
    # Add at beginning after first few paras
    idx = 0
    for i, p in enumerate(doc.paragraphs[:15]):
        if "Repositorio:" in p.text or "github.com" in p.text.lower():
            idx = i + 1
            break
    if idx == 0:
        idx = 3
    anchor = doc.paragraphs[min(idx, len(doc.paragraphs) - 1)]
    new_p = anchor.insert_paragraph_before(NOTA_V4)
    if new_p.runs:
        new_p.runs[0].italic = True
        new_p.runs[0].font.size = Pt(10)
    else:
        r = new_p.add_run(NOTA_V4)
        r.italic = True
        r.font.size = Pt(10)


def add_anexo(doc):
    doc.add_page_break()
    for text, bold, size in ANEXO_PARAGRAPHS:
        p = doc.add_paragraph()
        run = p.add_run(text)
        run.font.size = Pt(size)
        run.bold = bold


def add_alcance_paragraphs(doc):
    """Inserta ítems nuevos en alcance si encuentra sección Módulo de Pagos."""
    extras = [
        "• Pago en línea con tarjeta mediante Stripe (moneda GTQ, Stripe Elements).",
        "• Módulo de traslados entre buses (SolicitudTraslado).",
        "• Alertas de proximidad del bus (Haversine ~250 m casa, ~80 m colegio).",
        "• Configuración inicial del padre con dirección del alumno en mapa (Leaflet + Nominatim).",
    ]
    for i, p in enumerate(doc.paragraphs):
        if "Módulo de Pagos" in p.text:
            nxt = doc.paragraphs[i + 1].text if i + 1 < len(doc.paragraphs) else ""
            if "Registro de pagos" in nxt or "pagos realizados" in nxt.lower():
                for ex in reversed(extras):
                    doc.paragraphs[i + 1].insert_paragraph_before(ex)
                return len(extras)
    return 0


def main():
    doc = Document(str(DOCX))
    paras_before = len(doc.paragraphs)
    tables_before = len(doc.tables)

    n = apply_replacements(doc)
    remove_code_test_example(doc)
    add_nota_after_title(doc)
    add_alcance_paragraphs(doc)
    add_anexo(doc)

    doc.save(str(DOCX))
    doc2 = Document(str(DOCX))
    words = sum(len(p.text.split()) for p in doc2.paragraphs)

    print(f"Reemplazos en párrafos/celdas: {n}")
    print(f"Párrafos: {paras_before} -> {len(doc2.paragraphs)}")
    print(f"Tablas: {tables_before} -> {len(doc2.tables)}")
    print(f"Palabras aprox: {words}")
    print(f"Guardado: {DOCX}")


if __name__ == "__main__":
    main()
