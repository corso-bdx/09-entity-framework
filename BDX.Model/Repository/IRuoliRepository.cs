using BDX.Model.DTO;

namespace BDX.Model.Repository
{
    internal interface IRuoliRepository
    {
        DTO.Ruolo Create(CreaRuolo creaRuolo);

        List<DTO.Ruolo> Read(RicercaRuolo ricercaRuolo);

        DTO.Ruolo Update(AggiornaRuolo aggiornaRuolo);

        void Delete(EliminaRuolo eliminaRuolo);
    }
}
