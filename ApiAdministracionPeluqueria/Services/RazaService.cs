using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiAdministracionPeluqueria.Services
{
    public class RazaService : GenericService<Raza, RazaCreacionDTO, RazaDTO>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public RazaService(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager) : base(context, mapper, userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async override Task DeleteAsync(int idEntidad, string emailUsuario)
        {
            var usuario = await _userManager.FindByEmailAsync(emailUsuario);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

            var raza = await _context.Razas.Where(raza => raza.IdUsuario == usuario.Id && raza.Id == idEntidad)
                                              .Include(raza => raza.Mascotas)
                                              .FirstOrDefaultAsync();

            if (raza == null) throw new BadRequestException("No existe una raza con el Id especificado");


            if (raza.Mascotas.Count() > 0) throw new MensajePersonalizadoException("No se puede eliminar la raza porque tiene mascotas asociadas");

            _context.Remove(raza);

            await _context.SaveChangesAsync();
        }
    }
}
