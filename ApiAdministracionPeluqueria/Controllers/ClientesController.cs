using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;


        #region Constructor
        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }
        #endregion



        #region MOSTRAR CLIENTES

        [HttpGet]
        public async Task<ActionResult<List<ClienteSinMascotasDTO>>> GetAll()
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var clientes = await _clienteService.GetAllByIdUserAsync(idUsuario);

                return Ok(clientes);

            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClienteDTO>> GetPorId([FromRoute]int id)
        {
                try
                {
                    var idUsuario = ExtraerClaim("id");

                    var clientes = await _clienteService.GetByIdAsync(id, idUsuario);

                    return Ok(clientes);

                }
                catch (NotFoundException)
                {
                    return NotFound();
                }
                catch (Exception)
                {
                    return StatusCode(500, "Error interno del servidor");
                }
        }

        [HttpGet("turnos/{idCliente:int}")]
        public async Task<ActionResult<List<TurnoDTO>>> GetTurnosTomados([FromRoute] int idCliente)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var turnos = await _clienteService.GetTurnosTomadosAsync(idCliente, idUsuario);

                return Ok(turnos);

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }


        }


        #endregion



        #region INSERTAR CLIENTE

        [HttpPost]
        public async Task<ActionResult<ClienteSinMascotasDTO>> Post([FromBody]ClienteCreacionDTO nuevoClienteDTO)
        {

            try
            {

                var email = ExtraerClaim("email");

                var cliente = await _clienteService.CreateAsync(nuevoClienteDTO, email);

                return Ok(cliente);
                
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



        #region MODIFICAR CLIENTES

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ClienteSinMascotasDTO>> Put([FromRoute]int id, [FromBody]ClienteCreacionDTO clienteDTO)
        {
            try
            {

                var email = ExtraerClaim("email");

                var cliente = await _clienteService.UpdateAsync(id, clienteDTO, email);

                return Ok(cliente);

            }
            catch(NotFoundException)
            {
                return NotFound();
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



        #region ELIMINAR CLIENTE

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var email = ExtraerClaim("email");

                await _clienteService.DeleteAsync(id, email);

                return NoContent();

            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(MensajePersonalizadoException ex)
            {
                return StatusCode(499, ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }

        }

        #endregion

        private string ExtraerClaim(string tipoClaim)
        {
            var claim = HttpContext.User.Claims.Where(claim => claim.Type == tipoClaim).FirstOrDefault();

            return claim.Value;
        }


    }
}
