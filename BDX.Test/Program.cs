// See https://aka.ms/new-console-template for more information
using BDX.Model;
using BDX.Model.Repository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

Console.WriteLine("Hello, World!");

var connectionstring = "Server=localhost\\SQLEXPRESS;Database=BDX;Trusted_Connection=True";

var optionsBuilder = new DbContextOptionsBuilder<UtentiDbContext>();
optionsBuilder.UseSqlServer(connectionstring);

using (RuoliRepository ruoliRepository = new RuoliRepository(new UtentiDbContext(optionsBuilder.Options)))
using (UtentiRepository utentiRepository = new UtentiRepository(new UtentiDbContext(optionsBuilder.Options)))
{
    //var rr = new BDX.Model.DTO.CreaRuolo
    //{
    //    Nome = "utente",
    //    Descrizione = "ruolo utente",
    //    Categoria = "non definita"
    //};

    //ICollection<ValidationResult>? results = null;

    //if (Validator.TryValidateObject(rr, new ValidationContext(rr), results, true))
    //{
    //    ruoliRepository.Create(rr);

    //    var ruoli = ruoliRepository.Read(new BDX.Model.DTO.RicercaRuolo() { Nome = "uten" });

    //    Console.WriteLine(ruoli);
    //}

    //var cc = new BDX.Model.DTO.CreaUtente
    //{
    //    NomeUtente = "NanuT",
    //    Nome = "Tommaso",
    //    Cognome = "Nanu",
    //    Email = "tommaso.nanu@alad.cloud",
    //    Password = "Tommaso",
    //    ConfermaPassword = "Tommaso",
    //    NomeRuolo = "utente"
    //};

    //if (Validator.TryValidateObject(cc, new ValidationContext(cc), results, true))
    //{
    //    utentiRepository.Create(cc);

    //    var utenti = utentiRepository.Read(new BDX.Model.DTO.RicercaUtente() { NomeUtente = "NanuT" });

    //    Console.WriteLine(utenti);
    //}

    //var utenti = utentiRepository.Read(new BDX.Model.DTO.RicercaUtente() { NomeUtente = "NanuT" });

    Console.WriteLine(utentiRepository.VerificaPassword(new BDX.Model.DTO.VerificaPassword { NomeUtente = "NanuT", Password = "Tommaso2" }));
}




