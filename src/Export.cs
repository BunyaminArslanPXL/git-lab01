using System.Globalization;
using System.Text;

namespace MiniShop;

public static class Export
{
    /// <summary>The cart as CSV text with a header row.</summary>
    public static string ToCsv(IEnumerable<Item> items)
    {
        var csv = new StringBuilder();
        csv.Append("name,price,qty\n");
        foreach (var i in items)
        {
            // Invariant culture: always "1.25", also on a Belgian (nl-BE) machine.
            var price = i.Price.ToString(CultureInfo.InvariantCulture);
            csv.Append($"{i.Name},{price},{i.Qty}\n");
        }
        return csv.ToString();
    }
}
