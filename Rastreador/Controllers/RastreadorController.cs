using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rastreador.Context;
using Rastreador.Models;
using Rastreador.Utils;

namespace Rastreador.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RastreadorController : ControllerBase
    {
        private readonly RastreadorContext _context;

        public RastreadorController(RastreadorContext context)
        {
            _context = context;
        }

        [HttpPost("CadastrarRastreador")]
        public async Task<IActionResult> CadastrarRastreador(RastreadorModulo rastreador)
        {
            var veiculoExistente = await _context.Veiculos.FindAsync(rastreador.VeiculoId);

            if (veiculoExistente == null)
            {
                return BadRequest("Veículo não encontrado.");
            }

            rastreador.DataCadastro = DateTime.UtcNow;
            rastreador.Ativo = true;
            rastreador.ModoEconomia = false;
            rastreador.UltimaAtualizacao = DateTime.UtcNow;
            _context.Rastreadors.Add(rastreador);
            await _context.SaveChangesAsync();
            return Ok("Rastreador cadastrado com sucesso.");
        }

        [HttpGet("GetLocalizacaoAtual")]
        public async Task<IActionResult> GetLocalizacaoAtual(int veiculoId, double latitude, double longitude)
        {
            var veiculoRastreado = await _context.Rastreadors
                .Include(v => v.Veiculo)
                .Include(p => p.Posicoes)
                .FirstOrDefaultAsync(v => v.VeiculoId == veiculoId);

            if (veiculoRastreado == null)
            {
                return BadRequest("Veículo não registrado.");
            }

            var ultimaPosicao = veiculoRastreado.Posicoes.OrderByDescending(p => p.DataHora).FirstOrDefault();

            if (!Calculos.VerificarSeEstarForaDaAncoragem(latitude, longitude, ultimaPosicao.Latitude, ultimaPosicao.Latitude, veiculoRastreado.Veiculo.Ancoragem.RaioMetros))
            {
                await EnviarMensagemDeAlerta(veiculoId);
                return Ok(new
                {
                    LocalizacaoAtual = $"O veículo {veiculoRastreado.Veiculo.Placa} está fora da área de ancoragem. Enviando mensagem de alerta para o cliente."
                });
            }

            veiculoRastreado.Posicoes.Add(new PosicaoVeiculo
            {
                Latitude = latitude,
                Longitude = longitude,
                DataHora = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();

            return Ok(new
            {
                LocalizacaoAtual = $"Localização atual do veículo {veiculoRastreado.Veiculo.Placa}: Latitude: {latitude}, Longitude: {longitude}"
            });
        }

        [HttpPost("ColocarAncoragem")]
        public async Task<IActionResult> ColocarAncoragem(int veiculoId)
        {
            var veiculoRastreado = await _context.Rastreadors
                .Include(v => v.Veiculo)
                .Include(p => p.Posicoes)
                .Include(a => a.Veiculo.Ancoragem)
                .FirstOrDefaultAsync(v => v.VeiculoId == veiculoId);

            if (veiculoRastreado == null)
            {
                return BadRequest("Veículo não registrado.");
            }

            if (veiculoRastreado.Veiculo.Ancoragem.Ativa == true)
            {
                return BadRequest("Ancoragem já está ativa.");
            }

            // Salvar localização atual com a ancoragem
            var ultimaPosicao = veiculoRastreado.Posicoes.OrderByDescending(p => p.DataHora).FirstOrDefault();

            if (ultimaPosicao != null)
            {
                ultimaPosicao.Ancorado = true;
                veiculoRastreado.Veiculo.Ancoragem.Latitude = ultimaPosicao.Latitude;
                veiculoRastreado.Veiculo.Ancoragem.Longitude = ultimaPosicao.Longitude;
                veiculoRastreado.Veiculo.Ancoragem.Ativa = true;
            }
            
            await _context.AddAsync(veiculoRastreado);
            await _context.SaveChangesAsync();

            return Ok("Ancoragem ativada com sucesso.");
        }

        [HttpPost("RemoverAncoragem")]
        public async Task<IActionResult> RemoverAncoragem(int veiculoId)
        {
            var veiculoRastreado = await _context.Rastreadors
                .Include(v => v.Veiculo)
                .Include(p => p.Posicoes)
                .Include(a => a.Veiculo.Ancoragem)
                .FirstOrDefaultAsync(v => v.VeiculoId == veiculoId);

            if (veiculoRastreado == null)
            {
                return BadRequest("Veículo não registrado.");
            }

            if (veiculoRastreado.Veiculo.Ancoragem.Ativa == false)
            {
                return BadRequest("Ancoragem já está desativada.");
            }

            var posicaoAncorada = veiculoRastreado.Posicoes.OrderByDescending(p => p.DataHora).FirstOrDefault();

            if (posicaoAncorada != null)
            {
                posicaoAncorada.Ancorado = false;
                veiculoRastreado.Veiculo.Ancoragem.Ativa = false;
            }

            await _context.AddAsync(veiculoRastreado);
            await _context.SaveChangesAsync();

            return Ok("Ancoragem desativada com sucesso.");
        }

        [HttpGet("EnviarMensagemDeAlerta")]
        public async Task<IActionResult> EnviarMensagemDeAlerta(int veiculoId)
        {
            var veiculoAncorado = await _context.Rastreadors
                .Include(v => v.Veiculo)
                .Include(p => p.Posicoes)
                .Include(a => a.Veiculo.Ancoragem)
                .Include(c => c.Veiculo.Cliente)
                .FirstOrDefaultAsync(v => v.VeiculoId == veiculoId);

            var cliente = veiculoAncorado.Veiculo.Cliente.Nome;

            return Ok(new
            {
                EnviarMensagemDeAlerta = $"Alerta: Prezado cliente {cliente}, seu veículo saiu da área de ancoragem, por favor confirme se é você conduzindo o veículo."
            });
        }
    }
}
