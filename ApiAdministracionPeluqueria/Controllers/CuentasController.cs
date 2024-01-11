using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.Auth;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentaService _cuentaService;
        private readonly IUserService _userService;

        public CuentasController(ICuentaService cuentaService, IUserService userService)
        {
            _cuentaService = cuentaService;
            _userService = userService;
        }


        #region REGISTRO    

        [HttpPost("registrar")]
        
        public async Task<ActionResult> Registro(CreacionUsuarioDTO creacionUsuarioDTO)
        {
            try
            {
                await _cuentaService.RegisterAsync(creacionUsuarioDTO);

                return NoContent();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }

        #endregion


        #region LOGIN
        [HttpPost("login")]
        public async Task<ActionResult<ResAuth>> Login(CredencialesUsuario credencialesUsuario)
        {
            try
            {
                var respuesta = await _cuentaService.LoginAsync(credencialesUsuario.Email, credencialesUsuario.Password);

                return Ok(respuesta);
            }
            catch (BadRequestException)
            {
                return BadRequest("Credenciales inválidas");
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }


        #endregion


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<UsuarioDTO>> GetDatosUsuario()
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim=>claim.Type == "email").FirstOrDefault();

                var claimEmailValue = claimEmail.Value;

                var usuario =  await _userService.GetDtoByEmailAsync(claimEmailValue);

                if (usuario == null) return Unauthorized();

                return Ok(usuario);

            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }

        }

    }
}
