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
        public async Task<ActionResult<ModeloRespuesta>> Get()
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var razas = await context.Razas.Where(raza => raza.IdUsuario == usuario.Id).ToListAsync();

               
               return responseApi.respuestaExitosa(mapper.Map<List<RazaDTO>>(razas));

            }
            catch (Exception ex)
            {
               return  responseApi.respuestaError(ex.Message);
               
            }
            
        }

        #endregion


        #region INSERTAR RAZA


        [HttpPost]
        public async Task<ActionResult<ModeloRespuesta>> PostRaza([FromBody]RazaCreacionDTO nuevaRazaDTO)
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

               
                return responseApi.respuestaExitosa(mapper.Map<RazaDTO>(nuevaRaza));


            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);

            }


        }

        #endregion



        #region MODIFICAR RAZA
        [HttpPut]
        public async Task<ActionResult<ModeloRespuesta>> PutRaza([FromBody] RazaDTO razaDTO)
        {

            try
            {


                bool existe = await context.Razas.AnyAsync(raza => raza.Id == razaDTO.Id);

                if (!existe) return responseApi.respuestaError("No existe una raza con el Id especificado");
                 

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;
                var usuario = await userManager.FindByEmailAsync(email);

                var raza = mapper.Map<Raza>(razaDTO);
                raza.IdUsuario = usuario.Id;



                context.Update(raza);
                await context.SaveChangesAsync();

                return responseApi.respuestaExitosa();

            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);

            }

        }


        #endregion



        #region ELIMINAR RAZA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> DeleteRaza([FromRoute]int id)
        {


            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;
                var usuario = await userManager.FindByEmailAsync(email);



                var raza = await context.Razas.Where(raza=>raza.IdUsuario == usuario.Id)
                                               .Include(raza=>raza.Mascotas)
                                               .FirstOrDefaultAsync(raza => raza.Id == id);
                
                if (raza == null) return responseApi.respuestaError("No existe una raza con el Id especificado");

                if (raza.Mascotas.Count() > 0) return responseApi.respuestaErrorEliminacion("No se puede eliminar la raza porque tiene mascotas asociadas");

                context.Razas.Remove(raza);

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
