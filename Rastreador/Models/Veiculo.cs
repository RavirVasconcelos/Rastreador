using Rastreador.Models.Enums;
using System.Collections.ObjectModel;

namespace Rastreador.Models
{
    public class Veiculo
    {
        public int VeiculoId { get; set; }
        public string? Placa { get; set; }
        public string? MarcaModelo { get; set; }
        public TipoVeiculo? TipoVeiculo { get; set; }
        public int? AnoVeiculo { get; set; }
        public string CpfProprietario { get; set; }
        public DateTime DataCadastro { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public RastreadorModulo? Rastreador { get; set; }
        public Ancoragem? Ancoragem { get; set; }
    }
}
