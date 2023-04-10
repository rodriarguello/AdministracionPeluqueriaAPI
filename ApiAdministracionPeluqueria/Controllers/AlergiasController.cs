using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
        public async Task<ActionResult<ModeloRespuesta>> Get()
        {

            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);


                var alergias = await context.Alergias.Where(alergia => alergia.IdUsuario == usuario.Id).ToListAsync();


                return responseApi.respuestaExitosa(mapper.Map<List<AlergiaDTO>>(alergias));


            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
            }
            

        }



        #endregion


        #region INSERTAR ALERGIA

        [HttpPost]
        public async Task<ActionResult<ModeloRespuesta>> Post([FromBody]AlergiaCreacionDTO nuevaAlergiaDTO)
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

                return responseApi.respuestaExitosa(mapper.Map<AlergiaDTO>(nuevaAlergia));
            }
            catch (Exception ex)
            {

                return responseApi.respuestaError(ex.Message);
 
            }

        }

        #endregion



        #region MODIFICAR ALERGIA

        [HttpPut]
        public async Task<ActionResult<ModeloRespuesta>> Put([FromBody] AlergiaDTO alergiaDTO)
        {
            try
            {
                bool existe = await context.Alergias.AnyAsync(alergia => alergia.Id == alergiaDTO.Id);


                if (!existe) return responseApi.respuestaError("No existe una alergia con el Id especificado");

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;
                var usuario = await userManager.FindByEmailAsync(email);



                var alergia = mapper.Map<Alergia>(alergiaDTO);
                alergia.IdUsuario = usuario.Id;



                context.Update(alergia);
                await context.SaveChangesAsync();

                return responseApi.respuestaExitosa();
            }
            catch (Exception ex)
            {
                
                return responseApi.respuestaError(ex.Message);

            }
        }

        #endregion



        #region ELIMINAR ALERGIA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> Delete([FromRoute]int id)
        {

            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;
                var usuario = await userManager.FindByEmailAsync(email);
                
                var alergia = await context.Alergias.Where(alergia=>alergia.IdUsuario == usuario.Id).FirstOrDefaultAsync(alergia => alergia.Id == id);

                if (alergia == null) return responseApi.respuestaError("No existe una alergia con el Id especificado");

                context.Remove(alergia);
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
