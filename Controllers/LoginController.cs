using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class LoginController : BaseController
{
    public LoginController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService)
        : base(categoriesService, mapper, shopCartService)
    {

    }

public async Task<IActionResult> Index()
{

    ViewData["ActivePage"] = "Login";
    
    return View();
}
}