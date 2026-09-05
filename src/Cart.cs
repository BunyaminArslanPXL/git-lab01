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

    /// <summary>Shipping costs: free from 50 euro, otherwise 4.95.</summary>
    public static decimal Shipping(decimal amount)
    {
        if (amount > 50)
            return 0m;
        return 4.95m;
    }

    /// <summary>Subtotal minus discount, plus shipping.</summary>
    public static decimal Total(IEnumerable<Item> items, decimal discount = 0)
    {
        var amount = ApplyDiscount(Subtotal(items), discount);
        return Math.Round(amount + Shipping(amount), 2);
    }
}
