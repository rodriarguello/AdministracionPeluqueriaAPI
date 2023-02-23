using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Raza
    {
        [Required]
        [Key] 
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }



    }
}
