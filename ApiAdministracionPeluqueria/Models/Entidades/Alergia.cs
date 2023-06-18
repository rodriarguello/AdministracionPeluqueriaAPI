using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Alergia
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string IdUsuario { get; set; }


        public List<MascotaAlergia> MascotasAlergia { get; set; }

    }
}
