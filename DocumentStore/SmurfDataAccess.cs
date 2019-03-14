namespace DocumentStore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Marten;
    using Model;

    public class SmurfDataAccess
    {
        private readonly IDocumentStore store;

        public SmurfDataAccess(IDocumentStore store)
        {
            this.store = store;
        }

        public async Task<IReadOnlyList<Smurf>> GetAll()
        {
            using (var session = this.store.OpenSession())
            {
                var smurfs = await session.Query<Smurf>().ToListAsync();
                return smurfs;
            }
        }

        public async Task<IReadOnlyList<SmurfLeader>> GetLeaders(int? maxRank)
        {
            using (var session = this.store.OpenSession())
            {
                IQueryable<SmurfLeader> query = session.Query<SmurfLeader>();
                if (maxRank.HasValue)
                    query = query.Where(x => x.Rank <= maxRank);

                var smurfs = await query.ToListAsync();
                return smurfs;
            }
        }

        public async Task<Smurf> GetById(Guid id)
        {
            using (var session = this.store.OpenSession())
            {
                var smurf = await session.LoadAsync<Smurf>(id);
                return smurf;
            }
        }

        public async Task Save(Smurf smurf)
        {
            using (var session = this.store.OpenSession())
            {
                session.Store(smurf);
                await session.SaveChangesAsync();
            }
        }

        public void Delete(Smurf smurf)
        {
            using (var session = this.store.OpenSession())
            {
                session.Delete(smurf);
                session.SaveChanges();
            }
        }
    }
}