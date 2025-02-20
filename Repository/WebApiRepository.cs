using System.Reflection;

using Newtonsoft.Json;
using RestSharp;

public interface IWebApiRepository
{
    public  Task<ProductsDMO> GetAllProducts();

    public Task<ProductDetailDMO> GetProductDetail(int id);   
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