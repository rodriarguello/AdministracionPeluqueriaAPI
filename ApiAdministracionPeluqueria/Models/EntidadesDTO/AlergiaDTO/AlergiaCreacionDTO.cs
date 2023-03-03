using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO
{
    public class AlergiaCreacionDTO
    {



        [Required]

        public string Nombre { get; set; }

        [Required]
        public string IdUsuario { get; set; }

    }
}
