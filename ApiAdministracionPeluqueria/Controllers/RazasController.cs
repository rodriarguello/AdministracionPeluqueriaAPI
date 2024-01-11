using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/razas")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RazasController : ControllerBase
    {
        private readonly IGenericService<RazaCreacionDTO, RazaDTO> _razaService;

        #region Constructor


        public RazasController(IGenericService<RazaCreacionDTO,RazaDTO> razaService)
        {
            _razaService = razaService;
        }

        #endregion



        #region MOSTRAR RAZAS
        [HttpGet]
        public async Task<ActionResult<List<RazaDTO>>> Get()
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var razas = await _razaService.GetAllByIdUserAsync(idUsuario);

                return Ok(razas);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }

        }

        #endregion


        #region INSERTAR RAZA


        [HttpPost]
        public async Task<ActionResult<RazaDTO>> PostRaza([FromBody]RazaCreacionDTO nuevaRazaDTO)
        {

            try
            {
                var email = ExtraerClaim("email");

                var nuevaRaza = await _razaService.CreateAsync(nuevaRazaDTO, email);


                return Ok(nuevaRaza);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MensajePersonalizadoException ex)
            {
                return StatusCode(499, ex.Message);
            }
            catch (Exception)
            {

                return StatusCode(500, "Error interno del servidor");

            }

        }

        #endregion



        #region MODIFICAR RAZA
        [HttpPut("{id:int}")]
        public async Task<ActionResult<RazaDTO>> PutRaza([FromBody] RazaCreacionDTO razaDTO, [FromRoute] int id)
        {
            try
            {
                var email = ExtraerClaim("email");

                var razaModificada = await _razaService.UpdateAsync(id, razaDTO, email);

                return Ok(razaModificada);
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



        #region ELIMINAR RAZA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteRaza([FromRoute]int id)
        {


            try
            {
                var email = ExtraerClaim("email");

                await _razaService.DeleteAsync(id, email);

                return NoContent();

            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (MensajePersonalizadoException ex)
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
