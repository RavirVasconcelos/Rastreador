using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rastreador.Context;
using Rastreador.DTOs;
using Rastreador.Models;
using Rastreador.Models.Enums;
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

        #region MOBILE

        [HttpGet]
        public async Task<IActionResult> GetRastreadores()
        {
            var rastreadores = await _context.Rastreadors
                .Include(r => r.Veiculo)
                    .ThenInclude(v => v.Ancoragem)
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

                    PosicaoAtual = r.PosicaoAtual == null
                        ? null
                        : new PosicaoDTO
                        {
                            Latitude = r.PosicaoAtual.Latitude,
                            Longitude = r.PosicaoAtual.Longitude,
                            DataHora = r.PosicaoAtual.DataHora,
                            Velocidade = r.PosicaoAtual.VelocidadeKmH,

                            Ancorado = r.Veiculo.Ancoragem != null &&
                                       r.Veiculo.Ancoragem.Ativa
                        }
                })
                .ToListAsync();

            return Ok(rastreadores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRastreador(int id)
        {
            var rastreador = await _context.Rastreadors
                .Include(r => r.Veiculo)
                    .ThenInclude(v => v.Ancoragem)
                .Include(r => r.PosicaoAtual)
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

                    PosicaoAtual = r.PosicaoAtual == null
                        ? null
                        : new PosicaoDTO
                        {
                            Latitude = r.PosicaoAtual.Latitude,
                            Longitude = r.PosicaoAtual.Longitude,
                            DataHora = r.PosicaoAtual.DataHora,
                            Velocidade = r.PosicaoAtual.VelocidadeKmH,

                            Ancorado = r.Veiculo.Ancoragem != null &&
                                       r.Veiculo.Ancoragem.Ativa
                        }
                })
                .FirstOrDefaultAsync();

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
            var usuarioLogado = User.Claims.FirstOrDefault(c => c.Type == "Cpf")?.Value;
            var veiculoExistente = await _context.Veiculos.Where(v => v.Placa == dto.Placa).FirstOrDefaultAsync();
            var identificadorRastreador = await _context.Rastreadors.Where(r => r.Identificador == dto.IdentificadorRastreador).FirstOrDefaultAsync();

            if (veiculoExistente == null || identificadorRastreador == null)
            {
                return BadRequest("Veículo ou rastreador não encontrados.");
            }

            if (veiculoExistente.Rastreador.Identificador != dto.IdentificadorRastreador)
            {
                return BadRequest("Esse rastreador não pertence a esse veículo.");
            }

            if (veiculoExistente.CpfProprietario != usuarioLogado)
            {
                return BadRequest("Você não tem permissão para administrar esse recurso.");
            }

            var dataBase = new DateTime(2000, 1, 1);

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
                Mensagem = $"Modo Sono Definido com sucesso. Horário de inicio: {dto.HoraInicio} , Horário de Fim: {dto.HoraFim}"
            });
        }

        [HttpPatch("AlterarHorarioModoSono")]
        public async Task<IActionResult> AlterarHorarioModoSono(AlterarHorarioModoSonoDTO dto)
        {
            var usuarioLogado = User.Claims.FirstOrDefault(c => c.Type == "Cpf")?.Value;
            var identificadorRastreador = await _context.Rastreadors.Where(r => r.Identificador == dto.IdentificadorRastreador && r.Veiculo.CpfProprietario == usuarioLogado).FirstOrDefaultAsync();

            if (identificadorRastreador == null)
            {
                return BadRequest("Não tem rastreador cadastrado.");
            }

            if (identificadorRastreador.SonoRastreador == null)
            {
                return BadRequest("Modo sono ainda não configurado.");
            }

            identificadorRastreador.SonoRastreador.HoraInicio = dto.HoraInicio;
            identificadorRastreador.SonoRastreador.HoraFim = dto.HoraFim;

            await _context.SaveChangesAsync();

            return Ok($"Horário alterado. Novos horários, Horário de inicio: {dto.HoraInicio} , Horário de Fim: {dto.HoraFim}");
        }

        [HttpPatch("AlterarLocalizacaoModoSono")]
        public async Task<IActionResult> AlterarLocalizacaoModoSono(AlterarLocalizacaoModoSonoDTO dto)
        {
            var usuarioLogado = User.Claims.FirstOrDefault(c => c.Type == "Cpf")?.Value;
            var identificadorRastreador = await _context.Rastreadors.Where(r => r.Identificador == dto.IdentificadorRastreador && r.Veiculo.CpfProprietario == usuarioLogado).FirstOrDefaultAsync();

            if (identificadorRastreador == null)
            {
                return BadRequest("Rastreador não cadastrado.");
            }

            if (identificadorRastreador.SonoRastreador == null)
            {
                return BadRequest("Modo sono ainda não configurado.");
            }

            identificadorRastreador.SonoRastreador.Latitude = dto.Latitude;
            identificadorRastreador.SonoRastreador.Longitude = dto.Longitude;

            return Ok("Localização do Modo Sono alterada com sucesso.");
        }

        #endregion

        #region RASTREADOR

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

            var agora = DateTime.UtcNow;

            if (rastreador.PosicaoAtual == null)
            {
                rastreador.PosicaoAtual = new PosicaoVeiculo
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    DataHora = agora,
                    VelocidadeKmH = 0,
                    RastreadorModuloId = rastreador.RastreadorId
                };
            }
            else
            {
                var latitudeAnterior = rastreador.PosicaoAtual.Latitude;
                var longitudeAnterior = rastreador.PosicaoAtual.Longitude;
                var dataAnterior = rastreador.PosicaoAtual.DataHora;

                var distancia = Calculos.CalcularDistanciaEmMetros(latitudeAnterior, longitudeAnterior, latitude, longitude);

                var horas = (agora - dataAnterior).TotalHours;

                double velocidade = 0;

                if (horas > 0)
                {
                    velocidade = (distancia / 1000.0) / horas;
                }

                const double VELOCIDADE_MINIMA = 3.0;

                rastreador.PosicaoAtual.Latitude = latitude;
                rastreador.PosicaoAtual.Longitude = longitude;
                rastreador.PosicaoAtual.DataHora = agora;
                rastreador.PosicaoAtual.VelocidadeKmH = Math.Round(velocidade, 2);

                rastreador.PosicaoAtual.Estado = velocidade >= VELOCIDADE_MINIMA ? EstadoDoVeiculo.MOVIMENTANDO : EstadoDoVeiculo.PARADO;
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
                Longitude = longitude,
                Velocidade = rastreador.PosicaoAtual.VelocidadeKmH,
                Estado = rastreador.PosicaoAtual.Estado
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

            if (rastreador.SonoRastreador.Ativo == true)
            {
                return BadRequest("Modo sono já está ativado.");
            }

            var agora = TimeOnly.FromDateTime(DateTime.Parse("22:09:00"));

            var inicio = rastreador.SonoRastreador.HoraInicio;
            var fim = rastreador.SonoRastreador.HoraFim;

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

        [HttpPost("DesativarModoSono")]
        public async Task<IActionResult> DesativarModoSono(Guid identificadorRastreador, double latitude, double longitude)
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

            if (rastreador.SonoRastreador.Ativo == true)
            {
                return BadRequest("Modo sono já está desativado.");
            }

            var agora = TimeOnly.FromDateTime(DateTime.Parse("05:05:00"));

            var inicio = rastreador.SonoRastreador.HoraInicio;
            var fim = rastreador.SonoRastreador.HoraFim;

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


            rastreador.SonoRastreador.Ativo = false;

            await _context.SaveChangesAsync();

            return Ok("Modo sono desativado com sucesso.");
        }

        #endregion
    }
}