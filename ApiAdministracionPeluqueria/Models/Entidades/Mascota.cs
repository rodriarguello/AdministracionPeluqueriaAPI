using System.ComponentModel.DataAnnotations;

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
    }
}
