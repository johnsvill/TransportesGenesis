# ERD completo — Transportes Génesis (proyecto real)

Comparación con el diagrama de la presentación (solo geolocalización + `Pago` simplificado).

---

## Tablas que existen en el proyecto

### Identity (ASP.NET Core)

| Tabla | Esquema |
|-------|---------|
| `AspNetUsers` | dbo |
| `AspNetRoles` | dbo |
| `AspNetUserRoles` | dbo |
| `AspNetUserClaims` | dbo |
| `AspNetRoleClaims` | dbo |
| `AspNetUserLogins` | dbo |
| `AspNetUserTokens` | dbo |

### Negocio — Pagos (módulo compañero)

| Tabla | Esquema | Descripción |
|-------|---------|-------------|
| **`Pagos`** | genesis | Pago por alumno/padre, mes, comprobante, monto parcial |
| **`PagosPadres`** | dbo | Registro simplificado por usuario (Monto, TipoPago, ComprobanteUrl) |
| **`Bancos`** | genesis | Catálogo de bancos |
| **`TipoCuenta`** | genesis | Cuenta del padre ligada a banco |
| **`TipoRecorridoPago`** | genesis | Precio y día máximo de pago por tipo de recorrido/alumno |

### Negocio — Personas

| Tabla | Esquema |
|-------|---------|
| `Padres` | genesis |
| `Alumnos` | genesis |

### Negocio — Geolocalización / rutas

| Tabla | Esquema |
|-------|---------|
| `Buses` | genesis |
| `Rutas` | genesis |
| `Paradas` | genesis *(incluye `IdRuta`; no hay tabla `RutaParada` separada)* |
| `AsignacionPilotoBus` | genesis |
| `AsistenciaAlumno` | genesis *(equivale a “confirmación asistencia”)* |
| `UbicacionBusEnTiempoReal` | genesis |
| `RegistroRecogida` | genesis |
| `SolicitudTraslado` | genesis |
| `Alertas` | genesis |
| `AlertasProximidad` | genesis |
| `NotificacionProximidad` | genesis |
| `NotificacionRetraso` | genesis |
| `ConfiguracionSistema` | genesis *(coords colegio, etc.)* |

---

## Qué le falta al diagrama de tu amigo

| En su diagrama | En el proyecto real |
|----------------|---------------------|
| `Pago` simple | **`Pagos`** + **`PagosPadres`** + **`Bancos`** + **`TipoCuenta`** + **`TipoRecorridoPago`** |
| `RutaParada` (N:M) | **`Paradas.IdRuta`** directo (parada pertenece a una ruta) |
| `ConfirmacionAsistencia` | **`AsistenciaAlumno`** |
| — | **`UbicacionBusEnTiempoReal`**, **`SolicitudTraslado`**, **`Alertas*`**, **`ConfiguracionSistema`**, etc. |

---

## Diagrama Mermaid (copiar en [mermaid.live](https://mermaid.live) o en la presentación)

```mermaid
erDiagram
    %% ========== IDENTITY ==========
    AspNetUsers {
        string Id PK
        string UserName
        string Email
        string PasswordHash
        bool IsFirstLogin
        datetime LastLoginDate
    }

    AspNetRoles {
        string Id PK
        string Name
    }

    AspNetUserRoles {
        string UserId PK,FK
        string RoleId PK,FK
    }

    AspNetUsers ||--o{ AspNetUserRoles : tiene
    AspNetRoles ||--o{ AspNetUserRoles : tiene

    %% ========== PERSONAS ==========
    Padres {
        int IdPadre PK
        string Nombre
        string Apellido
        string UsuarioId FK
        int Activo
        datetime FechaRegistro
    }

    Alumnos {
        int IdAlumno PK
        int IdPadre FK
        int IdBusAsignado FK
        string Nombre
        string Apellido
        decimal Latitud
        decimal Longitud
        string Direccion
    }

    AspNetUsers ||--o| Padres : "login padre"
    Padres ||--o{ Alumnos : "tutor de"

    %% ========== PAGOS ==========
    Bancos {
        int IdBanco PK
        string Nombre
    }

    TipoCuenta {
        int IdTipoCuenta PK
        int IdBanco FK
        int IdPadre FK
        string Nombre
    }

    TipoRecorridoPago {
        int IdTipoRecorrido PK
        int IdAlumno FK
        int TipoRecorrido
        decimal Precio
        int DiaMaximoPago
    }

    Pagos {
        int IdPago PK
        int IdAlumno FK
        int IdPadre FK
        int IdTipoCuenta FK
        int IdTipoRecorrido FK
        int MesPagado
        int Anio
        bool PagoCompleto
        decimal MontoParcial
        string Imagen
        string Ubicacion
        datetime FechaModif
    }

    PagosPadres {
        int Id PK
        string UsuarioId FK
        decimal Monto
        datetime Fecha
        string TipoPago
        string ComprobanteUrl
        string Mes
        int Anio
    }

    Bancos ||--o{ TipoCuenta : ofrece
    Padres ||--o{ TipoCuenta : "cuenta de"
    Padres ||--o{ Pagos : paga
    Alumnos ||--o{ Pagos : "pago por"
    TipoCuenta ||--o{ Pagos : "medio pago"
    TipoRecorridoPago ||--o{ Pagos : tarifa
    Alumnos ||--o| TipoRecorridoPago : "plan de"
    AspNetUsers ||--o{ PagosPadres : "registro pago"

    %% ========== FLOTA Y RUTAS ==========
    Buses {
        int IdBus PK
        string Placa UK
        string Modelo
        int Capacidad
        bool Estado
    }

    Rutas {
        int IdRuta PK
        int IdBus FK
        string Nombre
        string TipoRuta
        time HoraInicio
        bool EsActiva
    }

    Paradas {
        int IdParada PK
        int IdRuta FK
        int IdAlumno FK
        decimal Latitud
        decimal Longitud
        string Direccion
        int Orden
        time HoraEstimada
        bool Completada
    }

    AsignacionPilotoBus {
        int IdAsignacion PK
        string IdUsuarioPiloto FK
        int IdBus FK
        datetime FechaAsignacion
        bool EsActual
    }

    Buses ||--o{ Rutas : recorre
    Buses ||--o| AsignacionPilotoBus : "piloto asignado"
    AspNetUsers ||--o{ AsignacionPilotoBus : "rol Piloto/Monitor"
    Rutas ||--o{ Paradas : contiene
    Alumnos ||--o{ Paradas : "parada de"
    Buses ||--o{ Alumnos : "bus fijo"

    %% ========== ASISTENCIA Y TRASLADOS ==========
    AsistenciaAlumno {
        int IdAsistencia PK
        int IdAlumno FK
        datetime Fecha
        bool AsisteMañana
        bool AsisteTarde
        datetime FechaConfirmacion
        int IdBusTemporalMañana FK
        int IdBusTemporalTarde FK
    }

    SolicitudTraslado {
        int IdSolicitud PK
        int IdAlumno FK
        int IdBusOrigen FK
        int IdBusDestino FK
        datetime FechaTraslado
        string Turno
        string Estado
        string AprobadoPor
    }

    Alumnos ||--o{ AsistenciaAlumno : asiste
    Buses ||--o{ AsistenciaAlumno : "bus temp mañana"
    Buses ||--o{ AsistenciaAlumno : "bus temp tarde"
    Alumnos ||--o{ SolicitudTraslado : solicita
    Buses ||--o{ SolicitudTraslado : origen
    Buses ||--o{ SolicitudTraslado : destino

    %% ========== TIEMPO REAL Y ALERTAS ==========
    UbicacionBusEnTiempoReal {
        int IdUbicacion PK
        int IdBus FK
        decimal Latitud
        decimal Longitud
        datetime FechaHora
        decimal Velocidad
        decimal Direccion
    }

    RegistroRecogida {
        int IdRegistro PK
        int IdParada FK
        int IdAlumno FK
        datetime FechaHoraRecogida
        string ConfirmadoPor FK
        bool AlumnoPresente
        decimal Latitud
        decimal Longitud
    }

    Alertas {
        int IdAlerta PK
        string TipoAlerta
        string Mensaje
        string IdRemitente FK
        string IdDestinatario FK
        datetime FechaEnvio
        bool Leida
    }

    AlertasProximidad {
        int Id PK
        int IdBus FK
        int IdAlumno FK
        string TipoAlerta
        string Mensaje
        string Estado
        string IdPadre FK
    }

    NotificacionProximidad {
        int IdNotificacion PK
        int IdParada FK
        int IdPadre FK
        string TipoNotificacion
        bool Enviada
    }

    NotificacionRetraso {
        int IdNotificacion PK
        int IdRuta FK
        int TiempoRetrasoMinutos
        string Motivo
        datetime FechaHora
    }

    ConfiguracionSistema {
        int IdConfiguracion PK
        string Clave
        string Valor
        string Categoria
        bool Activo
    }

    Buses ||--o{ UbicacionBusEnTiempoReal : "GPS vivo"
    Paradas ||--o{ RegistroRecogida : "recogida en"
    Alumnos ||--o{ RegistroRecogida : "alumno recogido"
    AspNetUsers ||--o{ RegistroRecogida : "confirmado por piloto"
    Rutas ||--o{ NotificacionRetraso : retraso
    Paradas ||--o{ NotificacionProximidad : proximidad
    Padres ||--o{ NotificacionProximidad : notifica
    Buses ||--o{ AlertasProximidad : alerta
    Alumnos ||--o{ AlertasProximidad : "alerta alumno"
```

---

## Versión simplificada para diapositiva (solo bloques)

```mermaid
flowchart TB
    subgraph IDENTITY["🔐 Identity"]
        U[AspNetUsers]
        R[AspNetRoles]
        UR[AspNetUserRoles]
        U --- UR --- R
    end

    subgraph PAGOS["💳 Pagos"]
        PG[Pagos]
        PP[PagosPadres]
        BK[Bancos]
        TC[TipoCuenta]
        TR[TipoRecorridoPago]
        BK --> TC
        TC --> PG
        TR --> PG
        PP --> U
    end

    subgraph PERSONAS["👨‍👩‍👧 Personas"]
        P[Padres]
        A[Alumnos]
        P --> A
        U --> P
    end

    subgraph GEO["🚌 Geolocalización"]
        B[Buses]
        RT[Rutas]
        PA[Paradas]
        APB[AsignacionPilotoBus]
        AA[AsistenciaAlumno]
        UB[UbicacionBusEnTiempoReal]
        RR[RegistroRecogida]
        ST[SolicitudTraslado]
        B --> RT --> PA
        U --> APB --> B
        A --> AA
        B --> UB
        PA --> RR
        A --> ST
    end

    P --> PG
    A --> PG
    A --> PA
    A --> B
```

---

*Fuente: `ApplicationDbContext.cs`, modelos en `Models/DB/`, migraciones EF Core.*
