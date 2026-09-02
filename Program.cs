using MiniShop;
using MiniShop.Tests;

// "dotnet run -- test" runs the tests, plain "dotnet run" runs a small demo.
if (args.Length > 0 && args[0] == "test")
{
    return TestRunner.Run();
}

var cart = new List<Item>
{
    new("Notebook", 4.50m, 2),
    new("Pen", 1.25m, 4),
};
Console.WriteLine($"Subtotal: {Cart.Subtotal(cart)}");
return 0;
