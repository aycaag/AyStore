using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        IConfiguration configuration,
        IFilterService filterService)
        : base(categoriesService, mapper, shopCartService,filterService)
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
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        string hashedPassword = _passwordHelper.HashPassword(model.Password);

        if (!ModelState.IsValid)
        {
            return View(model); // Hataları göstererek sayfayı tekrar yükler
        }

        try
        {
            int? userID = _loginService.SignIn(model.Email, hashedPassword);


            if (userID != null)
            {
                var user = await _loginService.GetUserInfo(userID);
                var token = await GenerateJwtToken(userID);
                
                // Kullanıcı bilgilerini Session'ye kaydediyoruz
                HttpContext.Session.SetString("JWToken", token);
                if (userID==1)
                {
                Response.Cookies.Append("JWToken", token, new CookieOptions { HttpOnly = true, Secure = true }); // cookie'ye kaydetme 
                }
                
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


    private async Task<string> GenerateJwtToken(int? userID)
    {
        var JwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        User user = await _loginService.GetUserInfo(userID);

        string userFullName = user.Name + " " + user.LastName;


        var claims = new[]
       {
            new Claim(ClaimTypes.Name, userFullName),
            new Claim(ClaimTypes.NameIdentifier, userID.ToString() ),
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

    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Bütün session verilerini siler

        return RedirectToAction("Index");
    }



}