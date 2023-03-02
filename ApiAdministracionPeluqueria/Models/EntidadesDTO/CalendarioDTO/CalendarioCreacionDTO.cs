using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO
{
    public class CalendarioCreacionDTO
    {
        
        [Required]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required]
        public int HoraInicioTurnos { get; set; }

        [Required]
        public int HoraFinTurnos { get; set; }

        [Required]
        public int IntervaloTurnos { get; set; }

        [Required]

        public int IdAdministrador { get; set; }

    }
}
