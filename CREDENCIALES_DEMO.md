# 🔑 CREDENCIALES DE USUARIOS PARA DEMO
## Sistema TransportesGenesis - Presentación Martes

---

## 📋 LISTA RÁPIDA DE USUARIOS

### 1️⃣ ADMINISTRADOR
```
👤 Usuario:    admin@genesis.com
🔒 Contraseña: Admin123!
📝 Rol:        Administrador
```
**Acceso a:**
- Panel de administración completo
- Gestión de usuarios
- Gestión de buses y rutas
- Gestión de alumnos
- Reportes y dashboards
- Planner del proyecto
- Alertas e historial
- Todas las funcionalidades del sistema

---

### 2️⃣ PILOTO
```
👤 Usuario:    piloto@genesis.com
🔒 Contraseña: Piloto123!
📝 Rol:        Piloto
```
**Acceso a:**
- Dashboard de piloto
- Vista de mi ruta asignada
- Mapa en tiempo real con paradas
- Listado de alumnos a recoger
- Actualización de ubicación en tiempo real
- Notificaciones de proximidad

---

### 3️⃣ MONITOR
```
👤 Usuario:    monitor@genesis.com
🔒 Contraseña: Monitor123!
📝 Rol:        Monitor
```
**Acceso a:**
- Dashboard de monitor
- Vista de ruta activa
- Registro de asistencia de alumnos
- Listado de alumnos del bus
- Mapa interactivo de paradas
- Control de presente/ausente

---

### 4️⃣ PADRE DE FAMILIA
```
👤 Usuario:    padre@genesis.com
🔒 Contraseña: Padre123!
📝 Rol:        PadreDeFamilia
```
**Acceso a:**
- Dashboard de padre
- Seguimiento de ubicación del bus
- Estado de asistencia de hijos
- Configuración de punto de recogida
- Notificaciones de proximidad
- Solicitudes de traslado
- Historial de asistencias

---

## 🚀 CÓMO EJECUTAR EL SCRIPT

### Opción 1: SQL Server Management Studio (SSMS)
1. Abre **SQL Server Management Studio**
2. Conéctate a tu servidor SQL
3. Abre el archivo: `Scripts/SeedData_Usuarios_Demo.sql`
4. Selecciona la base de datos: **TransportesGenesis**
5. Presiona **F5** o clic en "Ejecutar"
6. Verifica el mensaje de éxito en la salida

### Opción 2: Visual Studio
1. Abre **SQL Server Object Explorer** (View → SQL Server Object Explorer)
2. Conecta a tu base de datos local
3. Click derecho en **TransportesGenesis** → New Query
4. Pega el contenido del script
5. Click en "Ejecutar" (ícono verde ▶️)

### Opción 3: Package Manager Console
```powershell
# Desde Visual Studio Package Manager Console
Invoke-Sqlcmd -ServerInstance "(localdb)\MSSQLLocalDB" -Database "TransportesGenesis" -InputFile "Scripts/SeedData_Usuarios_Demo.sql"
```

---

## ✅ VERIFICACIÓN POST-INSTALACIÓN

### Verificar que los usuarios existen:
```sql
SELECT 
	u.Email,
	r.Name AS Rol,
	u.EmailConfirmed
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email IN (
	'admin@genesis.com',
	'piloto@genesis.com',
	'monitor@genesis.com',
	'padre@genesis.com'
)
```

Deberías ver 4 usuarios con sus roles correspondientes.

---

## 🎯 FLUJO DE LA PRESENTACIÓN SUGERIDO

### 1. Login como Administrador (2 minutos)
- Mostrar panel de administración
- Mostrar dashboard analítico con gráficos
- Mostrar reportes (Rutas, Asistencias, Alertas)
- Exportar un reporte a Excel
- Mostrar gestión de alumnos/buses

### 2. Login como Piloto (2 minutos)
- Mostrar dashboard de piloto
- Mostrar "Mi Ruta" con mapa
- Explicar paradas y alumnos a recoger
- Mostrar cómo funciona el tracking en tiempo real

### 3. Login como Monitor (2 minutos)
- Mostrar dashboard de monitor
- Mostrar listado de alumnos
- Demostrar registro de asistencia (toggle presente/ausente)
- Mostrar mapa de la ruta

### 4. Login como Padre (2 minutos)
- Mostrar seguimiento del bus en tiempo real
- Mostrar asistencias de hijos
- Mostrar notificaciones de proximidad
- Explicar configuración de punto de recogida

---

## 📱 TIPS PARA LA PRESENTACIÓN

### Preparación Previa:
✅ Ejecutar el script de usuarios 1 día antes
✅ Verificar que todos los usuarios funcionen
✅ Tener buses y rutas de ejemplo creadas
✅ Tener datos de asistencias de ejemplo
✅ Probar el login de cada usuario antes de la demo

### Durante la Presentación:
✅ Tener este documento abierto en otra pantalla
✅ Usar copiar/pegar para las credenciales
✅ Tener pestañas del navegador preparadas para cambio rápido
✅ Usar modo incógnito o diferentes perfiles de navegador por rol

### Solución Rápida de Problemas:
- **Si un usuario no puede loguearse:** Verificar que EmailConfirmed = 1
- **Si no se ve el rol:** Verificar tabla AspNetUserRoles
- **Si falta data:** Usar botón "Generar Datos de Ejemplo" en reportes

---

## 🔐 SEGURIDAD POST-DEMO

**⚠️ IMPORTANTE:** Después de la presentación, considera:

1. **Cambiar las contraseñas** de estos usuarios
2. **Eliminar usuarios de prueba** si no son necesarios
3. **Crear usuarios con contraseñas seguras** para producción

```sql
-- Script para eliminar usuarios demo (después de la presentación)
DELETE FROM AspNetUserRoles WHERE UserId IN (
	SELECT Id FROM AspNetUsers WHERE Email IN (
		'admin@genesis.com', 'piloto@genesis.com', 
		'monitor@genesis.com', 'padre@genesis.com'
	)
)

DELETE FROM AspNetUsers WHERE Email IN (
	'admin@genesis.com', 'piloto@genesis.com', 
	'monitor@genesis.com', 'padre@genesis.com'
)
```

---

## 📞 CONTACTO Y SOPORTE

Si algo no funciona durante la preparación:
- Verificar que la base de datos esté actualizada
- Ejecutar migraciones pendientes
- Revisar que los roles existan en AspNetRoles
- Verificar conexión a la base de datos

---

## ✨ ¡ÉXITO EN TU PRESENTACIÓN!

**Recuerda:**
- Respirar profundo antes de empezar 😊
- Hablar claro y despacio
- Mostrar confianza en tu trabajo
- ¡Has hecho un excelente trabajo con este sistema!

🎉 **¡Mucha suerte el martes!** 🎉
