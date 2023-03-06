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
using Microsoft.AspNetCore.Identity;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/enfermedades")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EnfermedadesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<Usuario> userManager;

        #region Constructor
        public EnfermedadesController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        #endregion



        #region MOSTRAR ENFERMEDADES

        [HttpGet]
        public async Task<ActionResult<List<EnfermedadDTO>>> Get()
        {
            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);


            var enfermedades = await context.Enfermedades.Where(enfermedad => enfermedad.IdUsuario == usuario.Id).ToListAsync();

            
            

            return mapper.Map<List<EnfermedadDTO>>(enfermedades);

        }



        #endregion


        #region INSERTAR ENFERMEDAD

        [HttpPost]
        public async Task<ActionResult<EnfermedadDTO>> Post([FromBody] EnfermedadCreacionDTO nuevaEnfermedadDTO)
        {
            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var nuevaEnfermedad = mapper.Map<Enfermedad>(nuevaEnfermedadDTO);

            nuevaEnfermedad.IdUsuario = usuario.Id;

            
            context.Add(nuevaEnfermedad);
            
            await context.SaveChangesAsync();

            return mapper.Map<EnfermedadDTO>(nuevaEnfermedad);
        }

        #endregion



        #region MODIFICAR ENFERMEDAD

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] EnfermedadDTO enfermedadDTO)
        {
            bool existe = await context.Enfermedades.AnyAsync(enfermedad => enfermedad.Id == enfermedadDTO.Id);

            if (!existe) return NotFound();


            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = claimEmail.Value;
            var usuario = await userManager.FindByEmailAsync(email);


            
            var enfermedad = mapper.Map<Enfermedad>(enfermedadDTO);
            enfermedad.IdUsuario = usuario.Id;

            
            context.Update(enfermedad);
            await context.SaveChangesAsync();


            return NoContent();
        }

        #endregion



        #region ELIMINAR ENFERMEDAD
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
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
