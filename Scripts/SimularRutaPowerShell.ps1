param(
	[string]$baseUrl = "https://localhost:7241",
	[int]$idBus = 1,
	[int]$delay = 4
)

# simulate-route.ps1 - Simula posiciones y POST /api/ubicaciones
# Uso: .\SimularRutaPowerShell.ps1 -baseUrl "https://localhost:7241" -idBus 1 -delay 4

Write-Host "Simulación iniciada hacia $baseUrl (Bus $idBus)" -ForegroundColor Cyan

# Rutas (mañana) - Zona 4, Ciudad de Guatemala
$route = @(
	@{ lat=14.6380; lon=-90.5240 },  # 7ma Av. y 2da Calle, Zona 4
	@{ lat=14.6369; lon=-90.5228 },
	@{ lat=14.6358; lon=-90.5215 },  # 5ta Av. y 4ta Calle, Zona 4
	@{ lat=14.6347; lon=-90.5203 },
	@{ lat=14.6336; lon=-90.5190 },  # Terminal Central, Zona 4
	@{ lat=14.6325; lon=-90.5178 },
	@{ lat=14.6314; lon=-90.5165 },  # Mercado El Guarda, Zona 4
	@{ lat=14.6303; lon=-90.5155 },
	@{ lat=14.6292; lon=-90.5145 },  # Av. Bolívar y 8va Calle, Zona 4
	@{ lat=14.6281; lon=-90.5135 },
	@{ lat=14.6270; lon=-90.5125 }   # Colegio Yulimay PC, Zona 4
)

# Desactivar validación SSL temporalmente (solo para pruebas locales)
Add-Type @"
using System.Net;
using System.Security.Cryptography.X509Certificates;
public class TrustAllCertsPolicy : ICertificatePolicy {
	public bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem) { return true; }
}
"@
[System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy

# Preparar headers
$headers = @{ 'Content-Type' = 'application/json' }

foreach ($p in $route) {
	$body = @{ 
		idBus    = $idBus
		latitud  = [decimal]$($p.lat)
		longitud = [decimal]$($p.lon)
		velocidad = 12.0
		direccion = 0
	} | ConvertTo-Json

	try {
		$uri = "$baseUrl/api/ubicaciones"
		$resp = Invoke-RestMethod -Uri $uri -Method Post -Body $body -Headers $headers -ContentType 'application/json'
		Write-Host "$(Get-Date -Format 'HH:mm:ss') -> Enviado: $($p.lat),$($p.lon)  | Respuesta: $(if ($resp) { ($resp | ConvertTo-Json -Depth 1) } else { 'No content' })" -ForegroundColor Green
	} catch {
		Write-Host "$(Get-Date -Format 'HH:mm:ss') -> Error enviando ubicación: $($_.Exception.Message)" -ForegroundColor Red
	}

	Start-Sleep -Seconds $delay
}

Write-Host "Simulación finalizada" -ForegroundColor Cyan
