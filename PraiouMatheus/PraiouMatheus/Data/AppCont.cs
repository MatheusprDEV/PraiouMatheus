using Microsoft.EntityFrameworkCore;
using PraiouMatheus.Models;

namespace PraiouMatheus.Data
{
    public class AppCont : DbContext
    {
        public AppCont(DbContextOptions<AppCont> options)
            : base(options)
        { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }

    }
}
