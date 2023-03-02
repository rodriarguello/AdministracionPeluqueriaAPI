﻿using ApiAdministracionPeluqueria.Models;
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

        public async Task<ActionResult> CrearCalendario([FromBody] CalendarioCreacionDTO nuevoCalendarioDTO)
        {
            if (nuevoCalendarioDTO.HoraInicioTurnos < 0) return BadRequest("La hora de inicio no puede ser un número negativo");

            if (nuevoCalendarioDTO.HoraFinTurnos < nuevoCalendarioDTO.HoraInicioTurnos) return BadRequest("La hora de fin no puede ser menor que la hora de inicio");

            if (nuevoCalendarioDTO.IntervaloTurnos < 10) return BadRequest("El intervalo entre turnos tiene que ser de 10 minutos como mínimo");

            if (nuevoCalendarioDTO.FechaFin == nuevoCalendarioDTO.FechaInicio) return BadRequest("La fecha de inicio del calendario no puede ser igual a la fecha de fin");

            
            var calendarios = await context.Calendarios.Where(calendario => calendario.IdAdministrador == nuevoCalendarioDTO.IdAdministrador).ToListAsync();


            if (calendarios.Count > 1) return BadRequest("No se puede tener más de 2 calendarios");
            

            string nombreCalendario = calendarios[0].Nombre;

            if (nuevoCalendarioDTO.Nombre.ToUpper() == nombreCalendario.ToUpper()) return BadRequest("No se puede tener 2 calendarios con el mismo nombre");
            

            

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

            nuevoCalendario.IdAdministrador = nuevoCalendarioDTO.IdAdministrador;

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
                    Turno nuevoTurno = new Turno(fecha.Id, horario.Id, true , false, nuevoCalendario.Id);

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
