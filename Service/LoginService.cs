using Azure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public interface ILoginService
{
    public bool SignIn (string email , string password) ;
}

public class LoginService : ILoginService
{
    private readonly IWebContextRepository _webContextRepository;

    public LoginService(
        IWebContextRepository webContextRepository)
    {
        _webContextRepository = webContextRepository;
    }
    public bool SignIn (string email,string password)
    {
        bool userControl =  _webContextRepository.SignIn (email, password);

        return userControl ;

    }
}