namespace DocumentStore.Extensions
{
    using Marten;
    using Model;

    public static class MartenRegistryExtension
    {
        public static StoreOptions GinIndex<T>(this StoreOptions options, string shortName, string column)
        {
            var doc      = options.Storage.MappingFor(typeof(MappingComplex));
            var ginIndex = new GinIndex(doc, shortName, column);
            doc.Indexes.Add(ginIndex);
            return options;
        }
    }
}