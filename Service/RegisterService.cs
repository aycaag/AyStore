using System.Runtime.InteropServices;
using AutoMapper;
using Azure;

public interface IRegisterService
{
    public Task<RegisterDTO> Add(RegisterDTO registerDTOs);
}
public class RegisterService : IRegisterService
{
    private IWebContextRepository _webcontextRepository;
    private IMapper _mapper;
    public RegisterService(IWebContextRepository webContextRepository,IMapper mapper)
    {
        _webcontextRepository = webContextRepository;
        _mapper = mapper;
    }
    public async Task<RegisterDTO> Add (RegisterDTO registerDTOs)
    {
        var registerdmo =  _mapper.Map<RegisterDMO>(registerDTOs);

        await _webcontextRepository.AddRegister(registerdmo);

        

        return registerDTOs;
    }
}