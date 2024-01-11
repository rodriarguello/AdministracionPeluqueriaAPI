using ApiAdministracionPeluqueria.Models.EntidadesDTO.IngresoDTO;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface ICajaService
    {
        Task<ResIngresos> GetIngresoAnualAsync(int anio, string idUsuario);

        Task<ResIngresos> GetIngresoMensualAsync(int anio, int mes, string idUsuario);

        Task<ResIngresos> GetIngresoDiarioAsync(int anio, int mes, int dia, string idUsuario);

        Task CrearIngresoAsync(DateTime fecha, decimal precio, string usuarioId, int idTurno);

        Task EliminarIngresoAsync(int idTurno, string idUsuario);

        Task ModificarPrecioIngresoAsync(int idTurno, string idUsuario, decimal nuevoPrecio);


    }
}
