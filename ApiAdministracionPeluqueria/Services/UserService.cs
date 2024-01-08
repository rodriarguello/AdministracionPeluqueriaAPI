using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ApiAdministracionPeluqueria.Services
{
    public class UserService:IUserService
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<Usuario> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UsuarioDTO> GetByEmailAsync(string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);

            return _mapper.Map<UsuarioDTO>(usuario);
        }

        public async Task CreateAsync(CreacionUsuarioDTO creacionUsuarioDTO)
        {
            var usuario = _mapper.Map<Usuario>(creacionUsuarioDTO);

            usuario.UserName = creacionUsuarioDTO.Email;

            var resultado = await _userManager.CreateAsync(usuario, creacionUsuarioDTO.Password);

            if (!resultado.Succeeded) throw new BadRequestException(string.Join("\n",resultado.Errors.Select(error => error.Description)));
        }
    }
}
