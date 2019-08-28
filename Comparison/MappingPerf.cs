namespace Comparison
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Engines;
    using DocumentStore;
    using Marten;
    using Model;
    using Model.Tests;

    public class MappingData
    {
        private static readonly Random Random = new Random();

        public MappingSimple[]  SimpleMappings  { get; set; }
        public MappingComplex[] ComplexMappings { get; set; }

        public string[] IdToSearch { get; set; }

        public MappingData(int n)
        {
            var frenchId = 1;
            this.ComplexMappings = Enumerable.Range(1, n)
                                             .Select(
                                                  i => new MappingComplex
                                                  {
                                                      WorldId = $"world id {i}",
                                                      FrenchIds = Enumerable.Range(1, RandomCount())
                                                                            .Select(_ => $"french id {frenchId++}")
                                                                            .ToList()
                                                  })
                                             .ToArray();

            this.SimpleMappings = this.ComplexMappings.SelectMany(
                                           m => m.FrenchIds.Select(
                                               id => new MappingSimple
                                               {
                                                   FrenchId = id,
                                                   WorldId  = m.WorldId
                                               }))
                                      .ToArray();

            this.IdToSearch = new[]
            {
                FirstNearMiddle(this.ComplexMappings, m => m.FrenchIds.Count == 1).FrenchIds.Last(),
                FirstNearMiddle(this.ComplexMappings, m => m.FrenchIds.Count == 3).FrenchIds.Last()
            };
        }

        private static T FirstNearMiddle<T>(T[] array, Func<T, bool> filter)
        {
            var middle = array.Length / 2d - 0.5;

            for (var i = 0; i <= array.Length / 2; i++)
            {
                var down = array[(int) (middle - i)];
                if (filter(down))
                    return down;

                var up = array[(int) (middle + i)];
                if (filter(up))
                    return up;
            }

            return array[(int) middle];
        }

        private static readonly (int, int)[] Weights =
        {
            (40, 1),
            (60, 2),
            (70, 3),
            (80, 4),
            (90, 5)
        };

        private static int RandomCount()
        {
            var percentage = Random.Next(1, 100);
            foreach (var (weight, value) in Weights)
            {
                if (percentage <= weight)
                    return value;
            }

            return 1;
        }
    }

    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Monitoring, launchCount: 2, warmupCount: 1, targetCount: 3, id: "mapping-bulkInsert")]
    public class MappingBulkInsert
    {
        [Params(10, 1000, 100000)]
        public int N
        {
            get => this.n;
            set
            {
                this.n    = value;
                this.data = new MappingData(this.n);
            }
        }

        private          int            n;
        private          MappingData    data;
        private readonly IDocumentStore documentStore;

        public MappingBulkInsert()
        {
            this.documentStore = DocumentStoreFactory.CreateDocumentStore(new ConnectionStrings());
        }

        [IterationSetup]
        [IterationCleanup]
        public void IterationCleanup()
        {
            this.documentStore.Advanced.Clean.DeleteDocumentsFor(typeof(MappingSimple));
            this.documentStore.Advanced.Clean.DeleteDocumentsFor(typeof(MappingComplex));
        }

        [Benchmark]
        public void Simple()
        {
            var da = new MappingSimpleDataAccess(this.documentStore);
            da.Insert(this.data.SimpleMappings);
        }

        [Benchmark]
        public void Complex()
        {
            var da = new MappingComplexDataAccess(this.documentStore);
            da.Insert(this.data.ComplexMappings);
        }
    }

    [MemoryDiagnoser]
    [SimpleJob(RunStrategy.Monitoring, launchCount: 2, warmupCount: 1, targetCount: 3, id: "mapping-query")]
    public class MappingQuery
    {
        private const    int            Repeat = 50;
        private          int            n;
        private          MappingData    data;
        private readonly IDocumentStore documentStore;

        public MappingQuery()
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
                this.data = new MappingData(this.n);
            }
        }

        [GlobalSetup(Target = nameof(Simple))]
        public void SetupSimple()
        {
            new MappingSimpleDataAccess(this.documentStore).Insert(this.data.SimpleMappings);
        }

        [GlobalCleanup(Target = nameof(Simple))]
        public void CleanupSimple()
        {
            this.documentStore.Advanced.Clean.DeleteDocumentsFor(typeof(MappingSimple));
        }

        [GlobalSetup(Targets = new[] {nameof(ComplexOne), nameof(ComplexMultiple)})]
        public void SetupComplex()
        {
            new MappingComplexDataAccess(this.documentStore).Insert(this.data.ComplexMappings);
        }

        [GlobalCleanup(Targets = new[] {nameof(ComplexOne), nameof(ComplexMultiple)})]
        public void CleanupComplex()
        {
            this.documentStore.Advanced.Clean.DeleteDocumentsFor(typeof(MappingComplex));
        }

        [Benchmark]
        public async Task Simple()
        {
            var da = new MappingSimpleDataAccess(this.documentStore);

            for (var i = 0; i < Repeat; i++)
                foreach (var id in this.data.IdToSearch)
                    await da.GetByFrenchId(id);
        }

        [Benchmark]
        public async Task ComplexOne()
        {
            var da = new MappingComplexDataAccess(this.documentStore);

            for (var i = 0; i < Repeat; i++)
                foreach (var id in this.data.IdToSearch)
                    await da.GetByFrenchId(id);
        }

        [Benchmark]
        public async Task ComplexMultiple()
        {
            var da = new MappingComplexDataAccess(this.documentStore);

            for (var i = 0; i < Repeat; i++)
                foreach (var id in this.data.IdToSearch)
                    await da.GetByFrenchIds(new[] {id});
        }
    }
}