namespace ApiAdministracionPeluqueria.Models.Interfaces
{
    public interface IIdUsuario:IId,INombre
    {
        string IdUsuario { get; set; }
    }
}
