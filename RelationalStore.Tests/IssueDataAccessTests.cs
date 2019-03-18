namespace RelationalStore.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using DeepEqual.Syntax;
    using Model;
    using Model.Tests;
    using Xunit;

    public class IssueDataAccessTests
    {
        private readonly IssueDataAccess issueDataAccess;

        public IssueDataAccessTests()
        {
            this.issueDataAccess = new IssueDataAccess(new ConnectionStrings());
        }

        public static Issue CreateIssue(string name, params string[] tags)
        {
            return new Issue
            {
                Name = name,
                Tags = tags.ToList()
            };
        }

        public async Task<Issue> CreateAndInsertIssue(string name, params string[] tags)
        {
            var issue = CreateIssue(name, tags);
            await this.issueDataAccess.Insert(issue);
            return issue;
        }

        [Fact]
        public async Task Should_C_R_U_D()
        {
            var issue1 = CreateIssue("issue without tags");
            var issue2 = CreateIssue("issue with tag",           "tag1");
            var issue3 = CreateIssue("issue with multiple tags", "tag1", "tag2");
            var issue4 = CreateIssue("issue with another tag",   "tag3");

            // Create
            await this.AssertInsert(issue1);
            await this.AssertInsert(issue2);
            await this.AssertInsert(issue3);
            await this.AssertInsert(issue4);

            // Read
            await this.AssertGetAll(issue1, issue2, issue3, issue4);
            await this.AssertGetByTags(new[] {"tag1"},         issue2, issue3);
            await this.AssertGetByTags(new[] {"tag1", "tag3"}, issue2, issue3, issue4);
            await this.AssertGetByTags(new[] {"tag unknown"});
            await this.AssertGetByTags(new string[] { });

            // Update
            issue3.Tags.Remove("tag1");
            await this.AssertUpdate(issue3);
            await this.AssertGetByTags(new[] {"tag1"}, issue2);

            // Delete
            await this.AssertDelete(issue1);
            await this.AssertDelete(issue2);
            await this.AssertDelete(issue3);
            await this.AssertDelete(issue4);
        }

        private async Task AssertGet(long id, Issue expected)
        {
            var actual = await this.issueDataAccess.GetById(id);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetAll(params Issue[] expected)
        {
            var actual = await this.issueDataAccess.GetAll();
            actual.OrderBy(x => x.Id).ShouldDeepEqual(expected.OrderBy(x => x.Id));

        }

        private async Task AssertGetByTags(string[] tags, params Issue[] expected)
        {
            var actual = await this.issueDataAccess.GetByTags(tags);
            actual.OrderBy(x => x.Id).ShouldDeepEqual(expected.OrderBy(x => x.Id));
        }

        private async Task AssertInsert(Issue issue)
        {
            await this.issueDataAccess.Insert(issue);
            await this.AssertGet(issue.Id, issue);
        }

        private async Task AssertUpdate(Issue issue)
        {
            await this.issueDataAccess.Update(issue);
            await this.AssertGet(issue.Id, issue);
        }

        private async Task AssertDelete(Issue issue)
        {
            await this.issueDataAccess.Delete(issue);
            await this.AssertGet(issue.Id, null);
        }
    }
}