# Mini Shop

A tiny shopping cart library, used in the Software Engineering course.

Needs the .NET 8 SDK or newer.

- Run the demo: `dotnet run`
- Run the tests: `dotnet run -- test`

## Features
- Subtotal of a cart
- Percentage discounts
- Free shipping from 50 euro
- Total price (incl. shipping)

## Usage
```csharp
using MiniShop;

var cart = new List<Item> { new("Pen", 1.25m, 4) };
Console.WriteLine(Cart.Total(cart, discount: 10));
```
