using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface IUserService
    {
        Task<UsuarioDTO> GetDtoByEmailAsync (string email);

        Task CreateAsync(CreacionUsuarioDTO creacionUsuarioDTO);

        Task<Usuario> GetByIdAsync(string id);
    }
}
