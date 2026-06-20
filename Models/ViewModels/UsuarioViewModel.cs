namespace TransportesGenesis.ViewModels
{
    public class UsuarioViewModel
    {
        public string Email { get; set; }            
        public IEnumerable<string> Roles { get; set; } 
        public DateTime? UltimoLogin { get; set; }    
        public bool Activo { get; set; }               
        public bool PrimerLoginPendiente { get; set; }
    }
}
