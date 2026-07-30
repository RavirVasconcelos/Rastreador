using Rastreador.Models.Enums;

namespace Rastreador.Models
{
    public class PosicaoVeiculo
    {
        public int PosicaoVeiculoId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public double VelocidadeKmH { get; set; }
        public double? DirecaoGraus { get; set; }

        public EstadoDoVeiculo Estado { get; set; }

        public DateTime DataHora { get; set; }

        public int RastreadorModuloId { get; set; }
        public RastreadorModulo? RastreadorModulo { get; set; }
    }
}
