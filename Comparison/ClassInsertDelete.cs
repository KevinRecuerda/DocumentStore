namespace Comparison
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using Model;
    using Model.Tests;

    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Monitoring, launchCount: 2, warmupCount: 1, targetCount: 3, id: "class-property_insert-delete")]
    [RPlotExporter]
    public class ClassInsertDelete
    {
        [Params(1, 10, 100, 1000)]
        public int N { get; set; }

        public List<User> Users { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            this.Users = Enumerable.Range(1, this.N)
                                   .Select(
                                        i => new User()
                                        {
                                            FirstName = $"name{i}",
                                            LastName  = "last",
                                            UserName  = "-",
                                            Internal  = true,
                                            Gender    = Gender.Male,
                                            Address   = new Address
                                            {
                                                Country = $"France-{i}",
                                                City = "Paris",
                                                Street = "-"
                                            }
                                        })
                                   .ToList();
        }

        [Benchmark(Description = "Document")]
        public async Task Document()
        {
            var da = new DocumentStore.UserDataAccess(DocumentStore.DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));

            foreach (var user in this.Users)
                await da.Insert(user);

            foreach (var user in this.Users)
                da.Delete(user);
        }

        [Benchmark(Description = "Relational")]
        public async Task Relational()
        {
            var da = new RelationalStore.UserDataAccess(new ConnectionStrings());

            foreach (var user in this.Users)
                await da.Insert(user);

            foreach (var user in this.Users)
                await da.Delete(user);
        }
    }
}