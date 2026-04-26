-- =====================================================
-- TABLA: ConfiguracionSistema
-- Proposito: Almacenar configuracion global del sistema
--            incluyendo ubicacion del colegio
-- =====================================================

USE TransportesGenesis;
GO

-- Crear tabla si no existe
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ConfiguracionSistema' AND schema_id = SCHEMA_ID('genesis'))
BEGIN
    CREATE TABLE genesis.ConfiguracionSistema (
        IdConfiguracion INT PRIMARY KEY IDENTITY(1,1),
        Clave NVARCHAR(100) NOT NULL UNIQUE,
        Valor NVARCHAR(MAX) NOT NULL,
        Descripcion NVARCHAR(500),
        Tipo NVARCHAR(50), -- 'Texto', 'Numero', 'Coordenada', 'Booleano'
        Categoria NVARCHAR(50), -- 'General', 'Ubicacion', 'Notificaciones', etc.
        Activo BIT DEFAULT 1,
        FechaRegistro DATETIME DEFAULT GETDATE(),
        UltimaModificacion DATETIME,
        ModificadoPor NVARCHAR(100)
    );

    PRINT 'Tabla ConfiguracionSistema creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla ConfiguracionSistema ya existe';
END
GO

-- Insertar configuracion inicial del colegio
IF NOT EXISTS (SELECT * FROM genesis.ConfiguracionSistema WHERE Clave = 'Colegio_Nombre')
BEGIN
    INSERT INTO genesis.ConfiguracionSistema (Clave, Valor, Descripcion, Tipo, Categoria)
    VALUES 
        ('Colegio_Nombre', 'Colegio Genesis', 'Nombre de la institucion educativa', 'Texto', 'Ubicacion'),
        ('Colegio_Direccion', 'Calle 100 #15-20, Bogota', 'Direccion completa del colegio', 'Texto', 'Ubicacion'),
        ('Colegio_Latitud', '4.6850', 'Coordenada GPS Latitud del colegio', 'Coordenada', 'Ubicacion'),
        ('Colegio_Longitud', '-74.0480', 'Coordenada GPS Longitud del colegio', 'Coordenada', 'Ubicacion'),
        ('Colegio_HoraInicioClases', '07:00', 'Hora de inicio de clases (manana)', 'Texto', 'General'),
        ('Colegio_HoraFinClases', '14:30', 'Hora de finalizacion de clases (tarde)', 'Texto', 'General'),
        ('Sistema_DiasHabiles', 'Lunes,Martes,Miercoles,Jueves,Viernes', 'Dias en que operan las rutas', 'Texto', 'General');

    PRINT 'Configuracion inicial insertada exitosamente';
    PRINT '';
    PRINT 'Configuraciones creadas:';
    SELECT Clave, Valor, Descripcion FROM genesis.ConfiguracionSistema WHERE Categoria = 'Ubicacion';
END
ELSE
BEGIN
    PRINT 'Configuracion del colegio ya existe';
    SELECT Clave, Valor, Descripcion FROM genesis.ConfiguracionSistema WHERE Categoria = 'Ubicacion';
END
GO
