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
    [SimpleJob(RunStrategy.Monitoring, launchCount: 2, warmupCount: 1, targetCount: 10, id: "list-property_query")]
    [RPlotExporter, PlainExporter]
    public class ListQuery
    {
        private DocumentStore.IssueDataAccess   doc;
        private RelationalStore.IssueDataAccess rel;

        [Params(1, 10, 100, 1000)]
        public int N { get; set; }

        private static readonly Random Random = new Random();

        public string[] Tags          { get; set; }
        public string[] TagsToRequest { get; set; }

        public List<Issue> Issues    { get; set; }
        public List<Issue> DocIssues { get; set; }
        public List<Issue> RelIssues { get; set; }

        [GlobalSetup]
        public async Task Setup()
        {
            this.Tags          = Enumerable.Range(1, 10).Select(i => $"Tag{i}").ToArray();
            this.TagsToRequest = this.Tags.Take(3).ToArray();

            this.Issues = Enumerable.Range(1, this.N)
                                    .Select(
                                         i => new Issue()
                                         {
                                             Name = $"Issue{i}",
                                             Tags = Enumerable.Range(1, Random.Next(1, 3))
                                                              .Select(_ => this.Tags[Random.Next(0, 9)])
                                                              .ToList()
                                         })
                                    .ToList();

            this.DocIssues = this.Issues.Select(i => new Issue {Name = i.Name, Tags = i.Tags.ToList()}).ToList();
            this.RelIssues = this.Issues.Select(i => new Issue {Name = i.Name, Tags = i.Tags.ToList()}).ToList();

            this.doc = new DocumentStore.IssueDataAccess(DocumentStore.DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings()));
            this.rel = new RelationalStore.IssueDataAccess(new ConnectionStrings());

            foreach (var issue in this.DocIssues)
                await this.doc.Save(issue);

            foreach (var issue in this.RelIssues)
                await this.rel.Insert(issue);
        }

        [GlobalCleanup]
        public async Task Cleanup()
        {
            foreach (var issue in this.DocIssues)
                this.doc.Delete(issue);

            foreach (var issue in this.RelIssues)
                await this.rel.Delete(issue);
        }

        [Benchmark(Description = "Document")]
        public async Task Document()
        {
            await this.doc.GetByTags(this.TagsToRequest);
        }

        [Benchmark(Description = "Relational")]
        public async Task Relational()
        {
            await this.rel.GetByTags(this.TagsToRequest);
        }
    }
}