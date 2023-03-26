
using ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO
{
    public class ClienteDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Telefono { get; set; }


        public string? Email { get; set; }

        public List<MascotaSinClienteDTO> Mascotas { get; set; }


    }
}
