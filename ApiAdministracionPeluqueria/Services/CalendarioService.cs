using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Services.Interfaces;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.CalendarioDTO;
using ApiAdministracionPeluqueria.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace ApiAdministracionPeluqueria.Services
{
    public class CalendarioService:ICalendarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly ITurnoService _turnoService;
        private readonly IMapper _mapper;

        public CalendarioService(ApplicationDbContext context, IUserService userService, ITurnoService turnoService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _turnoService = turnoService;
            _mapper = mapper;
        }

        public async Task<CalendarioDTO> GetByIdUserAsync(string idUsuario)
        {
            var usuario = await _userService.GetByIdAsync(idUsuario);

            if (usuario == null) throw new NotFoundException();

            var calendario = await _context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync();

            if (calendario == null) throw new NotFoundException();

            return _mapper.Map<CalendarioDTO>(calendario);

        }


        public async Task<CalendarioDTO> CreateAsync(string idUsuario, CalendarioCreacionDTO nuevoCalendarioDTO)
        {
            var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {


                #region VALIDACIONES


                if (nuevoCalendarioDTO.HoraInicioTurnos < 0) throw new BadRequestException("La hora de inicio no puede ser un número negativo");

                if (nuevoCalendarioDTO.HoraFinTurnos < nuevoCalendarioDTO.HoraInicioTurnos) throw new BadRequestException("La hora de fin no puede ser menor que la hora de inicio");

                if (nuevoCalendarioDTO.HoraFinTurnos > 24) throw new BadRequestException("La hora de fin no puede ser mayor a 24");

                if (nuevoCalendarioDTO.IntervaloTurnos < 10) throw new BadRequestException("El intervalo entre turnos tiene que ser de 10 minutos como mínimo");

                if (nuevoCalendarioDTO.FechaFin <= nuevoCalendarioDTO.FechaInicio) throw new BadRequestException("La fecha de fin del calendario no puede ser menor o igual a la fecha de fin");

                var usuario = await _userService.GetByIdAsync(idUsuario);

                if (usuario == null) throw new BadRequestException("El usuario no existe");


                var calendario = await _context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync();



                if (calendario != null) throw new BadRequestException("No se puede tener más de 1 calendario");


                #endregion


                TimeSpan horaInicio = new TimeSpan(nuevoCalendarioDTO.HoraInicioTurnos, 0, 0);

                TimeSpan horaFin = new TimeSpan(nuevoCalendarioDTO.HoraFinTurnos, 0, 0);

                TimeSpan intervalo = new TimeSpan(0, nuevoCalendarioDTO.IntervaloTurnos, 0);

                Calendario nuevoCalendario = new Calendario();

                nuevoCalendario.Nombre = nuevoCalendarioDTO.Nombre;

                nuevoCalendario.FechaInicio = nuevoCalendarioDTO.FechaInicio;

                nuevoCalendario.FechaFin = nuevoCalendarioDTO.FechaFin;

                nuevoCalendario.HoraInicioTurnos = horaInicio;

                nuevoCalendario.HoraFinTurnos = horaFin;

                nuevoCalendario.IntervaloTurnos = intervalo;

                nuevoCalendario.IdUsuario = usuario.Id;

                nuevoCalendario.Usuario = usuario;


                _context.Calendarios.Add(nuevoCalendario);

                await _context.SaveChangesAsync();


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

                    horaInicio += intervalo;

                }

                #endregion



                ////Por cada dia cargado, se generan los turnos con cada horario disponible
                await _turnoService.CrearTurnosAsync(listFechas, listHorarios, nuevoCalendario, usuario);


                nuevoCalendario.CantidadHorarios = listHorarios.Count;

                //Guardar los turnos cargados en la base de datos

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return _mapper.Map<CalendarioDTO>(nuevoCalendario);

            }
            catch (Exception)
            {
                await transaccion.RollbackAsync();

                throw;
            }
        }


        public async Task<CalendarioDTO> UpdateNameAsync(string nuevoNombre, string idUsuario)
        {
            var usuario = await _userService.GetByIdAsync(idUsuario);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

            var calendario = await _context.Calendarios.Where(calendario => calendario.IdUsuario == idUsuario).FirstOrDefaultAsync();

            if (calendario == null)throw new NotFoundException();

            calendario.Nombre = nuevoNombre;

            await _context.SaveChangesAsync();

            return _mapper.Map<CalendarioDTO>(calendario);
        }


        public async Task<CalendarioDTO> ExtendAsync(DateTime nuevaFechaFin, string idUsuario)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var usuario = await _userService.GetByIdAsync(idUsuario);

                if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

                var calendario = await _context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (calendario == null) throw new BadRequestException("El usuario no posee un calendario");

                if (calendario.FechaFin.Date > nuevaFechaFin.Date) throw new BadRequestException("La fecha enviada es anterior a la fecha actual de finalización");


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



                //Por cada dia cargado, se generan los turnos con cada horario disponible

                await _turnoService.CrearTurnosAsync(listFechas, listHorarios, calendario, usuario);


                calendario.FechaFin = nuevaFechaFin;

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return _mapper.Map<CalendarioDTO>(calendario);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }


        }


        public async Task<CalendarioDTO> ReduceAsync(DateTime nuevaFechaFin, string idUsuario)
        {
            var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                var usuario = await _userService.GetByIdAsync(idUsuario);

                if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");


                var calendario = await _context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (calendario == null) throw new BadRequestException("El usuario no posee un calendario");

                if (calendario.FechaFin.Date < nuevaFechaFin.Date) throw new BadRequestException("La fecha enviada es superior a la fecha actual de finalización");

                var existeTurnoReservado = await _context.Turnos
                            .Where(turno => turno.IdCalendario == calendario.Id)
                            .Where(turno => turno.Fecha.Date > nuevaFechaFin.Date && turno.Disponible == false)
                            .AnyAsync();

                if (existeTurnoReservado) throw new MensajePersonalizadoException("No se puede eliminar alguno de los turnos ya que estan ocupados.Primero debe reorganizar esos turnos");


                var turnosEliminar = await _context.Turnos
                            .Where(turno => turno.IdCalendario == calendario.Id)
                            .Where(turno => turno.Fecha.Date > nuevaFechaFin.Date)
                            .ToListAsync();

                foreach (Turno turno in turnosEliminar)
                {
                    _context.Remove(turno);
                }

                calendario.FechaFin = nuevaFechaFin;

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return _mapper.Map<CalendarioDTO>(calendario);

            }
            catch (Exception)
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id, string idUsuario)
        {
            var transaccion = await _context.Database.BeginTransactionAsync();
            try
            {
                var usuario = await _userService.GetByIdAsync(idUsuario);

                if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");


                var calendario = await _context.Calendarios.Where(c => c.Id == id && c.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (calendario == null) throw new NotFoundException();



                var turnos = await _context.Turnos.Where(turno => turno.IdCalendario == calendario.Id).ToListAsync();

                turnos.ForEach(turno => _context.Remove(turno));

                _context.Remove(calendario);

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();

            }
            catch (Exception)
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

    }
}
