using System.Text.Json.Serialization;

namespace WebAPI1.Models.Livro
{
    public class LivroModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString().Substring(0, 8);
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int AnoPublicacao { get; set; } = 0;
        public string Categoria { get; set; } = string.Empty;
        public bool Disponivel { get; set; } = true;
        public bool Novo { get; set; } = true;
        public DateTime EmprestadoEm { get; set; } = DateTime.MinValue;
        public DateTime DevolvidoEm { get; set; } = DateTime.MinValue;
        public DateTime VendidoEm { get; set; } = DateTime.MinValue;
        public decimal Preco { get; set; } = 0.0M;
        public bool Inativo { get; set; } = false;
        public bool Promocao { get; set; } = false;
        public bool Vendido { get; set; } = false;
    }
}
