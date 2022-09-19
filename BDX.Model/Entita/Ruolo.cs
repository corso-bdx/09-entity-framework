using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDX.Model;

[Table("Ruolo", Schema = "BDX")]
public partial class Ruolo : IEntityTypeConfiguration<Ruolo>
{
	[Key]
	public string Nome { get; set; } = string.Empty;

	public string Descrizione { get; set; } = string.Empty;

	public string Categoria { get; set; } = string.Empty;

	public List<Utente> Utenti { get; set; } = new List<Utente>();

	public void Configure(EntityTypeBuilder<Ruolo> modelBuilder)
	{
		modelBuilder.HasKey(t => new { t.Nome });

		modelBuilder.HasMany(p => p.Utenti).WithOne(p => p.Ruolo).HasForeignKey(p => p.NomeUtente);
	}
}