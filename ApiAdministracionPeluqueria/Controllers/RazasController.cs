using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/razas")]
    [ApiController]
    public class RazasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        #region Constructor


        public RazasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        #endregion



        #region Metodos GET
        [HttpGet]
        public async Task<ActionResult<List<RazaDTO>>> GetRazas()
        {
            var razas = await context.Razas.ToListAsync();

            return mapper.Map<List<RazaDTO>>(razas);

        }

        #endregion


        #region Metodos POST


        [HttpPost]
        public async Task<ActionResult<RazaDTO>> PostRaza([FromBody]RazaCreacionDTO nuevaRazaDTO)
        {

            var nuevaRaza = mapper.Map<Raza>(nuevaRazaDTO);

            context.Add(nuevaRaza);
            await context.SaveChangesAsync();

            return mapper.Map<RazaDTO>(nuevaRaza);
            
        }

        #endregion



        #region Metodos PUT
        [HttpPut]
        public async Task<ActionResult<RazaDTO>> PutRaza([FromBody] RazaDTO razaDTO)
        {

            bool existe = await context.Razas.AnyAsync(raza => raza.Id == razaDTO.Id);

            if (!existe) return NotFound();

            var raza = mapper.Map<Raza>(razaDTO);

            context.Update(raza);
            
            await context.SaveChangesAsync();

            return NoContent();
        }


        #endregion



        #region Metodos DELETE
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteRaza([FromRoute]int id)
        {
            bool existe = await context.Razas.AnyAsync(raza => raza.Id == id);

            if (!existe) return NotFound();

            var raza = await context.Razas.FirstOrDefaultAsync(raza=> raza.Id == id);

            context.Razas.Remove(raza);

            await context.SaveChangesAsync();

            return NoContent();
        }



        #endregion




    }
}
