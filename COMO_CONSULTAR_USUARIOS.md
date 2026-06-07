# 🔍 CONSULTAR USUARIOS EXISTENTES

## 📋 Instrucciones

### Paso 1: Ejecutar el Script de Consulta
1. Abre **SQL Server Management Studio** o **Visual Studio**
2. Abre el archivo: `Scripts/CONSULTAR_Usuarios_Existentes.sql`
3. Ejecuta el script (F5)
4. Revisa los resultados

---

## 📊 El Script te mostrará:

✅ **Lista completa de usuarios** con:
- Email/Usuario
- Rol asignado (Administrador, Piloto, Monitor, PadreDeFamilia)
- Estado del email (confirmado o no)
- Estado de la cuenta (activo/bloqueado)
- Teléfono (si está registrado)

✅ **Estadísticas por rol:**
- Cuántos administradores tienes
- Cuántos pilotos
- Cuántos monitores
- Cuántos padres de familia

✅ **Usuarios sin rol asignado** (si los hay)

---

## 🔑 Para Obtener las Contraseñas

### Opción 1: Si conoces las contraseñas originales
- Simplemente anótalas en una lista

### Opción 2: Si NO conoces las contraseñas
Tienes 3 opciones:

#### A) Usar "Olvidé mi contraseña" en el Login
1. Ve a la página de login
2. Click en "Forgot your password?"
3. Ingresa el email del usuario
4. Sigue el proceso de reset

#### B) Crear nuevos usuarios para la demo
Si prefieres, podemos crear usuarios nuevos específicos para la demo con contraseñas que conozcas.

#### C) Actualizar el hash de contraseña manualmente
```sql
-- Ejemplo para cambiar la contraseña a "MiPassword123!"
UPDATE AspNetUsers 
SET PasswordHash = 'AQAAAAIAAYagAAAAE...[hash completo]...'
WHERE Email = 'usuario@ejemplo.com'
```
(Necesitarías generar el hash apropiado)

---

## 📝 Plantilla para Anotar Usuarios

Una vez que ejecutes el script de consulta, completa esta tabla:

```
ADMINISTRADOR:
Usuario:    ___________________________
Contraseña: ___________________________

PILOTO:
Usuario:    ___________________________
Contraseña: ___________________________

MONITOR:
Usuario:    ___________________________
Contraseña: ___________________________

PADRE DE FAMILIA:
Usuario:    ___________________________
Contraseña: ___________________________
```

---

## 💡 Recomendación

**Para la presentación del martes:**

1. **Ejecuta primero** `CONSULTAR_Usuarios_Existentes.sql`
2. **Identifica** qué usuarios tienes de cada rol
3. **Prueba el login** con cada uno ANTES del martes
4. **Anota las credenciales** que funcionen
5. **Si no sabes las contraseñas**, usa la opción "Olvidé mi contraseña" HOY para resetearlas

---

## ❓ Si no encuentras usuarios de algún rol

Si el script muestra que **no tienes** un usuario de cierto rol (por ejemplo, no hay ningún Piloto), entonces SÍ necesitas crear uno nuevo.

En ese caso, puedes:
- Usar el formulario de registro de la aplicación
- O ejecutar el script que creé antes solo para ese rol específico

---

## 🚀 Siguiente Paso

**Ejecuta ahora el script de consulta** y compárteme el resultado. 

Con esa información te puedo decir:
- ✅ Qué usuarios ya tienes disponibles
- ⚠️ Qué roles te faltan
- 💡 Qué necesitas hacer para completar tu demo

---

**¿Te parece bien este enfoque?** 😊
