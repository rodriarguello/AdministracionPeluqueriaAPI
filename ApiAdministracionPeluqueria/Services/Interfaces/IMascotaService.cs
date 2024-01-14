using ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO;

namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface IMascotaService
    {
        Task<List<MascotaDTO>> GetAllByIdUserAsync(string idUsuario);
        Task<MascotaDTO> GetByIdAsync(int id, string idUsuario);
        Task<MascotaDTO> CreateAsync(MascotaCreacionDTO nuevaMascotaDTO, string emailUsuario);

        Task<MascotaDTO> UpdateAsync(int idEntidad, MascotaModificarDTO mascotaDTO, string emailUsuario);

        Task DeleteAsync(int idEntidad, string emailUsuario);
    }
}
