using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface ITurnoService
    {
        Task<List<TurnoDTO>> GetAllAsync(int idCalendario, string idUsuario);

        Task<ResTurnosFiltrados> FiltrarPorFechasAsync(int calendarioId, DateTime fechaInicio, DateTime fechaFin, string idUsuario);

        Task ReservarTurnoAsync(int idTurno, TurnoModificarDTO turnoModificarDTO, string idUsuario);

        Task CancelarTurnoAsync(int idTurno, string idUsuario);

        Task ModificarAsistenciaAsync(int idTurno, string idUsuario, bool asistio);

        Task ModificarPrecioAsync(int idTurno, decimal nuevoPrecio, string idUsuario);
    }
}
