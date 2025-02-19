using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class CheckoutController : BaseController
{
    public CheckoutController(
        ICategoriesService categoriesService, 
        IMapper mapper,
        IShopCartService shopCartService) 
        : base(categoriesService, mapper, shopCartService)
    {
    }

    public async Task<IActionResult> Index()
    {

        ViewData["ActivePage"] = "Checkout";
        
        return View();
    }
}