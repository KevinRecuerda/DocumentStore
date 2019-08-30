namespace Comparison
{
    using System;
    using System.Reflection;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Running;
    using Comparison.DocPerf;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Run all
            //BenchmarkRunner.Run(Assembly.GetExecutingAssembly());

            // Run alone
            //BenchmarkRunner.Run<UserCRUD>();
            //BenchmarkRunner.Run<IssueCRUD>();
            //BenchmarkRunner.Run<IssueQueryByList>();
            //BenchmarkRunner.Run<MappingBulkInsert>();
            //BenchmarkRunner.Run<MappingQuery>();
            BenchmarkRunner.Run<SearchSimilarPerf>();

            //var summary = BenchmarkRunner.Run<IssueStorage>(new DebugBuildConfig());
            //Console.ReadLine();

            // For joined results
            //BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).RunAllJoined();
        }
    }

    // TODO : compare storage
}