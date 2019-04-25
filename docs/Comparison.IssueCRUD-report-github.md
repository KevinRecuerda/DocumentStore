``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.1004 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648438 Hz, Resolution=377.5810 ns, Timer=TSC
.NET Core SDK=2.1.502
  [Host]     : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT  [AttachedDebugger]
  issue-CRUD : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT

Job=issue-CRUD  IterationCount=3  LaunchCount=2  
RunStrategy=Monitoring  WarmupCount=1  

```
|     Method |   N |        Mean |       Error |     StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|----------- |---- |------------:|------------:|-----------:|------------:|------------:|------------:|--------------------:|
|   **Document** |   **1** |    **85.16 ms** |    **22.75 ms** |   **8.113 ms** |           **-** |           **-** |           **-** |          **1051.91 KB** |
| Relational |   1 |    48.18 ms |    14.34 ms |   5.114 ms |           - |           - |           - |           284.56 KB |
|   **Document** |  **10** |   **324.44 ms** |    **19.44 ms** |   **6.931 ms** |           **-** |           **-** |           **-** |          **1051.65 KB** |
| Relational |  10 |   549.71 ms |   128.49 ms |  45.821 ms |           - |           - |           - |           284.56 KB |
|   **Document** | **100** | **2,976.40 ms** |   **268.03 ms** |  **95.580 ms** |   **1000.0000** |           **-** |           **-** |          **1051.63 KB** |
| Relational | 100 | 5,835.24 ms | 1,303.31 ms | 464.771 ms |           - |           - |           - |           284.52 KB |
