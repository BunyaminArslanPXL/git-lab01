namespace MiniShop;

public static class Search
{
    /// <summary>All items whose name contains the query (case-insensitive).</summary>
    public static List<Item> FindItems(IEnumerable<Item> items, string query)
    {
        var q = query.Trim().ToLowerInvariant();
        return items.Where(i => i.Name.ToLowerInvariant().Contains(q)).ToList();
    }
}
