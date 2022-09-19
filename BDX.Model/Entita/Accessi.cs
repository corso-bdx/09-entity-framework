using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDX.Model;

[Table("Accesso", Schema = "BDX")]
public partial class Accesso : IEntityTypeConfiguration<Accesso>
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string ID { get; set; } = string.Empty;

    public string NomeUtente { get; set; } = string.Empty;

    public DateTime Data { get; set; }

    public Utente Utente { get; set; } = new Utente();

    public void Configure(EntityTypeBuilder<Accesso> modelBuilder)
    {
        modelBuilder.HasKey(t => new { t.ID });

        modelBuilder.HasOne(p => p.Utente).WithMany(p => p.Accessi).HasForeignKey(p => p.NomeUtente);
    }
}