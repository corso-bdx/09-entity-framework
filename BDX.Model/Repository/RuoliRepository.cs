using AutoMapper;
using BDX.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace BDX.Model.Repository
{
    public class RuoliRepository : IRuoliRepository, IDisposable
    {
        private readonly UtentiDbContext context;
        private readonly IMapper mapper;

        public RuoliRepository(UtentiDbContext context)
        {
            this.context = context;
            this.mapper =
                new Mapper(
                    new MapperConfiguration(cfg => {
                        cfg.CreateMap<Ruolo, DTO.Ruolo>();
                        cfg.CreateMap<DTO.Ruolo, Ruolo>();

                        cfg.CreateMap<CreaRuolo, Ruolo>();
                        cfg.CreateMap<AggiornaRuolo, Ruolo>();
                    }));
        }

        public RuoliRepository(IDbContextFactory<UtentiDbContext> factory, IMapper mapper)
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

        public DTO.Ruolo Create(CreaRuolo creaRuolo)
        {
            if (TryReadFromPrimaryKey(creaRuolo, out Ruolo? Ruolo))
                throw new Exception($"Ruolo '{Ruolo!.Nome}' già censito");

            Ruolo = mapper.Map<Ruolo>(creaRuolo);

            context.Add(Ruolo);

            SaveChanges();

            return mapper.Map<DTO.Ruolo>(Ruolo!);
        }

        public List<DTO.Ruolo> Read(RicercaRuolo ricercaRuolo)
        {
            IQueryable<Ruolo> IQueryableRuolo = context.ListaRuoli;

            if (ricercaRuolo.Nome is not null)
                IQueryableRuolo = IQueryableRuolo.Where(p => p.Nome.Contains(ricercaRuolo.Nome));

            if (ricercaRuolo.Descrizione is not null)
                IQueryableRuolo = IQueryableRuolo.Where(p => p.Descrizione.Contains(ricercaRuolo.Descrizione));

            if (ricercaRuolo.Categoria is not null)
                IQueryableRuolo = IQueryableRuolo.Where(p => p.Categoria.Contains(ricercaRuolo.Categoria));

            var list = IQueryableRuolo.ToList();

            return list.Select(p => mapper.Map<DTO.Ruolo>(p)).ToList();
        }

        public DTO.Ruolo Update(AggiornaRuolo aggiornaRuolo)
        {
            Ruolo Ruolo = ReadFromPrimaryKey(aggiornaRuolo);

            mapper.Map(aggiornaRuolo, Ruolo);

            SaveChanges();

            return mapper.Map<DTO.Ruolo>(Ruolo);
        }

        public void Delete(EliminaRuolo eliminaRuolo)
        {
            context.Remove(ReadFromPrimaryKey(eliminaRuolo));

            SaveChanges();
        }

        private bool TryReadFromPrimaryKey(RuoloPrimaryKey RuoloPrimaryKey, out Ruolo? Ruolo)
        {
            Ruolo = context.ListaRuoli.SingleOrDefault(p => p.Nome == RuoloPrimaryKey.Nome);

            return Ruolo != null;
        }

        private Ruolo ReadFromPrimaryKey(RuoloPrimaryKey RuoloPrimaryKey)
        {
            if (!TryReadFromPrimaryKey(RuoloPrimaryKey, out Ruolo? Ruolo))
                throw new Exception($"Ruolo '{RuoloPrimaryKey.Nome}' non trovato");

            return Ruolo!;
        }
    }
}
