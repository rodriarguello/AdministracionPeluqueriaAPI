using ApiAdministracionPeluqueria.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO
{
    public class EnfermedadCreacionDTO:INombre
    {
        [Required]
        public string Nombre { get;set; }

        
    }
}
