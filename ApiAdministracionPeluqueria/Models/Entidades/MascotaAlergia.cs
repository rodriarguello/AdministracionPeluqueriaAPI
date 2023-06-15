using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class MascotaAlergia
    {

        [Required]
        public int IdMascota { get; set; }
        public int IdAlergia { get; set; }

        public Mascota Mascota { get; set; }

        public Alergia Alergia { get; set; }

        [Required]
        public string IdUsuario { get; set; }
    }
}
