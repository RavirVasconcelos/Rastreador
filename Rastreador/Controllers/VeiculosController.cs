using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rastreador.Context;
using Rastreador.DTOs;
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
            var usuarioLogadoId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value);

            var veiculos = await _context.Veiculos.Where(v => v.ClienteId == usuarioLogadoId).ToListAsync();
            return Ok(veiculos);
        }

        [HttpPost("CadastrarVeiculo")]
        public async Task<IActionResult> CadastrarVeiculo([FromBody] CadastrarVeiculoDTO dto)
        {
            var usuarioLogadoId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value);
            var usuarioLogadoCpf = User.Claims.FirstOrDefault(c => c.Type == "Cpf")?.Value;

            var veiculoExistente = await _context.Veiculos.Where(v => v.Placa == dto.Placa && v.ClienteId == usuarioLogadoId).FirstOrDefaultAsync();

            if (veiculoExistente != null)
            {
                return BadRequest("Veículo já cadastrado.");
            }

            var veiculoNovo = new Veiculo
            {
                Placa = dto.Placa,
                MarcaModelo = dto.MarcaModelo,
                TipoVeiculo = dto.TipoVeiculo,
                AnoVeiculo = dto.AnoVeiculo,
                ClienteId = usuarioLogadoId,
                CpfProprietario = usuarioLogadoCpf
            };


            _context.Veiculos.Add(veiculoNovo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVeiculos), new { id = veiculoNovo.VeiculoId }, veiculoNovo);
        }
    }
}
