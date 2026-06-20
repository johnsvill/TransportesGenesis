"""
Genera presentación PowerPoint v3 alineada al Word y al código implementado.
Salida: Transportes_Genesis_Presentacion_v3_20062026.pptx
"""
from pathlib import Path
from pptx import Presentation
from pptx.util import Inches, Pt
from pptx.dml.color import RGBColor
from pptx.enum.text import PP_ALIGN, MSO_ANCHOR
from pptx.enum.chart import XL_CHART_TYPE, XL_LEGEND_POSITION
from pptx.chart.data import CategoryChartData

OUT_DIR = Path(__file__).parent
OUT_FILE = OUT_DIR / "Transportes_Genesis_Presentacion_v3_20062026.pptx"

# Colores corporativos
BLUE = RGBColor(0x1A, 0x56, 0xDB)
DARK = RGBColor(0x1E, 0x29, 0x3B)
GRAY = RGBColor(0x64, 0x74, 0x8B)
WHITE = RGBColor(0xFF, 0xFF, 0xFF)
GREEN = RGBColor(0x05, 0x96, 0x69)
ORANGE = RGBColor(0xEA, 0x58, 0x0C)


def set_slide_bg(slide, color=RGBColor(0xF8, 0xFA, 0xFC)):
    fill = slide.background.fill
    fill.solid()
    fill.fore_color.rgb = color


def add_title_bar(slide, title: str, subtitle: str = None):
    bar = slide.shapes.add_shape(
        1, Inches(0), Inches(0), Inches(10), Inches(1.1)
    )
    bar.fill.solid()
    bar.fill.fore_color.rgb = BLUE
    bar.line.fill.background()
    tf = bar.text_frame
    tf.word_wrap = True
    p = tf.paragraphs[0]
    p.text = title
    p.font.size = Pt(28)
    p.font.bold = True
    p.font.color.rgb = WHITE
    if subtitle:
        box = slide.shapes.add_textbox(Inches(0.5), Inches(1.25), Inches(9), Inches(0.5))
        p2 = box.text_frame.paragraphs[0]
        p2.text = subtitle
        p2.font.size = Pt(14)
        p2.font.color.rgb = GRAY


def add_bullets(slide, items, left=0.6, top=1.6, width=8.8, height=5.2, font_size=18):
    box = slide.shapes.add_textbox(Inches(left), Inches(top), Inches(width), Inches(height))
    tf = box.text_frame
    tf.word_wrap = True
    for i, item in enumerate(items):
        p = tf.paragraphs[0] if i == 0 else tf.add_paragraph()
        p.text = item
        p.font.size = Pt(font_size)
        p.font.color.rgb = DARK
        p.space_after = Pt(8)
        p.level = 0


def add_two_columns(slide, left_title, left_items, right_title, right_items):
    # Left
    lt = slide.shapes.add_textbox(Inches(0.5), Inches(1.5), Inches(4.3), Inches(0.4))
    lt.text_frame.paragraphs[0].text = left_title
    lt.text_frame.paragraphs[0].font.bold = True
    lt.text_frame.paragraphs[0].font.size = Pt(20)
    lt.text_frame.paragraphs[0].font.color.rgb = BLUE
    add_bullets(slide, left_items, left=0.5, top=2.0, width=4.3, height=4.5, font_size=16)
    # Right
    rt = slide.shapes.add_textbox(Inches(5.2), Inches(1.5), Inches(4.3), Inches(0.4))
    rt.text_frame.paragraphs[0].text = right_title
    rt.text_frame.paragraphs[0].font.bold = True
    rt.text_frame.paragraphs[0].font.size = Pt(20)
    rt.text_frame.paragraphs[0].font.color.rgb = GREEN
    add_bullets(slide, right_items, left=5.2, top=2.0, width=4.3, height=4.5, font_size=16)


def slide_title(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide, BLUE)
    t = slide.shapes.add_textbox(Inches(0.8), Inches(1.8), Inches(8.4), Inches(1.5))
    p = t.text_frame.paragraphs[0]
    p.text = "Transportes Génesis"
    p.font.size = Pt(44)
    p.font.bold = True
    p.font.color.rgb = WHITE
    p.alignment = PP_ALIGN.CENTER
    s = slide.shapes.add_textbox(Inches(0.8), Inches(3.2), Inches(8.4), Inches(1.2))
    sp = s.text_frame.paragraphs[0]
    sp.text = (
        "Plataforma web de transporte escolar\n"
        "Pagos · Geolocalización · Rutas · Asistencia"
    )
    sp.font.size = Pt(22)
    sp.font.color.rgb = RGBColor(0xDB, 0xEA, 0xFE)
    sp.alignment = PP_ALIGN.CENTER
    f = slide.shapes.add_textbox(Inches(0.8), Inches(5.0), Inches(8.4), Inches(1.0))
    fp = f.text_frame.paragraphs[0]
    fp.text = "Universidad Galileo · v3.0 · 20/06/2026\nAlineado al código implementado"
    fp.font.size = Pt(14)
    fp.font.color.rgb = WHITE
    fp.alignment = PP_ALIGN.CENTER


def slide_problem(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Problemática y Objetivos")
    add_two_columns(
        slide,
        "Desafíos actuales",
        [
            "Rutas planificadas «a ojo» sin optimización",
            "Padres sin visibilidad del bus en ruta",
            "Cobros desorganizados vía WhatsApp",
            "Recogidas y asistencias en papel",
        ],
        "Solución Génesis v3",
        [
            "Plataforma web centralizada (.NET 8)",
            "Mapa en vivo: OpenStreetMap + Leaflet",
            "Pagos: boleta + Stripe en línea",
            "GPS → API → SignalR → padre",
            "Rutas: algoritmo vecino más cercano",
        ],
    )


def slide_scope(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Alcance v1.0 — Implementado")
    items = [
        "🔐 Roles: Admin, Piloto, Monitor, Padre (ASP.NET Identity)",
        "🗺️ Mapas: OpenStreetMap + Leaflet + OSRM + Nominatim (sin Google Maps)",
        "📍 Geo en vivo: POST /api/ubicaciones → BD → SignalR /notificacionesHub",
        "💳 Pagos: subir boleta + Stripe (GTQ) + historial del padre",
        "✅ Asistencia diaria y recogidas desde MiRuta (monitor)",
        "🔄 Traslados entre buses y alertas de proximidad (~250 m)",
        "📊 Reportes admin: rutas, asistencias, asignaciones (Excel)",
    ]
    add_bullets(slide, items, font_size=17)
    note = slide.shapes.add_textbox(Inches(0.6), Inches(6.0), Inches(8.8), Inches(0.6))
    note.text_frame.paragraphs[0].text = (
        "Pendiente v2.0: validación admin Pendiente/Aprobado · webhooks Stripe · app nativa"
    )
    note.text_frame.paragraphs[0].font.size = Pt(12)
    note.text_frame.paragraphs[0].font.italic = True
    note.text_frame.paragraphs[0].font.color.rgb = ORANGE


def slide_architecture(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Arquitectura del Sistema")
    items = [
        "Cliente web: Bootstrap 5 + Leaflet + SignalR JS",
        "Servidor: ASP.NET Core 8 — Razor Pages + MVC (pagos) + API REST",
        "Tiempo real: NotificacionesHub (/notificacionesHub)",
        "Base de datos: SQL Server — esquemas genesis + dbo (Identity)",
        "Servicios externos: OpenStreetMap · OSRM · Nominatim · Stripe API",
        "Despliegue: Azure PaaS (Q 210/mes) u on-premise (IIS + SQL local)",
    ]
    add_bullets(slide, items, font_size=17)
    # Simple flow boxes as text
    flow = slide.shapes.add_textbox(Inches(0.6), Inches(5.2), Inches(8.8), Inches(1.2))
    fp = flow.text_frame.paragraphs[0]
    fp.text = (
        "Flujo geo:  Piloto/Monitor (GPS navegador)  →  API REST  →  SQL Server  →  SignalR  →  Mapa padre"
    )
    fp.font.size = Pt(14)
    fp.font.bold = True
    fp.font.color.rgb = BLUE
    fp.alignment = PP_ALIGN.CENTER


def slide_payments(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Módulo de Pagos", "Controllers/PagosPadresFamilia · Tabla PagosPadres")
    add_two_columns(
        slide,
        "Implementado v1.0",
        [
            "Panel padre: /PagosPadresFamilia",
            "Subir boleta (mes, monto, imagen)",
            "Pago en línea con Stripe (GTQ)",
            "Historial personal del padre",
            "Alerta de meses pendientes (Ene–Oct)",
        ],
        "Roadmap v2.0",
        [
            "Panel admin: pagos pendientes",
            "Estados Pendiente / Aprobado / Rechazado",
            "Webhooks Stripe",
            "Reporte de ingresos por fechas",
            "Monto automático por tarifa de recorrido",
        ],
    )


def slide_geo(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Geolocalización en Tiempo Real")
    items = [
        "1. Piloto o monitor obtiene GPS del navegador (Geolocation API)",
        "2. Cliente envía coordenadas a POST /api/ubicaciones",
        "3. Servidor guarda en UbicacionBusEnTiempoReal (SQL Server)",
        "4. Servidor emite evento UbicacionBusActualizada vía SignalR",
        "5. Padre recibe actualización y mueve el bus en mapa Leaflet",
        "6. Alertas automáticas: ~250 m casa del alumno · ~80 m colegio (Haversine)",
        "",
        "❌ No usamos Google Maps  ·  ✅ OpenStreetMap + OSRM para rutas en calles",
    ]
    add_bullets(slide, items, font_size=17)


def slide_signalr(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Monitoreo SignalR")
    add_two_columns(
        slide,
        "Características",
        [
            "Hub: /notificacionesHub",
            "Grupos: Bus_{id} · Alumno_{id}",
            "Eventos: UbicacionBusActualizada, AlertaRecibida",
            "Reconexión automática ante pérdida de red",
            "Intervalo demo: ~2 s (objetivo ≤10 s)",
        ],
        "Pantallas",
        [
            "Padre: DashboardRutaBusAsignado",
            "Piloto: Piloto/MiRuta",
            "Monitor: Monitor/MiRuta",
            "Admin demo: Geolocalizacion/MapaEnTiempoReal",
        ],
    )


def slide_routes(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Cálculo de Rutas")
    items = [
        "Turnos Mañana (termina en colegio) y Tarde (empieza en colegio)",
        "Algoritmo: vecino más cercano (heurística eficiente)",
        "Considera asistencia confirmada del padre",
        "Horarios estimados según distancia (~30 km/h)",
        "Visualización en mapa con geometría real vía OSRM",
        "",
        "Nota: no es TSP óptimo exhaustivo — suficiente para el volumen de paradas del proyecto",
    ]
    add_bullets(slide, items, font_size=17)


def slide_roles(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Usuarios y Redirección")
    items = [
        "Administrador → /Admin (buses, rutas, alumnos, reportes, traslados)",
        "Piloto → /Piloto/MiRuta (ruta + transmisión GPS)",
        "Monitor → /Monitor/MiRuta (paradas + recogidas)",
        "Padre → /PagosPadresFamilia (panel principal al login)",
        "Padre también accede: mapa del bus, asistencia, traslados",
    ]
    add_bullets(slide, items, font_size=18)


def slide_economic_summary(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Especificación Económica", "Tarifa de consultoría integrada")
    items = [
        "Tarifa: USD 21.45 / hora (base USD 15.00 + utilidad 30 %)",
        "Esfuerzo: 275 horas de ingeniería",
        "Inversión inicial total: Q 48,950.00",
        "  · Ingeniería y QA: Q 46,000.00",
        "  · Implantación y capacitación: Q 1,450.00",
        "  · Soporte preventivo (6 meses): Q 1,500.00",
        "Infraestructura cloud Azure PaaS: Q 210.00 / mes (recurrente)",
    ]
    add_bullets(slide, items, font_size=18)


def slide_investment_pie(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Composición de la Inversión Inicial", "Q 48,950.00")
    chart_data = CategoryChartData()
    chart_data.categories = ["Ingeniería QA", "Capacitación", "Soporte 6m"]
    chart_data.add_series("Q", (46000, 1450, 1500))
    x, y, cx, cy = Inches(1.2), Inches(1.8), Inches(7.5), Inches(4.8)
    chart = slide.shapes.add_chart(XL_CHART_TYPE.PIE, x, y, cx, cy, chart_data).chart
    chart.has_legend = True
    chart.legend.position = XL_LEGEND_POSITION.BOTTOM
    chart.has_title = False


def slide_subscription(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Plan de Suscripción y ROI")
    add_two_columns(
        slide,
        "Modelo SaaS",
        [
            "Q 68.00 por alumno / mes",
            "75 alumnos → Q 5,100 bruto/mes",
            "Menos Azure Q 210 → Q 4,890 neto/mes",
            "Cubre infraestructura y actualizaciones",
            "Crecimiento: ~2 colegios adicionales/año",
        ],
        "Recuperación",
        [
            "Inversión: Q 48,950",
            "Escenario base: ~10 meses (48,950 ÷ 4,890)",
            "Escenario presentación: 16 meses",
            "  (con expansión gradual)",
            "ROI: operativo + suscripción",
        ],
    )


def slide_payback_chart(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Recuperación de la Inversión", "Flujo neto Q 4,890 / mes · Inversión Q 48,950")
    months = ["M0", "M2", "M4", "M6", "M8", "M10", "M12", "M14", "M16"]
    accumulated = [0, 9780, 19560, 29340, 39120, 48900, 58680, 68460, 78240]
    chart_data = CategoryChartData()
    chart_data.categories = months
    chart_data.add_series("Flujo neto acumulado (Q)", accumulated)
    chart_data.add_series("Inversión referencia (Q)", [48950] * len(months))
    x, y, cx, cy = Inches(0.8), Inches(1.7), Inches(8.4), Inches(4.5)
    chart = slide.shapes.add_chart(
        XL_CHART_TYPE.COLUMN_CLUSTERED, x, y, cx, cy, chart_data
    ).chart
    chart.has_legend = True
    chart.legend.position = XL_LEGEND_POSITION.BOTTOM
    note = slide.shapes.add_textbox(Inches(0.8), Inches(6.3), Inches(8.4), Inches(0.5))
    note.text_frame.paragraphs[0].text = (
        "Equilibrio escenario base: ~mes 10  |  Recuperación total con crecimiento: ~mes 16"
    )
    note.text_frame.paragraphs[0].font.size = Pt(13)
    note.text_frame.paragraphs[0].font.bold = True
    note.text_frame.paragraphs[0].font.color.rgb = GREEN
    note.text_frame.paragraphs[0].alignment = PP_ALIGN.CENTER


def slide_azure(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Entorno Cloud Azure PaaS")
    items = [
        "Despliegue sobre Microsoft Azure (App Service + SQL + Blob Storage)",
        "Costo estimado: Q 210.00 / mes",
        "Escalabilidad sin mantenimiento de hardware local",
        "Alternativa: servidores Windows propios (IIS + SQL Server) para demo/desarrollo",
        "Mapas: OpenStreetMap — sin costo de API Google en v1.0",
    ]
    add_bullets(slide, items, font_size=18)


def slide_corrections(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Correcciones v2 → v3", "Lo que cambió respecto al documento Word original")
    items = [
        "Google Maps → OpenStreetMap + Leaflet + OSRM + Nominatim",
        "Stripe «futuro» → Stripe implementado (pagos en línea GTQ)",
        "GPS solo SignalR → API REST + BD + SignalR",
        "Hub /geolocalizacionHub → /notificacionesHub",
        "TSP óptimo → Vecino más cercano (heurística)",
        "Padre entra al mapa → Padre entra a panel de pagos",
        "Costo Google Maps eliminado del presupuesto",
    ]
    add_bullets(slide, items, font_size=17)


def slide_risks(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Análisis de Riesgos")
    add_two_columns(
        slide,
        "Riesgo · Mitigación",
        [
            "Pérdida señal 4G → Reconexión SignalR + reintento API",
            "Resistencia al sistema → UX simple + capacitación",
            "Boletas falsas → Verificación manual admin (v2.0)",
            "Límites OSRM/Nominatim → Instancia propia en prod.",
            "Pago Stripe antes de confirmar → Webhooks v2.0",
        ],
        "Técnicos",
        [
            "Caída SignalR → Fallback SSE / long polling",
            "Muchas paradas → Límite ~30 + heurística NN",
            "Seguridad XSS/CSRF → Identity + anti-CSRF + EF",
        ],
    )


def slide_demo(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide)
    add_title_bar(slide, "Demo y Credenciales")
    items = [
        "Padre: padre1@gmail.com → /PagosPadresFamilia → mapa en /Padres/DashboardRutaBusAsignado",
        "Monitor: monitor1@transportesgenesis.com → /Monitor/MiRuta",
        "Contraseña demo: Admin123!",
        "",
        "Mensajes clave para la defensa:",
        "· OpenStreetMap + Leaflet (no Google Maps)",
        "· Stripe ya funciona para pagos en línea",
        "· Validación admin de pagos = roadmap v2.0",
    ]
    add_bullets(slide, items, font_size=17)


def slide_closing(prs):
    slide = prs.slides.add_slide(prs.slide_layouts[6])
    set_slide_bg(slide, BLUE)
    t = slide.shapes.add_textbox(Inches(1), Inches(2.5), Inches(8), Inches(2))
    p = t.text_frame.paragraphs[0]
    p.text = "Transportes Génesis v3.0\n¿Preguntas?"
    p.font.size = Pt(40)
    p.font.bold = True
    p.font.color.rgb = WHITE
    p.alignment = PP_ALIGN.CENTER
    s = slide.shapes.add_textbox(Inches(1), Inches(4.5), Inches(8), Inches(1))
    sp = s.text_frame.paragraphs[0]
    sp.text = "github.com/johnsvill/TransportesGenesis · Documento: Proyecto_Transportes_Genesis_v3_ACTUALIZADO"
    sp.font.size = Pt(14)
    sp.font.color.rgb = RGBColor(0xDB, 0xEA, 0xFE)
    sp.alignment = PP_ALIGN.CENTER


def build():
    prs = Presentation()
    prs.slide_width = Inches(10)
    prs.slide_height = Inches(7.5)

    slide_title(prs)
    slide_problem(prs)
    slide_scope(prs)
    slide_architecture(prs)
    slide_payments(prs)
    slide_geo(prs)
    slide_signalr(prs)
    slide_routes(prs)
    slide_roles(prs)
    slide_economic_summary(prs)
    slide_investment_pie(prs)
    slide_subscription(prs)
    slide_payback_chart(prs)
    slide_azure(prs)
    slide_corrections(prs)
    slide_risks(prs)
    slide_demo(prs)
    slide_closing(prs)

    prs.save(str(OUT_FILE))
    print(f"Presentación creada: {OUT_FILE}")
    print(f"Total slides: {len(prs.slides)}")


if __name__ == "__main__":
    build()
