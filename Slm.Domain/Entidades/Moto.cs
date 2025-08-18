using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slm.Domain.Entidades
{
    [Table("motos")]
    public class Moto
    {
        [Key]
        [Column("identificador")]
        [Required]
        [StringLength(50)]
        public string Identificador { get; set; }

        [Column("ano")]
        [Required]
        public int Ano { get; set; }

        [Column("modelo")]
        [Required]
        [StringLength(100)]
        public string Modelo { get; set; }

        [Column("placa")]
        [Required]
        [StringLength(10)]
        public string Placa { get; set; }
    }
}
