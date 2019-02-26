namespace DocumentStore.Tests
{
    using System.Linq;
    using System.Threading.Tasks;
    using DeepEqual.Syntax;
    using Xunit;

    public class IssueDataAccessTests
    {
        private readonly IssueDataAccess issueDataAccess;

        public IssueDataAccessTests()
        {
            this.issueDataAccess = new IssueDataAccess(DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));
        }

        public static Issue CreateIssue(string name, params string[] tags)
        {
            return new Issue
            {
                Name = name,
                Tags = tags.ToList()
            };
        }

        [Fact]
        public async Task Should_C_R_U_D()
        {
            var issue1 = CreateIssue("issue without tags");
            var issue2 = CreateIssue("issue with tag", "tag1");
            var issue3 = CreateIssue("issue with multiple tags", "tag1", "tag2");
            var issue4 = CreateIssue("issue with another tag", "tag3");

            // Create
            await this.AssertSave(issue1);
            await this.AssertSave(issue2);
            await this.AssertSave(issue3);
            await this.AssertSave(issue4);

            // Read
            await this.AssertGetAll(issue1, issue2, issue3, issue4);
            await this.AssertGetByTags(new[] {"tag1"}, issue2, issue3);
            await this.AssertGetByTags(new[] {"tag1", "tag3"}, issue2, issue3, issue4);
            await this.AssertGetByTags(new string[] {});

            // Update
            issue3.Tags.Remove("tag1");
            await this.AssertSave(issue3);
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
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetByTags(string[] tags, params Issue[] expected)
        {
            var actual = await this.issueDataAccess.GetByTags(tags);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertSave(Issue issue)
        {
            await this.issueDataAccess.Save(issue);
            await this.AssertGet(issue.Id, issue);
        }

        private async Task AssertDelete(Issue issue)
        {
            this.issueDataAccess.Delete(issue);
            await this.AssertGet(issue.Id, null);
        }
    }
}
