namespace DocumentStore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Marten;

    public class IssueDataAccess
    {
        private readonly IDocumentStore store;

        public IssueDataAccess(IDocumentStore store)
        {
            this.store = store;
        }

        public async Task<IReadOnlyList<Issue>> GetAll()
        {
            using (var session = this.store.OpenSession())
            {
                var issues = await session.Query<Issue>().ToListAsync();
                return issues;
            }
        }

        public async Task<IReadOnlyList<Issue>> GetByTags(params string[] tags)
        {
            using (var session = this.store.OpenSession())
            {
                var issues = await session.Query<Issue>()
                                          .Where(x => x.Tags.Any(tags.Contains))
                                          .ToListAsync();
                return issues;
            }
        }

        public async Task<Issue> GetById(long id)
        {
            using (var session = this.store.OpenSession())
            {
                var issue = await session.LoadAsync<Issue>(id);
                return issue;
            }
        }

        public async Task Save(Issue issue)
        {
            using (var session = this.store.OpenSession())
            {
                session.Store(issue);
                await session.SaveChangesAsync();
            }
        }

        public void Delete(Issue issue)
        {
            using (var session = this.store.OpenSession())
            {
                session.Delete(issue);
                session.SaveChanges();
            }
        }
    }

    public class Issue
    {
        public long   Id   { get; set; }
        public string Name { get; set; }

        public List<string> Tags { get; set; }

        public long? AssigneeId { get; set; }
        public long? ReporterId   { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}