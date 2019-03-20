namespace Comparison
{
    using System.Reflection;
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Run all
            BenchmarkRunner.Run(Assembly.GetExecutingAssembly());

            // Run alone
            //BenchmarkRunner.Run<ClassInsertDelete>();
            //BenchmarkRunner.Run<ListInsertDelete>();
            //BenchmarkRunner.Run<ListQuery>();

            // For joined results
            //BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).RunAllJoined();
        }
    }
}