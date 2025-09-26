namespace WebAPI1.Dtos
{
    public class ResponseLivroDto
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int AnoPublicacao { get; set; }
        public string Categoria { get; set; }
        public decimal Preco { get; set; }
        public bool Novo { get; set; }
    }
}
