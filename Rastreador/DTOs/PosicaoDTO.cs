namespace Rastreador.DTOs
{
    public class PosicaoDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime DataHora { get; set; }
        public double Velocidade { get; set; }
        public bool Ancorado { get; set; }
    }
}