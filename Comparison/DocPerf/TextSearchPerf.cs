namespace Comparison.DocPerf
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
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
            this.TextSearches = File.ReadAllLines("DocPerf/sample.txt")
                                    .Select(x => new TextSearch() {Text = x})
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
    [SimpleJob(RunStrategy.Monitoring, launchCount: 2, warmupCount: 1, targetCount: 3, id: "mapping-query")]
    public class TextSearchPerf
    {
        private const int            Repeat = 50;
        private       int            n;
        private       TextSearchData data;
        private       IDocumentStore documentStore;

        public TextSearchPerf()
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

        [GlobalSetup(Target = nameof(WithoutIndex))]
        public void SetupWithoutIndex()
        {
            void RemovingIndex(StoreOptions options)
            {
                var doc = options.Storage.MappingFor(typeof(TextSearch));
                doc.Indexes.Clear();
            }

            void Configure(StoreOptions options)
            {
                var alterProperty = options.Schema.GetType().GetProperty("alter", BindingFlags.NonPublic | BindingFlags.Instance);
                alterProperty.SetValue(options.Schema, (Action<StoreOptions>) RemovingIndex);
            }

            this.documentStore = DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings(), Configure);
        }

        [GlobalSetup(Target = nameof(WithIndex))]
        public void SetupWithIndex()
        {
            this.documentStore = DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings());
        }

        [Benchmark]
        public async Task WithoutIndex()
        {
            await this.Search();
        }

        [Benchmark]
        public async Task WithIndex()
        {
            await this.Search();
        }

        private async Task Search()
        {
            var da = new TextSearchDataAccess(this.documentStore);

            for (var i = 0; i < Repeat; i++)
                foreach (var id in this.data.TextToSearch)
                    await da.SearchByText(id);
        }
    }
}