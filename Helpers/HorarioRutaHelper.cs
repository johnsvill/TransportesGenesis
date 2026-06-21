using TransportesGenesis.DTOs.Geolocalizacion;

namespace TransportesGenesis.Helpers
{
    public static class HorarioRutaHelper
    {
        public static bool EstaEnHorarioRecorrido(RutaDto ruta, DateTime? ahora = null)
        {
            if (!ruta.EsActiva)
                return false;

            var momento = ahora ?? DateTime.Now;
            var hora = momento.TimeOfDay;
            var tipo = (ruta.TipoRuta ?? string.Empty).Trim();

            var esManana = tipo.Contains("Mañana", StringComparison.OrdinalIgnoreCase)
                || tipo.Contains("Manana", StringComparison.OrdinalIgnoreCase);
            var esTarde = tipo.Contains("Tarde", StringComparison.OrdinalIgnoreCase);

            if (esManana)
            {
                var inicio = ruta.HoraInicio > TimeSpan.Zero ? ruta.HoraInicio : new TimeSpan(6, 0, 0);
                return hora >= inicio && hora < TimeSpan.FromHours(12);
            }

            if (esTarde)
            {
                var inicio = ruta.HoraInicio > TimeSpan.FromHours(12)
                    ? ruta.HoraInicio
                    : new TimeSpan(14, 30, 0);
                return hora >= inicio && hora < TimeSpan.FromHours(19);
            }

            return hora >= ruta.HoraInicio
                && hora <= ruta.HoraInicio.Add(TimeSpan.FromHours(3));
        }

        public static string ObtenerMensajeFueraDeHorario(RutaDto ruta)
        {
            return $"El autobús no está en recorrido por el horario ({ruta.TipoRuta}, inicio {ruta.HoraInicio:hh\\:mm}).";
        }

        public static string SugerirTurnoActual()
        {
            return DateTime.Now.Hour < 12 ? "Mañana" : "Tarde";
        }
    }
}
