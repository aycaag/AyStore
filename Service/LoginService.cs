using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface ILoginService
{
    public int? SignIn(string email, string password);
    public Task<User> GetUserInfo(int? userID);

    public Task<Address> GetAddressInfo(int? userID);

    public Task<Login> GetLoginInfo(int? userID);


}

public class LoginService : ILoginService
{
    private readonly IWebContextRepository _webContextRepository;

    public LoginService(
        IWebContextRepository webContextRepository)
    {
        _webContextRepository = webContextRepository;
    }

    public async Task<User> GetUserInfo(int? userID)
    {
        User user = await _webContextRepository.GetUserInfo(userID);
        return user;
    }

    public async Task<Address> GetAddressInfo(int? userID)
    {
        Address address = await _webContextRepository.GetAddressInfo(userID);
        return address;
    }
    public async Task<Login> GetLoginInfo (int? userID)
    {
        Login login = await _webContextRepository.GetLoginInfo(userID);
        return login;
    }
    public int? SignIn(string email, string password)
    {
        int? userID = _webContextRepository.SignIn(email, password);

        return userID;

    }


}