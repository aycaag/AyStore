using AutoMapper;
using Microsoft.AspNetCore.Mvc;

public class LoginController : BaseController
{
    private readonly ILoginService _loginService;
    private readonly IPasswordHelper _passwordHelper;
    public LoginController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService,
        ILoginService loginService,
        IPasswordHelper passwordHelper)
        : base(categoriesService, mapper, shopCartService)
    {
        _loginService = loginService;
        _passwordHelper = passwordHelper;
    }

    public async Task<IActionResult> Index()
    {

        ViewData["ActivePage"] = "Login";

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        string hashedPassword = _passwordHelper.HashPassword(model.Password);

        if (!ModelState.IsValid)
        {
            return View(model); // Hataları göstererek sayfayı tekrar yükler
        }

        try
        {
            bool userControl = _loginService.SignIn(model.Email, hashedPassword);
            if (userControl)
            {
                TempData["Success"] = "Giriş işlemi başarılı şekilde gerçekleşti!";
            }
            return RedirectToAction("Index");

        }
        catch (Exception ex)
        {

            Console.WriteLine("Giriş Hatası : {0}", ex.Message);
            TempData["Error"] = ex.Message != null ? ex.Message : "Geçersiz email veya şifre ! Tekrar deneyiniz";
            return View(model);
        }

    }


}