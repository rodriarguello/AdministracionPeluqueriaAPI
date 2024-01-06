using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Services
{
    public class AlergiaService: IGenericService<AlergiaCreacionDTO, AlergiaDTO>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<Usuario> _userManager;

        public AlergiaService(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<List<AlergiaDTO>> GetByIdUser(string idUsuario)
        {
               
            var alergias = await _context.Alergias.Where(alergia => alergia.IdUsuario == idUsuario).ToListAsync();

            return _mapper.Map<List<AlergiaDTO>>(alergias);

        }

        public async Task<AlergiaDTO> Create(AlergiaCreacionDTO alergiaCreacionDTO, string emailUsuario)
        {

            var nuevaAlergia = _mapper.Map<Alergia>(alergiaCreacionDTO);

            var usuario = await _userManager.FindByEmailAsync(emailUsuario);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");

            var existe = await _context.Alergias.Where(x => x.Nombre == nuevaAlergia.Nombre).AnyAsync();

            if (existe) throw new MensajePersonalizadoException("Ya existe una alergia con ese nombre");

            nuevaAlergia.IdUsuario = usuario.Id;

            _context.Alergias.Add(nuevaAlergia);

            await _context.SaveChangesAsync();

            return _mapper.Map<AlergiaDTO>(nuevaAlergia);

        }

        public async Task<AlergiaDTO> Update(int idAlergia, AlergiaCreacionDTO alergiaDTO, string emailUsuario)
        {

            var usuario = await _userManager.FindByEmailAsync(emailUsuario);

            if(usuario == null) throw new BadRequestException("No existe un usuario con el id especificado");


            var alergia = await _context.Alergias.Where(x => x.Id == idAlergia && x.IdUsuario == usuario.Id).FirstOrDefaultAsync();

            if (alergia == null) throw new BadRequestException("No existe una alergia con el id especificado");

            alergia.Nombre = alergiaDTO.Nombre;

            await _context.SaveChangesAsync();

            return _mapper.Map<AlergiaDTO>(alergia);

        }

        public async Task Delete(int idAlergia, string emailUsuario)
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
