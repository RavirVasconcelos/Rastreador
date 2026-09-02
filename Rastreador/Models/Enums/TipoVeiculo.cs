using System.Text.Json.Serialization;

namespace Rastreador.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoVeiculo
    {
        Carro,
        Moto,
        Caminhonete
    }
}