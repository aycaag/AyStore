using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using AyStore.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

public class ShopCartController : BaseController
{
    //private readonly IShopCartService _shopCartService;
    private readonly IProductService _productService;

    public ShopCartController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService,
        IProductService productService,
        IFilterService filterService
        )
        : base(categoriesService, mapper, shopCartService, filterService)
    {
        _productService = productService;
    }

    public ActionResult Index()
    {
        ViewData["ActivePage"] = "ShopCart";

        ShopCartViewModel model = new ShopCartViewModel();

        Cart cart = _shopCartService.GetCart();

        model.CartItems = cart.CartItems;
        //model.TotalPrice = cart.TotalPrice; 

        return View(model);
    }

    public async Task<IActionResult> AddToCart(int productId, string returnUrl,int quantity)
    //  (int productId,string productBrand ,string productModel, decimal price, decimal previousPrice,int quantity)
    {

        ProductDetailDTO productDTO = await _productService.GetProductDetail(productId);
        Product product = productDTO.product;

        // int quantity = 1;
        var item = new CartItem
        {
            ProductId = productId,
            Product = product,

            Quantity = quantity
        };

        _shopCartService.AddToCart(item);
        //return RedirectToAction("Index","Home");

        // Sepetin toplam öğe sayısını al
        var totalItems = _shopCartService.GetCart().CartItems.Sum(x => x.Quantity);

        return Json(new { success = true, message = "Ürün sepete eklendi!", totalItems = totalItems, returnUrl = returnUrl });

    }

    public async Task<IActionResult> RemoveFromCart(int productId)
    {

        _shopCartService.RemoveFromCart(productId);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DecreaseQuantityOfProduct(int productId)
    {
        _shopCartService.DecreaseQuantityOfProduct(productId);
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> IncreaseQuantityOfProduct(int productId)
    {
        _shopCartService.IncreaseQuantityOfProduct(productId);
        return RedirectToAction("Index");
    }

    public JsonResult GetCartCount()
    {
        var cart = _shopCartService.GetCart(); // Sepeti al
        var totalItems = cart.CartItems.Sum(item => item.Quantity); // Miktarları topla
        return Json(new { totalItems = totalItems }); // Toplam sayıyı döndür
    }
}