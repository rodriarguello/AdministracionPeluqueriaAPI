using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/turnos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TurnosController : ControllerBase
    {
        private readonly ITurnoService _turnoService;

        #region Constructor
        public TurnosController(ITurnoService turnoService)
        {
            _turnoService = turnoService;
        }


        #endregion

        

        #region MOSTRAR TURNOS

        [HttpGet("{calendarioId:int}")]

        public async Task<ActionResult<List<TurnoDTO>>> GetAll(int calendarioId)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var turnos = await _turnoService.GetAllAsync(calendarioId, idUsuario);

                return Ok(turnos);
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


        [HttpGet("filtrar/{calendarioId:int},{fechaInicio},{fechaFin}")]

        public async Task<ActionResult<ResTurnosFiltrados>> FiltrarPorFechas([FromRoute] int calendarioId, DateTime fechaInicio,DateTime fechaFin)
        {
            try
            {

                var idUsuario = ExtraerClaim("id");

                var turnos = await _turnoService.FiltrarPorFechasAsync(calendarioId, fechaInicio, fechaFin, idUsuario);

                return Ok(turnos);

            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor");
            }

        }

        #endregion

        #region MODIFICAR TURNO


        [HttpPut("reservar/{id:int}")]
        public async Task<ActionResult> ReservarTurno([FromRoute] int id , [FromBody] TurnoModificarDTO turnoDTO)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                await _turnoService.ReservarTurnoAsync(id, turnoDTO, idUsuario);

                return NoContent();

            }
            catch(BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }




        [HttpPut("cancelar/{id:int}")]
        public async Task<ActionResult> CancelarReserva([FromRoute]int id)
        {

            try
            {
                var idUsuario = ExtraerClaim("id");

                await _turnoService.CancelarTurnoAsync(id, idUsuario);

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


        [HttpPut("asistencia/{id:int}")]
        public async Task<ActionResult> ModificarAsistencia([FromRoute] int id, [FromBody] bool asistio)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                await _turnoService.ModificarAsistenciaAsync(id, idUsuario, asistio);

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

        [HttpPut("precio/{id:int}")]
        public async Task<ActionResult> ModificarPrecio([FromRoute] int id, [FromBody] int nuevoPrecio)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                await _turnoService.ModificarPrecioAsync(id, nuevoPrecio, idUsuario);

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

        private string ExtraerClaim(string tipoClaim)
        {
            var claim = HttpContext.User.Claims.Where(claim => claim.Type == tipoClaim).FirstOrDefault();
            
            return claim.Value;
        }



    }
}
