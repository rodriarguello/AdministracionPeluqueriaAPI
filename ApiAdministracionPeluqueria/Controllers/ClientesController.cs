using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<Usuario> userManager;


        #region Constructor
        public ClientesController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        #endregion



        #region Metodos GET

        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> GetClientes()
        {
            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var clientes = await context.Clientes.Where(cliente=> cliente.IdUsuario == usuario.Id).ToListAsync();


            return mapper.Map<List<ClienteDTO>>(clientes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente([FromRoute]int id)
        {
            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var cliente = await context.Clientes.Where(cliente=>cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(x=> x.Id == id);

            if (cliente == null) return NotFound();



            return mapper.Map<ClienteDTO>(cliente);

        }


        #endregion



        #region Metodos POST

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> PostCliente([FromBody]ClienteCreacionDTO nuevoClienteDTO)
        {
            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var nuevoCliente = mapper.Map<Cliente>(nuevoClienteDTO);

            nuevoCliente.IdUsuario = usuario.Id;
            context.Add(nuevoCliente);


           await context.SaveChangesAsync();


            return mapper.Map<ClienteDTO>(nuevoCliente);
        }

        #endregion



        #region Metodos PUT

        [HttpPut]
        public async Task<ActionResult> PutCliente([FromBody]ClienteDTO clienteDTO)
        {

            bool existe = await context.Clientes.AnyAsync(cliente => cliente.Id == clienteDTO.Id);

            if (!existe) return NotFound();
            
            var cliente = mapper.Map<Cliente>(clienteDTO);
            context.Update(cliente);
            await context.SaveChangesAsync();

            return NoContent();
        }

        #endregion



        #region Metodos DELETE

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCliente([FromRoute] int id)
        {

            bool existe = await context.Clientes.AnyAsync(cliente => cliente.Id == id);

            if (!existe) return NotFound();

            var cliente = await context.Clientes.FirstOrDefaultAsync(cliente => cliente.Id == id);

            context.Remove(cliente);
            await context.SaveChangesAsync();

            return Ok();
        }

        #endregion


    }
}
