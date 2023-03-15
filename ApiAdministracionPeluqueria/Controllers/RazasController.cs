using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.RazaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/razas")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RazasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<Usuario> userManager;
        private readonly ResponseApi responseApi;

        #region Constructor


        public RazasController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager, ResponseApi responseApi)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.responseApi = responseApi;
        }

        #endregion



        #region MOSTRAR RAZAS
        [HttpGet]
        public async Task<ActionResult<ResponseApi>> Get()
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var razas = await context.Razas.Where(raza => raza.IdUsuario == usuario.Id).ToListAsync();

                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mapper.Map<List<RazaDTO>>(razas);

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


        #region INSERTAR RAZA


        [HttpPost]
        public async Task<ActionResult<ResponseApi>> PostRaza([FromBody]RazaCreacionDTO nuevaRazaDTO)
        {

            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var nuevaRaza = mapper.Map<Raza>(nuevaRazaDTO);

                nuevaRaza.IdUsuario = usuario.Id;


                context.Add(nuevaRaza);

                await context.SaveChangesAsync();

               
                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mapper.Map<RazaDTO>(nuevaRaza);


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



        #region MODIFICAR RAZA
        [HttpPut]
        public async Task<ActionResult<ResponseApi>> PutRaza([FromBody] RazaDTO razaDTO)
        {

            try
            {


                bool existe = await context.Razas.AnyAsync(raza => raza.Id == razaDTO.Id);

                if (!existe)
                {

                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe una raza con el Id especificado";
                    responseApi.Data = null;

                    return responseApi;
                }

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;
                var usuario = await userManager.FindByEmailAsync(email);



                var raza = mapper.Map<Raza>(razaDTO);
                raza.IdUsuario = usuario.Id;



                context.Update(raza);
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



        #region ELIMINAR RAZA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseApi>> DeleteRaza([FromRoute]int id)
        {


            try
            {
                bool existe = await context.Razas.AnyAsync(raza => raza.Id == id);

                if (!existe) {

                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe una raza con el Id especificado";
                    responseApi.Data = null;

                    return responseApi;
                }

                var raza = await context.Razas.FirstOrDefaultAsync(raza => raza.Id == id);

                context.Razas.Remove(raza);

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
