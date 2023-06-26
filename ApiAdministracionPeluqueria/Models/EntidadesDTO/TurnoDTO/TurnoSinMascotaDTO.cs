﻿using ApiAdministracionPeluqueria.Models.EntidadesDTO.FechaDTO;
using ApiAdministracionPeluqueria.Models.EntidadesDTO.HorarioDTO;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.EntidadesDTO.TurnoDTO
{
    public class TurnoSinMascotaDTO
    {

        [Required]
        public int Id { get; set; }

        public FechaSinCalendarioDTO Fecha { get; set; }


        public HorarioSinCalendarioDTO Horario { get; set; }

        [Required]
        public bool Disponible { get; set; }

        public bool? Asistio { get; set; }

        public int? Precio { get; set; }

        [Required]
        public int IdCalendario { get; set; }

    }
}
