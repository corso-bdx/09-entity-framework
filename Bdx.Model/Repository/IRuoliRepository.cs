using Bdx.Model.Dto;

namespace Bdx.Model.Repository;

internal interface IRuoliRepository
{
    Dto.Ruolo Create(CreaRuolo creaRuolo);

    List<Dto.Ruolo> Read(RicercaRuolo ricercaRuolo);

    Dto.Ruolo Update(AggiornaRuolo aggiornaRuolo);

    void Delete(EliminaRuolo eliminaRuolo);
}
