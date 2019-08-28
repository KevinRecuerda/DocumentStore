``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.1268 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648434 Hz, Resolution=377.5816 ns, Timer=TSC
.NET Core SDK=2.2.300
  [Host]        : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT  [AttachedDebugger]
  mapping-query : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT

Job=mapping-query  IterationCount=3  LaunchCount=2  
RunStrategy=Monitoring  WarmupCount=1  

```
|          Method |      N |       Mean |       Error |     StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|---------------- |------- |-----------:|------------:|-----------:|------------:|------------:|------------:|--------------------:|
|          **Simple** |     **10** |   **433.5 ms** |    **40.54 ms** |  **14.458 ms** |           **-** |           **-** |           **-** |            **26.23 KB** |
|      ComplexOne |     10 |   439.0 ms |    36.50 ms |  13.017 ms |           - |           - |           - |            27.23 KB |
| ComplexMultiple |     10 |   440.6 ms |    18.81 ms |   6.708 ms |           - |           - |           - |            28.29 KB |
|          **Simple** |   **1000** |   **457.8 ms** |    **90.98 ms** |  **32.446 ms** |           **-** |           **-** |           **-** |            **26.23 KB** |
|      ComplexOne |   1000 |   417.6 ms |    94.30 ms |  33.629 ms |           - |           - |           - |            27.59 KB |
| ComplexMultiple |   1000 |   432.2 ms |   155.98 ms |  55.625 ms |           - |           - |           - |            28.46 KB |
|          **Simple** | **100000** |   **453.6 ms** |    **84.11 ms** |  **29.994 ms** |           **-** |           **-** |           **-** |             **26.4 KB** |
|      ComplexOne | 100000 | 1,945.9 ms | 2,030.13 ms | 723.964 ms |           - |           - |           - |            29.09 KB |
| ComplexMultiple | 100000 |   925.8 ms |    93.83 ms |  33.459 ms |           - |           - |           - |            28.46 KB |
