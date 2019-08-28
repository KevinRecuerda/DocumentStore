``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.1268 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648434 Hz, Resolution=377.5816 ns, Timer=TSC
.NET Core SDK=2.2.300
  [Host]        : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT  [AttachedDebugger]
  mapping-Query : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT

Job=mapping-Query  IterationCount=3  LaunchCount=2  
RunStrategy=Monitoring  WarmupCount=1  

```
|  Method |      N |      Mean |     Error |     StdDev | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|-------- |------- |----------:|----------:|-----------:|------------:|------------:|------------:|--------------------:|
|  **Simple** |     **10** |  **9.905 ms** |  **3.591 ms** |  **1.2806 ms** |           **-** |           **-** |           **-** |            **27.04 KB** |
| Complex |     10 |  8.935 ms |  2.310 ms |  0.8236 ms |           - |           - |           - |            28.39 KB |
|  **Simple** |   **1000** |  **9.293 ms** |  **4.330 ms** |  **1.5440 ms** |           **-** |           **-** |           **-** |            **27.04 KB** |
| Complex |   1000 | 11.200 ms |  7.792 ms |  2.7788 ms |           - |           - |           - |            28.74 KB |
|  **Simple** | **100000** |  **9.817 ms** |  **2.537 ms** |  **0.9046 ms** |           **-** |           **-** |           **-** |            **27.38 KB** |
| Complex | 100000 | 54.241 ms | 35.298 ms | 12.5877 ms |           - |           - |           - |             28.7 KB |
