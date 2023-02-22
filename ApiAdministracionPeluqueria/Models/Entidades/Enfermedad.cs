using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Enfermedad
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        public string nombre { get; set; }


    }
}
