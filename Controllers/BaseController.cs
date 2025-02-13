using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public abstract class BaseController : Controller
    {
        protected readonly ICategoriesService _categoriesService;
        protected readonly IMapper _mapper;

        public BaseController(ICategoriesService categoriesService,IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
        }

        // Her action çalışmadan önce bu metod çalışır.
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Kategori verisini çekiyoruz
            var categories = await _categoriesService.GetCategories();            
            var categoryViewModel = _mapper.Map<List<CategoryViewModel>>(categories);
            // ViewBag'e atama yapıyoruz, böylece layout veya view'larda kullanabiliriz
            ViewBag.Categories = categoryViewModel;

            // Action'ın çalışmasına devam et
            await next();
        }
    }
