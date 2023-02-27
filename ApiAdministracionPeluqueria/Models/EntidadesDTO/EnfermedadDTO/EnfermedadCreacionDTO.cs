using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO
{
    public class EnfermedadCreacionDTO
    {
        [Required]
        public string Nombre { get;set; }
    }
}
