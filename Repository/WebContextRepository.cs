using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public interface IWebContextRepository
{
    public Task<List<CategoriesDMO>> GetCategories();

    public Task<RegisterDMO> AddRegister(RegisterDMO registerDMOs);
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




}
