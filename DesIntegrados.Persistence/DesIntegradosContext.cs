using DesIntegrados.Models;
using DesIntegrados.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DesIntegrados.Persistence
{
    public class DesIntegradosContext : DbContext
    {
        public DesIntegradosContext(DbContextOptions options) : base(options)
        {

        }

        protected DesIntegradosContext()
        { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Imagem> Imagens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(UsuarioConfiguration)));
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ImagemConfiguration)));
            base.OnModelCreating(modelBuilder);
        }
    }
}
