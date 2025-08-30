using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AcademiaMvc.Models
{
    public class AcademiaDbContext : DbContext
    {
        public AcademiaDbContext(DbContextOptions<AcademiaDbContext> options) : base(options) { }

        public DbSet<Aluno> Alunos { get; set; } = default!;
        public DbSet<Categoria> Categorias { get; set; } = default!;
        public DbSet<Professor> Professores { get; set; } = default!;
        public DbSet<Equipamento> Equipamentos { get; set; } = default!;
    }
}
