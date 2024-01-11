using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/alergias")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AlergiasController : ControllerBase
    {
        private readonly IGenericService<AlergiaCreacionDTO, AlergiaDTO> _alergiaService;



        #region Constructor
        public AlergiasController(IGenericService<AlergiaCreacionDTO,AlergiaDTO> alergiaService)
        {
            _alergiaService = alergiaService;
        }

        #endregion



        #region MOSTRAR ALERGIAS

        [HttpGet]
        public async Task<ActionResult<List<AlergiaDTO>>> Get()
        {

            try
            {
                var idUsuario = ExtraerClaim("id");

                var alergias = await _alergiaService.GetAllByIdUserAsync(idUsuario);
                
                return Ok(alergias);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
            

        }



        #endregion


        #region INSERTAR ALERGIA

        [HttpPost]
        public async Task<ActionResult<AlergiaDTO>> Post([FromBody]AlergiaCreacionDTO nuevaAlergiaDTO)
        {
            try
            {
                var email = ExtraerClaim("email");

                var nuevaAlergia = await _alergiaService.CreateAsync(nuevaAlergiaDTO, email);
                

                return Ok(nuevaAlergia);
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

                return StatusCode(500,"Error interno del servidor");
 
            }
        }

        #endregion



        #region MODIFICAR ALERGIA

        [HttpPut("{id:int}")]
        public async Task<ActionResult<AlergiaDTO>> Put([FromBody] AlergiaCreacionDTO alergiaCreacionDTO, [FromRoute] int id)
        {
            try
            {

                var email = ExtraerClaim("email");

                var alergiaModificada = await _alergiaService.UpdateAsync(id, alergiaCreacionDTO, email);

                return Ok(alergiaModificada);
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



        #region ELIMINAR ALERGIA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {

            try
            {
                var email = ExtraerClaim("email");

                await _alergiaService.DeleteAsync(id,email);

                return NoContent();
            
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
