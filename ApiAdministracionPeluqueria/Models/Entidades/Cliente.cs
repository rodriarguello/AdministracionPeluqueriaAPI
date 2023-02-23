﻿using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Cliente
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string? Telefono { get; set; }

        public string? Mail { get; set; }

        
        public int? IdMascota { get; set; }

        public List<Mascota> Mascota { get; set; }

    }
}
