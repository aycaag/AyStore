using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public abstract class BaseController : Controller
{
    protected readonly ICategoriesService _categoriesService;
    protected readonly IMapper _mapper;

    protected readonly IShopCartService _shopCartService;

    public BaseController(ICategoriesService categoriesService, IMapper mapper, IShopCartService shopCartService)
    {
        _categoriesService = categoriesService;
        _mapper = mapper;
        _shopCartService = shopCartService;
    }

    // Her action çalışmadan önce bu metod çalışır.
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        // Oturum açmış mı kontrolü
        var token = Request.Cookies["JWToken"];

        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToList();

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            HttpContext.User = principal;
        }
        ///////////
        
        // Kategori verisini çekiyoruz
        var categories = await _categoriesService.GetCategories();
        var categoryViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
        // ViewBag'e atama yapıyoruz, böylece layout veya view'larda kullanabiliriz
        ViewBag.Categories = categoryViewModel;

        // Sepetteki ürün saysısını bulalım 
        int cartsCount = _shopCartService.GetCart().CartItems.Sum(x => x.Quantity);

        ViewBag.CartItemsCount = cartsCount;



        // Action'ın çalışmasına devam et
        await next();
    }
}
