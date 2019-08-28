``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.1004 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648438 Hz, Resolution=377.5810 ns, Timer=TSC
.NET Core SDK=2.1.502
  [Host]              : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT  [AttachedDebugger]
  issue-query-by-list : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT

Job=issue-query-by-list  IterationCount=10  LaunchCount=2  
RunStrategy=Monitoring  WarmupCount=1  

```
|     Method |   N |      Mean |      Error |     StdDev |    Median | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|----------- |---- |----------:|-----------:|-----------:|----------:|------------:|------------:|------------:|--------------------:|
|   **Document** |   **1** |  **5.177 ms** |  **0.7982 ms** |  **0.9192 ms** |  **5.034 ms** |           **-** |           **-** |           **-** |            **43.88 KB** |
| Relational |   1 | 22.148 ms | 14.0008 ms | 16.1233 ms | 12.479 ms |           - |           - |           - |             5.88 KB |
|   **Document** |  **10** |  **5.084 ms** |  **0.4328 ms** |  **0.4984 ms** |  **4.960 ms** |           **-** |           **-** |           **-** |            **43.88 KB** |
| Relational |  10 | 13.704 ms |  6.4159 ms |  7.3886 ms | 11.489 ms |           - |           - |           - |             5.88 KB |
|   **Document** | **100** |  **7.698 ms** |  **1.1370 ms** |  **1.3093 ms** |  **7.950 ms** |           **-** |           **-** |           **-** |            **43.88 KB** |
| Relational | 100 | 14.271 ms |  4.2782 ms |  4.9268 ms | 12.523 ms |           - |           - |           - |             5.88 KB |
