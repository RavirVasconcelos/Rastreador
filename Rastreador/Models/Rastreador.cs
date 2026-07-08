namespace Rastreador.Models
{
    public class Rastreador
    {
        public int RastreadorId { get; set; }
        public string Identificador { get; set; }
        public string Imei { get; set; }
        public string NumeroChip { get; set; }

        public bool Ativo { get; set; }
        public bool ModoEconomia { get; set; }
        public DateTime? UltimaAtualizacao { get; set; }

        public int VeiculoId { get; set; }
        public Veiculo? Veiculo { get; set; }

        public List<PosicaoVeiculo> Posicoes { get; set; } = new List<PosicaoVeiculo>();
    }
}
