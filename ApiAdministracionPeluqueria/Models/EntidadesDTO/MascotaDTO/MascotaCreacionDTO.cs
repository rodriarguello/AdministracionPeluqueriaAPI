using ApiAdministracionPeluqueria.Models.Entidades;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO
{
    public class MascotaCreacionDTO
    {

        

        [Required]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public int IdCliente { get; set; }

       
        [Required]
        public int IdRaza { get; set; }

       
        [Required]
        public int IdEnfermedad { get; set; }

       

        [Required]
        public int IdAlergia { get; set; }

      
    

    }
}
