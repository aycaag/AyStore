using AutoMapper;

public class MappingHelper : Profile
{
    public MappingHelper()
    {
        //Product
        CreateMap<ProductsDMO,ProductsDTO>().ReverseMap(); 
        CreateMap<ProductsDTO,ProductViewModel>().ReverseMap();

        //ProductDetail
        CreateMap<ProductDetailDMO,ProductDetailDTO>().ReverseMap(); 
        CreateMap<ProductDetailDTO,ProductDetailViewModel>().ReverseMap();
        

        //Categories 
        CreateMap<CategoriesDMO,CategoriesDTO>().ReverseMap(); 
        CreateMap<CategoriesDTO,CategoryViewModel>().ReverseMap(); 

        // Registers
        CreateMap<RegisterDMO,RegisterDTO>().ReverseMap();
        CreateMap<RegisterDTO,RegisterViewModel>().ReverseMap();

        // Filters 
            // Price
            CreateMap<PriceFiltersDMO,PriceFiltersDTO>().ReverseMap();
            CreateMap<PriceFiltersDTO,PriceFilters>().ReverseMap();

    }

   
}