
namespace ApiAdministracionPeluqueria.Services.Interfaces
{
    public interface IGenericService<TCreacionDTO, TDTO>
    {
        Task<List<TDTO>> GetAllByIdUser(string idUsuario);

        Task <TDTO> Create(TCreacionDTO dtoCreacion, string emailUsuario);

        Task<TDTO> Update(int idEntidad, TCreacionDTO dtoCreacion, string email);

        Task Delete (int idEntidad, string emailUsuario);
    }
}
