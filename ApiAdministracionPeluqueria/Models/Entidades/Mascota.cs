using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Mascota
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set;}

        [Required]
        public DateTime FechaNacimiento { get; set;}

        [Required]
        public int IdCliente { get; set;}

        public Cliente Cliente { get; set;}

        [Required]
        public int IdRaza { get; set;}

        public Raza Raza { get; set;}

        [Required]
        public int IdEnfermedad { get; set;}

        public List<Enfermedad> Enfermedad { get; set;}

        [Required]
        public int IdAlergia { get; set;}

        public List<Alergia> Alergia { get; set;}

        public  int? IdTurno { get; set;}

        public List<Turno>? Turno { get; set;}

        
    }
}
