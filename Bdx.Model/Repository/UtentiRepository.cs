using AutoMapper;
using Bdx.Model.Dto;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Bdx.Model.Repository;

public class UtentiRepository : IUtentiRepository, IDisposable
{
    private readonly UtentiDbContext _context;
    private readonly bool _disposeContext;
    private readonly IMapper _mapper;
    
    private bool _disposed;

    public UtentiRepository(UtentiDbContext context)
    {
        _context = context;

        _mapper = new Mapper(
            new MapperConfiguration(cfg => {
                cfg.CreateMap<Utente, Dto.Utente>();
                cfg.CreateMap<Dto.Utente, Utente>();

                cfg.CreateMap<CreaUtente, Utente>();
                cfg.CreateMap<AggiornaUtente, Utente>();
            }));
    }

    public UtentiRepository(IDbContextFactory<UtentiDbContext> factory, IMapper mapper)
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

    public Dto.Utente Create(CreaUtente creaUtente)
    {
        if (TryReadFromPrimaryKey(creaUtente, out Utente? utente))
            throw new Exception($"Utente '{utente.NomeUtente}' già censito");

        utente = _mapper.Map<Utente>(creaUtente);
        utente.DataCreazione = DateTime.Now;

        ImpostaPassword(ref utente, creaUtente.Password);

        _context.Add(utente);

        SaveChanges();

        return _mapper.Map<Dto.Utente>(utente!);
    }

    public List<Dto.Utente> Read(RicercaUtente ricercaUtente)
    {
        IQueryable<Utente> IQueryableUtente = _context.ListaUtenti;

        if (ricercaUtente.NomeUtente is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.NomeUtente == ricercaUtente.NomeUtente);

        if (ricercaUtente.Nome is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.Nome == ricercaUtente.Nome);

        if (ricercaUtente.Cognome is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.Cognome == ricercaUtente.Cognome);

        if (ricercaUtente.Email is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.Email == ricercaUtente.Email);

        if (ricercaUtente.NomeRuolo is not null)
            IQueryableUtente = IQueryableUtente.Where(p => p.NomeRuolo == ricercaUtente.NomeRuolo);

        var list = IQueryableUtente.ToList();

        return list.Select(p => _mapper.Map<Dto.Utente>(p)).ToList();
    }

    public Dto.Utente Update(AggiornaUtente aggiornaUtente)
    {
        Utente utente = ReadFromPrimaryKey(aggiornaUtente);

        _mapper.Map(aggiornaUtente, utente);

        SaveChanges();

        return _mapper.Map<Dto.Utente>(utente);
    }

    public void Delete(EliminaUtente eliminaUtente)
    {
        _context.Remove(ReadFromPrimaryKey(eliminaUtente));

        SaveChanges();
    }

    public void AggiornaPassword(AggiornaPassword aggiornaPassword)
    {
        Utente utente = ReadFromPrimaryKey(aggiornaPassword);

        ImpostaPassword(ref utente, aggiornaPassword.Password);
    }

    public bool VerificaPassword(VerificaPassword verificaPassword)
    {
        Utente utente = ReadFromPrimaryKey(verificaPassword);

        return utente.Password == GetHash(verificaPassword.Password, utente.Salt);
    }

    private Utente ImpostaPassword(ref Utente utente, string password)
    {
        utente.Password = GetHash(password, out string salt);
        utente.Salt = salt;
        utente.DataUltimoCambioPassword = DateTime.Now;

        return utente;
    }

    private bool TryReadFromPrimaryKey(UtentePrimaryKey utentePrimaryKey, [NotNullWhen(true)] out Utente? utente)
    {
        utente = _context.ListaUtenti.SingleOrDefault(p => p.NomeUtente == utentePrimaryKey.NomeUtente);

        return utente != null;
    }

    private Utente ReadFromPrimaryKey(UtentePrimaryKey utentePrimaryKey)
    {
        if (!TryReadFromPrimaryKey(utentePrimaryKey, out Utente? utente))
            throw new Exception($"Utente '{utentePrimaryKey.NomeUtente}' non trovato");

        return utente!;
    }

    private string GetHash(string password, out string salt)
    {
        salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128 / 8));

        return GetHash(password, salt);
    }

    private string GetHash(string password, string salt)
    {
        return Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 512 / 8));
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
