namespace Comparison.DocPerf
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using DocumentStore;
    using Marten;
    using Model;
    using Model.Tests;

    public class TextSearchData
    {
        public TextSearchData(int n)
        {
            var lines = File.ReadAllLines("DocPerf/sample.txt").ToList();
            while (lines.Count < n)
                lines.AddRange(lines.ToList());

            this.TextSearches = lines.Select(x => new TextSearch() {Text = x})
                                     .Take(n)
                                     .ToArray();

            this.TextToSearch = new[]
            {
                "fran",
                "tech corp",
                "bus"
            };
        }

        public TextSearch[] TextSearches { get; set; }

        public string[] TextToSearch { get; set; }
    }

    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Monitoring, launchCount: 2, warmupCount: 1, targetCount: 3, id: "search-similar")]
    public class SearchSimilarPerf
    {
        private int            n;
        private TextSearchData data;
        private IDocumentStore documentStore;

        public SearchSimilarPerf()
        {
            this.documentStore = DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings());
        }

        [Params(10, 1000, 100000)]
        public int N
        {
            get => this.n;
            set
            {
                this.n    = value;
                this.data = new TextSearchData(this.n);
            }
        }

        [Params(3, 10, 100)]
        public int Top { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            new TextSearchDataAccess(this.documentStore).Insert(this.data.TextSearches);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            this.documentStore.Advanced.Clean.DeleteDocumentsFor(typeof(TextSearch));
        }

        [Benchmark]
        public async Task WithoutIndex()
        {
            this.documentStore = DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings(), false);
            await this.Search();
        }

        [Benchmark]
        public async Task WithIndex()
        {
            this.documentStore = DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings(), true);
            await this.Search();
        }

        private async Task Search()
        {
            var da = new TextSearchDataAccess(this.documentStore);

            foreach (var id in this.data.TextToSearch)
                await da.SearchByText(id, this.Top);
        }
    }
}