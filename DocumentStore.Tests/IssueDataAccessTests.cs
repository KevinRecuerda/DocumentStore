namespace DocumentStore.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DeepEqual.Syntax;
    using FluentAssertions;
    using Marten;
    using Model;
    using Model.Tests;
    using Xunit;

    public class IssueDataAccessTests : IDisposable
    {
        private readonly IssueDataAccess     issueDataAccess;
        private readonly UserDataAccessTests userTests;
        private readonly IDocumentStore documentStore;

        public IssueDataAccessTests()
        {
            this.documentStore = DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings());
            this.issueDataAccess = new IssueDataAccess(this.documentStore);
            this.userTests       = new UserDataAccessTests();
        }

        public void Dispose()
        {
            this.documentStore.Advanced.Clean.DeleteDocumentsFor(typeof(User));
            this.documentStore.Advanced.Clean.DeleteDocumentsFor(typeof(Issue));
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
            var issue2 = CreateIssue("issue with tag",           "tag1");
            var issue3 = CreateIssue("issue with multiple tags", "tag1", "tag2");
            var issue4 = CreateIssue("issue with another tag",   "tag3");

            // Create
            await this.AssertSave(issue1);
            await this.AssertSave(issue2);
            await this.AssertSave(issue3);
            await this.AssertSave(issue4);

            // Read
            await this.AssertGetAll(issue1, issue2, issue3, issue4);
            await this.AssertGetByTags(new[] {"tag1"},         issue2, issue3);
            await this.AssertGetByTags(new[] {"tag1", "tag3"}, issue2, issue3, issue4);
            await this.AssertGetByTags(new[] {"tag unknown"});
            await this.AssertGetByTags(new string[] { });

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

        [Fact]
        public async Task Should_Include_reference()
        {
            // Arrange
            var issue1 = CreateIssue("issue without tags");
            var issue2 = CreateIssue("issue with tag",           "tag1");
            var issue3 = CreateIssue("issue with multiple tags", "tag1", "tag2");
            var issue4 = CreateIssue("issue with another tag",   "tag3");

            var user1  = await this.userTests.CreateAndInsertUser("user 1", "-", Gender.Female, "Country");
            var user2  = await this.userTests.CreateAndInsertUser("user 2", "-", Gender.Female, "Country");

            issue2.AssigneeId = user1.Id;
            issue3.AssigneeId = user2.Id;
            
            await this.AssertSave(issue1);
            await this.AssertSave(issue2);
            await this.AssertSave(issue3);
            await this.AssertSave(issue4);

            // ById
            await this.AssertGetByIdIncludingAssignee(issue1, null);
            await this.AssertGetByIdIncludingAssignee(issue2, user1);
            await this.AssertGetByIdIncludingAssignee(issue3, user2);
            await this.AssertGetByIdIncludingAssignee(issue4, null);

            // ByTags
            await this.AssertGetByTagsIncludingAssignee(new[] {"tag1"}, new[] {issue2, issue3}, new[] {user1, user2});
            await this.AssertGetByTagsIncludingAssignee(new[] {"tag3"}, new[] {issue4}, new User[] { });
            await this.AssertGetByTagsIncludingAssignee(new[] {"tag unknown"}, new Issue[] { }, new User[] { });

            // ByFirstName
            await this.AssertGetByFirstNameIncludingAssignee(user1.FirstName, user1, issue2);
            await this.AssertGetByFirstNameIncludingAssignee(user2.FirstName, user2, issue3);
            await this.AssertGetByFirstNameIncludingAssignee("toto", null);

            // Delete
            await this.AssertDelete(issue1);
            await this.AssertDelete(issue2);
            await this.AssertDelete(issue3);
            await this.AssertDelete(issue4);
            await this.userTests.AssertDelete(user1);
            await this.userTests.AssertDelete(user2);
        }

        private async Task AssertGetAll(params Issue[] expected)
        {
            var actual = await this.issueDataAccess.GetAll();
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetById(long id, Issue expected)
        {
            var actual = await this.issueDataAccess.GetById(id);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetByIdIncludingAssignee(Issue expectedIssue, User expectedUser)
        {
            var (actualIssue, actualUser) = await this.issueDataAccess.GetByIdIncludingAssignee(expectedIssue.Id);

            actualIssue.ShouldDeepEqual(expectedIssue);
            actualUser.ShouldDeepEqual(expectedUser);
        }

        private async Task AssertGetByTags(string[] tags, params Issue[] expected)
        {
            var actual = await this.issueDataAccess.GetByTags(tags);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetByTagsIncludingAssignee(string[] tags, Issue[] expectedIssues, User[] expectedUsers)
        {
            var (actualIssues, actualUsers) = await this.issueDataAccess.GetByTagsIncludingAssignee(tags);

            actualIssues.ShouldDeepEqual(expectedIssues);
            actualUsers.Values.ShouldDeepEqual(expectedUsers);
        }

        private async Task AssertGetByFirstNameIncludingAssignee(string firstName, User expectedUser, params Issue[] expectedIssues)
        {
            var (actualIssues, actualUsers) = await this.issueDataAccess.GetByFirstNameIncludingAssignee(firstName);

            if (expectedUser == null)
            {
                actualIssues.Should().BeEmpty();
                actualUsers.Should().BeEmpty();
            }
            else
            {
                actualIssues.ShouldDeepEqual(expectedIssues);
                actualUsers.Values.Single().ShouldDeepEqual(expectedUser);
            }
        }

        private async Task AssertSave(Issue issue)
        {
            await this.issueDataAccess.Save(issue);
            await this.AssertGetById(issue.Id, issue);
        }

        private async Task AssertDelete(Issue issue)
        {
            this.issueDataAccess.Delete(issue);
            await this.AssertGetById(issue.Id, null);
        }
    }
}