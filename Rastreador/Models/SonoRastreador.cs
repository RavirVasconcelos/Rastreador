namespace Rastreador.Models
{
    public class SonoRastreador
    {
        public int SonoId { get; set; }

        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }

        //Configurar no local onde o rastreador está dormindo, para que quando ele acordar, ele retorne para o local onde estava.
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RaioPerimetro { get; set; } = 100.00;
        public bool Ativo { get; set; }

        public int RastreadorId { get; set; }
        public RastreadorModulo? Rastreador { get; set; }
    }
}
