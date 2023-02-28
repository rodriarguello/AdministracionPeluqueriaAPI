using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/calendario")]
    [ApiController]
    public class CalendarioController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CalendarioController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost]

        public async Task<ActionResult> GenerarCalendario([FromBody] CrearCalendario parametros)
        {
            
            DateTime fechaCargar= DateTime.Now;
            
            
            for (int i = 0; i < parametros.Cantidad; i++)
            {
                var fecha = new Fecha();
                
                fecha.Dia = fechaCargar;
                
                context.Fechas.Add(fecha);
                
                fechaCargar = fechaCargar.AddDays(1);

            }

            TimeSpan horaInicio = new TimeSpan(parametros.HoraInicio,0,0);
            
            TimeSpan horaFin = new TimeSpan(parametros.HoraFin, 0, 0);

            TimeSpan intervalo = new TimeSpan(0,parametros.IntervaloTurnos,0);

            
            while (horaInicio<horaFin)
            {
                Horario horario = new Horario();

                horario.Hora = horaInicio;
            
                context.Horarios.Add(horario);
            
                horaInicio = horaInicio + intervalo;

            }

            await context.SaveChangesAsync();

            var fechas = await context.Fechas.ToListAsync();
            var horarios = await context.Horarios.ToListAsync();


            fechas.ForEach(fecha  =>
            {
                horarios.ForEach(horario =>
                {
                    Turno nuevoTurno = new Turno(fecha.Id, horario.Id, true , false);

                    context.Turnos.Add(nuevoTurno);
                    
                });
 
            });

            await context.SaveChangesAsync();


            return Ok();

        }

    }
}
