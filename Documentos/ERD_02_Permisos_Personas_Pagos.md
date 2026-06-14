# ERD 2 — Permisos, Personas y Pagos

> Equivalente Django → Transportes Génesis:
> - `auth_permission` ≈ `AspNetRoleClaims` / `AspNetUserClaims`
> - `auth_user_user_permissions` ≈ `AspNetUserClaims`
> - Dominio de negocio: `Padres`, `Alumnos`, `Pagos`, catálogos de pago

## Diagrama UML (StarUML)

```
┌──────────────────────────────────────────────────────────────┐
│ <<table>> AspNetUsers                                        │
├──────────────────────────────────────────────────────────────┤
│ + Id : nvarchar(450)              {PK}                       │
│ + UserName : nvarchar(256)                                   │
│ + Email : nvarchar(256)                                      │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> AspNetRoles                                        │
├──────────────────────────────────────────────────────────────┤
│ + Id : nvarchar(450)              {PK}                       │
│ + Name : nvarchar(256)                                       │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> AspNetUserClaims                                   │
├──────────────────────────────────────────────────────────────┤
│ + Id : int                        {PK}                       │
│ + UserId : nvarchar(450)          {FK → AspNetUsers}         │
│ + ClaimType : nvarchar(max)                                  │
│ + ClaimValue : nvarchar(max)                                 │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> AspNetRoleClaims                                    │
├──────────────────────────────────────────────────────────────┤
│ + Id : int                        {PK}                       │
│ + RoleId : nvarchar(450)          {FK → AspNetRoles}         │
│ + ClaimType : nvarchar(max)                                  │
│ + ClaimValue : nvarchar(max)                                 │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> Padres                          schema genesis     │
├──────────────────────────────────────────────────────────────┤
│ + IdPadre : int                   {PK}                       │
│ + Nombre : nvarchar(50)                                      │
│ + Apellido : nvarchar(50)                                    │
│ + Activo : int                                               │
│ + FechaRegistro : datetime2                                  │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> Alumnos                         schema genesis     │
├──────────────────────────────────────────────────────────────┤
│ + IdAlumno : int                  {PK}                       │
│ + IdPadre : int                   {FK → Padres}              │
│ + Nombre : nvarchar(max)                                     │
│ + Apellido : nvarchar(max)                                   │
│ + IdBusAsignado : int             {FK → Buses, 0..1}         │
│ + Latitud : decimal(10,7)                                    │
│ + Longitud : decimal(10,7)                                   │
│ + Direccion : nvarchar(250)                                  │
│ + Activo : int                                               │
│ + FechaRegistro : datetime2                                  │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> Bancos                          schema genesis     │
├──────────────────────────────────────────────────────────────┤
│ + IdBanco : int                   {PK}                       │
│ + Nombre : nvarchar(50)                                      │
│ + Activo : int                                               │
│ + FechaRegistro : datetime2                                  │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> TipoCuenta                      schema genesis     │
├──────────────────────────────────────────────────────────────┤
│ + IdTipoCuenta : int              {PK}                       │
│ + IdPadre : int                   {FK → Padres}              │
│ + IdBanco : int                   {FK → Bancos}              │
│ + Nombre : nvarchar(50)                                      │
│ + Activo : int                                               │
│ + FechaRegistro : datetime2                                  │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> TipoRecorridoPago                schema genesis     │
├──────────────────────────────────────────────────────────────┤
│ + IdTipoRecorrido : int           {PK}                       │
│ + TipoRecorrido : int                                        │
│ + Precio : decimal(18,2)                                     │
│ + DiaMaximoPago : int                                        │
│ + Activo : int                                               │
│ + FechaRegistro : datetime2                                  │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> Pagos                           schema genesis     │
├──────────────────────────────────────────────────────────────┤
│ + IdPago : int                    {PK}                       │
│ + IdAlumno : int                  {FK → Alumnos}             │
│ + IdPadre : int                   {FK → Padres}              │
│ + IdTipoCuenta : int              {FK → TipoCuenta}          │
│ + IdTipoRecorrido : int           {FK → TipoRecorridoPago}    │
│ + MesPagado : int                                            │
│ + Anio : int                                                 │
│ + MontoParcial : decimal(18,2)                             │
│ + PagoCompleto : bit                                         │
│ + Imagen : nvarchar(250)                                     │
│ + Ubicacion : nvarchar(500)                                  │
│ + FechaModif : datetime2                                     │
│ + Activo : int                                               │
│ + FechaRegistro : datetime2                                  │
└──────────────────────────────────────────────────────────────┘

┌──────────────────────────────────────────────────────────────┐
│ <<table>> PagosPadres                     schema dbo         │
├──────────────────────────────────────────────────────────────┤
│ + Id : int                        {PK}                       │
│ + UsuarioId : nvarchar(max)       {FK → AspNetUsers}         │
│ + Mes : nvarchar(max)                                        │
│ + Anio : int                                                 │
│ + Monto : decimal(18,2)                                      │
│ + TipoPago : nvarchar(max)                                   │
│ + ComprobanteUrl : nvarchar(max)                             │
│ + Fecha : datetime2                                          │
└──────────────────────────────────────────────────────────────┘

Relaciones (multiplicidad UML):
  AspNetUsers       (1) ──────── (0..*) AspNetUserClaims
  AspNetRoles       (1) ──────── (0..*) AspNetRoleClaims
  AspNetUsers       (1) ──────── (0..*) PagosPadres
  Padres            (1) ──────── (1..*) Alumnos
  Padres            (1) ──────── (0..*) TipoCuenta
  Padres            (1) ──────── (0..*) Pagos
  Bancos            (1) ──────── (1..*) TipoCuenta
  Alumnos           (1) ──────── (0..*) Pagos
  TipoCuenta        (1) ──────── (0..*) Pagos
  TipoRecorridoPago (1) ──────── (0..*) Pagos
  Alumnos           (1) ──────── (0..1) TipoRecorridoPago
```

## Mermaid

```mermaid
erDiagram
    AspNetUsers {
        string Id PK
        string UserName
        string Email
    }

    AspNetRoles {
        string Id PK
        string Name
    }

    AspNetUserClaims {
        int Id PK
        string UserId FK
        string ClaimType
        string ClaimValue
    }

    AspNetRoleClaims {
        int Id PK
        string RoleId FK
        string ClaimType
        string ClaimValue
    }

    Padres {
        int IdPadre PK
        string Nombre
        string Apellido
        int Activo
        datetime FechaRegistro
    }

    Alumnos {
        int IdAlumno PK
        int IdPadre FK
        string Nombre
        string Apellido
        int IdBusAsignado FK
        decimal Latitud
        decimal Longitud
        string Direccion
        int Activo
        datetime FechaRegistro
    }

    Bancos {
        int IdBanco PK
        string Nombre
        int Activo
        datetime FechaRegistro
    }

    TipoCuenta {
        int IdTipoCuenta PK
        int IdPadre FK
        int IdBanco FK
        string Nombre
        int Activo
        datetime FechaRegistro
    }

    TipoRecorridoPago {
        int IdTipoRecorrido PK
        int TipoRecorrido
        decimal Precio
        int DiaMaximoPago
        int Activo
        datetime FechaRegistro
    }

    Pagos {
        int IdPago PK
        int IdAlumno FK
        int IdPadre FK
        int IdTipoCuenta FK
        int IdTipoRecorrido FK
        int MesPagado
        int Anio
        decimal MontoParcial
        bool PagoCompleto
        string Imagen
        string Ubicacion
        datetime FechaModif
        int Activo
        datetime FechaRegistro
    }

    PagosPadres {
        int Id PK
        string UsuarioId FK
        string Mes
        int Anio
        decimal Monto
        string TipoPago
        string ComprobanteUrl
        datetime Fecha
    }

    AspNetUsers ||--o{ AspNetUserClaims : permisos_usuario
    AspNetRoles ||--o{ AspNetRoleClaims : permisos_rol
    AspNetUsers ||--o{ PagosPadres : registra_pago
    Padres ||--|{ Alumnos : tutor
    Padres ||--o{ TipoCuenta : cuenta
    Padres ||--o{ Pagos : paga
    Bancos ||--|{ TipoCuenta : banco
    Alumnos ||--o{ Pagos : pago_alumno
    TipoCuenta ||--o{ Pagos : medio_pago
    TipoRecorridoPago ||--o{ Pagos : tarifa
    Alumnos ||--o| TipoRecorridoPago : plan
```
