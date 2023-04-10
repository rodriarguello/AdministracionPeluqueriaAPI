using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.HorarioDTO
{
    public class HorarioSinCalendarioDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }
    }
}
