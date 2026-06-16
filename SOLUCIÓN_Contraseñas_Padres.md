# 🔐 SOLUCIÓN RÁPIDA: Contraseñas de Padres

## 🎯 Problema:
No sabes si `padre@test.com` y `padre1@gmail.com` tienen la contraseña `Admin123!`

---

## ✅ SOLUCIÓN EN 2 PASOS:

### Paso 1: VERIFICAR (opcional - solo si tienes curiosidad)
```sql
-- Ejecuta este script para ver si tienen Admin123!
Scripts/VERIFICAR_Contraseña_Padres.sql
```
Este script te dirá si tienen la misma contraseña que el admin o no.

---

### Paso 2: IGUALAR (recomendado - hazlo directamente)
```sql
-- Ejecuta este script para que TODOS tengan Admin123!
Scripts/IGUALAR_Contraseña_Padres.sql
```

Este script:
- ✅ Copia el hash de Admin123! a los dos padres
- ✅ Confirma sus emails automáticamente
- ✅ Desbloquea las cuentas
- ✅ Los deja listos para login

---

## 🚀 RECOMENDACIÓN RÁPIDA:

### Si tienes prisa (para tu demo del martes):

**Ve directo al Paso 2:**
1. Abre `Scripts/IGUALAR_Contraseña_Padres.sql`
2. Ejecuta (F5)
3. ¡Listo! Ambos padres ahora tienen `Admin123!`

---

## 📋 Después de ejecutar el script:

Intenta loguearte con:

```
Usuario: padre@test.com
Contraseña: Admin123!

Usuario: padre1@gmail.com
Contraseña: Admin123!
```

Si alguno no funciona, ejecuta el script de nuevo.

---

## 💡 ¿Por qué funciona esto?

ASP.NET Identity guarda las contraseñas como "hashes" (valores encriptados).
El mismo texto siempre produce el mismo hash.

Entonces:
- Si copiamos el hash del admin (que sabemos es Admin123!)
- A los padres
- Los padres también tendrán Admin123!

---

## ⚠️ IMPORTANTE:

Este script solo funciona si:
- ✅ Tienes un usuario Administrador
- ✅ El administrador tiene la contraseña Admin123!
- ✅ Los usuarios padre@test.com y padre1@gmail.com existen

Si algo falla, el script te dirá qué está mal.

---

## 🎉 DESPUÉS DE ESTO:

Tendrás **TODOS** tus usuarios con la misma contraseña:
- Admin → Admin123!
- Piloto → Admin123!
- Monitor → Admin123!
- **Padre@test.com → Admin123!** ✅
- **Padre1@gmail.com → Admin123!** ✅

¡Perfecto para tu demo! 🚀
