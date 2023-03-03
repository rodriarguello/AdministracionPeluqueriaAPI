using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/enfermedades")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EnfermedadesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        #region Constructor
        public EnfermedadesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        #endregion



        #region Metodos GET

        [HttpGet]
        public async Task<ActionResult<List<EnfermedadDTO>>> GetEnfermedades()
        {

            var enfermedades = await context.Enfermedades.ToListAsync();

            return mapper.Map<List<EnfermedadDTO>>(enfermedades);

        }



        #endregion


        #region Metodos POST

        [HttpPost]
        public async Task<ActionResult<EnfermedadDTO>> PostEnfermedad([FromBody] EnfermedadCreacionDTO nuevaEnfermedadDTO)
        {

            var nuevaEnfermedad = mapper.Map<Enfermedad>(nuevaEnfermedadDTO);

            context.Add(nuevaEnfermedad);
            await context.SaveChangesAsync();

            return mapper.Map<EnfermedadDTO>(nuevaEnfermedad);
        }

        #endregion



        #region Metodos PUT

        [HttpPut]
        public async Task<ActionResult> PutEnfermedad([FromBody] EnfermedadDTO enfermedadDTO)
        {
            bool existe = await context.Enfermedades.AnyAsync(enfermedad => enfermedad.Id == enfermedadDTO.Id);

            if (!existe) return NotFound();

            var enfermedad = mapper.Map<Enfermedad>(enfermedadDTO);

            context.Update(enfermedad);
            await context.SaveChangesAsync();


            return NoContent();
        }

        #endregion



        #region Metodos DELETE
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEnfermedad([FromRoute] int id)
        {
            bool existe = await context.Enfermedades.AnyAsync(enfermedad => enfermedad.Id == id);

            if (!existe) return NotFound();

            var enfermedad = await context.Enfermedades.FirstOrDefaultAsync(enfermedad => enfermedad.Id == id);

            context.Remove(enfermedad);
            await context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

    }
}
