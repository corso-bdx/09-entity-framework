using AutoMapper;
using Bdx.Model.Dto;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Bdx.Model.Repository;

public class RuoliRepository : IRuoliRepository, IDisposable
{
    private readonly UtentiDbContext _context;
    private readonly bool _disposeContext;
    private readonly IMapper _mapper;
    
    private bool _disposed;

    public RuoliRepository(UtentiDbContext context)
    {
        _context = context;

        _mapper = new Mapper(
            new MapperConfiguration(cfg => {
                cfg.CreateMap<Ruolo, Dto.Ruolo>();
                cfg.CreateMap<Dto.Ruolo, Ruolo>();

                cfg.CreateMap<CreaRuolo, Ruolo>();
                cfg.CreateMap<AggiornaRuolo, Ruolo>();
            }));
    }

    public RuoliRepository(IDbContextFactory<UtentiDbContext> factory, IMapper mapper)
    {
        _context = factory.CreateDbContext();
        _disposeContext = true;

        _mapper = mapper;
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }

    public int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        return _context.SaveChanges(acceptAllChangesOnSuccess);
    }

    public Dto.Ruolo Create(CreaRuolo creaRuolo)
    {
        if (TryReadFromPrimaryKey(creaRuolo, out Ruolo? Ruolo))
            throw new Exception($"Ruolo '{Ruolo.Nome}' già censito");

        Ruolo = _mapper.Map<Ruolo>(creaRuolo);

        _context.Add(Ruolo);

        SaveChanges();

        return _mapper.Map<Dto.Ruolo>(Ruolo!);
    }

    public List<Dto.Ruolo> Read(RicercaRuolo ricercaRuolo)
    {
        IQueryable<Ruolo> IQueryableRuolo = _context.ListaRuoli;

        if (ricercaRuolo.Nome is not null)
            IQueryableRuolo = IQueryableRuolo.Where(p => p.Nome.Contains(ricercaRuolo.Nome));

        if (ricercaRuolo.Descrizione is not null)
            IQueryableRuolo = IQueryableRuolo.Where(p => p.Descrizione.Contains(ricercaRuolo.Descrizione));

        if (ricercaRuolo.Categoria is not null)
            IQueryableRuolo = IQueryableRuolo.Where(p => p.Categoria.Contains(ricercaRuolo.Categoria));

        var list = IQueryableRuolo.ToList();

        return list.Select(p => _mapper.Map<Dto.Ruolo>(p)).ToList();
    }

    public Dto.Ruolo Update(AggiornaRuolo aggiornaRuolo)
    {
        Ruolo Ruolo = ReadFromPrimaryKey(aggiornaRuolo);

        _mapper.Map(aggiornaRuolo, Ruolo);

        SaveChanges();

        return _mapper.Map<Dto.Ruolo>(Ruolo);
    }

    public void Delete(EliminaRuolo eliminaRuolo)
    {
        _context.Remove(ReadFromPrimaryKey(eliminaRuolo));

        SaveChanges();
    }

    private bool TryReadFromPrimaryKey(RuoloPrimaryKey ruoloPrimaryKey, [NotNullWhen(true)] out Ruolo? ruolo)
    {
        ruolo = _context.ListaRuoli.SingleOrDefault(p => p.Nome == ruoloPrimaryKey.Nome);

        return ruolo != null;
    }

    private Ruolo ReadFromPrimaryKey(RuoloPrimaryKey ruoloPrimaryKey)
    {
        if (!TryReadFromPrimaryKey(ruoloPrimaryKey, out Ruolo? Ruolo))
            throw new Exception($"Ruolo '{ruoloPrimaryKey.Nome}' non trovato");

        return Ruolo!;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_disposeContext)
                    _context.Dispose();
            }

            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
