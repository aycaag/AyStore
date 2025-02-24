using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
namespace AyStore.Controllers;

public class ProductController : BaseController
{
    private readonly IProductService _productService;
    public ProductController
        (
            ICategoriesService categoriesService,
            IMapper mapper,
            IProductService productService,
            IShopCartService shopCartService,
            IFilterService filterService
        )
        : base(categoriesService, mapper, shopCartService,filterService)
    {
        _productService = productService;
    }

    public async Task<ActionResult> Index()
    {
        ViewData["ActivePage"] = "Product";

        ProductViewModel model = new ProductViewModel();
        //Product
        var products = await _productService.GetAllProducts();
        var productViewModel = _mapper.Map<ProductViewModel>(products);
        model.products = productViewModel.products;


        // Filtre alanındaki price verisini çekelim ;
        List<PriceFiltersDTO> priceListDTOs = await _filterService.GetPriceFilters();
        List<PriceFilters> priceList = _mapper.Map<List<PriceFilters>>(priceListDTOs);
        model.filters.Price = priceList;
        
        return View(model);
    }


    public async Task<ActionResult> ProductDetail(int id)
    {
        ProductDetailViewModel model = new ProductDetailViewModel();
        // Product Detail
        var productDetail = await _productService.GetProductDetail(id);
        var productDetailModel = _mapper.Map<ProductDetailViewModel>(productDetail);
        model.product = productDetailModel.product;

        return View(model);
    }

    [HttpGet]
    public async Task<ActionResult> ProductbyCategory(string categoryName)
    {
        if (categoryName == null || categoryName == "all")
        {
            return RedirectToAction("Index");
        }
        ViewData["ActivePage"] = "Product";
        ViewData["ActiveCategory"] = categoryName;

        categoryName = categoryName.ToLower().Replace("ı", "i");

        ProductViewModel model = new ProductViewModel();

        var productsbyCategory = await _productService.GetAllProductbyCategory(categoryName);
        var productViewModel = _mapper.Map<ProductViewModel>(productsbyCategory);
        model.products = productViewModel.products;

        return View("Index", productViewModel);

    }


    [HttpPost]
    public async Task<ActionResult> FilterbyProduct(Filters filters)
    {
        
        string categoryName = filters.CategoryName ?? "all";
        string price = filters.PriceName  ?? "all";
        string color = filters.Colors ?? "all";


        // hiç filtreleme yapılmadıysa 

        if ((categoryName == null || categoryName == "all") && price=="all"  && color == "all")
        {
            return RedirectToAction("Index");
        }

        // Filtreleme yapıldıysa ;

        ViewData["ActivePage"] = "Product";
        ViewData["ActiveCategory"] = categoryName;

        categoryName = categoryName.ToLower().Replace("ı", "i");

        ProductViewModel model = new ProductViewModel();


        // Filtre alanındaki price verisini çekelim ;
        List<PriceFiltersDTO> priceListDTOs = await _filterService.GetPriceFilters();
        List<PriceFilters> priceList = _mapper.Map<List<PriceFilters>>(priceListDTOs);
        model.filters.Price = priceList;
        

        ProductsDTO product;
        ProductViewModel productViewModel;

        if (categoryName == "all")
        {
            product = await _productService.GetAllProducts();
        }
        else
        {
            product = await _productService.GetAllProductbyCategory(categoryName);
        }

        //2. filtre
        if(price != "all")
        {            

           ViewData["ActivePriceFilter"] = filters.PriceName;
           int minPrice = Convert.ToInt32(priceList.Where(x=>x.Name == filters.PriceName).Select(s=>s.MinPrice).FirstOrDefault()) ;
           int maxPrice = Convert.ToInt32(priceList.Where(x=>x.Name == filters.PriceName).Select(s=>s.MaxPrice).FirstOrDefault()) ;
           product.products = product.products.Where(s=>s.price >= minPrice && s.price<=maxPrice).ToList();        
        } 

        //3.filtre

        productViewModel = _mapper.Map<ProductViewModel>(product);
        model.products = productViewModel.products;
        model.filters.Price = priceList;

        return View("Index", model);

    }




}