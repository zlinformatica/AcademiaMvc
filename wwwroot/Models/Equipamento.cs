namespace AcademiaMvc.Models
{
    public class Equipamento
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}
