using ApiAdministracionPeluqueria.Models.Entidades;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO
{
    public class TurnoDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdFecha { get; set; }

        public Fecha? Fecha { get; set; }

        [Required]
        public int IdHorario { get; set; }

        public Horario? Horario { get; set; }

        [Required]
        public bool Disponible { get; set; }


        public int? IdMascota { get; set; }

        //public Mascota? Mascota { get; set; }

        public bool? Asistio { get; set; }

        public int? Precio { get; set; }

        [Required]
        public int IdCalendario { get; set; }
    }
}
