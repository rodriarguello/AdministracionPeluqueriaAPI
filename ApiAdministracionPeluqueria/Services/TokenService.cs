using ApiAdministracionPeluqueria.Models.EntidadesDTO.Autenticacion;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiAdministracionPeluqueria.Services
{
    public class TokenService:ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public RespuestaAutenticacion ConstruirToken(string email, string idUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim ("email", email),
                new Claim ("id",idUsuario)
            };


            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["llaveJwt"]));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new RespuestaAutenticacion()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiracion = expiracion
            };


        }
    }
}
