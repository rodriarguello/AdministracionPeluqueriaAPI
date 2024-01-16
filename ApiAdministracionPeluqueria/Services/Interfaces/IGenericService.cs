
namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface IGenericService<TCreacionDTO, TDTO>
    {
        Task<List<TDTO>> GetAllByIdUserAsync(string idUsuario);

        Task <TDTO> CreateAsync(TCreacionDTO dtoCreacion, string emailUsuario);

        Task<TDTO> UpdateAsync(int idEntidad, TCreacionDTO dtoCreacion, string emailUsuario);

        Task DeleteAsync (int idEntidad, string emailUsuario);
    }
}
