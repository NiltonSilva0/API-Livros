using System.Text.Json.Serialization;

namespace WebAPI1.Dtos.Cliente
{
    public class ClienteResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DataNascimento { get; set; }
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DataCadastro { get; set; } = DateTime.Now.Date;
        public bool Ativo { get; set; }
    }
}
