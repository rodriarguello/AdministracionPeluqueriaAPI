using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/turnos")]
    [ApiController]
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


        #region METODOS POST

        #endregion



        #region METODOS PUT

        #endregion



        #region METODOS DELETE

        #endregion
    }
}
