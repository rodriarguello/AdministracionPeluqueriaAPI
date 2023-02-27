using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.ClienteDTO;
using ApiAdministracionPeluqueria.Utilidades;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/clientes")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ClientesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> GetClientes()
        {


            var clientes = await context.Clientes.ToListAsync();


            return mapper.Map<List<ClienteDTO>>(clientes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClienteDTO>> GetCliente([FromRoute]int id)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(x=> x.Id == id);

            if (cliente == null) return NotFound();



            return mapper.Map<ClienteDTO>(cliente);

        }


        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> PostCliente([FromBody]ClienteCreacionDTO nuevoClienteDTO)
        {

            var nuevoCliente = mapper.Map<Cliente>(nuevoClienteDTO);

            context.Add(nuevoCliente);
           await context.SaveChangesAsync();


            return mapper.Map<ClienteDTO>(nuevoCliente);
        }

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




    }
}
