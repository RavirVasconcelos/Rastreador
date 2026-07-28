namespace Rastreador.Models
{
    public class PosicaoVeiculo
    {
        public int PosicaoVeiculoId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DataHora { get; set; }

        public int RastreadorModuloId { get; set; }
        public RastreadorModulo? RastreadorModulo { get; set; }

        public bool Ancorado { get; set; }
    }
}
