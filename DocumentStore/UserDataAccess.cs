namespace DocumentStore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Marten;

    public class UserDataAccess
    {
        private readonly IDocumentStore store;

        public UserDataAccess(IDocumentStore store)
        {
            this.store = store;
        }

        public async Task<IReadOnlyList<User>> GetAll()
        {
            using (var session = this.store.OpenSession())
            {
                var users = await session.Query<User>().ToListAsync();
                return users;
            }
        }

        public async Task<IReadOnlyList<User>> GetByLastName(string lastName)
        {
            using (var session = this.store.OpenSession())
            {
                var users = await session.Query<User>().Where(x => x.LastName == lastName).ToListAsync();
                return users;
            }
        }

        public async Task<User> GetById(long id)
        {
            using (var session = this.store.OpenSession())
            {
                var user = await session.LoadAsync<User>(id);
                return user;
            }
        }

        public async Task Save(User user)
        {
            using (var session = this.store.OpenSession())
            {
                session.Store(user);
                await session.SaveChangesAsync();
            }
        }

        public async Task Insert(User user)
        {
            using (var session = this.store.OpenSession())
            {
                session.Insert(user);
                await session.SaveChangesAsync();
            }
        }

        public async Task Update(User user)
        {
            using (var session = this.store.OpenSession())
            {
                session.Update(user);
                await session.SaveChangesAsync();
            }
        }

        public void Delete(User user)
        {
            using (var session = this.store.OpenSession())
            {
                session.Delete(user);
                session.SaveChanges();
            }
        }

        public void DeleteByLastName(string lastName)
        {
            using (var session = this.store.OpenSession())
            {
                session.DeleteWhere<User>(x => x.LastName == lastName);
                session.SaveChanges();
            }
        }
    }

    public class User
    {
        public long   Id        { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public bool   Internal  { get; set; }
        public string UserName  { get; set; }

        public Address Address { get; set; }

        public override string ToString()
        {
            return $"{this.FirstName} {this.LastName}";
        }
    }

    public class Address
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}