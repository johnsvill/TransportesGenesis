# Resumen de entrega — Tesis Transportes Génesis (75 %)

**Fecha de entrega:** martes 4 de agosto de 2026  
**Documento principal:** `TESIS_TRANSPORTES_GENESIS_v4.docx`  
**Carpeta:** `Documentos/Tesis/verison_quiinta_4_agisto`

---

## 1. Qué se entrega (75 %)

| Entregable | Estado |
|---|---|
| Tesis Word (v4) con formato institucional | Listo |
| Fuente Markdown regenerable | Listo |
| Capítulos 1 a 6 redactados | Listo (resultados parciales) |
| Diagramas de arquitectura y flujos (Figuras 4.2–4.6) | Listo |
| Evidencia audiovisual (fotos y videos) | Recibida e integrada |
| Apéndices (UAT, consentimiento, evidencias, bitácora, diagramas) | Listos como plantillas / inventario |
| Enlace institucional de evidencias | Incluido |

**Enlace de evidencias (grupo universidad):**  
https://drive.google.com/drive/folders/1d1_k0cRqK9md96MxalGPg3LT3fl4RPZg?usp=sharing

---

## 2. Qué decir en la exposición (guion corto)

### A. Contexto y problema (1–2 min)
- Transportes Génesis opera transporte escolar en San Cristóbal, San Marcos.
- Antes: información dispersa (llamadas, WhatsApp, registros separados).
- Problema: poca trazabilidad de rutas, asistencia, abordaje y pagos.
- Pregunta: ¿la integración de geolocalización y asistencia/abordaje mejora eficiencia y percepción de seguridad?

### B. Objetivo e hipótesis (1 min)
- **Objetivo general:** validar si geolocalización + asistencia/abordaje mejoran eficiencia y seguridad percibida de los padres.
- **Objetivos específicos:**
  1. Geolocalización en tiempo real de buses y rutas.
  2. Módulo de asistencia y abordaje.
- **Hipótesis:** esa integración mejora eficiencia operativa y percepción de seguridad.
- **Estado actual:** hipótesis **parcialmente respaldada** (funcional/cualitativa), **no confirmada** de forma definitiva.

### C. Solución tecnológica (2 min)
- Plataforma web .NET 8 con roles: Administrador, Piloto, Monitor, Padre.
- Stack: SQL Server, EF Core, Identity, SignalR, Leaflet, OpenStreetMap, OSRM.
- Módulos: mapas, paradas, asistencia, recogidas, pagos (este último complementario).
- Mostrar diagramas:
  - Figura 4.2 — Arquitectura general
  - Figura 4.4 — Geolocalización / SignalR
  - Figura 4.5 — Asistencia y abordaje
  - Figura 4.6 — Pagos (diseño; no UAT cerrado)

### D. Metodología (1–2 min)
- Investigación aplicada, sin encuestas (justificado por guía FISICC: 2–3 métodos).
- Métodos elegidos: UAT + observación, automatización (Selenium) y pruebas de carga.
- Evidencia actual: fotos y videos de demostraciones controladas.
- No se inventan métricas ni se afirma “aprobado UAT” sin acta.

### E. Resultados a la fecha (2–3 min)
**Sí se demuestra:**
- Gestión de paradas (admin).
- Mapas y movimiento del marcador del bus.
- Calendario de asistencia por fecha y turno.
- Solicitud de traslado temporal.
- Retroalimentación cualitativa (paradas, GPS, calendario, seguridad percibida).

**Aún no se demuestra del todo:**
- Registro audiovisual de abordaje por el monitor.
- Flujo completo de pagos con video.
- Sincronización SignalR grabada en tres roles a la vez.
- Tiempos antes/después (eficiencia).
- Escala o instrumento de percepción de seguridad.
- Matriz UAT firmada + bitácora + acta de aceptación.

### F. Conclusión del avance (1 min)
- Avance estimado: **~75 %**.
- Lo construido y documentado permite entregar un borrador integral.
- Falta cerrar validación formal para confirmar la hipótesis al 100 %.

### G. Trabajo pendiente (30–45 s)
1. Completar UAT con casos, tiempos y acta.
2. Video de abordaje (monitor) y, si aplica, flujo de pagos.
3. Medir eficiencia y percepción de seguridad con criterio claro.
4. Actualizar índices Word (`Ctrl + A`, `F9`) y datos de portada/asesor.

---

## 3. Mensajes clave (para no contradecirse)

1. **No digan** “hipótesis confirmada”.  
   Digan: “parcialmente respaldada con evidencia funcional y cualitativa”.
2. **No digan** “UAT finalizada”.  
   Digan: “evidencia de demostración recibida; cierre formal pendiente”.
3. **No digan** “se redujeron tiempos / mayor satisfacción en X %”.  
   Digan: “aún no hay medición cuantitativa; hay indicios cualitativos”.
4. **Sí pueden decir** que geolocalización y asistencia están implementadas y demostradas en ambiente controlado.
5. **Pagos:** existen en el sistema y en diagrama; la validación audiovisual completa sigue pendiente.

---

## 4. Checklist visual de lo que llevan

- [ ] `TESIS_TRANSPORTES_GENESIS_v4.docx`
- [ ] `TESIS_TRANSPORTES_GENESIS_v4.md` (opcional, respaldo)
- [ ] Carpeta `diagramas_integrados/`
- [ ] Acceso al Drive de evidencias
- [ ] Este resumen (`RESUMEN_ENTREGA_75.md`) para la oral

---

## 5. Frase de cierre sugerida

> “A este corte entregamos la tesis al 75 %: problema, objetivos, diseño, implementación y resultados preliminares con evidencia audiovisual. La hipótesis se respalda parcialmente; el 25 % restante corresponde al cierre formal de UAT, métricas de eficiencia y percepción de seguridad, y evidencia específica de abordaje.”
