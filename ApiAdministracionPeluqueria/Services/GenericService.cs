﻿using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.Interfaces;
using ApiAdministracionPeluqueria.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Services
{
    public abstract class GenericService<TEntidad, TCreacionDTO, TEntidadDTO> : IGenericService<TCreacionDTO, TEntidadDTO> 
        where TEntidad : class, IIdUsuario
        where TCreacionDTO: class, INombre
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;

        public GenericService(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<TEntidadDTO> Create(TCreacionDTO dtoCreacion, string emailUsuario)
        {
            var nuevoRegistro = _mapper.Map<TEntidad>(dtoCreacion);

            var usuario = await _userManager.FindByEmailAsync(emailUsuario);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

            var existe = await _context.Alergias.Where(x => x.Nombre == dtoCreacion.Nombre).AnyAsync();

            if (existe) throw new MensajePersonalizadoException("Ya existe un registro con ese nombre");

            nuevoRegistro.IdUsuario = usuario.Id;

            _context.Set<TEntidad>().Add(nuevoRegistro);

            await _context.SaveChangesAsync();

            return _mapper.Map<TEntidadDTO>(nuevoRegistro);
        }

        public abstract Task Delete(int idEntidad, string emailUsuario);
        //{
            //var usuario = await _userManager.FindByEmailAsync(emailUsuario);

            //if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

            //var alergia = await _context.Set<TEntidad>().Where(alergia => alergia.IdUsuario == usuario.Id)
            //                                    .Include(alergia => alergia.MascotasAlergia)
            //                                    .FirstOrDefaultAsync(alergia => alergia.Id == idEntidad);

            //if (alergia == null) throw new BadRequestException("No existe una alergia con el id especificado");


            //if (alergia.MascotasAlergia.Count() > 0) throw new MensajePersonalizadoException("No se puede eliminar porque tiene mascotas asociadas");

            //_context.Remove(alergia);

            //await _context.SaveChangesAsync();
        //}


        public async Task<List<TEntidadDTO>> GetAllByIdUser(string idUsuario)
        {
            var registros = await _context.Set<TEntidad>().Where(alergia => alergia.IdUsuario == idUsuario).ToListAsync();

            return _mapper.Map<List<TEntidadDTO>>(registros);

        }

        public async Task<TEntidadDTO> Update(int idEntidad, TCreacionDTO dtoCreacion, string emailUsuario)
        {
            var usuario = await _userManager.FindByEmailAsync(emailUsuario);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");


            var registro = await _context.Set<TEntidad>().Where(x => x.Id == idEntidad && x.IdUsuario == usuario.Id).FirstOrDefaultAsync();

            if (registro == null) throw new BadRequestException("No existe un registro con el id especificado");

            registro.Nombre = dtoCreacion.Nombre;

            await _context.SaveChangesAsync();

            return _mapper.Map<TEntidadDTO>(registro);
        }

        
    }
}
