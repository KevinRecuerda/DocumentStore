namespace Comparison
{
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
            //BenchmarkRunner.Run<ClassInsertDelete>();
            //BenchmarkRunner.Run<ListInsertDelete>();
            BenchmarkRunner.Run<ListQuery>();
        }
    }
}