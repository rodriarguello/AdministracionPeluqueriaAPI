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
        private readonly ResponseApi responseApi;


        #region Constructor
        public ClientesController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager, ResponseApi responseApi)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.responseApi = responseApi;
        }
        #endregion



        #region MOSTRAR CLIENTES

        [HttpGet]
        public async Task<ActionResult<ModeloRespuesta>> Get()
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var clientes = await context.Clientes.Include(cliente=>cliente.Mascotas).Where(cliente => cliente.IdUsuario == usuario.Id).ToListAsync();


                return responseApi.respuestaExitosa(mapper.Map<List<ClienteDTO>>(clientes));

            }
            catch (Exception ex)
            {
               return  responseApi.respuestaError(ex.Message);
                
            }

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> GetPorId([FromRoute]int id)
        {
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var cliente = await context.Clientes.Include(cliente=>cliente.Mascotas).Where(cliente=>cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(x=> x.Id == id);

                if (cliente == null) return  responseApi.respuestaError("No existe un cliente con el id especificado");
                 


               return responseApi.respuestaExitosa(mapper.Map<ClienteDTO>(cliente));


            }
            catch (Exception ex)
            {

              return responseApi.respuestaError(ex.Message);
            }

        }


        #endregion



        #region INSERTAR CLIENTE

        [HttpPost]
        public async Task<ActionResult<ModeloRespuesta>> Post([FromBody]ClienteCreacionDTO nuevoClienteDTO)
        {

            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var nuevoCliente = mapper.Map<Cliente>(nuevoClienteDTO);

                nuevoCliente.IdUsuario = usuario.Id;
                context.Add(nuevoCliente);


                await context.SaveChangesAsync();

                return responseApi.respuestaExitosa(mapper.Map<ClienteSinMascotasDTO>(nuevoCliente));
            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
                
            }

        }

        #endregion



        #region MODIFICAR CLIENTES

        [HttpPut]
        public async Task<ActionResult<ModeloRespuesta>> Put([FromBody]ClienteModificarDTO clienteDTO)
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var cliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id)
                    .Include(cliente=>cliente.Mascotas)
                    .FirstOrDefaultAsync(cliente => cliente.Id == clienteDTO.Id);



                

                if (cliente == null) return responseApi.respuestaError("No existe un cliente con Id especificado");

                cliente = mapper.Map(clienteDTO,cliente);


                await context.SaveChangesAsync();

                return responseApi.respuestaExitosa();

            }
            catch (Exception ex)
            {

              return  responseApi.respuestaError(ex.Message);
            }

        }

        #endregion



        #region ELIMINAR CLIENTE

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> Delete([FromRoute] int id)
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var cliente = await context.Clientes.Where(cliente=>cliente.IdUsuario == usuario.Id)
                                                    .Include(cliente=> cliente.Mascotas)
                                                    .FirstOrDefaultAsync(cliente => cliente.Id == id);

                if (cliente==null) return responseApi.respuestaError("No existe un cliente con el Id especificado");

                if (cliente.Mascotas.Count() > 0) return responseApi.respuestaErrorEliminacion("No se puede eliminar el cliente porque tiene mascotas asociadas");

                context.Remove(cliente);
                await context.SaveChangesAsync();

                return responseApi.respuestaExitosa();
            }
            catch (Exception ex)
            {
              return responseApi.respuestaError(ex.Message);
            }

        }

        #endregion


    }
}
