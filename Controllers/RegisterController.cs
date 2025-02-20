using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class RegisterController : BaseController
{
    public RegisterController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService)
        : base(categoriesService, mapper, shopCartService)
    {

    }

    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "Register";


        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(RegisterViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Hataları göstererek sayfayı tekrar yükler
            }
        }
        catch (Exception ex)
        {
            // Hata mesajını TempData'ya ekle
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Register");
        }
    return View(model); 



    }
}