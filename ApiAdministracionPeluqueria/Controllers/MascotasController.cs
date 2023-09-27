using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.MascotaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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

                

                var mascotas = await context.Mascotas.Include(mascotas=>mascotas.Cliente)
                                                 .Include(mascotas=>mascotas.Raza)
                                                 .Include(mascotas=>mascotas.MascotaEnfermedades)
                                                         .ThenInclude(mascotaEnfermedad => mascotaEnfermedad.Enfermedad)
                                                 .Include(mascotas=>mascotas.MascotaAlergias)
                                                          .ThenInclude(mascotaAlergia=> mascotaAlergia.Alergia)
                                                 .Include(mascota => mascota.Turnos)
                                                 .Where(mascota=>mascota.IdUsuario == usuario.Id).ToListAsync();







                return responseApi.respuestaExitosa(mapper.Map<List<MascotaDTO>>(mascotas));


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

                var mascota = await context.Mascotas.Include(mascota => mascota.Cliente)
                    .Include(mascota => mascota.MascotaAlergias)
                            .ThenInclude(mascotaAlergia => mascotaAlergia.Alergia)
                    
                    .Include(mascota => mascota.MascotaEnfermedades)
                            .ThenInclude(mascotaEnfermedad=>mascotaEnfermedad.Enfermedad)
                    
                    .Include(mascota=>mascota.Raza)

                    
                    
                    .Where(mascotas => mascotas.Id == id).FirstOrDefaultAsync();

                if(mascota == null) return responseApi.respuestaError("No existe una mascota con el Id especificado");

                return responseApi.respuestaExitosa(mapper.Map<MascotaDTO>(mascota));

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
            var transaccion = await context.Database.BeginTransactionAsync();

            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);


                var cliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(cliente => cliente.Id == nuevaMascotaDTO.IdCliente);

                if (cliente == null) return responseApi.respuestaError("No existe un Cliente con el Id especificado");


                var raza = await context.Razas.Where(razas => razas.IdUsuario == usuario.Id).FirstOrDefaultAsync(razas => razas.Id == nuevaMascotaDTO.IdRaza);

                if (raza == null) return responseApi.respuestaError("No existe una Raza con el Id especificado");

                //ENFERMEDADES

                if (nuevaMascotaDTO.IdEnfermedades.Count < 1) return  responseApi.respuestaError("El campo enfermedad es obligatorio");


                var listEnfermedades = await context.Enfermedades
                                             .Where(enfermedad => nuevaMascotaDTO.IdEnfermedades.Contains(enfermedad.Id)).ToListAsync();

                if (listEnfermedades.Count != nuevaMascotaDTO.IdEnfermedades.Count) return responseApi.respuestaError("Por lo menos una de las enfermedades enviadas no existe");

                //ALERGIAS

                if (nuevaMascotaDTO.IdAlergias.Count < 1) return responseApi.respuestaError("El campo alergia es obligatorio");


                var listAlergias = await context.Alergias
                                             .Where(alergia => nuevaMascotaDTO.IdAlergias.Contains(alergia.Id)).ToListAsync();

                if (listAlergias.Count != nuevaMascotaDTO.IdAlergias.Count) return responseApi.respuestaError("Por lo menos una de las alergias enviadas no existe");


                var nuevaMascota = mapper.Map<Mascota>(nuevaMascotaDTO);
                
                nuevaMascota.Cliente = cliente;
                
                nuevaMascota.Raza= raza;

                nuevaMascota.IdUsuario = usuario.Id;

                nuevaMascota.FechaCreacion = DateTime.Now;

                context.Mascotas.Add(nuevaMascota);



                await context.SaveChangesAsync();


                foreach (var enfermedad in listEnfermedades)
                {

                    context.MascotasEnfermedades.Add(new MascotaEnfermedad { 

                        IdMascota = nuevaMascota.Id,

                        IdEnfermedad = enfermedad.Id,

                        Enfermedad = enfermedad,

                        Mascota = nuevaMascota,
                        

                        IdUsuario = usuario.Id
                    });

                }


                foreach (var alergia in listAlergias)
                {

                    context.MascotasAlergias.Add(new MascotaAlergia
                    {

                        IdMascota = nuevaMascota.Id,

                        IdAlergia = alergia.Id,

                        Alergia = alergia,

                        Mascota = nuevaMascota,

                        IdUsuario = usuario.Id
                    });

                }


                await context.SaveChangesAsync();

                await transaccion.CommitAsync();




                return responseApi.respuestaExitosa(mapper.Map<MascotaNombreFechaNacimientoDTO>(nuevaMascota));

            }
            catch (Exception ex)
            {   
                await transaccion.RollbackAsync();
                return responseApi.respuestaError(ex.Message);
            }


        }

        #endregion


        #region MODIFICAR MASCOTA

        [HttpPut]
        public async Task<ActionResult<ModeloRespuesta>> ModificarMascota([FromBody]MascotaModificarDTO mascotaDTO)
        {
            var transaccion = await context.Database.BeginTransactionAsync();

            try
            {
                var claimEmail = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();

                var email = claimEmail.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var mascota = await context.Mascotas.Where(mascota=>mascota.IdUsuario == usuario.Id)
                                

                            .FirstOrDefaultAsync(mascota=>mascota.Id == mascotaDTO.Id);

                if (mascota==null) return responseApi.respuestaError("No existe una mascota con el Id especificado");


                var cliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).FirstOrDefaultAsync(cliente => cliente.Id == mascotaDTO.IdCliente);

                if (cliente == null) return responseApi.respuestaError("No existe un Cliente con el Id especificado");



                var raza = await context.Razas.Where(razas => razas.IdUsuario == usuario.Id).FirstOrDefaultAsync(razas => razas.Id == mascotaDTO.IdRaza);

                if (raza == null) return responseApi.respuestaError("No existe una Raza con el Id especificado");

                //VALIDACION ENFERMEDADES

                if (mascotaDTO.IdEnfermedades.Count < 1) return responseApi.respuestaError("El campo enfermedad es obligatorio");

                var nuevasEnfermedades = await context.Enfermedades.Where(enfermedad=> mascotaDTO.IdEnfermedades.Contains(enfermedad.Id)).ToListAsync();

                if (nuevasEnfermedades.Count != mascotaDTO.IdEnfermedades.Count) return responseApi.respuestaError("Alguna de las enfermedades enviadas no existe");


                //VALIDACION ALERGIAS

                
                if (mascotaDTO.IdAlergias.Count < 1) return responseApi.respuestaError("El campo alergia es obligatorio");

                var nuevasAlergias = await context.Alergias.Where(alergia => mascotaDTO.IdAlergias.Contains(alergia.Id)).ToListAsync();

                if (nuevasAlergias.Count != mascotaDTO.IdAlergias.Count) return responseApi.respuestaError("Alguna de las alergias enviadas no existe");




                mascota.IdCliente = mascotaDTO.IdCliente;
                mascota.Cliente = cliente;
                mascota.IdRaza = mascotaDTO.IdRaza;
                mascota.Raza = raza;
                mascota.Nombre = mascotaDTO.Nombre;
                mascota.FechaNacimiento = mascotaDTO.FechaNacimiento;

                                
                //ENFERMEDADES

                var enfermedadesEliminar = await context.MascotasEnfermedades
                                                .Where(me => me.IdMascota == mascotaDTO.Id && me.IdUsuario == usuario.Id)
                                                .Where(enfermedadMascota => !mascotaDTO.IdEnfermedades.Contains(enfermedadMascota.IdEnfermedad)).ToListAsync();


                //Enfermedades existentes que tienen el mismo id que las enfermedades que se envían con la mascota

                var enfermedadesExistentes = await context.MascotasEnfermedades
                                                .Where(me => me.IdMascota == mascotaDTO.Id && me.IdUsuario == usuario.Id)
                                                .Where(enfermedadMascota => mascotaDTO.IdEnfermedades.Contains(enfermedadMascota.IdEnfermedad)).ToListAsync();

                if (enfermedadesEliminar.Count > 0)
                {

                    foreach (var enfermedadEliminar in enfermedadesEliminar)
                    {

                        context.MascotasEnfermedades.Remove(enfermedadEliminar);

                    }

                }


               

                var IdEnfermedadesExistentes = new List<int>();

                //Se extrae el Id de la enfermedad y se lo guarda en una lista

                foreach (var enfermedadExistente in enfermedadesExistentes)
                {
                    IdEnfermedadesExistentes.Add(enfermedadExistente.IdEnfermedad);
                }

                //Se compara los valores de la lista con los id de las enfermedades existentes, y cuando no hay igualdad, 
                //se guarda la enfermedad para cargarla en la base de datos
                
                var cargarEnfermedades = nuevasEnfermedades.Where(nuevaEnfermedad => !IdEnfermedadesExistentes.Contains(nuevaEnfermedad.Id)).ToList();


                foreach (var enfermedad in cargarEnfermedades)
                {
                    context.MascotasEnfermedades.Add(new MascotaEnfermedad{
                        IdEnfermedad = enfermedad.Id,
                        Enfermedad = enfermedad,
                        Mascota = mascota,
                        IdMascota = mascota.Id,
                        IdUsuario = usuario.Id
                    });
                }

                //ALERGIAS


                var alergiasEliminar = await context.MascotasAlergias
                                                .Where(ma => ma.IdMascota == mascotaDTO.Id && ma.IdUsuario == usuario.Id)
                                                .Where(mascotaAlergia => !mascotaDTO.IdAlergias.Contains(mascotaAlergia.IdAlergia)).ToListAsync();


                //Alergias existentes que tienen el mismo id que las alergias que se envían con la mascota

                var alergiasExistentes = await context.MascotasAlergias
                                                .Where(ma => ma.IdMascota == mascotaDTO.Id && ma.IdUsuario == usuario.Id)
                                                .Where(mascotaAlergia => mascotaDTO.IdAlergias.Contains(mascotaAlergia.IdAlergia)).ToListAsync();

                if (alergiasEliminar.Count > 0)
                {

                    foreach (var alergiaEliminar in alergiasEliminar)
                    {

                        context.MascotasAlergias.Remove(alergiaEliminar);

                    }

                }


              var idAlergiasExistentes = new List<int>();


                foreach(var alergia in alergiasExistentes)
                {

                    idAlergiasExistentes.Add(alergia.IdAlergia);

                }

                var agregarAlergias = nuevasAlergias.Where(nuevaAlergia => !idAlergiasExistentes.Contains(nuevaAlergia.Id)).ToList();


                foreach (var alergia in agregarAlergias)
                {
                    context.MascotasAlergias.Add(new MascotaAlergia
                    {
                        IdAlergia = alergia.Id,
                        Alergia = alergia,
                        IdMascota = mascota.Id,
                        Mascota = mascota,
                        IdUsuario = usuario.Id
                    });
                }

                await context.SaveChangesAsync();

                await transaccion.CommitAsync();


                return responseApi.respuestaExitosa();

            }
            catch (Exception ex)
            {   
                await transaccion.RollbackAsync();
                return responseApi.respuestaError(ex.Message);
            }

        }

        #endregion


        #region ELIMINAR MASCOTA
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ModeloRespuesta>> EliminarMascota([FromRoute]int id)
        {
            var transaccion = await context.Database.BeginTransactionAsync();

            try
            {
                var claim = HttpContext.User.Claims.Where(claim=>claim.Type =="email").FirstOrDefault();
                var claimValue = claim.Value;
                var usuario = await userManager.FindByEmailAsync(claimValue);

                var mascota = await context.Mascotas.Where(mascota=>mascota.IdUsuario == usuario.Id)
                                                    
                                                    .Include(mascota=>mascota.MascotaEnfermedades)
                                                    
                                                    .Include(mascota=>mascota.MascotaAlergias)

                                                    .Include(mascota=>mascota.Turnos)
                                                    
                                                    .FirstOrDefaultAsync(mascota=>mascota.Id==id);

                if(mascota == null) return responseApi.respuestaError("No existe una mascota con el Id especificado");

                if (mascota.Turnos.Count > 0) return responseApi.respuestaErrorEliminacion("Error al eliminar la mascota porque tiene turnos asociados");

                foreach (var mascotaEnfermedad in mascota.MascotaEnfermedades)
                {
                    context.MascotasEnfermedades.Remove(mascotaEnfermedad);
                }

                foreach (var mascotaAlergia in mascota.MascotaAlergias)
                {
                    context.MascotasAlergias.Remove(mascotaAlergia);
                }

                context.Remove(mascota);
                await context.SaveChangesAsync();

                await transaccion.CommitAsync();

                return responseApi.respuestaExitosa();
            }
            catch (Exception ex)
            {
                await transaccion.RollbackAsync();
                return responseApi.respuestaError(ex.Message);
            }

        }




        #endregion
    }
}
