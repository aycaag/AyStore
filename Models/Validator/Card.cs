public class CartItem
{
    public int ProductId { get; set; }
    // public string? ProductBrand { get; set; }
    // public string? ProductModel { get; set; }
    // public decimal Price{ get; set; }
    // public decimal previousPrice { get; set; }
    public Product Product{ get; set; }
    public int Quantity { get; set; }
}

public class Cart
{
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    public decimal TotalPrice => CartItems.Sum(item => item.Product.price * item.Quantity);
}