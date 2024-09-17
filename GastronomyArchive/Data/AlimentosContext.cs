using Microsoft.EntityFrameworkCore;
using AlimentosAPI.Models;

namespace AlimentosAPI.Data
{
    public class AlimentosContext : DbContext
    {
        public AlimentosContext(DbContextOptions<AlimentosContext> options) : base(options)
        {
        }

        public DbSet<Alimento> Alimentos { get; set; }
        public DbSet<Receta> Recetas { get; set; }
        public DbSet<RecetaAlimento> RecetaAlimentos { get; set; }

    }
}
