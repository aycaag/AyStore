using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using AyStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

public class ShopCartController : BaseController
{
    //private readonly IShopCartService _shopCartService;
    private readonly IProductService _productService;

    public ShopCartController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService,
        IProductService productService
        )
        : base(categoriesService, mapper,shopCartService)
    {
        _productService = productService;
    }

    public ActionResult Index()
    {
        ViewData["ActivePage"] = "ShopCart";

        ShopCartViewModel model= new ShopCartViewModel();

        Cart cart = _shopCartService.GetCart();

        model.CartItems = cart.CartItems;
        //model.TotalPrice = cart.TotalPrice; 

        return View(model);
    }

     public async Task<IActionResult> AddToCart(int productId,string returnUrl)
    //  (int productId,string productBrand ,string productModel, decimal price, decimal previousPrice,int quantity)
    {

        ProductDetailDTO productDTO = await _productService.GetProductDetail(productId);
        Product product=productDTO.product;

        int quantity = 1;
        var item = new CartItem
        {
            ProductId = productId,
            Product = product,
            
            Quantity = quantity
        };

        _shopCartService.AddToCart(item);
        //return RedirectToAction("Index","Home");
        return Json(new { success = true, message = "Ürün sepete eklendi!", totalItems = _shopCartService.GetCart().CartItems.Count , returnUrl = returnUrl });
    }

    public async Task<IActionResult> RemoveFromCart (int productId)
    {

        _shopCartService.RemoveFromCart(productId);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DecreaseQuantityOfProduct (int productId  )
    {

        _shopCartService.DecreaseQuantityOfProduct(productId);
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> IncreaseQuantityOfProduct (int productId)
    {
        _shopCartService.IncreaseQuantityOfProduct(productId);
        return RedirectToAction("Index");
    }
}