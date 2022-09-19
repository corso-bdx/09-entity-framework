using AutoMapper;
using BDX.Model.DTO;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace BDX.Model.Repository
{
    public class UtentiRepository : IUtentiRepository, IDisposable
    {
        private readonly UtentiDbContext context;
        private readonly IMapper mapper;

        public UtentiRepository(IDbContextFactory<UtentiDbContext> factory, IMapper mapper)
        {
            this.context = factory.CreateDbContext();
            this.mapper = mapper;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public int SaveChanges()
        {
            return context.SaveChanges();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return context.SaveChanges(acceptAllChangesOnSuccess);
        }

        public DTO.Utente Create(CreaUtente creaUtente)
        {
            if (TryReadFromPrimaryKey(creaUtente, out Utente? utente))
                throw new Exception($"Utente '{utente!.NomeUtente}' già censito");

            utente = mapper.Map<Utente>(creaUtente);
            utente.DataCreazione = DateTime.Now;

            ImpostaPassword(ref utente, creaUtente.Password);

            context.Add(utente);

            SaveChanges();

            return mapper.Map<DTO.Utente>(utente);
        }

        public List<DTO.Utente> Read(RicercaUtente ricercaUtente)
        {
            IQueryable<Utente> IQueryableUtente = context.Utenti;

            if (ricercaUtente.NomeUtente is not null)
                IQueryableUtente = IQueryableUtente.Where(p => p.NomeUtente == ricercaUtente.NomeUtente);

            if (ricercaUtente.Nome is not null)
                IQueryableUtente = IQueryableUtente.Where(p => p.Nome == ricercaUtente.Nome);

            if (ricercaUtente.Cognome is not null)
                IQueryableUtente = IQueryableUtente.Where(p => p.Cognome == ricercaUtente.Cognome);

            if (ricercaUtente.EMail is not null)
                IQueryableUtente = IQueryableUtente.Where(p => p.EMail == ricercaUtente.EMail);

            return IQueryableUtente.ToList().Select(p => mapper.Map<DTO.Utente>(p)).ToList();
        }

        public DTO.Utente Update(AggiornaUtente aggiornaUtente)
        {
            Utente utente = ReadFromPrimaryKey(aggiornaUtente);

            mapper.Map(aggiornaUtente, utente);

            SaveChanges();

            return mapper.Map<DTO.Utente>(utente);
        }

        public void Delete(EliminaUtente eliminaUtente)
        {
            context.Remove(ReadFromPrimaryKey(eliminaUtente));

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

        private bool TryReadFromPrimaryKey(UtentePrimaryKey utentePrimaryKey, out Utente? utente)
        {
            utente = context.Utenti.SingleOrDefault(p => p.NomeUtente == utentePrimaryKey.NomeUtente);

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
    }
}
