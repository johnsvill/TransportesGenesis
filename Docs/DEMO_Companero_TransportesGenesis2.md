# Demo geolocalización — BD del compañero (`TransportesGenesis2`)

Guía para montar y probar la demo en la BD **`TransportesGenesis2`** (otro dev / otra máquina).

> **Simulación en vivo (3 ventanas):** ver **`SIMULACION_Companero.md`** — piloto1 + padre1 + admin BUS-001.

---

## Configuración

| Item | Valor |
|------|--------|
| Base de datos | `TransportesGenesis2` |
| Connection string | `appsettings.json` o `appsettings.Development.json` |
| URL app | `https://localhost:7241` (o la que use el perfil https) |

Ejemplo connection string:

```json
"TransportesGenesisConnection": "Server=(local);Database=TransportesGenesis2;Trusted_Connection=True;TrustServerCertificate=True"
```

Reiniciar la app después de cambiar la cadena.

---

## Orden de ejecución de scripts (importante)

Tu compañero tiene razón: **si corre los scripts en mal orden, la demo se rompe** aunque cada script “funcione” solo.

### Regla de oro

```
1. Migraciones + app (usuarios Identity)
2. seed_completo_demo_geolocalizacion.sql   ← alumnos, rutas, paradas
3. (Opcional) Seed_Escenarios_Completos.sql ← solo si después va a Admin/CalcularRutas
4. (Opcional) Admin → Calcular Rutas        ← obligatorio si corrió el paso 3
```

### Paso a paso

| # | Qué | Script / acción | Depende de |
|---|-----|-----------------|------------|
| **0** | Crear esquema BD | `dotnet ef database update` | — |
| **1** | Usuarios (`piloto1`, `admin`, `padre1`…) | Arrancar la app **una vez** | Paso 0 |
| **2** | Buses + piloto1 → BUS-001 | Script manual de buses/asignación (si no existen) | Paso 1 |
| **3** | Demo completa BUS-001 | `seed_completo_demo_geolocalizacion.sql` (`USE TransportesGenesis2`) | Pasos 1–2 |
| **3b** | Si falla `IdAlumno` | Fix alumnos + **solo sección paradas** del seed | Paso 3 |
| **4** | Asistencias 20 días | `Seed_Escenarios_Completos.sql` | **Alumnos con GPS ya creados (paso 3)** |
| **5** | Rutas dinámicas Admin | `/Admin/CalcularRutas` día hábil | **Solo si corrió paso 4** |

### Errores típicos por mal orden

| Lo que hizo | Qué pasa |
|-------------|----------|
| `Seed_Escenarios_Completos` **antes** de alumnos | 0 asistencias; Admin calc falla en todos los buses |
| `Seed_Escenarios_Completos` **después** del seed **sin** Admin calc | **`UPDATE Rutas SET EsActiva = 0`** deja Mi Ruta vacía |
| Admin calc **antes** de alumnos | Filas rojas “no hay alumnos” (normal en buses vacíos) |
| Seed completo **varias veces** a medias | Duplicados (21 alumnos, paradas huérfanas) |
| Script de buses **después** del seed | Puede chocar asignaciones (`IX_AsignacionPilotoBus_IdBus`) |

### Camino mínimo recomendado (solo Mi Ruta + simulación)

Si no necesita recalcular desde Admin:

1. Pasos **0 → 1 → 2 → 3** (con fix `IdAlumno` si hace falta)  
2. **NO** ejecutar `Seed_Escenarios_Completos.sql`  
3. Probar `/Piloto/MiRuta?turno=Mañana` con **piloto1**

El seed completo ya deja rutas y paradas activas.

### Camino con Admin/CalcularRutas

1. Pasos **0 → 1 → 2 → 3**  
2. `Seed_Escenarios_Completos.sql` (desactiva rutas viejas a propósito)  
3. Admin → Calcular Rutas → fecha hábil → turno Mañana (BUS-001 debe salir **Exitoso**)  
4. Mi Ruta / Selenium

---

| Usuario | Bus | Placa | Rol |
|---------|-----|-------|-----|
| **piloto1** | **4** | **BUS-001** | Ver ruta + simular |
| admin | — | — | Calcular rutas |
| padre1 | — | — | (Opcional) ver mapa del bus del hijo |

**Contraseña:** `Admin123!`

---

## Paso 1 — Base y usuarios

```powershell
dotnet ef database update
dotnet run
```

Arrancar **una vez** para crear usuarios Identity. Luego cerrar.

---

## Paso 2 — Buses + asignar piloto1 a BUS-001

Si aún no tiene buses ni asignación, insertar buses y:

```sql
USE TransportesGenesis2;

-- Verificar piloto1 → BUS-001
SELECT u.UserName, apb.IdBus, b.Placa, apb.EsActual
FROM genesis.AsignacionPilotoBus apb
JOIN dbo.AspNetUsers u ON u.Id = apb.IdUsuarioPiloto
JOIN genesis.Buses b ON b.IdBus = apb.IdBus
WHERE u.UserName = N'piloto1' AND apb.EsActual = 1;
-- Debe mostrar IdBus = 4, Placa = BUS-001
```

Si no hay fila, asignar (ajustar `@IdUsuario` si hace falta):

```sql
DECLARE @IdUsuario NVARCHAR(450) = (SELECT Id FROM dbo.AspNetUsers WHERE UserName = N'piloto1');
DECLARE @IdBus INT = (SELECT IdBus FROM genesis.Buses WHERE Placa = N'BUS-001');

IF NOT EXISTS (SELECT 1 FROM genesis.AsignacionPilotoBus WHERE IdBus = @IdBus AND EsActual = 1)
    INSERT INTO genesis.AsignacionPilotoBus (IdUsuarioPiloto, IdBus, FechaAsignacion, EsActual, Activo, FechaRegistro)
    VALUES (@IdUsuario, @IdBus, GETDATE(), 1, 1, GETDATE());
```

---

## Paso 3 — Seed demo BUS-001

1. Abrir `Scripts/seed_completo_demo_geolocalizacion.sql`
2. Cambiar línea 17: `USE TransportesGenesis2;`
3. Ejecutar completo (F5)

### Si falla `IdAlumno` NULL

En BD nueva, `genesis.Alumnos.IdAlumno` **no autoincrementa**. El seed falla al insertar alumnos.

**Síntoma:**

```
Msg 515: Cannot insert the value NULL into column 'IdAlumno'
```

**Fix:** ejecutar el bloque de alumnos con `IdAlumno` explícito (ver conversación / sección 5 del seed adaptada). Luego **volver a ejecutar solo la sección 7 (Paradas)** del mismo script (desde `DELETE FROM genesis.Paradas` hasta `PRINT '✓ Paradas...'`).

### Si falla asignación duplicada en bus 4

```
Msg 2601: IX_AsignacionPilotoBus_IdBus — duplicate key (4)
```

**Ignorar.** Significa que piloto1 ya está en BUS-001. Es correcto.

---

## Paso 4 — (Opcional) Calcular rutas desde Admin

1. Login: `admin` / `Admin123!`
2. `/Admin/CalcularRutas`
3. Fecha: **día hábil** (lunes–viernes, ej. mañana)
4. Turno: **Mañana** → Calcular

### Resultado esperado

| Bus | Estado | Significado |
|-----|--------|-------------|
| **BUS-001** | **Exitoso** | Listo — tiene alumnos |
| P-001GT, BUS-002, etc. | Fallo | **Normal** — buses vacíos, sin alumnos |

**No es error de la app.** Solo importa la fila verde de **BUS-001**.

Para no ver filas rojas (opcional):

```sql
USE TransportesGenesis2;

-- Tabla: genesis.Buses | Columna: Estado (bit)
UPDATE genesis.Buses SET Estado = 0 WHERE Placa <> N'BUS-001';
UPDATE genesis.Buses SET Estado = 1 WHERE Placa = N'BUS-001';
```

`CalcularRutas` usa `Estado = 1`, no la columna `Activo`.

---

## Paso 5 — (Opcional) Asistencias

Solo si quieres recalcular rutas varios días:

```
Scripts/Seed_Escenarios_Completos.sql
```

Cambiar `USE TransportesGenesis2;`. **Requiere alumnos ya creados** con `IdBusAsignado` + GPS.

---

## Paso 6 — Probar Mi Ruta

1. Login: **`piloto1`** / `Admin123!`
2. Ir a: **`/Piloto/MiRuta?turno=Mañana`**
3. Debe verse mapa + lista de paradas

Domingo: puede aparecer mensaje de fin de semana; con ruta activa en BD aún puede cargar. El lunes debe verse con normalidad.

---

## Verificación SQL

```sql
USE TransportesGenesis2;

SELECT COUNT(*) AS AlumnosBus4
FROM genesis.Alumnos WHERE IdBusAsignado = 4 AND Activo = 1;
-- Ideal: 10. Aceptable: > 0 (ej. 21 si hubo duplicados)

SELECT COUNT(*) AS ParadasConAlumno
FROM genesis.Paradas p
JOIN genesis.Rutas r ON r.IdRuta = p.IdRuta
WHERE r.IdBus = 4 AND r.EsActiva = 1 AND p.IdAlumno IS NOT NULL;
-- Ideal: ~20. Aceptable: >= 5

SELECT u.UserName, apb.IdBus, b.Placa
FROM genesis.AsignacionPilotoBus apb
JOIN dbo.AspNetUsers u ON u.Id = apb.IdUsuarioPiloto
JOIN genesis.Buses b ON b.IdBus = apb.IdBus
WHERE apb.EsActual = 1;
```

---

## Checklist “ya funciona”

- [ ] Connection string apunta a `TransportesGenesis2`
- [ ] `piloto1` asignado a BUS-001 (IdBus 4)
- [ ] Alumnos en bus 4 con GPS (`AlumnosBus4` > 0)
- [ ] BUS-001 **Exitoso** en Admin **o** rutas/paradas del seed
- [ ] `/Piloto/MiRuta?turno=Mañana` muestra paradas

---

## Diferencias vs BD de David

| Tema | David (`TransportesGenesis`) | Compañero (`TransportesGenesis2`) |
|------|------------------------------|-----------------------------------|
| Simulación en vivo con padre1 | **monitor1** → bus 4 | **piloto1** → bus 4 (más simple) |
| piloto1 | Bus 1 (P-001GT) | Bus 4 (BUS-001) |
| Alumnos | Histórico / muchos registros | Seed reciente; puede haber duplicados |
| Admin calcular | Varios buses en rojo es normal | Igual |

---

## Problemas frecuentes

| Síntoma | Causa | Qué hacer |
|---------|--------|-----------|
| No tienes bus asignado | Falta fila en `AsignacionPilotoBus` | Paso 2 |
| Calcular rutas todo rojo | Sin alumnos en ningún bus | Paso 3 (fix IdAlumno) |
| BUS-001 verde, resto rojo | Normal | Ignorar otros buses |
| Mi Ruta vacía | Sin rutas activas o sin paradas | Seed sección 6–7 o Admin calc |
| Error `Turno` en traslados | Columna faltante | `ALTER TABLE genesis.SolicitudTraslado ADD Turno NVARCHAR(20) NOT NULL DEFAULT 'Ambos'` |

---

## Scripts de referencia

| Script | Uso |
|--------|-----|
| `Scripts/seed_completo_demo_geolocalizacion.sql` | Demo BUS-001 (cambiar `USE`) |
| `Scripts/Seed_Escenarios_Completos.sql` | Asistencias 20 días hábiles |
| `Scripts/VERIFICAR_Demo_Padre_Piloto.sql` | Diagnóstico padre–piloto–bus |
