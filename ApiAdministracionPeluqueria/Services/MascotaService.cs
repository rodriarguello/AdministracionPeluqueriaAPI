using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;

namespace ApiAdministracionPeluqueria.Services
{
    public class MascotaService : IMascotaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;

        public MascotaService(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<List<MascotaDTO>> GetAllByIdUserAsync(string idUsuario)
        {
                var mascotas = await _context.Mascotas.Where(mascota => mascota.IdUsuario == idUsuario)
                                                 .Include(mascotas => mascotas.Cliente)
                                                 .Include(mascotas => mascotas.Raza)
                                                 .Include(mascotas => mascotas.MascotaEnfermedades)
                                                         .ThenInclude(mascotaEnfermedad => mascotaEnfermedad.Enfermedad)
                                                 .Include(mascotas => mascotas.MascotaAlergias)
                                                          .ThenInclude(mascotaAlergia => mascotaAlergia.Alergia)
                                                 .Include(mascota => mascota.Turnos)
                                                 .ToListAsync();
                return _mapper.Map<List<MascotaDTO>>(mascotas);
        }
        public async Task<MascotaDTO> GetByIdAsync(int id, string idUsuario)
        {
            var mascota = await _context.Mascotas.Where(mascota => mascota.IdUsuario == idUsuario)
                                                 .Include(mascotas => mascotas.Cliente)
                                                 .Include(mascotas => mascotas.Raza)
                                                 .Include(mascotas => mascotas.MascotaEnfermedades)
                                                         .ThenInclude(mascotaEnfermedad => mascotaEnfermedad.Enfermedad)
                                                 .Include(mascotas => mascotas.MascotaAlergias)
                                                          .ThenInclude(mascotaAlergia => mascotaAlergia.Alergia)
                                                 .Include(mascota => mascota.Turnos)
                                                 .ToListAsync();

            if (mascota == null) throw new NotFoundException();
            
            return _mapper.Map<MascotaDTO>(mascota);
        }
        public async Task<MascotaDTO> CreateAsync(MascotaCreacionDTO nuevaMascotaDTO, string emailUsuario)
        {
            var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuario = await _userManager.FindByEmailAsync(emailUsuario);


                var cliente = await _context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(cliente => cliente.Id == nuevaMascotaDTO.IdCliente);

                if (cliente == null) throw new BadRequestException("No existe un Cliente con el Id especificado");


                var raza = await _context.Razas.Where(razas => razas.IdUsuario == usuario.Id).FirstOrDefaultAsync(razas => razas.Id == nuevaMascotaDTO.IdRaza);

                if (raza == null) throw new BadRequestException("No existe una Raza con el Id especificado");

                //ENFERMEDADES

                if (nuevaMascotaDTO.IdEnfermedades.Count < 1) throw new BadRequestException("El campo enfermedad es obligatorio");


                var listEnfermedades = await _context.Enfermedades
                                             .Where(enfermedad => nuevaMascotaDTO.IdEnfermedades.Contains(enfermedad.Id)).ToListAsync();

                if (listEnfermedades.Count != nuevaMascotaDTO.IdEnfermedades.Count) throw new BadRequestException("Por lo menos una de las enfermedades enviadas no existe");

                //ALERGIAS

                if (nuevaMascotaDTO.IdAlergias.Count < 1) throw new BadRequestException("El campo alergia es obligatorio");


                var listAlergias = await _context.Alergias
                                             .Where(alergia => nuevaMascotaDTO.IdAlergias.Contains(alergia.Id)).ToListAsync();

                if (listAlergias.Count != nuevaMascotaDTO.IdAlergias.Count) throw new BadRequestException("Por lo menos una de las alergias enviadas no existe");


                var nuevaMascota = _mapper.Map<Mascota>(nuevaMascotaDTO);

                nuevaMascota.Cliente = cliente;

                nuevaMascota.Raza = raza;
                nuevaMascota.IdUsuario = usuario.Id;

                nuevaMascota.FechaCreacion = DateTime.Now;

                _context.Mascotas.Add(nuevaMascota);



                await _context.SaveChangesAsync();


                foreach (var enfermedad in listEnfermedades)
                {

                    _context.MascotasEnfermedades.Add(new MascotaEnfermedad
                    {

                        IdMascota = nuevaMascota.Id,

                        IdEnfermedad = enfermedad.Id,

                        Enfermedad = enfermedad,

                        Mascota = nuevaMascota,


                        IdUsuario = usuario.Id
                    });
                }


                foreach (var alergia in listAlergias)
                {

                    _context.MascotasAlergias.Add(new MascotaAlergia
                    {

                        IdMascota = nuevaMascota.Id,

                        IdAlergia = alergia.Id,

                        Alergia = alergia,

                        Mascota = nuevaMascota,

                        IdUsuario = usuario.Id
                    });

                }


                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();




                return _mapper.Map<MascotaDTO>(nuevaMascota);
            }
            catch (Exception)
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }
        

        public async Task<MascotaDTO> UpdateAsync(int idEntidad, MascotaModificarDTO mascotaDTO, string emailUsuario)
        {
            var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {

                var usuario = await _userManager.FindByEmailAsync(emailUsuario);

                var mascota = await _context.Mascotas.Where(m => m.Id == idEntidad && m.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (mascota == null) throw new NotFoundException();


                var cliente = await _context.Clientes.Where(c => c.Id == mascotaDTO.IdCliente && c.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (cliente == null) throw new BadRequestException("No existe un Cliente con el Id especificado");


                var raza = await _context.Razas.Where(r => r.Id == mascotaDTO.IdRaza &&  r.IdUsuario == usuario.Id).FirstOrDefaultAsync();

                if (raza == null) throw new BadRequestException("No existe una Raza con el Id especificado");

                //VALIDACION ENFERMEDADES

                if (mascotaDTO.IdEnfermedades.Count < 1) throw new BadRequestException("El campo enfermedad es obligatorio");

                var nuevasEnfermedades = await _context.Enfermedades.Where(enfermedad => mascotaDTO.IdEnfermedades.Contains(enfermedad.Id)).ToListAsync();

                if (nuevasEnfermedades.Count != mascotaDTO.IdEnfermedades.Count) throw new BadRequestException("Alguna de las enfermedades enviadas no existe");


                //VALIDACION ALERGIAS


                if (mascotaDTO.IdAlergias.Count < 1) throw new BadRequestException("El campo alergia es obligatorio");

                var nuevasAlergias = await _context.Alergias.Where(alergia => mascotaDTO.IdAlergias.Contains(alergia.Id)).ToListAsync();

                if (nuevasAlergias.Count != mascotaDTO.IdAlergias.Count) throw new BadRequestException("Alguna de las alergias enviadas no existe");

                mascota.IdCliente = mascotaDTO.IdCliente;
                mascota.Cliente = cliente;
                mascota.IdRaza = mascotaDTO.IdRaza;
                mascota.Raza = raza;
                mascota.Nombre = mascotaDTO.Nombre;
                mascota.FechaNacimiento = mascotaDTO.FechaNacimiento;


                //ENFERMEDADES

                var enfermedadesEliminar = await _context.MascotasEnfermedades
                                                .Where(me => me.IdMascota == mascota.Id && me.IdUsuario == usuario.Id)
                                                .Where(enfermedadMascota => !mascotaDTO.IdEnfermedades.Contains(enfermedadMascota.IdEnfermedad)).ToListAsync();


                //Enfermedades existentes que tienen el mismo id que las enfermedades que se envían con la mascota

                var enfermedadesExistentes = await _context.MascotasEnfermedades
                                                .Where(me => me.IdMascota == mascota.Id && me.IdUsuario == usuario.Id)
                                                .Where(enfermedadMascota => mascotaDTO.IdEnfermedades.Contains(enfermedadMascota.IdEnfermedad)).ToListAsync();

                if (enfermedadesEliminar.Count > 0)
                {

                    foreach (var enfermedadEliminar in enfermedadesEliminar)
                    {

                        _context.MascotasEnfermedades.Remove(enfermedadEliminar);

                    }

                }




                var IdEnfermedadesExistentes = new List<int>();

                //Se extrae el Id de la enfermedad y se lo guarda en una lista

                foreach (var enfermedadExistente in enfermedadesExistentes)
                {
                    IdEnfermedadesExistentes.Add(enfermedadExistente.IdEnfermedad);
                }

                //Se compara los valores de la lista con los id de las enfermedades existentes, y cuando no hay igualdad, 
                //se guarda la enfermedad para cargarla en la base de datos

                var cargarEnfermedades = nuevasEnfermedades.Where(nuevaEnfermedad => !IdEnfermedadesExistentes.Contains(nuevaEnfermedad.Id)).ToList();


                foreach (var enfermedad in cargarEnfermedades)
                {
                    _context.MascotasEnfermedades.Add(new MascotaEnfermedad
                    {
                        IdEnfermedad = enfermedad.Id,
                        Enfermedad = enfermedad,
                        Mascota = mascota,
                        IdMascota = mascota.Id,
                        IdUsuario = usuario.Id
                    });
                }

                //ALERGIAS


                var alergiasEliminar = await _context.MascotasAlergias
                                                .Where(ma => ma.IdMascota == mascota.Id && ma.IdUsuario == usuario.Id)
                                                .Where(mascotaAlergia => !mascotaDTO.IdAlergias.Contains(mascotaAlergia.IdAlergia)).ToListAsync();


                //Alergias existentes que tienen el mismo id que las alergias que se envían con la mascota

                var alergiasExistentes = await _context.MascotasAlergias
                                                .Where(ma => ma.IdMascota == mascota.Id && ma.IdUsuario == usuario.Id)
                                                .Where(mascotaAlergia => mascotaDTO.IdAlergias.Contains(mascotaAlergia.IdAlergia)).ToListAsync();

                if (alergiasEliminar.Count > 0)
                {

                    foreach (var alergiaEliminar in alergiasEliminar)
                    {

                        _context.MascotasAlergias.Remove(alergiaEliminar);

                    }

                }


                var idAlergiasExistentes = new List<int>();


                foreach (var alergia in alergiasExistentes)
                {

                    idAlergiasExistentes.Add(alergia.IdAlergia);

                }

                var agregarAlergias = nuevasAlergias.Where(nuevaAlergia => !idAlergiasExistentes.Contains(nuevaAlergia.Id)).ToList();


                foreach (var alergia in agregarAlergias)
                {
                    _context.MascotasAlergias.Add(new MascotaAlergia
                    {
                        IdAlergia = alergia.Id,
                        Alergia = alergia,
                        IdMascota = mascota.Id,
                        Mascota = mascota,
                        IdUsuario = usuario.Id
                    });
                }

                await _context.SaveChangesAsync();

                await transaccion.CommitAsync();


                return _mapper.Map<MascotaDTO>(mascota);

            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int idEntidad, string emailUsuario)
        {
            var transaccion = await _context.Database.BeginTransactionAsync();

            try
            {
                var usuario = await _userManager.FindByEmailAsync(emailUsuario);

                if (usuario == null) throw new BadRequestException("No existe un usuario con el email especificado");

                var mascota = await _context.Mascotas.Where(m => m.Id == idEntidad && m.IdUsuario == usuario.Id)

                                                    .Include(mascota => mascota.MascotaEnfermedades)

                                                    .Include(mascota => mascota.MascotaAlergias)

                                                    .Include(mascota => mascota.Turnos)

                                                    .FirstOrDefaultAsync();

                if (mascota == null) throw new NotFoundException("No existe una mascota con el Id especificado");

                if (mascota.Turnos.Count > 0) throw new MensajePersonalizadoException("Error al eliminar la mascota porque tiene turnos asociados");

                _context.Remove(mascota);
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
