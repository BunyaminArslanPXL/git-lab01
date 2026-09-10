namespace MiniShop.Tests;

public static class ExportTests
{
    [Test]
    public static void CsvHasHeaderAndOneLinePerItem()
    {
        var items = new List<Item> { new("Pen", 1.25m, 4) };
        Check.Equal("name,price,qty\nPen,1.25,4\n", Export.ToCsv(items));
    }
}
