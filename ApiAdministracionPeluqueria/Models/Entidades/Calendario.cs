﻿using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Calendario
    {

        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public TimeSpan HoraInicioTurnos { get; set; }
        [Required]
        public TimeSpan HoraFinTurnos { get; set; }
        [Required]
        public TimeSpan IntervaloTurnos { get; set; }

        [Required]
        public string IdUsuario  { get; set; }

        public Usuario Usuario { get; set;}
    }
}
