using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Usuario : IdentityUser
    {
        [MaxLength(50)]
        public string? Nombres { get; set; }
        [MaxLength(50)]
        public string? Apellido { get; set;}
        [MaxLength(50)]
        public string? NombrePeluqueria { get; set;}

    }
}
