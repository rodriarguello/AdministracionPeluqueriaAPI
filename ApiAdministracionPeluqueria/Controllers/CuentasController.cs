using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.Autenticacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Usuario> userManager;
        private readonly IConfiguration configuration;
        private readonly SignInManager<Usuario> signInManager;

        public CuentasController(ApplicationDbContext context, UserManager<Usuario> userManager, 
            IConfiguration configuration, SignInManager<Usuario> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
        }


        #region REGISTRO    

        [HttpPost("registrar")]
        
        public async Task<ActionResult<RespuestaAutenticacion>> RegistroUsuario(CredencialesUsuario credencialesUsuario)
        {
            var usuario = new Usuario { UserName = credencialesUsuario.Email, Email = credencialesUsuario.Email };

            var resultado = await userManager.CreateAsync(usuario, credencialesUsuario.Password);

            if (resultado.Succeeded) return ConstruirToken(credencialesUsuario);
            

            else return BadRequest(resultado.Errors);


        }

        #endregion


        #region LOGIN
        [HttpPost("login")]
        public async Task<ActionResult<RespuestaAutenticacion>> Login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password,

            isPersistent: false, lockoutOnFailure: false);


            if (resultado.Succeeded) return ConstruirToken(credencialesUsuario);       
        
            else return BadRequest("Login Incorrecto");
        
        }





        #endregion

        #region CONSTRUIR TOKEN

        private RespuestaAutenticacion ConstruirToken(CredencialesUsuario credencialesUsuario)
        {
            var claims = new List<Claim>()
            {
                new Claim ("email", credencialesUsuario.Email)
            };


            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llaveJwt"]));

            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);

            var token = new JwtSecurityToken(issuer:null, audience:null,claims:claims,expires:expiracion, signingCredentials:creds);

            return new RespuestaAutenticacion() 
            { Token = new JwtSecurityTokenHandler().WriteToken(token), 
            Expiracion= expiracion};


        }

        #endregion
    }
}
