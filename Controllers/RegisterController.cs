using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class RegisterController : BaseController
{
    private readonly IRegisterService _registerService;
    private readonly IPasswordHelper _passwordHelper;   
    public RegisterController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService,
        IRegisterService registerService,
        IPasswordHelper passwordHelper)
        : base(categoriesService, mapper, shopCartService)
    {   
        _registerService = registerService;
        _passwordHelper = passwordHelper;
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

            string hashedPassword = _passwordHelper.HashPassword(model.Login.Password); 
            string hashedConfirmPassword = _passwordHelper.HashPassword(model.Login.ConfirmPassword); 

            model.Login.Password = hashedPassword;
            model.Login.ConfirmPassword = hashedConfirmPassword;    

            RegisterDTO registerdto = _mapper.Map<RegisterDTO>(model);
            await _registerService.Add(registerdto);

             // Kayıt başarılıysa, TempData'ya mesaj ekleyin
            TempData["Success"] = "Kayıt işlemi başarılı şekilde gerçekleşti!";
            return RedirectToAction("Index","Login"); 

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