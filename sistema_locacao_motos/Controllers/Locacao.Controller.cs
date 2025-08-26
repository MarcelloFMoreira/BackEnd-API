
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
            public DateTime data_devolucao { get; set; }
        }



        // POST /locacao - Endpoint para criar uma nova locação
        [HttpPost]
        public IActionResult CriarLocacao([FromBody] CriarLocacaoRequest request)
        {

            // Valida se o corpo da requisição é nulo
            if (request == null)
                return BadRequest();


            // data de início deve ser pelo menos 1 dia após a data atual
            if (request.data_inicio.Date <= DateTime.UtcNow.Date.AddDays(1))
            {
                return BadRequest("A data de início deve ser pelo menos 1 dia após a data atual");
            }

            // Busca o entregador pelo ID fornecido
            var entregador = _db.Entregador.Find(request.entregador_id);
            // Retorna erro 404 se o entregador não for encontrado
            if (entregador == null)
                return NotFound("Entregador não encontrado");

            // Valida se o entregador possui CNH categoria 'A'
            if (!entregador.tipo_cnh.Contains("A"))
                return BadRequest("Somente entregadores com CNH categoria A podem alugar motos");


            // Busca a moto pelo ID fornecido
            var moto = _db.Moto.Find(request.moto_id);
            // Retorna erro 404 se a moto não for encontrada
            if (moto == null)
                return NotFound("Moto não encontrada");

            // Define os planos de locação válidos em dias
            var planosValidos = new[] { 7, 15, 30, 45, 50 };

            // Valida se o plano da requisição está entre os planos válidos
            if (!planosValidos.Contains(request.plano))
                return BadRequest("Plano inválido. Os planos disponíveis são: 7, 15, 30, 45 ou 50 dias.");

            // Calcula o valor da diária com base no plano usando um switch expression
            decimal valorDiaria = request.plano switch
            {
                7 => 30,
                15 => 28,
                30 => 22,
                45 => 20,
                50 => 18,
                _ => 0
            };



            // Busca o último identificador de locação para gerar o próximo
            var ultimoIdentificador = _db.Locacoes
                .OrderByDescending(l => l.Identificador)
                .Select(l => l.Identificador)
                .FirstOrDefault();

            int proximoNumero = 1;
            // Se o último identificador existir e começar com "locacao"
            if (!string.IsNullOrEmpty(ultimoIdentificador) && ultimoIdentificador.StartsWith("locacao"))
            {
                // localiza o número do identificador e incrementa
                var numeroStr = ultimoIdentificador.Substring("locacao".Length);
                if (int.TryParse(numeroStr, out int numero))
                {
                    proximoNumero = numero + 1;
                }
            }

            // Constrói o novo identificador
            string identificador = $"locacao{proximoNumero}";


            // Checa se a moto já está alugada no período solicitado
            var conflito = _db.Locacoes
                .Where(l => l.MotoId == request.moto_id)
                .Any(l =>
                    (request.data_inicio >= l.DataInicio && request.data_inicio <= l.DataTermino) ||
                    (request.data_termino >= l.DataInicio && request.data_termino <= l.DataTermino) ||
                    (request.data_inicio <= l.DataInicio && request.data_termino >= l.DataTermino)
                );

            // Retorna erro se houver conflito de agendamento
            if (conflito)
                return BadRequest("A moto já está alugada neste período.");

            // Cria uma nova instância da entidade Locacao
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

            // Adiciona a nova locação ao contexto do banco de dados
            _db.Locacoes.Add(locacao);
            // Salva as mudanças no banco de dados
            _db.SaveChanges();

            // Retorna uma resposta 201 (Created) com a URL para a nova locação
            return CreatedAtAction(nameof(GetById), new { id = locacao.Identificador }, locacao);
        }

        // GET /locacao/{id} - consultar uma locação pelo seu ID
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            // Busca a locação pelo identificador
            var locacao = _db.Locacoes.Find(id);
            // Retorna erro 404 se a locação não for encontrada
            if (locacao == null) return NotFound();
            // Retorna a locação encontrada com status 200 (OK)
            return Ok(locacao);
        }


        [HttpPut("{id}/devolucao")]
        public IActionResult Devolver(string id, [FromBody] DevolucaoRequest request)
        {
            //  Validação inicial da requisição de devolução
            if (request == null || request.data_devolucao == default)
                return BadRequest("Data de devolução é obrigatória.");

            // Busca e validação da locação
            var locacao = _db.Locacoes.Find(id);
            if (locacao == null)
                return NotFound("Locação não encontrada.");

            // Valida se a data de devolução é anterior à data de início da locação
            if (request.data_devolucao < locacao.DataInicio)
                return BadRequest("A data de devolução não pode ser anterior à data de início da locação.");

            // Cálculo do valor total da locação com base na data de devolução
            decimal valorTotal = 0;

            // Devolução antes da data de previsão de término
            if (request.data_devolucao < locacao.DataPrevisaoTermino)
            {
                // Calcula a quantidade de dias utilizados na locação
                int diasUsados = (int)Math.Ceiling((request.data_devolucao - locacao.DataInicio).TotalDays);
                // Calcula a quantidade de dias não utilizados
                int diasNaoUsados = locacao.Plano - diasUsados;

                // Define a multa percentual com base no plano
                decimal multaPercentual = locacao.Plano switch
                {
                    7 => 0.20m, //  multa para plano de 7 dias
                    15 => 0.40m, //  multa para plano de 15 dias
                    _ => 0m // Sem multa para outros planos (30, 45, 50)
                };

                // Calcula o valor total: dias usados + multa sobre os dias não utilizados
                valorTotal = (diasUsados * locacao.ValorDiaria)
                           + (diasNaoUsados * locacao.ValorDiaria * multaPercentual);
            }
            
            else if (request.data_devolucao == locacao.DataPrevisaoTermino)
            {
                // O valor total é o valor do plano data devolução na data prevista
                valorTotal = locacao.Plano * locacao.ValorDiaria;
            }
            // Devolução após a data de previsão de término
            else
            {
                // Calcula a quantidade de dias extras
                int diasExtras = (int)Math.Ceiling((request.data_devolucao - locacao.DataPrevisaoTermino).TotalDays);
                // O valor total é o valor do plano + a multa de R$ 50 por dia extra
                valorTotal = (locacao.Plano * locacao.ValorDiaria)
                           + (diasExtras * 50);
            }

            
            // Atualiza a data de devolução e o valor total na entidade de locação
            locacao.DataDevolucao = request.data_devolucao;
            locacao.ValorTotal = valorTotal;

            _db.Locacoes.Update(locacao);
            _db.SaveChanges();


            // Retorna um JSON
            return Ok(new
            {
                locacao.Identificador,
                locacao.EntregadorId,
                locacao.MotoId,
                locacao.DataInicio,
                locacao.DataPrevisaoTermino,
                data_devolucao = locacao.DataDevolucao,
                locacao.Plano,
                locacao.ValorDiaria,
                ValorTotal = locacao.ValorTotal
            });
        }
    }
}