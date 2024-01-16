using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/mascotas")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MascotasController : ControllerBase
    {
        private readonly IMascotaService _mascotaService;

        public MascotasController(IMascotaService mascotaService)
        {
            _mascotaService = mascotaService;
        }


        private string ExtraerClaim(string tipoClaim)
        {
            var claim = HttpContext.User.Claims.Where(c => c.Type == tipoClaim).FirstOrDefault();

            return claim.Value;
        }

        [HttpGet]
        public async Task<ActionResult<List<MascotaDTO>>> GetAll()
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var mascotas = await _mascotaService.GetAllByIdUserAsync(idUsuario);

                return Ok(mascotas);

            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<MascotaDTO>> MostrarUnaMascota([FromRoute]int id)
        {
            try
            {
                var idUsuario = ExtraerClaim("id");

                var mascotas = await _mascotaService.GetByIdAsync(id, idUsuario);

                return Ok(mascotas);

            }
            catch(NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "Error interno del servidor");
            }


        }


        [HttpPost]
        public async Task<ActionResult<MascotaDTO>> Post([FromBody]MascotaCreacionDTO nuevaMascotaDTO)
        {
            try
            {
                var emailUsuario = ExtraerClaim("email");

                var mascotas = await _mascotaService.CreateAsync(nuevaMascotaDTO, emailUsuario);

                return Ok(mascotas);

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

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> ModificarMascota([FromRoute]int id, [FromBody]MascotaModificarDTO mascotaDTO)
        {
            try
            {
                var emailUsuario = ExtraerClaim("email");

                var mascota = await _mascotaService.UpdateAsync(id, mascotaDTO, emailUsuario);

                return Ok(mascota);

            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
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

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> EliminarMascota([FromRoute]int id)
        {
            try
            {
                var emailUsuario = ExtraerClaim("email");

                await _mascotaService.DeleteAsync(id, emailUsuario);

                return NoContent();
            }
            catch (MensajePersonalizadoException ex)
            {
                return StatusCode(499, ex.Message);
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
    }
}
