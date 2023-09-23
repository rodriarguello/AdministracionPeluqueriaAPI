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

                    horaInicio +=  intervalo;

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

                nuevoCalendario.CantidadHorarios = listHorarios.Count;

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

        #region MODIFICAR NOMBRE CALENDARIO

        [HttpPut("modificarnombre")]
        public async Task<ActionResult<ModeloRespuesta>> ModificarNombreCalendario(string nuevoNombre)
        {
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (calendario == null) return responseApi.respuestaError("El usuario no posee ningún calendario");

                calendario.Nombre = nuevoNombre;


                await context.SaveChangesAsync();

                return responseApi.respuestaExitosa(mapper.Map<CalendarioDTO>(calendario));
            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);

            }

           
        }

        #endregion



        #region AGREGAR TURNOS

        [HttpPost("agregarturnos/{nuevaFechaFin}")]
        public async Task<ActionResult<ModeloRespuesta>> AgregarTurnos([FromRoute]DateTime nuevaFechaFin)
        {
            var transaccion = await context.Database.BeginTransactionAsync();
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);


                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (calendario == null) return responseApi.respuestaError("El usuario no posee un calendario");

                if (calendario.FechaFin.Date > nuevaFechaFin.Date) return responseApi.respuestaError("La fecha enviada es anterior a la fecha actual de finalización");

                
                #region CARGAR DIAS

                DateTime fechaCargar = calendario.FechaFin.AddDays(1);

                var listFechas = new List<DateTime>();

                while (fechaCargar.Date <= nuevaFechaFin.Date)
                {
                    listFechas.Add(fechaCargar);

                    fechaCargar = fechaCargar.AddDays(1);
                }

                #endregion


                #region CARGAR HORARIOS

                var horaInicio = calendario.HoraInicioTurnos;

                var intervalo = calendario.IntervaloTurnos;

                var listHorarios = new List<TimeSpan>();

                while (horaInicio < calendario.HoraFinTurnos)
                {


                    listHorarios.Add(horaInicio);

                    horaInicio += intervalo;

                }

                #endregion


                #region GENERAR TURNOS


                ////Por cada dia cargado, se generan los turnos con cada horario disponible
                listFechas.ForEach(fecha =>
                {
                    listHorarios.ForEach(horario =>
                    {
                        Turno nuevoTurno = new Turno(true, false, calendario.Id, usuario.Id);

                        nuevoTurno.Fecha = fecha;
                        nuevoTurno.Horario = horario;
                        nuevoTurno.Calendario = calendario;
                        context.Turnos.Add(nuevoTurno);

                    });

                });



                #endregion

                calendario.FechaFin = nuevaFechaFin;

                await context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return responseApi.respuestaExitosa(mapper.Map<CalendarioDTO>(calendario));


            }
            catch (Exception ex)
            {   
                await transaccion.RollbackAsync();
                return responseApi.respuestaError(ex.Message);
            }

            
        }

        #endregion



        #region ELIMINAR TURNOS


        [HttpPut("eliminarturnos/{nuevaFechaFin}")]
        public async Task<ActionResult<ModeloRespuesta>> EliminarTurnos([FromRoute] DateTime nuevaFechaFin)
        {
            var transaccion = await context.Database.BeginTransactionAsync();
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);


                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (calendario == null) return responseApi.respuestaError("El usuario no posee un calendario");

                if (calendario.FechaFin.Date < nuevaFechaFin.Date) return responseApi.respuestaError("La fecha enviada es superior a la fecha actual de finalización");

                var turnosEliminar = await context.Turnos
                            .Where(turno => turno.IdCalendario == calendario.Id)
                            .Where(turno=>turno.Fecha.Date>nuevaFechaFin.Date)
                            .ToListAsync();

                foreach (Turno turno in turnosEliminar)
                {

                    if (turno.Disponible == false) return responseApi.respuestaErrorEliminacion("No se puede eliminar alguno de los turnos ya que estan ocupados. Primero debe reorganizar esos turnos");
                    context.Remove(turno);

                }


               

                calendario.FechaFin = nuevaFechaFin;

                await context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return responseApi.respuestaExitosa(mapper.Map<CalendarioDTO>(calendario));


            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                return responseApi.respuestaError(ex.Message);
            }
        }


        #endregion

        
        
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
