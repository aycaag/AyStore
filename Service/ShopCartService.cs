using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
public interface IShopCartService
{
    public Cart GetCart();
    public void AddToCart(CartItem item);
    public void DeleteFromCart(int productId);
    public void ClearCart();

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
        _httpContextAccessor.HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
    }

     public Cart GetCart()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var cart = session.GetObjectFromJson<Cart>(CartSessionKey)?? new Cart();
        return cart;

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

    public void ClearCart()
    {
        throw new NotImplementedException();
    }

    public void DeleteFromCart(int productId)
    {
        throw new NotImplementedException();
    }

   
}