using AutoMapper;

public interface IProductService
{
    public  Task<ProductsDTO> GetAllProducts();
    public  Task<ProductDetailDTO> GetProductDetail(int id);

    public Task<ProductsDTO> GetAllProductbyCategory(string categoryName);
    
}

public class ProductService : IProductService
{
    private IWebApiRepository _webapiRepository;
    private IMapper _mapper;

    public ProductService(IWebApiRepository webapiRepository,IMapper mapper)
    {
        _webapiRepository = webapiRepository;
        _mapper = mapper;
    }

    public async Task<ProductsDTO> GetAllProductbyCategory(string categoryName)
    {
        var products =  await _webapiRepository.GetAllProductbyCategory(categoryName);

        ProductsDTO result = _mapper.Map<ProductsDTO>(products);

        return result;
    }

    public async Task<ProductsDTO> GetAllProducts()
    {
        var products =  await _webapiRepository.GetAllProducts();
        ProductsDTO result = _mapper.Map<ProductsDTO>(products);

        return result;
    }

    public async Task<ProductDetailDTO> GetProductDetail(int id)
    {
        var productDetail = await _webapiRepository.GetProductDetail(id);

        ProductDetailDTO result = _mapper.Map<ProductDetailDTO>(productDetail);

        return result;

    }

   
}