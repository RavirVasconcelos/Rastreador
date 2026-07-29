using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rastreador.Context;
using Rastreador.DTOs;
using Rastreador.Models;
using Rastreador.Utils;
using System.Runtime.InteropServices;

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

        [HttpGet]
        public async Task<IActionResult> GetRastreadores()
        {
            var rastreadores = await _context.Rastreadors
                                      .Include(r => r.Veiculo)
                                      .Include(r => r.PosicaoAtual)
                                      .Select(r => new RastreadorDTO
                                      {
                                          RastreadorId = r.RastreadorId,
                                          Identificador = r.Identificador,
                                          Imei = r.Imei,
                                          NumeroChip = r.NumeroChip,
                                          Ativo = r.Ativo,
                                          ModoEconomia = r.ModoEconomia,
                                          DataCadastro = r.DataCadastro,
                                          UltimaAtualizacao = r.UltimaAtualizacao,

                                          Veiculo = new VeiculoDTO
                                          {
                                              VeiculoId = r.VeiculoId,
                                              Placa = r.Veiculo.Placa,
                                              MarcaModelo = r.Veiculo.MarcaModelo,
                                              TipoVeiculo = r.Veiculo.TipoVeiculo.ToString(),
                                              AnoVeiculo = r.Veiculo.AnoVeiculo
                                          },

                                          PosicaoAtual = new PosicaoDTO
                                          {
                                                Latitude = r.PosicaoAtual.Latitude,
                                                Longitude = r.PosicaoAtual.Longitude,
                                                DataHora = r.PosicaoAtual.DataHora,
                                                Ancorado = r.Veiculo.Ancoragem.Ativa
                                          }
                                      })
                                      .ToListAsync();

            return Ok(rastreadores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRastreador(int id)
        {
            var rastreador = await _context.Rastreadors
                                    .Include(v => v.Veiculo)
                                    .Include(p => p.PosicaoAtual)
                                    .Where(r => r.RastreadorId == id)
                                    .Select(r => new RastreadorDTO
                                    {
                                        RastreadorId = r.RastreadorId,
                                        Identificador = r.Identificador,
                                        Imei = r.Imei,
                                        NumeroChip = r.NumeroChip,
                                        Ativo = r.Ativo,
                                        ModoEconomia = r.ModoEconomia,
                                        DataCadastro = r.DataCadastro,
                                        UltimaAtualizacao = r.UltimaAtualizacao,


                                        Veiculo = new VeiculoDTO
                                        {
                                            VeiculoId = r.VeiculoId,
                                            Placa = r.Veiculo.Placa,
                                            MarcaModelo = r.Veiculo.MarcaModelo,
                                            TipoVeiculo = r.Veiculo.TipoVeiculo.ToString(),
                                            AnoVeiculo = r.Veiculo.AnoVeiculo
                                        },

                                        PosicaoAtual = new PosicaoDTO
                                        {
                                            Latitude = r.PosicaoAtual.Latitude,
                                            Longitude = r.PosicaoAtual.Longitude,
                                            DataHora = r.PosicaoAtual.DataHora,
                                            Ancorado = r.Veiculo.Ancoragem.Ativa
                                        }
                                    }).FirstOrDefaultAsync();

            if (rastreador == null)
                return NotFound();

            return Ok(rastreador);
        }

        [HttpPost("CadastrarRastreador")]
        public async Task<IActionResult> CadastrarRastreador(CadastrarRastreadorDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Dados inválidos");
            }

            var identificadorGuid = Guid.NewGuid();

            var rastreadorExistente = await _context.Rastreadors.Where(r => r.Identificador == identificadorGuid && r.Imei == dto.Imei).FirstOrDefaultAsync();

            if (rastreadorExistente != null)
            {
                return BadRequest("Rastreador já existente.");
            }

            dto.Placa = dto.Placa
                        .Replace("-", "")
                        .Replace(".", "")
                        .Trim()
                        .ToUpper();

            var veiculoExistente = await _context.Veiculos.Where(v => v.Placa.ToUpper() == dto.Placa).FirstOrDefaultAsync();

            if (veiculoExistente == null)
            {
                return BadRequest("Veiculo não cadastrado.");
            }

            var veiculoTemRastreador = await _context.Rastreadors.AnyAsync(r => r.VeiculoId == veiculoExistente.VeiculoId);

            if (veiculoTemRastreador)
            {
                return BadRequest("Esse veículo já possui um rastreador.");
            }

            var novoRastreador = new RastreadorModulo
            {
                Identificador = identificadorGuid,
                Imei = dto.Imei,
                NumeroChip = dto.Chip,
                VeiculoId = veiculoExistente.VeiculoId,
                DataCadastro = DateTime.UtcNow,
                Ativo = true,
                ModoEconomia = false,
                UltimaAtualizacao = DateTime.UtcNow
            };

            _context.Rastreadors.Add(novoRastreador);

            _context.Ancoragems.Add(new Ancoragem
            {
                VeiculoId = veiculoExistente.VeiculoId,
                Ativa = false,
                Latitude = 0,
                Longitude = 0,
                RaioMetros = 300
            });

            await _context.SaveChangesAsync();
            return Ok("Rastreador cadastrado com sucesso.");
        }

        [HttpGet("GetLocalizacaoAtual")]
        public async Task<IActionResult> GetLocalizacaoAtual(int veiculoId, double latitude, double longitude)
        {
            var rastreador = await _context.Rastreadors
                .Include(r => r.Veiculo)
                    .ThenInclude(v => v.Ancoragem)
                .Include(r => r.PosicaoAtual)
                .FirstOrDefaultAsync(r => r.VeiculoId == veiculoId);


            if (rastreador == null)
            {
                return BadRequest("Veículo não registrado.");
            }


            if (rastreador.PosicaoAtual == null)
            {
                rastreador.PosicaoAtual = new PosicaoVeiculo
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    DataHora = DateTime.UtcNow,
                    RastreadorModuloId = rastreador.RastreadorId
                };
            }
            else
            {
                rastreador.PosicaoAtual.Latitude = latitude;
                rastreador.PosicaoAtual.Longitude = longitude;
                rastreador.PosicaoAtual.DataHora = DateTime.UtcNow;
            }


            if (rastreador.Veiculo.Ancoragem != null &&
                rastreador.Veiculo.Ancoragem.Ativa)
            {
                bool dentroDaAncoragem = Calculos.VerificarSeEstarForaDaAncoragem(
                    latitude,
                    longitude,
                    rastreador.Veiculo.Ancoragem.Latitude,
                    rastreador.Veiculo.Ancoragem.Longitude,
                    rastreador.Veiculo.Ancoragem.RaioMetros
                );


                if (!dentroDaAncoragem)
                {
                    await EnviarMensagemDeAlerta(veiculoId);

                    return Ok(new
                    {
                        LocalizacaoAtual = $"O veículo {rastreador.Veiculo.Placa} está fora da área de ancoragem."
                    });
                }
            }


            await _context.SaveChangesAsync();


            return Ok(new
            {
                LocalizacaoAtual = $"Localização atual do veículo {rastreador.Veiculo.Placa}",
                Latitude = latitude,
                Longitude = longitude
            });
        }

        [HttpPost("ColocarAncoragem")]
        public async Task<IActionResult> ColocarAncoragem(int veiculoId)
        {
            var rastreador = await _context.Rastreadors
                .Include(r => r.Veiculo)
                    .ThenInclude(v => v.Ancoragem)
                .Include(r => r.PosicaoAtual)
                .FirstOrDefaultAsync(r => r.VeiculoId == veiculoId);


            if (rastreador == null)
            {
                return BadRequest("Veículo não registrado.");
            }


            if (rastreador.Veiculo.Ancoragem == null)
            {
                return BadRequest("Veículo não possui ancoragem cadastrada.");
            }


            if (rastreador.Veiculo.Ancoragem.Ativa)
            {
                return BadRequest("Ancoragem já está ativa.");
            }


            if (rastreador.PosicaoAtual == null)
            {
                return BadRequest("Veículo ainda não possui localização.");
            }


            rastreador.Veiculo.Ancoragem.Latitude = rastreador.PosicaoAtual.Latitude;
            rastreador.Veiculo.Ancoragem.Longitude = rastreador.PosicaoAtual.Longitude;
            rastreador.Veiculo.Ancoragem.Ativa = true;


            await _context.SaveChangesAsync();


            return Ok("Ancoragem ativada com sucesso.");
        }

        [HttpPost("RemoverAncoragem")]
        public async Task<IActionResult> RemoverAncoragem(int veiculoId)
        {
            var rastreador = await _context.Rastreadors
                .Include(r => r.Veiculo)
                    .ThenInclude(v => v.Ancoragem)
                .FirstOrDefaultAsync(r => r.VeiculoId == veiculoId);


            if (rastreador == null)
            {
                return BadRequest("Veículo não registrado.");
            }


            if (!rastreador.Veiculo.Ancoragem.Ativa)
            {
                return BadRequest("Ancoragem já está desativada.");
            }


            rastreador.Veiculo.Ancoragem.Ativa = false;


            await _context.SaveChangesAsync();


            return Ok("Ancoragem desativada com sucesso.");
        }

        [HttpGet("VerificarAncoragem")]
        public async Task<IActionResult> VerificarAncoragem(int veiculoId)
        {
            var rastreador = await _context.Rastreadors
                .Include(r => r.Veiculo)
                    .ThenInclude(v => v.Ancoragem)
                .FirstOrDefaultAsync(r => r.VeiculoId == veiculoId);

            
            if (rastreador == null)
            {
                return BadRequest("Veículo não registrado.");
            }


            if (rastreador.Veiculo.Ancoragem == null)
            {
                return BadRequest("Veículo não possui ancoragem cadastrada.");
            }


            return Ok(new
            {
                AncoragemAtiva = rastreador.Veiculo.Ancoragem.Ativa
            });
        }

        [HttpPost("DefinirModoSono")]
        public async Task<IActionResult> DefinirModoSono(DefinirModoSonoDTO dto)
        {
            var veiculoExistente = await _context.Veiculos.Where(v => v.Placa == dto.Placa).FirstOrDefaultAsync();
            var identificadorRastreador = await _context.Rastreadors.Where(r => r.Identificador == dto.IdentificadorRastreador).FirstOrDefaultAsync();

            if (veiculoExistente == null || identificadorRastreador == null)
            {
                return BadRequest("Veículo ou rastreador não encontrados.");
            }

            var definirSono = new SonoRastreador
            {
                HoraInicio = dto.HoraInicio,
                HoraFim = dto.HoraFim,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Ativo = false,
                RastreadorId = identificadorRastreador.RastreadorId
            };

            _context.SonoRastreadors.Add(definirSono);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Mensagem = $"Modo Sono Definido com sucesso. Horário de inicio: {dto.HoraFim} , Horário de Fim: {dto.HoraFim}"
            });
        }

        [HttpPost("AtivarModoSono")]
        public async Task<IActionResult> AtivarModoSono(Guid identificadorRastreador, double latitude, double longitude)
        {
            var rastreador = await _context.Rastreadors
                .Include(s => s.SonoRastreador)
                .Where(r => r.Identificador == identificadorRastreador).FirstOrDefaultAsync();

            if (rastreador == null)
            {
                return BadRequest("Rastreador não cadastrado.");
            }

            if (rastreador.SonoRastreador == null)
            {
                return BadRequest("Modo sono não foi configurado ainda.");
            }

            var agora = DateTime.Now.TimeOfDay;

            var inicio = rastreador.SonoRastreador.HoraInicio.Value.TimeOfDay;
            var fim = rastreador.SonoRastreador.HoraFim.Value.TimeOfDay;

            bool dentroHorario;

            if (inicio < fim)
            {
                dentroHorario = agora >= inicio && agora <= fim;
            }
            else
            {
                dentroHorario = agora >= inicio || agora <= fim;
            }

            if (!dentroHorario)
            {
                return BadRequest("Fora do horário configurado para o modo sono.");
            }

            bool dentroDoPerimetro = Calculos.VerificarSeEstarForaDaAncoragem(
                                                    latitude,
                                                    longitude,
                                                    rastreador.SonoRastreador.Latitude,
                                                    rastreador.SonoRastreador.Longitude,
                                                    rastreador.SonoRastreador.RaioPerimetro
                                                );

            if (!dentroDoPerimetro)
            {
                return BadRequest("O veículo não está no local configurado para ativar o modo sono.");
            }


            rastreador.SonoRastreador.Ativo = true;

            await _context.SaveChangesAsync();

            return Ok("Modo sono ativado com sucesso.");
        }

        [HttpGet("EnviarMensagemDeAlerta")]
        public async Task<IActionResult> EnviarMensagemDeAlerta(int veiculoId)
        {
            var rastreador = await _context.Rastreadors
                .Include(r => r.Veiculo)
                    .ThenInclude(v => v.Cliente)
                .FirstOrDefaultAsync(r => r.VeiculoId == veiculoId);


            if (rastreador == null)
            {
                return BadRequest("Veículo não encontrado.");
            }


            var cliente = rastreador.Veiculo.Cliente.Nome;


            return Ok(new
            {
                Mensagem = $"Alerta: Prezado cliente {cliente}, seu veículo saiu da área de ancoragem, confirme se é você conduzindo o veículo."
            });
        }
    }
}
