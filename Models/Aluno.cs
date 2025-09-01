namespace AcademiaMvc.Models
{
    public class Aluno
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public int Idade { get; set; }

        public string? Email { get; set; }

        public string Cpf { get; set; } = string.Empty; // 🔹 Novo campo

        // 🔹 FK para Equipamento
        public int? EquipamentoId { get; set; }
        public Equipamento? Equipamento { get; set; }

        // 🔹 FK para Categoria
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
    }
}
