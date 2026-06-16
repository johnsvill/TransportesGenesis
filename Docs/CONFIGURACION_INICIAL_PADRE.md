# ✨ Nueva Funcionalidad: Configuración Inicial del Padre

## 🎯 Descripción

Cuando un padre de familia inicia sesión por primera vez (después de cambiar su contraseña), el sistema lo redirige automáticamente a una página de **Configuración Inicial** donde debe proporcionar la dirección donde el bus debe recoger a su hijo(s).

---

## 🔄 Flujo Completo

### 1. **Registro Inicial** (Administrador)

```
Admin crea usuario para el padre:
├─→ Email: juan.perez@gmail.com
├─→ Contraseña temporal: Temp123
├─→ Rol: Padre
└─→ Se crea en AspNetUsers

Admin crea entidad Padre:
├─→ INSERT INTO genesis.Padres (Nombre, Apellido, UsuarioId)
└─→ VALUES ('Juan', 'Pérez', '[Id del usuario]')

Admin crea entidad Alumno:
├─→ INSERT INTO genesis.Alumnos (Nombre, Apellido, IdPadre, IdBusAsignado)
├─→ VALUES ('María', 'Pérez', [IdPadre], [IdBus])
└─→ Latitud/Longitud: NULL (aún no configuradas)
```

---

### 2. **Primer Login del Padre**

```
1. Juan abre la aplicación
   └─→ URL: /Auth/Login

2. Ingresa credenciales:
   ├─→ Email: juan.perez@gmail.com
   └─→ Password: Temp123

3. Sistema detecta: Primera vez o sin contraseña cambiada
   └─→ Redirige a: /Account/Manage/ChangePassword

4. Juan cambia su contraseña
   └─→ Nueva contraseña: MiPassword123

5. Sistema detecta: Alumno(s) sin coordenadas
   └─→ Redirige a: /Padre/ConfiguracionInicial
```

---

### 3. **Configuración Inicial** (Nueva Página)

#### URL: `/Padre/ConfiguracionInicial`

#### Interfaz:

```
┌────────────────────────────────────────────────────────┐
│  Bienvenido a Transportes Genesis                      │
├────────────────────────────────────────────────────────┤
│                                                         │
│  ℹ️  Configuración Inicial Requerida                   │
│                                                         │
│  Para poder usar el sistema de seguimiento de buses,   │
│  necesitamos saber dónde recoger a tu hijo(s).        │
│                                                         │
│  Por favor indica la dirección donde el bus debe       │
│  recoger al alumno.                                    │
│                                                         │
├────────────────────────────────────────────────────────┤
│                                                         │
│  👤 Configurando para: María Pérez                     │
│                                                         │
│  📍 Dirección de Recogida *                            │
│  [_________________________________] [🔍 Buscar Mapa]  │
│  ℹ️ Escribe tu dirección completa y haz clic en        │
│     "Buscar en Mapa"                                    │
│                                                         │
│  ✅ Ubicación encontrada:                              │
│  5ta Avenida 12-34 Zona 10, Guatemala                  │
│                                                         │
│  🗺️ Vista Previa de la Ubicación                      │
│  [           MAPA INTERACTIVO CON MARCADOR           ] │
│  ℹ️ Verifica que el marcador esté en la ubicación      │
│     correcta                                            │
│                                                         │
│  💡 Consejos:                                          │
│  • Sé lo más específico posible con la dirección       │
│  • Incluye zona, colonia o referencias conocidas       │
│  • Verifica en el mapa que la ubicación sea correcta   │
│  • Ejemplo: "6ta Avenida 9-50 Zona 9, Guatemala"       │
│                                                         │
│  [   Guardar y Continuar   ]  [ Cancelar ]            │
│                                                         │
└────────────────────────────────────────────────────────┘
```

---

### 4. **Proceso de Geocodificación**

```
Padre escribe: "5ta Avenida 12-34 Zona 10, Guatemala"
	   │
	   └─→ Hace clic en "🔍 Buscar en Mapa"
			  │
			  └─→ JavaScript ejecuta:
					│
					├─→ Llama a Nominatim API:
					│   https://nominatim.openstreetmap.org/search
					│   ?format=json
					│   &q=5ta+Avenida+12-34+Zona+10,+Guatemala
					│
					├─→ Recibe respuesta:
					│   {
					│     "lat": "14.590843",
					│     "lon": "-90.551780",
					│     "display_name": "5ta Avenida 12-34..."
					│   }
					│
					├─→ Actualiza campos hidden:
					│   Latitud: 14.590843
					│   Longitud: -90.551780
					│
					├─→ Muestra marcador verde en el mapa
					│
					├─→ Centra el mapa en la ubicación
					│
					└─→ Habilita botón "Guardar y Continuar"
```

---

### 5. **Guardar Configuración**

```
Padre hace clic en "Guardar y Continuar"
	   │
	   └─→ POST /Padre/ConfiguracionInicial
			  │
			  ├─→ Backend recibe:
			  │   - IdAlumno: 1
			  │   - Direccion: "5ta Avenida 12-34 Zona 10"
			  │   - Latitud: 14.590843
			  │   - Longitud: -90.551780
			  │
			  ├─→ Actualiza tabla Alumnos:
			  │   UPDATE genesis.Alumnos
			  │   SET Latitud = 14.590843,
			  │       Longitud = -90.551780
			  │   WHERE IdAlumno = 1;
			  │
			  ├─→ Crea Parada automáticamente:
			  │   INSERT INTO genesis.Paradas
			  │   (IdRuta, IdAlumno, Latitud, Longitud, Direccion, Orden)
			  │   VALUES
			  │   ([IdRuta], 1, 14.590843, -90.551780, '5ta Avenida...', [Orden])
			  │
			  ├─→ Marca configuración como completada:
			  │   - Agrega claim: "ConfiguracionInicial" = "Completada"
			  │
			  └─→ Redirige a: /Padre/Index (Dashboard del Padre)
```

---

### 6. **Dashboard del Padre** (Ahora Funcional)

```
/Padre/Index
├─→ Muestra mapa con:
│   ├─→ Ubicación actual del bus (marcador azul)
│   ├─→ Parada de su hijo (marcador verde)
│   └─→ ETA: "Bus llegará en 10 minutos"
│
├─→ Información del alumno:
│   ├─→ Nombre: María Pérez
│   ├─→ Bus asignado: BUS-001
│   ├─→ Dirección de recogida: 5ta Avenida 12-34 Zona 10
│   └─→ Estado: "Bus en camino"
│
└─→ Notificaciones en tiempo real:
	└─→ "🚌 El bus está a 5 minutos"
```

---

## 📁 Archivos Creados/Modificados

### Nuevos Archivos:

1. **`Pages/Padre/ConfiguracionInicial.cshtml`** (Vista Razor)
   - Formulario con geocodificación
   - Mapa interactivo
   - Validaciones

2. **`Pages/Padre/ConfiguracionInicial.cshtml.cs`** (PageModel)
   - Lógica de backend
   - Actualización de coordenadas
   - Creación automática de parada

3. **`Repositories/Interfaces/IAlumnoRepository.cs`** (Interface)
   - Contrato del repositorio de alumnos

4. **`Repositories/Implementations/AlumnoRepository.cs`** (Implementación)
   - Métodos para obtener alumnos por padre
   - CRUD completo de alumnos

5. **`Scripts/Add_UsuarioId_To_Padres.sql`** (Migración)
   - Script SQL para agregar campo UsuarioId

### Archivos Modificados:

1. **`Models/DB/Negocio/Padres.cs`**
   - Agregado campo: `public string? UsuarioId { get; set; }`

2. **`Startup.cs`**
   - Registrado `IAlumnoRepository`
   - Registrado `IParadaRepository`

---

## 🔧 Configuración Requerida

### Paso 1: Ejecutar Migración de BD

```sql
-- En SQL Server Management Studio
USE TransportesGenesisDB;
GO

ALTER TABLE genesis.Padres
ADD UsuarioId NVARCHAR(450) NULL;
GO

CREATE INDEX IX_Padres_UsuarioId ON genesis.Padres(UsuarioId);
GO
```

### Paso 2: Compilar el Proyecto

```bash
dotnet build
```

### Paso 3: Vincular Usuarios Existentes (Si ya tienes datos)

```sql
-- Para cada padre existente
UPDATE genesis.Padres
SET UsuarioId = (SELECT Id FROM AspNetUsers WHERE Email = 'padre@gmail.com')
WHERE Nombre = 'Nombre' AND Apellido = 'Apellido';
```

---

## 🎓 Lógica de Redirección

### ¿Cuándo se muestra la página de Configuración Inicial?

```csharp
// En el login o en _Layout.cshtml
var user = await _userManager.GetUserAsync(User);
var alumnos = await _alumnoRepository.GetAlumnosByPadreUserIdAsync(user.Id);

// Verificar si algún alumno NO tiene coordenadas
var necesitaConfiguracion = alumnos.Any(a => 
	!a.Latitud.HasValue || 
	!a.Longitud.HasValue || 
	a.Latitud == 0 || 
	a.Longitud == 0
);

if (necesitaConfiguracion)
{
	// Redirigir a configuración
	return RedirectToPage("/Padre/ConfiguracionInicial");
}
```

---

## ✅ Ventajas de Esta Implementación

### 1. **Experiencia de Usuario Mejorada**
- ✅ Proceso guiado paso a paso
- ✅ Validación en tiempo real
- ✅ Feedback visual inmediato

### 2. **Datos Más Precisos**
- ✅ Geocodificación automática
- ✅ Verificación visual en el mapa
- ✅ Menos errores de entrada manual

### 3. **Menos Carga para el Administrador**
- ✅ El padre proporciona su propia dirección
- ✅ Admin solo debe vincular padre con alumno
- ✅ Sistema crea parada automáticamente

### 4. **Escalabilidad**
- ✅ Funciona con múltiples hijos
- ✅ Padre configura cada alumno uno por uno
- ✅ Fácil de mantener

---

## 📊 Flujo Comparativo

### ANTES (Sin esta funcionalidad):

```
1. Admin pregunta dirección al padre por teléfono/email
2. Admin busca dirección en Google Maps manualmente
3. Admin copia lat/lng a Excel
4. Admin ingresa datos en el sistema
5. Posibles errores de transcripción
```

### AHORA (Con esta funcionalidad):

```
1. Padre inicia sesión
2. Sistema detecta que falta dirección
3. Padre escribe su dirección
4. Hace clic en "Buscar en Mapa"
5. Verifica visualmente
6. Guarda
7. ✅ Listo automáticamente
```

---

## 🧪 Cómo Probar

### Test Case 1: Padre Nuevo

```
1. Crear usuario padre en AspNetUsers
2. Vincular con entidad Padres (UsuarioId)
3. Crear alumno SIN coordenadas
4. Login como padre
5. Verificar redirección a /Padre/ConfiguracionInicial
6. Ingresar dirección
7. Geocodificar
8. Guardar
9. Verificar redirección a /Padre/Index
10. Verificar coordenadas en BD
```

### Test Case 2: Padre con Múltiples Hijos

```
1. Crear padre con 2 alumnos (ambos sin coordenadas)
2. Login como padre
3. Configurar dirección del primer hijo
4. Guardar
5. Sistema debe mostrar segundo hijo
6. Configurar dirección del segundo hijo
7. Guardar
8. Redirigir a dashboard
```

### Test Case 3: Dirección No Encontrada

```
1. Login como padre
2. Ingresar dirección vaga: "Zona 10"
3. Hacer clic en "Buscar en Mapa"
4. Verificar mensaje de error
5. Ingresar dirección más específica
6. Volver a buscar
7. Verificar que ahora sí funciona
```

---

## 📝 TODO / Mejoras Futuras

- [ ] Permitir al padre editar su dirección después
- [ ] Enviar notificación al admin cuando padre configure dirección
- [ ] Agregar opción de "Click en el Mapa" como alternativa
- [ ] Soporte para múltiples direcciones (casa de mamá/papá)
- [ ] Validación de que la dirección esté en Guatemala
- [ ] Historial de cambios de dirección

---

## ❓ FAQ

### ¿Qué pasa si el padre nunca configura la dirección?
**R**: No podrá usar el sistema de seguimiento. Cada vez que inicie sesión, será redirigido a la página de configuración.

### ¿El admin puede saltarse este paso?
**R**: No. Es obligatorio para que el sistema funcione correctamente.

### ¿Se puede cambiar la dirección después?
**R**: Actualmente no, pero se puede implementar fácilmente en el perfil del padre.

### ¿Qué pasa si la geocodificación falla?
**R**: El padre verá un mensaje de error y sugerencias para mejorar la búsqueda.

---

**Última Actualización**: Mayo 2026  
**Autor**: David  
**Estado**: ✅ Implementado y Listo para Probar
