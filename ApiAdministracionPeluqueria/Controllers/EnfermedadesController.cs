using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.EnfermedadDTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/enfermedades")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EnfermedadesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<Usuario> userManager;
        private readonly ResponseApi responseApi;

        #region Constructor
        public EnfermedadesController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager, ResponseApi responseApi)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.responseApi = responseApi;
        }

        #endregion



        #region MOSTRAR ENFERMEDADES

        [HttpGet]
        public async Task<ActionResult<ModeloRespuesta>> Get()
        {

            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);


                var enfermedades = await context.Enfermedades.Where(enfermedad => enfermedad.IdUsuario == usuario.Id).ToListAsync();

    
                return responseApi.respuestaExitosa(mapper.Map<List<EnfermedadDTO>>(enfermedades));

            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
            }

        }



        #endregion


        #region INSERTAR ENFERMEDAD

        [HttpPost]
        public async Task<ActionResult<ModeloRespuesta>> Post([FromBody] EnfermedadCreacionDTO nuevaEnfermedadDTO)
        {

            try
            {
            
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var nuevaEnfermedad = mapper.Map<Enfermedad>(nuevaEnfermedadDTO);

                nuevaEnfermedad.IdUsuario = usuario.Id;

            
                context.Add(nuevaEnfermedad);
            
                await context.SaveChangesAsync();

                return responseApi.respuestaExitosa();
            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
            }

        }

        #endregion



        #region MODIFICAR ENFERMEDAD

        [HttpPut]
        public async Task<ActionResult<ModeloRespuesta>> Put([FromBody] EnfermedadDTO enfermedadDTO)
        {

            try
            {

                bool existe = await context.Enfermedades.AnyAsync(enfermedad => enfermedad.Id == enfermedadDTO.Id);

                if (!existe) return responseApi.respuestaError("No existe una enfermedad con el Id especificado");


                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;
                var usuario = await userManager.FindByEmailAsync(email);


            
                var enfermedad = mapper.Map<Enfermedad>(enfermedadDTO);
                enfermedad.IdUsuario = usuario.Id;

            
                context.Update(enfermedad);
                await context.SaveChangesAsync();


                return responseApi.respuestaExitosa();
            }
            catch (Exception ex)
            {
                return  responseApi.respuestaError(ex.Message);
            }

        }

        #endregion



        #region ELIMINAR ENFERMEDAD
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> Delete([FromRoute] int id)
        {

            try
            {

                bool existe = await context.Enfermedades.AnyAsync(enfermedad => enfermedad.Id == id);

                if (!existe) return responseApi.respuestaError("No existe una enfermedad con el Id especificado");
                    
                var enfermedad = await context.Enfermedades.FirstOrDefaultAsync(enfermedad => enfermedad.Id == id);

                context.Remove(enfermedad);
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
