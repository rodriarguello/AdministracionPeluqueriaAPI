using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Services
{
    public class AlergiaService: GenericService<Alergia,AlergiaCreacionDTO,AlergiaDTO>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;

        public AlergiaService(ApplicationDbContext context,IMapper mapper, UserManager<Usuario> userManager):base(context, mapper, userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public override async Task DeleteAsync(int idAlergia, string emailUsuario)
        {
            var usuario = await _userManager.FindByEmailAsync(emailUsuario);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

            var alergia = await _context.Alergias.Where(alergia => alergia.IdUsuario == usuario.Id)
                                                .Include(alergia => alergia.MascotasAlergia)
                                                .FirstOrDefaultAsync(alergia => alergia.Id == idAlergia);

            if (alergia == null) throw new BadRequestException("No existe una alergia con el id especificado");


            if (alergia.MascotasAlergia.Count() > 0) throw new MensajePersonalizadoException("No se puede eliminar porque tiene mascotas asociadas");

            _context.Remove(alergia);

            await _context.SaveChangesAsync();
        }

    }
}
