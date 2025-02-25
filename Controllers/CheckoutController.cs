using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class CheckoutController : BaseController
{
    private readonly ILoginService _loginService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CheckoutController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService,
        IFilterService filterService,
        ILoginService loginService,
        IHttpContextAccessor httpContextAccessor)
        : base(categoriesService, mapper, shopCartService, filterService)
    {
        _loginService = loginService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> Index()
    {

        ViewData["ActivePage"] = "Checkout";

        CheckoutViewModel model = new CheckoutViewModel();

        var token = HttpContext.Session.GetString("JWToken");

        if (token != null)
        {
            int? userId;

            // kullanıcı doğrulamak için 
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            // 
            if (userIdClaim != null)
            {
                userId = int.Parse(userIdClaim.Value);
                // Kullanıcı ID'ye göre işlemler yapılabilir

                User user = await _loginService.GetUserInfo(userId);
                model.user = user;

                Address address = await _loginService.GetAddressInfo(userId);
                model.address = address;

                Login login = await _loginService.GetLoginInfo(userId);
                model.login = login;


                Cart cart = _shopCartService.GetCart();
                model.shopcartVM.CartItems = cart.CartItems;

                return View(model);

            }
            else
            {
                throw new Exception("User ID bulunamadı ! ");
            }
        }

        else
        {
            return RedirectToAction("Index", "Login");
        }


    }

    [HttpPost]
    public async Task<IActionResult> Index(CheckoutViewModel model)
    {
        model.user = new User();
        model.address = new Address();
        model.login = new Login();
        

        if (!ModelState.IsValid)
        {
            
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Hata: {key} => {error.ErrorMessage}");
                    }
                }
            

            var token = HttpContext.Session.GetString("JWToken");

            if (token != null)
            {
                int? userId;

                // kullanıcı doğrulamak için 
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                // 
                if (userIdClaim != null)
                {
                    userId = int.Parse(userIdClaim.Value);
                    // Kullanıcı ID'ye göre işlemler yapılabilir

                    User user = await _loginService.GetUserInfo(userId);
                    model.user = user;

                    Address address = await _loginService.GetAddressInfo(userId);
                    model.address = address;

                    Login login = await _loginService.GetLoginInfo(userId);
                    model.login = login;


                    Cart cart = _shopCartService.GetCart();
                    model.shopcartVM.CartItems = cart.CartItems;

                    return View(model);
                }
                else
                {
                    throw new Exception("UserID bulunamadı ' ");
                }

            }

            return RedirectToAction("Index", "Login");

        }

        @TempData["Success"] = "Sipariş başarıyla oluşturulmuştur!";

        // HttpContext.Session.Remove(CartSessionKey);
        _httpContextAccessor.HttpContext.Session.Remove("Cart");

        return RedirectToAction("Index", "Home");

    }

}