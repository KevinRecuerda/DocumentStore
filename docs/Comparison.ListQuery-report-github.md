``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.967 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648437 Hz, Resolution=377.5812 ns, Timer=TSC
.NET Core SDK=2.1.502
  [Host]              : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT  [AttachedDebugger]
  list-property_query : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT

Job=list-property_query  IterationCount=10  LaunchCount=2  
RunStrategy=ColdStart  WarmupCount=1  

```
|     Method |  N |     Mean |     Error |   StdDev |    Median | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|----------- |--- |---------:|----------:|---------:|----------:|------------:|------------:|------------:|--------------------:|
|   **Document** |  **1** | **22.42 ms** | **46.740 ms** | **53.83 ms** |  **5.466 ms** |           **-** |           **-** |           **-** |            **43.88 KB** |
| Relational |  1 | 16.76 ms |  9.904 ms | 11.41 ms | 12.685 ms |           - |           - |           - |             5.88 KB |
|   **Document** | **10** | **27.27 ms** | **57.446 ms** | **66.15 ms** |  **5.847 ms** |           **-** |           **-** |           **-** |            **43.88 KB** |
| Relational | 10 | 16.79 ms | 10.715 ms | 12.34 ms | 13.088 ms |           - |           - |           - |             5.88 KB |
