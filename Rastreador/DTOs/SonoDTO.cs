namespace Rastreador.DTOs
{

    public record DefinirModoSonoDTO
    {
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Placa { get; set; }
        public Guid IdentificadorRastreador { get; set; }
    }

    public record AlterarHorarioModoSonoDTO
    {
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFim { get; set; }
        public Guid IdentificadorRastreador { get; set; }
    }

    public record AlterarLocalizacaoModoSonoDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid IdentificadorRastreador { get; set; }
    }
}