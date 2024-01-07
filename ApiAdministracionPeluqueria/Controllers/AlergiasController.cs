using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models;
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
        public async Task<ActionResult<ModeloRespuesta>> Get()
        {

            try
            {
                var claimId = HttpContext.User.Claims.Where(claim => claim.Type == "id").FirstOrDefault();

                var idUsuario = claimId.Value;

                var alergias = await _alergiaService.GetAllByIdUser(idUsuario);
                
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
        public async Task<ActionResult<ModeloRespuesta>> Post([FromBody]AlergiaCreacionDTO nuevaAlergiaDTO)
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var nuevaAlergia = await _alergiaService.Create(nuevaAlergiaDTO, email);
                

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
        public async Task<ActionResult<ModeloRespuesta>> Put([FromBody] AlergiaCreacionDTO alergiaCreacionDTO, [FromRoute] int id)
        {
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;

                var alergiaModificada = await _alergiaService.Update(id, alergiaCreacionDTO, email);

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
        public async Task<ActionResult<ModeloRespuesta>> Delete([FromRoute]int id)
        {

            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;

                await _alergiaService.Delete(id,email);

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




    }
}
