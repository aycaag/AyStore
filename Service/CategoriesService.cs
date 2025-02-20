using System.Threading.Tasks;
using AutoMapper;

public interface ICategoriesService
{
    public Task<List<CategoriesDTO>> GetCategories();
}

public class CategoriesService : ICategoriesService
{

    private IWebContextRepository _webcontextRepository;
    private IMapper _mapper;
    public CategoriesService(IWebContextRepository webcontextRepository,IMapper mapper)
    {
        _webcontextRepository = webcontextRepository;
        _mapper = mapper;
    }
    public async Task<List<CategoriesDTO>> GetCategories()
    {
        var categories = await _webcontextRepository.GetCategories();
        List<CategoriesDTO> result = _mapper.Map<List<CategoriesDTO>>(categories);
        return result;
    }
}