# 🔄 Migración de Entity Framework - Datos de Prueba

## 📌 Información de la Migración

- **Nombre:** `SeedData_DashboardMonitor`
- **Timestamp:** `20260512200000`
- **Ubicación:** `Migrations/20260512200000_SeedData_DashboardMonitor.cs`

---

## 🎯 ¿Qué hace esta migración?

Esta migración **NO modifica el esquema** de la base de datos. Solo **inserta datos de prueba** para el Dashboard Monitor:

✅ **1 Bus** (BUS-001, IdBus = 4)  
✅ **8 Padres** de familia (IDs 100-107)  
✅ **10 Alumnos** asignados al bus (IDs 200-209)  
✅ **2 Rutas** (Mañana y Tarde, IDs 10-11)  
✅ **22 Paradas** distribuidas geográficamente (IDs 100-130)

---

## 🚀 Cómo Aplicar la Migración

### **Opción 1: Desde Visual Studio**

1. Abre **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console)
2. Ejecuta:
   ```powershell
   Update-Database
   ```

### **Opción 2: Desde Terminal/PowerShell**

1. Abre una terminal en la carpeta del proyecto
2. Ejecuta:
   ```powershell
   dotnet ef database update
   ```

### **Opción 3: Aplicar desde otro proyecto/carpeta**

```powershell
dotnet ef database update --project "C:\Proyectos\TransportesGenesis\TransportesGenesis.csproj"
```

---

## 🔄 Cómo Revertir la Migración

Si necesitas **eliminar los datos de prueba** insertados por esta migración:

### **Método 1: Revertir a la migración anterior**

```powershell
# Revertir a la última migración antes de esta
dotnet ef database update 20260505170000_CreateAlertaProximidadTable
```

### **Método 2: Ejecutar SQL directamente**

```sql
DELETE FROM genesis.Paradas WHERE IdParada BETWEEN 100 AND 200;
DELETE FROM genesis.Rutas WHERE IdRuta BETWEEN 10 AND 12;
DELETE FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220;
DELETE FROM genesis.Padres WHERE IdPadre BETWEEN 100 AND 110;
-- Opcional: eliminar el bus
-- DELETE FROM genesis.Buses WHERE IdBus = 4;
```

---

## 📋 Verificar Migraciones Aplicadas

Para ver todas las migraciones aplicadas en la base de datos:

```powershell
dotnet ef migrations list
```

Deberías ver algo como:

```
20241029172435_Scaffold_de_usuarios (Applied)
20241030190844_Create_TipoRecorridoPago_Banco_Padres_Alumnos_TipoCuentaEntity (Applied)
20241030214842_Create_Pago_Entity (Applied)
20241105000000_Add_Sistema_Geolocalizacion_Completo (Applied)
...
20260512200000_SeedData_DashboardMonitor (Applied) ✅
```

---

## 🔍 Consultar el Historial de Migraciones en la BD

```sql
SELECT * FROM __EFMigrationsHistory 
ORDER BY MigrationId DESC;
```

Deberías ver la migración `20260512200000_SeedData_DashboardMonitor` en la tabla.

---

## ⚠️ Notas Importantes

### **1. Esta migración es idempotente**
Si la ejecutas múltiples veces, no generará errores porque:
- **Verifica si el bus ya existe** antes de insertarlo
- **Elimina los datos anteriores** antes de insertar nuevos (rangos de IDs específicos)

### **2. No afecta datos existentes**
Los datos insertados usan **rangos de IDs específicos**:
- Padres: 100-110
- Alumnos: 200-220
- Rutas: 10-12
- Paradas: 100-200

Esto asegura que **no interfiera con datos reales** de tu base de datos.

### **3. Solo para desarrollo/presentaciones**
Esta migración está diseñada para:
- ✅ Desarrollo local
- ✅ Presentaciones y demos
- ✅ Pruebas de integración
- ❌ **NO** para producción

---

## 🆚 Diferencias con el Script SQL

| Aspecto | Migración EF | Script SQL |
|---------|-------------|------------|
| **Ejecución** | `dotnet ef database update` | SSMS/Azure Data Studio |
| **Reversión** | `dotnet ef database update [anterior]` | SQL manual |
| **Historial** | Registrado en `__EFMigrationsHistory` | No registrado |
| **Control de versiones** | Automático (Git) | Manual |
| **Requiere SQL Server directo** | ❌ No | ✅ Sí |
| **Integración con CI/CD** | ✅ Fácil | ⚠️ Complejo |

---

## 🐛 Solución de Problemas

### **Error: "A network-related or instance-specific error occurred"**
**Causa:** No hay conexión a la base de datos.

**Solución:** Verifica tu connection string en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TransportesGenesis;..."
  }
}
```

### **Error: "The migration '20260512200000_SeedData_DashboardMonitor' has already been applied"**
**Causa:** La migración ya fue aplicada.

**Solución:** No es un error. Los datos ya están en la BD. Si quieres volver a insertarlos:
1. Revierte la migración: `dotnet ef database update 20260505170000_CreateAlertaProximidadTable`
2. Vuelve a aplicarla: `dotnet ef database update`

### **Error: "Cannot insert duplicate key"**
**Causa:** Ya existen datos con los mismos IDs.

**Solución:** Ejecuta el SQL de limpieza primero:
```sql
DELETE FROM genesis.Paradas WHERE IdParada BETWEEN 100 AND 200;
DELETE FROM genesis.Rutas WHERE IdRuta BETWEEN 10 AND 12;
DELETE FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220;
DELETE FROM genesis.Padres WHERE IdPadre BETWEEN 100 AND 110;
```

Luego vuelve a aplicar la migración.

---

## 📞 Preguntas Frecuentes

### **¿Puedo aplicar esta migración en producción?**
❌ **No recomendado.** Esta migración es solo para datos de prueba.

### **¿Afectará mis datos existentes?**
❌ **No.** Los IDs están en rangos específicos que no interfieren con datos reales.

### **¿Puedo modificar los datos insertados?**
✅ **Sí.** Edita el archivo `Migrations/20260512200000_SeedData_DashboardMonitor.cs` antes de aplicarla.

### **¿Qué pasa si hago merge con mi compañero?**
✅ **Compatible.** Ambos pueden aplicar la misma migración sin conflictos. EF Core controla qué migraciones ya fueron aplicadas.

---

## ✅ Checklist de Verificación

Después de aplicar la migración, verifica:

- [ ] La migración aparece como "Applied" en `dotnet ef migrations list`
- [ ] Existe 1 bus con placa "BUS-001" (IdBus = 4)
- [ ] Existen 8 padres con IDs entre 100-107
- [ ] Existen 10 alumnos con IDs entre 200-209
- [ ] Existen 2 rutas (Mañana y Tarde) para el bus 4
- [ ] Existen 22 paradas distribuidas en ambas rutas
- [ ] El Dashboard Monitor muestra los 10 alumnos

---

**¡Listo para usar!** 🚀
