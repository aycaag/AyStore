using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestSharp;

public interface IWebApiRepository
{
    public  Task<ProductsDMO> GetAllProducts();

    public Task<List<CategoriesDMO>> GetCategories(); 

    public Task<ProductDetailDMO> GetProductDetail(int id);   
}
public class WebApiRepository : IWebApiRepository
{
    RestClientOptions options = new RestClientOptions("https://fakestoreapi.in/api/");
    RestClient client;

    private readonly AyStoreContext _ayStoreContext;
    public WebApiRepository(AyStoreContext ayStoreContext)
    {
        _ayStoreContext = ayStoreContext;
        client = new RestClient(options);
    }

    public async Task<List<CategoriesDMO>> GetCategories()
    {
        // var request = new RestRequest("products/category", Method.Get);
        // var response = await client.GetAsync(request);
        // CategoriesDMO result = JsonConvert.DeserializeObject<CategoriesDMO>(response.Content);
        // return result; 

        var response =  await _ayStoreContext.Category.Select(s => new CategoriesDMO
        {
            Id = s.Id,
            Name = s.Name,
            Image = s.Image,


        }).ToListAsync();

        return response;

    }

    public async Task<ProductsDMO> GetAllProducts()
    {
        var request = new RestRequest("products",Method.Get);
        var response =  await client.GetAsync(request);

        ProductsDMO result = JsonConvert.DeserializeObject<ProductsDMO>(response.Content);
        return result;
    }

        public async Task<ProductDetailDMO> GetProductDetail(int id)
    {
        var request = new RestRequest("products/"+id,Method.Get);
        var response =  await client.GetAsync(request);

        ProductDetailDMO result = JsonConvert.DeserializeObject<ProductDetailDMO>(response.Content);
        return result;
    }



}