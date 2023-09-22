using Bdx.Model;
using Bdx.Model.Repository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

Console.WriteLine("Hello, World!");

// la connection string indica come collegarsi al database, questo è Microsoft SQL Server Express
string connectionstring = "Server=localhost\\SQLEXPRESS;Database=BDX;Trusted_Connection=True";

// questo è un server Microsoft SQL Server installato in un container Docker
// docker run --rm -it --name mssql -e ACCEPT_EULA=Y -e MSSQL_PID=Developer -e MSSQL_SA_PASSWORD=p4ssw0rD -p 2433:1433 mcr.microsoft.com/mssql/server:2019-latest
//string connectionstring = "Server=localhost,2433;Database=BDX;User Id=sa;Password=p4ssw0rD;Encrypt=False";

DbContextOptionsBuilder<UtentiDbContext> optionsBuilder = new DbContextOptionsBuilder<UtentiDbContext>();
optionsBuilder.UseSqlServer(connectionstring);

using UtentiDbContext ctx1 = new UtentiDbContext(optionsBuilder.Options);
using RuoliRepository ruoliRepository = new RuoliRepository(ctx1);

Bdx.Model.Dto.CreaRuolo rr = new Bdx.Model.Dto.CreaRuolo
{
    Nome = "utente",
    Descrizione = "ruolo utente",
    Categoria = "non definita"
};

ICollection<ValidationResult>? results = null;

if (Validator.TryValidateObject(rr, new ValidationContext(rr), results, true))
{
    ruoliRepository.Create(rr);

    var ruoli = ruoliRepository.Read(new Bdx.Model.Dto.RicercaRuolo() { Nome = "uten" });

    Console.WriteLine(ruoli);
}

using UtentiDbContext ctx2 = new UtentiDbContext(optionsBuilder.Options);
using UtentiRepository utentiRepository = new UtentiRepository(ctx2);

Bdx.Model.Dto.CreaUtente cc = new Bdx.Model.Dto.CreaUtente
{
    NomeUtente = "NanuT",
    Nome = "Tommaso",
    Cognome = "Nanu",
    Email = "tommaso.nanu@alad.cloud",
    Password = "Tommaso",
    ConfermaPassword = "Tommaso",
    NomeRuolo = "utente"
};

if (Validator.TryValidateObject(cc, new ValidationContext(cc), results, true))
{
    utentiRepository.Create(cc);

    List<Bdx.Model.Dto.Utente> utenti = utentiRepository.Read(new Bdx.Model.Dto.RicercaUtente() { NomeUtente = "NanuT" });

    Console.WriteLine(utenti);
}

//List<Bdx.Model.Dto.Utente> utenti = utentiRepository.Read(new Bdx.Model.Dto.RicercaUtente() { NomeUtente = "NanuT" });

Console.WriteLine(utentiRepository.VerificaPassword(new Bdx.Model.Dto.VerificaPassword { NomeUtente = "NanuT", Password = "Tommaso2" }));
