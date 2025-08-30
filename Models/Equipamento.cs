namespace AcademiaMvc.Models
{
    public class Equipamento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        // Adicionando FK e navega��o
        public int CategoriaId { get; set; }  // FK para Categoria
    //    public Categoria Categoria { get; set; }  // Navega��o opcional
        public string Marca { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}
