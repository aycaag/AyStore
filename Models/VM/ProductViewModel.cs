public class ProductViewModel
{
    public string status { get; set; }
    public string message { get; set; }
    public List<Product> products { get; set; }  
    public Filters filters{ get; set;} = new Filters { };
}

