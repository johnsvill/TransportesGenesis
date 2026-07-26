# Genera TransportesGenesis_FullRestore.sql - Script completo de restauracion
param(
    [string]$Server = "(local)",
    [string]$Database = "TransportesGenesis",
    [string]$OutputFile = "C:\Proyectos\TransportesGenesis\Scripts\TransportesGenesis_FullRestore.sql"
)

function Escape-SqlValue($val, $type) {
    if ($null -eq $val -or [DBNull]::Value.Equals($val)) { return "NULL" }
    switch -Regex ($type) {
        "bit" { if ($val -eq $true -or $val -eq 1) { return "1" } else { return "0" } }
        "int|bigint|smallint|tinyint|decimal|numeric|float|real|money" { return $val.ToString() }
        "datetime|datetime2|date|smalldatetime" {
            return "CAST(N'$($val.ToString('yyyy-MM-dd HH:mm:ss.fffffff'))' AS datetime2)"
        }
        "datetimeoffset" {
            return "CAST(N'$($val.ToString('yyyy-MM-dd HH:mm:ss.fffffff zzz'))' AS datetimeoffset)"
        }
        "time" { return "CAST(N'$($val.ToString())' AS time)" }
        default {
            $s = $val.ToString().Replace("'", "''")
            return "N'$s'"
        }
    }
}

function Get-SqlType($row) {
    $t = $row["DATA_TYPE"]
    $len = $row["CHARACTER_MAXIMUM_LENGTH"]
    $prec = $row["NUMERIC_PRECISION"]
    $scale = $row["NUMERIC_SCALE"]
    switch ($t) {
        "nvarchar" { if ($len -eq -1) { "nvarchar(max)" } else { "nvarchar($len)" } }
        "varchar" { if ($len -eq -1) { "varchar(max)" } else { "varchar($len)" } }
        "decimal" { "decimal($prec,$scale)" }
        "numeric" { "numeric($prec,$scale)" }
        "datetime2" { "datetime2" }
        default { $t }
    }
}

$connStr = "Server=$Server;Database=$Database;Trusted_Connection=True;TrustServerCertificate=True"
$conn = New-Object System.Data.SqlClient.SqlConnection($connStr)
$conn.Open()

$sb = New-Object System.Text.StringBuilder
$nl = [Environment]::NewLine

function Append($text) { [void]$sb.Append($text + $nl) }

Append "/*============================================================================"
Append "  TransportesGenesis - Script completo de restauracion"
Append "  Generado: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
Append "  Origen:   $Server / $Database"
Append "  Destino:  SQL Server 2022 (RTM) - 16.0.1000.6"
Append "  "
Append "  INSTRUCCIONES:"
Append "  1. Ejecutar en SSMS conectado a la instancia destino."
Append "  2. Revisar/adaptar el nombre de la base de datos si es necesario."
Append "  3. El script crea la BD, esquemas, tablas, indices, FKs y datos."
Append "  4. Tablas vacias se incluyen con comentario -- (sin datos)."
Append "  "
Append "  NOTAS DE DRIFT (BD actual vs modelo EF):"
Append "  - genesis.Padres.UsuarioId: EXISTE en BD actual (incluida)."
Append "  - dbo.PagosPadresDb: NO existe en BD actual; se crea segun EF."
Append "  - genesis.CuentasUsuarios: NO existe en BD actual; se crea segun EF."
Append "  - genesis.MontosPadres: NO existe en BD actual; se crea segun EF."
Append "  - genesis.TipoRecorridoPago: 0 filas (Alumnos sin FK 1:1 en BD actual)."
Append "  - FKs adicionales del modelo EF incluidas al final del DDL."
Append "============================================================================*/"
Append ""
Append "SET NOCOUNT ON;"
Append "SET XACT_ABORT ON;"
Append "GO"
Append ""

# --- DATABASE ---
Append "/* ========== CREACION DE BASE DE DATOS ========== */"
Append "IF DB_ID(N'TransportesGenesis') IS NULL"
Append "BEGIN"
Append "    CREATE DATABASE [TransportesGenesis]"
Append "        COLLATE SQL_Latin1_General_CP1_CI_AS;"
Append "END"
Append "GO"
Append ""
Append "USE [TransportesGenesis];"
Append "GO"
Append ""

# --- SCHEMA ---
Append "/* ========== ESQUEMAS ========== */"
Append "IF SCHEMA_ID(N'genesis') IS NULL EXEC(N'CREATE SCHEMA [genesis];');"
Append "GO"
Append ""

# --- TABLE DDL (ordered by dependencies) ---
$tableOrder = @(
    "dbo.__EFMigrationsHistory",
    "dbo.AspNetRoles",
    "dbo.AspNetUsers",
    "dbo.AspNetRoleClaims",
    "dbo.AspNetUserClaims",
    "dbo.AspNetUserLogins",
    "dbo.AspNetUserRoles",
    "dbo.AspNetUserTokens",
    "dbo.PagosPadresDb",
    "genesis.Bancos",
    "genesis.Buses",
    "genesis.Padres",
    "genesis.ConfiguracionSistema",
    "genesis.Alertas",
    "genesis.TipoRecorridoPago",
    "genesis.MontosPadres",
    "genesis.CuentasUsuarios",
    "genesis.AsignacionPilotoBus",
    "genesis.Rutas",
    "genesis.UbicacionBusEnTiempoReal",
    "genesis.TipoCuenta",
    "genesis.Alumnos",
    "genesis.NotificacionRetraso",
    "genesis.AsistenciaAlumno",
    "genesis.Pagos",
    "genesis.Paradas",
    "genesis.SolicitudTraslado",
    "genesis.NotificacionProximidad",
    "genesis.RegistroRecogida",
    "genesis.AlertasProximidad"
)

# Tables defined only in EF (not in live DB)
$efOnlyTables = @{
    "dbo.PagosPadresDb" = @"
CREATE TABLE [dbo].[PagosPadresDb] (
    [Id] int NOT NULL IDENTITY(1,1),
    [UsuarioId] nvarchar(max) NOT NULL,
    [Monto] decimal(18,2) NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [FechaPago] datetime2 NOT NULL,
    [TipoPago] nvarchar(max) NOT NULL,
    [ComprobanteUrl] nvarchar(max) NOT NULL,
    [Mes] nvarchar(max) NOT NULL,
    [Anio] int NOT NULL,
    [EstadoAdmin] nvarchar(max) NOT NULL,
    [EstadoStripe] nvarchar(max) NOT NULL,
    [NumeroComprobante] nvarchar(max) NOT NULL,
    [IdBanco] int NULL,
    [IdCuentaUsuario] int NULL,
    CONSTRAINT [PK_PagosPadresDb] PRIMARY KEY ([Id])
);
"@
    "genesis.MontosPadres" = @"
CREATE TABLE [genesis].[MontosPadres] (
    [IdMontoPadre] int NOT NULL IDENTITY(1,1),
    [UsuarioId] nvarchar(max) NOT NULL,
    [MontoAsignado] decimal(18,2) NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_MontosPadres] PRIMARY KEY ([IdMontoPadre])
);
"@
    "genesis.CuentasUsuarios" = @"
CREATE TABLE [genesis].[CuentasUsuarios] (
    [IdCuentaUsuario] int NOT NULL IDENTITY(1,1),
    [UsuarioId] nvarchar(max) NOT NULL,
    [IdBanco] int NOT NULL,
    [NumeroCuenta] nvarchar(max) NOT NULL,
    [Alias] nvarchar(max) NOT NULL,
    [Activo] int NOT NULL,
    [FechaRegistro] datetime2 NOT NULL,
    CONSTRAINT [PK_CuentasUsuarios] PRIMARY KEY ([IdCuentaUsuario])
);
"@
}

Append "/* ========== TABLAS (ordenadas por dependencias) ========== */"

foreach ($fullTable in $tableOrder) {
    $parts = $fullTable.Split('.')
    $schema = $parts[0]
    $table = $parts[1]
    $qualified = "[$schema].[$table]"

    Append "-- $qualified"
    Append "IF OBJECT_ID(N'$schema.$table', N'U') IS NULL"
    Append "BEGIN"

    if ($efOnlyTables.ContainsKey($fullTable)) {
        Append "    $($efOnlyTables[$fullTable].Trim())"
        Append "END"
        Append "ELSE PRINT N'[INFO] Tabla $qualified ya existe - omitiendo CREATE.';"
        Append "GO"
        Append ""
        continue
    }

    # Build CREATE TABLE from INFORMATION_SCHEMA
    $colCmd = $conn.CreateCommand()
    $colCmd.CommandText = @"
SELECT c.COLUMN_NAME, c.DATA_TYPE, c.CHARACTER_MAXIMUM_LENGTH, c.NUMERIC_PRECISION, c.NUMERIC_SCALE,
       c.IS_NULLABLE, c.COLUMN_DEFAULT,
       CASE WHEN ic.object_id IS NOT NULL THEN 1 ELSE 0 END AS IsIdentity,
       CASE WHEN pk.COLUMN_NAME IS NOT NULL THEN 1 ELSE 0 END AS IsPK
FROM INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN sys.columns sc ON sc.object_id = OBJECT_ID('$schema.$table') AND sc.name = c.COLUMN_NAME
LEFT JOIN sys.identity_columns ic ON ic.object_id = sc.object_id AND ic.column_id = sc.column_id
LEFT JOIN (
    SELECT ku.TABLE_SCHEMA, ku.TABLE_NAME, ku.COLUMN_NAME
    FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc
    JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE ku ON tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
    WHERE tc.CONSTRAINT_TYPE = 'PRIMARY KEY'
) pk ON pk.TABLE_SCHEMA = c.TABLE_SCHEMA AND pk.TABLE_NAME = c.TABLE_NAME AND pk.COLUMN_NAME = c.COLUMN_NAME
WHERE c.TABLE_SCHEMA = '$schema' AND c.TABLE_NAME = '$table'
ORDER BY c.ORDINAL_POSITION
"@
    $reader = $colCmd.ExecuteReader()
    $cols = @()
    $pkCols = @()
    while ($reader.Read()) {
        $colName = $reader["COLUMN_NAME"]
        $sqlType = Get-SqlType $reader
        $nullable = $reader["IS_NULLABLE"]
        $isIdentity = [int]$reader["IsIdentity"] -eq 1
        $isPK = [int]$reader["IsPK"] -eq 1
        $default = $reader["COLUMN_DEFAULT"]

        $colDef = "    [$colName] $sqlType"
        if ($isIdentity) { $colDef += " IDENTITY(1,1)" }
        if ($nullable -eq "NO") { $colDef += " NOT NULL" } else { $colDef += " NULL" }
        if ($default -and -not $isIdentity) {
            $defVal = $default.ToString().Trim()
            if ($defVal -match "^\(\(.*\)\)$") { $colDef += " DEFAULT $($defVal.Substring(1, $defVal.Length-2))" }
            elseif ($defVal -match "^\('") { $colDef += " DEFAULT $defVal" }
            elseif ($defVal -match "^\(getdate") { $colDef += " DEFAULT (getdate())" }
            elseif ($defVal -match "^\(\d") { $colDef += " DEFAULT $($defVal.Trim('(',')'))" }
        }
        $cols += $colDef
        if ($isPK) { $pkCols += "[$colName]" }
    }
    $reader.Close()

    if ($cols.Count -eq 0) {
        Append "    -- ADVERTENCIA: Tabla $qualified no encontrada en BD origen."
        Append "END"
        Append "GO"
        Append ""
        continue
    }

    Append "    CREATE TABLE $qualified ("
    Append ($cols -join ",$nl")
    if ($pkCols.Count -gt 0) {
        Append ",    CONSTRAINT [PK_$table] PRIMARY KEY ($($pkCols -join ', '))"
    }
    Append "    );"
    Append "END"
    Append "ELSE PRINT N'[INFO] Tabla $qualified ya existe - omitiendo CREATE.';"
    Append "GO"
    Append ""
}

# --- CHECK CONSTRAINTS ---
Append "/* ========== CHECK CONSTRAINTS ========== */"
$chkCmd = $conn.CreateCommand()
$chkCmd.CommandText = @"
SELECT OBJECT_SCHEMA_NAME(parent_object_id) AS Sch, OBJECT_NAME(parent_object_id) AS Tbl, name, definition
FROM sys.check_constraints
WHERE OBJECT_SCHEMA_NAME(parent_object_id) IN ('dbo','genesis')
"@
$chkReader = $chkCmd.ExecuteReader()
while ($chkReader.Read()) {
    $sch = $chkReader["Sch"]
    $tbl = $chkReader["Tbl"]
    $name = $chkReader["name"]
    $def = $chkReader["definition"]
    Append "IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'$name')"
    Append "    ALTER TABLE [$sch].[$tbl] ADD CONSTRAINT [$name] CHECK $def;"
    Append "GO"
}
$chkReader.Close()
Append ""

# --- FOREIGN KEYS (from live DB + EF additions) - se aplican al final ---
$fkCmd = $conn.CreateCommand()
$fkCmd.CommandText = @"
SELECT fk.name, SCHEMA_NAME(tp.schema_id) AS PS, tp.name AS PT, cp.name AS PC,
       SCHEMA_NAME(tr.schema_id) AS RS, tr.name AS RT, cr.name AS RC,
       fk.delete_referential_action_desc AS OnDelete
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
JOIN sys.tables tp ON fkc.parent_object_id = tp.object_id
JOIN sys.columns cp ON fkc.parent_object_id = cp.object_id AND fkc.parent_column_id = cp.column_id
JOIN sys.tables tr ON fkc.referenced_object_id = tr.object_id
JOIN sys.columns cr ON fkc.referenced_object_id = cr.object_id AND fkc.referenced_column_id = cr.column_id
ORDER BY PS, PT, fk.name
"@
$fkReader = $fkCmd.ExecuteReader()
$fks = @()
while ($fkReader.Read()) {
    $fks += @{
        Name = $fkReader["name"]
        PS = $fkReader["PS"]; PT = $fkReader["PT"]; PC = $fkReader["PC"]
        RS = $fkReader["RS"]; RT = $fkReader["RT"]; RC = $fkReader["RC"]
        OnDelete = $fkReader["OnDelete"]
    }
}
$fkReader.Close()

# EF FKs missing in live DB - obtener nombres de columna reales desde BD
$mananaCol = "IdBusTemporalMañana"
$tardeCol = "IdBusTemporalTarde"
$colNameCmd = $conn.CreateCommand()
$colNameCmd.CommandText = "SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('genesis.AsistenciaAlumno') AND name LIKE 'IdBusTemporal%' ORDER BY column_id"
$colNameReader = $colNameCmd.ExecuteReader()
$tempCols = @()
while ($colNameReader.Read()) { $tempCols += $colNameReader["name"].ToString() }
$colNameReader.Close()
if ($tempCols.Count -ge 1) { $mananaCol = $tempCols[0] }
if ($tempCols.Count -ge 2) { $tardeCol = $tempCols[1] }

$efFks = @(
    @{ Name="FK_Alumnos_Buses_IdBusAsignado"; PS="genesis"; PT="Alumnos"; PC="IdBusAsignado"; RS="genesis"; RT="Buses"; RC="IdBus"; OnDelete="SET_NULL" },
    @{ Name="FK_Alumnos_TipoRecorridoPago_IdAlumno"; PS="genesis"; PT="Alumnos"; PC="IdAlumno"; RS="genesis"; RT="TipoRecorridoPago"; RC="IdTipoRecorrido"; OnDelete="CASCADE" },
    @{ Name="FK_AsistenciaAlumno_Buses_IdBusTemporalManana"; PS="genesis"; PT="AsistenciaAlumno"; PC=$mananaCol; RS="genesis"; RT="Buses"; RC="IdBus"; OnDelete="SET_NULL" },
    @{ Name="FK_AsistenciaAlumno_Buses_IdBusTemporalTarde"; PS="genesis"; PT="AsistenciaAlumno"; PC=$tardeCol; RS="genesis"; RT="Buses"; RC="IdBus"; OnDelete="SET_NULL" },
    @{ Name="FK_AlertasProximidad_Buses_IdBus"; PS="genesis"; PT="AlertasProximidad"; PC="IdBus"; RS="genesis"; RT="Buses"; RC="IdBus"; OnDelete="NO_ACTION" },
    @{ Name="FK_AlertasProximidad_Alumnos_IdAlumno"; PS="genesis"; PT="AlertasProximidad"; PC="IdAlumno"; RS="genesis"; RT="Alumnos"; RC="IdAlumno"; OnDelete="SET_NULL" },
    @{ Name="FK_SolicitudTraslado_Buses_IdBusDestino"; PS="genesis"; PT="SolicitudTraslado"; PC="IdBusDestino"; RS="genesis"; RT="Buses"; RC="IdBus"; OnDelete="NO_ACTION" }
)

$existingFkNames = @($fks | ForEach-Object { $_.Name })
$allFks = @($fks)
foreach ($fk in $efFks) {
    if ($existingFkNames -notcontains $fk.Name) { $allFks += $fk }
}
$optionalFkNames = @('FK_Alumnos_TipoRecorridoPago_IdAlumno')

# --- INDEXES ---
Append "/* ========== INDICES (no-PK) ========== */"
$idxCmd = $conn.CreateCommand()
$idxCmd.CommandText = @"
SELECT SCHEMA_NAME(t.schema_id) AS Sch, t.name AS Tbl, i.name AS IdxName, i.is_unique, i.has_filter, i.filter_definition,
       STRING_AGG(c.name, ', ') WITHIN GROUP (ORDER BY ic.key_ordinal) AS Cols
FROM sys.indexes i
JOIN sys.tables t ON i.object_id = t.object_id
JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE i.is_primary_key = 0 AND i.type > 0 AND t.is_ms_shipped = 0
  AND SCHEMA_NAME(t.schema_id) IN ('dbo','genesis')
GROUP BY t.schema_id, t.name, i.name, i.is_unique, i.has_filter, i.filter_definition
ORDER BY Sch, Tbl, IdxName
"@
$idxReader = $idxCmd.ExecuteReader()
while ($idxReader.Read()) {
    $sch = $idxReader["Sch"]; $tbl = $idxReader["Tbl"]
    $idxName = $idxReader["IdxName"]; $unique = [bool]$idxReader["is_unique"]
    $hasFilter = [bool]$idxReader["has_filter"]; $filter = $idxReader["filter_definition"]
    $cols = ($idxReader["Cols"] -split ', ' | ForEach-Object { "[$_]" }) -join ", "
    $uniqueStr = if ($unique) { "UNIQUE " } else { "" }
    $filterStr = if ($hasFilter) { " WHERE $filter" } else { "" }
    Append "IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'$idxName' AND object_id = OBJECT_ID(N'$sch.$tbl'))"
    Append "    CREATE $($uniqueStr)NONCLUSTERED INDEX [$idxName] ON [$sch].[$tbl] ($cols)$filterStr;"
    Append "GO"
}
$idxReader.Close()
Append ""

# --- DATA INSERTS ---
Append "/* ========== DATOS (INSERT) - ordenados por dependencias ========== */"
Append "PRINT N'Iniciando carga de datos...';"
Append "GO"
Append ""

$dataOrder = @(
    "dbo.__EFMigrationsHistory", "dbo.AspNetRoles", "dbo.AspNetUsers",
    "dbo.AspNetRoleClaims", "dbo.AspNetUserClaims", "dbo.AspNetUserLogins",
    "dbo.AspNetUserRoles", "dbo.AspNetUserTokens",
    "genesis.Bancos", "genesis.Buses", "genesis.Padres", "genesis.ConfiguracionSistema",
    "genesis.Alertas", "genesis.TipoRecorridoPago",
    "genesis.AsignacionPilotoBus", "genesis.Rutas", "genesis.UbicacionBusEnTiempoReal",
    "genesis.TipoCuenta", "genesis.Alumnos", "genesis.NotificacionRetraso",
    "genesis.AsistenciaAlumno", "genesis.Pagos", "genesis.Paradas",
    "genesis.SolicitudTraslado", "genesis.NotificacionProximidad",
    "genesis.RegistroRecogida", "genesis.AlertasProximidad"
)

foreach ($fullTable in $dataOrder) {
    $parts = $fullTable.Split('.')
    $schema = $parts[0]; $table = $parts[1]
    $qualified = "[$schema].[$table]"

    if ($efOnlyTables.ContainsKey($fullTable)) {
        Append "-- $qualified (tabla EF - sin datos en BD origen)"
        Append "-- (sin datos)"
        Append ""
        continue
    }

    $existsCmd = $conn.CreateCommand()
    $existsCmd.CommandText = "SELECT OBJECT_ID(N'$schema.$table', N'U')"
    $objId = $existsCmd.ExecuteScalar()
    if (-not $objId) {
        Append "-- ADVERTENCIA: $qualified no existe en BD origen"
        Append ""
        continue
    }

    $countCmd = $conn.CreateCommand()
    $countCmd.CommandText = "SELECT COUNT(*) FROM $qualified"
    $count = [int]$countCmd.ExecuteScalar()

    Append "-- ============================================================"
    Append "-- $qualified ($count filas)"
    Append "-- ============================================================"

    if ($count -eq 0) {
        Append "-- (sin datos)"
        Append ""
        continue
    }

    $colCmd = $conn.CreateCommand()
    $colCmd.CommandText = @"
SELECT c.COLUMN_NAME, c.DATA_TYPE,
       CASE WHEN ic.object_id IS NOT NULL THEN 1 ELSE 0 END AS IsIdentity
FROM INFORMATION_SCHEMA.COLUMNS c
LEFT JOIN sys.columns sc ON sc.object_id = OBJECT_ID('$schema.$table') AND sc.name = c.COLUMN_NAME
LEFT JOIN sys.identity_columns ic ON ic.object_id = sc.object_id AND ic.column_id = sc.column_id
WHERE c.TABLE_SCHEMA = '$schema' AND c.TABLE_NAME = '$table'
ORDER BY c.ORDINAL_POSITION
"@
    $colReader = $colCmd.ExecuteReader()
    $columns = @(); $hasIdentity = $false
    while ($colReader.Read()) {
        $columns += @{ Name = $colReader["COLUMN_NAME"]; Type = $colReader["DATA_TYPE"] }
        if ([int]$colReader["IsIdentity"] -eq 1) { $hasIdentity = $true }
    }
    $colReader.Close()

    if ($hasIdentity) { Append "SET IDENTITY_INSERT $qualified ON;" }

    $selectCmd = $conn.CreateCommand()
    $selectCmd.CommandText = "SELECT * FROM $qualified"
    $dataReader = $selectCmd.ExecuteReader()
    $colNames = ($columns | ForEach-Object { "[$($_.Name)]" }) -join ", "

    while ($dataReader.Read()) {
        $values = @()
        for ($i = 0; $i -lt $columns.Count; $i++) {
            $values += (Escape-SqlValue $dataReader.GetValue($i) $columns[$i].Type)
        }
        Append "INSERT INTO $qualified ($colNames) VALUES ($($values -join ', '));"
    }
    $dataReader.Close()

    if ($hasIdentity) { Append "SET IDENTITY_INSERT $qualified OFF;" }
    Append "GO"
    Append ""
}

# --- FOREIGN KEYS after data ---
Append "/* ========== CLAVES FORANEAS (despues de datos) ========== */"
foreach ($fk in $allFks) {
    $onDel = switch ($fk.OnDelete) {
        "CASCADE" { "ON DELETE CASCADE" }
        "SET_NULL" { "ON DELETE SET NULL" }
        "NO_ACTION" { "" }
        default { "" }
    }
    if ($optionalFkNames -contains $fk.Name) {
        Append "-- OPCIONAL: $($fk.Name) - comentada porque TipoRecorridoPago esta vacio."
        Append "-- Descomentar tras poblar genesis.TipoRecorridoPago (1 fila por Alumno, IdTipoRecorrido = IdAlumno)."
        Append "-- IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'$($fk.Name)')"
        Append "--     ALTER TABLE [$($fk.PS)].[$($fk.PT)] ADD CONSTRAINT [$($fk.Name)]"
        Append "--         FOREIGN KEY ([$($fk.PC)]) REFERENCES [$($fk.RS)].[$($fk.RT)] ([$($fk.RC)]) $onDel;"
        Append "-- GO"
        Append ""
        continue
    }
    Append "IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'$($fk.Name)')"
    Append "    ALTER TABLE [$($fk.PS)].[$($fk.PT)] ADD CONSTRAINT [$($fk.Name)]"
    Append "        FOREIGN KEY ([$($fk.PC)]) REFERENCES [$($fk.RS)].[$($fk.RT)] ([$($fk.RC)]) $onDel;"
    Append "GO"
}
Append ""

Append "/* ========== FIN DEL SCRIPT ========== */"
Append "PRINT N'Restauracion completada: TransportesGenesis';"
Append "GO"

$conn.Close()
[System.IO.File]::WriteAllText($OutputFile, $sb.ToString(), (New-Object System.Text.UTF8Encoding $false))
Write-Host "Script generado: $OutputFile"
Write-Host "Tamano: $((Get-Item $OutputFile).Length / 1MB) MB"
