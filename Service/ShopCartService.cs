using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
public interface IShopCartService
{
    public Cart GetCart();
    public void AddToCart(CartItem item);
    public void RemoveFromCart(int productId);
    public void DecreaseQuantityOfProduct(int productId);
    public void IncreaseQuantityOfProduct(int productId);

}


public class ShopCartService : IShopCartService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CartSessionKey = "Cart";
    public ShopCartService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private void SaveCart(Cart cart)
    {
         Console.WriteLine("Sepet kaydediliyor... Ürün sayısı: " + cart.CartItems.Count);
        _httpContextAccessor.HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
    }

    public Cart GetCart()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var cart = session.GetObjectFromJson<Cart>(CartSessionKey);

        Console.WriteLine(cart != null ? "Sepet oturumdan alındı." : "Sepet boş, yeni oluşturuluyor.");

        return cart ?? new Cart();

    }
    public void AddToCart(CartItem item)
    {
        var cart = GetCart();
        var existItem = cart.CartItems.FirstOrDefault(x => x.ProductId == item.ProductId);

        if (existItem != null)
        {
            existItem.Quantity += item.Quantity;
            item.Quantity += item.Quantity;
        }
        else
        {
            cart.CartItems.Add(item);
        }

        SaveCart(cart);
    }

    public void DecreaseQuantityOfProduct(int productId)
    {
        var cart = GetCart();
        // var existItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
        CartItem cartItem = cart.CartItems.FirstOrDefault(x=>x.ProductId==productId);  

        cartItem.Quantity --;

        if (cartItem.Quantity == 0 )
        {
            cart.CartItems.RemoveAll(x => x.ProductId == productId);
        }
        

        SaveCart(cart);

    }

    public void IncreaseQuantityOfProduct(int productId)
    {
        var cart = GetCart();
        // var existItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);
        CartItem cartItem = cart.CartItems.FirstOrDefault(x=>x.ProductId==productId);  

        cartItem.Quantity ++;

    
        SaveCart(cart);

    }
    public void RemoveFromCart(int productId)
    {
        var cart = GetCart();
        
        // Console.WriteLine("Sepetteki ürün sayısı önce : {0}",cart.CartItems.Count());

        cart.CartItems.RemoveAll(x => x.ProductId == productId);

        // Console.WriteLine("Sepetteki ürün sayısı sonra : {0}",cart.CartItems.Count());

        SaveCart(cart);
    }


}