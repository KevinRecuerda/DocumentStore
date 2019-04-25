namespace Comparison
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using Model;
    using Model.Tests;

    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Monitoring, launchCount: 2, warmupCount: 1, targetCount: 3, id: "issue-CRUD")]
    public class IssueCRUD
    {
        [Params(1, 10, 100)]
        public int N { get; set; }

        private static readonly Random Random = new Random();

        public List<Issue> Issues { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            var tags = Enumerable.Range(1, 10).Select(i => $"Tag{i}").ToArray();

            this.Issues = Enumerable.Range(1, this.N)
                                    .Select(
                                         i => new Issue()
                                         {
                                             Name = $"Issue{i}",
                                             Tags = Enumerable.Range(1, Random.Next(1, 3))
                                                              .Select(_ => tags[Random.Next(0, 9)])
                                                              .ToList()
                                         })
                                    .ToList();
        }

        [Benchmark(Description = "Document")]
        public async Task Document()
        {
            var da = new DocumentStore.IssueDataAccess(DocumentStore.DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));

            foreach (var issue in this.Issues)
                await da.Save(issue);

            foreach (var issue in this.Issues)
                da.Delete(issue);
        }

        [Benchmark(Description = "Relational")]
        public async Task Relational()
        {
            var da = new RelationalStore.IssueDataAccess(new ConnectionStrings());

            foreach (var issue in this.Issues)
                await da.Insert(issue);

            foreach (var issue in this.Issues)
                await da.Delete(issue);
        }
    }
}