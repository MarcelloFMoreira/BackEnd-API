using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slm.Domain.Entidades
{
    [Table("entregadores")]
    public class Entregador
    {
        [Key] //chave primaria
        [Column("identificador")]//nome da coluna no banco
        [Required]//obrigatorio
        [StringLength(50)]//tamanho maximo
        public string identificador { get; set; }

        [Column("nome")]//nome da coluna no banco
        [Required]//obrigatorio
        [StringLength(200)]//tamanho maximo
        public string nome { get; set; }

        [Column("cnpj")]//nome da coluna no banco
        [Required]//obrigatorio
        [StringLength(14)]//tamanho maximo
        public string cnpj { get; set; }

        [Column("data_nascimento")]//nome da coluna no banco
        [Required]//obrigatorio
        public DateTime data_nascimento { get; set; }

        [Column("numero_cnh")]//nome da coluna no banco
        [Required]//obrigatorio
        [StringLength(11)]//tamanho maximo
        public string numero_cnh { get; set; }

        [Column("tipo_cnh")]//nome da coluna no banco
        [Required]//obrigatorio
        [StringLength(3)]//tamanho maximo
        public string tipo_cnh { get; set; } 

        [Column("imagem_cnh")]//nome da coluna no banco
        public string imagem_cnh { get; set; }
    }
}