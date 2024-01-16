using Microsoft.AspNetCore.Mvc;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ApiAdministracionPeluqueria.Services.Interfaces;
using ApiAdministracionPeluqueria.Exceptions;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/enfermedades")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EnfermedadesController : ControllerBase
    {
        private readonly IGenericService<EnfermedadCreacionDTO, EnfermedadDTO> _enfermedadService;

        #region Constructor
        public EnfermedadesController(IGenericService<EnfermedadCreacionDTO, EnfermedadDTO> enfermedadService)
        {
            _enfermedadService = enfermedadService;
        }

        #endregion



        #region MOSTRAR ENFERMEDADES

        [HttpGet]
        public async Task<ActionResult<List<EnfermedadDTO>>> Get()
        {

            try
            {
                var id = ExtraerClaim("id");

                var enfermedades = await _enfermedadService.GetAllByIdUserAsync(id);
    
                return Ok(enfermedades);

            }
            catch (Exception ex)
            {
                return StatusCode(500,"Error interno del servidor");
            }

        }



        #endregion


        #region INSERTAR ENFERMEDAD

        [HttpPost]
        public async Task<ActionResult<EnfermedadDTO>> Post([FromBody] EnfermedadCreacionDTO nuevaEnfermedadDTO)
        {

            try
            {
                var email = ExtraerClaim("email");

                var nuevaEnfermedad = await _enfermedadService.CreateAsync(nuevaEnfermedadDTO, email);


                return Ok(nuevaEnfermedad);
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



        #region MODIFICAR ENFERMEDAD

        [HttpPut("{id:int}")]
        public async Task<ActionResult<EnfermedadDTO>> Put([FromBody] EnfermedadCreacionDTO enfermedadDTO, [FromRoute]int id)
        {

            try
            {
                var email = ExtraerClaim("email");

                var enfermedadModificada = await _enfermedadService.UpdateAsync(id, enfermedadDTO, email);

                return Ok(enfermedadModificada);
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



        #region ELIMINAR ENFERMEDAD
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {

            try
            {

                var email = ExtraerClaim("email");

                await _enfermedadService.DeleteAsync(id, email);

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
