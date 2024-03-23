using Microsoft.EntityFrameworkCore;
using Univesp.CaminhoDoMar.ProjetoIntegrador.ApplicationCore.Business;

namespace Univesp.CaminhoDoMar.ProjetoIntegradorInfrastructure.Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tabela> Tabelas { get; set; }
        public DbSet<Lista> Listas { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        
    }
}
