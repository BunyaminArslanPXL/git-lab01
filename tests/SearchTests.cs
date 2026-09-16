namespace MiniShop.Tests;

public static class SearchTests
{
    static readonly List<Item> Items = new()
    {
        new("Notebook", 4.50m, 2),
        new("Pen", 1.25m, 4),
        new("Pencil", 0.80m, 10),
    };

    [Test]
    public static void SearchIsCaseInsensitive()
    {
        var names = Search.FindItems(Items, "pen").Select(i => i.Name);
        Check.Equal("Pen,Pencil", string.Join(",", names));
    }

    [Test]
    public static void NoMatchGivesEmptyList() =>
        Check.Equal(0, Search.FindItems(Items, "  stapler ").Count);
}
