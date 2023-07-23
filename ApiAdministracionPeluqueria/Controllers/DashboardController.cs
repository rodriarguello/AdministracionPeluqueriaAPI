using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace ApiAdministracionPeluqueria.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Usuario> userManager;
        private readonly ResponseApi responseApi;
        private readonly IMapper mapper;

        public DashboardController(ApplicationDbContext context, UserManager<Usuario> userManager, ResponseApi responseApi, IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.responseApi = responseApi;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ModeloRespuesta>> ResumenDiario()
        {

            try
            {
                var claim = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

                var email = claim.Value;

                var usuario = await userManager.FindByEmailAsync(email);

                var fechaActual = DateTime.Now;
                var calendario = await context.Calendarios.Where(calendario => calendario.IdUsuario == usuario.Id).
                    FirstOrDefaultAsync();

                var turnos = await context.Turnos.Where(turnos => turnos.IdCalendario == calendario.Id)
                    .Include(turnos => turnos.Fecha)
                    .Include(turnos => turnos.Horario)
                    .Include(turnos => turnos.Mascota)
                    .Where(turnos => turnos.Fecha.Dia.Date == fechaActual.Date)
                .ToListAsync();



                var ingresosMensual = await context.Caja.Where(ingreso => ingreso.IdUsuario == usuario.Id).Where(ingreso => ingreso.Fecha.Month == fechaActual.Month).ToListAsync();

                var totalIngresosMensual = ingresosMensual.Sum(ingreso => ingreso.Precio);


                var ingresoDiario = ingresosMensual.Where(ingreso => ingreso.Fecha.Date == fechaActual.Date).ToList();

                var totalIngresoDiario = ingresoDiario.Sum(ingreso => ingreso.Precio);






                var clientes = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).ToListAsync();

                var mascotas = await context.Mascotas.Where(mascota=>mascota.IdUsuario == usuario.Id).ToListAsync();



                var dataRespuesta = new
                {
                    turnos = mapper.Map<List<TurnoDTO>>(turnos),
                    ingresoDiario = new {cantidadIngresos = ingresoDiario.Count, total = totalIngresoDiario },
                    ingresoMensual = new { cantidadIngresos = ingresosMensual.Count, total = totalIngresosMensual },
                    cantidadClientes = clientes.Count,
                    cantidadMascotas = mascotas.Count
                };


                return responseApi.respuestaExitosa(dataRespuesta);
            }
            catch (Exception ex)
            {
                return responseApi.respuestaError(ex.Message);
                
            }
            



            
        
      




        }
    }
}
