-- Alumnos sin bus ni parada para probar "Gestionar Paradas"
-- Ejecutar: sqlcmd -S "(local)" -d TransportesGenesis -E -i Scripts\SeedData_GestionarParadas.sql

SET NOCOUNT ON;

DECLARE @IdPadre INT = (SELECT TOP 1 IdPadre FROM genesis.Padres ORDER BY IdPadre);
DECLARE @NextId INT = ISNULL((SELECT MAX(IdAlumno) FROM genesis.Alumnos), 0) + 1;

IF @IdPadre IS NULL
BEGIN
    RAISERROR('No hay padres registrados. Cree al menos un padre antes de ejecutar este script.', 16, 1);
    RETURN;
END

PRINT 'Insertando alumnos sin bus ni parada asignados...';

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Sofía' AND Apellido = N'Mendoza Ruiz')
BEGIN
    INSERT INTO genesis.Alumnos (IdAlumno, IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@NextId, @IdPadre, N'Sofía', N'Mendoza Ruiz', NULL, 14.621405, -90.513709, N'5ta Avenida 12-34 Zona 10, Guatemala', 1, GETDATE());
    SET @NextId = @NextId + 1;
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Diego' AND Apellido = N'Castillo Vega')
BEGIN
    INSERT INTO genesis.Alumnos (IdAlumno, IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@NextId, @IdPadre, N'Diego', N'Castillo Vega', NULL, 14.618200, -90.515800, N'10ma Calle 5-20 Zona 10, Guatemala', 1, GETDATE());
    SET @NextId = @NextId + 1;
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Valentina' AND Apellido = N'Ortiz Morales')
BEGIN
    INSERT INTO genesis.Alumnos (IdAlumno, IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@NextId, @IdPadre, N'Valentina', N'Ortiz Morales', NULL, 14.625100, -90.509400, N'2da Avenida 8-15 Zona 9, Guatemala', 1, GETDATE());
    SET @NextId = @NextId + 1;
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Mateo' AND Apellido = N'Herrera Paz')
BEGIN
    INSERT INTO genesis.Alumnos (IdAlumno, IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@NextId, @IdPadre, N'Mateo', N'Herrera Paz', NULL, 14.632800, -90.518600, N'7ma Avenida 3-45 Zona 4, Guatemala', 1, GETDATE());
    SET @NextId = @NextId + 1;
END

IF NOT EXISTS (SELECT 1 FROM genesis.Alumnos WHERE Nombre = N'Isabella' AND Apellido = N'Reyes Luna')
BEGIN
    INSERT INTO genesis.Alumnos (IdAlumno, IdPadre, Nombre, Apellido, IdBusAsignado, Latitud, Longitud, Direccion, Activo, FechaRegistro)
    VALUES (@NextId, @IdPadre, N'Isabella', N'Reyes Luna', NULL, 14.619700, -90.521300, N'14 Calle 6-78 Zona 1, Guatemala', 1, GETDATE());
END

SELECT COUNT(*) AS AlumnosSinAsignar
FROM genesis.Alumnos a
WHERE a.IdBusAsignado IS NULL
  AND NOT EXISTS (SELECT 1 FROM genesis.Paradas p WHERE p.IdAlumno = a.IdAlumno);

PRINT 'Listo. Recargue la pagina Gestionar Paradas.';
