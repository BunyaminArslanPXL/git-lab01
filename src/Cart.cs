namespace MiniShop;

public record Item(string Name, decimal Price, int Qty);

public static class Cart
{
    /// <summary>Sum of price * qty for every item in the cart.</summary>
    public static decimal Subtotal(IEnumerable<Item> items) =>
        Math.Round(items.Sum(i => i.Price * i.Qty), 2);

    /// <summary>Amount after a percentage discount (0-100).</summary>
    public static decimal ApplyDiscount(decimal amount, decimal percent) =>
        Math.Round(amount * (1 - percent / 100), 2);
}
