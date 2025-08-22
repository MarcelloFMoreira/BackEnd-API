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
            if (tipoCnhUpperCase != "A" && tipoCnhUpperCase != "B" && tipoCnhUpperCase != "A+B" && tipoCnhUpperCase !="AB")
                return BadRequest("Tipo de CNH inválido. Use 'A', 'B' ou 'A+B'.");

            // CNPJ único
            if (_db.Entregador.Any(e => e.cnpj == entregador.cnpj))
                return Conflict("CNPJ já cadastrado.");

            // CNH única
            if (_db.Entregador.Any(e => e.numero_cnh == entregador.numero_cnh))
                return Conflict("CNH já cadastrada.");

            _db.Entregador.Add(entregador);
            _db.SaveChanges();

            return CreatedAtAction(null, new { id = entregador.identificador }, entregador);
        }

        [HttpPost("{id}/cnh")]
        public IActionResult UploadCnh(string id, [FromForm] AtualizarImagemCnhRequest body)
        {
            if (body.imagem_cnh == null || body.imagem_cnh.Length == 0)
                return BadRequest("A imagem da CNH é obrigatória.");

            // valida extensão e content-type
            var extensao = Path.GetExtension(body.imagem_cnh.FileName).ToLowerInvariant();
            var contentType = body.imagem_cnh.ContentType.ToLowerInvariant();

            var formatosPermitidos = new[] { ".png", ".bmp" };
            var contentTypesPermitidos = new[] { "image/png", "image/bmp" };

            if (!formatosPermitidos.Contains(extensao) || !contentTypesPermitidos.Contains(contentType))
                return BadRequest("Apenas imagens PNG ou BMP são permitidas.");

            // garante que a pasta Storage existe
            var storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Storage");
            if (!Directory.Exists(storagePath))
                Directory.CreateDirectory(storagePath);

            // cria nome único para evitar conflitos
            var fileName = $"{Guid.NewGuid()}{extensao}";
            var filePath = Path.Combine(storagePath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                body.imagem_cnh.CopyTo(stream);
            }

            // busca entregador
            var entregador = _db.Entregador.Find(id);
            if (entregador == null)
                return NotFound($"Entregador com ID '{id}' não encontrado.");

            // salva path como string no banco
            entregador.imagem_cnh = Path.Combine("Storage", fileName);

            _db.Entregador.Update(entregador);
            _db.SaveChanges();

            return Ok(new
            {
                message = "Imagem da CNH atualizada com sucesso.",
                path = entregador.imagem_cnh
            });
        }
    }

        public class AtualizarImagemCnhRequest
    {
        public IFormFile imagem_cnh { get; set; }

    }
}