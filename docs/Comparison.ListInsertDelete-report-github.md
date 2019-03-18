``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.967 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648437 Hz, Resolution=377.5812 ns, Timer=TSC
.NET Core SDK=2.1.502
  [Host]                      : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT  [AttachedDebugger]
  list-property_insert-delete : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT

Job=list-property_insert-delete  IterationCount=3  LaunchCount=2  
RunStrategy=ColdStart  WarmupCount=1  

```
|     Method |  N |       Mean |      Error |   StdDev |      Median | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|----------- |--- |-----------:|-----------:|---------:|------------:|------------:|------------:|------------:|--------------------:|
|   **Document** |  **1** |   **524.0 ms** | **1,875.7 ms** | **668.9 ms** |    **94.49 ms** |           **-** |           **-** |           **-** |          **1051.64 KB** |
| Relational |  1 |   435.5 ms | 1,492.6 ms | 532.3 ms |   131.21 ms |           - |           - |           - |           284.41 KB |
|   **Document** | **10** |   **908.6 ms** | **1,973.6 ms** | **703.8 ms** |   **516.21 ms** |           **-** |           **-** |           **-** |          **1051.64 KB** |
| Relational | 10 | 1,242.1 ms | 1,429.7 ms | 509.8 ms | 1,010.20 ms |           - |           - |           - |           284.45 KB |
