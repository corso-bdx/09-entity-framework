using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace BDX.Model.DTO
{
    public class Ruolo
    {
        public string Nome { get; set; } = null!;

        public string Descrizione { get; set; } = null!;

        public string Categoria { get; set; } = null!;
    }

    public class CreaRuolo : RuoloPrimaryKey
    {
        [Required]
        public string Descrizione { get; set; } = null!;

        [Required]
        public string Categoria { get; set; } = null!;
    }

    public class RicercaRuolo
    {
        public string? Nome { get; set; }

        public string? Descrizione { get; set; }

        public string? Categoria { get; set; }
    }

    public class AggiornaRuolo : RuoloPrimaryKey
    {
        public string Descrizione { get; set; } = null!;

        public string Categoria { get; set; } = null!;
    }

    public class EliminaRuolo : RuoloPrimaryKey
    {

    }

    public abstract class RuoloPrimaryKey
    {
        [Required]
        public string Nome { get; set; } = null!;
    }
}
