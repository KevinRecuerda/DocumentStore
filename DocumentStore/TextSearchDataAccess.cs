namespace DocumentStore
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DocumentStore.Extensions;
    using Marten;
    using Model;

    public class TextSearchDataAccess
    {
        private readonly IDocumentStore store;

        public TextSearchDataAccess(IDocumentStore store)
        {
            this.store = store;
        }

        public async Task<IReadOnlyList<TextSearch>> GetAll()
        {
            using (var session = this.store.OpenSession())
            {
                var textSearches = await session.Query<TextSearch>().ToListAsync();
                return textSearches;
            }
        }

        public async Task<TextSearch> GetById(long id)
        {
            using (var session = this.store.OpenSession())
            {
                var textSearch = await session.LoadAsync<TextSearch>(id);
                return textSearch;
            }
        }

        public async Task<IReadOnlyList<TextSearch>> SearchByText(string search, int top = 10)
        {
            using (var session = this.store.OpenSession())
            {
                var textSearches = await session.SearchSimilarAsync<TextSearch>(x => x.Text, search, top);
                return textSearches;
            }
        }

        public async Task Save(TextSearch textSearch)
        {
            using (var session = this.store.OpenSession())
            {
                session.Store(textSearch);
                await session.SaveChangesAsync();
            }
        }

        public void Insert(params TextSearch[] textSearches)
        {
            this.store.BulkInsert(textSearches, BulkInsertMode.OverwriteExisting);
        }

        public async Task Delete(TextSearch textSearch)
        {
            using (var session = this.store.OpenSession())
            {
                session.Delete(textSearch);
                await session.SaveChangesAsync();
            }
        }
    }
}