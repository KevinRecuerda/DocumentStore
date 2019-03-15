namespace Comparison
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using BenchmarkDotNet.Running;
    using Model;
    using Model.Tests;

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<InsertDelete>();
        }
    }

    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart, launchCount: 2, warmupCount: 1, id: "insert-delete")]
    [RPlotExporter, RankColumn]
    public class InsertDelete
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
                                            Address   = new Address() {Country = $"C-{i}"}
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

    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.ColdStart, launchCount: 2, warmupCount: 1, id: "query-list")]
    [RPlotExporter, RankColumn]
    public class QueryList
    {
        [Params(1, 10, 100, 1000)] 
        public int N { get; set; }

        public List<Issue> Issues{ get; set; }

        [GlobalSetup]
        public void Setup()
        {
            this.Issues = Enumerable.Range(1, this.N)
                                   .Select(
                                        i => new Issue()
                                        {
                                        })
                                   .ToList();
        }

        [Benchmark(Description = "Document")]
        public async Task Document()
        {
            var da = new DocumentStore.UserDataAccess(DocumentStore.DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));
        }

        [Benchmark(Description = "Relational")]
        public async Task Relational()
        {
            var da = new RelationalStore.UserDataAccess(new ConnectionStrings());
        }
    }
}