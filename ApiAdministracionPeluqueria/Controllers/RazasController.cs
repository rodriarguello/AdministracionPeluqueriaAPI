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
                var claimId = HttpContext.User.Claims.Where(claim => claim.Type == "id").FirstOrDefault();
                var idUsuario = claimId.Value;

                var razas = await _razaService.GetAllByIdUser(idUsuario);

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
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var nuevaRaza = await _razaService.Create(nuevaRazaDTO, email);


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

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;

                var razaModificada = await _razaService.Update(id, razaDTO, email);

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

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;

                await _razaService.Delete(id, email);

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

    }
}
