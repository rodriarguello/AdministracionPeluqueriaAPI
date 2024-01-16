using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface ICalendarioService
    {

        Task<CalendarioDTO> GetByIdUserAsync(string idUsuario);
        Task<CalendarioDTO> CreateAsync(string idUsuario, CalendarioCreacionDTO nuevoCalendarioDTO);

        Task<CalendarioDTO> UpdateNameAsync(string nuevoNombre, string idUsuario);

        Task<CalendarioDTO> ExtendAsync(DateTime nuevaFechaFin, string idUsuario);

        Task<CalendarioDTO> ReduceAsync(DateTime nuevaFechaFin, string idUsuario);

        Task DeleteAsync(int id, string idUsuario);
    }
}
