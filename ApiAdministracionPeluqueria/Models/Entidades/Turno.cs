using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Turno
    {


        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int IdFecha { get; set; }

        public Fecha Fecha { get; set; }

        [Required]
        public int IdHorario { get; set; }

        public Horario Horario { get; set;}

        [Required]
        public bool Disponible { get; set; }

        [Required]
        public int IdMascota { get; set; }

        public Mascota Mascota { get; set; }

        public bool Asistio { get; set; }

    }
}
