using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAdministracionPeluqueria.Models.Entidades
{
    public class Ingreso
    {
        [Required]
        public int Id { get; set; }
        [Required]

        [Column(TypeName = "date")]
        public DateTime Fecha { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Precio { get; set; }
        [Required]
        public string IdUsuario { get; set; }
        
        public Usuario Usuario { get; set;}
        
        [Required]
        public int IdTurno { get; set; }
        
        
    }
}
