using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.Auth;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.UsuarioDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace ApiAdministracionPeluqueria.Services
{
    public class CuentaService:ICuentaService
    {
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public CuentaService(SignInManager<Usuario> signInManager,IUserService userService, ITokenService tokenService)
        {
            _signInManager = signInManager;
            _userService = userService;
            _tokenService = tokenService;
        }
        
        public async Task<ResAuth> LoginAsync(string email, string password)
        {
            var resultado = await _signInManager.PasswordSignInAsync(email, password,
            isPersistent: false, lockoutOnFailure: false);

            if (!resultado.Succeeded) throw new BadRequestException();

            var usuario = await _userService.GetDtoByEmailAsync(email);

            var respuesta = new ResAuth
            {
                Token = _tokenService.ConstruirToken(email, usuario.Id),
                Usuario = usuario
            };

            return respuesta;
        }

        public async Task RegisterAsync(CreacionUsuarioDTO creacionUsuarioDTO)
        {
            await _userService.CreateAsync(creacionUsuarioDTO);   
        }

    }
}
