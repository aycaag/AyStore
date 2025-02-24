public class Filters
{
    public string CategoryName { get; set; }
    public List<PriceFilters> Price { get; set; } = new List<PriceFilters>();
    public string PriceName { get; set; }
    public string Colors { get; set; }
}

