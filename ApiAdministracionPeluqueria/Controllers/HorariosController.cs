using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.HorarioDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/horarios")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HorariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ResponseApi responseApi;
        private readonly IMapper mapper;

        public HorariosController(ApplicationDbContext context, ResponseApi responseApi, IMapper mapper)
        {
            this.context = context;
            this.responseApi = responseApi;
            this.mapper = mapper;

        }

        [HttpGet("{idCalendario:int}")]
        public async Task<ActionResult<ModeloRespuesta>> MostrarHorarios([FromRoute]int idCalendario)
        {
            try
            {

                var horarios = await context.Horarios.Where(horario => horario.IdCalendario == idCalendario).ToListAsync();
                
                if (horarios == null) return responseApi.respuestaError("No hay turnos para el id especificado");
                
                var horariosDTO = mapper.Map<List<HorarioSinCalendarioDTO>>(horarios);

                return responseApi.respuestaExitosa(horarios);


            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
            }

        }
    }
}
