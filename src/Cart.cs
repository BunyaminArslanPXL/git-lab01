namespace MiniShop;

public record Item(string Name, decimal Price, int Qty);

public static class Cart
{
    /// <summary>Sum of price * qty for every item in the cart.</summary>
    public static decimal Subtotal(IEnumerable<Item> items) =>
        Math.Round(items.Sum(i => i.Price * i.Qty), 2);
}
