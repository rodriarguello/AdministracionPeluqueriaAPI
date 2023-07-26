using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO
{
    public class TurnoDTO
    {
        [Required]
        public int Id { get; set; }
        
        public DateTime Fecha { get; set; }

        
        public TimeSpan Horario { get; set; }

        [Required]
        public bool Disponible { get; set; }

        public MascotaDTO.MascotaNombreFechaNacimientoDTO? Mascota { get; set; }

        public bool? Asistio { get; set; }

        public int? Precio { get; set; }

        [Required]
        public int IdCalendario { get; set; }
    }
}
