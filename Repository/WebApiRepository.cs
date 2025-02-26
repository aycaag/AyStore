using System.Reflection;

using Newtonsoft.Json;
using RestSharp;

public interface IWebApiRepository
{
    public Task<ProductsDMO> GetAllProducts();

    public Task<ProductDetailDMO> GetProductDetail(int id);

    public Task<ProductsDMO> GetAllProductbyCategory(string categoryName);

    public Task<ColorsFilters> GetProductColorsAsync();

    public Task<ProductsDMO> GetProductbySearch(string searchTerm);
}
public class WebApiRepository : IWebApiRepository
{
    RestClientOptions options = new RestClientOptions("https://fakestoreapi.in/api/");
    RestClient client;

    // private readonly AyStoreContext _ayStoreContext;
    public WebApiRepository()
    {
        client = new RestClient(options);
    }

    public async Task<ProductsDMO> GetAllProductbyCategory(string categoryName)
    {
        var request = new RestRequest("products/category?type=" + categoryName, Method.Get);
        var response = await client.GetAsync(request);

        ProductsDMO result = JsonConvert.DeserializeObject<ProductsDMO>(response.Content);
        return result;
    }

    public async Task<ProductsDMO> GetAllProducts()
    {
        var request = new RestRequest("products?limit=150", Method.Get);
        var response = await client.GetAsync(request);

        ProductsDMO result = JsonConvert.DeserializeObject<ProductsDMO>(response.Content);
        return result;
    }

    public async Task<ProductsDMO> GetProductbySearch(string searchTerm)
    {
        var request = new RestRequest("products", Method.Get);
        var response = await client.GetAsync(request);

        ProductsDMO result = JsonConvert.DeserializeObject<ProductsDMO>(response.Content);
        
        result.products = result.products.Where(x => x.brand.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase) || x.model.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

        return result;        
    }

    public async Task<ProductDetailDMO> GetProductDetail(int id)
    {
        var request = new RestRequest("products/" + id, Method.Get);
        var response = await client.GetAsync(request);

        ProductDetailDMO result = JsonConvert.DeserializeObject<ProductDetailDMO>(response.Content);
        return result;
    }


    public async Task<ColorsFilters> GetProductColorsAsync()
    {
        ProductsDMO productsDMO = await GetAllProducts();

    
        var colors = productsDMO.products
                 .Where(p => !string.IsNullOrEmpty(p.color)) // Boş renkleri filtrele
                 .Select(p => p.color.ToUpper())             
                 .Distinct()
                 .ToList();

    return new ColorsFilters { Colors = colors };
    }




}