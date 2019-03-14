namespace RelationalStore.Tests
{
    using System.Threading.Tasks;
    using DeepEqual.Syntax;
    using Model;
    using Model.Tests;
    using Xunit;

    public class UserDataAccessTests
    {
        private readonly UserDataAccess userDataAccess;

        public UserDataAccessTests()
        {
            this.userDataAccess = new UserDataAccess(new ConnectionStrings());
        }

        public static User CreateUser(string firstName, string lastName, Gender gender, string country)
        {
            return new User
            {
                FirstName = firstName,
                LastName  = lastName,
                Gender    = gender,
                Address   = new Address {Country = country}
            };
        }

        public async Task<User> CreateAndInsertUser(string firstName, string lastName, Gender gender, string country)
        {
            var user = CreateUser(firstName, lastName, gender, country);
            await this.userDataAccess.Insert(user);
            return user;
        }

        [Fact]
        public async Task Should_C_R_U_D()
        {
            var user1 = CreateUser("Jason",    "Statham", Gender.Male, "England");
            var user2 = CreateUser("Zinedine", "Zidane",  Gender.Male, "France");

            // Create
            await this.AssertInsert(user1);
            await this.AssertInsert(user2);

            // Read
            await this.AssertGetAll(user1, user2);

            // Update
            user1.UserName = "JS";
            user1.Address.Street = "unknown";
            await this.AssertUpdate(user1);

            // Delete
            await this.AssertDelete(user1);
            await this.AssertDelete(user2);
        }

        private async Task AssertGet(long id, User expected)
        {
            var actual = await this.userDataAccess.GetById(id);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertGetAll(params User[] expected)
        {
            var actual = await this.userDataAccess.GetAll();
            actual.ShouldDeepEqual(expected);

        }
        private async Task AssertInsert(User user)
        {
            await this.userDataAccess.Insert(user);
            await this.AssertGet(user.Id, user);
        }

        private async Task AssertUpdate(User user)
        {
            await this.userDataAccess.Update(user);
            await this.AssertGet(user.Id, user);
        }

        private async Task AssertDelete(User user)
        {
            await this.userDataAccess.Delete(user);
            await this.AssertGet(user.Id, null);
        }
    }
}