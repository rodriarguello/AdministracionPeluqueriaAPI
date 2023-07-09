using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Turno
    {
        public Turno()
        {

        }

        public Turno (int idFecha, int idHorario, bool disponible, bool asistio, int idCalendario, string idUsuario)
        {
            IdFecha = idFecha;
            IdHorario = idHorario;
            Disponible = disponible;
            Asistio = asistio;
            IdCalendario = idCalendario;
            IdUsuario = idUsuario;
        }

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

        
        public int? IdMascota { get; set; }

        public Mascota? Mascota { get; set; }

        public bool? Asistio { get; set; }

        [Column(TypeName ="decimal(18,2)")]
        public decimal? Precio { get; set; }

        [Required]
        public int IdCalendario { get; set; }

        public Calendario Calendario { get; set; }

        [Required]

        public string IdUsuario { get; set; }

        


    }
}
