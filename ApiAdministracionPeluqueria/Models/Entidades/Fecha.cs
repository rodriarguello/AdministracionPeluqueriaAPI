using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Fecha
    {

        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public DateTime Dia { get; set; }
    }
}
