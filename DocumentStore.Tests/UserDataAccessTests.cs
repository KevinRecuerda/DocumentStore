namespace DocumentStore.Tests
{
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using DeepEqual.Syntax;
    using Xunit;

    public class UserDataAccessTests
    {
        private readonly UserDataAccess userDataAccess;

        public UserDataAccessTests()
        {
            this.userDataAccess = new UserDataAccess(DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));
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
            await this.AssertSave(user2);

            // Read
            await this.AssertGetAll(user1, user2);

            // Update
            user1.UserName = "JS";
            await this.AssertUpdate(user1);
            user2.UserName = "Zizou";
            await this.AssertSave(user2);

            // Delete
            await this.AssertDelete(user1);
            await this.AssertDelete(user2);
        }

        [Fact]
        public async Task Should_Get_and_Delete_by_lastName()
        {
            var user1 = CreateUser("Jason",    "Statham", Gender.Male, "England");
            var user2 = CreateUser("Zinedine", "Zidane",  Gender.Male, "France");
            var user3 = CreateUser("Enzo",     "Zidane",  Gender.Male, "France");

            // Create
            await this.AssertSave(user1);
            await this.AssertSave(user2);
            await this.AssertSave(user3);

            // Read
            await this.AssertGetByLastName("Zidane", user2, user3);

            // Delete
            await this.AssertDeleteByLastName("Zidane");
            await this.AssertDelete(user1);
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

        private async Task AssertGetByLastName(string lastName, params User[] expected)
        {
            var actual = await this.userDataAccess.GetByLastName(lastName);
            actual.ShouldDeepEqual(expected);
        }

        private async Task AssertSave(User user)
        {
            await this.userDataAccess.Save(user);
            await this.AssertGet(user.Id, user);
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
            this.userDataAccess.Delete(user);
            await this.AssertGet(user.Id, null);
        }

        private async Task AssertDeleteByLastName(string lastName)
        {
            this.userDataAccess.DeleteByLastName(lastName);
            await this.AssertGetByLastName(lastName);
        }
    }
}