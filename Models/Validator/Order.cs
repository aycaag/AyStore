public class Order 
{
    public int ID { get; set; }
    public int? UserID { get; set; }
    public string OrderNo { get; set; }
    public DateTime OrderDate { get; set; } 
    public string ProductName { get; set; }
    public decimal? Price { get; set; }
    public int Quantity { get; set; }
    public decimal? TotalPrice { get; set; } 
}