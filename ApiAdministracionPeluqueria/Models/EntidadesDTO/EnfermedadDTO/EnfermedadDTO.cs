using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO
{
    public class EnfermedadDTO
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }


    }
}
