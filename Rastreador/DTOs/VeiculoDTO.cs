using Rastreador.Models.Enums;

namespace Rastreador.DTOs
{
    public class CadastrarVeiculoDTO
    {
        public string Placa { get; set; }
        public string MarcaModelo { get; set; }
        public TipoVeiculo? TipoVeiculo { get; set; }
        public int? AnoVeiculo { get; set; }
    }
}
