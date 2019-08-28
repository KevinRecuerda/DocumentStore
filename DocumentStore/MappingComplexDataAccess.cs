namespace DocumentStore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DocumentStore.Extensions;
    using Marten;
    using Model;

    public class MappingComplexDataAccess
    {
        private readonly IDocumentStore store;

        public MappingComplexDataAccess(IDocumentStore store)
        {
            this.store = store;
        }

        public async Task<IReadOnlyList<MappingComplex>> GetAll()
        {
            using (var session = this.store.OpenSession())
            {
                var mappings = await session.Query<MappingComplex>().ToListAsync();
                return mappings;
            }
        }

        public async Task<MappingComplex> GetById(string id)
        {
            using (var session = this.store.OpenSession())
            {
                var mapping = await session.LoadAsync<MappingComplex>(id);
                return mapping;
            }
        }

        public async Task<MappingComplex> GetByFrenchId(string id)
        {
            using (var session = this.store.OpenSession())
            {
                var mapping = await session.Query<MappingComplex>()
                                           .Where(x => x.FrenchIds.Contains(id))
                                           .FirstOrDefaultAsync();
                return mapping;
            }
        }

        public async Task<IReadOnlyList<MappingComplex>> GetByFrenchIds(string[] ids)
        {
            using (var session = this.store.OpenSession())
            {
                var mappings = await session.Query<MappingComplex>()
                                            .Where(x => x.FrenchIds.ContainsAny(ids))
                                            .ToListAsync();
                return mappings;
            }
        }

        public void Insert(params MappingComplex[] mappings)
        {
            this.store.BulkInsert(mappings, BulkInsertMode.OverwriteExisting);
        }

        public async Task DeleteAll()
        {
            using (var session = this.store.OpenSession())
            {
                session.DeleteWhere<MappingComplex>(x => true);
                await session.SaveChangesAsync();
            }
        }
    }
}