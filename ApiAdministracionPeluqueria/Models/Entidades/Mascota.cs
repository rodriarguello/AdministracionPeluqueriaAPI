using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Mascota
    {
        [Required]
        [Key]
        public int id { get; set; }

        [Required]
        public string nombre { get; set;}

        [Required]
        public DateTime fechaNacimiento { get; set;}

        [Required]
        public int idRaza { get; set;}

        [Required]
        public int idEnfermedad { get; set;}

        [Required]
        public int idAlergia { get; set;}

        public  int idTurno { get; set;}
    }
}
