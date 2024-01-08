using ApiAdministracionPeluqueria.Models.EntidadesDTO.Autenticacion;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface ITokenService
    {
        RespuestaAutenticacion ConstruirToken(string email, string idUsuario);
    }
}
