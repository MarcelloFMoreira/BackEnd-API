using Microsoft.AspNetCore.Mvc;
using slm.infraestrutura;
using Slm.Domain.Entidades;
using System.Linq;

namespace sistema_locacao_motos.Controllers
{
    [Route("/motos")]
    [ApiController]
    public class motosController : ControllerBase
    {
        private readonly SlmDbContext _db;

        public motosController(SlmDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // POST /motos  (cadastrar uma nova moto)
        [HttpPost]
        public IActionResult Create([FromBody] Moto moto)
        {
            // Valida se o corpo da requisição é nulo.
            if (moto == null) return BadRequest();

            // Valida se a placa é única
            if (_db.Moto.Any(m => m.Placa == moto.Placa))
                return Conflict("Placa já cadastrada.");

            _db.Moto.Add(moto);
            _db.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = moto.Identificador }, moto);
        }

        // GET /motos  (consultar motos + filtro por placa)
        [HttpGet]
        public IActionResult Get([FromQuery] string? placa = null)
        {
            // Consulta base
            var query = _db.Moto.AsQueryable();

            // Se a placa for fornecida,  filtrar
            if (!string.IsNullOrWhiteSpace(placa))
                query = query.Where(m => m.Placa == placa);

            return Ok(query.ToList());
        }

        // GET /motos/{id}  (consultar moto pelo identificador)
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            // Busca a moto pelo identificador.
            var moto = _db.Moto.Find(id);

            // Retorna 404 (Not Found) se a moto não for encontrada.
            if (moto == null) return NotFound();

            return Ok(moto);
        }

        // PUT /motos/{id}/placa  (modificar somente a placa)
        [HttpPut("{id}/placa")]
        public IActionResult UpdatePlaca(string id, [FromBody] AtualizarPlacaRequest body)
        {
            // Valida se o corpo da req. é nula
            if (body == null || string.IsNullOrWhiteSpace(body.Placa))
                return BadRequest("Informe a nova placa no corpo: { \"placa\": \"ABC-1234\" }");

            // Busca a moto pelo identificador.
            var moto = _db.Moto.Find(id);

            // Retorna 404 se a moto não for encontrada.
            if (moto == null) return NotFound();

            // valida duplicidade de placa 
            if (_db.Moto.Any(m => m.Placa == body.Placa && m.Identificador != id))
                return Conflict("Placa já cadastrada.");

            moto.Placa = body.Placa;
            _db.SaveChanges();

            // Retorna 204  indicando que a atualização foi bem sucedida.
            return NoContent();
        }

        // DELETE /motos/{id}  (remover moto)
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            // Busca a moto pelo identificador.
            var moto = _db.Moto.Find(id);

            // Retorna 404 se a moto não for encontrada.
            if (moto == null) return NotFound();

            _db.Moto.Remove(moto);
            _db.SaveChanges();

            // Retorna 204  indicando que a atualização foi bem sucedida.
            return NoContent();
        }
    }

    // DTO para PUT /motos/{id}/placa
    public class AtualizarPlacaRequest
    {
        public string Placa { get; set; }
    }



}

