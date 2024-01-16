using ApiAdministracionPeluqueria.Exceptions;
using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.Entidades;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.IngresoDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiAdministracionPeluqueria.Services
{
    public class CajaService:ICajaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public CajaService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }


        public async Task<ResIngresos> GetIngresoAnualAsync(int anio, string idUsuario)
        {
            if (anio > DateTime.Now.Year) throw new BadHttpRequestException("No se puede enviar un año mayor al actual");



            var ingresos = await _context.Ingresos.Where(ingreso => ingreso.IdUsuario == idUsuario)
                                             .Where(ingreso => ingreso.Fecha.Year == anio)
                                             .ToListAsync();

            var totalIngresos = ingresos.Sum(ingreso => ingreso.Precio);

            var respuesta = new ResIngresos
            {
                CantidadIngresos = ingresos.Count,
                Total = totalIngresos
            };

            return respuesta;
        }

        public async Task<ResIngresos> GetIngresoMensualAsync(int mes, int anio, string idUsuario)
        {
            if (anio > DateTime.Now.Year) throw new BadHttpRequestException("No se puede enviar un año mayor al actual");


            var ingresos = await _context.Ingresos.Where(ingreso => ingreso.IdUsuario == idUsuario)
                                             .Where(ingreso => ingreso.Fecha.Year == anio && ingreso.Fecha.Month == mes)
                                             .ToListAsync();

            var totalIngresos = ingresos.Sum(ingreso => ingreso.Precio);

            var respuesta = new ResIngresos
            {
                CantidadIngresos = ingresos.Count,
                Total = totalIngresos
            };

            return respuesta;
        }

        public async Task<ResIngresos> GetIngresoDiarioAsync(int anio, int mes, int dia, string idUsuario)
        {
            if (anio > DateTime.Now.Year) throw new BadHttpRequestException("No se puede enviar un año mayor al actual");

            var fecha = new DateTime(anio, mes, dia);

            var ingresos = await _context.Ingresos.Where(ingreso => ingreso.IdUsuario == idUsuario).Where(ingreso => ingreso.Fecha.Date == fecha.Date).ToListAsync();

            var totalIngresos = ingresos.Sum(ingreso => ingreso.Precio);

            var respuesta = new ResIngresos
            {
                CantidadIngresos = ingresos.Count,
                Total = totalIngresos
            };

            return respuesta;
        }

        public async Task CrearIngresoAsync(DateTime fecha, decimal precio, Usuario usuario, int idTurno)
        {

            var nuevoIngreso = new Ingreso
            {
                Fecha = fecha,
                Precio = precio,
                IdUsuario = usuario.Id,
                Usuario = usuario,
                IdTurno = idTurno

            };

            _context.Ingresos.Add(nuevoIngreso);

            await _context.SaveChangesAsync();
        }

        public async Task EliminarIngresoAsync(int idTurno, string idUsuario)
        {
            var registroCaja = await _context.Ingresos.Where(caja => caja.IdUsuario == idUsuario).FirstOrDefaultAsync(caja => caja.IdTurno == idTurno);

            if (registroCaja != null)
            {
                _context.Ingresos.Remove(registroCaja);
            }

            await _context.SaveChangesAsync();
        }

        public async Task ModificarPrecioIngresoAsync(int idTurno, string idUsuario, decimal nuevoPrecio)
        {
            var registroCaja = await _context.Ingresos.Where(registro => registro.IdUsuario == idUsuario).FirstOrDefaultAsync(registro => registro.IdTurno == idTurno);

            if (registroCaja == null) throw new BadRequestException("No existe un registro con el idTurno especificado");
            
            registroCaja.Precio = nuevoPrecio;

            await _context.SaveChangesAsync();

        }
    }
}
