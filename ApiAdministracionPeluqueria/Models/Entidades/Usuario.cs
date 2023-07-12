using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Usuario : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string Nombres { get; set; }
        [Required]
        [MaxLength(50)]
        public string Apellido { get; set;}
        [Required]
        [MaxLength(50)]
        public string NombrePeluqueria { get; set;}
        [Required]
        public DateTime FechaCreacion { get; set;}

    }
}
