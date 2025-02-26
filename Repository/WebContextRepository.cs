using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public interface IWebContextRepository
{
    public Task<List<CategoriesDMO>> GetCategories();

    public Task<RegisterDMO> AddRegister(RegisterDMO registerDMOs);

    public int? SignIn(string email, string password);

    public Task<User> GetUserInfo(int? userID);
    public Task<Address> GetAddressInfo(int? userID);
    public Task<Login> GetLoginInfo(int? userID);
    public Task<bool> AddOrderNumber (string orderNumber, int? userID);

    public Task<bool> AddOrder (Order order);

    public Task<List<PriceFiltersDMO>> GetAllPriceFilters();
}

public class WebContextRepository : IWebContextRepository
{

    private readonly AyStoreContext _ayStoreContext;

    public WebContextRepository(AyStoreContext ayStoreContext)
    {
        _ayStoreContext = ayStoreContext;
    }

    public async Task<RegisterDMO> AddRegister(RegisterDMO registerDMOs)
    {
        int lastUserID = _ayStoreContext.Login.Max(x => (int?)x.UserId) ?? 0;
        int nowUserID = lastUserID + 1;

        bool emailExists = _ayStoreContext.Login.Any(x => x.Email == registerDMOs.Login.Email);

        if (emailExists)
        {
            throw new Exception("Bu e-posta adresi zaten sistemde mevcut !! ");

        }


        _ayStoreContext.Users.AddRange(new User
        {
            Id = nowUserID,
            Name = registerDMOs.User.Name,
            LastName = registerDMOs.User.LastName,
            PhoneNumber = registerDMOs.User.PhoneNumber,
        });

        _ayStoreContext.Login.AddRange(new Login
        {
            UserId = nowUserID,
            Email = registerDMOs.Login.Email,
            Password = registerDMOs.Login.Password,
            ConfirmPassword = registerDMOs.Login.ConfirmPassword,
        });

        _ayStoreContext.Addresses.AddRange(new Address
        {
            UserId = nowUserID,
            AddressOne = registerDMOs.Address.AddressOne,
            AddressTwo = registerDMOs.Address.AddressTwo,
            Country = registerDMOs.Address.Country,
            City = registerDMOs.Address.City,
            State = registerDMOs.Address.State,
            Code = registerDMOs.Address.Code,

        });

        await _ayStoreContext.SaveChangesAsync();

        return registerDMOs;
    }


    public async Task<bool> AddOrder (Order order)
    {
        try{
            _ayStoreContext.Order.Add(
                new Order{
                    UserID = order.UserID,
                    OrderNo =order.OrderNo,
                    OrderDate = order.OrderDate,
                    ProductName = order.ProductName,
                    Price = order.Price,
                    Quantity = order.Quantity,
                    TotalPrice = order.TotalPrice,
                }
            );
            await _ayStoreContext.SaveChangesAsync();

            return true;
        }
        catch
        {
        throw new Exception("Sipariş-kaydı oluşturulurken hata oluştu!");
        }
    }

    public async Task<bool> AddOrderNumber (string orderNumber, int? userID)
    {
        try
        {
        _ayStoreContext.OrderNumbers.Add(
            new OrderNumbers{
                OrderNo = orderNumber,
                UserID = userID,
            }
        );
        await _ayStoreContext.SaveChangesAsync();

        return true;

        }
        catch 
        {
            
        throw new Exception("Sipariş-no oluşturulurken hata oluştu!");
         
        }
    }
    


    public async Task<List<CategoriesDMO>> GetCategories()
    {

        var response = await _ayStoreContext.Category.Select(s => new CategoriesDMO
        {
            Id = s.Id,
            Name = s.Name,
            Image = s.Image,


        }).ToListAsync();

        return response;

    }

    public async Task<User> GetUserInfo(int? userID)
    {
        if (userID == null)
        {
            throw new Exception("User ID bulunamadı ! ");
        }
        var user = await _ayStoreContext.Users
        .Where(s => s.Id == userID)
        .Select(s => new User
        {
            Id = s.Id,
            Name = s.Name,
            LastName = s.LastName,
            PhoneNumber = s.PhoneNumber
        })
        .FirstOrDefaultAsync();


        return user;

    }


    public async Task<Address> GetAddressInfo(int? userID)
    {
        if (userID == null)
        {
            throw new Exception("User ID bulunamadı ! ");
        }
        var address = await _ayStoreContext.Addresses
            .Where(s => s.UserId == userID)
            .Select(s => new Address
            {
                UserId = s.UserId,
                AddressOne = s.AddressOne,
                AddressTwo = s.AddressTwo,
                Country = s.Country,
                City = s.City,
                State = s.State,
                Code = s.Code,

            })
            .FirstOrDefaultAsync();


        return address;

    }

    public async Task<Login> GetLoginInfo(int? userID)
    {
        if (userID == null)
        {
            throw new Exception("User ID bulunamadı ! ");
        }
        var login = await _ayStoreContext.Login
                    .Where(s => s.UserId == userID)
                    .Select(s=> new Login 
                       {
                        UserId = s.UserId,
                        Email = s.Email,
                        
                       })
                    .FirstOrDefaultAsync();



        return login;

    }

    public int? SignIn(string email, string password)
    {

        int? userID = _ayStoreContext.Login
        .Where(x => x.Email == email && x.Password == password)
        .Select(x => x.UserId)
        .FirstOrDefault();

        if (userID == null)
        {
            throw new Exception("Email ya da şifre hatalı ! ");

        }
        else
        {
            return userID;
        }
    }

    public async Task<List<PriceFiltersDMO>> GetAllPriceFilters()
    {
        var responsePriceList = await _ayStoreContext.PriceFilters.Select(s => new PriceFiltersDMO
        {
            Id = s.Id,
            Name = s.Name,
            MinPrice = s.MinPrice,
            MaxPrice = s.MaxPrice,

        }).ToListAsync();

        return responsePriceList;
    }

}
