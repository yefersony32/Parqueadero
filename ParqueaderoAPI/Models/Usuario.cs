using System.ComponentModel.DataAnnotations;

namespace ParqueaderoAPI.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }

        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        public string ContraseñaHash { get; set; }

        public string Rol { get; set; } = "Admin"; 

        public DateTime FechaCreacion { get; set; }
    }
}
