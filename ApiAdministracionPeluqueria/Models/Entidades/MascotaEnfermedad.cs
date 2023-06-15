
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class MascotaEnfermedad
    {
        [Required]
        public int IdMascota { get; set; }
        [Required]
        public int IdEnfermedad { get; set; }

        
        public Enfermedad Enfermedad { get; set;}

        public Mascota Mascota { get; set;}
        
        [Required]
        public string IdUsuario { get; set;}

        

    }
}
