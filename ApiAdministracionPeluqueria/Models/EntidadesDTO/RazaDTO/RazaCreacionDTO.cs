using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO
{
    public class RazaCreacionDTO
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string IdUsuario { get; set; }


    }
}
