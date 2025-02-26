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
    private readonly ICheckoutService _checkoutService;

    public CheckoutController(
        ICategoriesService categoriesService,
        IMapper mapper,
        IShopCartService shopCartService,
        IFilterService filterService,
        ILoginService loginService,
        IHttpContextAccessor httpContextAccessor,
        ICheckoutService checkoutService)
        : base(categoriesService, mapper, shopCartService, filterService)
    {
        _loginService = loginService;
        _httpContextAccessor = httpContextAccessor;
        _checkoutService = checkoutService;

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

        // önce token bakıyoruz. token yoksa login ekranına gönderiyoruz. 
        // token varsa modeli dolduruyoruz.
        // eğer model is valid değilse modele geri gönderiyoruz. 
        // isvalid ise sipariş oluşturuyoruz ana sayfaya gönderiyoruz. 


        var token = HttpContext.Session.GetString("JWToken");

        int? userId = 0;

        if (token != null)
        {

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

            }
            else
            {
                throw new Exception("UserID bulunamadı ' ");
            }

        }
        else if (token == null) // token yoksa kullanıcı giriş yapmamıştır giriş yapılmasını zorluyoruz.
        {
            return RedirectToAction("Index", "Login");
        }

        if (!ModelState.IsValid)
        {
            // hataları console'a yazdıralım kontrol için 
            // foreach (var key in ModelState.Keys)
            // {
            //     var errors = ModelState[key].Errors;
            //     foreach (var error in errors)
            //     {
            //         Console.WriteLine($"Hata: {key} => {error.ErrorMessage}");
            //     }
            // }
            return View(model);
        }

        // herşey yolundaysa sipariş başarılıdır. Ancak burada database'e yazmamız gerekir.


        // Başarılı siparişleri database'e ekleyelim 

        DateTime simdi = DateTime.Now;

        // Random sipariş no oluşturalım : 
        Random random = new Random(); // Yeni bir Random nesnesi oluştur
        string harf = (Convert.ToChar(random.Next(97, 122)) + Convert.ToChar(random.Next(97, 122))).ToString();
        string sayi = random.Next(1234, 10000).ToString();
        string orderNo = '#' + harf + sayi;
        Console.WriteLine("Orderno : {0}" + orderNo);
        //// 

        ///Sipariş numarasını OrderNumber'a gönderip kaydedelim 
        await _checkoutService.AddOrderNumber(orderNo, userId);

        /// Siparşi Order tablosuna kaydedelim.       
        if (model.shopcartVM.CartItems.Count > 0)   
        {

            foreach ( var item in model.shopcartVM.CartItems )
            {
                Order order = new Order();

                order.ProductName = item.Product.brand.ToUpper() + item.Product.model.ToUpper();
                order.Price = item.Product.price;
                order.Quantity = item.Quantity;
                order.TotalPrice = item.Quantity*item.Product.price;
                order.OrderDate = simdi;
                order.OrderNo = orderNo;
                order.UserID = userId;


            await _checkoutService.AddOrder(order);

            };

        }

        @TempData["Success"] = "Sipariş başarıyla oluşturulmuştur! Sipariş No :"+orderNo;
        // HttpContext.Session.Remove(CartSessionKey);
        _httpContextAccessor.HttpContext.Session.Remove("Cart");

        return RedirectToAction("Index", "Home");

    }

}