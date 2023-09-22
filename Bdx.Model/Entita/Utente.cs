using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bdx.Model;

[Table("Utenti", Schema = "Bdx")]
public class Utente
{
    [Key]
    public string NomeUtente { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Cognome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Salt { get; set; } = string.Empty;

    public string NomeRuolo { get; set; } = string.Empty;

    public DateTime DataCreazione { get; set; }

    public DateTime DataUltimoCambioPassword { get; set; }

    public Ruolo Ruolo { get; set; } = null!;

    public List<Accesso> Accessi { get; set; } = new List<Accesso>();
}