using AutoMapper;
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
        var token = HttpContext.Session.GetString("JWTToken");

        // if (string.IsNullOrEmpty(token))
        // {
        //     // Kullanıcı giriş yapmamışsa, Login sayfasına yönlendir
        //     context.Result = RedirectToAction("Index", "Login");
        // }
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
