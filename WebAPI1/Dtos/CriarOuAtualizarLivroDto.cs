namespace WebAPI1.Dtos
{
    public class CriarOuAtualizarLivroDto
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int AnoPublicacao { get; set; }
        public string Genero { get; set; }
        public decimal Preco { get; set; }
        public bool Novo { get; set; }
    }
}
