using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO
{
    public class MascotaDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public int IdCliente { get; set; }

        public ClienteSinMascotasDTO? Cliente { get; set; }

        [Required]
        public int IdRaza { get; set; }

        public RazaDTO.RazaDTO? Raza { get; set; }
        
        [Required]
        public List<int> IdEnfermedades { get; set; }


        public List<EnfermedadDTO.EnfermedadDTO> Enfermedades { get; set; }

        [Required]
        public List<int> IdAlergias { get; set; }
        
        public List<AlergiaDTO.AlergiaDTO> Alergias { get; set; }
       
        public List<TurnoSinMascotaDTO> Turno { get; set; }

       

    }
}
