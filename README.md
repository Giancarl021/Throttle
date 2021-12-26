# Throttle

![](assets/icon.png)

Throttle multiple asynchronous functions

## Installation

You can get the package on the [Nuget](https://www.nuget.org/packages/Throttle/) website.

### Usage

First you need to add the `Throttle` namespace to your project.

```csharp
using Throttle;
```

Then you will have access to the `Throttler` class, which have two overloaded static methods: `Task Throttler.Throttle(IEnumerable<Func<Task>> callbacks, int ticks)` and `Task<IEnumerable<T>> Throttler.Throttle<T>(IEnumerable<Func<Task<T>>> callbacks, int ticks)`.

#### `Task Throttler.Throttle(IEnumerable<Func<Task>> callbacks, int ticks)`

This method is used to throttle asynchronous functions that does not return any value.

Example:

```csharp
var callbacks = new List<Func<Task>>
{
    async () => { await Task.Delay(1000); Console.WriteLine("Callback 1"); },
    async () => { await Task.Delay(1000); Console.WriteLine("Callback 2"); },
    async () => { await Task.Delay(2000); Console.WriteLine("Callback 3"); },
    async () => { await Task.Delay(2000); Console.WriteLine("Callback 4"); },
    async () => { await Task.Delay(3000); Console.WriteLine("Callback 5"); },
    async () => { await Task.Delay(3000); Console.WriteLine("Callback 6"); },
    async () => { await Task.Delay(10000); Console.WriteLine("Callback 7"); }
};

await Throttler.Throttle(callbacks, 2);
```

Will result in something like:

```bash
$ dotnet run
# After ~1 second
Callback 2
Callback 1
# After ~2 seconds
Callback 4
Callback 3
# After ~3 seconds
Callback 6
Callback 5
# After ~10 seconds
Callback 7
```

#### `Task<IEnumerable<T>> Throttler.Throttle<T>(IEnumerable<Func<Task<T>>> callbacks, int ticks)`

This method is used to throttle asynchronous functions that does return a value, specified in the method generic.

Example:

```csharp
var callbacks = new List<Func<Task<int>>>
{
    async () => { await Task.Delay(1000); Console.WriteLine("Callback 1"); return 1; },
    async () => { await Task.Delay(1000); Console.WriteLine("Callback 2"); return 2; },
    async () => { await Task.Delay(2000); Console.WriteLine("Callback 3"); return 3; },
    async () => { await Task.Delay(2000); Console.WriteLine("Callback 4"); return 4; },
    async () => { await Task.Delay(3000); Console.WriteLine("Callback 5"); return 5; },
    async () => { await Task.Delay(3000); Console.WriteLine("Callback 6"); return 6; },
    async () => { await Task.Delay(10000); Console.WriteLine("Callback 7"); return 7; }
};

System.Console.WriteLine(string.Join(", ", await Throttler.Throttle(callbacks, 2)));
```

Will result in something like:

```bash
$ dotnet run
# After ~1 second
Callback 2
Callback 1
# After ~2 seconds
Callback 4
Callback 3
# After ~3 seconds
Callback 6
Callback 5
# After ~10 seconds
Callback 7
# After finished
1, 2, 3, 4, 5, 6, 7
```