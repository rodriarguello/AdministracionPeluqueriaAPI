using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        #region Constructor
        public TurnosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        #endregion


        #region METODOS GET

        [HttpGet]
        public async Task<ActionResult<List<TurnoDTO>>> GetTurnos()
        {

            var turnos = await context.Turnos.ToListAsync();
            

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
