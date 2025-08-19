using Microsoft.AspNetCore.Mvc;
using slm.infraestrutura;
using Slm.Domain.Entidades;
using System.Linq;

namespace sistema_locacao_motos.Controllers
{
    [Route("/entregadores")]
    [ApiController]
    public class entregadoresController : ControllerBase
    {
        private readonly SlmDbContext _db;

        public entregadoresController(SlmDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // POST /entregadores (cadastrar novo entregador)
        [HttpPost]
        public IActionResult Create([FromBody] Entregador entregador)
        {
            if (entregador == null) return BadRequest();

            // validações de tamanho
            if (entregador.cnpj?.Length > 14)
                return BadRequest("CNPJ não pode ter mais que 14 caracteres.");
            if (entregador.numero_cnh?.Length > 11)
                return BadRequest("Número da CNH não pode ter mais que 11 caracteres.");
            string tipoCnhUpperCase = entregador.tipo_cnh.ToUpper();
                if (tipoCnhUpperCase != "A" && tipoCnhUpperCase != "B" && tipoCnhUpperCase != "A+B")
                return BadRequest("Tipo de CNH inválido. Use 'A', 'B' ou 'A+B'.");

            // CNPJ único
            if (_db.Entregador.Any(e => e.cnpj == entregador.cnpj))
                return Conflict("CNPJ já cadastrado.");

            // CNH única
            if (_db.Entregador.Any(e=> e.numero_cnh == entregador.numero_cnh))
                return Conflict("CNH já cadastrada.");

            _db.Entregador.Add(entregador);
            _db.SaveChanges();

            return CreatedAtAction(null, new { id = entregador.identificador }, entregador);
        }

        [HttpPost("{id}/cnh")]
        public IActionResult UploadCnh(string id, [FromBody] AtualizarImagemCnhRequest body)
        {
            if (body == null || string.IsNullOrWhiteSpace(body.imagem_cnh))
                return BadRequest("Informe a imagem CNH no corpo: { \"imagem_cnh\": \"base64string\" }");

            var entregador = _db.Entregador.Find(id);
            if (entregador == null) return NotFound($"Entregador com ID '{id}' não encontrado.");

            entregador.imagem_cnh = body.imagem_cnh;
            _db.SaveChanges();

            return NoContent();
        }
    }

    public class AtualizarImagemCnhRequest
    {
        public string imagem_cnh { get; set; }
    }
}