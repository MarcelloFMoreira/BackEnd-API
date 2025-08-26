using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slm.Domain.Entidades
{
    [Table("motos")]
    public class Moto
    {
        [Key]
        [Column("identificador")]//nome da coluna no banco
        [Required]//obrigaorio
        [StringLength(50)]//tamanho maximo
        public string Identificador { get; set; }

        [Column("ano")]//nome da coluna no banco
        [Required]//obrigaorio
        public int Ano { get; set; }

        [Column("modelo")]//nome da coluna no banco
        [Required]//obrigaorio
        [StringLength(100)]//tamanho maximo
        public string Modelo { get; set; }

        [Column("placa")]//nome da coluna no banco
        [Required]//obrigaorio
        [StringLength(10)]//tamanho maximo
        public string Placa { get; set; }
    }
}
