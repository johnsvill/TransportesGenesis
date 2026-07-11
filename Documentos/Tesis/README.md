# Documentos de Tesis — Transportes Génesis

Estructura formal de documentos para la tesis y anexos del proyecto **Transportes Génesis**.

## Ubicación de archivos

| Documento | Markdown (.md) | Word (.docx) |
|-----------|----------------|--------------|
| **Tesis — Cap. 1 y 2** | `01_Introduccion_y_Marco_Teorico.md` | `01_Introduccion_y_Marco_Teorico.docx` |
| **Plan de pruebas (anexo)** | `pruebas_tesis/PLAN_PRUEBAS_UAT_CARGA_RENDIMIENTO.md` | `pruebas_tesis/PLAN_PRUEBAS_UAT_CARGA_RENDIMIENTO.docx` |

## Documentos relacionados

| Archivo | Uso |
|---------|-----|
| `Docs/CONFIGURACION_PRUEBAS_PADRE_PILOTO_MONITOR.md` | Usuarios, buses y demo en vivo para pruebas |
| `generar_documentos_formales.py` | Regenera los `.docx` desde los `.md` |

## Estructura académica de la tesis (Capítulos 1 y 2)

**Capítulo 1 — Introducción:** 1.1 Tema principal · 1.2 Contexto · 1.3 Hipótesis · 1.4 Objetivo general · 1.4.1 Objetivos específicos · 1.4.2 Instrumento de evaluación · 1.5 Planteamiento del problema · 1.6 Resumen · 1.7 Justificación · 1.8 Delimitación · 1.9 Alcance y limitaciones

**Capítulo 2 — Marco teórico:** 2.1 Trasfondo (+ antecedentes) · 2.2 Geolocalización · 2.3 Pagos digitales · 2.4 Integración · 2.5 Fundamentos de pruebas · 2.6 Definiciones

**Anexo:** Plan de pruebas UAT, Selenium, carga y rendimiento — vinculado a objetivos 5–6 e hipótesis H1–H3.

## Formato formal aplicado

| Elemento | Valor |
|----------|--------|
| Fuente | Times New Roman, 12 pt |
| Interlineado | 1.5 |
| Alineación cuerpo | Justificado |
| Margen superior / inferior | 2.5 cm |
| Margen izquierdo | 3.0 cm (encuadernación) |
| Margen derecho | 2.5 cm |
| Sangría primera línea | 0.63 cm |
| Portada | Generada en Word desde metadatos YAML del `.md` |
| Numeración | Pie de página centrado |

Los metadatos (universidad, autor, asesor, lugar) se editan en el bloque `---` al inicio de cada `.md`. La portada del Word se genera desde ahí; **no** duplicar portada en el cuerpo del Markdown.

## Regenerar archivos Word

Desde la raíz del repositorio:

```powershell
cd C:\Proyectos\TransportesGenesis
python Documentos\Tesis\generar_documentos_formales.py
```

Requisito: `pip install python-docx`

## Antes de entregar — checklist

- [ ] Completar `[Nombre de la universidad]`, carrera, autor y asesor en YAML
- [ ] Confirmar nombre geográfico correcto (San Cristóbal, San Marcos)
- [ ] Completar referencias bibliográficas en formato APA (o el exigido)
- [ ] Revisar Word: márgenes, justificación, portada, numeración
- [ ] Alinear fechas del calendario de pruebas con el colegio
