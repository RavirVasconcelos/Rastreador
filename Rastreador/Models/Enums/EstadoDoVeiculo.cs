using System.Text.Json.Serialization;

namespace Rastreador.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EstadoDoVeiculo
    {
        MOVIMENTANDO = 0,
        PARADO = 1
    }
}