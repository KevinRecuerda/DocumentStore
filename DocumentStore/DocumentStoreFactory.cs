namespace DocumentStore
{
    using System;
    using DocumentStore.Extensions;
    using Marten;
    using Marten.Schema;
    using Model;

    public static class DocumentStoreFactory
    {
        public static IDocumentStore CreateDocumentStore(IConnectionStrings connectionStrings, Action<StoreOptions> configure = null)
        {
            return DocumentStore.For(
                options =>
                {
                    // For debug only
                    //options.Logger(new ConsoleMartenLogger());
                    options.Connection(connectionStrings.ConnectionString);
                    options.AutoCreateSchemaObjects = AutoCreate.All;

                    options.Linq.MethodCallParsers.Add(new ContainsAny());
                    options.Linq.MethodCallParsers.Add(new SearchRegular());

                    options.DatabaseSchemaName = "docs";

                    options.UseDefaultSerialization(EnumStorage.AsString);

                    options.Schema
                           .For<Issue>()
                           .ForeignKey<User>(issue => issue.AssigneeId)
                           .ForeignKey<User>(issue => issue.ReporterId);

                    options.Schema
                           .For<Smurf>()
                           .AddSubClassHierarchy(
                                typeof(SmurfLeader),
                                typeof(PapaSmurf),
                                typeof(Smurfette),
                                typeof(HeftySmurf),
                                typeof(BrainySmurf));

                    options.Schema
                           .For<MappingSimple>()
                           .Identity(m => m.FrenchId)
                           .Index(m => m.FrenchId);

                    options.Schema
                           .For<MappingComplex>()
                           .Identity(m => m.WorldId)
                           .GinIndexJsonData()
                           .Index(m => m.FrenchIds, index => index.Method = IndexMethod.gin);

                    //options.GinIndex<MappingComplex>("french_ids", "data->'FrenchIds'");

                    // TODO: use gin_trgm_ops
                    //options.Schema
                    //       .For<TextSearch>()
                    //       .Index(x => x.Text, index =>
                    //        {
                    //            index.Method = IndexMethod.gin;
                    //        });

                    configure?.Invoke(options);
                });
        }
    }
}