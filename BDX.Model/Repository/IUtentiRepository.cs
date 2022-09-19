using BDX.Model.DTO;

namespace BDX.Model.Repository
{
    internal interface IUtentiRepository
    {
        DTO.Utente Create(CreaUtente creaUtente);

        List<DTO.Utente> Read(RicercaUtente ricercaUtente);

        DTO.Utente Update(AggiornaUtente aggiornaUtente);

        void Delete(EliminaUtente eliminaUtente);

        void AggiornaPassword(AggiornaPassword aggiornaPassword);

        bool VerificaPassword(VerificaPassword verificaPassword);
    }
}
