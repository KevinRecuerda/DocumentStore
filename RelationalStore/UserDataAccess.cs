namespace RelationalStore
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data;
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
JOIN rel.addresses a
    on u.address_id = a.id
";

        public UserDataAccess(IConnectionStrings connectionStrings) : base(connectionStrings)
        {
            OrmConfiguration.GetDefaultEntityMapping<AddressDto>()
                            .SetSchemaName("rel")
                            .SetTableName("addresses")
                            .SetProperty(address => address.Id, prop => prop.SetPrimaryKey().SetDatabaseGenerated(DatabaseGeneratedOption.Identity));

            OrmConfiguration.GetDefaultEntityMapping<UserDto>()
                            .SetSchemaName("rel")
                            .SetTableName("users")
                            .SetProperty(user => user.Id, prop => prop.SetPrimaryKey().SetDatabaseGenerated(DatabaseGeneratedOption.Identity));
        }

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
                var query = $@"
{SelectQuery}
WHERE u.id = :id";
                var users = await db.QueryAsync<User, Address, User>(
                                query,
                                (user, address) =>
                                {
                                    user.Address = address;
                                    return user;
                                },
                                new {id});
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
                user.Id = userDto.Id;
            }
        }

        public async Task Update(User user)
        {
            using (var db = this.Open())
            {
                var dto = await GetDtoById(user, db);
                
                var addressDto = user.Address.ToDto();
                addressDto.Id = dto.AddressId;
                await db.UpdateAsync(addressDto);

                var userDto = user.ToDto(dto.AddressId);
                await db.UpdateAsync(userDto);
            }
        }

        public async Task Delete(User user)
        {
            using (var db = this.Open())
            {
                var dto = await GetDtoById(user, db);

                var userDto = user.ToDto(dto.AddressId);
                await db.DeleteAsync(userDto);
                
                var addressDto = user.Address.ToDto();
                addressDto.Id = dto.AddressId;
                await db.DeleteAsync(addressDto);
            }
        }

        private static async Task<UserDto> GetDtoById(User user, IDbConnection db)
        {
            var dto = await db.FindAsync<UserDto>(
                          statementOptions => statementOptions.Where($"id = :id")
                                                              .WithParameters(new {id = user.Id}));
            return dto.FirstOrDefault();
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