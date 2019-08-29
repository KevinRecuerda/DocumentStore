namespace Comparison.DocRel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Model;
    using Model.Tests;

    //[SimpleJob(RunStrategy.Monitoring, launchCount: 1, warmupCount: 0, targetCount: 1, id: "issue-storage")]
    public class IssueStorage
    {
        private DocumentStore.IssueDataAccess   doc;
        private RelationalStore.IssueDataAccess rel;

        public static string[,] Sizes;
        public static Dictionary<int, int> ParamToRowIndex;

        //static IssueStorage()
        //{
        //    var parameters = typeof(IssueStorage).GetProperty(nameof(N))
        //                                         .GetAttribute<ParamsAttribute>()
        //                                         .Values
        //                                         .Cast<int>()
        //                                         .OrderBy(i => i)
        //                                         .ToArray();

        //    ParamToRowIndex = new Dictionary<int, int>();

        //    Sizes = new string[parameters.Length + 1, 3];
        //    Sizes[0, 0] = "";
        //    Sizes[0, 1] = "Document";
        //    Sizes[0, 2] = "Relational";

        //    for (var i = 0; i < parameters.Length; i++)
        //    {
        //        Sizes[i + 1, 0] = parameters[i].ToString();
        //        ParamToRowIndex[]
        //    }
        //}

        //[Params(1, 10, 100, 1000)]
        [Params(1)]
        public int N { get; set; }

        [Params("--")]
        public string Size { get; set; }

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
            this.Size = await GetTableSize("docs", "mt_doc_issue");

            var current = System.IO.Directory.GetCurrentDirectory();
            Console.WriteLine(current);
            System.IO.File.WriteAllText($"Document_{this.N}", this.Size);
        }

        [Benchmark(Description = "Relational")]
        public async Task Relational()
        {
            this.Size = await GetTableSize("rel", "issues");
        }

        private static async Task<string> GetTableSize(string schema, string table)
        {
            var storageDataAccess = new StorageDataAccess(new ConnectionStrings());
            var size = await storageDataAccess.GetTableSize(schema, table);
            return size;
        }
    }
}