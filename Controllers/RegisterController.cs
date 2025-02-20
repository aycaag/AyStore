using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class RegisterController : BaseController
{
    private readonly IRegisterService _registerService;
    public RegisterController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService,
        IRegisterService registerService)
        : base(categoriesService, mapper, shopCartService)
    {   
        _registerService = registerService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "Register";


        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(RegisterViewModel model)
    {
         if (!ModelState.IsValid)
            {
                return View(model); // Hataları göstererek sayfayı tekrar yükler
            }
        try
        {
            RegisterDTO registerdto = _mapper.Map<RegisterDTO>(model);
            await _registerService.Add(registerdto);

             // Kayıt başarılıysa, TempData'ya mesaj ekleyin
            TempData["Success"] = "Kayıt işlemi başarılı şekilde gerçekleşti!";
            return View("Index"); 

        }
        catch (Exception ex)
        {
            // Hata mesajını TempData'ya ekle
            TempData["Error"] = "Kayıt işlemi başarılı değil ! Tekrar deneyiniz";
            Console.WriteLine("Hata: {0}", ex.Message);
            return RedirectToAction("Index", "Home");
        }
  



    }
}