namespace Rastreador.Models
{
    public class Ancoragem
    {
        public int Id { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double RaioMetros { get; set; } = 300;

        public bool Ativa { get; set; }

        public TimeOnly? HoraInicio { get; set; }
        public TimeOnly? HoraFim { get; set; }


        public int VeiculoId { get; set; }
        public Veiculo? Veiculo { get; set; }
    }
}