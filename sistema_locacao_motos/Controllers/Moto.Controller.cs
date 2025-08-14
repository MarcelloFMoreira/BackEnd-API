using Microsoft.AspNetCore.Mvc;
using slm.infraestrutura;
using Slm.Domain.Entidades;
using System.Linq;

namespace sistema_locacao_motos.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MotosController : ControllerBase
    {
        private readonly SlmDbContext _db;

        public MotosController(SlmDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        // POST /motos - Cadastrar uma nova moto
        [HttpPost]
        public IActionResult Create([FromBody] Moto moto)
        {
            if (moto == null) return BadRequest();

            // Verifica se a placa já existe
            if (_db.Moto.Any(m => m.Placa == moto.Placa))
                return Conflict("Placa já cadastrada.");

            _db.Moto.Add(moto);
            _db.SaveChanges();

            // Aqui você poderia publicar o evento de moto cadastrada via mensageria
            // e criar um consumidor para notificações de ano == 2024

            return CreatedAtAction(nameof(GetById), new { id = moto.Identificador }, moto);
        }

        // GET /motos - Consultar todas as motos (filtro opcional por placa)
        [HttpGet]
        public IActionResult Get([FromQuery] string placa = null)
        {
            var query = _db.Moto.AsQueryable();

            if (!string.IsNullOrEmpty(placa))
                query = query.Where(m => m.Placa == placa);

            return Ok(query.ToList());
        }

        // GET /motos/{id} - Consultar moto pelo ID
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var moto = _db.Moto.Find(id);
            if (moto == null) return NotFound();
            return Ok(moto);
        }

        // PUT /motos/{id}/placa - Modificar a placa de uma moto
        [HttpPut("{id}/placa")]
        public IActionResult UpdatePlaca(string id, [FromBody] string novaPlaca)
        {
            var moto = _db.Moto.Find(id);
            if (moto == null) return NotFound();

            // Verifica se a nova placa já existe
            if (_db.Moto.Any(m => m.Placa == novaPlaca && m.Identificador != id))
                return Conflict("Placa já cadastrada.");

            moto.Placa = novaPlaca;
            _db.SaveChanges();

            return NoContent();
        }

        // DELETE /motos/{id} - Remover moto pelo ID
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var moto = _db.Moto.Find(id);
            if (moto == null) return NotFound();

            // Aqui você poderia checar se a moto tem locações ativas antes de remover
            _db.Moto.Remove(moto);
            _db.SaveChanges();

            return NoContent();
        }
    }
}