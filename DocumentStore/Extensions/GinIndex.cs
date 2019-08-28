namespace DocumentStore.Extensions
{
    using Marten.Schema;
    using Marten.Storage;

    public class GinIndex : IIndexDefinition
    {
        public GinIndex(IQueryableDocument mapping, string shortName, string column)
        {
            this.IndexName = $"{mapping.Table.Name}_idx_{shortName}";
            this.Table     = mapping.Table;
            this.Column    = column;
        }

        public string ToDDL()
        {
            return $"CREATE INDEX {this.IndexName} ON {this.Table.QualifiedName} USING gin (({this.Column}));";
        }

        public bool Matches(ActualIndex index)
        {
            return index != null;
        }

        public string       IndexName { get; }
        public DbObjectName Table     { get; }
        public string       Column    { get; }
    }
}