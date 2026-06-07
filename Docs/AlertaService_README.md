# Módulo 2: Servicio de Alertas - TransportesGenesis

## Descripción
Este módulo implementa el servicio de negocio para gestionar alertas de proximidad y retrasos, proporcionando una capa de abstracción entre los controladores y el acceso a datos.

## Archivos Creados

### 1. Interfaces de Servicio
**Ubicación:** `Services/Interfaces/IAlertaService.cs`

Métodos principales:
- `RegistrarAlertaAsync()`: Registra nueva alerta
- `GetHistorialAlertasPorBusAsync()`: Obtiene historial por bus
- `GetHistorialAlertasPorAlumnoAsync()`: Obtiene historial por alumno
- `GetAlertasActivasPorPadreAsync()`: Obtiene alertas activas por padre
- `MarcarAlertaComoResueltaAsync()`: Marca alerta como resuelta
- `ConfirmarRecepcionPadreAsync()`: Confirma recepción por padre
- `LimpiarAlertasAntiguasAsync()`: Limpieza automática de alertas

### 2. Implementación del Servicio
**Ubicación:** `Services/Implementations/AlertaService.cs`

Características:
- Implementa inyección de dependencias
- Usa AutoMapper para conversiones DTO/Entidad
- Incluye validaciones de negocio
- Enriquece DTOs con información relacionada
- Manejo de errores con try-catch

### 3. Interfaces de Repositorio
**Ubicación:** `Repositories/Interfaces/IAlertaProximidadRepository.cs`

Extiende `IRepositoryBase<AlertaProximidad>` con métodos específicos:
- `GetAlertasPorBusAsync()`: Consultas filtradas por bus
- `GetAlertasPorAlumnoAsync()`: Consultas filtradas por alumno
- `GetAlertasActivasPorPadreAsync()`: Consultas por padre
- `GetAlertasPorTipoYEstadoAsync()`: Consultas por tipo y estado
- `ConfirmarRecepcionPadreAsync()`: Actualización de confirmación
- `MarcarComoResueltaAsync()`: Actualización de estado

### 4. Implementación del Repositorio
**Ubicación:** `Repositories/Implementations/AlertaProximidadRepository.cs`

Características:
- Extiende `RepositoryBase<AlertaProximidad>`
- Incluye relaciones con `Include()` para Bus y Alumno
- Consultas optimizadas con índices
- Operaciones bulk para limpieza
- Filtros por estado y tipo

### 5. Perfil de AutoMapper
**Ubicación:** `Mappings/AlertaProximidadMappingProfile.cs`

Mapeos configurados:
- `AlertaProximidad -> AlertaProximidadDto`
- `AlertaProximidadCreateDto -> AlertaProximidad`
- `AlertaProximidadUpdateDto -> AlertaProximidad`
- Campos calculados e ignorados apropiadamente

### 6. Controlador API
**Ubicación:** `Controllers/Api/AlertasController.cs`

Endpoints REST:
- `POST /api/alertas` - Crear alerta
- `GET /api/alertas/bus/{idBus}` - Historial por bus
- `GET /api/alertas/alumno/{idAlumno}` - Historial por alumno
- `GET /api/alertas/padre/{idPadre}` - Alertas por padre
- `GET /api/alertas/{id}` - Alerta específica
- `GET /api/alertas/tipo/{tipoAlerta}` - Alertas por tipo
- `PUT /api/alertas/{id}/resolver` - Marcar como resuelta
- `PUT /api/alertas/{id}/confirmar` - Confirmar recepción
- `PUT /api/alertas/{id}` - Actualizar alerta
- `DELETE /api/alertas/limpiar` - Limpiar alertas antiguas

### 7. Clase de Pruebas Actualizada
**Ubicación:** `Tests/AlertaProximidadTest.cs`

Métodos de prueba usando el servicio:
- `CrearAlertaPruebaAsync()`: Prueba creación de alerta
- `ObtenerAlertasActivasPorBusAsync()`: Prueba consultas
- `ConfirmarAlertaPadreAsync()`: Prueba confirmación
- `ResolverAlertaAsync()`: Prueba resolución
- `PruebaFlujoCompletoAsync()`: Prueba flujo completo

## Configuración de Inyección de Dependencias

### Startup.cs - Registros agregados:
```csharp
// Repositorio
services.AddScoped<IAlertaProximidadRepository, AlertaProximidadRepository>();

// Servicio
services.AddScoped<IAlertaService, AlertaService>();
```

## Patrones y Arquitectura Implementados

### 1. Patrón Repository
- Abstrae el acceso a datos
- Implementa operaciones CRUD específicas
- Usa Entity Framework con Include para relaciones

### 2. Patrón Service Layer
- Lógica de negocio centralizada
- Validaciones y transformaciones
- Coordinación entre múltiples repositorios

### 3. Inyección de Dependencias
- Interfaces bien definidas
- Bajo acoplamiento entre capas
- Fácil testing y mocking

### 4. DTO Pattern
- Transferencia de datos optimizada
- Separación entre modelo de dominio y API
- Validaciones en DTOs

## Flujo de Funcionamiento del Servicio

```
1. Controlador recibe request
   ↓
2. Controlador valida ModelState
   ↓
3. Controlador llama al AlertaService
   ↓
4. ServicioAlerta valida datos de negocio
   ↓
5. ServicioAlerta usa AutoMapper para conversiones
   ↓
6. ServicioAlerta llama al Repository
   ↓
7. Repository ejecuta consultas Entity Framework
   ↓
8. ServicioAlerta enriquece DTOs con datos adicionales
   ↓
9. Controlador retorna respuesta HTTP
```

## Ejemplos de Uso

### Crear una alerta:
```json
POST /api/alertas
{
  "tipoAlerta": "proximidad",
  "mensaje": "Bus a 2 paradas de su destino",
  "idBus": 1,
  "idAlumno": 5,
  "estado": "activo"
}
```

### Obtener alertas de un bus:
```
GET /api/alertas/bus/1?incluirResueltas=false
```

### Confirmar recepción:
```
PUT /api/alertas/123/confirmar
```

## Validaciones Implementadas

### En el Servicio:
- Validación de existencia del bus
- Validación de tipos de alerta permitidos
- Validación de estados válidos

### En el Controlador:
- Validación de ModelState
- Validación de parámetros de ruta
- Manejo de errores con responses apropiados

## Estado del Módulo
✅ **Completado**: Servicio, repositorio, controlador API y pruebas
✅ **Registrado**: Inyección de dependencias en Startup.cs
✅ **Compilado**: Sin errores de compilación
⏳ **Pendiente**: Integración con frontend y SignalR

## Próximos Módulos

- **Módulo 3**: Detección de proximidad y generación automática de alertas
- **Módulo 4**: Interfaz frontend con notificaciones en tiempo real
- **Módulo 5**: Confirmación de recogida en parada

## Notas Técnicas

- Sigue el mismo patrón de otros servicios existentes (BusService, NotificacionService)
- Usa AutoMapper para todas las conversiones
- Implementa logging con Console.WriteLine para debugging
- Manejo de errores robusto con try-catch
- Consultas optimizadas con Include() y filtros apropiados
- API RESTful con responses HTTP estándar

## Testing

Para probar el servicio, puedes:

1. Usar el controlador API con Postman/Swagger
2. Inyectar `IAlertaService` en cualquier clase
3. Usar la clase `AlertaProximidadTest` para pruebas unitarias
4. Verificar datos directamente en la tabla `genesis.AlertasProximidad`