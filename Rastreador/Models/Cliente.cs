using System.Collections.ObjectModel;

namespace Rastreador.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }

        public ICollection<Veiculo> Veiculos { get; set; } = new Collection<Veiculo>();
    }
}