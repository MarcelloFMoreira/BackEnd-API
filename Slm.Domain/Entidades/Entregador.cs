using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slm.Domain.Entidades
{
    [Table("entregadores")]
    public class Entregador
    {
        [Key]
        [Column("identificador")]
        [Required]
        [StringLength(50)]
        public string identificador { get; set; }

        [Column("nome")]
        [Required]
        [StringLength(200)]
        public string nome { get; set; }

        [Column("cnpj")]
        [Required]
        [StringLength(14)]
        public string cnpj { get; set; }

        [Column("data_nascimento")]
        [Required]
        public DateTime data_nascimento { get; set; }

        [Column("numero_cnh")]
        [Required]
        [StringLength(11)]
        public string numero_cnh { get; set; }

        [Column("tipo_cnh")]
        [Required]
        [StringLength(3)]
        public string tipo_cnh { get; set; } // Aceita "A", "B" ou "A+B"

        [Column("imagem_cnh")]
        public string imagem_cnh { get; set; }
    }
}