``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.967 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648437 Hz, Resolution=377.5812 ns, Timer=TSC
.NET Core SDK=2.1.502
  [Host]                       : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT  [AttachedDebugger]
  class-property_insert-delete : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT

Job=class-property_insert-delete  IterationCount=3  LaunchCount=2  
RunStrategy=ColdStart  WarmupCount=1  

```
|     Method |  N |     Mean |      Error |   StdDev |    Median | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|----------- |--- |---------:|-----------:|---------:|----------:|------------:|------------:|------------:|--------------------:|
|   **Document** |  **1** | **636.5 ms** | **2,584.6 ms** | **921.7 ms** |  **72.59 ms** |           **-** |           **-** |           **-** |           **780.55 KB** |
| Relational |  1 | 392.7 ms | 1,469.4 ms | 524.0 ms |  56.38 ms |           - |           - |           - |           284.37 KB |
|   **Document** | **10** | **972.0 ms** | **1,820.5 ms** | **649.2 ms** | **735.75 ms** |           **-** |           **-** |           **-** |           **780.55 KB** |
| Relational | 10 | 888.9 ms | 1,713.3 ms | 611.0 ms | 571.77 ms |           - |           - |           - |           284.37 KB |
