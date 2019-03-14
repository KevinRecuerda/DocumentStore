namespace RelationalStore
{
    using System;
    using System.Data;
    using System.Text.RegularExpressions;
    using Dapper.FastCrud;
    using Dapper.FastCrud.Configuration;
    using Dapper.FastCrud.Mappings;
    using Model;
    using Npgsql;

    public abstract class BaseDataAccess
    {
        private readonly IConnectionStrings connectionStrings;

        protected BaseDataAccess(IConnectionStrings connectionStrings)
        {
            this.connectionStrings = connectionStrings;

            OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
            OrmConfiguration.Conventions    = new PostgreOrmConventions();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        protected IDbConnection Open()
        {
            var dbConnection = new NpgsqlConnection(this.connectionStrings.ConnectionString);
            dbConnection.Open();
            return dbConnection;
        }
    }

    public class PostgreOrmConventions : OrmConventions
    {
        public override SqlDatabaseOptions GetDatabaseOptions(SqlDialect dialect)
        {
            if (dialect == SqlDialect.PostgreSql)
                return new PostgreSqlDatabaseOptions();

            return base.GetDatabaseOptions(dialect);
        }

        public override string GetTableName(Type entityType)
        {
            var tableName            = base.GetTableName(entityType);
            var tableNameUnderscored = CamelCaseToUnderscored(tableName);
            return tableNameUnderscored.ToLower();
        }

        public override void ConfigureEntityPropertyMapping(PropertyMapping propertyMapping)
        {
            base.ConfigureEntityPropertyMapping(propertyMapping);

            var columnName            = propertyMapping.DatabaseColumnName;
            var columnNameUnderscored = CamelCaseToUnderscored(columnName);
            propertyMapping.SetDatabaseColumnName(columnNameUnderscored.ToLower());
        }

        private static string CamelCaseToUnderscored(string tableName)
        {
            return Regex.Replace(tableName, "(?<=.)([A-Z])", "_$0", RegexOptions.Compiled);
        }
    }

    public class PostgreSqlDatabaseOptions : SqlDatabaseOptions
    {
        public PostgreSqlDatabaseOptions()
        {
            this.StartDelimiter = this.EndDelimiter = string.Empty;
            this.IsUsingSchemas = true;
        }
    }
}