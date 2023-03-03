namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.Autenticacion
{
    public class RespuestaAutenticacion
    {
        public string Token { get; set; }

        public DateTime Expiracion { get; set; }
    }
}
