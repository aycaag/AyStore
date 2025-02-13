public class ShopCartViewModel
{
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();

    public decimal TotalPrice => CartItems.Sum(item => item.Product.price * item.Quantity);

}