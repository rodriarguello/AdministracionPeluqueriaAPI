using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Cliente
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        public string nombre { get; set; }

        public string telefono { get; set; }

        public string mail { get; set; }

        public int idMascota { get; set; }

    }
}
