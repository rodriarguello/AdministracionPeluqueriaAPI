using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
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


        #region MOSTRAR TURNOS

        [HttpGet("{calendarioId:int}")]

        public async Task<ActionResult<List<TurnoDTO>>> Get(int calendarioId)
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


        #region MODIFICAR TURNO


        [HttpPut("reservar/{id:int}")]
        public async Task<ActionResult> ReservarTurno([FromRoute] int id , [FromBody] TurnoModificarDTO turnoDTO)
        {
            
            
            if (turnoDTO.Asistio) return BadRequest("Al reservar un turno no se puede enviar TRUE en el campo asistio");
            
            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);



            var turno = await context.Turnos.Where(turno => turno.IdUsuario == usuario.Id).FirstOrDefaultAsync(turno => turno.Id == id);

            if (turno == null) return NotFound("No existe el turno con el Id especificado");

            if (!turno.Disponible) return BadRequest("El turno no esta disponible");

            var existeMascota = await context.Mascotas.Where(mascota => mascota.IdUsuario == usuario.Id).AnyAsync(mascota => mascota.Id == turnoDTO.IdMascota);

            if(!existeMascota) return NotFound("No existe una mascota con el Id especificado");

            if (turnoDTO.Precio <= 0) return BadRequest("El precio debe ser mayor a 0");



            turno.Disponible = false;
            turno.IdMascota = turnoDTO.IdMascota;
            turno.Asistio = turnoDTO.Asistio;
            turno.Precio = turnoDTO.Precio;

            context.Update(turno);
            await context.SaveChangesAsync();

            return NoContent();
        }




        [HttpPut("cancelar/{id:int}")]
        public async Task<ActionResult> CancelarReserva([FromRoute]int id)
        {
            var emailClaim = HttpContext.User.Claims.Where(claim=>claim.Type=="email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);


            var turno = await context.Turnos.Where(turno => turno.IdUsuario == usuario.Id).FirstOrDefaultAsync(turno=>turno.Id == id);

            if (turno == null) return BadRequest("No existe un turno con el Id especificado");

            if (turno.Disponible) return BadRequest("El turno NO estaba reservado");

            turno.Disponible = true;
            turno.Asistio = null;
            turno.Precio = null;
            turno.IdMascota = null;

            context.Update(turno);
            await context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("asistencia/{id:int}")]
        public async Task<ActionResult> ModificarAsistencia([FromRoute] int id, [FromBody] bool asistio)
        {

            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);


            var turno = await context.Turnos.Where(turno => turno.IdUsuario == usuario.Id).FirstOrDefaultAsync(turno => turno.Id == id);

            if (turno == null) return BadRequest("No existe un turno con el Id especificado");

            if (turno.Disponible) return BadRequest("El turno esta disponible, por lo cual no se le puede modificar la asistencia");


            turno.Asistio = asistio;

            context.Update(turno);
            await context.SaveChangesAsync();

            return NoContent();


        }

        [HttpPut("precio/{id:int}")]
        public async Task<ActionResult> ModificarPrecio([FromRoute] int id, [FromBody] int nuevoPrecio)
        {

            if (nuevoPrecio <= 0) return BadRequest("El precio debe ser mayor a 0");


            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
            var email = emailClaim.Value;
            var usuario = await userManager.FindByEmailAsync(email);


            var turno = await context.Turnos.Where(turno => turno.IdUsuario == usuario.Id).FirstOrDefaultAsync(turno => turno.Id == id);

            if (turno == null) return BadRequest("No existe un turno con el Id especificado");


            if (turno.Disponible) return BadRequest("El turno esta disponible, por lo cual no se le puede modificar el precio");

            turno.Precio = nuevoPrecio;
           

            context.Update(turno);
            await context.SaveChangesAsync();

            return NoContent();


        }

        #endregion




    }
}
