using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO
{
    public class CreacionUsuarioDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

        [Required]
        
        public string Nombres { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        public string NombrePeluqueria { get; set; }
        
    }
}
