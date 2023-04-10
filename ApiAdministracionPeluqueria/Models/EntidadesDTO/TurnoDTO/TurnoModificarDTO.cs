using ApiAdministracionPeluqueria.Models.Entidades;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO
{
    public class TurnoModificarDTO
    {
        
        
        public int IdMascota { get; set; }


        [Required]
        public int Precio { get; set; }
    }
}
