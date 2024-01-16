using ApiAdministracionPeluqueria.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO
{
    public class RazaCreacionDTO:INombre
    {
        [Required]
        public string Nombre { get; set; }


    }
}
