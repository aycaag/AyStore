public interface IAdminContextRepository
{

    public Task<int?> UserCount ();
    public Task<int?> SalesCount ();
    public Task<int?> Revenue();
    public Task<int?> TotalProductQuantity ();

}

public class AdminContextRepository : IAdminContextRepository
{
    private readonly AyStoreContext _ayStoreContext;

    public AdminContextRepository(
        AyStoreContext ayStoreContext
        )
    {
        _ayStoreContext = ayStoreContext;
        
    }

    public async Task<int?> UserCount ()
    {

       int? userCount =  _ayStoreContext.Users.Select(u=>u.Id).Count() ;
    
       userCount  = userCount==null?0:userCount;

       return userCount;
    }

    public async Task<int?> SalesCount()
    {
        int? salesCount = _ayStoreContext.OrderNumbers.Select(s=>s.OrderNo).Count();

        salesCount = salesCount==null?0:salesCount;

        return salesCount;
    }

    public async Task<int?> Revenue()
    {
        int? revenue = _ayStoreContext.Order.Select(s=>s.TotalPrice).Sum();

        revenue = revenue==null?0:revenue;
        return revenue;
    }
    
     public async Task<int?> TotalProductQuantity()
    {
        int? totalQuantity = _ayStoreContext.Order.Select(s=>s.Quantity).Sum();

        totalQuantity = totalQuantity==null?0:totalQuantity;
        return totalQuantity;
    }
}