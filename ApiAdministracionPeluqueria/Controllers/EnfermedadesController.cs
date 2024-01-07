using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ApiAdministracionPeluqueria.Services.Interfaces;
using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Services;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;

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
                var claimId = HttpContext.User.Claims.Where(claim => claim.Type == "id").FirstOrDefault();

                var id = claimId.Value;

                var enfermedades = await _enfermedadService.GetAllByIdUser(id);
    
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
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;

                var nuevaEnfermedad = await _enfermedadService.Create(nuevaEnfermedadDTO, email);


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

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;

                var enfermedadModificada = await _enfermedadService.Update(id, enfermedadDTO, email);

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

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;

                await _enfermedadService.Delete(id, email);

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
