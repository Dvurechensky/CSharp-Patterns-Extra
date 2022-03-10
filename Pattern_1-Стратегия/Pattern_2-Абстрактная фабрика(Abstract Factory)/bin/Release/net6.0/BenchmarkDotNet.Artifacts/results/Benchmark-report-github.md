``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19042.1415 (20H2/October2020Update)
Intel Core i5-9600K CPU 3.70GHz (Coffee Lake), 1 CPU, 6 logical and 6 physical cores
.NET SDK=6.0.200
  [Host]     : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT
  DefaultJob : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT


```
|         Method |      Mean |     Error |    StdDev |    Median | Rank | Allocated |
|--------------- |----------:|----------:|----------:|----------:|-----:|----------:|
| ArrayListBench | 23.203 ms | 2.8609 ms | 8.4355 ms | 25.529 ms |    2 |      2 KB |
|      ListBench |  6.062 ms | 0.2350 ms | 0.6855 ms |  6.106 ms |    1 |      2 KB |
