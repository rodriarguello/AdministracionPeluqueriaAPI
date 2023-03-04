using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/alergias")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AlergiasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<Usuario> userManager;

        #region Constructor
        public AlergiasController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;

        }

        #endregion



        #region Metodos GET

        [HttpGet]
        public async Task<ActionResult<List<AlergiaDTO>>> GetAlergias()
        {

            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);


            var alergias = await context.Alergias.Where(alergia => alergia.IdUsuario == usuario.Id).ToListAsync();

            return mapper.Map<List<AlergiaDTO>>(alergias);

        }



        #endregion


        #region Metodos POST

        [HttpPost]
        public async Task<ActionResult<AlergiaDTO>> PostAlergia([FromBody]AlergiaCreacionDTO nuevaAlergiaDTO)
        {

            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var nuevaAlergia = mapper.Map<Alergia>(nuevaAlergiaDTO);

            nuevaAlergia.IdUsuario = usuario.Id;

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
