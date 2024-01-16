using ApiAdministracionPeluqueria.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Enfermedad:IIdUsuario
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string IdUsuario { get; set; }


        public List<MascotaEnfermedad> MascotasEnfermedad { get; set; }



        


    }
}
