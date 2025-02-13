using AutoMapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Context
builder.Services.AddDbContext<AyStoreContext>(option =>
  option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingHelper).Assembly);

// Bağımlılıklar ; 
builder.Services.AddScoped<IWebApiRepository,WebApiRepository>();
builder.Services.AddScoped<IMapper,Mapper>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoriesService,CategoriesService>();



///// 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();
app.MapStaticAssets();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
