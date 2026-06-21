"""Segunda pasada: tablas económicas v4."""
from docx import Document
from pathlib import Path

DOCX = Path(__file__).parent / "Proyecto_Transportes_Genesis_v4.docx"

def set_cell(row, col, text):
    row.cells[col].text = text


def fix_economic_tables(doc):
    changes = 0
    for ti, table in enumerate(doc.tables):
        for row in table.rows:
            cells = [c.text.strip() for c in row.cells]
            joined = " | ".join(cells)
            # Total general breakdown
            if len(cells) >= 2 and cells[0] == "Infraestructura" and "750" in cells[1]:
                row.cells[1].text = "Q210.00/mes (Azure PaaS, recurrente — no en capex)"
                changes += 1
            if len(cells) >= 2 and cells[0] == "Mantenimiento" and "750" in cells[1]:
                row.cells[0].text = "Soporte preventivo (6 meses)"
                row.cells[1].text = "Q1,500.00"
                changes += 1
            if len(cells) >= 2 and cells[0] == "Técnico" and any("750" in c for c in cells):
                set_cell(row, 0, "Soporte preventivo")
                if len(row.cells) > 1:
                    row.cells[1].text = "6 meses"
                if len(row.cells) > 3:
                    row.cells[3].text = "Q1,500"
                changes += 1
            # Subtotal rows with lone Q750 in infra section
            if "Subtotal" in cells[0] and len(cells) >= 4:
                if cells[-1] == "Q750" and ti in (43, 45):
                    row.cells[-1].text = "Q210/mes (recurrente)" if ti == 43 else "Q1,500"
                    changes += 1
    return changes


def add_saas_paragraph(doc):
    """Añade párrafo modelo SaaS en §3.4 si no existe."""
    for p in doc.paragraphs:
        if "Q 68.00 por alumno" in p.text or "Q68.00 por alumno" in p.text:
            return 0
    for i, p in enumerate(doc.paragraphs):
        if p.text.strip().startswith("3.4") and "Modelo" in p.text:
            text = (
                "Modelo de suscripción: Q 68.00 por alumno/mes (75 alumnos = Q 5,100 bruto/mes). "
                "Costo cloud Azure Q 210/mes → flujo neto Q 4,890/mes. "
                "Recuperación estimada: 16 meses (crecimiento ~2 colegios/año); escenario base ~10 meses."
            )
            doc.paragraphs[i + 1].insert_paragraph_before(text)
            return 1
    return 0


def main():
    doc = Document(str(DOCX))
    c1 = fix_economic_tables(doc)
    c2 = add_saas_paragraph(doc)
    doc.save(str(DOCX))
    print(f"Tablas corregidas: {c1}, párrafos SaaS: {c2}")


if __name__ == "__main__":
    main()
