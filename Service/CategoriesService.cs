using System.Threading.Tasks;
using AutoMapper;

public interface ICategoriesService
{
    public Task<List<CategoriesDTO>> GetCategories();
}

public class CategoriesService : ICategoriesService
{

    private IWebApiRepository _webapiRepository;
    private IMapper _mapper;
    public CategoriesService(IWebApiRepository webapiRepository,IMapper mapper)
    {
        _webapiRepository = webapiRepository;
        _mapper = mapper;
    }
    public async Task<List<CategoriesDTO>> GetCategories()
    {
        var categories = await _webapiRepository.GetCategories();
        List<CategoriesDTO> result = _mapper.Map<List<CategoriesDTO>>(categories);
        return result;
    }
}