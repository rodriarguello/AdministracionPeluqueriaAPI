using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/alergias")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AlergiasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<Usuario> userManager;
        private readonly ResponseApi responseApi;

        #region Constructor
        public AlergiasController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager, ResponseApi responseApi)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.responseApi = responseApi;

        }

        #endregion



        #region MOSTRAR ALERGIAS

        [HttpGet]
        public async Task<ActionResult<ResponseApi>> Get()
        {

            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);


                var alergias = await context.Alergias.Where(alergia => alergia.IdUsuario == usuario.Id).ToListAsync();



                responseApi.Resultado = 1;
                responseApi.Mensaje = "";
                responseApi.Data = mapper.Map<List<AlergiaDTO>>(alergias);




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


        #region INSERTAR ALERGIA

        [HttpPost]
        public async Task<ActionResult<ResponseApi>> Post([FromBody]AlergiaCreacionDTO nuevaAlergiaDTO)
        {



            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var nuevaAlergia = mapper.Map<Alergia>(nuevaAlergiaDTO);

                nuevaAlergia.IdUsuario = usuario.Id;

                context.Add(nuevaAlergia);
                await context.SaveChangesAsync();

                responseApi.Resultado = 1;
                responseApi.Mensaje = "";
                responseApi.Data = mapper.Map<AlergiaDTO>(nuevaAlergia);
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



        #region MODIFICAR ALERGIA

        [HttpPut]
        public async Task<ActionResult<ResponseApi>> Put([FromBody] AlergiaDTO alergiaDTO)
        {
            try
            {
                bool existe = await context.Alergias.AnyAsync(alergia=> alergia.Id == alergiaDTO.Id);


                if (!existe)
                {

                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe una alergia con el Id especificado";
                    responseApi.Data = null;


                    return responseApi;

                }
            
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;
                var usuario = await userManager.FindByEmailAsync(email);

            
            
                var alergia = mapper.Map<Alergia>(alergiaDTO);
                alergia.IdUsuario = usuario.Id;

            
            
                context.Update(alergia);
                await context.SaveChangesAsync();


                responseApi.Resultado = 1;
                responseApi.Mensaje = "Se actualizo correctamente";
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



        #region ELIMINAR ALERGIA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseApi>> Delete([FromRoute]int id)
        {

            try
            {
                bool existe = await context.Alergias.AnyAsync(alergia => alergia.Id == id);

                if (!existe)
                {
                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe una alergia con el Id especificado";
                    responseApi.Data = null;

                    return responseApi;
                }        
                
                var alergia = await context.Alergias.FirstOrDefaultAsync(alergia => alergia.Id == id);

                context.Remove(alergia);
                await context.SaveChangesAsync();

                responseApi.Resultado = 1;
                responseApi.Mensaje = "";
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
