

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO
{
    public class TurnoSinMascotaDTO
    {

        public int Id { get; set; }

        public DateTime Fecha { get; set; }


        public TimeSpan Horario { get; set; }

        public bool Disponible { get; set; }

        public bool? Asistio { get; set; }

        public int? Precio { get; set; }

        
        public int IdCalendario { get; set; }

    }
}
