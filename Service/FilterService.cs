using AutoMapper;

public interface IFilterService
{
    public Task<List<PriceFiltersDTO>> GetPriceFilters();

    public Task<ColorsFilters> GetProductColorsAsync();
}

public class FilterService : IFilterService
{
    private readonly IWebContextRepository _webContextRepository;
    private readonly IWebApiRepository _webApiRepository;
    private readonly IMapper _mapper;

    public FilterService(IWebContextRepository webContextRepository,
                         IWebApiRepository webApiRepository,
                         IMapper mapper)
    {
        _webContextRepository = webContextRepository;
        _webApiRepository = webApiRepository;
        
        _mapper = mapper;
    }

    public async Task<ColorsFilters> GetProductColorsAsync()
    {
        ColorsFilters colorsFilters = await _webApiRepository.GetProductColorsAsync();  

        return colorsFilters; 
    }

    public async Task<List<PriceFiltersDTO>> GetPriceFilters()
    {
        List<PriceFiltersDMO> priceFilters = await _webContextRepository.GetAllPriceFilters();
        List<PriceFiltersDTO> priceFiltersDTOs =_mapper.Map<List<PriceFiltersDTO>>(priceFilters);

        return priceFiltersDTOs;

    }
}