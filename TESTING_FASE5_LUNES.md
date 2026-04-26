# 🧪 TESTING FASE 5 - Plan para Lunes 27/04/2026

## ✅ Estado Actual
- **FASE 5 implementada al 100%** (domingo 26/04/2026)
- **Compilación exitosa** sin errores
- **3 tests básicos completados** (IdBus 4, turno Mañana)
- **Pendiente:** Testing con datos reales en día hábil

---

## 🎯 Objetivos del Testing

Validar todas las funcionalidades de FASE 5 con escenarios reales:
1. ✅ Cálculo de rutas turno **Tarde** (colegio primera parada)
2. ✅ Cálculo masivo desde página **Admin**
3. ✅ Integración con **traslados temporales**
4. ✅ Visualización correcta en **mapa piloto**

---

## 📋 Plan de Testing (30-45 minutos)

### PASO 1: Preparación de Datos (10 min)

#### 1.1 Verificar Buses Existen
```sql
-- Ejecutar en SQL Server
USE TransportesGenesis;
GO

SELECT IdBus, Placa, Modelo, Capacidad, Activo 
FROM genesis.Buses
ORDER BY IdBus;
GO
```

**Si no hay buses, ejecutar:**
```powershell
# Desde PowerShell en C:\Proyectos\TransportesGenesis\
sqlcmd -S (local) -d TransportesGenesis -i Scripts\Setup_Buses_Prueba.sql
```

#### 1.2 Crear Asistencias Confirmadas para Varios Buses
```sql
-- Crear asistencias para Buses 1, 2, 3 (además del 4 que ya tiene)
USE TransportesGenesis;
GO

DECLARE @FechaLunes DATE = '2026-04-27';

-- Bus 1: 3 alumnos
UPDATE genesis.AsistenciaAlumno
SET IdBus = 1, 
    FechaConfirmacion = GETDATE(),
    ConfirmadoPor = 'Padre'
WHERE IdAlumno IN (2, 3, 5)  -- María, Carlos, Luis
  AND Fecha = @FechaLunes;

-- Bus 2: 2 alumnos
UPDATE genesis.AsistenciaAlumno
SET IdBus = 2, 
    FechaConfirmacion = GETDATE(),
    ConfirmadoPor = 'Padre'
WHERE IdAlumno IN (4, 6)  -- Ana, Laura
  AND Fecha = @FechaLunes;

-- Bus 3: 2 alumnos
UPDATE genesis.AsistenciaAlumno
SET IdBus = 3, 
    FechaConfirmacion = GETDATE(),
    ConfirmadoPor = 'Padre'
WHERE IdAlumno IN (7, 8)  -- Diego, Sofía
  AND Fecha = @FechaLunes;

-- Verificar distribución
SELECT 
    IdBus,
    COUNT(*) as CantidadAlumnos,
    STRING_AGG(CAST(IdAlumno AS VARCHAR), ', ') as IdsAlumnos
FROM genesis.AsistenciaAlumno
WHERE Fecha = @FechaLunes
  AND FechaConfirmacion IS NOT NULL
GROUP BY IdBus
ORDER BY IdBus;
GO
```

#### 1.3 Crear Asistencias para Turno Tarde
```sql
-- Crear confirmaciones para turno Tarde (misma fecha)
USE TransportesGenesis;
GO

DECLARE @FechaLunes DATE = '2026-04-27';

-- Si no existen registros para Tarde, insertar
IF NOT EXISTS (SELECT 1 FROM genesis.AsistenciaAlumno WHERE Fecha = @FechaLunes AND IdAlumno = 1)
BEGIN
    -- Reutilizar alumnos del Bus 4 para turno Tarde
    INSERT INTO genesis.AsistenciaAlumno 
    (IdAlumno, Fecha, IdBus, FechaConfirmacion, ConfirmadoPor, FechaRegistro, RegistradoPor)
    SELECT 
        IdAlumno,
        Fecha,
        IdBus,
        FechaConfirmacion,
        ConfirmadoPor,
        GETDATE(),
        'testing'
    FROM genesis.AsistenciaAlumno
    WHERE Fecha = @FechaLunes 
      AND IdBus = 4
      AND FechaConfirmacion IS NOT NULL;
END

PRINT '✓ Asistencias turno Tarde listas';
GO
```

---

### PASO 2: Test Cálculo Masivo Admin (10 min)

#### 2.1 Acceder a Página Admin
1. Abrir navegador
2. Ir a: `https://localhost:XXXXX/Admin/CalcularRutas`
3. Seleccionar:
   - **Fecha:** Lunes 27/04/2026
   - **Turno:** Mañana
4. Click **"Calcular Todas las Rutas"**

#### 2.2 Verificar Resultados Esperados

**Tabla de Resultados debe mostrar:**

| ID Bus | Estado | Paradas Esperadas | Verificar |
|--------|--------|-------------------|-----------|
| 1 | ✅ Exitoso | 4 (3 alumnos + colegio) | Badge verde, cantidad correcta |
| 2 | ✅ Exitoso | 3 (2 alumnos + colegio) | Badge verde, cantidad correcta |
| 3 | ✅ Exitoso | 3 (2 alumnos + colegio) | Badge verde, cantidad correcta |
| 4 | ✅ Exitoso | 8 (7 alumnos + colegio) | Badge verde, cantidad correcta |

**Tarjetas de Resumen:**
- 🚌 Buses Procesados: **4**
- 📍 Paradas Generadas: **18** (4+3+3+8)
- 📅 Fecha: 27/04/2026 - Mañana

#### 2.3 Verificar Visualización de Rutas
1. En la tabla, click **"Ver"** en Bus 4
2. Debe abrir nueva pestaña con mapa
3. **Verificar:**
   - ✅ 8 marcadores visibles (7 rojos + 1 azul 🏫)
   - ✅ Colegio es **última parada** (orden 8)
   - ✅ Polyline azul conecta todas las paradas
   - ✅ Lista muestra 8 paradas ordenadas

---

### PASO 3: Test Turno Tarde (10 min)

#### 3.1 Calcular Ruta Tarde
1. Regresar a `/Admin/CalcularRutas`
2. Seleccionar:
   - **Fecha:** Lunes 27/04/2026
   - **Turno:** **Tarde**
3. Click **"Calcular Todas las Rutas"**

#### 3.2 Verificar Orden de Paradas
1. Click **"Ver"** en Bus 4 (tarde)
2. **CRÍTICO:** Verificar que:
   - ✅ Colegio aparece como **PRIMERA parada** (orden 1)
   - ✅ Icono 🏫 está en posición 1 de la lista
   - ✅ Alumnos ordenados después (2, 3, 4, 5, 6, 7, 8)
   - ✅ Polyline inicia en colegio

#### 3.3 Consultar en Base de Datos
```sql
-- Verificar orden de paradas turno Tarde
USE TransportesGenesis;
GO

SELECT TOP 1
    r.IdRuta,
    r.TipoRuta,
    r.HoraInicio,
    p.Orden,
    p.IdAlumno,
    CASE 
        WHEN p.IdAlumno IS NULL THEN '🏫 COLEGIO'
        ELSE a.Nombre
    END as Nombre,
    p.HoraEstimada
FROM genesis.Rutas r
INNER JOIN genesis.Paradas p ON r.IdRuta = p.IdRuta
LEFT JOIN genesis.Alumnos a ON p.IdAlumno = a.IdAlumno
WHERE r.IdBus = 4
  AND r.TipoRuta = 'Tarde'
  AND CAST(r.FechaCreacion AS DATE) = '2026-04-27'
ORDER BY r.FechaCreacion DESC, p.Orden;
GO
```

**Resultado esperado primera parada:**
```
Orden: 1
IdAlumno: NULL
Nombre: 🏫 COLEGIO
```

---

### PASO 4: Test Traslados Temporales (10 min)

#### 4.1 Crear Traslado Temporal
```sql
-- Crear traslado: Alumno 3 (Carlos) de Bus 4 → Bus 2 temporal
USE TransportesGenesis;
GO

DECLARE @FechaLunes DATE = '2026-04-27';

-- Insertar solicitud de traslado
SET IDENTITY_INSERT genesis.SolicitudesTraslado ON;

INSERT INTO genesis.SolicitudesTraslado 
(IdSolicitud, IdAlumno, IdBusOriginal, IdBusDestino, FechaInicio, FechaFin, 
 Motivo, Estado, FechaSolicitud, SolicitadoPor, FechaRespuesta, RespondidoPor, FechaRegistro, RegistradoPor)
VALUES 
(100, 3, 4, 2, @FechaLunes, @FechaLunes, 
 'Testing traslado temporal', 'Aprobado', GETDATE(), 'Padre', GETDATE(), 'admin', GETDATE(), 'testing');

SET IDENTITY_INSERT genesis.SolicitudesTraslado OFF;

-- Actualizar asistencia con traslado
UPDATE genesis.AsistenciaAlumno
SET IdBusTemporalMañana = 2  -- Bus destino
WHERE IdAlumno = 3
  AND Fecha = @FechaLunes;

PRINT '✓ Traslado temporal creado: Alumno 3 (Carlos) → Bus 2';
GO
```

#### 4.2 Recalcular Rutas con Traslado
1. Ir a `/Admin/CalcularRutas`
2. Seleccionar: Fecha = 27/04/2026, Turno = Mañana
3. Click **"Calcular Todas las Rutas"**

#### 4.3 Verificar Redistribución
**Verificar en tabla:**
- **Bus 2:** Ahora debe tener **4 paradas** (era 3)
  - Ana, Laura, **Carlos** (trasladado), + Colegio
- **Bus 4:** Ahora debe tener **7 paradas** (era 8)
  - Juan, María, Luis, Laura, Diego, Sofía, + Colegio (sin Carlos)

**Verificar en mapa:**
1. Click "Ver" Bus 2 → Debe mostrar 4 marcadores (incluyendo Carlos)
2. Click "Ver" Bus 4 → Debe mostrar 7 marcadores (sin Carlos)

#### 4.4 Consulta SQL de Verificación
```sql
-- Verificar Carlos en Bus 2
SELECT 
    r.IdRuta,
    r.IdBus,
    r.TipoRuta,
    COUNT(p.IdParada) as TotalParadas,
    STRING_AGG(a.Nombre, ', ') as Alumnos
FROM genesis.Rutas r
INNER JOIN genesis.Paradas p ON r.IdRuta = p.IdRuta
LEFT JOIN genesis.Alumnos a ON p.IdAlumno = a.IdAlumno
WHERE r.IdBus IN (2, 4)
  AND r.TipoRuta = 'Mañana'
  AND CAST(r.FechaCreacion AS DATE) = '2026-04-27'
GROUP BY r.IdRuta, r.IdBus, r.TipoRuta
ORDER BY r.IdBus;
GO
```

---

### PASO 5: Test Página Piloto (5 min)

#### 5.1 Acceder como Piloto
1. Ir a: `https://localhost:XXXXX/Piloto/MiRuta`
2. **Verificar:**
   - ✅ Mapa muestra ruta del día
   - ✅ Estadísticas correctas (Total/Completadas/Pendientes)
   - ✅ Lista de paradas ordenada
   - ✅ Botones "Marcar Completada" funcionales

#### 5.2 Test Marcar Paradas
1. Click **"Marcar Completada"** en parada 1
2. **Verificar:**
   - ✅ Badge cambia a verde: "Completada"
   - ✅ Marcador en mapa cambia de rojo a verde
   - ✅ Botón cambia a "Desmarcar"
   - ✅ Estadísticas actualizan: +1 Completada, -1 Pendiente
   - ✅ Barra de progreso aumenta
   - ✅ Toast de notificación aparece

3. Click **"Desmarcar"** en misma parada
4. **Verificar:**
   - ✅ Vuelve a estado original (gris/rojo)

---

## ✅ Checklist de Validación

### Funcionalidades Core
- [ ] **Cálculo masivo Admin**: 4 buses procesados exitosamente
- [ ] **Paradas correctas**: Cantidad esperada por bus (4, 3, 3, 8)
- [ ] **Turno Mañana**: Colegio última parada (orden final)
- [ ] **Turno Tarde**: Colegio primera parada (orden 1)
- [ ] **Traslado temporal**: Alumno cambia de bus correctamente
- [ ] **Mapa piloto**: Visualización completa con iconos
- [ ] **Marcar paradas**: Estado persiste en BD
- [ ] **Estadísticas**: Cálculos correctos en tiempo real

### Elementos Visuales
- [ ] **Icono colegio**: 🏫 azul 40px visible
- [ ] **Marcadores alumnos**: Círculos rojos 30px con número
- [ ] **Polyline**: Línea azul conecta todas las paradas
- [ ] **Badges estado**: Verde (completada) / Gris (pendiente)
- [ ] **Tabla admin**: Filas correctamente coloreadas
- [ ] **Toast**: Notificaciones aparecen al marcar

### Validaciones
- [ ] **Fin de semana**: Mensaje educativo si se intenta sábado/domingo
- [ ] **Sin asistencias**: Mensaje apropiado si bus sin alumnos confirmados
- [ ] **Console logs**: Verificar en DevTools (F12) que no hay errores

---

## 🐛 Problemas Comunes y Soluciones

### Problema 1: "No hay rutas para mostrar"
**Causa:** No hay asistencias confirmadas  
**Solución:** Ejecutar script PASO 1.2

### Problema 2: Colegio no aparece en mapa
**Causa:** Configuración del colegio no existe  
**Solución:** Ejecutar `Scripts/Setup_ConfiguracionColegio.sql`

### Problema 3: Marcadores no cargan en mapa
**Causa:** Error JavaScript o coordenadas inválidas  
**Solución:** 
- Abrir DevTools (F12) → Console
- Buscar errores rojos
- Verificar coordenadas GPS en BD no sean NULL

### Problema 4: Traslado no se aplica
**Causa:** Campo `IdBusTemporalMañana` no actualizado  
**Solución:** Ejecutar UPDATE en PASO 4.1 completo

### Problema 5: Tabla admin vacía
**Causa:** Fecha seleccionada es fin de semana  
**Solución:** Validación debe rechazar, revisar logs del servidor

---

## 📊 Resultados Esperados

**Al finalizar este testing, debes tener:**

✅ **4 rutas Mañana calculadas** (Buses 1, 2, 3, 4)  
✅ **4 rutas Tarde calculadas** (Buses 1, 2, 3, 4)  
✅ **18+ paradas generadas** en total  
✅ **1 traslado temporal funcionando** (Alumno 3)  
✅ **Visualización completa** en mapa piloto  
✅ **Marcar paradas operativo**  

**Tiempo invertido:** 30-45 minutos  
**Issues encontrados:** Documentar en GitHub Issues  
**Screenshots:** Capturar tabla admin + mapa piloto para documentación

---

## 🚀 Después del Testing

Una vez validado todo:

1. **Commit de Testing:**
   ```bash
   git add .
   git commit -m "✅ FASE 5 validada con datos reales - Todos los tests pasaron"
   git push origin dev_david
   ```

2. **Actualizar documentación:**
   - Marcar checklist en `FASE5_ESTADO_FINAL_100.md`
   - Agregar screenshots si es necesario

3. **Continuar a FASE 7:**
   - Notificaciones en tiempo real con SignalR
   - Eventos: Bus cerca, Parada completada, Broadcast admin

---

## 📝 Notas Importantes

- **Ejecutar en día hábil:** Lunes 27/04/2026 (no funciona en fin de semana)
- **Tener VS Code/Visual Studio abierto:** Para ver logs en consola
- **SQL Server Management Studio:** Útil para queries de verificación
- **Navegador en modo desarrollo:** F12 abierto para ver console logs
- **No modificar datos existentes:** Usar alumnos/buses de prueba

---

**Preparado por:** David  
**Fecha:** 26/04/2026  
**Para ejecutar el:** Lunes 27/04/2026  
**Duración estimada:** 30-45 minutos  
**Prerequisito:** FASE 5 implementada al 100% ✅
