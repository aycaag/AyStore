using AutoMapper;

public interface IFilterService
{
    public Task<List<PriceFiltersDTO>> GetPriceFilters();
}

public class FilterService : IFilterService
{
    private readonly IWebContextRepository _webContextRepository;
    private readonly IMapper _mapper;

    public FilterService(IWebContextRepository webContextRepository,
                         IMapper mapper)
    {
        _webContextRepository = webContextRepository;
        _mapper = mapper;
    }
    
    public async Task<List<PriceFiltersDTO>> GetPriceFilters()
    {
        List<PriceFiltersDMO> priceFilters = await _webContextRepository.GetAllPriceFilters();
        List<PriceFiltersDTO> priceFiltersDTOs =_mapper.Map<List<PriceFiltersDTO>>(priceFilters);

        return priceFiltersDTOs;

    }
}