using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;
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

        public async Task<ActionResult> CrearCalendario([FromBody] CalendarioDTO parametros)
        {
            
            #region CARGAR DIAS
            
            DateTime fechaCargar= DateTime.Now;


            for (int i = 0; i < parametros.CantidadDias; i++)
            {
                var fecha = new Fecha();
                
                fecha.Dia = fechaCargar;
                
                context.Fechas.Add(fecha);
                
                fechaCargar = fechaCargar.AddDays(1);

            }

            #endregion


            #region CARGAR HORARIOS

            TimeSpan horaInicio = new TimeSpan(parametros.HoraInicioTurnos,0,0);
            
            TimeSpan horaFin = new TimeSpan(parametros.HoraFinTurnos, 0, 0);

            TimeSpan intervalo = new TimeSpan(0,parametros.IntervaloTurnos,0);

            
            while (horaInicio<horaFin)
            {
                Horario horario = new Horario();

                horario.Hora = horaInicio;
            
                context.Horarios.Add(horario);
            
                horaInicio = horaInicio + intervalo;

            }

            #endregion

            //Guardar los dias y horarios cargados en la Base de Datos
            
            await context.SaveChangesAsync();

            

            //Buscar los dias y horarios en la base de datos
            var fechas = await context.Fechas.ToListAsync();
            var horarios = await context.Horarios.ToListAsync();

            #region GENERAR TURNOS
            //Por cada dia cargado, se generan los turnos con cada horario disponible
            fechas.ForEach(fecha  =>
            {
                horarios.ForEach(horario =>
                {
                    Turno nuevoTurno = new Turno(fecha.Id, horario.Id, true , false);

                    context.Turnos.Add(nuevoTurno);
                    
                });
 
            });

            #endregion



            //Guardar los turnos cargados en la base de datos

            await context.SaveChangesAsync();


            return Ok();

        }

    }
}
