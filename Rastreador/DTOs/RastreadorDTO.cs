namespace Rastreador.DTOs
{
    public class CadastrarRastreadorDTO
    {
        public string Imei { get; set; }
        public string Chip { get; set; }
        public string Placa { get; set; }
    }

    public class RastreadorDTO
    {
        public int RastreadorId { get; set; }
        public Guid Identificador { get; set; }
        public string Imei { get; set; }
        public string NumeroChip { get; set; }

        public bool Ativo { get; set; }
        public bool ModoEconomia { get; set; }

        public DateTime DataCadastro { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }

        public VeiculoDTO Veiculo { get; set; }

        public PosicaoDTO PosicaoAtual { get; set; }
    }

    public record RastreadorModoEconomiaResponse
    {
        public int RastreadorId { get; set; }
        public string Placa { get; set; }
        public string NomeCliente { get; set; }
        public bool ModoEconomia { get; set; }
        public int ConsumoReal { get; set; }
    }
}