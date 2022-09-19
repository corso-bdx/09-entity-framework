using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDX.Model;

[Table("Utente", Schema = "BDX")]
public partial class Utente : IEntityTypeConfiguration<Utente>
{
    [Key]
    public string NomeUtente { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Cognome { get; set; } = string.Empty;

    public string EMail { get; set; } = string.Empty;

    public DateTime DataCreazione { get; set; }

    public DateTime DataUltimoCambioPassword { get; set; }

    public Ruolo Ruolo { get; set; } = new Ruolo();

    public List<Accesso> Accessi { get; set; } = new List<Accesso>();

    public void Configure(EntityTypeBuilder<Utente> modelBuilder)
    {
        modelBuilder.HasKey(t => new { t.NomeUtente });

        modelBuilder.HasOne(p => p.Ruolo).WithMany(p => p.Utenti).HasForeignKey(p => p.NomeUtente);
        modelBuilder.HasMany(p => p.Accessi).WithOne(p => p.Utente).HasForeignKey(p => p.NomeUtente);
    }
}