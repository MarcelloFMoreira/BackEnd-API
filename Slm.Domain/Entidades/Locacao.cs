using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slm.Domain.Entidades
{
    [Table("locacao")]
    public class Locacao
    {
        [Key]//chave primaria
        [Column("identificador")]//nome da coluna no banco
        public string Identificador { get; set; }

        [Column("valor_diaria")]//nome da coluna no banco
        public decimal ValorDiaria { get; set; }

        [Column("valor_total")]//nome da coluna no banco
        public decimal? ValorTotal { get; set; } 

        [Column("entregador_id")]//nome da coluna no banco
        public string EntregadorId { get; set; }

        [Column("moto_id")]//nome da coluna no banco
        public string MotoId { get; set; }

        [Required]//obrigaorio
        [Column("data_inicio")]//nome da coluna no banco
        public DateTime DataInicio { get; set; }

        [Required]//obrigaorio
        [Column("data_termino")]//nome da coluna no banco
        public DateTime? DataTermino { get; set; }

        [Required]//obrigaorio
        [Column("data_previsao_termino")]//nome da coluna no banco
        public DateTime DataPrevisaoTermino { get; set; }

        [Column("plano")]//nome da coluna no banco
        public int Plano { get; set; }

        
        [Column("data_devolucao")]//nome da coluna no banco
        public DateTime?  DataDevolucao { get; set; }
    }
}