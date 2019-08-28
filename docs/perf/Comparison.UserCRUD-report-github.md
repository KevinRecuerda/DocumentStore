``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.1004 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648438 Hz, Resolution=377.5810 ns, Timer=TSC
.NET Core SDK=2.1.502
  [Host]    : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT  [AttachedDebugger]
  user-CRUD : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT

Job=user-CRUD  IterationCount=3  LaunchCount=2  
RunStrategy=Monitoring  WarmupCount=1  

```
|     Method |   N |        Mean |        Error |     StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|----------- |---- |------------:|-------------:|-----------:|------------:|------------:|------------:|--------------------:|
|   **Document** |   **1** |   **376.69 ms** |   **955.415 ms** | **340.710 ms** |           **-** |           **-** |           **-** |           **780.55 KB** |
| Relational |   1 |    37.14 ms |     7.792 ms |   2.779 ms |           - |           - |           - |           284.48 KB |
|   **Document** |  **10** |   **347.06 ms** |   **200.127 ms** |  **71.367 ms** |           **-** |           **-** |           **-** |           **780.55 KB** |
| Relational |  10 |   415.18 ms |    68.777 ms |  24.527 ms |           - |           - |           - |           284.48 KB |
|   **Document** | **100** | **3,912.10 ms** | **1,369.737 ms** | **488.461 ms** |   **1000.0000** |           **-** |           **-** |           **780.55 KB** |
| Relational | 100 | 5,090.64 ms | 1,966.811 ms | 701.384 ms |           - |           - |           - |           284.48 KB |
