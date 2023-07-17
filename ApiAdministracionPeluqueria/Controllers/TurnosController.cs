using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly ResponseApi responseApi;

        #region Constructor
        public TurnosController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager, ResponseApi responseApi)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.responseApi = responseApi;
        }


        #endregion

        

        #region MOSTRAR TURNOS

        [HttpGet("{calendarioId:int}")]

        public async Task<ActionResult<ModeloRespuesta>> Get(int calendarioId)
        {
            try
            {
                var existeCalendario = await context.Calendarios.AnyAsync(calendario => calendario.Id == calendarioId);

                if (!existeCalendario) return responseApi.respuestaError("No existe un calendario con el Id especificado");
            
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).
                    FirstOrDefaultAsync(calendario=>calendario.Id == calendarioId);

                if (calendario==null) return responseApi.respuestaError("No existe un calendario con el Id especificado para este usuario");

                var turnos = await context.Turnos.Where(turnos => turnos.IdCalendario == calendarioId)
                    .Include(turnos=> turnos.Fecha)
                    .Include(turnos=>turnos.Horario)
                    .Include(turnos=>turnos.Mascota)
                    .ToListAsync();

                 


                return responseApi.respuestaExitosa(mapper.Map<List<TurnoDTO>>(turnos));


            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
            }

        }


        [HttpGet("filtrar/{calendarioId:int},{fechaInicio},{fechaFin}")]

        public async Task<ActionResult<ModeloRespuesta>> FiltrarPorFechas([FromRoute] int calendarioId, DateTime fechaInicio,DateTime fechaFin)
        {
            try
            {
                var existeCalendario = await context.Calendarios.AnyAsync(calendario => calendario.Id == calendarioId);

                if (!existeCalendario) return responseApi.respuestaError("No existe un calendario con el Id especificado");

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var nuevaFechaInicio = new DateTime(fechaInicio.Year,fechaInicio.Month,fechaInicio.Day,0,0,0);
                var nuevaFechaFin = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day, 23, 59, 59);

                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).
                    FirstOrDefaultAsync(calendario => calendario.Id == calendarioId);

                if (calendario == null) return responseApi.respuestaError("No existe un calendario con el Id especificado para este usuario");

                var turnos = await context.Turnos.Where(turnos => turnos.IdCalendario == calendarioId)
                    .Include(turnos => turnos.Fecha)
                    .Include(turnos => turnos.Horario)
                    .Include(turnos => turnos.Mascota)
                    .Where(turnos=> turnos.Fecha.Dia>=nuevaFechaInicio && turnos.Fecha.Dia <=nuevaFechaFin)
                    .ToListAsync();


                if(turnos.Count == 0) return responseApi.respuestaError("No existen turnos en esa fecha");

                return responseApi.respuestaExitosa(mapper.Map<List<TurnoDTO>>(turnos));


            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
            }

        }

        #endregion

        [HttpGet("cliente/{idCliente:int}")]
        public async Task<ActionResult<ModeloRespuesta>> MostrarTurnosCliente(int idCliente)
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);


                var mascotasCliente = await context.Clientes
                    .Where(cliente => cliente.Id == idCliente)
                    .Include(cliente => cliente.Mascotas)
                    .FirstOrDefaultAsync(cliente=>cliente.IdUsuario==usuario.Id);

                if (mascotasCliente == null) return responseApi.respuestaError("No existe un cliente con el id especificado");
                
                var listaIdsMascotas = new List<int?>();

                if(mascotasCliente.Mascotas.Count() <1) return responseApi.respuestaExitosa(listaIdsMascotas);


                mascotasCliente.Mascotas.ForEach(mascota=> listaIdsMascotas.Add(mascota.Id));

                var turnos = await context.Turnos.Where(turno=>turno.IdUsuario==usuario.Id).Where(turno=>listaIdsMascotas.Contains(turno.IdMascota))
                     .Include(turno=>turno.Mascota)
                     .Include(turno=>turno.Horario)
                     .Include(turno=>turno.Fecha)
                    .ToListAsync();


               
                
                
                return responseApi.respuestaExitosa(mapper.Map<List<TurnoDTO>>(turnos));


            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
            }



        }


        #region MODIFICAR TURNO


        [HttpPut("reservar/{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> ReservarTurno([FromRoute] int id , [FromBody] TurnoModificarDTO turnoDTO)
        {
            try
            {
                
               
            
                var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = emailClaim.Value;
                var usuario = await userManager.FindByEmailAsync(email);



                var turno = await context.Turnos.Where(turno => turno.IdUsuario == usuario.Id)
                    .Include(turno=>turno.Fecha)
                    .Include(turno=>turno.Horario)
                    .Include(turno=>turno.Mascota)
                    .FirstOrDefaultAsync(turno => turno.Id == id);

                if (turno == null) return responseApi.respuestaError("No existe el turno con el Id especificado");

                if (!turno.Disponible) return responseApi.respuestaError("El turno no esta disponible");

                var mascota = await context.Mascotas.Where(mascota => mascota.IdUsuario == usuario.Id)
                    .FirstOrDefaultAsync(mascota => mascota.Id == turnoDTO.IdMascota);

                if(mascota == null) return responseApi.respuestaError("No existe una mascota con el Id especificado");

                if (turnoDTO.Precio <= 0) return responseApi.respuestaError("El precio debe ser mayor a 0");



                turno.Disponible = false;
                turno.IdMascota = turnoDTO.IdMascota;
                turno.Mascota = mascota;
                turno.Asistio = false;
                turno.Precio = turnoDTO.Precio;
                
                
                await context.SaveChangesAsync();


                return responseApi.respuestaExitosa();
            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
                
            }
        }




        [HttpPut("cancelar/{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> CancelarReserva([FromRoute]int id)
        {

            try
            {

                var emailClaim = HttpContext.User.Claims.Where(claim=>claim.Type=="email").FirstOrDefault();
                var email = emailClaim.Value;
                var usuario = await userManager.FindByEmailAsync(email);


                var turno = await context.Turnos.Where(turno => turno.IdUsuario == usuario.Id)
                    .Include(turno=>turno.Fecha)
                    .Include(turno=>turno.Horario)
                    .Include(turno=>turno.Mascota)
                    .FirstOrDefaultAsync(turno=>turno.Id == id);

                if (turno == null) return responseApi.respuestaError("No existe un turno con el Id especificado");

                if (turno.Disponible) return responseApi.respuestaError("El turno NO estaba reservado");

                if (turno.Asistio == true) return responseApi.respuestaError("El turno no se puede cancelar, porque esta marcado como que la mascota asistió");

                turno.Disponible = true;
                turno.Asistio = null;
                turno.Precio = null;
                turno.IdMascota = null;
                turno.Mascota = null;


                
                await context.SaveChangesAsync();


                return responseApi.respuestaExitosa();

            }
            catch (Exception ex)
            {

                return responseApi.respuestaError(ex.Message);
            }
        }


        [HttpPut("asistencia/{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> ModificarAsistencia([FromRoute] int id, [FromBody] bool asistio)
        {
            var transaccion = await context.Database.BeginTransactionAsync();
            try
            {
                var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = emailClaim.Value;
                var usuario = await userManager.FindByEmailAsync(email);


                var turno = await context.Turnos.Where(turno => turno.IdUsuario == usuario.Id)
                    .Include(turno=>turno.Fecha)
                    .Include(turno=>turno.Horario)
                    .Include(turno=>turno.Mascota)
                    .FirstOrDefaultAsync(turno => turno.Id == id);

                if (turno == null) return responseApi.respuestaError("No existe un turno con el Id especificado");

                if (turno.Disponible) return responseApi.respuestaError("El turno esta disponible, por lo cual no se le puede modificar la asistencia");

                if (turno.Asistio == asistio) return responseApi.respuestaError("El turno ya se encuentra con ese estado de asistencia");

                //Agregar Ingreso

                if (turno.Asistio == null && asistio || turno.Asistio == false && asistio)
                {
                    Caja nuevoIngreso = new Caja
                    {
                        Fecha = turno.Fecha.Dia,
                        Precio = (decimal)turno.Precio!,
                        IdUsuario = usuario.Id,
                        Usuario = usuario,
                        IdTurno = turno.Id

                    };

                    context.Caja.Add(nuevoIngreso);


                }

                //Eliminar Ingreso
                if (turno.Asistio == true && asistio == false)
                {
                    var registroCaja = await context.Caja.Where(caja => caja.IdUsuario == usuario.Id).FirstOrDefaultAsync(caja => caja.IdTurno == turno.Id);

                    if (registroCaja != null)
                    {
                        context.Caja.Remove(registroCaja);
                    }
                }


                turno.Asistio = asistio;

                
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

        [HttpPut("precio/{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> ModificarPrecio([FromRoute] int id, [FromBody] int nuevoPrecio)
        {
            var transaccion = await context.Database.BeginTransactionAsync();

            try
            {

                if (nuevoPrecio <= 0) return responseApi.respuestaError("El precio debe ser mayor a 0");


                var emailClaim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = emailClaim.Value;
                var usuario = await userManager.FindByEmailAsync(email);


                var turno = await context.Turnos.Where(turno => turno.IdUsuario == usuario.Id)
                    .Include(turno=>turno.Fecha)
                    .Include(turno=>turno.Horario)
                    .Include(turno=>turno.Mascota)
                    .FirstOrDefaultAsync(turno => turno.Id == id);

                if (turno == null) return responseApi.respuestaError("No existe un turno con el Id especificado");


                if (turno.Disponible) return responseApi.respuestaError("El turno esta disponible, por lo cual no se le puede modificar el precio");



                //Modifica precio en la entidad caja
                if (turno.Asistio == true)
                {
                    var registroCaja = await context.Caja.Where(registro => registro.IdUsuario == usuario.Id).FirstOrDefaultAsync(registro => registro.IdTurno == turno.Id);

                    if (registroCaja != null)
                    {
                        registroCaja.Precio = nuevoPrecio;
                    }
                }

                turno.Precio = nuevoPrecio;
           

                
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
