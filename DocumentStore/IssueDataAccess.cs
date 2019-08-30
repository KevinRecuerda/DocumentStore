namespace DocumentStore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DocumentStore.Extensions;
    using Marten;
    using Marten.Linq.MatchesSql;
    using Marten.Services.Includes;
    using Model;

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

                                           // ReSharper disable once ConvertClosureToMethodGroup
                                           // Lambda is required
                                          .Where(x => x.Tags.Any(tag => tags.Contains(tag)))
                                          .ToListAsync();
                return issues;
            }
        }

        public async Task<(IReadOnlyList<Issue>, Dictionary<long, User>)> GetByTagsIncludingAssignee(params string[] tags)
        {
            using (var session = this.store.OpenSession())
            {
                var users = new Dictionary<long, User>();

                var issues = await session.Query<Issue>()

                                           // LeftOuter does not work: https://github.com/JasperFx/marten/issues/1222
                                          .Include(i => i.AssigneeId, users, JoinType.LeftOuter)

                                           // ReSharper disable once ConvertClosureToMethodGroup
                                           // Lambda is required
                                           //.Where(x => x.Tags.Any(tag => tags.Contains(tag))) // does not work: https://github.com/JasperFx/marten/issues/1220
                                          .Where(
                                               x => x.MatchesSql(
                                                   new CollectionWhereFragment("d.data->'Tags' ?| ??", new object[] {tags}))) // workaround
                                          .ToListAsync();

                return (issues, users);
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

        public async Task<(Issue, User)> GetByIdIncludingAssignee(long id)
        {
            using (var session = this.store.OpenSession())
            {
                User assignee = null;
                var issue = await session.Query<Issue>()
                                         .Include<User>(i => i.AssigneeId, u => assignee = u, JoinType.LeftOuter)
                                         .Where(i => i.Id == id)
                                         .SingleOrDefaultAsync();
                return (issue, assignee);
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
}