using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/calendario")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CalendarioController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Usuario> userManager;
        private readonly IMapper mapper;


        public CalendarioController(ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
        }


        #region MOSTRAR CALENDARIOS

        [HttpGet]

        public async Task<ActionResult<List<CalendarioDTO>>> GetCalendarios()
        {
            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var calendarios = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).ToListAsync();

            

            return mapper.Map<List<CalendarioDTO>>(calendarios);


        }


        #endregion



        #region CREAR CALENDARIO

        [HttpPost]
        public async Task<ActionResult> CrearCalendario([FromBody] CalendarioCreacionDTO nuevoCalendarioDTO)
        {

            var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = emailClaim.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var usuarioId = usuario.Id;

            #region VALIDACIONES


            if (nuevoCalendarioDTO.HoraInicioTurnos < 0) return BadRequest("La hora de inicio no puede ser un número negativo");

            if (nuevoCalendarioDTO.HoraFinTurnos < nuevoCalendarioDTO.HoraInicioTurnos) return BadRequest("La hora de fin no puede ser menor que la hora de inicio");

            if (nuevoCalendarioDTO.IntervaloTurnos < 10) return BadRequest("El intervalo entre turnos tiene que ser de 10 minutos como mínimo");

            if (nuevoCalendarioDTO.FechaFin == nuevoCalendarioDTO.FechaInicio) return BadRequest("La fecha de inicio del calendario no puede ser igual a la fecha de fin");

            
            var calendarios = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuarioId).ToListAsync();



            if (calendarios.Count > 0)
            {   
                if(calendarios.Count == 2) return BadRequest("No se puede tener más de 2 calendarios");

                string nombreCalendario = calendarios[0].Nombre;

                if (nuevoCalendarioDTO.Nombre.ToUpper() == nombreCalendario.ToUpper()) return BadRequest("No se puede tener 2 calendarios con el mismo nombre");

            }


            #endregion


            TimeSpan horaInicio = new TimeSpan(nuevoCalendarioDTO.HoraInicioTurnos,0,0);
            
            TimeSpan horaFin = new TimeSpan(nuevoCalendarioDTO.HoraFinTurnos, 0, 0);

            TimeSpan intervalo = new TimeSpan(0,nuevoCalendarioDTO.IntervaloTurnos,0);

            Calendario nuevoCalendario = new Calendario();
            
            nuevoCalendario.Nombre = nuevoCalendarioDTO.Nombre;
            
            nuevoCalendario.FechaInicio = nuevoCalendarioDTO.FechaInicio;

            nuevoCalendario.FechaFin = nuevoCalendarioDTO.FechaFin;

            nuevoCalendario.HoraInicioTurnos = horaInicio;
            
            nuevoCalendario.HoraFinTurnos = horaFin;
            
            nuevoCalendario.IntervaloTurnos= intervalo;

            nuevoCalendario.IdUsuario = usuarioId;

            context.Calendarios.Add(nuevoCalendario);
            await context.SaveChangesAsync();


            #region CARGAR DIAS

            DateTime fechaCargar = nuevoCalendarioDTO.FechaInicio;


            while (fechaCargar < nuevoCalendario.FechaFin)
            {
                var fecha = new Fecha();

                fecha.Dia = fechaCargar;

                fecha.IdCalendario = nuevoCalendario.Id;

                context.Fechas.Add(fecha);

                fechaCargar = fechaCargar.AddDays(1);
            }

            #endregion


            #region CARGAR HORARIOS

            
            while (horaInicio<horaFin)
            {
                Horario horario = new Horario();

                horario.Hora = horaInicio;

                horario.IdCalendario = nuevoCalendario.Id;
            
                context.Horarios.Add(horario);
            
                horaInicio = horaInicio + intervalo;

            }

            #endregion

            //Guardar los dias y horarios cargados en la Base de Datos
            
            await context.SaveChangesAsync();

            

            //Buscar los dias y horarios en la base de datos
            var fechas = await context.Fechas.Where(fecha => fecha.IdCalendario == nuevoCalendario.Id).ToListAsync();
            var horarios = await context.Horarios.Where(horario => horario.IdCalendario == nuevoCalendario.Id).ToListAsync();

            #region GENERAR TURNOS


            //Por cada dia cargado, se generan los turnos con cada horario disponible
            fechas.ForEach(fecha  =>
            {
                horarios.ForEach(horario =>
                {
                    Turno nuevoTurno = new Turno(fecha.Id, horario.Id, true , false, nuevoCalendario.Id,usuario.Id);

                    context.Turnos.Add(nuevoTurno);
                    
                });
 
            });

            #endregion



            //Guardar los turnos cargados en la base de datos

            await context.SaveChangesAsync();


            return Ok();

        }

        #endregion



        #region ELIMINAR CALENDARIO 
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute]int id)
        {

            var existeCalendario = await context.Calendarios.AnyAsync(calendario => calendario.Id == id);

            if (!existeCalendario) return NotFound("No existe el calendario con el Id especificado");
            
            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);


            var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync(calendario=>calendario.Id==id);

            if (calendario == null) return NotFound("El usuario no posee un calendario con el Id especificado");

            var fechas = await context.Fechas.Where(fecha => fecha.IdCalendario == calendario.Id).ToListAsync();

            var horarios = await context.Horarios.Where(horario=>horario.IdCalendario==calendario.Id).ToListAsync();

            var turnos = await context.Turnos.Where(turno=>turno.IdCalendario == calendario.Id).ToListAsync();

            fechas.ForEach(fecha => context.Remove(fecha));


            horarios.ForEach(horario => context.Remove(horario));

            turnos.ForEach(turno=> context.Remove(turno));
            
            context.Remove(calendario);

            await context.SaveChangesAsync();

            return NoContent();

        }



        #endregion




    }
}
