# Tesis inicial — Transportes Génesis

Esta carpeta contiene el borrador completo de la tesis en dos formatos:

- `TESIS_TRANSPORTES_GENESIS.md`: fuente editable.
- `TESIS_TRANSPORTES_GENESIS.docx`: documento Word con formato institucional.
- `generar_tesis.py`: generador de Word.

## Campos pendientes

Antes de entregar, reemplace los campos entre corchetes de la carátula: facultad o escuela, carrera, autor, carné, asesor y grado académico.

El Capítulo 5 y las conclusiones contienen la palabra **Pendiente** porque todavía deben incorporarse resultados reales de UAT, encuesta y pruebas técnicas. No deben sustituirse por datos estimados.

## Regenerar Word

Desde la raíz del repositorio:

```powershell
python Documentos\Tesis\versioniniicial_tesis\generar_tesis.py
```

Requiere Python 3 y `python-docx`.

## Actualizar índices en Word

Al abrir el documento:

1. Presione `Ctrl + A`.
2. Presione `F9`.
3. Seleccione **Actualizar toda la tabla**.

Esto actualiza el índice general, el índice de cuadros, el índice de figuras y la numeración de páginas.

## Formato aplicado

- Papel carta.
- Times New Roman, 12 puntos.
- Texto a doble espacio y justificado.
- Sangría de primera línea de 0.63 cm.
- Márgenes superior e izquierdo de 4 cm.
- Márgenes inferior y derecho de 2.5 cm.
- Preliminares con numeración romana.
- Cuerpo con numeración arábiga y primera página sin número visible.
- Capítulos en mayúsculas, centrados y con inicio en página impar.

La versión final debe ser revisada por el asesor y por el servicio de corrección gramatical y estilística indicado por la Universidad Galileo.
