using System.Globalization;
using Microsoft.EntityFrameworkCore;
using TransportesGenesis.Data.Context;
using TransportesGenesis.DTOs.Geolocalizacion;
using TransportesGenesis.Models.DB.Negocio;
using TransportesGenesis.Services.Interfaces;

namespace TransportesGenesis.Services.Implementations
{
    public class ConfiguracionService : IConfiguracionService
    {
        private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

        // Defaults Guatemala (piloto) — no Bogotá
        private const decimal DefaultLat = 14.6235m;
        private const decimal DefaultLon = -90.4956m;

        private readonly ApplicationDbContext _context;

        public ConfiguracionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string?> ObtenerValorAsync(string clave)
        {
            var config = await _context.ConfiguracionSistemaDb
                .FirstOrDefaultAsync(c => c.Clave == clave && c.Activo);

            return config?.Valor;
        }

        public async Task<(decimal latitud, decimal longitud)> ObtenerCoordenadasColegioAsync()
        {
            var colegio = await ObtenerColegioAsync();
            return (colegio.Latitud, colegio.Longitud);
        }

        public async Task<string> ObtenerDireccionColegioAsync()
        {
            var colegio = await ObtenerColegioAsync();
            return string.IsNullOrWhiteSpace(colegio.Direccion) ? colegio.Nombre : colegio.Direccion;
        }

        public async Task<ColegioConfigDto> ObtenerColegioAsync()
        {
            var nombre = await ObtenerValorAsync("Colegio_Nombre");
            var direccion = await ObtenerValorAsync("Colegio_Direccion");
            var latitudStr = await ObtenerValorAsync("Colegio_Latitud");
            var longitudStr = await ObtenerValorAsync("Colegio_Longitud");
            var horaInicio = await ObtenerValorAsync("Colegio_HoraInicioClases");
            var horaFin = await ObtenerValorAsync("Colegio_HoraFinClases");

            decimal latitud = DefaultLat;
            decimal longitud = DefaultLon;

            if (!string.IsNullOrEmpty(latitudStr) &&
                decimal.TryParse(latitudStr, NumberStyles.Any, Invariant, out var lat))
            {
                latitud = lat;
            }

            if (!string.IsNullOrEmpty(longitudStr) &&
                decimal.TryParse(longitudStr, NumberStyles.Any, Invariant, out var lon))
            {
                longitud = lon;
            }

            return new ColegioConfigDto
            {
                Nombre = string.IsNullOrWhiteSpace(nombre) ? "Colegio Genesis" : nombre,
                Direccion = direccion ?? "",
                Latitud = latitud,
                Longitud = longitud,
                HoraInicioClases = string.IsNullOrWhiteSpace(horaInicio) ? "07:00" : horaInicio,
                HoraFinClases = string.IsNullOrWhiteSpace(horaFin) ? "14:30" : horaFin
            };
        }

        public async Task GuardarColegioAsync(ColegioConfigDto colegio, string? modificadoPor = null)
        {
            if (colegio == null) throw new ArgumentNullException(nameof(colegio));
            if (string.IsNullOrWhiteSpace(colegio.Nombre))
                throw new ArgumentException("El nombre del colegio es obligatorio.");

            var ahora = DateTime.Now;
            var usuario = modificadoPor ?? "admin";

            await UpsertAsync("Colegio_Nombre", colegio.Nombre.Trim(),
                "Nombre de la institucion educativa", "Texto", "Ubicacion", ahora, usuario);
            await UpsertAsync("Colegio_Direccion", colegio.Direccion?.Trim() ?? "",
                "Direccion completa del colegio", "Texto", "Ubicacion", ahora, usuario);
            await UpsertAsync("Colegio_Latitud", colegio.Latitud.ToString(Invariant),
                "Coordenada GPS Latitud del colegio", "Coordenada", "Ubicacion", ahora, usuario);
            await UpsertAsync("Colegio_Longitud", colegio.Longitud.ToString(Invariant),
                "Coordenada GPS Longitud del colegio", "Coordenada", "Ubicacion", ahora, usuario);
            await UpsertAsync("Colegio_HoraInicioClases",
                string.IsNullOrWhiteSpace(colegio.HoraInicioClases) ? "07:00" : colegio.HoraInicioClases.Trim(),
                "Hora de inicio de clases (manana)", "Texto", "General", ahora, usuario);
            await UpsertAsync("Colegio_HoraFinClases",
                string.IsNullOrWhiteSpace(colegio.HoraFinClases) ? "14:30" : colegio.HoraFinClases.Trim(),
                "Hora de finalizacion de clases (tarde)", "Texto", "General", ahora, usuario);

            await _context.SaveChangesAsync();
        }

        private async Task UpsertAsync(
            string clave,
            string valor,
            string descripcion,
            string tipo,
            string categoria,
            DateTime ahora,
            string modificadoPor)
        {
            var existente = await _context.ConfiguracionSistemaDb
                .FirstOrDefaultAsync(c => c.Clave == clave);

            if (existente == null)
            {
                _context.ConfiguracionSistemaDb.Add(new ConfiguracionSistema
                {
                    Clave = clave,
                    Valor = valor,
                    Descripcion = descripcion,
                    Tipo = tipo,
                    Categoria = categoria,
                    Activo = true,
                    FechaRegistro = ahora,
                    UltimaModificacion = ahora,
                    ModificadoPor = modificadoPor
                });
            }
            else
            {
                existente.Valor = valor;
                existente.Descripcion = descripcion;
                existente.Tipo = tipo;
                existente.Categoria = categoria;
                existente.Activo = true;
                existente.UltimaModificacion = ahora;
                existente.ModificadoPor = modificadoPor;
            }
        }
    }
}
