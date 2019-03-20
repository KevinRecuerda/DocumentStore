namespace Comparison
{
    using System.Reflection;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Run all
            BenchmarkRunner.Run(Assembly.GetExecutingAssembly());

            // Run alone
            //BenchmarkRunner.Run<UserCRUD>();
            //BenchmarkRunner.Run<IssueCRUD>();
            //BenchmarkRunner.Run<IssueQueryByList>();

            // For joined results
            //BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).RunAllJoined();
        }
    }
}