using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TransportesGenesis.Pages.Padres
{
    [Authorize(Roles = "PadreDeFamilia")]
    public class DashboardRutaBusAsignadoModel : PageModel
    {
        public string? NombreHijo { get; set; }
        public string? BusAsignado { get; set; }
        public string? RutaNombre { get; set; }
        public string? ParadaAsignada { get; set; }
        public string? HorarioMañana { get; set; }
        public string? HorarioTarde { get; set; }
        public bool EsFinDeSemana { get; set; }
        public bool MostrarRutaMañana { get; set; }
        public bool MostrarRutaTarde { get; set; }

        public void OnGet()
        {
            // Datos simulados del hijo y bus asignado
            NombreHijo = "Juan Pérez Rodríguez";
            BusAsignado = "Bus #001 - GT-001-A";
            RutaNombre = "Ruta Centro - Yulimay PC";
            ParadaAsignada = "Parada Central - 6ta Avenida";
            HorarioMañana = "6:30 AM";
            HorarioTarde = "2:15 PM";

            // Verificar día de la semana
            var ahora = DateTime.Now;
            EsFinDeSemana = ahora.DayOfWeek == DayOfWeek.Saturday || ahora.DayOfWeek == DayOfWeek.Sunday;

            // Verificar horarios para mostrar rutas
            var hora = ahora.Hour;
            MostrarRutaMañana = !EsFinDeSemana && hora >= 5 && hora <= 8;
            MostrarRutaTarde = !EsFinDeSemana && hora >= 12 && hora <= 15;
        }
    }
}