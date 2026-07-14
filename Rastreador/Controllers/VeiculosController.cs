using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rastreador.Context;
using Rastreador.Models;

namespace Rastreador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly RastreadorContext _context;

        public VeiculosController(RastreadorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetVeiculos()
        {
            var veiculos = await _context.Veiculos.ToListAsync();
            return Ok(veiculos);
        }

        [HttpPost("CadastrarVeiculo")]
        public async Task<IActionResult> CadastrarVeiculo([FromBody] Veiculo veiculo)
        {
            if (veiculo == null)
            {
                return BadRequest("O veículo não pode ser nulo.");
            }
            _context.Veiculos.Add(veiculo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVeiculos), new { id = veiculo.VeiculoId }, veiculo);
        }
    }
}
