namespace Rastreador.Models
{
    public class PosicaoVeiculo
    {
        public int PosicaoVeiculoId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DataHora { get; set; }

        public int RastreadorId { get; set; }
        public Rastreador? Rastreador { get; set; }

        public bool Ancorado { get; set; }
    }
}
