namespace RelationalStore
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using Dapper.FastCrud;
    using Model;

    public class UserDataAccess : BaseDataAccess
    {
        private const string SelectQuery = @"
SELECT *
FROM rel.users u
JOIN rel.address a
    on u.address_id = a.id
";

        public UserDataAccess(IConnectionStrings connectionStrings) : base(connectionStrings) { }

        public async Task<IEnumerable<User>> GetAll()
        {
            using (var db = this.Open())
            {
                var users = await db.QueryAsync<User, Address, User>(
                                SelectQuery,
                                (user, address) =>
                                {
                                    user.Address = address;
                                    return user;
                                });
                return users;
            }
        }

        public async Task<User> GetById(long id)
        {
            using (var db = this.Open())
            {
                var users = await db.QueryAsync<User, Address, User>(
                                SelectQuery,
                                (user, address) =>
                                {
                                    user.Address = address;
                                    return user;
                                });
                return users.FirstOrDefault();
            }
        }

        public async Task Insert(User user)
        {
            using (var db = this.Open())
            {
                var addressDto = user.Address.ToDto();
                await db.InsertAsync(addressDto);

                var userDto = user.ToDto(addressDto.Id);
                await db.InsertAsync(userDto);
            }
        }

        public async Task Update(User user)
        {
            using (var db = this.Open())
            {
                var dto = (await db.FindAsync<UserDto>(
                               statementOptions => statementOptions.Where($"id = :id")
                                                                   .WithParameters(new {id = user.Id})))
                   .FirstOrDefault();
                
                var addressDto = user.Address.ToDto();
                addressDto.Id = dto.AddressId;
                await db.UpdateAsync(addressDto);

                var userDto = user.ToDto(addressDto.Id);
                await db.UpdateAsync(userDto);
            }
        }

        public async Task Delete(User user)
        {
            using (var db = this.Open())
            {
                var dto = (await db.FindAsync<UserDto>(
                               statementOptions => statementOptions.Where($"id = :id")
                                                                   .WithParameters(new {id = user.Id})))
                   .FirstOrDefault();
                
                var addressDto = user.Address.ToDto();
                addressDto.Id = dto.AddressId;
                await db.DeleteAsync(addressDto);

                var userDto = user.ToDto(addressDto.Id);
                await db.DeleteAsync(userDto);
            }
        }
    }

    public class UserDto
    {
        public long   Id        { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public bool   Internal  { get; set; }
        public string UserName  { get; set; }
        public Gender Gender    { get; set; }

        public long AddressId { get; set; }
    }

    public class AddressDto
    {
        public long Id { get; set; }
        public string Country { get; set; }
        public string City    { get; set; }
        public string Street  { get; set; }
    }

    public static class DtoExtensions
    {
        public static UserDto ToDto(this User user, long addressId)
        {
            return new UserDto()
            {
                Id        = user.Id,
                FirstName = user.FirstName,
                LastName  = user.LastName,
                Internal  = user.Internal,
                UserName  = user.UserName,
                Gender    = user.Gender,
                AddressId = addressId
            };
        }

        public static AddressDto ToDto(this Address address)
        {
            return new AddressDto()
            {
                Country = address.Country,
                City    = address.City,
                Street  = address.Street
            };
        }
    }
}