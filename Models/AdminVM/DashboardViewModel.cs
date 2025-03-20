public class DashboardViewModel
{
    public  Widgets widgets {get; set;}  = new Widgets();  
    public int? VisitCount{get;set;} = 0;
    public List<Order> orders {get;set;} = new List<Order>();
    public List<VisitSummary> VisitSummaries{get;set;} = new List<VisitSummary>();

}

public class Widgets
{
    public int? UsersCount {get;set;} = 0;
    public int? SalesCount {get;set;} = 0;
    public decimal? Revenue {get;set;} = 0;
    public int? TotalProductQuantity {get;set;} = 0;  
}





