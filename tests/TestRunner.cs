using System.Reflection;

namespace MiniShop.Tests;

/// <summary>Marks a public static method as a test.</summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class TestAttribute : Attribute { }

public static class Check
{
    public static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new Exception($"expected <{expected}> but got <{actual}>");
    }
}

/// <summary>Tiny test runner: finds every [Test] method in this project and runs it.</summary>
public static class TestRunner
{
    public static int Run()
    {
        var tests = Assembly.GetExecutingAssembly().GetTypes()
            .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .Where(m => m.GetCustomAttribute<TestAttribute>() != null)
            .OrderBy(m => m.DeclaringType!.Name)
            .ThenBy(m => m.Name)
            .ToList();

        var failed = 0;
        foreach (var test in tests)
        {
            var name = $"{test.DeclaringType!.Name}.{test.Name}";
            try
            {
                test.Invoke(null, null);
                Console.WriteLine($"  PASS  {name}");
            }
            catch (TargetInvocationException e)
            {
                failed++;
                Console.WriteLine($"  FAIL  {name}: {e.InnerException?.Message}");
            }
        }

        Console.WriteLine();
        Console.WriteLine(failed == 0
            ? $"OK ({tests.Count} tests)"
            : $"FAILED ({failed} of {tests.Count} tests)");
        return failed == 0 ? 0 : 1;
    }
}
