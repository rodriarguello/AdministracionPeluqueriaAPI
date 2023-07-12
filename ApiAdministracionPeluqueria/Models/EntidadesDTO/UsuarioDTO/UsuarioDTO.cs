
namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO
{
    public class UsuarioDTO
    {
        public string Email { get; set; }
        
        public string Nombres { get; set; }
        
        public string Apellido { get; set; }
        
        public string NombrePeluqueria { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
