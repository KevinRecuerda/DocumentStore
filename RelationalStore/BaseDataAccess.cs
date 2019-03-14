namespace RelationalStore
{
    using System.Data;
    using Model;
    using Npgsql;

    public abstract class BaseDataAccess
    {
        private readonly IConnectionStrings connectionStrings;

        protected BaseDataAccess(IConnectionStrings connectionStrings)
        {
            this.connectionStrings = connectionStrings;
        }

        protected IDbConnection Open()
        {
            var dbConnection= new NpgsqlConnection(this.connectionStrings.ConnectionString);
            dbConnection.Open();
            return dbConnection;
        }
    }
}