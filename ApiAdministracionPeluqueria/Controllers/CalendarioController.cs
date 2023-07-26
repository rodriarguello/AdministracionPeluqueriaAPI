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
        private readonly ResponseApi responseApi;

        public CalendarioController(ApplicationDbContext context, UserManager<Usuario> userManager, IMapper mapper, ResponseApi responseApi)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.responseApi = responseApi;
        }



       

        #region MOSTRAR CALENDARIO

        [HttpGet]

        public async Task<ActionResult<ModeloRespuesta>> GetCalendario()
        {

            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync();


                
              
               
                
                return responseApi.respuestaExitosa(mapper.Map<CalendarioDTO>(calendario));
            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
                
            }

        }


        #endregion



        #region CREAR CALENDARIO

        [HttpPost]
        public async Task<ActionResult<ModeloRespuesta>> CrearCalendario([FromBody] CalendarioCreacionDTO nuevoCalendarioDTO)
        {
             var transaccion = await context.Database.BeginTransactionAsync();
            try
            {

                var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = emailClaim.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var usuarioId = usuario.Id;

                #region VALIDACIONES

                
                

                if (nuevoCalendarioDTO.HoraInicioTurnos < 0) return responseApi.respuestaError("La hora de inicio no puede ser un número negativo");

                if (nuevoCalendarioDTO.HoraFinTurnos < nuevoCalendarioDTO.HoraInicioTurnos) return responseApi.respuestaError("La hora de fin no puede ser menor que la hora de inicio");

                if (nuevoCalendarioDTO.IntervaloTurnos < 10) return responseApi.respuestaError("El intervalo entre turnos tiene que ser de 10 minutos como mínimo");

                if (nuevoCalendarioDTO.FechaFin <= nuevoCalendarioDTO.FechaInicio) return responseApi.respuestaError("La fecha de fin del calendario no puede ser menor o igual a la fecha de fin");

            
                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuarioId).FirstOrDefaultAsync();



                if (calendario!= null) return responseApi.respuestaError("No se puede tener más de 1 calendario");

                    


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

                nuevoCalendario.Usuario = usuario;
                
                
                context.Calendarios.Add(nuevoCalendario);
                await context.SaveChangesAsync();


                #region CARGAR DIAS

                DateTime fechaCargar = nuevoCalendarioDTO.FechaInicio;

                var listFechas = new List<DateTime>();

                while (fechaCargar <= nuevoCalendario.FechaFin)
                {
                    listFechas.Add(fechaCargar);

                    fechaCargar = fechaCargar.AddDays(1);
                }

                #endregion


                #region CARGAR HORARIOS

                var listHorarios = new List<TimeSpan>();

                while (horaInicio < horaFin)
                {


                    listHorarios.Add(horaInicio);

                    horaInicio = horaInicio + intervalo;

                }

                #endregion


                #region GENERAR TURNOS

                
                ////Por cada dia cargado, se generan los turnos con cada horario disponible
                listFechas.ForEach(fecha =>
                {
                    listHorarios.ForEach(horario =>
                    {
                        Turno nuevoTurno = new Turno(true, false, nuevoCalendario.Id, usuario.Id);

                        nuevoTurno.Fecha = fecha;
                        nuevoTurno.Horario = horario;
                        nuevoTurno.Calendario = nuevoCalendario;
                        context.Turnos.Add(nuevoTurno);

                    });

                });

                #endregion



                //Guardar los turnos cargados en la base de datos

                await context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return responseApi.respuestaExitosa();

            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();

                return responseApi.respuestaError(ex.Message);

            }


        }

        #endregion

        #region MODIFICAR CALENDARIO

        [HttpPut]
        public async Task<ActionResult<ModeloRespuesta>> ModificarDatosCalendario(CalendarioCreacionDTO calendarioDTO)
        {
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (calendario == null) return responseApi.respuestaError("El usuario no posee ningún calendario");

                if (calendarioDTO.IntervaloTurnos < 10) return responseApi.respuestaError("El intervalo entre turnos tiene que ser de 10 minutos como mínimo");

                if (calendarioDTO.HoraFinTurnos < calendarioDTO.HoraInicioTurnos) return responseApi.respuestaError("La hora de fin no puede ser menor que la hora de inicio");


                TimeSpan horaInicio = new TimeSpan(calendarioDTO.HoraInicioTurnos, 0, 0);

                TimeSpan horaFin = new TimeSpan(calendarioDTO.HoraFinTurnos, 0, 0);

                TimeSpan intervalo = new TimeSpan(0, calendarioDTO.IntervaloTurnos, 0);


                calendario.Nombre = calendarioDTO.Nombre;
                
                
                calendario.FechaFin = calendarioDTO.FechaFin;

                //Modificacion invervalo entre turnos

                if (calendario.IntervaloTurnos != intervalo || calendario.HoraInicioTurnos != horaInicio || calendario.HoraFinTurnos != horaFin)
                {
                    var turnos = await context.Turnos
                        .Where(turno => turno.IdUsuario == usuario.Id)
                        .Where(turno => turno.Fecha.Date > DateTime.Now.Date)
                        .ToListAsync();


                    var turnosTomados = turnos.Where(turno => turno.Disponible == true).ToList();

                    if (turnosTomados.Count > 0) return responseApi.respuestaError("No se puede modificar el intervalo o la hora de inicio o la hora de fin porque" +
                                                                            "ya hay turnos tomados y se van a perder. " +
                                                                            "Primero se deben eliminar o reasignar esos turnos.");


                    foreach (var turno in turnos)
                    {

                        context.Remove(turno);

                    }

                    #region CARGAR DIAS

                    DateTime fechaCargar = DateTime.Now;

                    //var listFechas = new List<Fecha>();
                    //while (fechaCargar <= calendarioDTO.FechaFin)
                    //{
                    //    var fecha = new Fecha();

                    //    fecha.Dia = fechaCargar;

                    //    fecha.IdCalendario = calendario.Id;

                    //    context.Fechas.Add(fecha);
                    //    listFechas.Add(fecha);

                    //    fechaCargar = fechaCargar.AddDays(1);
                    //}

                    #endregion


                    #region CARGAR HORARIOS

                    //No hace falta cargar los horarios ni las fechas en la base de datos- Se va a hacer mucho quilombo a la hora de agregar mas turnos o cambiar los intervalos

                    var listHorarios = new List<TimeSpan>();

                    while (horaInicio < horaFin)
                    {
                        //Horario horario = new Horario();

                        //horario.Hora = horaInicio;

                        //horario.IdCalendario = calendario.Id;

                        //context.Horarios.Add(horario);

                        listHorarios.Add(horaInicio);

                        horaInicio = horaInicio + intervalo;

                    }

                    #endregion

                    //Guardar los dias y horarios cargados en la Base de Datos

                    //await context.SaveChangesAsync();



                    //Buscar los dias y horarios en la base de datos
                    //var fechas = await context.Fechas.Where(fecha => fecha.IdCalendario == calendario.Id).Where(fecha=>fecha.Dia.Date>fechaCargar.Date).ToListAsync();
                    //var horarios = await context.Horarios.Where(horario => horario.IdCalendario == calendario.Id).ToListAsync();

                    #region GENERAR TURNOS


                    //Por cada dia cargado, se generan los turnos con cada horario disponible
                    
                    //var listTurnos = new List<Turno>();
                    //listFechas.ForEach(fecha =>
                    //{
                    //    listHorarios.ForEach(horario =>
                    //    {
                    //        Turno nuevoTurno = new Turno(fecha.Id, horario.Hours, true, false, calendario.Id, usuario.Id);
                            
                            
                    //        listTurnos.Add(nuevoTurno);
                    //        //context.Turnos.Add(nuevoTurno);

                    //    });

                    //});

                    #endregion









                }







                return responseApi.respuestaExitosa(mapper.Map<CalendarioDTO>(calendario));
            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);

            }

           
        }

        [HttpPut("/hola")]
        public async Task<ActionResult> AgregarTurnos()
        {
            return Ok();
        }

        [HttpPut("chau")]
        public async Task<ActionResult> EliminarTurnos()
        {
            return Ok();
        }

        #endregion

        #region ELIMINAR CALENDARIO 
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> Delete([FromRoute]int id)
        {
            var transaccion = await context.Database.BeginTransactionAsync();
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);


                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync(calendario=>calendario.Id==id);

                if (calendario == null) return responseApi.respuestaError("El usuario no posee un calendario con el Id especificado");

                

                var turnos = await context.Turnos.Where(turno=>turno.IdCalendario == calendario.Id).ToListAsync();

                
                turnos.ForEach(turno=> context.Remove(turno));
            
                context.Remove(calendario);

                await context.SaveChangesAsync();

                await transaccion.CommitAsync();

                
                return responseApi.respuestaExitosa();

            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                return responseApi.respuestaError(ex.Message);
            }
        }



        #endregion




    }
}
