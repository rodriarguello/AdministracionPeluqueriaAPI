using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/mascotas")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MascotasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<Usuario> userManager;
        private readonly ResponseApi responseApi;

        public MascotasController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager, ResponseApi responseApi)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.responseApi = responseApi;
        }



        #region MOSTRAR MASCOTAS

        [HttpGet]
        public async Task<ActionResult<ResponseApi>> MostrarMascotas()
        {

            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim=>claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var mascotas = new List<Mascota>();
                
                mascotas = await context.Mascotas.Include(mascotas=>mascotas.Cliente).
                                                  Include(mascotas=>mascotas.Raza).
                                                  Include(mascotas=>mascotas.Enfermedad).
                                                  Include(mascotas=>mascotas.Alergia).
                                                  Where(mascota=>mascota.IdUsuario == usuario.Id).ToListAsync();

                
              
                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mapper.Map<List<MascotaDTO>>(mascotas);


            }
            catch (Exception ex)
            {
                responseApi.Resultado = 0;
                responseApi.Mensaje = ex.Message;
                responseApi.Data = null;


            }

            return responseApi;

        }




        [HttpGet("{id:int}")]
        public async Task<ActionResult<ResponseApi>> MostrarUnaMascota([FromRoute]int id)
        {
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;


                var usuario = await userManager.FindByEmailAsync(email);

                var mascota = await context.Mascotas.Include(mascota=>mascota.Cliente)
                    .Include(mascota=>mascota.Alergia).Include(mascota=>mascota.Alergia)
                    .Include(mascota => mascota.Enfermedad)
                    .Include(mascota=>mascota.Raza)
                    .Where(mascotas => mascotas.Id == id).FirstOrDefaultAsync();

                if(mascota == null)
                {
                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe una mascota con el Id especificado";
                    responseApi.Data = null;

                    return responseApi;
                }
                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mapper.Map<MascotaDTO>(mascota);

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



        #region INSERTAR MASCOTA

        [HttpPost]
        public async Task<ActionResult<ResponseApi>> Post([FromBody]MascotaCreacionDTO nuevaMascotaDTO)
        {

            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var cliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(cliente => cliente.Id == nuevaMascotaDTO.IdCliente);

                if (cliente == null) return BadRequest("No existe un Cliente con el Id especificado");

                var raza = await context.Razas.Where(razas => razas.IdUsuario == usuario.Id).FirstOrDefaultAsync(razas => razas.Id == nuevaMascotaDTO.IdRaza);

                if (raza==null) return BadRequest("No existe una Raza con el Id especificado");

                var enfermedad = await context.Enfermedades.Where(enfermedad => enfermedad.IdUsuario == usuario.Id).FirstOrDefaultAsync(enfermedad => enfermedad.Id == nuevaMascotaDTO.IdEnfermedad);

                if (enfermedad == null) return BadRequest("No existe una Enfermedad con el Id especificado");


                var alergia = await context.Alergias.Where(alergias => alergias.IdUsuario == usuario.Id).FirstOrDefaultAsync(alergias => alergias.Id == nuevaMascotaDTO.IdAlergia);

                if (alergia==null) return BadRequest("No existe una Alergia con el Id especificado");

                var nuevaMascota = mapper.Map<Mascota>(nuevaMascotaDTO);
                nuevaMascota.Cliente = cliente;
                nuevaMascota.Raza= raza;
                
                nuevaMascota.Enfermedad = enfermedad;
                
                nuevaMascota.Alergia = alergia;


                nuevaMascota.IdUsuario = usuario.Id;

                context.Mascotas.Add(nuevaMascota);
                await context.SaveChangesAsync();

                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mapper.Map<MascotaDTO>(nuevaMascota);


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


        #region MODIFICAR MASCOTA

        [HttpPut]
        public async Task<ActionResult<ResponseApi>> ModificarMascota([FromBody]MascotaDTO mascotaDTO)
        {
            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var existeMascota = await context.Mascotas.Where(mascota=>mascota.IdUsuario == usuario.Id).AnyAsync(mascota=>mascota.Id == mascotaDTO.Id);
                
                if(!existeMascota)
                {
                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe una mascota con el Id especificado";
                    responseApi.Data = null;

                    return responseApi;
                }

                var cliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(cliente => cliente.Id == mascotaDTO.IdCliente);

                if (cliente == null) return BadRequest("No existe un Cliente con el Id especificado");

                var raza = await context.Razas.Where(razas => razas.IdUsuario == usuario.Id).FirstOrDefaultAsync(razas => razas.Id == mascotaDTO.IdRaza);

                if (raza == null) return BadRequest("No existe una Raza con el Id especificado");

                var enfermedad = await context.Enfermedades.Where(enfermedad => enfermedad.IdUsuario == usuario.Id).FirstOrDefaultAsync(enfermedad => enfermedad.Id == mascotaDTO.IdEnfermedad);

                if (enfermedad == null) return BadRequest("No existe una Enfermedad con el Id especificado");


                var alergia = await context.Alergias.Where(alergias => alergias.IdUsuario == usuario.Id).FirstOrDefaultAsync(alergias => alergias.Id == mascotaDTO.IdAlergia);

                if (alergia == null) return BadRequest("No existe una Alergia con el Id especificado");

                var mascota = mapper.Map<Mascota>(mascotaDTO);
                
                mascota.Cliente = cliente;
                mascota.Raza = raza;
                    
                mascota.Enfermedad = enfermedad;

                mascota.Alergia = alergia;

                mascota.IdUsuario = usuario.Id;

                context.Update(mascota);
                await context.SaveChangesAsync();

                responseApi.Resultado = 1;
                responseApi.Mensaje = null;
                responseApi.Data = mascota;

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


        #region ELIMINAR MASCOTA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ResponseApi>> EliminarMascota([FromRoute]int id)
        {


            try
            {
                var claim = HttpContext.User.Claims.Where(claim=>claim.Type =="email").FirstOrDefault();
                var claimValue = claim.Value;
                var usuario = await userManager.FindByEmailAsync(claimValue);

                var mascota = await context.Mascotas.Where(mascota=>mascota.IdUsuario == usuario.Id).FirstOrDefaultAsync(mascota=>mascota.Id==id);

                if(mascota == null)
                {
                    responseApi.Resultado = 0;
                    responseApi.Mensaje = "No existe una mascota con el Id especificado";
                    responseApi.Data = null;

                    return responseApi;
                }

                context.Remove(mascota);
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
