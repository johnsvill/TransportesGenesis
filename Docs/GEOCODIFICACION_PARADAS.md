# 🗺️ Geocodificación Automática de Paradas

## ✨ Nueva Funcionalidad Implementada

Se ha mejorado la pantalla de **Gestionar Paradas** para facilitar la entrada de ubicaciones usando direcciones en lugar de coordenadas manuales.

---

## 🎯 ¿Qué problema resuelve?

**ANTES:**
- El administrador tenía que ingresar latitud y longitud manualmente
- No era amigable ni intuitivo
- Podía causar errores de precisión

**AHORA:**
- El administrador escribe una dirección normal (ej: "5ta Avenida 12-34 Zona 10, Guatemala")
- Hace clic en "🔍 Buscar en Mapa"
- El sistema obtiene automáticamente las coordenadas
- El mapa se centra en la ubicación encontrada

---

## 🚀 Cómo Usar

### Opción 1: Geocodificación por Dirección (NUEVO)

1. Abre el modal de "Crear/Editar Parada"
2. Escribe una dirección en el campo "Dirección de la Parada"
   - Ejemplo: `6ta Avenida 9-50 Zona 1, Guatemala`
   - Ejemplo: `Centro Comercial Pradera, Zona 10`
   - Ejemplo: `Universidad Rafael Landívar, Vista Hermosa`
3. Haz clic en el botón **"🔍 Buscar en Mapa"**
4. El sistema:
   - Busca la dirección en OpenStreetMap
   - Obtiene las coordenadas automáticamente
   - Actualiza los campos Latitud y Longitud
   - Centra el mapa en la ubicación
   - Coloca un marcador rojo temporal
5. Verifica que la ubicación sea correcta en el mapa
6. Haz clic en "Guardar"

### Opción 2: Click en el Mapa (Ya existía)

1. Haz clic en el botón **"Agregar Parada"** (se pone azul)
2. Haz clic en cualquier punto del mapa
3. Se abrirá el modal con las coordenadas prellenadas
4. Escribe la dirección manualmente
5. Haz clic en "Guardar"

---

## 💡 Consejos para Mejores Resultados

### ✅ Direcciones que funcionan bien:
- `5ta Avenida 12-34 Zona 10, Guatemala`
- `Centro Comercial Oakland, Guatemala`
- `Universidad de San Carlos, Guatemala`
- `Aeropuerto La Aurora, Guatemala`
- `Antigua Guatemala, Sacatepéquez`

### ❌ Direcciones que pueden fallar:
- `Casa de Juan` (muy vaga)
- `Zona 10` (sin dirección específica)
- `Cerca del parque` (sin referencias concretas)

### 🎯 Tips:
1. **Sé específico**: Agrega Zona, Ciudad, Departamento
2. **Usa referencias conocidas**: Centros comerciales, universidades, iglesias
3. **Incluye "Guatemala"**: Si no está en la dirección, el sistema lo agrega automáticamente
4. **Verifica en el mapa**: Siempre revisa que el marcador esté en el lugar correcto

---

## 🔧 Tecnología Utilizada

- **API de Geocodificación**: [Nominatim (OpenStreetMap)](https://nominatim.openstreetmap.org/)
- **Librería de Mapas**: Leaflet.js
- **Proveedor de Mapas**: OpenStreetMap

### ¿Por qué Nominatim?
- ✅ **Gratuito**: No requiere clave API
- ✅ **Sin límites estrictos**: Uso razonable permitido
- ✅ **Datos abiertos**: Alimentado por OpenStreetMap
- ✅ **Cobertura global**: Funciona en todo el mundo, incluyendo Guatemala

---

## 🐛 Solución de Problemas

### Problema: "Dirección no encontrada"
**Causa**: La dirección es demasiado vaga o no existe en OpenStreetMap

**Solución**:
1. Agrega más detalles (Zona, Ciudad, Guatemala)
2. Usa referencias conocidas (centros comerciales, escuelas)
3. Intenta con una dirección cercana más conocida
4. Como última opción, usa el método de "Click en el Mapa"

### Problema: El mapa no se centra correctamente
**Causa**: El navegador bloqueó la petición o problemas de red

**Solución**:
1. Recarga la página (F5)
2. Verifica tu conexión a internet
3. Intenta con otra dirección
4. Usa el modo de "Click en el Mapa"

### Problema: Las coordenadas son de otro país
**Causa**: La dirección es ambigua y se encontró en otro país

**Solución**:
1. Agrega ", Guatemala" al final de la dirección
2. Sé más específico con la ubicación
3. Usa el botón de "Click en el Mapa" para ubicarla manualmente

---

## 📊 Ejemplo Completo

### Caso de Uso: Agregar parada en el Colegio Americano

**Paso a paso:**

1. **Abrir modal**: Haz clic en "Agregar Parada" en la tabla (NO en el mapa)
   - O haz clic en el botón "Agregar Parada" (azul) y luego en el mapa

2. **Escribir dirección**:
   ```
   Colegio Americano de Guatemala, Vista Hermosa, Guatemala
   ```

3. **Geocodificar**: Haz clic en "🔍 Buscar en Mapa"

4. **Resultado**:
   - ✅ Latitud: 14.5833 (se llena automáticamente)
   - ✅ Longitud: -90.5167 (se llena automáticamente)
   - ✅ Marcador rojo en el mapa
   - ✅ Mensaje: "✅ Ubicación encontrada: Colegio Americano de Guatemala..."

5. **Configurar parada**:
   - Orden: 1
   - Parada Activa: ✅ (checked)

6. **Guardar**: Haz clic en "Guardar"

7. **Verificar**: La parada aparece en la tabla y en el mapa con un marcador azul

---

## 🔄 Flujo de Trabajo Recomendado

### Para Administradores:

```
1. Preparar lista de paradas en Excel/Word
   ├─ Colegio Americano, Vista Hermosa
   ├─ Centro Comercial Pradera, Zona 10
   └─ Universidad Rafael Landívar

2. Abrir "Gestionar Paradas" en el sistema

3. Para cada parada:
   ├─ Clic en "Agregar Parada" (tabla)
   ├─ Pegar dirección
   ├─ Clic en "Buscar en Mapa"
   ├─ Verificar ubicación en el mapa
   └─ Guardar

4. Ajustar orden de las paradas si es necesario

5. Asignar paradas a rutas en "Calcular Rutas"
```

---

## 📝 Notas Técnicas

### Límites de Uso
- **Nominatim**: Máximo 1 petición por segundo (ya implementado con delay)
- **No requiere API Key**: Servicio completamente gratuito
- **Rate Limiting**: El navegador controla automáticamente las peticiones

### Seguridad
- ✅ No se exponen datos sensibles
- ✅ Las coordenadas se validan antes de guardar
- ✅ Los campos de latitud/longitud son de solo lectura

### Privacidad
- ✅ No se almacenan direcciones en servidores externos
- ✅ Solo se envía la dirección a Nominatim para geocodificación
- ✅ Los datos quedan en tu base de datos local

---

## 🎨 Mejoras Futuras (Opcional)

- [ ] **Autocompletar direcciones** mientras el usuario escribe
- [ ] **Geocodificación inversa**: Click en el mapa → Obtener dirección automáticamente
- [ ] **Validación de país**: Asegurar que todas las coordenadas estén en Guatemala
- [ ] **Historial de búsquedas**: Recordar direcciones previamente buscadas
- [ ] **Integración con Google Maps API**: Para mayor precisión (requiere clave API de pago)

---

## ✅ Checklist de Prueba

- [ ] Crear parada con dirección completa
- [ ] Crear parada con solo nombre de lugar conocido
- [ ] Crear parada con dirección vaga (debe fallar y mostrar mensaje)
- [ ] Editar parada existente y cambiar ubicación con geocodificación
- [ ] Verificar que el marcador temporal se limpia al guardar
- [ ] Verificar que el mapa se centra correctamente
- [ ] Probar con direcciones en diferentes zonas de Guatemala
- [ ] Verificar que las coordenadas se guardan correctamente en la BD

---

## 📞 Soporte

Si encuentras problemas con la geocodificación:
1. Verifica tu conexión a internet
2. Intenta con una dirección más específica
3. Usa el método alternativo de "Click en el Mapa"
4. Reporta el problema con ejemplos de direcciones que no funcionaron

---

**Última actualización**: Mayo 2026  
**Desarrollado por**: David & Jonathan - Transportes Genesis  
**Versión**: 1.0
