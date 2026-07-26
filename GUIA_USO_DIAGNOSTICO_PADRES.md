# 🔧 Guía de Uso: Diagnóstico y Corrección de Padre sin Ubicaciones

## 📋 Problema
El Padre de Familia no recibe las ubicaciones en tiempo real del bus donde va su hijo, a pesar de que el Monitor está enviando correctamente las posiciones.

## 🎯 Causa Principal
El problema ocurre cuando el usuario con rol `PadreDeFamilia` **NO tiene un registro vinculado** en la tabla `PadresDb`, o cuando los alumnos no están correctamente asignados a ese padre.

---

## ✅ Solución Paso a Paso

### Opción 1: Usar la Herramienta Web de Diagnóstico (RECOMENDADO)

1. **Inicia sesión como Administrador**
   - Usuario: `admin@transportesgenesis.com` (o tu usuario admin)

2. **Ve al Panel de Administrador**
   - URL: `https://localhost:7241/Admin`

3. **Click en "🔧 Diagnóstico Padres"**
   - URL directa: `https://localhost:7241/Admin/DiagnosticoPadres`

4. **Revisa los problemas detectados**
   La página mostrará:
   - ✅ Usuarios con rol PadreDeFamilia
   - ✅ Padres registrados en la tabla PadresDb
   - ✅ Alumnos y sus padres
   - ❌ Problemas encontrados

5. **Click en "Aplicar Corrección Automática"**
   Esto hará:
   - Vincular usuarios PadreDeFamilia con registros en PadresDb (por nombre)
   - Crear registros de Padre para usuarios sin entrada en PadresDb
   - Mostrar resultado de la corrección

6. **Verifica que todo esté correcto**
   - Todos los usuarios PadreDeFamilia deben tener estado: ✅ Vinculado a PadresDb
   - Todos los padres deben tener estado: ✅ Usuario vinculado
   - Todos los alumnos deben tener estado: ✅ Configurado correctamente

---

### Opción 2: Corrección Manual con SQL

Si prefieres usar SQL directamente:

1. **Ejecuta el script de diagnóstico**
   - Archivo: `Scripts/DiagnosticoPadreUbicaciones.sql`
   - En SQL Server Management Studio o Azure Data Studio

2. **Revisa los resultados**
   El script mostrará:
   - Usuarios con rol PadreDeFamilia
   - Padres sin usuario vinculado
   - Usuarios sin registro en PadresDb

3. **Ejecuta el script de corrección**
   - Archivo: `Scripts/CorreccionPadreUbicaciones.sql`
   - Esto vinculará automáticamente y creará registros faltantes

---

## 🧪 Validación Post-Corrección

1. **Reinicia la aplicación** (para limpiar caché)

2. **Abre la consola del servidor** (Output de Visual Studio)

3. **Inicia sesión como Padre**
   - Ve a: `/Padres/DashboardRutaBusAsignado`
   - Abre la consola del navegador (F12)

4. **Verifica los logs del servidor**:
   ```
   🔵 [PADRE] Cargando dashboard para userId: abc-123-def-456
   🔵 [AlumnoRepo] Buscando padre con UsuarioId: abc-123-def-456
   ✅ [AlumnoRepo] Padre encontrado: Juan Pérez (IdPadre: 1)
   ✅ [AlumnoRepo] Encontrados 2 alumnos para padre Juan
   ✅ [PADRE] IdsAlumnos cargados: [5, 6]
   ```

5. **Verifica los logs del navegador (Padre)**:
   ```javascript
   🔵 [PADRE] Configuración inicial:
	  - ID_BUS: 1
	  - IDS_ALUMNOS: [5, 6]  // ✅ Ya NO está vacío
	  - Grupos a suscribir: Bus_1, Alumno_5, Alumno_6
   ✅ [PadreDashboard] Unido a Bus_1
   ✅ [PadreDashboard] Unido a Alumno_5
   ✅ [PadreDashboard] Unido a Alumno_6
   ```

6. **Inicia sesión como Monitor** (en otro navegador)
   - Ve a: `/Monitor/MiRuta`
   - Click en "Simular Ruta en Tiempo Real"

7. **En la consola del Padre, debes ver**:
   ```javascript
   🔵 [PADRE] UbicacionBusActualizada recibida: {IdBus: 1, Latitud: 14.6235, ...}
   ```

8. **El mapa del Padre debe mostrar el bus moviéndose** 🎉

---

## 🔍 Logs de Diagnóstico Agregados

### En el Servidor (Output de Visual Studio):

- `🔵 [PADRE] Cargando dashboard para userId: ...`
- `🔵 [AlumnoRepo] Buscando padre con UsuarioId: ...`
- `✅ [AlumnoRepo] Padre encontrado: ...`
- `❌ [AlumnoRepo] No se encontró padre con UsuarioId: ...`
- `🔵 [UbicacionService] Enviando ubicación Bus X ...`
- `✅ [UbicacionService] Enviado a grupo Bus_X`
- `✅ [UbicacionService] Enviado a grupo Alumno_Y`

### En el Navegador del Padre (Consola F12):

- `🔵 [PADRE] Configuración inicial:`
- `✅ [PadreDashboard] Unido a Bus_X`
- `✅ [PadreDashboard] Unido a Alumno_Y`
- `🔵 [PADRE] UbicacionBusActualizada recibida: {...}`

---

## 📊 Flujo de Datos Correcto

```
Monitor (GPS/Simulación)
  ↓
POST /api/ubicaciones
  ↓
UbicacionBusService.RegistrarUbicacionYNotificarAsync()
  ↓
SignalR Broadcast:
  ├─→ Grupo "Bus_1" → Padre (suscrito a Bus_1) ✅
  └─→ Grupo "Alumno_5" → Padre (suscrito a Alumno_5) ✅

Padre.onUbicacionActualizada()
  ↓
actualizarMarcadorVivo()
  ↓
Mapa actualizado con ubicación en tiempo real 🗺️
```

---

## ❓ Preguntas Frecuentes

### ¿Por qué IDS_ALUMNOS estaba vacío?

**R:** Porque el usuario PadreDeFamilia no tenía un registro vinculado en `PadresDb`. El método `GetAlumnosByPadreUserIdAsync` busca:

1. Padre con `UsuarioId = userId`
2. Alumnos con `Padres.IdPadre = padre.IdPadre`

Si el padre no existe → retorna lista vacía → `IDS_ALUMNOS = []`

### ¿Cómo se vincula un usuario con un padre?

**R:** A través del campo `UsuarioId` en la tabla `PadresDb`:

```sql
UPDATE genesis.Padres 
SET UsuarioId = 'abc-123-def-456' 
WHERE IdPadre = 1;
```

### ¿Qué pasa si creo un usuario PadreDeFamilia nuevo?

**R:** Debes ejecutar la herramienta de diagnóstico para crear automáticamente el registro en `PadresDb`, o crearlo manualmente y vincular hijos.

---

## 🛠️ Archivos Creados

| Archivo | Propósito |
|---------|-----------|
| `Pages/Admin/DiagnosticoPadres.cshtml.cs` | Backend de la herramienta de diagnóstico web |
| `Pages/Admin/DiagnosticoPadres.cshtml` | Interfaz web para diagnóstico y corrección |
| `Scripts/DiagnosticoPadreUbicaciones.sql` | Script SQL de diagnóstico |
| `Scripts/CorreccionPadreUbicaciones.sql` | Script SQL de corrección automática |
| `GUIA_USO_DIAGNOSTICO_PADRES.md` | Este documento |

---

## ✅ Resumen

1. ✅ Herramienta web creada: `/Admin/DiagnosticoPadres`
2. ✅ Scripts SQL de diagnóstico y corrección
3. ✅ Logs de diagnóstico en servidor y navegador
4. ✅ Corrección automática con un click
5. ✅ Documentación completa

**Próximo paso**: Ve a `/Admin/DiagnosticoPadres` y haz click en "Aplicar Corrección Automática" 🚀
