using ApiAdministracionPeluqueria.Models;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.IngresoDTO;
using ApiAdministracionPeluqueria.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApiAdministracionPeluqueria.Services
{
    public class CajaService:ICajaService
    {
        private readonly ApplicationDbContext _context;

        public CajaService(ApplicationDbContext context)
        {
            _context = context;
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
    }
}
