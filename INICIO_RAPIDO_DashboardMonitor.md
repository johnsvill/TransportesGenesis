# 🚀 INICIO RÁPIDO - Datos de Prueba Dashboard Monitor

## ⏱️ Opción Rápida (2 minutos)

### **Si tienes SQL Server conectado directamente:**

1. Abre `Scripts/SeedData_DashboardMonitor_Test.sql`
2. Ejecuta en SSMS o Azure Data Studio (F5)
3. ✅ Listo!

---

### **Si NO tienes SQL Server conectado (usas EF Core):**

1. Abre terminal en Visual Studio
2. Ejecuta:
   ```powershell
   dotnet ef database update
   ```
3. ✅ Listo!

---

## 📋 ¿Qué datos se insertarán?

- **1 Bus**: BUS-001 (Placa)
- **8 Padres**: Apellidos Martínez, García, Rodríguez, López, Hernández, Gómez, Díaz, Torres
- **10 Alumnos**: Diego, Sofía, Mateo, Isabella, Santiago, Valentina, Sebastián, Camila, Matías, Lucía
- **2 Rutas**: Mañana (6:00 AM) y Tarde (2:00 PM)
- **22 Paradas**: Desde Zona 11 hasta Zona 1

---

## 🎮 Ver los datos en la aplicación

1. Ejecuta la app (F5)
2. Ve a **Dashboard Monitor** o **Registrar Asistencia**
3. Deberías ver **10 alumnos** listados

---

## 🧹 Limpiar datos de prueba

### **Si usaste el Script SQL:**
```sql
DELETE FROM genesis.Paradas WHERE IdParada BETWEEN 100 AND 200;
DELETE FROM genesis.Rutas WHERE IdRuta BETWEEN 10 AND 12;
DELETE FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220;
DELETE FROM genesis.Padres WHERE IdPadre BETWEEN 100 AND 110;
```

### **Si usaste la Migración EF:**
```powershell
dotnet ef database update 20260505170000_CreateAlertaProximidadTable
```

---

## 📚 Documentación Completa

- **Scripts SQL**: `Scripts/README_DashboardMonitor.md`
- **Migración EF**: `Migrations/README_SeedData_Migration.md`
- **Configuración**: `Scripts/SeedData_DashboardMonitor_Configuracion.sql`

---

## ❓ Preguntas Frecuentes

**P: ¿Afectará mis datos existentes?**  
R: ❌ No. Los IDs están en rangos específicos (100-220) que no interfieren.

**P: ¿Puedo ejecutarlo varias veces?**  
R: ✅ Sí. El script es idempotente (elimina y reinserta).

**P: ¿Funciona para la presentación?**  
R: ✅ Sí. Fue diseñado específicamente para eso.

---

## 🆘 Problemas Comunes

### "No hay alumnos en la ruta"
```sql
-- Verifica que el bus existe
SELECT * FROM genesis.Buses WHERE IdBus = 4;

-- Activa las rutas
UPDATE genesis.Rutas SET EsActiva = 1 WHERE IdBus = 4;
```

### "Cannot insert duplicate key"
```sql
-- Limpia primero los datos anteriores
DELETE FROM genesis.Paradas WHERE IdParada BETWEEN 100 AND 200;
DELETE FROM genesis.Rutas WHERE IdRuta BETWEEN 10 AND 12;
DELETE FROM genesis.Alumnos WHERE IdAlumno BETWEEN 200 AND 220;
DELETE FROM genesis.Padres WHERE IdPadre BETWEEN 100 AND 110;
```

---

**✅ ¡Todo listo para la presentación!** 🎉
