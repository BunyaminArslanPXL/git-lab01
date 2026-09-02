namespace MiniShop.Tests;

public static class CartTests
{
    static readonly List<Item> CartItems = new()
    {
        new("Notebook", 4.50m, 2),
        new("Pen", 1.25m, 4),
    };

    [Test]
    public static void SubtotalIsSumOfPriceTimesQty() =>
        Check.Equal(14.00m, Cart.Subtotal(CartItems));
}
