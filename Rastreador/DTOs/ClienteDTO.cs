namespace Rastreador.DTOs
{
    public class CadastrarClienteDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public string DataNascimento { get; set; }
    }

    public class ClienteDTO
    {
        public int ClienteId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
