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
        public async Task<ActionResult<ResponseApi>> Get()
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var clientes = await context.Clientes.Include(cliente=>cliente.Mascotas).Where(cliente => cliente.IdUsuario == usuario.Id).ToListAsync();

                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data= mapper.Map<List<ClienteDTO>>(clientes);
            }
            catch (Exception ex)
            {
                responseApi.Resultado=0;
                responseApi.Mensaje = ex.Message;
                responseApi.Data = null;
                
            }

            return responseApi;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseApi>> GetPorId([FromRoute]int id)
        {
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var cliente = await context.Clientes.Include(cliente=>cliente.Mascotas).Where(cliente=>cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(x=> x.Id == id);

                if (cliente == null)
                {
                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe un cliente con el id especificado";
                    responseApi.Data = null;

                    return responseApi;
                }


                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mapper.Map<ClienteDTO>(cliente);


            }
            catch (Exception ex)
            {

                responseApi.Resultado = 0;
                responseApi.Mensaje = ex.Message;
                responseApi.Data = null;
            }

            return responseApi;
        }


        #endregion



        #region INSERTAR CLIENTE

        [HttpPost]
        public async Task<ActionResult<ResponseApi>> Post([FromBody]ClienteCreacionDTO nuevoClienteDTO)
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

                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mapper.Map<ClienteDTO>(nuevoCliente);

            }
            catch (Exception ex)
            {
                responseApi.Resultado = 0;
                responseApi.Mensaje = ex.Message;
                responseApi.Data = null;
                
            }

            return responseApi;
        }

        #endregion



        #region MODIFICAR CLIENTES

        [HttpPut]
        public async Task<ActionResult<ResponseApi>> Put([FromBody]ClienteDTO clienteDTO)
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var cliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(cliente => cliente.Id == clienteDTO.Id);

                if (cliente == null)
                {
                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe un cliente con Id especificado";
                    responseApi.Data = null;

                    return responseApi;
                }

               

                cliente = mapper.Map<Cliente>(clienteDTO);
            

                context.Update(cliente);
            
                await context.SaveChangesAsync();

                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data  = null;


            }
            catch (Exception ex)
            {

                responseApi.Resultado = 0;
                responseApi.Mensaje = ex.Message;
                responseApi.Data = null;
            }

            return responseApi;
        }

        #endregion



        #region ELIMINAR CLIENTE

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseApi>> Delete([FromRoute] int id)
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var cliente = await context.Clientes.Where(cliente=>cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(cliente => cliente.Id == id);

                if (cliente==null)
                {
                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe un cliente con el Id especificado";
                    responseApi.Data = null;
                    return responseApi;
                }

                

                context.Remove(cliente);
                await context.SaveChangesAsync();

                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = null;

            }
            catch (Exception ex)
            {
                responseApi.Resultado = 0;
                responseApi.Mensaje = ex.Message;
                responseApi.Data = null;
            }

            return responseApi;
        }

        #endregion


    }
}
