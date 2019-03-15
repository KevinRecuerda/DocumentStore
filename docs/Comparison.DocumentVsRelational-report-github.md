``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.967 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648437 Hz, Resolution=377.5812 ns, Timer=TSC
.NET Core SDK=2.1.502
  [Host]               : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT  [AttachedDebugger]
  DocumentVsRelational : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT

Job=DocumentVsRelational  LaunchCount=2  RunStrategy=ColdStart  
WarmupCount=1  

```
|     Method |    N |         Mean |       Error |     StdDev |       Median | Rank | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|----------- |----- |-------------:|------------:|-----------:|-------------:|-----:|------------:|------------:|------------:|--------------------:|
|   **Document** |    **1** |     **98.72 ms** |    **32.12 ms** |   **136.0 ms** |     **66.87 ms** |    **2** |           **-** |           **-** |           **-** |           **780.53 KB** |
| Relational |    1 |     70.24 ms |    26.12 ms |   110.6 ms |     46.10 ms |    1 |           - |           - |           - |           284.34 KB |
|   **Document** |   **10** |    **340.69 ms** |    **38.53 ms** |   **163.1 ms** |    **307.03 ms** |    **3** |           **-** |           **-** |           **-** |           **780.53 KB** |
| Relational |   10 |    470.45 ms |    39.63 ms |   167.8 ms |    422.26 ms |    4 |           - |           - |           - |           284.34 KB |
|   **Document** |  **100** |  **3,011.33 ms** |   **108.73 ms** |   **460.4 ms** |  **2,931.06 ms** |    **5** |   **1000.0000** |           **-** |           **-** |           **780.53 KB** |
| Relational |  100 |  4,481.22 ms |   174.55 ms |   739.1 ms |  4,301.76 ms |    6 |           - |           - |           - |           284.34 KB |
|   **Document** | **1000** | **33,425.10 ms** | **1,136.57 ms** | **4,812.3 ms** | **32,526.08 ms** |    **7** |   **9000.0000** |   **3000.0000** |           **-** |           **812.55 KB** |
| Relational | 1000 | 50,215.14 ms | 1,851.32 ms | 7,838.6 ms | 47,347.82 ms |    8 |   6000.0000 |           - |           - |           284.34 KB |
