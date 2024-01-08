using ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface IUserService
    {
        Task<UsuarioDTO> GetByEmailAsync (string email);

        Task CreateAsync(CreacionUsuarioDTO creacionUsuarioDTO);
    }
}
