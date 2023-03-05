using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/turnos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TurnosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<Usuario> userManager;

        #region Constructor
        public TurnosController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }


        #endregion


        #region METODOS GET

        [HttpGet("{calendarioId:int}")]

        public async Task<ActionResult<List<TurnoDTO>>> GetTurnos(int calendarioId)
        {

            var existeCalendario = await context.Calendarios.AnyAsync(calendario => calendario.Id == calendarioId);

            if (!existeCalendario) return BadRequest("No existe un calendario con el Id especificado");
            
            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).
                FirstOrDefaultAsync(calendario=>calendario.Id == calendarioId);


            if (calendario==null) return BadRequest("No existe un calendario con el Id especificado para este usuario");


            var turnos = await context.Turnos.Where(turnos => turnos.IdCalendario == calendarioId).ToListAsync();
            

            return mapper.Map<List<TurnoDTO>>(turnos);
            
        }

        #endregion

        //Hay que cambiarlo por PATCH

        #region METODOS PUT
        [HttpPut]
        public async Task<ActionResult> PutTurno([FromBody]TurnoModificarDTO turnoDTO)
        {

            bool existe = await context.Turnos.AnyAsync(turno => turno.Id == turnoDTO.Id);

            if(!existe) return NotFound();

            var turno = mapper.Map<Turno>(turnoDTO);

            context.Update(turno);
            await context.SaveChangesAsync();

            return NoContent();
        }

        #endregion



        #region METODOS DELETE

        #endregion
    }
}
