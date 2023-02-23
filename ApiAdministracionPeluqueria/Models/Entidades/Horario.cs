using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Horario
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public TimeSpan Hora { get; set; }
    }
}
