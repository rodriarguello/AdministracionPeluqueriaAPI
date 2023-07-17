using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.Autenticacion;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
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
        private readonly ResponseApi response;
        private readonly IMapper mapper;

        public CuentasController(ApplicationDbContext context, UserManager<Usuario> userManager, 
            IConfiguration configuration, SignInManager<Usuario> signInManager, ResponseApi response, IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.response = response;
            this.mapper = mapper;
        }


        #region REGISTRO    

        [HttpPost("registrar")]
        
        public async Task<ActionResult<RespuestaAutenticacion>> RegistroUsuario(CreacionUsuarioDTO creacionUsuarioDTO)
        {
            var usuario = new Usuario { UserName = creacionUsuarioDTO.Email,
                                        Email = creacionUsuarioDTO.Email,
                                        Nombres = creacionUsuarioDTO.Nombres,
                                        Apellido = creacionUsuarioDTO.Apellido,
                                        NombrePeluqueria = creacionUsuarioDTO.NombrePeluqueria,
                                        FechaCreacion = DateTime.Now
                                      };

            var resultado = await userManager.CreateAsync(usuario, creacionUsuarioDTO.Password);

            if (resultado.Succeeded) return ConstruirToken(usuario.Email);
            

            else return BadRequest(resultado.Errors);


        }

        #endregion


        #region LOGIN
        [HttpPost("login")]
        public async Task<ActionResult<ModeloRespuesta>> Login(CredencialesUsuario credencialesUsuario)
        {
            var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Email, credencialesUsuario.Password,

            isPersistent: false, lockoutOnFailure: false);

            var respuesta = new
            {
                credenciales = ConstruirToken(credencialesUsuario.Email),
                usuario =  mapper.Map<UsuarioDTO>( await userManager.FindByEmailAsync(credencialesUsuario.Email))
            };


            if (resultado.Succeeded) return response.respuestaExitosa(respuesta);       
        
            else return BadRequest("Login Incorrecto");
        
        }





        #endregion


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<ModeloRespuesta>> MostrarDatosUsuario()
        {   

            var claimEmail = HttpContext.User.Claims.Where(claim=>claim.Type == "email").FirstOrDefault();

            var claimEmailValue = claimEmail.Value;

            var usuario =  await userManager.FindByEmailAsync(claimEmailValue);

            if (usuario == null) return Unauthorized();

            return response.respuestaExitosa(mapper.Map<UsuarioDTO>(usuario));


    
        }
        #region CONSTRUIR TOKEN

        private RespuestaAutenticacion ConstruirToken(string email)
        {
            var claims = new List<Claim>()
            {
                new Claim ("email", email)
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
