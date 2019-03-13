namespace DocumentStore
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Marten;

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

    public interface ISmurf
    {
        Guid   Id      { get; set; }
        string Ability { get; set; }
    }

    public class Smurf : ISmurf
    {
        public Guid   Id      { get; set; }
        public string Ability { get; set; }
    }

    public class SmurfLeader : Smurf
    {
        public int Rank { get; set; }
    }

    public class PapaSmurf : SmurfLeader
    {
        public int Age { get; set; }
    }

    public class Smurfette : Smurf
    {
        public int Suitors { get; set; }
    }

    public class HeftySmurf : SmurfLeader
    {
        public double Weight { get; set; }
    }

    public class BrainySmurf : Smurf
    {
        public int QI { get; set; }
    }
}