using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.AlergiaDTO;
using ApiAdministracionPeluqueria.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<ResponseApi>> Get()
        {

            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);


                var enfermedades = await context.Enfermedades.Where(enfermedad => enfermedad.IdUsuario == usuario.Id).ToListAsync();


                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mapper.Map<List<EnfermedadDTO>>(enfermedades) ;

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


        #region INSERTAR ENFERMEDAD

        [HttpPost]
        public async Task<ActionResult<ResponseApi>> Post([FromBody] EnfermedadCreacionDTO nuevaEnfermedadDTO)
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

                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mapper.Map<EnfermedadDTO>(nuevaEnfermedad);

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



        #region MODIFICAR ENFERMEDAD

        [HttpPut]
        public async Task<ActionResult<ResponseApi>> Put([FromBody] EnfermedadDTO enfermedadDTO)
        {

            try
            {

                bool existe = await context.Enfermedades.AnyAsync(enfermedad => enfermedad.Id == enfermedadDTO.Id);

                if (!existe){

                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe una enfermedad con el Id especificado";
                    responseApi.Data = null;

                    return NotFound(responseApi); 
                }


                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;
                var usuario = await userManager.FindByEmailAsync(email);


            
                var enfermedad = mapper.Map<Enfermedad>(enfermedadDTO);
                enfermedad.IdUsuario = usuario.Id;

            
                context.Update(enfermedad);
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



        #region ELIMINAR ENFERMEDAD
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseApi>> Delete([FromRoute] int id)
        {


            try
            {

                bool existe = await context.Enfermedades.AnyAsync(enfermedad => enfermedad.Id == id);

                if (!existe)
                {
                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe una enfermedad con el Id especificado";
                    responseApi.Data = null;

                    return NotFound(responseApi);

                }
                    
                var enfermedad = await context.Enfermedades.FirstOrDefaultAsync(enfermedad => enfermedad.Id == id);

                context.Remove(enfermedad);
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
