using ApiAdministracionPeluqueria.Models.Entidades;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO
{
    public class ClienteCreacionDTO
    {

        [Required]
        public string Nombre { get; set; }
        [Required]
        [Phone]
        public string Telefono { get; set; }

        public string? Mail { get; set; }

        

    }
}
