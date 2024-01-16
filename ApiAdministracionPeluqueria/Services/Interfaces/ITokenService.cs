using ApiAdministracionPeluqueria.Models.EntidadesDTO.Auth;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface ITokenService
    {
        string ConstruirToken(string email, string idUsuario);
    }
}
