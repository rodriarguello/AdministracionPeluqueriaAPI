using ApiAdministracionPeluqueria.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO
{
    public class AlergiaCreacionDTO:INombre
    {
        [Required]
        public string Nombre { get; set; }

    }
}
