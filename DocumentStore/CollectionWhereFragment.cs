namespace DocumentStore
{
    using System;
    using System.Linq;
    using Marten.Linq;

    public class CollectionWhereFragment : CustomizableWhereFragment
    {
        public CollectionWhereFragment(string sql, params object[] parameters) : base(
            sql,
            "??",
            parameters.Select(x => Tuple.Create<object, NpgsqlTypes.NpgsqlDbType?>(x, null)).ToArray()) { }
    }
}