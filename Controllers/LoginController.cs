using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

public class LoginController : BaseController
{
    private readonly ILoginService _loginService;
    private readonly IPasswordHelper _passwordHelper;
    private readonly IConfiguration _configuration;
    public LoginController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService,
        ILoginService loginService,
        IPasswordHelper passwordHelper,
        IConfiguration configuration)
        : base(categoriesService, mapper, shopCartService)
    {
        _loginService = loginService;
        _passwordHelper = passwordHelper;
        _configuration = configuration;
    }

    public IActionResult Index()
    {

        ViewData["ActivePage"] = "Login";

        return View();
    }

    [HttpPost]
    public IActionResult Index(LoginViewModel model)
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
                // JWT Token oluşturma 
                var token = GenerateJwtToken(model.Email);
                // Token'ı session ile oturumda sakla
                HttpContext.Session.SetString("JWTToken", token);
                // Mesajı ekrana gönder 
                TempData["Success"] = "Giriş işlemi başarılı şekilde gerçekleşti!";

                // Anasayfayı aç
                return RedirectToAction("Index", "Home");

            }

            return View(model);

        }
        catch (Exception ex)
        {

            Console.WriteLine("Giriş Hatası : {0}", ex.Message);
            TempData["Error"] = ex.Message != null ? ex.Message : "Geçersiz email veya şifre ! Tekrar deneyiniz";
            return View(model);
        }

    }


    private string GenerateJwtToken(string username)
    {
        var JwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
        new Claim(ClaimTypes.Name, username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var token = new JwtSecurityToken(
            issuer: JwtSettings["Issuer"],
            audience: JwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(30),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


}