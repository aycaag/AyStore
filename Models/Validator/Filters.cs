public class Filters
{
    public string CategoryName { get; set; }
    public List<PriceFilters> Price { get; set; } = new List<PriceFilters>();
    public string PriceName { get; set; }
    public List<string> Colors { get; set; } = new List<string>();
    public string ColorName { get; set; }
}

