using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BDX.Model;

[Table("Ruoli", Schema = "Bdx")]
public class Ruolo
{
	[Key]
	public string Nome { get; set; } = string.Empty;

	public string Descrizione { get; set; } = string.Empty;

	public string Categoria { get; set; } = string.Empty;

	public List<Utente> ListaUtenti { get; set; } = new List<Utente>();
}