using Microsoft.EntityFrameworkCore;

namespace Bdx.Model;

public class UtentiDbContext : DbContext
{
    public UtentiDbContext(DbContextOptions<UtentiDbContext> options) : base(options) { }

    public DbSet<Ruolo> ListaRuoli { get; set; } = null!;

    public DbSet<Utente> ListaUtenti { get; set; } = null!;

    public DbSet<Accesso> ListaAccessi { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Utente>().HasKey(t => new { t.NomeUtente });
        modelBuilder.Entity<Utente>().HasOne(p => p.Ruolo).WithMany(p => p.ListaUtenti).HasForeignKey(p => p.NomeRuolo);
        modelBuilder.Entity<Utente>().HasMany(p => p.Accessi).WithOne(p => p.Utente);//.HasForeignKey(p => p.NomeUtente);

        modelBuilder.Entity<Ruolo>().HasKey(t => new { t.Nome });
        modelBuilder.Entity<Ruolo>().HasMany(p => p.ListaUtenti).WithOne(p => p.Ruolo);//.HasForeignKey(p => p.NomeUtente);

        modelBuilder.Entity<Accesso>().HasKey(t => new { t.ID });
        modelBuilder.Entity<Accesso>().HasOne(p => p.Utente).WithMany(p => p.Accessi).HasForeignKey(p => p.NomeUtente);

        base.OnModelCreating(modelBuilder);
    }
}
