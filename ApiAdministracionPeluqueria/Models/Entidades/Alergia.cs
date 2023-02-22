using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Alergia
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        public string nombre { get; set; }


    }
}
