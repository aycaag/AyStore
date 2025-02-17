using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
namespace AyStore.Controllers;

public class ProductController: BaseController
{
    private readonly IProductService _productService;
    public ProductController
        (
            ICategoriesService categoriesService,
            IMapper mapper,
            IProductService productService,
            IShopCartService shopCartService
        )
        :base(categoriesService,mapper,shopCartService)
    {
       _productService = productService;
    }

    public async Task<ActionResult> Product()
    {
        ProductViewModel model = new ProductViewModel();
        //Product
        var products = await _productService.GetAllProducts();
        var productViewModel = _mapper.Map<ProductViewModel>(products);
        model.products = productViewModel.products;
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
    

}