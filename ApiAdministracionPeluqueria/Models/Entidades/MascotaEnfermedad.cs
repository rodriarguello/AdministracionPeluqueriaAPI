
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class MascotaEnfermedad
    {
        [Required]
        public int MascotaId { get; set; }
        [Required]
        public int EnfermedadId { get; set; }

        
        public Enfermedad Enfermedad { get; set;}

        public Mascota Mascota { get; set;}
        
        [Required]
        public string IdUsuario { get; set;}

        

    }
}
