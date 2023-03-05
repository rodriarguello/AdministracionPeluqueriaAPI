using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/razas")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RazasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<Usuario> userManager;

        #region Constructor


        public RazasController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        #endregion



        #region MOSTRAR RAZAS
        [HttpGet]
        public async Task<ActionResult<List<RazaDTO>>> Get()
        {

            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var razas = await context.Razas.Where(raza=> raza.IdUsuario == usuario.Id).ToListAsync();

            return mapper.Map<List<RazaDTO>>(razas);

        }

        #endregion


        #region INSERTAR RAZA


        [HttpPost]
        public async Task<ActionResult<RazaDTO>> PostRaza([FromBody]RazaCreacionDTO nuevaRazaDTO)
        {

            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var nuevaRaza = mapper.Map<Raza>(nuevaRazaDTO);

            nuevaRaza.IdUsuario = usuario.Id;


            context.Add(nuevaRaza);
            
            await context.SaveChangesAsync();

            return mapper.Map<RazaDTO>(nuevaRaza);
            
        }

        #endregion



        #region MODIFICAR RAZA
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



        #region ELIMINAR RAZA
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
