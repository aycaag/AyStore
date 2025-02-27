public interface IVisitService
{
    public void Visit ();

    public Task<bool> AddVisit ();
}

public class VisitService : IVisitService
{

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IWebContextRepository _webContextRepository;
    private const string VisitSessionKey = "Visit";
    public VisitService(IHttpContextAccessor httpContextAccessor, IWebContextRepository webContextRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _webContextRepository = webContextRepository;
    }
    public void Visit()
    {
        Random rnd = new Random();
        var deger  = rnd.Next();
        DateTime simdi = DateTime.Now;
    
        var key = deger.ToString()+ simdi.ToString("yyMMddhhmm");

        _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Visit",key);
    }

    public async Task<bool> AddVisit()
    {

        if (_httpContextAccessor.HttpContext.Session.GetObjectFromJson<string>("Visit") != null)
        {
            return false;
        }

        Visit();
        string visitSession = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<string>("Visit").ToString();
        DateTime simdi = DateTime.Now;
        Visits visit = new Visits{
            Tarih = simdi,
            VisitSession = visitSession,
        };

        bool response = await _webContextRepository.AddVisit(visit);

        return response;

    }

   
}