using ApiAdministracionPeluqueria.Models.EntidadesDTO.Auth;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface ICuentaService
    {
        Task<ResAuth> LoginAsync(string email, string password);

        Task RegisterAsync(CreacionUsuarioDTO creacionUsuarioDTO);
    }
}
