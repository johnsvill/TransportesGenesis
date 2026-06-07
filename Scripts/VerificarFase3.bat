@echo off
echo ========================================
echo Verificacion de FASE 3 - Geolocalizacion
echo ========================================
echo.

echo [1/4] Verificando buses en BD...
sqlcmd -S "(local)" -d "TransportesGenesis" -Q "SELECT COUNT(*) AS TotalBuses FROM genesis.Buses WHERE Estado = 1" -E -h -1
echo.

echo [2/4] Verificando ubicaciones en BD...
sqlcmd -S "(local)" -d "TransportesGenesis" -Q "SELECT COUNT(*) AS TotalUbicaciones FROM genesis.UbicacionBusEnTiempoReal" -E -h -1
echo.

echo [3/4] Listando buses con ultima ubicacion...
sqlcmd -S "(local)" -d "TransportesGenesis" -Q "SELECT b.Placa, u.Latitud, u.Longitud, u.Velocidad, DATEDIFF(MINUTE, u.FechaHora, GETDATE()) AS MinutosAtras FROM genesis.Buses b LEFT JOIN genesis.UbicacionBusEnTiempoReal u ON b.IdBus = u.IdBus WHERE b.Estado = 1 ORDER BY u.FechaHora DESC" -E -W
echo.

echo ========================================
echo Verificacion completada
echo ========================================
echo.
echo IMPORTANTE: Para probar el mapa, ejecuta la aplicacion con:
echo    dotnet run
echo.
echo Y navega a:
echo    https://localhost:5001/Geolocalizacion/MapaEnTiempoReal
echo    O: http://localhost:5000/Geolocalizacion/MapaEnTiempoReal
echo.
pause
