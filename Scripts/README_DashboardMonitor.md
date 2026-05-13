# 📋 Guía de Uso - Datos de Prueba Dashboard Monitor

## 🎯 Propósito
Scripts SQL y Migraciones de Entity Framework para cargar datos de prueba en el Dashboard Monitor de Transportes Genesis para presentaciones y demostraciones.

---

## 📂 Archivos Incluidos

### **Opción 1: Scripts SQL (Recomendado si tienes SQL Server conectado)**

#### 1. **SeedData_DashboardMonitor_Test.sql**
Script principal que inserta todos los datos de prueba:
- ✅ Bus BUS-001 (IdBus = 4)
- ✅ 8 Padres de familia
- ✅ 10 Alumnos asignados al bus
- ✅ 2 Rutas (Mañana y Tarde)
- ✅ 22 Paradas (11 por cada ruta)

#### 2. **SeedData_DashboardMonitor_Configuracion.sql**
Script auxiliar con opciones para:
- ⚙️ Activar/Desactivar rutas
- ⚙️ Marcar paradas como completadas
- ⚙️ Agregar más alumnos
- ⚙️ Consultas útiles para verificar datos
- ⚙️ Limpiar datos de prueba

### **Opción 2: Migración de Entity Framework (Si no tienes SQL Server conectado)**

#### 3. **Migrations/20260512200000_SeedData_DashboardMonitor.cs**
Migración de EF Core que inserta los mismos datos de prueba.
- ✅ Se aplica con: `dotnet ef database update`
- ✅ Se revierte con: `dotnet ef database update [migración-anterior]`
- ✅ Incluye método `Down()` para limpiar los datos

---

## 🚀 Instrucciones de Uso

### **MÉTODO A: Usar Scripts SQL Directamente (Recomendado)**

#### **Paso 1: Ejecutar el Script Principal**

1. Abre **SQL Server Management Studio (SSMS)** o **Azure Data Studio**
2. Conéctate a tu base de datos
3. Abre el archivo: `Scripts/SeedData_DashboardMonitor_Test.sql`
4. Ejecuta el script completo (F5 o botón "Execute")
5. Verifica que se muestre el mensaje: "✓ DATOS INSERTADOS EXITOSAMENTE"

---

### **MÉTODO B: Usar Migración de Entity Framework**

#### **Paso 1: Aplicar la Migración**

Desde la terminal de Visual Studio o PowerShell:

```powershell
# Aplicar la migración con datos de prueba
dotnet ef database update --project TransportesGenesis.csproj

# O si estás en la carpeta del proyecto:
dotnet ef database update
```

#### **Paso 2: Verificar que se aplicó correctamente**

```powershell
# Ver las migraciones aplicadas
dotnet ef migrations list
```

Deberías ver: `20260512200000_SeedData_DashboardMonitor` en la lista.

#### **Paso 3 (Opcional): Revertir la Migración**

Si necesitas eliminar los datos de prueba:

```powershell
# Revertir a la migración anterior
dotnet ef database update 20260505170000_CreateAlertaProximidadTable
```

---

### **Comparación de Métodos**

| Característica | Scripts SQL | Migración EF |
|----------------|-------------|--------------|
| **Requiere SQL Server** | ✅ Sí | ❌ No (usa connection string) |
| **Fácil de ejecutar** | ✅ Muy fácil | ⚙️ Requiere .NET CLI |
| **Reversible** | ⚠️ Manual | ✅ Automático (`Down()`) |
| **Integrado con EF** | ❌ No | ✅ Sí |
| **Mejor para** | Pruebas rápidas | Control de versiones |

---

## ✅ Verificar los Datos

Ejecuta esta consulta para confirmar que todo se insertó correctamente:

```sql
-- Ver resumen completo
SELECT 
    'Bus' AS Tipo,
    CAST(COUNT(*) AS VARCHAR) AS Cantidad
FROM genesis.Buses WHERE IdBus = 4

UNION ALL

SELECT 'Padres', CAST(COUNT(*) AS VARCHAR)
FROM genesis.Padres WHERE IdPadre BETWEEN 100 AND 110

UNION ALL

SELECT 'Alumnos', CAST(COUNT(*) AS VARCHAR)
FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220

UNION ALL

SELECT 'Rutas', CAST(COUNT(*) AS VARCHAR)
FROM genesis.Rutas WHERE IdBus = 4

UNION ALL

SELECT 'Paradas', CAST(COUNT(*) AS VARCHAR)
FROM genesis.Paradas WHERE IdRuta IN (10, 11);
```

---

## 🎮 Acceder al Dashboard Monitor

1. Ejecuta la aplicación (F5 en Visual Studio)
2. Inicia sesión como **Monitor** o **Piloto**
3. Navega a: **Dashboard Monitor** o **Registrar Asistencia**
4. Verás los 10 alumnos listados con sus paradas

---

## 🎬 Escenarios de Presentación

### **Escenario 1: Mostrar Ruta de Mañana**
```sql
-- Activar solo la ruta de Mañana
UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdRuta = 10;
UPDATE genesis.Rutas SET EsActiva = 0 WHERE IdRuta = 11;
UPDATE genesis.Paradas SET Completada = 0 WHERE IdRuta = 10;
```

### **Escenario 2: Mostrar Ruta de Tarde**
```sql
-- Activar solo la ruta de Tarde
UPDATE genesis.Rutas SET EsActiva = 0 WHERE IdRuta = 10;
UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdRuta = 11;
UPDATE genesis.Paradas SET Completada = 0 WHERE IdRuta = 11;
```

### **Escenario 3: Simular Progreso de Ruta**
```sql
-- Marcar primeras 3 paradas como completadas
UPDATE genesis.Paradas 
SET Completada = 1 
WHERE IdParada IN (120, 121, 122) AND IdRuta = 11;
```

### **Escenario 4: Simular "No hay ruta"**
```sql
-- Desactivar todas las rutas
UPDATE genesis.Rutas SET EsActiva = 0 WHERE IdBus = 4;
```

---

## 👥 Datos Insertados

### **Bus**
- **Placa:** BUS-001
- **Modelo:** Mercedes-Benz Sprinter
- **Capacidad:** 35 pasajeros
- **IdBus:** 4

### **Alumnos (10 total)**
| ID | Nombre | Zona | Dirección |
|----|--------|------|-----------|
| 200 | Diego Martínez | Zona 1 | 5a Avenida 12-45 |
| 201 | Sofía Martínez | Zona 1 | 5a Avenida 12-45 |
| 202 | Mateo García | Zona 2 | 12 Calle 8-30 |
| 203 | Isabella Rodríguez | Zona 2 | 7a Avenida 15-22 |
| 204 | Santiago López | Zona 4 | Ruta 6 9-55 |
| 205 | Valentina Hernández | Zona 4 | 11 Avenida 18-40 |
| 206 | Sebastián Gómez | Zona 9 | 4a Avenida 12-20 |
| 207 | Camila Díaz | Zona 10 | Boulevard Los Próceres 22-45 |
| 208 | Matías Torres | Zona 11 | Calzada Roosevelt 32-10 |
| 209 | Lucía Torres | Zona 11 | Calzada Roosevelt 32-10 |

### **Rutas**
- **Ruta Mañana** (IdRuta: 10): 06:00 AM - 11 paradas
- **Ruta Tarde** (IdRuta: 11): 02:00 PM - 11 paradas

---

## 🔍 Consultas Útiles

### Ver todos los alumnos del bus
```sql
SELECT 
    a.IdAlumno,
    a.Nombre + ' ' + a.Apellido AS Alumno,
    a.Direccion,
    b.Placa
FROM genesis.Alumnos a
INNER JOIN genesis.Buses b ON a.IdBusAsignado = b.IdBus
WHERE b.IdBus = 4
ORDER BY a.IdAlumno;
```

### Ver paradas de una ruta
```sql
SELECT 
    p.Orden,
    ISNULL(a.Nombre + ' ' + a.Apellido, 'COLEGIO') AS Alumno,
    p.Direccion,
    CONVERT(VARCHAR(5), p.HoraEstimada, 108) AS Hora,
    CASE WHEN p.Completada = 1 THEN 'Sí' ELSE 'No' END AS Completada
FROM genesis.Paradas p
LEFT JOIN genesis.Alumnos a ON p.IdAlumno = a.IdAlumno
WHERE p.IdRuta = 11  -- Cambiar por 10 o 11
ORDER BY p.Orden;
```

### Ver estado de las rutas
```sql
SELECT 
    r.IdRuta,
    r.Nombre,
    r.TipoRuta,
    CONVERT(VARCHAR(5), r.HoraInicio, 108) AS Hora,
    CASE WHEN r.EsActiva = 1 THEN 'Activa' ELSE 'Inactiva' END AS Estado,
    COUNT(p.IdParada) AS TotalParadas
FROM genesis.Rutas r
LEFT JOIN genesis.Paradas p ON r.IdRuta = p.IdRuta
WHERE r.IdBus = 4
GROUP BY r.IdRuta, r.Nombre, r.TipoRuta, r.HoraInicio, r.EsActiva;
```

---

## 🧹 Limpiar Datos de Prueba

Si necesitas empezar de nuevo:

```sql
BEGIN TRANSACTION;

DELETE FROM genesis.Paradas WHERE IdParada BETWEEN 100 AND 200;
DELETE FROM genesis.Rutas WHERE IdRuta BETWEEN 10 AND 12;
DELETE FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220;
DELETE FROM genesis.Padres WHERE IdPadre BETWEEN 100 AND 110;

-- Opcional: eliminar el bus
-- DELETE FROM genesis.Buses WHERE IdBus = 4;

COMMIT TRANSACTION;
```

Luego vuelve a ejecutar `SeedData_DashboardMonitor_Test.sql`

---

## ⚠️ Notas Importantes

1. **IdBus = 4**: El código del monitor usa `IdBus = 4` por defecto (ver `Pages\Monitor\MiRuta.cshtml.cs`)
2. **Turno Automático**: El sistema detecta automáticamente el turno según la hora:
   - Antes de 12:00 PM → Turno Mañana
   - Después de 12:00 PM → Turno Tarde
3. **Coordenadas Reales**: Todas las direcciones usan coordenadas GPS reales de la Ciudad de Guatemala
4. **Datos Aislados**: Los IDs están en rangos específicos (100-110, 200-220) para no interferir con datos reales

---

## 🆘 Solución de Problemas

### Problema: "No hay alumnos en la ruta"
**Solución:**
```sql
-- Verificar que el bus existe
SELECT * FROM genesis.Buses WHERE IdBus = 4;

-- Verificar que hay alumnos asignados
SELECT * FROM genesis.Alumnos WHERE IdBusAsignado = 4;

-- Verificar que hay rutas activas
SELECT * FROM genesis.Rutas WHERE IdBus = 4 AND EsActiva = 1;
```

### Problema: "No hay ruta calculada para este bus"
**Solución:**
```sql
-- Activar la ruta según el turno actual
UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdBus = 4 AND TipoRuta = 'Tarde'; -- o 'Mañana'
```

### Problema: Error de duplicados al ejecutar el script
**Solución:** El script ya maneja esto automáticamente. Si persiste, ejecuta primero la sección de limpieza.

---

## 📞 Contacto

Para dudas o problemas con los scripts, contacta al equipo de desarrollo.

---

**✅ ¡Listo para la presentación!** 🚀
