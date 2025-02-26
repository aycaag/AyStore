using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
// builder.Services.AddControllersWithViews();




// Otomatik gelen validaiton'lar kaldırılması için eklenmiştir.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


// Fluent Validation bağlantısı ;
builder.Services.AddControllersWithViews()
.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<RegisterModelValidator>())
.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<CheckoutModelValidator>());


//JWT Bearer
var JwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(option =>
{

    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = JwtSettings["Issuer"],
        ValidAudience = JwtSettings["Audience"],
        ClockSkew = TimeSpan.Zero, // token süresi kesin olarak dolması için eklenmiştir.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings["Key"]))
    };


});


builder.Services.AddAuthorization();

//Context
builder.Services.AddDbContext<AyStoreContext>(option =>
  option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);



//AutoMapper
builder.Services.AddAutoMapper(typeof(MappingHelper).Assembly);


// Uygulamada Session kullanmak istediğimiz için, sessiojn ayarlarını program.cs dosyasında yapıyoruz
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(30); // session'ın süresini  
    option.Cookie.HttpOnly = true; 
    option.Cookie.IsEssential = true; // Çerezleri zorunlu yaparak session'ı düzgün çalıştırabilirsiniz
});


builder.Services.AddHttpContextAccessor();
// Bağımlılıklar ; 
builder.Services.AddScoped<IWebApiRepository, WebApiRepository>();
builder.Services.AddScoped<IWebContextRepository, WebContextRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IShopCartService, ShopCartService>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IFilterService, FilterService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
// Admin Bağımlılıkları 
builder.Services.AddScoped<IAdminContextRepository,AdminContextRepository>();
builder.Services.AddScoped<IAdminDashboardService,AdminDashboardService>();


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

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseStaticFiles();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
