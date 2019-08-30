``` ini

BenchmarkDotNet=v0.11.4, OS=Windows 10.0.16299.1268 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-6820HQ CPU 2.70GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
Frequency=2648440 Hz, Resolution=377.5808 ns, Timer=TSC
.NET Core SDK=2.2.300
  [Host]         : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT  [AttachedDebugger]
  search-similar : .NET Core 2.1.11 (CoreCLR 4.6.27617.04, CoreFX 4.6.27617.02), 64bit RyuJIT

Job=search-similar  IterationCount=3  LaunchCount=2  
RunStrategy=Monitoring  WarmupCount=1  

```
|       Method |      N | Top |        Mean |      Error |     StdDev |      Median | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------------- |------- |---- |------------:|-----------:|-----------:|------------:|------------:|------------:|------------:|--------------------:|
| **WithoutIndex** |     **10** |   **3** |    **45.48 ms** |  **14.356 ms** |   **5.120 ms** |    **45.39 ms** |           **-** |           **-** |           **-** |           **550.55 KB** |
|    WithIndex |     10 |   3 |    45.44 ms |  13.126 ms |   4.681 ms |    44.13 ms |           - |           - |           - |           552.91 KB |
| **WithoutIndex** |     **10** |  **10** |    **44.81 ms** |  **16.806 ms** |   **5.993 ms** |    **43.05 ms** |           **-** |           **-** |           **-** |           **550.55 KB** |
|    WithIndex |     10 |  10 |    45.81 ms |  15.672 ms |   5.589 ms |    43.67 ms |           - |           - |           - |           552.91 KB |
| **WithoutIndex** |     **10** | **100** |    **43.31 ms** |  **11.809 ms** |   **4.211 ms** |    **42.63 ms** |           **-** |           **-** |           **-** |           **550.55 KB** |
|    WithIndex |     10 | 100 |    48.88 ms |   7.747 ms |   2.763 ms |    48.84 ms |           - |           - |           - |           552.91 KB |
| **WithoutIndex** |   **1000** |   **3** |    **57.86 ms** |  **16.439 ms** |   **5.862 ms** |    **59.03 ms** |           **-** |           **-** |           **-** |           **550.55 KB** |
|    WithIndex |   1000 |   3 |    45.93 ms |  19.201 ms |   6.847 ms |    43.56 ms |           - |           - |           - |           552.91 KB |
| **WithoutIndex** |   **1000** |  **10** |    **56.65 ms** |  **17.526 ms** |   **6.250 ms** |    **57.19 ms** |           **-** |           **-** |           **-** |           **550.55 KB** |
|    WithIndex |   1000 |  10 |    63.82 ms | 122.782 ms |  43.785 ms |    48.37 ms |           - |           - |           - |           552.98 KB |
| **WithoutIndex** |   **1000** | **100** |    **60.98 ms** |  **11.442 ms** |   **4.080 ms** |    **60.37 ms** |           **-** |           **-** |           **-** |           **550.55 KB** |
|    WithIndex |   1000 | 100 |    52.47 ms |  12.779 ms |   4.557 ms |    52.82 ms |           - |           - |           - |           552.98 KB |
| **WithoutIndex** | **100000** |   **3** | **1,238.00 ms** | **108.445 ms** |  **38.673 ms** | **1,231.95 ms** |           **-** |           **-** |           **-** |           **550.55 KB** |
|    WithIndex | 100000 |   3 |   255.94 ms | 195.307 ms |  69.649 ms |   253.62 ms |           - |           - |           - |           552.91 KB |
| **WithoutIndex** | **100000** |  **10** | **1,308.38 ms** | **396.732 ms** | **141.478 ms** | **1,230.25 ms** |           **-** |           **-** |           **-** |           **550.55 KB** |
|    WithIndex | 100000 |  10 |   212.47 ms |  27.087 ms |   9.659 ms |   216.63 ms |           - |           - |           - |           552.91 KB |
| **WithoutIndex** | **100000** | **100** | **1,235.40 ms** |  **45.326 ms** |  **16.164 ms** | **1,234.31 ms** |           **-** |           **-** |           **-** |           **550.55 KB** |
|    WithIndex | 100000 | 100 |   212.77 ms |  31.814 ms |  11.345 ms |   211.49 ms |           - |           - |           - |           552.91 KB |
