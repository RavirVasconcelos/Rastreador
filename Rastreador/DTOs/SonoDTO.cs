namespace Rastreador.DTOs
{

    public record DefinirModoSonoDTO
    {
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFim { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Placa { get; set; }
        public Guid IdentificadorRastreador { get; set; }
    }
}
