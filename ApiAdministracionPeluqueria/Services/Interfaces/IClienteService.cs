using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface IClienteService:IGenericService<ClienteCreacionDTO, ClienteSinMascotasDTO>
    {

        Task<ClienteDTO> GetByIdAsync(int idCliente, string idUsuario);

        Task<List<TurnoDTO>> GetTurnosTomadosAsync(int idCliente, string idUsuario);

    }
}
