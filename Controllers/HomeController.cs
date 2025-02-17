using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AyStore.Models;
using AutoMapper;
using System.Threading.Tasks;
using RestSharp;

namespace AyStore.Controllers;

public class HomeController : BaseController
{
    private readonly ILogger<HomeController> _logger;
    // private readonly IMapper _mapper;
    private readonly IProductService _productService;

    public HomeController(
        IMapper mapper,
        IProductService productService,
        ICategoriesService categoriesService,
        IShopCartService shopCartService)
        :base(categoriesService,mapper,shopCartService)
    {
        _productService = productService;
        // _mapper = mapper;
        // _categoriesService = categoriesService;
    }

    public async Task<IActionResult> Index()
    {
        IndexViewModel model = new IndexViewModel();

        //Product
        var products = await _productService.GetAllProducts();
        var productViewModel = _mapper.Map<ProductViewModel>(products);
        model.ProductVM = productViewModel;


        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
