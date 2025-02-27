public interface IAdminDashboardService
{
    public Task<int?> UserCount();
    public Task<int?> SalesCount();
    public Task<int?> Revenue();
    public Task<int?> TotalProductQuantity();
    public Task<int?> VisitCount();
    public Task<List<Order>> GetAllOrder();

    public Task<List<VisitSummary>> VisitSummaryGet(int lastDay,DateTime tarih);
}

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IAdminContextRepository _AdmincontextRepository;

    public AdminDashboardService(IAdminContextRepository adminContextRepository)
    {
        _AdmincontextRepository = adminContextRepository;
    }

    public async Task<int?> UserCount()
    {
        int? response = await _AdmincontextRepository.UserCount();
        
        return response;
        
    }
    public async Task<int?> SalesCount()
    {
        int? response = await _AdmincontextRepository.SalesCount();
        
        return response;
        
    }
    public async Task<int?> Revenue()
    {
        int? response = await _AdmincontextRepository.Revenue();
        
        return response;
    }
    
    public async Task<int?> TotalProductQuantity()
    {
        int? response = await _AdmincontextRepository.TotalProductQuantity();
        
        return response;
    }

    public async Task<int?> VisitCount()
    {
        int? response = await _AdmincontextRepository.VisitCount();
        
        return response;
    }

    public async Task<List<Order>> GetAllOrder()
    {
        List<Order> response = await _AdmincontextRepository.GetAllOrder();

        return response;
    }

    public async Task<List<VisitSummary>> VisitSummaryGet(int lastDay, DateTime tarih)
    {
        List<VisitSummary> response = await _AdmincontextRepository.VisitSummaryGet(lastDay,tarih);

        return response;
    }
}