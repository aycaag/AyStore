public class DashboardViewModel
{
    public  Widgets widgets {get; set;}  = new Widgets();  

}

public class Widgets
{
    public int? UsersCount {get;set;} = 0;

    public int? SalesCount {get;set;} = 0;
    public int? Revenue {get;set;} = 0;
    public int? TotalProductQuantity {get;set;} = 0;    
}