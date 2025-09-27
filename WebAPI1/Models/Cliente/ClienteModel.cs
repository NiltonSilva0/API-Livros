namespace WebAPI1.Models.Cliente
{
    public class ClienteModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 8);
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; } = DateTime.MinValue;
        public DateTime DataCadastro { get; set; } = DateTime.MinValue;
        public bool Ativo { get; set; } = true;
    }
}
