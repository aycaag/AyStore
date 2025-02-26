public interface ICheckoutService 
{
    public Task<bool> AddOrderNumber(string orderNo,int? userId);

    public Task<bool> AddOrder (Order order);

}

public class CheckoutService: ICheckoutService
{
    private readonly IWebContextRepository _webContextRepository;
    public CheckoutService(IWebContextRepository webContextRepository)
    {
        _webContextRepository = webContextRepository;
        
    }

    public async Task<bool> AddOrderNumber(string orderNo,int? userId)
    {
        bool response =await _webContextRepository.AddOrderNumber(orderNo, userId);

        return response;

    }

    public async Task<bool> AddOrder(Order order)
    {
        bool response = await _webContextRepository.AddOrder(order);

    return response;
    }

}