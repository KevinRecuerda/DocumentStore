namespace DocumentStore.Extensions
{
    using Marten;
    using Marten.Schema;

    public static class MartenRegistryExtensions
    {
        public static StoreOptions GinIndex<T>(this StoreOptions options, string shortName, string column, string op = "")
        {
            return options.Index<T>(shortName, column, IndexMethod.gin, op);
        }

        public static StoreOptions GistIndex<T>(this StoreOptions options, string shortName, string column, string op = "")
        {
            return options.Index<T>(shortName, column, IndexMethod.gist, op);
        }

        public static StoreOptions Index<T>(this StoreOptions options, string shortName, string column, IndexMethod method, string op = "")
        {
            var doc   = options.Storage.MappingFor(typeof(T));
            var index = new SimpleIndex(doc, shortName, column, method, op);;
            doc.Indexes.Add(index);
            return options;
        }
    }
}