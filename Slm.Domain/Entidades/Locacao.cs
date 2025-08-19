using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Slm.Domain.Entidades
{
    [Table("locacao")]
    public class Locacao
    {
        [Key]
        [Column("identificador")]
        public string Identificador { get; set; }

        [Column("valor_diaria")]
        public decimal ValorDiaria { get; set; }

        [Column("valor_total")]
        public decimal? ValorTotal { get; set; } 

        [Column("entregador_id")]
        public string EntregadorId { get; set; }

        [Column("moto_id")]
        public string MotoId { get; set; }

        [Column("data_inicio")]
        public DateTime DataInicio { get; set; }

        [Column("data_termino")]
        public DateTime? DataTermino { get; set; } 

        [Column("data_previsao_termino")]
        public DateTime DataPrevisaoTermino { get; set; }

        [Column("plano")]
        public int Plano { get; set; }

        [Column("data_devolucao")]
        public DateTime?  DataDevolucao { get; set; }
    }
}