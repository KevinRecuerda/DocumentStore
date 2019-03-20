namespace Comparison
{
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using Model;
    using RelationalStore;

    public class StorageDataAccess : BaseDataAccess
    {
        public StorageDataAccess(IConnectionStrings connectionStrings) : base(connectionStrings) { }

        public async Task<string> GetTableSize(string schema, string table)
        {
            var query = $@"
select 
    pg_size_pretty(pg_total_relation_size(fullname)::BIGINT) as total_table_size
from (
    select quote_ident(schemaname) || '.' || quote_ident(tablename) as ""fullname""
    from  pg_tables 
    where schemaname = :{nameof(schema)}
        and tablename = :{nameof(table)}
) r";
            using (var db = this.Open())
            {
                var sizes = await db.QueryAsync<string>(query, new {schema, table});
                return sizes.FirstOrDefault();
            }
        }
    }
}