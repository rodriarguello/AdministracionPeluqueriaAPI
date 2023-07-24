using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Mascota
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public int IdCliente { get; set; }

        public Cliente? Cliente { get; set; }

        [Required]
        public int IdRaza { get; set; }

        public Raza? Raza { get; set; }

        public List<MascotaEnfermedad> MascotaEnfermedades { get; set; }

        public List<MascotaAlergia> MascotaAlergias { get; set; }  
        
        public List<Turno> Turnos { get; set; }

        [Required]

        public string IdUsuario { get; set; }
        
        [Required]
        [Column(TypeName = "date")]
        public DateTime FechaCreacion { get; set; }
    }
}
