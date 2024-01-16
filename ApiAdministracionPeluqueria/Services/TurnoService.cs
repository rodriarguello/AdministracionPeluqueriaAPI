using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Services.Interfaces;
using ApiAdministracionPeluqueria.Models.Entidades;
using Microsoft.AspNetCore.Identity;

namespace ApiAdministracionPeluqueria.Services
{
    public class TurnoService : ITurnoService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICajaService _cajaService;
        private readonly UserManager<Usuario> _userManager;

        public TurnoService(ApplicationDbContext context, IMapper mapper, ICajaService cajaService, UserManager<Usuario> userManager)
        {
            _context = context;
            _mapper = mapper;
            _cajaService = cajaService;
            _userManager = userManager;
        }

        public async Task<List<TurnoDTO>> GetAllAsync(int idCalendario, string idUsuario)
        {
            var calendario = await _context.Calendarios.Where(calendario => calendario.IdUsuario == idUsuario).
                FirstOrDefaultAsync(calendario => calendario.Id == idCalendario);

            if (calendario == null) throw new BadRequestException("No existe un calendario con el Id especificado para este usuario");

            var turnos = await _context.Turnos.Where(turnos => turnos.IdCalendario == idCalendario)
                                .Include(turnos => turnos.Fecha)
                                .Include(turnos => turnos.Horario)
                                .Include(turnos => turnos.Mascota)
                                .ToListAsync();

            return _mapper.Map<List<TurnoDTO>>(turnos);
        }


        public async Task<ResTurnosFiltrados> FiltrarPorFechasAsync(int calendarioId, DateTime fechaInicio, DateTime fechaFin, string idUsuario)
        {
            var calendario = await _context.Calendarios.Where(calendario => calendario.IdUsuario == idUsuario).
                FirstOrDefaultAsync(calendario => calendario.Id == calendarioId);

            if (calendario == null) throw new BadRequestException("No existe un calendario con el Id especificado para este usuario");

            var nuevaFechaInicio = new DateTime(fechaInicio.Year, fechaInicio.Month, fechaInicio.Day, 0, 0, 0);
            var nuevaFechaFin = new DateTime(fechaFin.Year, fechaFin.Month, fechaFin.Day, 23, 59, 59);


            var turnos = await _context.Turnos.Where(turnos => turnos.IdCalendario == calendarioId)
                .Include(turnos => turnos.Mascota)
                .Where(turnos => turnos.Fecha.Date >= nuevaFechaInicio.Date && turnos.Fecha.Date <= nuevaFechaFin.Date)
                .ToListAsync();

            var listLunes = _mapper.Map<List<TurnoDTO>>(turnos.Where(turno => turno.Fecha.DayOfWeek == DayOfWeek.Monday).ToList());

            var listMartes = _mapper.Map<List<TurnoDTO>>(turnos.Where(turno => turno.Fecha.DayOfWeek == DayOfWeek.Tuesday).ToList());

            var listMiercoles = _mapper.Map<List<TurnoDTO>>(turnos.Where(turno => turno.Fecha.DayOfWeek == DayOfWeek.Wednesday).ToList());

            var listJueves = _mapper.Map<List<TurnoDTO>>(turnos.Where(turno => turno.Fecha.DayOfWeek == DayOfWeek.Thursday).ToList());

            var listViernes = _mapper.Map<List<TurnoDTO>>(turnos.Where(turno => turno.Fecha.DayOfWeek == DayOfWeek.Friday).ToList());

            var listSabado = _mapper.Map<List<TurnoDTO>>(turnos.Where(turno => turno.Fecha.DayOfWeek == DayOfWeek.Saturday).ToList());

            var listDomingo = _mapper.Map<List<TurnoDTO>>(turnos.Where(turno => turno.Fecha.DayOfWeek == DayOfWeek.Sunday).ToList());


            var turnosFiltrados = new ResTurnosFiltrados
            {
                Lunes = listLunes,
                Martes = listMartes,
                Miercoles = listMiercoles,
                Jueves = listJueves,
                Viernes = listViernes,
                Sabado = listSabado,
                Domingo = listDomingo,
                CantidadHorarios = calendario.CantidadHorarios
            };


            return turnosFiltrados;

        }


        public async Task ReservarTurnoAsync(int idTurno, TurnoModificarDTO turnoModificarDTO, string idUsuario)
        {
            var turno = await _context.Turnos.Where(turno => turno.IdUsuario == idUsuario)
                .Include(turno => turno.Mascota)
                .FirstOrDefaultAsync(turno => turno.Id == idTurno);

            if (turno == null) throw new BadRequestException("No existe el turno con el Id especificado");

            if (!turno.Disponible) throw new BadRequestException("El turno no esta disponible");

            var mascota = await _context.Mascotas.Where(mascota => mascota.IdUsuario == idUsuario)
                .FirstOrDefaultAsync(mascota => mascota.Id == turnoModificarDTO.IdMascota);

            if (mascota == null) throw new BadRequestException("No existe una mascota con el Id especificado");

            if (turnoModificarDTO.Precio <= 0) throw new BadRequestException("El precio debe ser mayor a 0");



            turno.Disponible = false;
            turno.IdMascota = turnoModificarDTO.IdMascota;
            turno.Mascota = mascota;
            turno.Asistio = false;
            turno.Precio = turnoModificarDTO.Precio;


            await _context.SaveChangesAsync();
        }

        public async Task CancelarTurnoAsync(int idTurno, string idUsuario)
        {
            var turno = await _context.Turnos.Where(turno => turno.IdUsuario == idUsuario)
                .Include(turno => turno.Mascota)
                .FirstOrDefaultAsync(turno => turno.Id == idTurno);

            if (turno == null) throw new BadRequestException("No existe un turno con el Id especificado");

            if (turno.Disponible) throw new BadRequestException("El turno NO estaba reservado");

            if (turno.Asistio == true) throw new BadRequestException("El turno no se puede cancelar, porque esta marcado como que la mascota asistió");

            turno.Disponible = true;
            turno.Asistio = null;
            turno.Precio = null;
            turno.IdMascota = null;
            turno.Mascota = null;



            await _context.SaveChangesAsync();
        }

        public async Task ModificarAsistenciaAsync(int idTurno, string idUsuario, bool asistio)
        {
            var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                var usuario = await _userManager.FindByIdAsync(idUsuario);

                if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

                var turno = await _context.Turnos.Where(turno => turno.IdUsuario == idUsuario)
                    .Include(turno => turno.Mascota)
                    .FirstOrDefaultAsync(turno => turno.Id == idTurno);

                if (turno == null) throw new BadRequestException("No existe un turno con el Id especificado");

                if (turno.Disponible) throw new BadRequestException("El turno esta disponible, por lo cual no se le puede modificar la asistencia");

                if (turno.Asistio == asistio) throw new BadRequestException("El turno ya se encuentra con ese estado de asistencia");

                if (asistio && turno.Fecha.Date > DateTime.UtcNow.AddHours(-3).Date) throw new MensajePersonalizadoException("No se puede marcar como que asistió, cuando todavía no ha llegado la fehca del turno.");

                //Agregar Ingreso

                if (asistio && (turno.Asistio == null || turno.Asistio == false))
                {
                    await _cajaService.CrearIngresoAsync(turno.Fecha, (decimal)turno.Precio, usuario, turno.Id);
                }

                //Eliminar Ingreso
                if (turno.Asistio == true && asistio == false)
                {
                    await _cajaService.EliminarIngresoAsync(turno.Id, idUsuario);
                }


                turno.Asistio = asistio;


                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();
            }
            catch (Exception)
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

        public async Task ModificarPrecioAsync(int idTurno, decimal nuevoPrecio, string idUsuario)
        {
            var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {

                if (nuevoPrecio <= 0) throw new BadRequestException("El precio debe ser mayor a 0");

                var usuario = await _userManager.FindByIdAsync(idUsuario);

                if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

                var turno = await _context.Turnos.Where(turno => turno.IdUsuario == idUsuario)
                    .Include(turno => turno.Mascota)
                    .FirstOrDefaultAsync(turno => turno.Id == idTurno);

                if (turno == null) throw new BadRequestException("No existe un turno con el Id especificado");


                if (turno.Disponible) throw new BadRequestException("El turno esta disponible, por lo cual no se le puede modificar el precio");



                //Modifica precio en la entidad caja
                if (turno.Asistio == true)
                {
                    await _cajaService.ModificarPrecioIngresoAsync(turno.Id, idUsuario, nuevoPrecio);
                }

                turno.Precio = nuevoPrecio;

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();

            }
            catch (Exception)
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

        public async Task CrearTurnosAsync(List<DateTime> listFechas, List<TimeSpan> listHorarios, Calendario calendario, Usuario usuario)
        {
            try
            {
                listFechas.ForEach(fecha =>
                {
                    listHorarios.ForEach(horario =>
                    {
                        Turno nuevoTurno = new Turno(true, false, calendario.Id, usuario.Id);

                        nuevoTurno.Fecha = fecha;
                        nuevoTurno.Horario = horario;
                        nuevoTurno.Calendario = calendario;
                        _context.Turnos.Add(nuevoTurno);

                    });

                });

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
