# 🔍 Debug: Solicitudes Nuevas No Aparecen en Tiempo Real

## 🎯 Problema Reportado
- Las solicitudes de prueba SÍ se ven ✅
- Pero al crear una nueva solicitud, NO aparece en el historial ❌

---

## 🛠️ **Cambios Implementados (Para Debugging)**

### **1. Logs agregados en el API**
Ahora el controlador imprime mensajes en la consola de Visual Studio:

#### **Al CREAR solicitud (POST):**
```
[TRASLADOS POST] Intentando crear solicitud para alumno 1
[TRASLADOS POST] Fecha: 2025-01-25, Turno: Mañana
[TRASLADOS POST] ✅ Solicitud creada exitosamente. ID: 5
```

**O si falla:**
```
[TRASLADOS POST] ❌ Exception: Cannot insert explicit value for identity column
```

#### **Al VER historial (GET):**
```
[TRASLADOS] GET alumno/1 - Solicitudes reales encontradas: 2
[TRASLADOS] Retornando 2 solicitudes reales
```

**O si no hay datos:**
```
[TRASLADOS] GET alumno/1 - Solicitudes reales encontradas: 0
[TRASLADOS] No hay solicitudes reales, retornando datos de prueba
```

---

## 🧪 **Cómo Ver los Logs (Paso a Paso)**

### **Paso 1: Abrir Output Window**
1. En Visual Studio, ve al menú superior
2. Click en **View** → **Output** (o presiona `Ctrl+Alt+O`)
3. Se abre el panel "Output" abajo

### **Paso 2: Seleccionar fuente correcta**
1. En el panel Output, hay un dropdown que dice "Show output from:"
2. Selecciona: **"Debug"** o **"Web Server"**

### **Paso 3: Limpiar logs anteriores**
1. Click derecho en el panel Output
2. Click en **"Clear All"** (o `Ctrl+L`)

---

## 🧪 **Test Completo con Logs**

### **Ejecución:**

1. **Limpia logs** (`Ctrl+L` en Output window)
2. **Ejecuta el proyecto** (`F5`)
3. **Ve al calendario**: `https://localhost:7240/Padres/ConfirmarAsistencia`
4. **Crea una solicitud**:
   - Selecciona día futuro
   - Click "Solicitar Traslado de Bus"
   - Llena formulario
   - Click "Enviar Solicitud"
5. **Observa Output window** inmediatamente
6. **Ve al historial**: Click botón "Ver Mis Traslados" o navega a `/Padres/Traslados`
7. **Observa Output window** de nuevo

---

## 📊 **Interpretación de Logs**

### **Escenario A: Todo funciona correctamente**
```
[TRASLADOS POST] Intentando crear solicitud para alumno 1
[TRASLADOS POST] Fecha: 2025-01-25, Turno: Mañana
[TRASLADOS POST] ✅ Solicitud creada exitosamente. ID: 5
[TRASLADOS] GET alumno/1 - Solicitudes reales encontradas: 1
[TRASLADOS] Retornando 1 solicitudes reales
```
**Resultado**: Deberías ver tu solicitud nueva + 3 de prueba (4 total)

---

### **Escenario B: POST falla (modo simulación)**
```
[TRASLADOS POST] Intentando crear solicitud para alumno 1
[TRASLADOS POST] ❌ Exception: The INSERT statement conflicted with...
[TRASLADOS] GET alumno/1 - Solicitudes reales encontradas: 0
[TRASLADOS] No hay solicitudes reales, retornando datos de prueba
```
**Resultado**: Solo ves las 3 solicitudes de prueba (no se guardó en BD)

**Causas comunes:**
- Tabla `genesis.SolicitudTraslado` no existe
- Foreign keys fallan (IdAlumno=1 no existe, IdBus no existe)
- Identity column issue

---

### **Escenario C: GET falla pero POST funciona**
```
[TRASLADOS POST] ✅ Solicitud creada exitosamente. ID: 5
[TRASLADOS ERROR] Object reference not set to an instance...
[TRASLADOS] Retornando datos de prueba por error
```
**Resultado**: Se guardó en BD pero GET no puede leerla

**Causas comunes:**
- Problema en AutoMapper
- Navigation properties NULL
- Include() faltante en query

---

## 🔧 **Soluciones Según el Error**

### **Error: "Cannot insert explicit value for identity column"**

**Causa**: Intentando insertar IdSolicitud manualmente

**Solución**:
```csharp
// En el Service, NO asignar IdSolicitud
var solicitud = new SolicitudTraslado
{
    // IdSolicitud = 1, ← QUITAR ESTO
    IdAlumno = dto.IdAlumno,
    FechaTraslado = dto.FechaTraslado,
    // ...
};
```

---

### **Error: "The INSERT statement conflicted with FOREIGN KEY constraint"**

**Causa**: IdAlumno=1 no existe, o IdBusOrigen/Destino no existen

**Solución 1 - Crear alumno de prueba:**
```sql
SET IDENTITY_INSERT genesis.Alumnos ON;

INSERT INTO genesis.Alumnos (IdAlumno, Nombres, Apellidos, FechaNacimiento, IdBusAsignado, Direccion, UsuarioCreacion, FechaCreacion)
VALUES (1, 'Juan', 'Pérez', '2015-05-15', 1, 'Calle Principal 123', 'System', GETDATE());

SET IDENTITY_INSERT genesis.Alumnos OFF;
```

**Solución 2 - Cambiar ID en JavaScript:**
Ver qué alumnos existen:
```sql
SELECT TOP 5 IdAlumno, Nombres, Apellidos FROM genesis.Alumnos;
```

Cambiar en `ConfirmarAsistencia.cshtml` línea ~318:
```javascript
const idAlumno = 1; // ← Cambiar por ID real
```

---

### **Error: "Object reference not set to an instance"**

**Causa**: AutoMapper intenta mapear navigation properties NULL

**Verificar** en `TrasladoService.cs`:
```csharp
// Asegúrate de que el repository incluya las navegaciones
var solicitudes = await _repository.GetByAlumnoAsync(idAlumno);
```

**Verificar** en `SolicitudTrasladoRepository.cs`:
```csharp
return await _context.SolicitudTraslado
    .Include(s => s.Alumno)           // ← Importante
    .Include(s => s.BusOrigen)        // ← Importante
    .Include(s => s.BusDestino)       // ← Importante
    .Where(s => s.IdAlumno == idAlumno)
    .ToListAsync();
```

---

## 🎨 **Cambio Temporal en GET (Para Debug)**

Ahora el GET retorna:
```
[Tus solicitudes reales] 
+
[3 solicitudes de prueba con prefijo "[DATOS DE PRUEBA]"]
```

**Ejemplo en el historial:**
```
┌──────────┬─────────┬─────────────┬────────────┐
│ Fecha    │ Turno   │ Bus         │ Estado     │
├──────────┼─────────┼─────────────┼────────────┤
│ Hoy      │ Mañana  │ BUS-003     │ Pendiente  │  ← TU SOLICITUD REAL
│ +3 días  │ Ambos   │ BUS-003     │ Pendiente  │  ← PRUEBA
│ Mes -1   │ Mañana  │ BUS-002     │ Aprobado   │  ← PRUEBA
│ Mes -2   │ Tarde   │ BUS-004     │ Rechazado  │  ← PRUEBA
└──────────┴─────────┴─────────────┴────────────┘
```

**Para diferenciarlas:**
- Solicitudes de prueba tienen `[DATOS DE PRUEBA]` en el motivo
- Al hacer click "Ver", verás el prefijo

---

## ✅ **Checklist de Verificación**

### **Antes de crear solicitud:**
- [ ] Output window abierto y limpio
- [ ] "Show output from" en "Debug" o "Web Server"
- [ ] Proyecto ejecutándose (`F5`)

### **Al crear solicitud:**
- [ ] Ver logs en Output window
- [ ] Buscar líneas que empiecen con `[TRASLADOS POST]`
- [ ] Verificar si dice "✅ Solicitud creada" o "❌ Exception"

### **Al ver historial:**
- [ ] Ver logs en Output window
- [ ] Buscar líneas que empiecen con `[TRASLADOS]`
- [ ] Verificar cuántas solicitudes reales encontró
- [ ] Contar filas en la tabla del navegador

---

## 📝 **Reporte para Debugging**

Después de hacer el test, copia y pega aquí:

**1. Logs del POST (Output window):**
```
[Copiar aquí las líneas que empiezan con [TRASLADOS POST]]
```

**2. Logs del GET (Output window):**
```
[Copiar aquí las líneas que empiezan con [TRASLADOS]]
```

**3. ¿Cuántas filas ves en el historial?**
- [ ] 0 (mensaje "No hay solicitudes")
- [ ] 3 (solo datos de prueba)
- [ ] 4+ (tu solicitud + datos de prueba)

**4. ¿Alguna tiene "[DATOS DE PRUEBA]" en el motivo?**
- [ ] Sí
- [ ] No
- [ ] No puedo verlo

---

## 🎯 **Próximo Paso**

Ejecuta el proyecto con los nuevos logs y repórtame:
1. Lo que ves en Output window (logs)
2. Cuántas solicitudes aparecen en el historial
3. Si ves tu solicitud nueva o solo las de prueba

Con esa info te doy la solución exacta. 🔧
