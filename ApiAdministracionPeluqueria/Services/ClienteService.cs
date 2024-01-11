using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;

namespace ApiAdministracionPeluqueria.Services
{
    public class ClienteService : IClienteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ClienteService(ApplicationDbContext context,IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }
        public async Task<List<ClienteSinMascotasDTO>> GetAllByIdUserAsync(string idUsuario)
        {
            var clientes = await _context.Clientes.Where(cliente => cliente.IdUsuario == idUsuario).ToListAsync();

            return _mapper.Map<List<ClienteSinMascotasDTO>>(clientes);
        }

        public async Task<ClienteDTO> GetByIdAsync(int idCliente, string idUsuario)
        {
            var cliente = await _context.Clientes.Where(c => c.Id == idCliente && c.IdUsuario == idUsuario).Include(c => c.Mascotas).FirstOrDefaultAsync();

            if (cliente == null) throw new NotFoundException();

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<List<TurnoDTO>> GetTurnosTomadosAsync(int idCliente, string idUsuario)
        {

            var existeCliente = await _context.Clientes.Where(c => c.Id == idCliente && c.IdUsuario == idUsuario).AnyAsync();

            if(!existeCliente) throw new NotFoundException();

            var turnosCliente = await _context.Clientes.Where(cliente => cliente.Id == idCliente && cliente.IdUsuario == idUsuario)
                .Include(cliente => cliente.Mascotas!)
                .ThenInclude(mascotas => mascotas.Turnos)
                .SelectMany(cliente => cliente.Mascotas!.SelectMany(m => m.Turnos)).ToListAsync();

            return _mapper.Map<List<TurnoDTO>>(turnosCliente);

        }


        public  async Task<ClienteSinMascotasDTO> CreateAsync(ClienteCreacionDTO dtoCreacion, string emailUsuario)
        {
            var usuario = await _userService.GetDtoByEmailAsync(emailUsuario);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el email especificado"); 

            var nuevoCliente = _mapper.Map<Cliente>(dtoCreacion);

            nuevoCliente.IdUsuario = usuario.Id;

            nuevoCliente.FechaCreacion = DateTime.Now;

            _context.Add(nuevoCliente);

            await _context.SaveChangesAsync();

            return _mapper.Map<ClienteSinMascotasDTO>(nuevoCliente);
        }


        public async Task<ClienteSinMascotasDTO> UpdateAsync(int idEntidad, ClienteCreacionDTO dtoCreacion, string email)
        {
            var usuario = await _userService.GetDtoByEmailAsync(email);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el email especificado");

            var cliente = await _context.Clientes.Where(cliente => cliente.Id == idEntidad && cliente.IdUsuario == usuario.Id)
                                                 .FirstOrDefaultAsync();

            if (cliente == null) throw new NotFoundException();

            cliente.Nombre = dtoCreacion.Nombre;
            cliente.Email = dtoCreacion.Email;
            cliente.Telefono = dtoCreacion.Telefono;
            
            await _context.SaveChangesAsync();

            return _mapper.Map<ClienteSinMascotasDTO>(cliente);
        }
        public async Task DeleteAsync(int idEntidad, string emailUsuario)
        {
            var usuario = await _userService.GetDtoByEmailAsync(emailUsuario);

            if (usuario == null) throw new BadRequestException("No existe un usuario con el email especificado");

            var clienteConMascotas = await _context.Clientes.Where(c => c.Id == idEntidad && c.IdUsuario == usuario.Id).Include( c => c.Mascotas).FirstOrDefaultAsync();

            if(clienteConMascotas == null) throw new NotFoundException();

            if (clienteConMascotas.Mascotas.Count > 0) throw new MensajePersonalizadoException("El cliente no se puede eliminar porque tiene mascotas asociadas.");

            _context.Remove(clienteConMascotas);
            
            await _context.SaveChangesAsync();

        }

    }
}
