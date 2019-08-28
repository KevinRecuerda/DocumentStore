``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.1268 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648434 Hz, Resolution=377.5816 ns, Timer=TSC
.NET Core SDK=2.2.300
  [Host]             : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT  [AttachedDebugger]
  mapping-bulkInsert : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT

Job=mapping-bulkInsert  InvocationCount=1  IterationCount=3  
LaunchCount=2  RunStrategy=Monitoring  UnrollFactor=1  
WarmupCount=1  

```
|  Method |      N |        Mean |      Error |     StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|-------- |------- |------------:|-----------:|-----------:|------------:|------------:|------------:|--------------------:|
|  **Simple** |     **10** |    **16.36 ms** |   **9.972 ms** |   **3.556 ms** |           **-** |           **-** |           **-** |            **22.23 KB** |
| Complex |     10 |    23.75 ms |   8.226 ms |   2.933 ms |           - |           - |           - |            29.55 KB |
|  **Simple** |   **1000** |    **71.94 ms** |   **8.412 ms** |   **3.000 ms** |           **-** |           **-** |           **-** |          **1776.24 KB** |
| Complex |   1000 |    53.98 ms |   7.982 ms |   2.847 ms |           - |           - |           - |          1065.96 KB |
|  **Simple** | **100000** | **7,653.97 ms** | **420.575 ms** | **149.981 ms** |  **43000.0000** |           **-** |           **-** |         **179796.8 KB** |
| Complex | 100000 | 4,991.27 ms | 296.293 ms | 105.661 ms |  25000.0000 |           - |           - |        104882.36 KB |
