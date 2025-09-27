using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI1.Dtos.Cliente
{
    public class CriarOuAtualizaClienteDto
    {
        [Required, MinLength(3), MaxLength(30)]
        public string Nome { get; set; } = string.Empty;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required, Phone]
        public string Telefone { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime DataNascimento { get; set; } = DateTime.MinValue;
        public bool Ativo { get; set; } = true;
        public string? Id { get; set; }
    }
}
