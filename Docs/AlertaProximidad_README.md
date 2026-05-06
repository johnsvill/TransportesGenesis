# Módulo de Alertas de Proximidad - TransportesGenesis

## Descripción
Este módulo implementa un sistema de alertas para notificar a los padres cuando el bus escolar se acerca a la parada de su hijo.

## Archivos Creados

### 1. DTOs (Data Transfer Objects)
**Ubicación:** `DTOs/Notificaciones/AlertaProximidadDto.cs`

Contiene las clases:
- `AlertaProximidadDto`: DTO principal para transferir datos de alertas
- `AlertaProximidadCreateDto`: DTO para crear nuevas alertas
- `AlertaProximidadUpdateDto`: DTO para actualizar el estado de alertas

### 2. Entidad de Base de Datos
**Ubicación:** `Models/DB/Negocio/AlertaProximidad.cs`

Propiedades principales:
- `Id`: Identificador único
- `TipoAlerta`: "proximidad" o "retraso"
- `Mensaje`: Descripción de la alerta
- `FechaHora`: Timestamp de creación
- `IdBus`: Referencia al bus
- `IdAlumno`: Referencia al alumno (opcional)
- `Estado`: "activo" o "resuelto"
- `ConfirmacionPadre`: Indicador de confirmación
- `FechaConfirmacionPadre`: Timestamp de confirmación

### 3. Configuración de Entity Framework
**Ubicación:** `Data/Configuraciones/AlertaProximidadConfig.cs`

Define:
- Mapeo de la entidad a la tabla `AlertasProximidad`
- Relaciones con entidades `Bus` y `Alumnos`
- Índices para optimización de consultas
- Restricciones de datos (check constraints)

### 4. Migración de Base de Datos
**Ubicación:** `Data/Migrations/20260505170000_CreateAlertaProximidadTable.cs`

Crea:
- Tabla `genesis.AlertasProximidad`
- Índices necesarios para rendimiento
- Restricciones de integridad

### 5. Clase de Pruebas
**Ubicación:** `Tests/AlertaProximidadTest.cs`

Métodos de prueba:
- `CrearAlertaPruebaAsync()`: Crear alerta de prueba
- `ObtenerAlertasActivasPorBusAsync()`: Obtener alertas por bus
- `ConfirmarAlertaPadreAsync()`: Confirmar recepción por padre
- `ResolverAlertaAsync()`: Resolver alerta

## Esquema de Base de Datos

```sql
CREATE TABLE [genesis].[AlertasProximidad] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TipoAlerta] varchar(20) NOT NULL,
    [Mensaje] nvarchar(500) NOT NULL,
    [FechaHora] datetime2 NOT NULL,
    [IdBus] int NOT NULL,
    [IdAlumno] int NULL,
    [Estado] varchar(15) NOT NULL DEFAULT 'activo',
    [FechaResolucion] datetime2 NULL,
    [ParadasRestantes] int NULL,
    [ParadaActual] nvarchar(200) NULL,
    [ParadaDestino] nvarchar(200) NULL,
    [ConfirmacionPadre] bit NOT NULL DEFAULT 0,
    [FechaConfirmacionPadre] datetime2 NULL,
    [IdPadre] nvarchar(450) NULL,
    [Activo] int NOT NULL DEFAULT 1,
    [FechaRegistro] datetime2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_AlertasProximidad] PRIMARY KEY ([Id]),
    CONSTRAINT [CK_AlertaProximidad_Estado] CHECK ([Estado] IN ('activo', 'resuelto')),
    CONSTRAINT [CK_AlertaProximidad_TipoAlerta] CHECK ([TipoAlerta] IN ('proximidad', 'retraso'))
)
```

## Flujo de Funcionamiento

1. **Detección de Proximidad**: El sistema detecta cuando un bus está a 2 paradas de la parada asignada
2. **Creación de Alerta**: Se crea una nueva `AlertaProximidad` con estado "activo"
3. **Notificación al Padre**: Se envía la alerta al padre del alumno
4. **Confirmación**: El padre confirma que recibió la alerta y estará esperando
5. **Resolución**: La alerta se marca como "resuelto" cuando el niño es recogido

## Próximas Fases

- **Fase 2**: Lógica de secuencia y detección de paradas
- **Fase 3**: Servicios de geolocalización y detección automática
- **Fase 4**: Interfaz frontend con SignalR para tiempo real
- **Fase 5**: Confirmación de recogida en parada

## Notas Técnicas

- Se usa el esquema `genesis` para mantener organización
- Los índices están optimizados para consultas frecuentes por bus y estado
- Las restricciones garantizan integridad de datos
- Compatible con .NET 8 y Entity Framework Core 8.0+

## Estado del Módulo
✅ **Completado**: Modelo, migración y estructura base
⏳ **Pendiente**: Integración con servicios de geolocalización y frontend