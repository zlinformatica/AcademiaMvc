using AcademiaMvc.Models;
using System.Linq;

namespace AcademiaMvc.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AcademiaDbContext context)
        {
            // Garante que o banco de dados est� criado
            context.Database.EnsureCreated();

            // --- CATEGORIAS ---
            if (!context.Categorias.Any())
            {
                var categorias = new Categoria[]
                {
                    new Categoria { Nome = "Cardio" },
                    new Categoria { Nome = "Muscula��o" }
                };

                foreach (var c in categorias)
                    context.Categorias.Add(c);

                context.SaveChanges(); // Salva as categorias primeiro
            }

            // Recupera as categorias existentes
            var categoriasExistentes = context.Categorias.ToList();

            // --- EQUIPAMENTOS ---
            if (!context.Equipamentos.Any())
            {
                // Busca categoria com seguran�a usando FirstOrDefault
                var cardio = categoriasExistentes.FirstOrDefault(c => c.Nome == "Cardio");
                var musculacao = categoriasExistentes.FirstOrDefault(c => c.Nome == "Muscula��o");

                // Se alguma categoria n�o existir, cria-a
                if (cardio == null)
                {
                    cardio = new Categoria { Nome = "Cardio" };
                    context.Categorias.Add(cardio);
                }
                if (musculacao == null)
                {
                    musculacao = new Categoria { Nome = "Muscula��o" };
                    context.Categorias.Add(musculacao);
                }

                context.SaveChanges(); // Salva novas categorias

                // Agora cria os equipamentos usando os Ids v�lidos
                var equipamentos = new Equipamento[]
                {
                    new Equipamento { Nome = "Esteira", CategoriaId = cardio.Id },
                    new Equipamento { Nome = "Bicicleta", CategoriaId = cardio.Id },
                    new Equipamento { Nome = "Supino", CategoriaId = musculacao.Id }
                };

                foreach (var e in equipamentos)
                    context.Equipamentos.Add(e);

                context.SaveChanges();
            }
        }
    }
}
