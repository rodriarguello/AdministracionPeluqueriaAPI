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


        public MascotasController(ApplicationDbContext context, IMapper mapper, UserManager<Usuario> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }



        #region MOSTRAR MASCOTAS

        [HttpGet("{idCliente:int}")]
        public async Task<ActionResult<List<MascotaDTO>>> GetMascotasCliente([FromRoute]int idCliente)
        {



            var claimEmail = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;


            var usuario = await userManager.FindByEmailAsync(email);

            var existeCliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).AnyAsync(cliente=> cliente.Id == idCliente);

            if (!existeCliente) return NotFound("No existe un cliente con el Id especificado");

            

            var mascotas = await context.Mascotas.Where(mascotas => mascotas.IdCliente == idCliente).ToListAsync();


            return mapper.Map<List<MascotaDTO>>(mascotas);

        }

        #endregion



        #region INSERTAR MASCOTA

        [HttpPost]
        public async Task<ActionResult<MascotaDTO>> Post([FromBody]MascotaCreacionDTO nuevaMascotaDTO)
        {
            var claimEmail = HttpContext.User.Claims.Where(claims => claims.Type == "email").FirstOrDefault();

            var email = claimEmail.Value;

            var usuario = await userManager.FindByEmailAsync(email);

            var existeCliente = await context.Clientes.Where(cliente => cliente.IdUsuario == usuario.Id).AnyAsync(cliente => cliente.Id == nuevaMascotaDTO.IdCliente);

            if (!existeCliente) return BadRequest("No existe un Cliente con el Id especificado");

            var existeRaza = await context.Razas.Where(razas => razas.IdUsuario == usuario.Id).AnyAsync(razas => razas.Id == nuevaMascotaDTO.IdRaza);

            if (!existeRaza) return BadRequest("No existe una Raza con el Id especificado");

            var existeEnfermedad = await context.Enfermedades.Where(enfermedad => enfermedad.IdUsuario == usuario.Id).AnyAsync(enfermedad => enfermedad.Id == nuevaMascotaDTO.IdEnfermedad);

            if (!existeEnfermedad) return BadRequest("No existe una Enfermedad con el Id especificado");


            var existeAlergia = await context.Alergias.Where(alergias => alergias.IdUsuario == usuario.Id).AnyAsync(alergias => alergias.Id == nuevaMascotaDTO.IdAlergia);

            if (!existeAlergia) return BadRequest("No existe una Alergia con el Id especificado");


            var nuevaMascota = mapper.Map<Mascota>(nuevaMascotaDTO);
            nuevaMascota.IdUsuario = usuario.Id;

            context.Mascotas.Add(nuevaMascota);
            await context.SaveChangesAsync();

            return mapper.Map<MascotaDTO>(nuevaMascota);

        }

        #endregion






    }
}
