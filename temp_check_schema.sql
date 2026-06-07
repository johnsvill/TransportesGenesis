IF SCHEMA_ID(N'genesis') IS NULL EXEC(N'CREATE SCHEMA [genesis];');
GO


CREATE TABLE [genesis].[Alertas] (
    [IdAlerta] int NOT NULL IDENTITY,
    [TipoAlerta] nvarchar(50) NOT NULL,
    [Mensaje] nvarchar(500) NOT NULL,
    [IdRemitente] nvarchar(450) NULL,
    [IdDestinatario] nvarchar(450) NULL,
    [FechaEnvio] datetime2 NOT NULL,
    [Leida] bit NOT NULL,
    [FechaLectura] datetime2 NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Alertas] PRIMARY KEY ([IdAlerta])
);
GO


CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [IsFirstLogin] bit NOT NULL,
    [LastLoginDate] datetime2 NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [genesis].[Bancos] (
    [IdBanco] int NOT NULL IDENTITY,
    [Nombre] nvarchar(50) NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Bancos] PRIMARY KEY ([IdBanco])
);
GO


CREATE TABLE [genesis].[Buses] (
    [IdBus] int NOT NULL IDENTITY,
    [Placa] nvarchar(20) NOT NULL,
    [Modelo] nvarchar(50) NULL,
    [Capacidad] int NOT NULL,
    [Estado] bit NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Buses] PRIMARY KEY ([IdBus])
);
GO


CREATE TABLE [genesis].[ConfiguracionSistema] (
    [IdConfiguracion] int NOT NULL IDENTITY,
    [Clave] nvarchar(100) NOT NULL,
    [Valor] nvarchar(max) NOT NULL,
    [Descripcion] nvarchar(500) NULL,
    [Tipo] nvarchar(50) NULL,
    [Categoria] nvarchar(50) NULL,
    [Activo] bit NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    [UltimaModificacion] datetime2 NULL,
    [ModificadoPor] nvarchar(100) NULL,
    CONSTRAINT [PK_ConfiguracionSistema] PRIMARY KEY ([IdConfiguracion])
);
GO


CREATE TABLE [genesis].[Padres] (
    [IdPadre] int NOT NULL IDENTITY,
    [Nombre] nvarchar(50) NOT NULL,
    [Apellido] nvarchar(50) NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Padres] PRIMARY KEY ([IdPadre])
);
GO


CREATE TABLE [PagosPadres] (
    [Id] int NOT NULL IDENTITY,
    [UsuarioId] nvarchar(max) NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [TipoPago] nvarchar(max) NOT NULL,
    [ComprobanteUrl] nvarchar(max) NOT NULL,
    [Mes] nvarchar(max) NOT NULL,
    [Anio] int NOT NULL,
    CONSTRAINT [PK_PagosPadres] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [genesis].[TipoRecorridoPago] (
    [IdTipoRecorrido] int NOT NULL IDENTITY,
    [TipoRecorrido] int NOT NULL,
    [Precio] decimal(18,2) NOT NULL,
    [DiaMaximoPago] int NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_TipoRecorridoPago] PRIMARY KEY ([IdTipoRecorrido])
);
GO


CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [genesis].[AsignacionPilotoBus] (
    [IdAsignacion] int NOT NULL IDENTITY,
    [IdUsuarioPiloto] nvarchar(450) NOT NULL,
    [IdBus] int NOT NULL,
    [FechaAsignacion] datetime2 NOT NULL,
    [FechaFinAsignacion] datetime2 NULL,
    [EsActual] bit NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_AsignacionPilotoBus] PRIMARY KEY ([IdAsignacion]),
    CONSTRAINT [FK_AsignacionPilotoBus_Buses_IdBus] FOREIGN KEY ([IdBus]) REFERENCES [genesis].[Buses] ([IdBus]) ON DELETE CASCADE
);
GO


CREATE TABLE [genesis].[Rutas] (
    [IdRuta] int NOT NULL IDENTITY,
    [IdBus] int NOT NULL,
    [Nombre] nvarchar(100) NOT NULL,
    [Descripcion] nvarchar(250) NULL,
    [TipoRuta] nvarchar(10) NOT NULL,
    [HoraInicio] time NOT NULL,
    [EsActiva] bit NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Rutas] PRIMARY KEY ([IdRuta]),
    CONSTRAINT [FK_Rutas_Buses_IdBus] FOREIGN KEY ([IdBus]) REFERENCES [genesis].[Buses] ([IdBus]) ON DELETE CASCADE
);
GO


CREATE TABLE [genesis].[UbicacionBusEnTiempoReal] (
    [IdUbicacion] int NOT NULL IDENTITY,
    [IdBus] int NOT NULL,
    [Latitud] decimal(10,7) NOT NULL,
    [Longitud] decimal(10,7) NOT NULL,
    [FechaHora] datetime2 NOT NULL,
    [Velocidad] decimal(5,2) NULL,
    [Direccion] decimal(5,2) NULL,
    CONSTRAINT [PK_UbicacionBusEnTiempoReal] PRIMARY KEY ([IdUbicacion]),
    CONSTRAINT [FK_UbicacionBusEnTiempoReal_Buses_IdBus] FOREIGN KEY ([IdBus]) REFERENCES [genesis].[Buses] ([IdBus]) ON DELETE CASCADE
);
GO


CREATE TABLE [genesis].[TipoCuenta] (
    [IdTipoCuenta] int NOT NULL IDENTITY,
    [Nombre] nvarchar(50) NOT NULL,
    [IdBanco] int NOT NULL,
    [IdPadre] int NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_TipoCuenta] PRIMARY KEY ([IdTipoCuenta]),
    CONSTRAINT [FK_TipoCuenta_Bancos_IdBanco] FOREIGN KEY ([IdBanco]) REFERENCES [genesis].[Bancos] ([IdBanco]) ON DELETE CASCADE,
    CONSTRAINT [FK_TipoCuenta_Padres_IdPadre] FOREIGN KEY ([IdPadre]) REFERENCES [genesis].[Padres] ([IdPadre]) ON DELETE CASCADE
);
GO


CREATE TABLE [genesis].[Alumnos] (
    [IdAlumno] int NOT NULL,
    [IdPadre] int NOT NULL,
    [Nombre] nvarchar(50) NOT NULL,
    [Apellido] nvarchar(50) NOT NULL,
    [IdBusAsignado] int NULL,
    [Latitud] decimal(10,7) NULL,
    [Longitud] decimal(10,7) NULL,
    [Direccion] nvarchar(250) NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Alumnos] PRIMARY KEY ([IdAlumno]),
    CONSTRAINT [FK_Alumnos_Buses_IdBusAsignado] FOREIGN KEY ([IdBusAsignado]) REFERENCES [genesis].[Buses] ([IdBus]) ON DELETE SET NULL,
    CONSTRAINT [FK_Alumnos_Padres_IdPadre] FOREIGN KEY ([IdPadre]) REFERENCES [genesis].[Padres] ([IdPadre]) ON DELETE CASCADE,
    CONSTRAINT [FK_Alumnos_TipoRecorridoPago_IdAlumno] FOREIGN KEY ([IdAlumno]) REFERENCES [genesis].[TipoRecorridoPago] ([IdTipoRecorrido]) ON DELETE CASCADE
);
GO


CREATE TABLE [genesis].[NotificacionRetraso] (
    [IdNotificacion] int NOT NULL IDENTITY,
    [IdRuta] int NOT NULL,
    [TiempoRetrasoMinutos] int NOT NULL,
    [Motivo] nvarchar(100) NOT NULL,
    [FechaHora] datetime2 NOT NULL,
    [NotificadoAPadres] bit NOT NULL,
    [FechaNotificacionPadres] datetime2 NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_NotificacionRetraso] PRIMARY KEY ([IdNotificacion]),
    CONSTRAINT [FK_NotificacionRetraso_Rutas_IdRuta] FOREIGN KEY ([IdRuta]) REFERENCES [genesis].[Rutas] ([IdRuta]) ON DELETE CASCADE
);
GO


CREATE TABLE [genesis].[AsistenciaAlumno] (
    [IdAsistencia] int NOT NULL IDENTITY,
    [IdAlumno] int NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [AsisteMañana] bit NOT NULL,
    [AsisteTarde] bit NOT NULL,
    [FechaConfirmacion] datetime2 NULL,
    [IdBusTemporalMañana] int NULL,
    [IdBusTemporalTarde] int NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_AsistenciaAlumno] PRIMARY KEY ([IdAsistencia]),
    CONSTRAINT [FK_AsistenciaAlumno_Alumnos_IdAlumno] FOREIGN KEY ([IdAlumno]) REFERENCES [genesis].[Alumnos] ([IdAlumno]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AsistenciaAlumno_Buses_IdBusTemporalMañana] FOREIGN KEY ([IdBusTemporalMañana]) REFERENCES [genesis].[Buses] ([IdBus]) ON DELETE SET NULL,
    CONSTRAINT [FK_AsistenciaAlumno_Buses_IdBusTemporalTarde] FOREIGN KEY ([IdBusTemporalTarde]) REFERENCES [genesis].[Buses] ([IdBus]) ON DELETE SET NULL
);
GO


CREATE TABLE [genesis].[Pagos] (
    [IdPago] int NOT NULL IDENTITY,
    [IdAlumno] int NOT NULL,
    [IdPadre] int NOT NULL,
    [IdTipoCuenta] int NOT NULL,
    [IdTipoRecorrido] int NOT NULL,
    [MesPagado] int NOT NULL,
    [Anio] int NOT NULL,
    [PagoCompleto] bit NOT NULL,
    [MontoParcial] decimal(18,2) NOT NULL,
    [Imagen] nvarchar(250) NOT NULL,
    [Ubicacion] nvarchar(500) NOT NULL,
    [FechaModif] datetime2 NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Pagos] PRIMARY KEY ([IdPago]),
    CONSTRAINT [FK_Pagos_Alumnos_IdAlumno] FOREIGN KEY ([IdAlumno]) REFERENCES [genesis].[Alumnos] ([IdAlumno]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Pagos_Padres_IdPadre] FOREIGN KEY ([IdPadre]) REFERENCES [genesis].[Padres] ([IdPadre]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Pagos_TipoCuenta_IdTipoCuenta] FOREIGN KEY ([IdTipoCuenta]) REFERENCES [genesis].[TipoCuenta] ([IdTipoCuenta]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Pagos_TipoRecorridoPago_IdTipoRecorrido] FOREIGN KEY ([IdTipoRecorrido]) REFERENCES [genesis].[TipoRecorridoPago] ([IdTipoRecorrido]) ON DELETE NO ACTION
);
GO


CREATE TABLE [genesis].[Paradas] (
    [IdParada] int NOT NULL IDENTITY,
    [IdRuta] int NOT NULL,
    [IdAlumno] int NULL,
    [Latitud] decimal(10,7) NOT NULL,
    [Longitud] decimal(10,7) NOT NULL,
    [Direccion] nvarchar(250) NULL,
    [Orden] int NOT NULL,
    [HoraEstimada] time NULL,
    [Completada] bit NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_Paradas] PRIMARY KEY ([IdParada]),
    CONSTRAINT [FK_Paradas_Alumnos_IdAlumno] FOREIGN KEY ([IdAlumno]) REFERENCES [genesis].[Alumnos] ([IdAlumno]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Paradas_Rutas_IdRuta] FOREIGN KEY ([IdRuta]) REFERENCES [genesis].[Rutas] ([IdRuta]) ON DELETE NO ACTION
);
GO


CREATE TABLE [genesis].[SolicitudTraslado] (
    [IdSolicitud] int NOT NULL IDENTITY,
    [IdAlumno] int NOT NULL,
    [IdBusOrigen] int NOT NULL,
    [IdBusDestino] int NULL,
    [FechaTraslado] datetime2 NOT NULL,
    [Turno] nvarchar(10) NOT NULL,
    [Motivo] nvarchar(250) NULL,
    [Estado] nvarchar(20) NOT NULL,
    [AprobadoPor] nvarchar(450) NULL,
    [FechaRespuesta] datetime2 NULL,
    [ComentarioAdmin] nvarchar(250) NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_SolicitudTraslado] PRIMARY KEY ([IdSolicitud]),
    CONSTRAINT [FK_SolicitudTraslado_Alumnos_IdAlumno] FOREIGN KEY ([IdAlumno]) REFERENCES [genesis].[Alumnos] ([IdAlumno]) ON DELETE NO ACTION,
    CONSTRAINT [FK_SolicitudTraslado_Buses_IdBusDestino] FOREIGN KEY ([IdBusDestino]) REFERENCES [genesis].[Buses] ([IdBus]),
    CONSTRAINT [FK_SolicitudTraslado_Buses_IdBusOrigen] FOREIGN KEY ([IdBusOrigen]) REFERENCES [genesis].[Buses] ([IdBus]) ON DELETE NO ACTION
);
GO


CREATE TABLE [genesis].[NotificacionProximidad] (
    [IdNotificacion] int NOT NULL IDENTITY,
    [IdParada] int NOT NULL,
    [IdPadre] int NOT NULL,
    [TipoNotificacion] nvarchar(50) NOT NULL,
    [Enviada] bit NOT NULL,
    [FechaHoraEnvio] datetime2 NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_NotificacionProximidad] PRIMARY KEY ([IdNotificacion]),
    CONSTRAINT [FK_NotificacionProximidad_Padres_IdPadre] FOREIGN KEY ([IdPadre]) REFERENCES [genesis].[Padres] ([IdPadre]) ON DELETE NO ACTION,
    CONSTRAINT [FK_NotificacionProximidad_Paradas_IdParada] FOREIGN KEY ([IdParada]) REFERENCES [genesis].[Paradas] ([IdParada]) ON DELETE NO ACTION
);
GO


CREATE TABLE [genesis].[RegistroRecogida] (
    [IdRegistro] int NOT NULL IDENTITY,
    [IdParada] int NOT NULL,
    [IdAlumno] int NOT NULL,
    [FechaHoraRecogida] datetime2 NOT NULL,
    [ConfirmadoPor] nvarchar(450) NULL,
    [Latitud] decimal(10,7) NULL,
    [Longitud] decimal(10,7) NULL,
    [AlumnoPresente] bit NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_RegistroRecogida] PRIMARY KEY ([IdRegistro]),
    CONSTRAINT [FK_RegistroRecogida_Alumnos_IdAlumno] FOREIGN KEY ([IdAlumno]) REFERENCES [genesis].[Alumnos] ([IdAlumno]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RegistroRecogida_Paradas_IdParada] FOREIGN KEY ([IdParada]) REFERENCES [genesis].[Paradas] ([IdParada]) ON DELETE NO ACTION
);
GO


CREATE INDEX [IX_Alertas_FechaEnvio] ON [genesis].[Alertas] ([FechaEnvio]);
GO


CREATE INDEX [IX_Alertas_IdDestinatario] ON [genesis].[Alertas] ([IdDestinatario]);
GO


CREATE INDEX [IX_Alertas_Leida] ON [genesis].[Alertas] ([Leida]);
GO


CREATE INDEX [IX_Alumnos_FechaRegistro] ON [genesis].[Alumnos] ([FechaRegistro]);
GO


CREATE INDEX [IX_Alumnos_IdBusAsignado] ON [genesis].[Alumnos] ([IdBusAsignado]);
GO


CREATE INDEX [IX_Alumnos_IdPadre] ON [genesis].[Alumnos] ([IdPadre]);
GO


CREATE INDEX [IX_AsignacionPilotoBus_EsActual] ON [genesis].[AsignacionPilotoBus] ([EsActual]);
GO


CREATE INDEX [IX_AsignacionPilotoBus_FechaAsignacion] ON [genesis].[AsignacionPilotoBus] ([FechaAsignacion]);
GO


CREATE UNIQUE INDEX [IX_AsignacionPilotoBus_IdBus] ON [genesis].[AsignacionPilotoBus] ([IdBus]);
GO


CREATE INDEX [IX_AsignacionPilotoBus_IdUsuarioPiloto] ON [genesis].[AsignacionPilotoBus] ([IdUsuarioPiloto]);
GO


CREATE INDEX [IX_AsistenciaAlumno_Fecha] ON [genesis].[AsistenciaAlumno] ([Fecha]);
GO


CREATE INDEX [IX_AsistenciaAlumno_FechaRegistro] ON [genesis].[AsistenciaAlumno] ([FechaRegistro]);
GO


CREATE INDEX [IX_AsistenciaAlumno_IdAlumno] ON [genesis].[AsistenciaAlumno] ([IdAlumno]);
GO


CREATE INDEX [IX_AsistenciaAlumno_IdBusTemporalMañana] ON [genesis].[AsistenciaAlumno] ([IdBusTemporalMañana]);
GO


CREATE INDEX [IX_AsistenciaAlumno_IdBusTemporalTarde] ON [genesis].[AsistenciaAlumno] ([IdBusTemporalTarde]);
GO


CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO


CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO


CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO


CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO


CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO


CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO


CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO


CREATE INDEX [IX_Bancos_FechaRegistro] ON [genesis].[Bancos] ([FechaRegistro]);
GO


CREATE INDEX [IX_Buses_FechaRegistro] ON [genesis].[Buses] ([FechaRegistro]);
GO


CREATE UNIQUE INDEX [IX_Buses_Placa] ON [genesis].[Buses] ([Placa]);
GO


CREATE INDEX [IX_NotificacionProximidad_Enviada] ON [genesis].[NotificacionProximidad] ([Enviada]);
GO


CREATE INDEX [IX_NotificacionProximidad_FechaHoraEnvio] ON [genesis].[NotificacionProximidad] ([FechaHoraEnvio]);
GO


CREATE INDEX [IX_NotificacionProximidad_IdPadre] ON [genesis].[NotificacionProximidad] ([IdPadre]);
GO


CREATE INDEX [IX_NotificacionProximidad_IdParada] ON [genesis].[NotificacionProximidad] ([IdParada]);
GO


CREATE INDEX [IX_NotificacionRetraso_FechaHora] ON [genesis].[NotificacionRetraso] ([FechaHora]);
GO


CREATE INDEX [IX_NotificacionRetraso_IdRuta] ON [genesis].[NotificacionRetraso] ([IdRuta]);
GO


CREATE INDEX [IX_NotificacionRetraso_NotificadoAPadres] ON [genesis].[NotificacionRetraso] ([NotificadoAPadres]);
GO


CREATE INDEX [IX_Padres_FechaRegistro] ON [genesis].[Padres] ([FechaRegistro]);
GO


CREATE INDEX [IX_Pagos_FechaRegistro] ON [genesis].[Pagos] ([FechaRegistro]);
GO


CREATE INDEX [IX_Pagos_IdAlumno] ON [genesis].[Pagos] ([IdAlumno]);
GO


CREATE INDEX [IX_Pagos_IdPadre] ON [genesis].[Pagos] ([IdPadre]);
GO


CREATE INDEX [IX_Pagos_IdTipoCuenta] ON [genesis].[Pagos] ([IdTipoCuenta]);
GO


CREATE INDEX [IX_Pagos_IdTipoRecorrido] ON [genesis].[Pagos] ([IdTipoRecorrido]);
GO


CREATE INDEX [IX_Paradas_FechaRegistro] ON [genesis].[Paradas] ([FechaRegistro]);
GO


CREATE INDEX [IX_Paradas_IdAlumno] ON [genesis].[Paradas] ([IdAlumno]);
GO


CREATE INDEX [IX_Paradas_IdRuta] ON [genesis].[Paradas] ([IdRuta]);
GO


CREATE INDEX [IX_Paradas_Orden] ON [genesis].[Paradas] ([Orden]);
GO


CREATE INDEX [IX_RegistroRecogida_AlumnoPresente] ON [genesis].[RegistroRecogida] ([AlumnoPresente]);
GO


CREATE INDEX [IX_RegistroRecogida_FechaHoraRecogida] ON [genesis].[RegistroRecogida] ([FechaHoraRecogida]);
GO


CREATE INDEX [IX_RegistroRecogida_IdAlumno] ON [genesis].[RegistroRecogida] ([IdAlumno]);
GO


CREATE INDEX [IX_RegistroRecogida_IdParada] ON [genesis].[RegistroRecogida] ([IdParada]);
GO


CREATE INDEX [IX_Rutas_FechaRegistro] ON [genesis].[Rutas] ([FechaRegistro]);
GO


CREATE INDEX [IX_Rutas_IdBus] ON [genesis].[Rutas] ([IdBus]);
GO


CREATE INDEX [IX_Rutas_TipoRuta] ON [genesis].[Rutas] ([TipoRuta]);
GO


CREATE INDEX [IX_SolicitudTraslado_Estado] ON [genesis].[SolicitudTraslado] ([Estado]);
GO


CREATE INDEX [IX_SolicitudTraslado_FechaRegistro] ON [genesis].[SolicitudTraslado] ([FechaRegistro]);
GO


CREATE INDEX [IX_SolicitudTraslado_FechaTraslado] ON [genesis].[SolicitudTraslado] ([FechaTraslado]);
GO


CREATE INDEX [IX_SolicitudTraslado_IdAlumno] ON [genesis].[SolicitudTraslado] ([IdAlumno]);
GO


CREATE INDEX [IX_SolicitudTraslado_IdBusDestino] ON [genesis].[SolicitudTraslado] ([IdBusDestino]);
GO


CREATE INDEX [IX_SolicitudTraslado_IdBusOrigen] ON [genesis].[SolicitudTraslado] ([IdBusOrigen]);
GO


CREATE INDEX [IX_TipoCuenta_FechaRegistro] ON [genesis].[TipoCuenta] ([FechaRegistro]);
GO


CREATE INDEX [IX_TipoCuenta_IdBanco] ON [genesis].[TipoCuenta] ([IdBanco]);
GO


CREATE INDEX [IX_TipoCuenta_IdPadre] ON [genesis].[TipoCuenta] ([IdPadre]);
GO


CREATE INDEX [IX_UbicacionBusEnTiempoReal_FechaHora] ON [genesis].[UbicacionBusEnTiempoReal] ([FechaHora]);
GO


CREATE INDEX [IX_UbicacionBusEnTiempoReal_IdBus] ON [genesis].[UbicacionBusEnTiempoReal] ([IdBus]);
GO


