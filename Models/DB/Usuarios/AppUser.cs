using Microsoft.AspNetCore.Identity;

namespace TransportesGenesis.Models.DB.Usuarios
{
    public class AppUser : IdentityUser
    {
        public bool IsFirstLogin { get; set; } = true;
        public DateTime? LastLoginDate { get; set; }
    }
}
