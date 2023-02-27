using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/alergias")]
    [ApiController]
    public class AlergiasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        #region Constructor
        public AlergiasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        #endregion



        #region Metodos GET

        [HttpGet]
        public async Task<ActionResult<List<AlergiaDTO>>> GetAlergias()
        {

            var alergias = await context.Alergias.ToListAsync();

            return mapper.Map<List<AlergiaDTO>>(alergias);

        }



        #endregion


        #region Metodos POST

        [HttpPost]
        public async Task<ActionResult<AlergiaDTO>> PostAlergia([FromBody]AlergiaCreacionDTO nuevaAlergiaDTO)
        {

            var nuevaAlergia = mapper.Map<Alergia>(nuevaAlergiaDTO);

            context.Add(nuevaAlergia);
            await context.SaveChangesAsync();

            return mapper.Map<AlergiaDTO>(nuevaAlergia);
        }

        #endregion



        #region Metodos PUT

        [HttpPut]
        public async Task<ActionResult> PutAlergia([FromBody] AlergiaDTO alergiaDTO)
        {
            bool existe = await context.Alergias.AnyAsync(alergia=> alergia.Id == alergiaDTO.Id);

            if (!existe) return NotFound();

            var alergia = mapper.Map<Alergia>(alergiaDTO);

            context.Update(alergia);
            await context.SaveChangesAsync();


            return NoContent();
        }

        #endregion



        #region Metodos DELETE
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAlergia([FromRoute]int id)
        {
            bool existe = await context.Alergias.AnyAsync(alergia => alergia.Id == id);

            if(!existe) return NotFound();

            var alergia = await context.Alergias.FirstOrDefaultAsync(alergia => alergia.Id == id);

            context.Remove(alergia);
            await context.SaveChangesAsync();

            return NoContent();
        }

        #endregion


    }
}
