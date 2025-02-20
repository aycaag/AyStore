using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// builder.Services.AddControllersWithViews();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


// Fluent Validation bağlantısı ;
builder.Services.AddControllersWithViews()
.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RegisterModelValidator>());


//Context
builder.Services.AddDbContext<AyStoreContext>(option =>
  option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingHelper).Assembly);


// Uygulamada Session kullanmak istediğimiz için, sessiojn ayarlarını program.cs dosyasında yapıyoruz
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30); // session'ın süresini belirledik!!   
});

builder.Services.AddHttpContextAccessor();
// Bağımlılıklar ; 
builder.Services.AddScoped<IWebApiRepository,WebApiRepository>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoriesService,CategoriesService>();
builder.Services.AddScoped<IShopCartService,ShopCartService>();



///// 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSession();
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
