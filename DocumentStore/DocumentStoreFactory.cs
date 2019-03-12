namespace DocumentStore
{
    using Marten;

    public static class DocumentStoreFactory
    {
        public static IDocumentStore CreateDocumentStore(IConnectionStrings connectionStrings)
        {
            return DocumentStore.For(
                options =>
                {
                    options.Logger(new ConsoleMartenLogger());
                    options.Connection(connectionStrings.ConnectionString);
                    options.AutoCreateSchemaObjects = AutoCreate.All;

                    options.DatabaseSchemaName = "docs";

                    options.UseDefaultSerialization(EnumStorage.AsString);

                    options.Schema
                           .For<Issue>()
                           .ForeignKey<User>(issue => issue.AssigneeId)
                           .ForeignKey<User>(issue => issue.ReporterId);
                });
        }
    }
}