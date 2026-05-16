# Scripts SQL - Solo para Testing Local

⚠️ **IMPORTANTE:** Esta carpeta contiene scripts SQL auxiliares para desarrollo y testing local.

## Propósito
- **Testing manual**: Scripts para insertar datos de prueba rápidamente durante el desarrollo
- **Debugging**: Scripts para corregir problemas específicos durante el desarrollo
- **Demostración**: Scripts para preparar presentaciones con datos consistentes

## ❌ NO son Migraciones Oficiales
- Estos scripts **NO** forman parte del pipeline de migraciones de Entity Framework
- **NO** se ejecutan automáticamente al iniciar la aplicación
- **NO** deben usarse en producción

## ✅ Migraciones Oficiales
Las migraciones oficiales del proyecto están en la carpeta raíz **`/Migrations`** y se aplican automáticamente con:
```bash
dotnet ef database update
```

## Uso Recomendado
1. **Para desarrollo local**: Puedes ejecutar estos scripts manualmente si necesitas datos de prueba rápidos
2. **Para producción**: Usa solo las migraciones oficiales de Entity Framework en `/Migrations`
3. **Para colaboración**: Estos scripts son opcionales y no afectan el trabajo de otros desarrolladores

## Estructura de Carpetas del Proyecto
```
TransportesGenesis/
├── Migrations/              ← Migraciones oficiales EF Core (USAR ESTOS)
│   ├── 20241105000000_Add_Sistema_Geolocalizacion_Completo.cs
│   ├── 20260426174720_AddPagosPadres.cs
│   └── ApplicationDbContextModelSnapshot.cs
│
└── Scripts/                 ← Scripts auxiliares de desarrollo (OPCIONAL)
    ├── SeedData_*.sql       ← Datos de prueba para testing manual
    └── Fix_*.sql            ← Scripts de corrección para debugging
```

---

**Última actualización**: Mayo 2026  
**Mantenido por**: Equipo TransportesGenesis (Jonathan & David)
