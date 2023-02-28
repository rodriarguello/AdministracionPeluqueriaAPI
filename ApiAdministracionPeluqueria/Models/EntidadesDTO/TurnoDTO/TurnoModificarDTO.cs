using ApiAdministracionPeluqueria.Models.Entidades;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO
{
    public class TurnoModificarDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int IdFecha { get; set; }

        [Required]
        public int IdHorario { get; set; }

        [Required]
        public bool Disponible { get; set; }

        [Required]
        public int IdMascota { get; set; }

        [Required]
        public bool Asistio { get; set; }

        [Required]
        public int Precio { get; set; }
    }
}
