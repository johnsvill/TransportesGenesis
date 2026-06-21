# Alineación: Word v3 ↔ Presentación Gestión de Pagos

**Word v4 (actual):** `Proyecto_Transportes_Genesis_v4.docx`  
**Presentación v3:** `Transportes_Genesis_Presentacion_v3_20062026.pptx`  
**Word v3:** eliminado (era resumen corto; reemplazado por v4 basada en v2)

---

## Cifras económicas — AHORA COINCIDEN

| Concepto | Presentación (slide 13–14) | Word v3 |
|----------|---------------------------|---------|
| Inversión inicial total | Q 48,950.00 | Q 48,950.00 |
| Ingeniería y QA (275 hrs) | Q 46,000.00 | Q 46,000.00 |
| Implantación y capacitación | Q 1,450.00 | Q 1,450.00 |
| Soporte preventivo 6 meses | Q 1,500.00 | Q 1,500.00 |
| Tarifa hora | USD 21.45 | USD 21.45 |
| Suscripción / alumno / mes | Q 68.00 | Q 68.00 |
| Ingreso bruto (75 alumnos) | Q 5,100.00 | Q 5,100.00 |
| Azure PaaS mensual | Q 210.00 | Q 210.00 |
| Flujo neto mensual | Q 4,890.00 | Q 4,890.00 |
| Payback presentación | 16 meses | 16 meses (+ nota ~10 meses base) |

---

## Temas técnicos — Word v3 más preciso que la PPT

| Tema | Presentación dice | Word v3 (código real) |
|------|-------------------|------------------------|
| Mapas | No detalla (diagramas genéricos) | OpenStreetMap + Leaflet + OSRM |
| Geo | SignalR cada 10 s | API REST + BD + SignalR |
| Pagos en línea | No menciona Stripe | Stripe implementado |
| Conciliación Pendiente/Aprobado | Slide 9 — sí | v1.0 **parcial** — tabla honesta en §3.4 |
| Hub SignalR | SignalR genérico | `/notificacionesHub` |
| Alertas cercanía | Haversine 250 m | Igual — implementado |

---

## Recomendación para la defensa oral

Usar **las cifras de la presentación** (48,950 / 4,890 / 16 meses) y el **Word v3** para detalle técnico.  
Si preguntan por conciliación admin: *«está en el roadmap v2.0; hoy el padre registra boleta o paga con Stripe y ve historial»*.

---

*Generado automáticamente al exportar Word — 20/06/2026*
