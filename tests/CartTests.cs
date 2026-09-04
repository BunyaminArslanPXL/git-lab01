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

    [Test]
    public static void DiscountIsAPercentage()
    {
        Check.Equal(80.00m, Cart.ApplyDiscount(100m, 20m));
        Check.Equal(14.00m, Cart.ApplyDiscount(14m, 0m));
    }

    [Test]
    public static void ShippingIsFreeForBigOrders()
    {
        Check.Equal(4.95m, Cart.Shipping(20m));
        Check.Equal(0m, Cart.Shipping(60m));
    }
}
