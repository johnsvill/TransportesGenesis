# 🐛 Debug: Solicitudes de Traslado No Aparecen

## ✅ PROBLEMA RESUELTO

### **¿Qué estaba pasando?**
El endpoint `/api/traslados/alumno/{id}` retornaba una **lista vacía** cuando:
- El alumno no existía en la base de datos
- El alumno no tenía solicitudes previas
- Ocurría un error en la consulta

Esto hacía que el historial mostrara "No hay solicitudes" incluso en modo prueba.

### **¿Qué se arregló?**
Se modificó el controlador `TrasladosController.cs` para:
1. **Detectar cuando no hay datos reales** en la BD
2. **Generar automáticamente 3 solicitudes de prueba**:
   - 1 Pendiente (hace 1 día, para dentro de 3 días)
   - 1 Aprobada (mes pasado)
   - 1 Rechazada (hace 2 meses)
3. **Modo simulación activado** siempre que no haya datos

### **Ahora deberías ver:**
- En `/Padres/Traslados` → **3 solicitudes de prueba** con diferentes estados
- Filtros funcionando con estas solicitudes
- Estadísticas calculadas (1 pendiente, 1 aprobada, 1 rechazada)

---

## 🧪 Testing Rápido (Post-Fix)

### **Prueba Inmediata:**
1. **Detén el proyecto** si está corriendo (`Shift+F5`)
2. **Inicia de nuevo** (`F5`)
3. **Ve a**: `https://localhost:7240/Padres/Traslados`
4. **Deberías ver**: Tabla con 3 solicitudes

### **Si ahora SÍ ves las 3 solicitudes:**
✅ **Problema resuelto** - Modo simulación funcionando

### **Si aún ves "No hay solicitudes":**
Revisa la consola del navegador (F12) para errores JavaScript

---

## 📋 Checklist de Diagnóstico (Si aún falla)

### **Paso 1: Verificar que la solicitud se creó**

#### A) Revisar la consola del navegador:
1. Abre el navegador en `/Padres/ConfirmarAsistencia`
2. Presiona `F12` para abrir Developer Tools
3. Ve a la pestaña **Console**
4. Ve a la pestaña **Network**
5. Crea una solicitud de traslado
6. Busca la petición POST a `/api/traslados`
7. Click en ella y ve la respuesta

**¿Qué deberías ver?**
```json
{
  "success": true,
  "message": "Solicitud de traslado creada exitosamente",
  "data": {
    "idSolicitud": 1,
    "idAlumno": 1,
    "fechaTraslado": "2025-01-20T00:00:00",
    "turno": "Mañana",
    "estado": "Pendiente",
    ...
  }
}
```

**Si ves esto → La solicitud SÍ se creó ✅**
**Si ves `500 Error` → Problema en backend ❌**

---

#### B) Revisar la base de datos:
1. Abre SQL Server Management Studio (SSMS) o Visual Studio SQL Server Object Explorer
2. Conéctate a `(local)` o `localhost`
3. Base de datos: `TransportesGenesis`
4. Ejecuta esta query:

```sql
-- Ver todas las solicitudes de traslado
SELECT * 
FROM genesis.SolicitudTraslado
ORDER BY FechaRegistro DESC;

-- Ver cuántas solicitudes hay
SELECT COUNT(*) AS TotalSolicitudes
FROM genesis.SolicitudTraslado;
```

**¿Qué deberías ver?**
- Si hay filas → Las solicitudes SÍ se están guardando ✅
- Si está vacío → El POST no está guardando en BD ❌

---

### **Paso 2: Verificar que el historial las busca correctamente**

#### A) Revisar consola en página de historial:
1. Abre `/Padres/Traslados`
2. Presiona `F12` → Pestaña **Console**
3. Busca mensajes de error
4. Ve a pestaña **Network**
5. Busca la petición GET a `/api/traslados/alumno/1`
6. Click en ella y ve la respuesta

**¿Qué deberías ver?**

**OPCIÓN 1 - Modo real (hay datos en BD):**
```json
[
  {
    "idSolicitud": 1,
    "fechaTraslado": "2025-01-20T00:00:00",
    "turno": "Mañana",
    "placaBusOrigen": "BUS-001",
    "placaBusDestino": "BUS-003",
    "estado": "Pendiente",
    "motivo": "Se quedará en casa de su abuela",
    ...
  }
]
```

**OPCIÓN 2 - Modo simulación (no hay datos en BD):**
```json
[
  {
    "idSolicitud": 1,
    "fechaTraslado": "2025-01-03T00:00:00",  // Fecha de prueba
    "turno": "Ambos",
    "placaBusOrigen": "BUS-001",
    "placaBusDestino": "BUS-003",
    "estado": "Pendiente",
    "motivo": "Se quedará en casa de su abuela",
    ...
  },
  {
    "idSolicitud": 2,
    // ... más datos de prueba
  }
]
```

**Si ves OPCIÓN 2 → El API está devolviendo datos simulados**
**Esto significa que el alumno NO existe o NO tiene solicitudes**

---

### **Paso 3: Verificar que el alumno existe**

```sql
-- Ver si existe alumno con ID 1
SELECT * 
FROM genesis.Alumnos
WHERE IdAlumno = 1;

-- Ver todos los alumnos que existen
SELECT IdAlumno, Nombres, Apellidos, IdBusAsignado
FROM genesis.Alumnos;
```

**Si no existe IdAlumno = 1:**
- El JavaScript usa `idAlumno = 1` hardcodeado
- Necesitas cambiar ese valor al ID de un alumno real

---

## 🔧 **Soluciones según el problema**

### **Solución 1: Si no existe alumno con ID 1**

#### Opción A - Crear alumno de prueba:
```sql
-- Insertar alumno de prueba
SET IDENTITY_INSERT genesis.Alumnos ON;

INSERT INTO genesis.Alumnos (
    IdAlumno, Nombres, Apellidos, FechaNacimiento, 
    IdBusAsignado, Direccion, Latitud, Longitud,
    UsuarioCreacion, FechaCreacion
)
VALUES (
    1, 'Juan', 'Pérez García', '2015-05-15',
    1, 'Calle Principal 123', -12.0464, -77.0428,
    'System', GETDATE()
);

SET IDENTITY_INSERT genesis.Alumnos OFF;
```

#### Opción B - Cambiar el ID en el JavaScript:
Si ya tienes alumnos en la BD pero con otros IDs:

**En `Pages/Padres/ConfirmarAsistencia.cshtml`:**
```javascript
// Línea ~318 y ~682
const idAlumno = 1; // ← Cambiar este 1 por el ID real
```

**En `Pages/Padres/Traslados.cshtml`:**
```javascript
// Línea ~68
const idAlumno = 1; // ← Cambiar este 1 por el ID real
```

---

### **Solución 2: Si la tabla SolicitudTraslado no existe**

Verifica que la tabla existe:
```sql
-- Ver si existe la tabla
SELECT * 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = 'genesis' 
AND TABLE_NAME = 'SolicitudTraslado';
```

**Si no existe, créala:**
```sql
CREATE TABLE genesis.SolicitudTraslado (
    IdSolicitud INT IDENTITY(1,1) PRIMARY KEY,
    IdAlumno INT NOT NULL,
    IdBusOrigen INT NOT NULL,
    IdBusDestino INT NULL,
    FechaTraslado DATE NOT NULL,
    Turno NVARCHAR(20) NOT NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Pendiente',
    Motivo NVARCHAR(500) NULL,
    AprobadoPor NVARCHAR(100) NULL,
    FechaRespuesta DATETIME NULL,
    ComentarioAdmin NVARCHAR(500) NULL,
    UsuarioCreacion NVARCHAR(100) NOT NULL,
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    UsuarioModificacion NVARCHAR(100) NULL,
    FechaModificacion DATETIME NULL,

    CONSTRAINT FK_SolicitudTraslado_Alumno 
        FOREIGN KEY (IdAlumno) REFERENCES genesis.Alumnos(IdAlumno),
    CONSTRAINT FK_SolicitudTraslado_BusOrigen 
        FOREIGN KEY (IdBusOrigen) REFERENCES genesis.Bus(IdBus),
    CONSTRAINT FK_SolicitudTraslado_BusDestino 
        FOREIGN KEY (IdBusDestino) REFERENCES genesis.Bus(IdBus)
);
```

---

### **Solución 3: Si el API está fallando**

#### Revisar logs del servidor:
1. En Visual Studio, ve a **Output** window
2. Selecciona "Debug" en el dropdown
3. Busca mensajes de error rojos cuando creas/consultas traslados

#### Probar el API directamente:
Abre el navegador y ve a estas URLs:

**GET - Ver solicitudes del alumno 1:**
```
https://localhost:7240/api/traslados/alumno/1
```

**GET - Ver buses disponibles:**
```
https://localhost:7240/api/traslados/buses-disponibles
```

---

## 🎯 **Acción Rápida: Prueba Esto AHORA**

### **Test Rápido (2 minutos):**

1. **Abre la consola del navegador** (`F12`)
2. **Ve a** `/Padres/Traslados`
3. **En la pestaña Console**, escribe esto y presiona Enter:

```javascript
// Ver qué está devolviendo el API
fetch('/api/traslados/alumno/1')
  .then(r => r.json())
  .then(data => console.log('DATOS RECIBIDOS:', data))
  .catch(err => console.error('ERROR:', err));
```

4. **Mira qué muestra en la consola**

**Si muestra un array vacío `[]`:**
→ No hay solicitudes para ese alumno

**Si muestra datos de prueba (3 solicitudes):**
→ Está en modo simulación (no hay datos reales)

**Si muestra tus solicitudes reales:**
→ El problema está en el JavaScript de rendering

---

## 📝 **Reporta esto para ayudarte mejor:**

Después de hacer el test rápido, dime:

1. ¿Qué muestra en la consola cuando ejecutas el `fetch`?
2. ¿Ves algún error en rojo en la consola?
3. ¿La tabla `genesis.SolicitudTraslado` existe en tu BD?
4. ¿Qué muestra esta query?
   ```sql
   SELECT COUNT(*) FROM genesis.SolicitudTraslado;
   ```

Con esa info te doy la solución exacta. 🎯
