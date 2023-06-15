using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<ModeloRespuesta>> MostrarMascotas()
        {

            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim=>claim.Type == "email").FirstOrDefault();
                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var mascotas = new List<Mascota>();
                
                mascotas = await context.Mascotas.Include(mascotas=>mascotas.Cliente)
                                                 .Include(mascotas=>mascotas.Raza)
                                                 .Include(mascotas=>mascotas.Enfermedades)
                                                         .ThenInclude(enfermedades => enfermedades.Enfermedad)
                                                 .Include(mascotas=>mascotas.Alergia)
                                                 .Where(mascota=>mascota.IdUsuario == usuario.Id).ToListAsync();

                 
                




                return responseApi.respuestaExitosa(mapper.Map<List<MascotaSinClienteDTO>>(mascotas));


            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);


            }

        }




        [HttpGet("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> MostrarUnaMascota([FromRoute]int id)
        {
            try
            {

                var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;


                var usuario = await userManager.FindByEmailAsync(email);

                var mascota = await context.Mascotas.Include(mascota=>mascota.Cliente)
                    .Include(mascota=>mascota.Alergia)
                    .Include(mascota=>mascota.Alergia)
                    .Include(mascota => mascota.Enfermedades)
                            .ThenInclude(mascotaEnfermedad=>mascotaEnfermedad.Enfermedad)
                    .Include(mascota=>mascota.Raza)
                    .Where(mascotas => mascotas.Id == id).FirstOrDefaultAsync();

                if(mascota == null) return responseApi.respuestaError("No existe una mascota con el Id especificado");
                
                return responseApi.respuestaExitosa(mapper.Map<MascotaSinClienteDTO>(mascota));

            }
            catch (Exception ex)
            {

                return  responseApi.respuestaError(ex.Message);
            }

            
        }

        #endregion



        #region INSERTAR MASCOTA

        [HttpPost]
        public async Task<ActionResult<ModeloRespuesta>> Post([FromBody]MascotaCreacionDTO nuevaMascotaDTO)
        {

            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                
                var cliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(cliente => cliente.Id == nuevaMascotaDTO.IdCliente);

                if (cliente == null) return responseApi.respuestaError("No existe un Cliente con el Id especificado");


                var raza = await context.Razas.Where(razas => razas.IdUsuario == usuario.Id).FirstOrDefaultAsync(razas => razas.Id == nuevaMascotaDTO.IdRaza);

                if (raza==null) return responseApi.respuestaError("No existe una Raza con el Id especificado");


                if (nuevaMascotaDTO.IdEnfermedad.Count < 1) return  responseApi.respuestaError("El campo enfermedad es obligatorio");
                

                var listEnfermedades = await context.Enfermedades
                                             .Where(enfermedad => nuevaMascotaDTO.IdEnfermedad.Contains(enfermedad.Id)).ToListAsync();

                if (listEnfermedades.Count != nuevaMascotaDTO.IdEnfermedad.Count) return responseApi.respuestaError("Por lo menos alguna de las enfermedades enviadas no existe");


                var alergia = await context.Alergias.Where(alergias => alergias.IdUsuario == usuario.Id).FirstOrDefaultAsync(alergias => alergias.Id == nuevaMascotaDTO.IdAlergia);

                if (alergia==null) return responseApi.respuestaError("No existe una Alergia con el Id especificado");

                var nuevaMascota = mapper.Map<Mascota>(nuevaMascotaDTO);
                nuevaMascota.Cliente = cliente;
                nuevaMascota.Raza= raza;
                
                nuevaMascota.Alergia = alergia;


                nuevaMascota.IdUsuario = usuario.Id;

                context.Mascotas.Add(nuevaMascota);

                

                await context.SaveChangesAsync();


                foreach (var enfermedad in listEnfermedades)
                {
                    
                    context.MascotaEnfermedades.Add(new MascotaEnfermedad { 
                        
                        MascotaId = nuevaMascota.Id,
                        
                        EnfermedadId = enfermedad.Id,
                        
                        IdUsuario = usuario.Id
                    });

                }


                await context.SaveChangesAsync();

                
                
                
                return responseApi.respuestaExitosa(mapper.Map<MascotaNombreFechaNacimientoDTO>(nuevaMascota));

            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
            }


        }

        #endregion


        #region MODIFICAR MASCOTA

        [HttpPut]
        public async Task<ActionResult<ModeloRespuesta>> ModificarMascota([FromBody]MascotaModificarDTO mascotaDTO)
        {
            try
            {   
                var claimEmail = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var existeMascota = await context.Mascotas.Where(mascota=>mascota.IdUsuario == usuario.Id).AnyAsync(mascota=>mascota.Id == mascotaDTO.Id);
                
                if(!existeMascota) return responseApi.respuestaError("No existe una mascota con el Id especificado");
              

                var cliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(cliente => cliente.Id == mascotaDTO.IdCliente);

                if (cliente == null) return responseApi.respuestaError("No existe un Cliente con el Id especificado");

                var raza = await context.Razas.Where(razas => razas.IdUsuario == usuario.Id).FirstOrDefaultAsync(razas => razas.Id == mascotaDTO.IdRaza);

                if (raza == null) return responseApi.respuestaError("No existe una Raza con el Id especificado");


                if (mascotaDTO.IdEnfermedades.Count < 1) return responseApi.respuestaError("El campo enfermedad es obligatorio");

                var nuevasEnfermedades = await context.Enfermedades.Where(enfermedad=> mascotaDTO.IdEnfermedades.Contains(enfermedad.Id)).ToListAsync();

                if (nuevasEnfermedades.Count != mascotaDTO.IdEnfermedades.Count) return responseApi.respuestaError("Alguna de las enfermedades enviadas no existe");


                var enfermedadesEliminar = await context.MascotaEnfermedades
                                                .Where(me=> me.MascotaId == mascotaDTO.Id && me.IdUsuario == usuario.Id)
                                                .Where(enfermedadMascota =>!mascotaDTO.IdEnfermedades.Contains(enfermedadMascota.EnfermedadId)).ToListAsync();


                //Enfermedades existentes que tienen el mismo id que las enfermedades que se envían con la mascota

                var enfermedadesExistentes = await context.MascotaEnfermedades
                                                .Where(me => me.MascotaId == mascotaDTO.Id && me.IdUsuario == usuario.Id)
                                                .Where(enfermedadMascota => mascotaDTO.IdEnfermedades.Contains(enfermedadMascota.EnfermedadId)).ToListAsync();

                if (enfermedadesEliminar.Count > 0)
                {

                    foreach (var enfermedadEliminar in enfermedadesEliminar)
                    {
                        
                            context.MascotaEnfermedades.Remove(enfermedadEliminar);

                    }

                }


                foreach (var enfermedad in nuevasEnfermedades)
                {

                    if(enfermedadesExistentes.Count> 0)
                    {

                        foreach (var enfermedadExistente in enfermedadesExistentes)
                        {
                            if (enfermedad.Id == enfermedadExistente.EnfermedadId)
                            {
                                continue;
                            }
                            context.Add(new MascotaEnfermedad
                            {
                                EnfermedadId = enfermedad.Id,
                                MascotaId = mascotaDTO.Id,
                                IdUsuario = usuario.Id
                            });
                            

                        }
                        
                        continue;
                    }
                        
                    context.Add(new MascotaEnfermedad
                        {
                            EnfermedadId = enfermedad.Id,
                            MascotaId = mascotaDTO.Id,
                            IdUsuario = usuario.Id
                        });
                    
                }
                

                var alergia = await context.Alergias.Where(alergias => alergias.IdUsuario == usuario.Id)
                                                    .FirstOrDefaultAsync(alergias => alergias.Id == mascotaDTO.IdAlergia);

                if (alergia == null) return responseApi.respuestaError("No existe una Alergia con el Id especificado");

                var mascota = mapper.Map<Mascota>(mascotaDTO);
                
                mascota.Cliente = cliente;
                mascota.Raza = raza;
               
                mascota.Alergia = alergia;

                mascota.IdUsuario = usuario.Id;

                context.Update(mascota);

                await context.SaveChangesAsync();


                return responseApi.respuestaExitosa();

            }
            catch (Exception ex)
            {
              return responseApi.respuestaError(ex.ToString());
            }

        }

        #endregion


        #region ELIMINAR MASCOTA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> EliminarMascota([FromRoute]int id)
        {


            try
            {
                var claim = HttpContext.User.Claims.Where(claim=>claim.Type =="email").FirstOrDefault();
                var claimValue = claim.Value;
                var usuario = await userManager.FindByEmailAsync(claimValue);

                var mascota = await context.Mascotas.Where(mascota=>mascota.IdUsuario == usuario.Id)
                                                    .Include(mascota=>mascota.Enfermedades)
                                                    .FirstOrDefaultAsync(mascota=>mascota.Id==id);

                if(mascota == null) return responseApi.respuestaError("No existe una mascota con el Id especificado");

                foreach (var enfermedad in mascota.Enfermedades)
                {
                    context.MascotaEnfermedades.Remove(enfermedad);
                }
                
                context.Remove(mascota);
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
