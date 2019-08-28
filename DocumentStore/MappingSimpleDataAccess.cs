namespace DocumentStore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Marten;
    using Model;

    public class MappingSimpleDataAccess
    {
        private readonly IDocumentStore store;

        public MappingSimpleDataAccess(IDocumentStore store)
        {
            this.store = store;
        }

        public async Task<IReadOnlyList<MappingSimple>> GetAll()
        {
            using (var session = this.store.OpenSession())
            {
                var mappings = await session.Query<MappingSimple>().ToListAsync();
                return mappings;
            }
        }

        public async Task<MappingSimple> GetById(string id)
        {
            using (var session = this.store.OpenSession())
            {
                var mapping = await session.LoadAsync<MappingSimple>(id);
                return mapping;
            }
        }

        public async Task<MappingSimple> GetByFrenchId(string id)
        {
            using (var session = this.store.OpenSession())
            {
                var mapping = await session.Query<MappingSimple>().Where(x => x.FrenchId == id).FirstOrDefaultAsync();
                return mapping;
            }
        }

        public void Insert(params MappingSimple[] mappings)
        {
            this.store.BulkInsert(mappings);
        }

        public async Task DeleteAll()
        {
            using (var session = this.store.OpenSession())
            {
                session.DeleteWhere<MappingSimple>(x => true);
                await session.SaveChangesAsync();
            }
        }
    }
}