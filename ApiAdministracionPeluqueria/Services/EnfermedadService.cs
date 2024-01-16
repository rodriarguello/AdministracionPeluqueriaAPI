using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Services
{
    public class EnfermedadService : GenericService<Enfermedad, EnfermedadCreacionDTO, EnfermedadDTO>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public EnfermedadService(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager) : base(context, mapper, userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async override Task DeleteAsync(int idEntidad, string emailUsuario)
        {
            var usuario = await _userManager.FindByEmailAsync(emailUsuario);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

            var registro = await _context.Enfermedades.Where(x => x.IdUsuario == usuario.Id && x.Id == idEntidad)
                                                .Include(x => x.MascotasEnfermedad)
                                                .FirstOrDefaultAsync();

            if (registro == null) throw new BadRequestException("No existe una enfermedad con el id especificado");


            if (registro.MascotasEnfermedad.Count() > 0) throw new MensajePersonalizadoException("No se puede eliminar porque tiene mascotas asociadas");

            _context.Remove(registro);

            await _context.SaveChangesAsync();
        }
    }
}
