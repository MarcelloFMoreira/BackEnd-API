using Microsoft.AspNetCore.Mvc;
using slm.infraestrutura;
using Slm.Domain.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace sistema_locacao_motos.Controllers
{
    [Route("/locacao")]
    [ApiController]
    public class locaçãoController : ControllerBase
    {
        private readonly SlmDbContext _db;

        public locaçãoController(SlmDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }


        public class CriarLocacaoRequest
        {
            public string entregador_id { get; set; }
            public string moto_id { get; set; }
            public int plano { get; set; }
            public DateTime data_inicio { get; set; }
            public DateTime data_termino { get; set; } 
            public DateTime data_previsao_termino { get; set; }
        }

        public class DevolucaoRequest
        {
            public DateTime DataDevolucao { get; set; }
        }
        // POST /locacao  criar nova locação
        [HttpPost]
        public IActionResult CriarLocacao([FromBody] CriarLocacaoRequest request)
        {
            if (request == null)
                return BadRequest();

            var entregador = _db.Entregador.Find(request.entregador_id);
            if (entregador == null)
                return NotFound("Entregador não encontrado");

            if (!entregador.tipo_cnh.Contains("A"))
                return BadRequest("Somente entregadores com CNH categoria A podem alugar motos");

            var moto = _db.Moto.Find(request.moto_id);
            if (moto == null)
                return NotFound("Moto não encontrada");

            // valor da diária conforme plano
            decimal valorDiaria = request.plano switch
            {
                7 => 30,
                15 => 28,
                30 => 22,
                45 => 20,
                50 => 18,
                _ => throw new ArgumentException("Plano inválido")
            };

            var ultimoIdentificador = _db.Locacoes
            .OrderByDescending(l => l.Identificador)
            .Select(l => l.Identificador)
            .FirstOrDefault();

            int proximoNumero = 1;
            if (!string.IsNullOrEmpty(ultimoIdentificador) && ultimoIdentificador.StartsWith("locacao"))
            {
                var numeroStr = ultimoIdentificador.Substring("locacao".Length);
                if (int.TryParse(numeroStr, out int numero))
                {
                    proximoNumero = numero + 1;
                }
            }

            string identificador = $"locacao{proximoNumero}";

             var conflito = _db.Locacoes
                .Where(l => l.MotoId == request.moto_id)
                .Any(l =>
                    (request.data_inicio >= l.DataInicio && request.data_inicio <= l.DataTermino) ||
                    (request.data_termino >= l.DataInicio && request.data_termino <= l.DataTermino) ||
                    (request.data_inicio <= l.DataInicio && request.data_termino >= l.DataTermino)
                );

            if (conflito)
                return BadRequest("A moto já está alugada neste período.");

            var locacao = new Locacao
            {
                Identificador = identificador,  
                EntregadorId = request.entregador_id,
                MotoId = request.moto_id,
                DataInicio = request.data_inicio,
                DataTermino = request.data_termino,
                DataPrevisaoTermino = request.data_previsao_termino,
                ValorDiaria = valorDiaria,
                Plano = request.plano
            };

            _db.Locacoes.Add(locacao);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = locacao.Identificador }, locacao);
        }

        // GET /locacao/{id} consultar locação pelo ID
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var locacao = _db.Locacoes.Find(id);
            if (locacao == null) return NotFound();
            return Ok(locacao);
        }


        [HttpPut("{id}/devolucao")]
        public IActionResult Devolver(string id, [FromBody] DevolucaoRequest request)
        {
            if (request == null || request.DataDevolucao == default)
                return BadRequest("Data de devolução é obrigatória.");

            var locacao = _db.Locacoes.Find(id);
            if (locacao == null)
                return NotFound("Locação não encontrada.");

            if (request.DataDevolucao < locacao.DataInicio)
                return BadRequest("A data de devolução não pode ser anterior à data de início da locação.");

            decimal valorTotal = 0;

            // devolução antes da previsão 
            if (request.DataDevolucao < locacao.DataPrevisaoTermino)
            {
                int diasUsados = (int)Math.Ceiling((request.DataDevolucao - locacao.DataInicio).TotalDays);
                int diasNaoUsados = locacao.Plano - diasUsados;

                decimal multaPercentual = locacao.Plano switch
                {
                    7 => 0.20m,
                    15 => 0.40m,
                    _ => 0m
                };

                valorTotal = (diasUsados * locacao.ValorDiaria)
                           + (diasNaoUsados * locacao.ValorDiaria * multaPercentual);
            }
            // devolução no prazo 
            else if (request.DataDevolucao <= locacao.DataPrevisaoTermino)
            {
                valorTotal = locacao.Plano * locacao.ValorDiaria;
            }
            // devolução depois da previsão 
            else
            {
                int diasExtras = (int)Math.Ceiling((request.DataDevolucao - locacao.DataPrevisaoTermino).TotalDays);
                valorTotal = (locacao.Plano * locacao.ValorDiaria)
                           + (diasExtras * 50);
            }

            // atualiza locação
            locacao.DataDevolucao = request.DataDevolucao; 
            locacao.ValorTotal = valorTotal;

            _db.Locacoes.Update(locacao);
            _db.SaveChanges();

            return Ok(new
            {
                locacao.Identificador,
                locacao.EntregadorId,
                locacao.MotoId,
                locacao.DataInicio,
                locacao.DataPrevisaoTermino,
                DataDevolucao = locacao.DataDevolucao,
                locacao.Plano,
                locacao.ValorDiaria,
                ValorTotal = locacao.ValorTotal
            });
        }
    }
}