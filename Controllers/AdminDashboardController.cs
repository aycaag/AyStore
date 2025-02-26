using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AdminDashboardController : Controller   
{


private readonly IAdminDashboardService _adminDashboardService;

public AdminDashboardController(IAdminDashboardService adminDashboardService)
{
    _adminDashboardService = adminDashboardService;
}


    public async Task<IActionResult> Index()
    {
        DashboardViewModel model = new DashboardViewModel();    

        // User Sayısını bulalım
        int? userCount = await _adminDashboardService.UserCount();
        int? salesCount = await _adminDashboardService.SalesCount();
        int? revenue = await _adminDashboardService.Revenue();
        int? totalProductQuantity = await _adminDashboardService.TotalProductQuantity();

        model.widgets.UsersCount = userCount;
        model.widgets.SalesCount = salesCount;
        model.widgets.Revenue = revenue;
        model.widgets.TotalProductQuantity = totalProductQuantity;

        return View(model);
    }




}