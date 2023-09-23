using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Cliente
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string IdUsuario { get; set; }

        public List<Mascota>? Mascotas { get; set; }
        
        [Required]
        [Column(TypeName ="date")]
        public DateTime FechaCreacion { get; set; }

    }
}
