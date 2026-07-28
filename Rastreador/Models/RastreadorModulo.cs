namespace Rastreador.Models
{
    public class RastreadorModulo
    {
        public int RastreadorId { get; set; }

        public Guid Identificador { get; set; }
        public string Imei { get; set; }
        public string NumeroChip { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }
        public bool ModoEconomia { get; set; }

        public DateTime? UltimaAtualizacao { get; set; }

        public int VeiculoId { get; set; }
        public Veiculo? Veiculo { get; set; }

        public PosicaoVeiculo? PosicaoAtual { get; set; }
    }
}
