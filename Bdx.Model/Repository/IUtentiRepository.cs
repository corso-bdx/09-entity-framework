using Bdx.Model.Dto;

namespace Bdx.Model.Repository;

internal interface IUtentiRepository
{
    Dto.Utente Create(CreaUtente creaUtente);

    List<Dto.Utente> Read(RicercaUtente ricercaUtente);

    Dto.Utente Update(AggiornaUtente aggiornaUtente);

    void Delete(EliminaUtente eliminaUtente);

    void AggiornaPassword(AggiornaPassword aggiornaPassword);

    bool VerificaPassword(VerificaPassword verificaPassword);
}
