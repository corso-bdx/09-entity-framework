using Microsoft.EntityFrameworkCore;

namespace BDX.Model
{
    public class UtentiDbContext : DbContext
    {
        public DbSet<Utente> Utenti { get; set; } = null!;
        public DbSet<Ruolo> Ruoli { get; set; } = null!;
    }
}
