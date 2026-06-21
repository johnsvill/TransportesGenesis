# Registro de cambios — Proyecto_Transportes_Genesis_v4.docx

**Fecha:** 20/06/2026  
**Base:** `Proyecto_Transportes_Genesis_v2 (1).docx` (copia íntegra)  
**Método:** correcciones quirúrgicas (no reescritura)  
**Scripts:** `aplicar_correcciones_v4.py`, `fix_v4_economia.py`

---

## Archivos

| Acción | Archivo |
|--------|---------|
| Creado | `Proyecto_Transportes_Genesis_v4.docx` |
| Eliminado | `Proyecto_Transportes_Genesis_v3_ACTUALIZADO.docx` |
| Eliminado | `Proyecto_Transportes_Genesis_v3_ACTUALIZADO.md` |
| Conservado | PPT v3, scripts, ALINEACION |

---

## Métricas (validación)

| Métrica | v2 | v4 |
|---------|----|----|
| Párrafos | ~735 | ~752+ |
| Tablas | 49 | 49 |
| Palabras | ~5.822 | ~6.087+ |

---

## Correcciones aplicadas (44+ reemplazos)

### Mapas y geo
- Google Maps → OpenStreetMap + Leaflet + OSRM + Nominatim
- `/geolocalizacionHub` → `/notificacionesHub`
- Flujo GPS: POST `/api/ubicaciones` → BD → SignalR

### Pagos
- Stripe marcado como **implementado v1.0**
- `PagosController` → `PagosPadresFamiliaController`
- Storage boletas: `wwwroot/BoletasPago`
- Estados Pendiente: nota v2.0

### Rutas
- TSP → heurística vecino más cercano (salvo mención en anexo)

### Modelo de datos
- `AlumnoPadreFamilia` → `Padres` / `Alumnos.IdPadre`
- `ConfirmacionAsistencia` → `AsistenciaAlumno`
- `RegistrarRecogidas` → `Monitor/MiRuta`
- `ParadaRuta` / `RutaParada` → `Paradas.IdRuta`

### Roles
- Padre → `PagosPadresFamilia` (no mapa como home)

### Restricciones
- RES-TEC-03: Azure **u** on-premise
- RES-TEC-04: sin Google Maps obligatorio

### Economía (presentación pagos)
- Payback: 16 meses + nota ~10 meses base
- Flujo neto Q 4,890/mes (Azure Q 210)
- Soporte preventivo: Q 1,500 (6 meses)
- Infraestructura: Azure Q 210/mes recurrente

### Contenido añadido
- Nota v4.0 al inicio
- 4 ítems en alcance (Stripe, traslados, alertas, config. padre)
- Anexo A con resumen de correcciones
- Metodología de testing sin bloque código C#

---

## Pendiente revisión manual en Word (opcional)

- [ ] Regenerar índice / TOC (clic derecho → Actualizar campos)
- [ ] Revisar diagramas C4 embebidos como imágenes (si existen imágenes, editar manualmente)
- [ ] Insertar gráfica payback desde PPT slide 15 si se desea imagen en §3.5
- [ ] Revisar fila Total General sigue en Q 48,950.00

---

*Generado al ejecutar plan v4.*
